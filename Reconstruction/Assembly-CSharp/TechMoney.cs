using System;

// Token: 0x020001B3 RID: 435
public class TechMoney : Technology
{
	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x06000B2D RID: 2861 RVA: 0x0001D288 File Offset: 0x0001B488
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHMONEY;
		}
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06000B2E RID: 2862 RVA: 0x0001D28C File Offset: 0x0001B48C
	public override float KeyValue
	{
		get
		{
			return 600f;
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06000B2F RID: 2863 RVA: 0x0001D293 File Offset: 0x0001B493
	public override float KeyValue2
	{
		get
		{
			return 300f;
		}
	}

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0001D29C File Offset: 0x0001B49C
	public override string DisplayValue1
	{
		get
		{
			return this.finalValue.ToString();
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06000B31 RID: 2865 RVA: 0x0001D2B8 File Offset: 0x0001B4B8
	public override string DisplayValue2
	{
		get
		{
			return this.KeyValue2.ToString();
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06000B32 RID: 2866 RVA: 0x0001D2D3 File Offset: 0x0001B4D3
	private int finalValue
	{
		get
		{
			return (int)this.KeyValue + GameRes.CurrentWave / 10 * (int)this.KeyValue2;
		}
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x0001D2ED File Offset: 0x0001B4ED
	public override bool OnGet2()
	{
		Singleton<GameManager>.Instance.GainMoney(this.finalValue);
		return false;
	}
}
