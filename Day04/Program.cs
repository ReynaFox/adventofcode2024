part1();
part2();

static void part1() {
	var lines = new List<string>();
	foreach (var line in File.ReadLines("input.txt")) {
		if (string.IsNullOrEmpty(line)) continue;
		lines.Add(line);
	}

	var grid = new char[lines[0].Length, lines.Count];
	for (int y = 0; y < lines.Count; y++) {
		var line = lines[y];
		for (int x = 0; x < line.Length; x++) {
			grid[x, y] = line[x];
		}
	}

	var directions = new (int dx, int dy)[] {
		(1, 0),
		(1, 1),
		(0, 1),
		(-1, 1),
		(-1, 0),
		(-1, -1),
		(0, -1),
		(1, -1),
	};

	var count = 0;

	for (int y = 0; y < grid.GetLength(1); y++) {
		for (int x = 0; x < grid.GetLength(0); x++) {
			if (grid[x, y] != 'X') continue;

			foreach (var dir in directions) {
				if (matchAlongLine(x, y, dir.dx, dir.dy)) count++;
			}
		}
	}

	Console.WriteLine(count);

	bool matchAlongLine(int x, int y, int dx, int dy) {
		foreach (var ch in "MAS") {
			x += dx;
			y += dy;
			if (
				x < 0 || x >= grid.GetLength(0) ||
				y < 0 || y>= grid.GetLength(1)
			) return false;

			if (grid[x, y] != ch) return false;
		}

		return true;
	}
}

static void part2() {
	var lines = new List<string>();
	foreach (var line in File.ReadLines("input.txt")) {
		if (string.IsNullOrEmpty(line)) continue;
		lines.Add(line);
	}

	var grid = new char[lines[0].Length, lines.Count];
	for (int y = 0; y < lines.Count; y++) {
		var line = lines[y];
		for (int x = 0; x < line.Length; x++) {
			grid[x, y] = line[x];
		}
	}

	var count = 0;

	for (int y = 1; y < grid.GetLength(1)-1; y++) {
		for (int x = 1; x < grid.GetLength(0)-1; x++) {
			if (grid[x, y] != 'A') continue;

			if (
				(
					(grid[x-1, y-1] == 'S' && grid[x+1, y+1] == 'M') ||
					(grid[x-1, y-1] == 'M' && grid[x+1, y+1] == 'S')
				) && (
					(grid[x+1, y-1] == 'S' && grid[x-1, y+1] == 'M') ||
					(grid[x+1, y-1] == 'M' && grid[x-1, y+1] == 'S')
				)) count++;
		}
	}

	Console.WriteLine(count);
}