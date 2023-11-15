using System;
using System.Collections.Generic;

// Token: 0x02000A8C RID: 2700
public class UIStringFormatter
{
	// Token: 0x0600527E RID: 21118 RVA: 0x001D8735 File Offset: 0x001D6935
	public string Format(string format, string s0)
	{
		return this.Replace(format, "{0}", s0);
	}

	// Token: 0x0600527F RID: 21119 RVA: 0x001D8744 File Offset: 0x001D6944
	public string Format(string format, string s0, string s1)
	{
		return this.Replace(this.Replace(format, "{0}", s0), "{1}", s1);
	}

	// Token: 0x06005280 RID: 21120 RVA: 0x001D8760 File Offset: 0x001D6960
	private string Replace(string format, string key, string value)
	{
		UIStringFormatter.Entry entry = default(UIStringFormatter.Entry);
		if (this.activeStringCount >= this.entries.Count)
		{
			entry.format = format;
			entry.key = key;
			entry.value = value;
			entry.result = entry.format.Replace(key, value);
			this.entries.Add(entry);
		}
		else
		{
			entry = this.entries[this.activeStringCount];
			if (entry.format != format || entry.key != key || entry.value != value)
			{
				entry.format = format;
				entry.key = key;
				entry.value = value;
				entry.result = entry.format.Replace(key, value);
				this.entries[this.activeStringCount] = entry;
			}
		}
		this.activeStringCount++;
		return entry.result;
	}

	// Token: 0x06005281 RID: 21121 RVA: 0x001D8850 File Offset: 0x001D6A50
	public void BeginDrawing()
	{
		this.activeStringCount = 0;
	}

	// Token: 0x06005282 RID: 21122 RVA: 0x001D8859 File Offset: 0x001D6A59
	public void EndDrawing()
	{
	}

	// Token: 0x0400371E RID: 14110
	private int activeStringCount;

	// Token: 0x0400371F RID: 14111
	private List<UIStringFormatter.Entry> entries = new List<UIStringFormatter.Entry>();

	// Token: 0x020019A5 RID: 6565
	private struct Entry
	{
		// Token: 0x040076EE RID: 30446
		public string format;

		// Token: 0x040076EF RID: 30447
		public string key;

		// Token: 0x040076F0 RID: 30448
		public string value;

		// Token: 0x040076F1 RID: 30449
		public string result;
	}
}
