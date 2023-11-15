using System;
using STRINGS;

// Token: 0x0200077E RID: 1918
internal struct EffectorEntry
{
	// Token: 0x06003517 RID: 13591 RVA: 0x0011FC3F File Offset: 0x0011DE3F
	public EffectorEntry(string name, float value)
	{
		this.name = name;
		this.value = value;
		this.count = 1;
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x0011FC58 File Offset: 0x0011DE58
	public override string ToString()
	{
		string arg = "";
		if (this.count > 1)
		{
			arg = string.Format(UI.OVERLAYS.DECOR.COUNT, this.count);
		}
		return string.Format(UI.OVERLAYS.DECOR.ENTRY, GameUtil.GetFormattedDecor(this.value, false), this.name, arg);
	}

	// Token: 0x0400202A RID: 8234
	public string name;

	// Token: 0x0400202B RID: 8235
	public int count;

	// Token: 0x0400202C RID: 8236
	public float value;
}
