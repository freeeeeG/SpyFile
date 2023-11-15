using System;
using System.Collections.Generic;

// Token: 0x02000A8B RID: 2699
public class UIFloatFormatter
{
	// Token: 0x06005279 RID: 21113 RVA: 0x001D860F File Offset: 0x001D680F
	public string Format(string format, float value)
	{
		return this.Replace(format, "{0}", value);
	}

	// Token: 0x0600527A RID: 21114 RVA: 0x001D8620 File Offset: 0x001D6820
	private string Replace(string format, string key, float value)
	{
		UIFloatFormatter.Entry entry = default(UIFloatFormatter.Entry);
		if (this.activeStringCount >= this.entries.Count)
		{
			entry.format = format;
			entry.key = key;
			entry.value = value;
			entry.result = entry.format.Replace(key, value.ToString());
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
				entry.result = entry.format.Replace(key, value.ToString());
				this.entries[this.activeStringCount] = entry;
			}
		}
		this.activeStringCount++;
		return entry.result;
	}

	// Token: 0x0600527B RID: 21115 RVA: 0x001D8717 File Offset: 0x001D6917
	public void BeginDrawing()
	{
		this.activeStringCount = 0;
	}

	// Token: 0x0600527C RID: 21116 RVA: 0x001D8720 File Offset: 0x001D6920
	public void EndDrawing()
	{
	}

	// Token: 0x0400371C RID: 14108
	private int activeStringCount;

	// Token: 0x0400371D RID: 14109
	private List<UIFloatFormatter.Entry> entries = new List<UIFloatFormatter.Entry>();

	// Token: 0x020019A4 RID: 6564
	private struct Entry
	{
		// Token: 0x040076EA RID: 30442
		public string format;

		// Token: 0x040076EB RID: 30443
		public string key;

		// Token: 0x040076EC RID: 30444
		public float value;

		// Token: 0x040076ED RID: 30445
		public string result;
	}
}
