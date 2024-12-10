//part1();

part2();

static int[,] read(string file, out List<(int x, int y)> starts) {
	var lines = new List<string>();
	foreach (var line in File.ReadLines(file)) {
		if (string.IsNullOrEmpty(line)) continue;
		lines.Add(line);
	}

	starts = new List<(int x, int y)>();

	var grid = new int[lines[0].Length, lines.Count];
	for (int y = 0; y < lines.Count; y++) {
		var line = lines[y];
		for (int x = 0; x < line.Length; x++) {
			grid[x, y] = line[x]-'0';
			if (grid[x, y]==0) starts.Add((x, y));
		}
	}

	return grid;
}

static void part1() {
	var grid = read("input.txt", out var starts);

	var directions = new (int dx, int dy)[] {
		(1, 0),
		(0, 1),
		(-1, 0),
		(0, -1)
	};

	var sum = 0;
	HashSet<(int x, int y)> marked;
	foreach (var (x, y) in starts) {
		marked = new HashSet<(int x, int y)>();
		var score = trace(x, y);
		Console.WriteLine($"{x}, {y} -> {score}");
		sum += score;
	}
	Console.WriteLine(sum);


	int trace(int x, int y) {
		var level = grid[x, y];
		if (level == 9) {
			if (marked.Contains((x, y))) return 0;
			marked.Add((x, y));
			return 1;
		}

		var sum = 0;
		foreach (var (dx, dy) in directions) {
			var newX = x+dx;
			var newY = y+dy;

			if (newX < 0 || newY < 0 || newX >= grid.GetLength(0) || newY >= grid.GetLength(1)) continue;
			if (grid[newX, newY] != level+1) continue;

			sum += trace(newX, newY);
		}
		return sum;
	}
}

static void part2() {
	var grid = read("input.txt", out var starts);

	var directions = new (int dx, int dy)[] {
		(1, 0),
		(0, 1),
		(-1, 0),
		(0, -1),
	};

	var sum = 0;
	foreach (var (x, y)in starts) {
		var score = trace(x, y);
		Console.WriteLine($"{x}, {y} -> {score}");
		sum += score;
	}
	Console.WriteLine(sum);


	int trace(int x, int y) {
		var level = grid[x, y];
		if (level == 9) {
			return 1;
		}

		var sum = 0;
		foreach (var (dx, dy) in directions) {
			var newX = x+dx;
			var newY = y+dy;

			if (newX < 0 || newY < 0 || newX >= grid.GetLength(0) || newY >= grid.GetLength(1)) continue;
			if (grid[newX, newY] != level+1) continue;

			sum += trace(newX, newY);
		}
		return sum;
	}
}