using System;

// Token: 0x020001BE RID: 446
public class TechDieProtect : Technology
{
	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0001D658 File Offset: 0x0001B858
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHDIEPROTECT;
		}
	}

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0001D65C File Offset: 0x0001B85C
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0001D664 File Offset: 0x0001B864
	public override string DisplayValue1
	{
		get
		{
			return this.KeyValue.ToString();
		}
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x0001D67F File Offset: 0x0001B87F
	public override bool OnGet2()
	{
		GameRes.DieProtect += (int)this.KeyValue;
		return false;
	}
}
