using System;

// Token: 0x0200019B RID: 411
public class TechPerfect : Technology
{
	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0001C1DA File Offset: 0x0001A3DA
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHPERFECT;
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0001C1DD File Offset: 0x0001A3DD
	public override float KeyValue
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0001C1E4 File Offset: 0x0001A3E4
	public override float KeyValue2
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0001C1EC File Offset: 0x0001A3EC
	public override string DisplayValue1
	{
		get
		{
			return this.finalCount.ToString();
		}
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0001C208 File Offset: 0x0001A408
	public override string DisplayValue2
	{
		get
		{
			return this.KeyValue2.ToString();
		}
	}

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0001C223 File Offset: 0x0001A423
	private int finalCount
	{
		get
		{
			return (int)this.KeyValue + GameRes.CurrentWave / 10 * (int)this.KeyValue2;
		}
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x0001C23D File Offset: 0x0001A43D
	public override bool OnGet2()
	{
		Singleton<GameManager>.Instance.GainPerfectElement(this.finalCount);
		return false;
	}
}
