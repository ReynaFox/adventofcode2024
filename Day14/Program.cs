using System.Drawing;
using System.Text.RegularExpressions;

//part1();
part2();


static List<Robot> read(string file) {
	var regex = new Regex(@"p=(\d+),(\d+)+ v=(-?\d+),(-?\d+)");

	var result = new List<Robot>();
	foreach (var line in File.ReadLines(file)) {
		if (string.IsNullOrEmpty(line)) {
			continue;
		}
		var match = regex.Match(line);
		if (match.Success) {
			var robot = new Robot {
				x = int.Parse(match.Groups[1].Value),
				y = int.Parse(match.Groups[2].Value),
				dx = int.Parse(match.Groups[3].Value),
				dy = int.Parse(match.Groups[4].Value)
			};
			result.Add(robot);
		}
	}
	return result;
}

static void part1() {
	//*
	var robots = read("input.txt");
	const int roomWidth = 101;
	const int roomHeight = 103;
	/*/
	var robots = read("example.txt");
	const int roomWidth = 11;
	const int roomHeight = 7;
	//*/
	const int midX = roomWidth/2;
	const int midY = roomHeight/2;
	const int steps = 100;

	var quadrantCounts = new int[4];
	foreach (var r in robots) {
		var endX = r.x+steps*r.dx;
		var endY = r.y+steps*r.dy;

		endX = (endX + steps * roomWidth)%roomWidth;
		endY = (endY + steps * roomHeight)%roomHeight;

		if (endX < midX) {
			if (endY < midY) quadrantCounts[0]++;
			else if (endY > midY) quadrantCounts[2]++;
		} else if (endX > midX) {
			if (endY < midY) quadrantCounts[1]++;
			else if (endY > midY) quadrantCounts[3]++;
		}
	}

	Console.WriteLine(quadrantCounts[0]*quadrantCounts[1]*quadrantCounts[2]*quadrantCounts[3]);
}

static void part2() {
	var robots = read("input.txt");
	const int roomWidth = 101;
	const int roomHeight = 103;

	var coords = new Point[robots.Count];
	for (int j = 0; j<robots.Count; j++) {
		coords[j].X = robots[j].x;
		coords[j].Y = robots[j].y;
	}
	for (int i = 0; i < 10000; i++) {
		for (int j = 0; j<robots.Count; j++) {
			var r = robots[j];
			var newX = coords[j].X+r.dx;
			var newY = coords[j].Y+r.dy;

			coords[j].X = (newX + roomWidth)%roomWidth;
			coords[j].Y = (newY + roomHeight)%roomHeight;
		}
		var mean = computeMean(coords);
		var stdDev = computeStdDev(coords, mean);
		var cov = computeCovariance(coords, mean);
		if (stdDev.x < 20 && stdDev.y < 20) {
			Console.WriteLine($"{i}, {mean.x}, {mean.y}, {stdDev.x}, {stdDev.y}, {cov}");
			Console.WriteLine($"After {i+1} seconds");
			printGrid(coords, roomWidth, roomHeight);
			Console.ReadKey();
		}
	}

	(float x, float y) computeMean(Point[] coords) {
		var sumX = 0;
		var sumY = 0;
		foreach (var c in coords) {
			sumX += c.X;
			sumY += c.Y;
		}
		return (sumX/(float)coords.Length, sumY/(float)coords.Length);
	}

	(double x, double y) computeStdDev(Point[] coords, (float x, float y) mean) {
		var x = Math.Sqrt(coords.Sum(c => (c.X-mean.x)*(c.X-mean.x))/coords.Length);
		var y = Math.Sqrt(coords.Sum(c => (c.Y-mean.y)*(c.Y-mean.y))/coords.Length);
		return (x, y);
	}

	float computeCovariance(Point[] coords, (float meanX, float meanY) mean) {
		var sum = 0f;
		foreach (var c in coords) {
			sum+= (c.X-mean.meanX)*(c.Y-mean.meanY);
		}
		return sum/coords.Length;
	}
}

static void printGrid(Point[] coords, int w, int h) {
	var grid = new bool[w, h];
	foreach (var c in coords) {
		grid[c.X, c.Y] = true;
	}
	for (int y = 0; y<h; y++) {
		for (int x = 0; x<w; x++) {
			Console.Write(grid[x, y] ? 'x' : '.');
		}
		Console.WriteLine();
	}
}


class Robot {
	public int x, y;
	public int dx, dy;
	public override string ToString() {
		return $"Robot ({x}, {y}) >({dx}, {dy})";
	}
}