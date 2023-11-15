using System;
using System.Collections.Generic;

// Token: 0x0200004D RID: 77
public class HitSlow : ElementSkill
{
	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060001EE RID: 494 RVA: 0x000077E0 File Offset: 0x000059E0
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				0,
				2
			};
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060001EF RID: 495 RVA: 0x000077FC File Offset: 0x000059FC
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060001F0 RID: 496 RVA: 0x00007803 File Offset: 0x00005A03
	public override float KeyValue2
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000780C File Offset: 0x00005A0C
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x060001F2 RID: 498 RVA: 0x00007838 File Offset: 0x00005A38
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x00007864 File Offset: 0x00005A64
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixRange += (int)this.KeyValue * (this.strategy.WaterCount / (int)this.KeyValue2);
		this.strategy.Concrete.GenerateRange();
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x000078B4 File Offset: 0x00005AB4
	public override void EndTurn()
	{
		base.EndTurn();
		this.strategy.Concrete.GenerateRange();
	}
}
