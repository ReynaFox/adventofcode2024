//part1();
part2();

static void part1() {
	var list1 = new List<int>();
	var list2 = new List<int>();

	foreach (var line in File.ReadLines("input.txt")) {
		var parts = line.Split(' ');
		list1.Add(int.Parse(parts[0]));
		list2.Add(int.Parse(parts[^1]));
	}

	list1.Sort();
	list2.Sort();

	var sum = 0;
	for (int i = 0; i < list1.Count; i++) {
		sum += Math.Abs(list1[i] - list2[i]);
	}

	Console.WriteLine(sum);
}

static void part2() {
	var list = new List<int>();
	var counts = new Dictionary<int, int>();

	foreach (var line in File.ReadLines("input.txt")) {
		var parts = line.Split(' ');
		list.Add(int.Parse(parts[0]));
		var right = int.Parse(parts[^1]);
		if (!counts.ContainsKey(right)) counts[right] = 0;
		counts[right]++;
	}

	var sum = 0;
	foreach (var num in list) {
		sum += num * counts.GetValueOrDefault(num, 0);
	}

	Console.WriteLine(sum);
}