using System.Text.RegularExpressions;

//part1();
part2();

static void part1() {
	var regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
	var sum = 0L;
	foreach (var line in File.ReadLines("input.txt")) {
		var matches = regex.Matches(line);
		foreach (Match match in matches) {
			var int1 = int.Parse(match.Groups[1].Value);
			var int2 = int.Parse(match.Groups[2].Value);
			sum += int1*int2;
		}
	}

	Console.WriteLine(sum);
}

static void part2() {
	var regex = new Regex(@"(?<op>mul)\((\d{1,3}),(\d{1,3})\)|(?<op>don't)\(\)|(?<op>do)\(\)");
	var sum = 0L;
	var enabled = true;
	foreach (var line in File.ReadLines("input.txt")) {
		var matches = regex.Matches(line);
		foreach (Match match in matches) {
			switch(match.Groups["op"].Value) {
				case "mul":
					if (enabled) {
						var int1 = int.Parse(match.Groups[1].Value);
						var int2 = int.Parse(match.Groups[2].Value);
						sum += int1*int2;
					}
					break;
				case "do":
					enabled = true;
					break;
				case "don't":
					enabled = false;
					break;
			}
		}
	}

	Console.WriteLine(sum);
}