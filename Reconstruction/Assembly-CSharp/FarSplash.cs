using System;
using System.Collections.Generic;

// Token: 0x0200005B RID: 91
public class FarSplash : ElementSkill
{
	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x0600024E RID: 590 RVA: 0x000082FC File Offset: 0x000064FC
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				3,
				3,
				4
			};
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x0600024F RID: 591 RVA: 0x00008318 File Offset: 0x00006518
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000250 RID: 592 RVA: 0x0000831F File Offset: 0x0000651F
	public override float KeyValue2
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000251 RID: 593 RVA: 0x00008328 File Offset: 0x00006528
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000252 RID: 594 RVA: 0x00008354 File Offset: 0x00006554
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000838F File Offset: 0x0000658F
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixCriticalPercentage += (float)this.strategy.DustCount * this.KeyValue2;
	}
}
