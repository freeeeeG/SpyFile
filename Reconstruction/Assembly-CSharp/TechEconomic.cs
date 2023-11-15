using System;

// Token: 0x02000197 RID: 407
public class TechEconomic : Technology
{
	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0001BF33 File Offset: 0x0001A133
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHECONOMIC;
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0001BF36 File Offset: 0x0001A136
	public override float KeyValue
	{
		get
		{
			return 0.04f;
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0001BF3D File Offset: 0x0001A13D
	public override float KeyValue2
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0001BF44 File Offset: 0x0001A144
	public override string DisplayValue1
	{
		get
		{
			return (this.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0001BF70 File Offset: 0x0001A170
	public override string DisplayValue3
	{
		get
		{
			return (this.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0001BF9B File Offset: 0x0001A19B
	public override void OnGet()
	{
		base.OnGet();
		GameRes.BuildDiscount += this.KeyValue;
	}
}
