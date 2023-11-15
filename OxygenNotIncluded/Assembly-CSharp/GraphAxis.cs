using System;

// Token: 0x02000B0B RID: 2827
[Serializable]
public struct GraphAxis
{
	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x06005746 RID: 22342 RVA: 0x001FE9E4 File Offset: 0x001FCBE4
	public float range
	{
		get
		{
			return this.max_value - this.min_value;
		}
	}

	// Token: 0x04003AE9 RID: 15081
	public string name;

	// Token: 0x04003AEA RID: 15082
	public float min_value;

	// Token: 0x04003AEB RID: 15083
	public float max_value;

	// Token: 0x04003AEC RID: 15084
	public float guide_frequency;
}
