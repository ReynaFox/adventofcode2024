using System.Text.RegularExpressions;

part1();
part2();


static List<Machine> read(string file, long offset) {
	var regexA = new Regex(@"Button A: X\+(\d+), Y\+(\d+)");
	var regexB = new Regex(@"Button B: X\+(\d+), Y\+(\d+)");
	var regexPrize = new Regex(@"Prize: X=(\d+), Y=(\d+)");

	var result = new List<Machine>();
	var curr = new Machine();
	foreach (var line in File.ReadLines(file)) {
		if (string.IsNullOrEmpty(line)) {
			continue;
		}
		var matchA = regexA.Match(line);
		var matchB = regexB.Match(line);
		var prize = regexPrize.Match(line);
		if (matchA.Success) {
			curr.dxA = int.Parse(matchA.Groups[1].Value);
			curr.dyA = int.Parse(matchA.Groups[2].Value);
		} else if (matchB.Success) {
			curr.dxB = int.Parse(matchB.Groups[1].Value);
			curr.dyB = int.Parse(matchB.Groups[2].Value);
		} else if (prize.Success) {
			curr.tgtX = int.Parse(prize.Groups[1].Value)+offset;
			curr.tgtY = int.Parse(prize.Groups[2].Value)+offset;
			result.Add(curr);
			curr = new Machine();
		}
	}
	return result;
}

static long calcCost(Machine m) {
	var divisor = m.dxB * m.dyA - m.dxA * m.dyB;
	if (divisor == 0) {
		return 0;
	}
	var divA = (m.tgtY * m.dxB - m.tgtX * m.dyB);
	var divB = (m.tgtY * m.dxA - m.tgtX * m.dyA);
	if (divA % divisor != 0 || divB % divisor != 0) {
		return 0;
	}
	var aHits = divA / divisor;
	var bHits = divB / -divisor;

	return aHits*3 + bHits;
}

static void part1() {
	var machines = read("input.txt", 0);

	var sum = 0L;

	foreach (var m in machines) {
		var cost = calcCost(m);
		sum += cost;
	}

	Console.WriteLine(sum);
}

static void part2() {
	var machines = read("input.txt", 10000000000000L);

	var sum = 0L;

	foreach (var m in machines) {
		var cost = calcCost(m);
		sum += cost;
	}

	Console.WriteLine(sum);
}

class Machine {
	public long dxA, dyA;
	public long dxB, dyB;
	public long tgtX, tgtY;
}