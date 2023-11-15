using System;
using System.Collections.Generic;

// Token: 0x02000047 RID: 71
public class ConFirerate : ElementSkill
{
	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060001CA RID: 458 RVA: 0x0000746C File Offset: 0x0000566C
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				4,
				4,
				1
			};
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060001CB RID: 459 RVA: 0x00007488 File Offset: 0x00005688
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060001CC RID: 460 RVA: 0x0000748F File Offset: 0x0000568F
	public override float KeyValue2
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x060001CD RID: 461 RVA: 0x00007498 File Offset: 0x00005698
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060001CE RID: 462 RVA: 0x000074C4 File Offset: 0x000056C4
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x060001CF RID: 463 RVA: 0x000074FF File Offset: 0x000056FF
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixSplashPercentage += (float)this.strategy.WoodCount * this.KeyValue2;
	}
}
