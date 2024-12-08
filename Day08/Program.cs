using System.Drawing;

//part1();
part2();


static void read(out Dictionary<char, List<Point>> coordinates, out int width, out int height) {
	coordinates = new Dictionary<char, List<Point>>();
	var readY = 0;
	width = 0;
	foreach (var line in File.ReadLines("input.txt")) {
		if (string.IsNullOrEmpty(line)) continue;
		width = line.Length;
		for (int x = 0; x < line.Length; x++) {
			var freq = line[x];
			if (freq != '.') {
				if (!coordinates.ContainsKey(freq)) coordinates[freq] = new List<Point>();
				coordinates[freq].Add(new Point(x, readY));
			}
		}

		readY++;
	}
	height = readY;
}

static void part1() {
	read(out var coordinates, out var width, out var height);

	var marks = new bool[width, height];
	foreach (var (freq, coords) in coordinates) {
		markForFreq(coords);
	}

	var count = 0;
	for (int y = 0; y < marks.GetLength(1); y++)
	for (int x = 0; x < marks.GetLength(0); x++) {
		if (marks[x, y]) count++;
	}

	Console.WriteLine(count);

	void markForFreq(List<Point> coords) {
		for (int i = 0; i < coords.Count; i++)
		for (int j = i+1; j < coords.Count; j++) {
			var dX = coords[j].X-coords[i].X;
			var dY = coords[j].Y-coords[i].Y;

			markIfValid(coords[i].X-dX, coords[i].Y-dY);
			markIfValid(coords[j].X+dX, coords[j].Y+dY);
		}
	}

	void markIfValid(int x, int y) {
		if (x>=0 && y>=0 && x<width && y<height) {
			marks[x, y] = true;
		}
	}
}

static void part2() {
	read(out var coordinates, out var width, out var height);

	var marks = new bool[width, height];
	foreach (var (freq, coords) in coordinates) {
		markForFreq(coords);
	}

	var count = 0;
	for (int y = 0; y < marks.GetLength(1); y++)
	for (int x = 0; x < marks.GetLength(0); x++) {
		if (marks[x, y]) count++;
	}

	Console.WriteLine(count);

	void markForFreq(List<Point> coords) {
		for (int i = 0; i < coords.Count; i++)
		for (int j = i+1; j < coords.Count; j++) {
			var dX = coords[j].X-coords[i].X;
			var dY = coords[j].Y-coords[i].Y;

			var x = coords[i].X;
			var y = coords[i].Y;
			while (markIfValid(x, y)) {
				x -= dX;
				y -= dY;
			}

			x = coords[j].X;
			y = coords[j].Y;
			while (markIfValid(x, y)) {
				x += dX;
				y += dY;
			}
		}
	}

	bool markIfValid(int x, int y) {
		if (x>=0 && y>=0 && x<width && y<height) {
			marks[x, y] = true;
			return true;
		}
		return false;
	}
}