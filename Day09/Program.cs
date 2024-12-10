//part1();
part2();


static void part1() {
	var line = File.ReadLines("input.txt").First();

	var ranges = new List<Range>();
	var id = 0;
	var isFile = true;
	var totalSize = 0L;
	for (int i = 0; i < line.Length; i++) {
		var len = line[i]-'0';
		totalSize += len;
		if (len > 0) {
			ranges.Add(new Range(isFile ? id : -1, len));
		}

		if (isFile) id++;
		isFile = !isFile;
	}

	var writeRange = 0;
	var writePos = 0;
	var sum = 0L;
	var readRange = ranges.Count-1;
	var readPos = totalSize-1;
	var readRemain = ranges[readRange].length;
	do {
		// Find next free space, and compute checksum of files
		while (writePos<=readPos && ranges[writeRange].id != -1) {
			var range = ranges[writeRange];
			for (int i = 0; i < range.length; i++) {
				sum += range.id*writePos;
				writePos++;
				if (writePos>readPos) break;
			}
			writeRange++;
		}

		if (writePos>readPos) break;

		var writeRemain = ranges[writeRange].length;
		do {
			// "Copy" over the end of the source to the free space
			var range = ranges[readRange];
			do {
				sum += range.id*writePos;
				writePos++;
				readPos--;
				readRemain--;
				writeRemain--;
			} while (writePos<=readPos && readRemain > 0 && writeRemain > 0);

			if (writePos>readPos) break;

			if (readRemain == 0) {
				// Find next tail-end block to read from
				do {
					readRange--;
					if (ranges[readRange].id==-1) readPos -= ranges[readRange].length;
				} while (ranges[readRange].id==-1);
				readRemain = ranges[readRange].length;
			}
		} while (writePos<=readPos && writeRemain > 0);
		writeRange++;
	} while (writePos<=readPos);

	Console.WriteLine(sum);
}

static void part2() {
	var line = File.ReadLines("input.txt").First();

	RangeNode head = null;
	RangeNode tail = null;
	var id = 0;
	var isFile = true;
	var pos = 0L;
	for (int i = 0; i < line.Length; i++) {
		var len = line[i]-'0';
		if (len > 0) {
			var curr = new RangeNode(isFile ? id : -1, pos, len);
			curr.prev = tail;
			if (tail!=null) tail.next = curr;
			if (head==null) head = curr;
			tail = curr;
		}

		pos += len;

		if (isFile) id++;
		isFile = !isFile;
	}

	for (var readRange = tail; readRange != null;) {
		if (readRange.id == -1) {
			readRange = readRange.prev;
			continue;
		}

		var readPrev = readRange.prev;
		for (var writeRange = head; writeRange != null && writeRange.pos < readRange.pos; writeRange = writeRange.next) {
			if (writeRange.id != -1 || writeRange.length < readRange.length) continue;

			if (readRange.prev != null) readRange.prev.next = readRange.next;
			if (readRange.next != null) readRange.next.prev = readRange.prev;

			readRange.prev = writeRange.prev;
			writeRange.prev.next = readRange;
			readRange.next = writeRange;
			writeRange.prev = readRange;

			readRange.pos = writeRange.pos;
			writeRange.length -= readRange.length;
			writeRange.pos += readRange.length;
			break;
		}

		//dump();

		readRange = readPrev;
	}

	var sum = 0L;
	for (var range = head; range != null; range = range.next) {
		if (range.id == -1) continue;

		for (int i = 0; i < range.length; i++) {
			sum += range.id*(range.pos+i);
		}
	}

	Console.WriteLine(sum);
}

class Range(int id, int length) {
	public int id = id;
	public int length = length;
}

class RangeNode(int id, long pos, int length) {
	public RangeNode prev, next;
	public int id = id;
	public long pos = pos;
	public int length = length;
}