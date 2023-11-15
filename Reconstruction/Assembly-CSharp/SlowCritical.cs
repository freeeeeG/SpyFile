using System;
using System.Collections.Generic;

// Token: 0x02000053 RID: 83
public class SlowCritical : ElementSkill
{
	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600021A RID: 538 RVA: 0x00007CDD File Offset: 0x00005EDD
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				4,
				4,
				3
			};
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x0600021B RID: 539 RVA: 0x00007CF9 File Offset: 0x00005EF9
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x0600021C RID: 540 RVA: 0x00007D00 File Offset: 0x00005F00
	public override float KeyValue2
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x0600021D RID: 541 RVA: 0x00007D08 File Offset: 0x00005F08
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x0600021E RID: 542 RVA: 0x00007D34 File Offset: 0x00005F34
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x0600021F RID: 543 RVA: 0x00007D6F File Offset: 0x00005F6F
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixSplashPercentage += (float)this.strategy.FireCount * this.KeyValue2;
	}
}
