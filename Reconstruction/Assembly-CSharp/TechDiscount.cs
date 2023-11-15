using System;

// Token: 0x020001B8 RID: 440
public class TechDiscount : Technology
{
	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06000B4B RID: 2891 RVA: 0x0001D446 File Offset: 0x0001B646
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHDISCOUNT;
		}
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0001D44A File Offset: 0x0001B64A
	public override float KeyValue
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0001D454 File Offset: 0x0001B654
	public override string DisplayValue1
	{
		get
		{
			return (this.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0001D47F File Offset: 0x0001B67F
	public override void OnGet()
	{
		base.OnGet();
		GameRes.TurretUpgradeDiscount += this.KeyValue;
	}
}
