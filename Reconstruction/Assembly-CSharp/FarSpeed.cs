using System;
using System.Collections.Generic;

// Token: 0x02000046 RID: 70
public class FarSpeed : ElementSkill
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060001C3 RID: 451 RVA: 0x000073A4 File Offset: 0x000055A4
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				3,
				3,
				1
			};
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060001C4 RID: 452 RVA: 0x000073C0 File Offset: 0x000055C0
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060001C5 RID: 453 RVA: 0x000073C7 File Offset: 0x000055C7
	public override float KeyValue2
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060001C6 RID: 454 RVA: 0x000073D0 File Offset: 0x000055D0
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060001C7 RID: 455 RVA: 0x000073FC File Offset: 0x000055FC
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x00007437 File Offset: 0x00005637
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixCriticalPercentage += (float)this.strategy.WoodCount * this.KeyValue2;
	}
}
