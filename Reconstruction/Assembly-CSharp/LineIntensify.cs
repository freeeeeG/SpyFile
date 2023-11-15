using System;
using System.Collections.Generic;

// Token: 0x0200005C RID: 92
public class LineIntensify : ElementSkill
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000255 RID: 597 RVA: 0x000083C4 File Offset: 0x000065C4
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				13,
				13,
				11
			};
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000256 RID: 598 RVA: 0x000083E3 File Offset: 0x000065E3
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000257 RID: 599 RVA: 0x000083EC File Offset: 0x000065EC
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x00008427 File Offset: 0x00006627
	public override void Build()
	{
		base.Build();
		this.strategy.RangeType = RangeType.Line;
		if (this.strategy.Concrete != null)
		{
			this.strategy.Concrete.GenerateRange();
		}
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000845E File Offset: 0x0000665E
	public override void StartTurn2()
	{
		base.StartTurn2();
		if (this.strategy.RangeType == RangeType.Line)
		{
			this.strategy.TurnFixDamageBonus += this.strategy.FinalBulletDamageIntensify;
		}
	}
}
