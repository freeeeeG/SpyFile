using System;

// Token: 0x020001BF RID: 447
public class TechBlueprint : Technology
{
	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0001D69C File Offset: 0x0001B89C
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHBLUEPRINT;
		}
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x06000B6A RID: 2922 RVA: 0x0001D6A0 File Offset: 0x0001B8A0
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0001D6A8 File Offset: 0x0001B8A8
	public override string DisplayValue1
	{
		get
		{
			return this.KeyValue.ToString();
		}
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x0001D6C3 File Offset: 0x0001B8C3
	public override bool OnGet2()
	{
		GameRes.ShopCapacity += (int)this.KeyValue;
		return false;
	}
}
