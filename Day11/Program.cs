//part1();
part2();


static void part1() {
	var ints = File.ReadLines("input.txt").First()
		.Split(' ').Select(s => long.Parse(s)).ToList();

	for (int i = 0; i < 25; i++) {
		ints = step(ints);
		Console.WriteLine();
	}

	Console.WriteLine(ints.Count);

	List<long> step(List<long> ints) {
		var result = new List<long>();

		foreach (var i in ints) {
			var digits = (int)Math.Floor(Math.Log10(i))+1;
			if (i == 0) result.Add(1);
			else if ((digits % 2)==0) {
				var power = (long)Math.Pow(10, digits/2);
				result.Add(i/power);
				result.Add(i%power);
			} else {
				result.Add(2024*i);
			}
		}

		return result;
	}
}

static void part2() {
	var ints = File.ReadLines("input.txt").First()
		.Split(' ').Select(s => long.Parse(s)).ToList();

	var memory = new Dictionary<(long num, int remain), long>();
	var sum = ints.Select(i => stepAll(i, 75)).Sum();

	Console.WriteLine(sum);

	long stepAll(long i, int steps) {
		if (steps == 0) return 1;
		if (memory.TryGetValue((i, steps), out var count)) return count;

		var digits = (int)Math.Floor(Math.Log10(i))+1;
		long result;
		if (i == 0) {
			result = stepAll(1, steps-1);
		} else if ((digits % 2)==0) {
			var power = (long)Math.Pow(10, digits/2);

			result =
				stepAll(i%power, steps-1) +
			  stepAll(i/power, steps-1);
		} else {
			result = stepAll(i*2024, steps-1);
		}

		memory[(i, steps)] = result;

		return result;
	}
}