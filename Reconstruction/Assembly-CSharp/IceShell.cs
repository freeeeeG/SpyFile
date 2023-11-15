using System;
using System.Collections.Generic;

// Token: 0x02000097 RID: 151
public class IceShell : ElementSkill
{
	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x0600038F RID: 911 RVA: 0x0000A5F7 File Offset: 0x000087F7
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				2,
				3,
				4
			};
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000390 RID: 912 RVA: 0x0000A613 File Offset: 0x00008813
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000391 RID: 913 RVA: 0x0000A61C File Offset: 0x0000881C
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0000A648 File Offset: 0x00008848
	public override void StartTurn2()
	{
		base.StartTurn2();
		if (this.strategy.RangeType != RangeType.Circle)
		{
			if (!this.changeCircle)
			{
				this.changeCircle = true;
				this.lastType = this.strategy.RangeType;
				this.strategy.RangeType = RangeType.Circle;
			}
		}
		else
		{
			this.strategy.TurnFixRange++;
		}
		if (this.strategy.Concrete != null)
		{
			this.strategy.Concrete.GenerateRange();
		}
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0000A6CC File Offset: 0x000088CC
	public override void EndTurn()
	{
		base.EndTurn();
		if (this.changeCircle)
		{
			this.strategy.RangeType = this.lastType;
		}
		this.changeCircle = false;
		this.strategy.Concrete.GenerateRange();
	}

	// Token: 0x04000178 RID: 376
	private bool changeCircle;

	// Token: 0x04000179 RID: 377
	private RangeType lastType;
}
