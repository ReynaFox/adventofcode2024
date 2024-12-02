//part1();
part2();

static void part1() {
	var list1 = new List<int>();
	var list2 = new List<int>();

	int count = 0;
	foreach (var line in File.ReadLines("input.txt")) {
		var ints = line.Split(' ').Select((i) => int.Parse(i)).ToArray();
		var min = int.MaxValue;
		var max = int.MinValue;
		for (int i = 1; i < ints.Length; i++) {
			var diff = ints[i-1]-ints[i];
			min = Math.Min(min, diff);
			max = Math.Max(max, diff);
		}

		var absMin = Math.Abs(min);
		var absMax = Math.Abs(max);
		if (Math.Sign(min) == Math.Sign(max) && 1 <= absMin && absMin <= 3 && 1 <= absMax && absMax <= 3) {
			count++;
		}
	}

	Console.WriteLine(count);
}

static void part2() {
	var list1 = new List<int>();
	var list2 = new List<int>();

	int count = 0;
	foreach (var line in File.ReadLines("input.txt")) {
		var ints = line.Split(' ').Select((i) => int.Parse(i)).ToArray();

		if (isSafe(ints)) {
			count++;
		} else {
			// Simple exhaustive search excluding one element at a time
			for (int i = 0; i < ints.Length; i++) {
				var ints2 = new List<int>(ints);
				ints2.RemoveAt(i);
				if (isSafe(ints2.ToArray())) {
					count++;
					break;
				}
			}
		}
	}

	Console.WriteLine(count);

	bool isSafe(int[] ints) {
		var min = int.MaxValue;
		var max = int.MinValue;
		for (int i = 1; i < ints.Length; i++) {
			var diff = ints[i-1]-ints[i];
			min = Math.Min(min, diff);
			max = Math.Max(max, diff);
		}

		var absMin = Math.Abs(min);
		var absMax = Math.Abs(max);
		return Math.Sign(min) == Math.Sign(max) && 1 <= absMin && absMin <= 3 && 1 <= absMax && absMax <= 3;
	}
}