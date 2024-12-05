part1();
part2();

static void part1() {
	var rules = new Dictionary<int, List<int>>();
	var inRules = true;
	var sum = 0;
	foreach (var line in File.ReadLines("input.txt")) {
		if (string.IsNullOrEmpty(line)) {
			if (inRules) inRules = false;
			continue;
		}
		if (inRules) {
			var parts = line.Split('|');
			var first = int.Parse(parts[0]);
			if (!rules.ContainsKey(first)) rules[first] = new List<int>();
			rules[first].Add(int.Parse(parts[1]));
		} else {
			var pages = line.Split(',').Select(s => int.Parse(s)).ToArray();
			var isOk = true;
			for (int i = 1; i < pages.Length; i++) {
				var second = pages[i];
				for (int j = 0; j < i; j++) {
					var first = pages[j];
					if (
						(!rules.ContainsKey(first) || !rules[first].Contains(second)) &&
						(rules.ContainsKey(second) && rules[second].Contains(first))
					) {
						isOk = false;
						break;
					}
				}
				if (!isOk) break;
			}

			if (isOk) {
				sum += pages[pages.Length/2];
			}
		}
	}

	Console.WriteLine(sum);
}

static void part2() {
	var rules = new Dictionary<int, List<int>>();
	var inRules = true;
	var sum = 0;
	foreach (var line in File.ReadLines("input.txt")) {
		if (string.IsNullOrEmpty(line)) {
			if (inRules) inRules = false;
			continue;
		}
		if (inRules) {
			var parts = line.Split('|');
			var first = int.Parse(parts[0]);
			if (!rules.ContainsKey(first)) rules[first] = new List<int>();
			rules[first].Add(int.Parse(parts[1]));
		} else {
			var pages = line.Split(',').Select(s => int.Parse(s)).ToArray();
			var isOk = true;
			for (int i = 1; i < pages.Length; i++) {
				var second = pages[i];
				for (int j = 0; j < i; j++) {
					var first = pages[j];
					if (
						(!rules.ContainsKey(first) || !rules[first].Contains(second)) &&
						(rules.ContainsKey(second) && rules[second].Contains(first))
					) {
						isOk = false;
						(pages[i], pages[j]) = (pages[j], pages[i]);
					}
				}
			}

			if (!isOk) {
				sum += pages[pages.Length/2];
			}
		}
	}

	Console.WriteLine(sum);
}