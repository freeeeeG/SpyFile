using System;
using System.Collections.Generic;

// Token: 0x02000045 RID: 69
public class TimeSpeed : ElementSkill
{
	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060001BB RID: 443 RVA: 0x000072B2 File Offset: 0x000054B2
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				0,
				1
			};
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060001BC RID: 444 RVA: 0x000072CE File Offset: 0x000054CE
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060001BD RID: 445 RVA: 0x000072D5 File Offset: 0x000054D5
	public override float KeyValue2
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060001BE RID: 446 RVA: 0x000072DC File Offset: 0x000054DC
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060001BF RID: 447 RVA: 0x00007308 File Offset: 0x00005508
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00007334 File Offset: 0x00005534
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixRange += (int)this.KeyValue * (this.strategy.WoodCount / (int)this.KeyValue2);
		this.strategy.Concrete.GenerateRange();
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00007384 File Offset: 0x00005584
	public override void EndTurn()
	{
		base.EndTurn();
		this.strategy.Concrete.GenerateRange();
	}
}
