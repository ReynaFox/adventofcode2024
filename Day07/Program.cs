//part1();
part2();

static void part1() {
	var sum = 0L;

	foreach (var line in File.ReadLines("input.txt")) {
		if (string.IsNullOrEmpty(line)) continue;

		var parts = line.Split(':');
		var target = long.Parse(parts[0]);
		var ints = parts[1].Trim().Split(' ').Select(s => int.Parse(s)).ToArray();
		if (testOperators(0, ints, 0, target)) {
			sum += target;
		}
	}

	Console.WriteLine(sum);

	bool testOperators(long total, int[] ints, int index, long target) {
		if (index == 0) return testOperators(ints[0], ints, 1, target);
		if (total > target) return false;
		if (index == ints.Length) return total == target;

		if (testOperators(total*ints[index], ints, index+1, target)) return true;
		return testOperators(total+ints[index], ints, index+1, target);
	}
}

static void part2() {
	var sum = 0L;

	foreach (var line in File.ReadLines("input.txt")) {
		if (string.IsNullOrEmpty(line)) continue;

		var parts = line.Split(':');
		var target = long.Parse(parts[0]);
		var ints = parts[1].Trim().Split(' ').Select(s => int.Parse(s)).ToArray();
		if (testOperators(0, ints, 0, target)) {
			sum += target;
		}
	}

	Console.WriteLine(sum);

	bool testOperators(long total, int[] ints, int index, long target) {
		if (index == 0) return testOperators(ints[0], ints, 1, target);
		if (total > target) return false;
		if (index == ints.Length) return total == target;

		var curr = ints[index];
		if (testOperators(total*curr, ints, index+1, target)) return true;
		if (testOperators(total+curr, ints, index+1, target)) return true;
		return testOperators(
			curr <=1
				? total*10+curr
				: total*(long)Math.Pow(10, Math.Ceiling(Math.Log10(curr))) + curr,
			ints, index+1, target);
	}
}