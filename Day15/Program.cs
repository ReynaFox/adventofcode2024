using System.Drawing;
using System.Runtime.InteropServices;

//part1();
part2();

static void read(string file, out Square[,] grid, out Direction[] moves, out Point start) {
	var moveList = new List<Direction>();
	var lines = new List<string>();
	var inMap = true;
	foreach (var line in File.ReadLines(file)) {
		if (string.IsNullOrEmpty(line)) {
			inMap = false;
			continue;
		}
		if (inMap) {
			lines.Add(line);
		} else {
			moveList.AddRange(line.Select(c => c switch {
				'^' => Direction.up,
				'>' => Direction.right,
				'v' => Direction.down,
				'<' => Direction.left,
			}));
		}
	}
	moves = moveList.ToArray();

	start = new Point(-1, -1);
	grid = new Square[lines[0].Length, lines.Count];
	for (int y = 0; y < lines.Count; y++) {
		var line = lines[y];
		for (int x = 0; x < line.Length; x++) {
			grid[x, y] = line[x] switch {
				'#' => Square.wall,
				'O' => Square.boxLeft,
				_ => Square.empty
			};
			if (line[x]=='@') start = new Point(x, y);
		}
	}
}

static void part1() {
	read("input.txt", out var grid, out var moves, out var start);

	var pos = start;
	foreach (var d in moves) {
		pos = push(grid, d, pos);
	}

	var sum = 0;
	for (int y = 0; y < grid.GetLength(1); y++)
	for (int x = 0; x < grid.GetLength(0); x++) {
		if (grid[x, y]==Square.boxLeft) {
			sum += y*100+x;
		}
	}

	Console.WriteLine(sum);


	Point push(Square[,] grid, Direction move, Point pos) {
		var dirs = new (int dx, int dy)[] {
			(0, -1),
			(1, 0),
			(0, 1),
			(-1, 0)
		};
		var dir = dirs[(int)move];

		// Scan for the first non-box.
		var checkX = pos.X + dir.dx;
		var checkY = pos.Y + dir.dy;
		var nextToBox = grid[checkX, checkY] == Square.boxLeft;
		while (grid[checkX, checkY]==Square.boxLeft) {
			checkX += dir.dx;
			checkY += dir.dy;
		}

		if (grid[checkX, checkY]==Square.wall) {
			// We can't move.
			return pos;
		}

		// If next to us in the direction of movement has a box, we're pushing a chain of boxes.
		var newPos = new Point(pos.X + dir.dx, pos.Y + dir.dy);
		if (nextToBox) {
			grid[newPos.X, newPos.Y] = Square.empty;
			grid[checkX, checkY] = Square.boxLeft;
		}
		return newPos;
	}
}

static void part2() {
	var dirs = new (int dx, int dy)[] {
		(0, -1),
		(1, 0),
		(0, 1),
		(-1, 0)
	};

	read("input.txt", out var sourceGrid, out var moves, out var start);
	// Scale up
	var grid = new Square[sourceGrid.GetLength(0)*2, sourceGrid.GetLength(1)];
	for (int y = 0; y < sourceGrid.GetLength(1); y++)
	for (int x = 0; x < sourceGrid.GetLength(0); x++) {
		grid[x*2, y] = sourceGrid[x, y];
		grid[x*2+1, y] = sourceGrid[x, y] == Square.boxLeft ? Square.boxRight : sourceGrid[x, y];
	}
	start.X *= 2;

	//dump(grid, start,false);

	var robotPos = start;
	foreach (var d in moves) {
		robotPos = push(grid, d, robotPos);
	}

	var sum = 0L;
	for (int y = 0; y < grid.GetLength(1); y++)
	for (int x = 0; x < grid.GetLength(0); x++) {
		if (grid[x, y]==Square.boxLeft) {
			//Console.WriteLine($"Box at {x},{y} = {y*100+x}");
			sum += y*100+x;
		}
	}

	//dump(grid, robotPos, false);

	Console.WriteLine(sum);

	Point push(Square[,] grid, Direction move, Point pos) {
		return move == Direction.left || move == Direction.right
			? pushHoriz(grid, move, pos)
			: pushVert(grid, move, pos);
	}

	Point pushHoriz(Square[,] grid, Direction move, Point pos) {
		var dir = dirs[(int)move];

		// Scan for the first non-box.
		var checkX = pos.X + dir.dx;
		var checkY = pos.Y + dir.dy;
		var nextToBox = grid[checkX, checkY] == Square.boxLeft || grid[checkX, checkY] == Square.boxRight;
		while (grid[checkX, checkY]==Square.boxLeft || grid[checkX, checkY]==Square.boxRight) {
			checkX += dir.dx;
			checkY += dir.dy;
		}

		if (grid[checkX, checkY]==Square.wall) {
			// We can't move.
			return pos;
		}

		// If next to us in the direction of movement has a box, we're pushing a chain of boxes.
		var newPos = new Point(pos.X + dir.dx, pos.Y + dir.dy);
		if (nextToBox) {
			for (int x = checkX; x-dir.dx != pos.X; x -= dir.dx) {
				grid[x, newPos.Y] = grid[x-dir.dx, newPos.Y];
			}
			grid[newPos.X, newPos.Y] = Square.empty;
		}
		return newPos;
	}

	Point pushVert(Square[,] grid, Direction move, Point pos) {
		var dy = dirs[(int)move].dy;
		var moved = new List<Point>();

		if (grid[pos.X, pos.Y+dy]==Square.empty) {
			return new Point(pos.X, pos.Y+dy);
		}
		if (grid[pos.X, pos.Y+dy]==Square.wall) {
			return pos;
		}

		if (!canPushV(grid, dy, pos.X, pos.Y+dy, moved)) return pos;

		// Sort farthest boxes first.
		if (dy < 0) moved.Sort((a, b) => Comparer<int>.Default.Compare(a.Y, b.Y));
		else        moved.Sort((a, b) => Comparer<int>.Default.Compare(b.Y, a.Y));

		foreach (var boxP in moved) {
			grid[boxP.X,   boxP.Y+dy] = grid[boxP.X,   boxP.Y];
			grid[boxP.X+1, boxP.Y+dy] = grid[boxP.X+1, boxP.Y];
			grid[boxP.X,   boxP.Y] = Square.empty;
			grid[boxP.X+1, boxP.Y] = Square.empty;
		}

		return new Point(pos.X, pos.Y+dy);
	}

	// x,y is a coordinate of a box to check for pushability.
	bool canPushV(Square[,] grid, int dy, int x, int y, List<Point> moved) {
		var otherX = grid[x, y]==Square.boxLeft ? x+1 : x-1;
		var nextY = y+dy;

		if (grid[x, nextY] == Square.wall || grid[otherX, nextY] == Square.wall) return false;

		var moveEntry = new Point(grid[x, y]==Square.boxLeft ? x : x-1, y);
		if (moved.Contains(moveEntry)) return true;
		moved.Add(moveEntry);

		if (isBox(grid[x, nextY]) && !canPushV(grid, dy, x, nextY, moved)) return false;
		if (isBox(grid[otherX, nextY]) && !canPushV(grid, dy, otherX, nextY, moved)) return false;
		return true;

		bool isBox(Square s) => s == Square.boxLeft || s == Square.boxRight;
	}
}

static void dump(Square[,] grid, Point botPos, bool smallBox) {
	for (int y = 0; y < grid.GetLength(1); y++) {
		var s = "";
		for (int x = 0; x < grid.GetLength(0); x++) {
			if (x==botPos.X && y==botPos.Y) {
				s+='@';
			} else {
				s+= grid[x, y] switch {
					Square.empty => '.',
					Square.wall => '#',
					Square.boxLeft => smallBox ? 'O' : '[',
					Square.boxRight => ']',
				};
			}
		}
		Console.WriteLine(s);
	}
}

enum Square {
	empty,
	wall,
	boxLeft,
	boxRight
}

enum Direction {
	up = 0,
	right = 1,
	down = 2,
	left = 3
}