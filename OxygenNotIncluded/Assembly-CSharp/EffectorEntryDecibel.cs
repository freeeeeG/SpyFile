using System;

// Token: 0x0200077F RID: 1919
internal struct EffectorEntryDecibel
{
	// Token: 0x06003519 RID: 13593 RVA: 0x0011FCB1 File Offset: 0x0011DEB1
	public EffectorEntryDecibel(string name, float value)
	{
		this.name = name;
		this.value = value;
		this.count = 1;
	}

	// Token: 0x0400202D RID: 8237
	public string name;

	// Token: 0x0400202E RID: 8238
	public int count;

	// Token: 0x0400202F RID: 8239
	public float value;
}
