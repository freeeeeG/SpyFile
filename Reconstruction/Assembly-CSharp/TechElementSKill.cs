using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class TechElementSKill : GlobalSkill
{
	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000275 RID: 629 RVA: 0x0000889A File Offset: 0x00006A9A
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.TechElementSkill;
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000276 RID: 630 RVA: 0x0000889D File Offset: 0x00006A9D
	public override float KeyValue
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000277 RID: 631 RVA: 0x000088A4 File Offset: 0x00006AA4
	public override float KeyValue2
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000278 RID: 632 RVA: 0x000088AB File Offset: 0x00006AAB
	public override float KeyValue3
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06000279 RID: 633 RVA: 0x000088B2 File Offset: 0x00006AB2
	public override void Build()
	{
		base.Build();
	}

	// Token: 0x0600027A RID: 634 RVA: 0x000088BC File Offset: 0x00006ABC
	private void AddRandomElement()
	{
		switch (Random.Range(0, 5))
		{
		case 0:
			this.strategy.TempGoldCount++;
			return;
		case 1:
			this.strategy.TempWoodCount++;
			return;
		case 2:
			this.strategy.TempWaterCount++;
			return;
		case 3:
			this.strategy.TempFireCount++;
			return;
		case 4:
			this.strategy.TempDustCount++;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00008950 File Offset: 0x00006B50
	public override void StartTurn()
	{
		base.StartTurn();
		if (this.IsAbnormal)
		{
			if (this.strategy.GoldCount > 0)
			{
				this.strategy.TempGoldCount += (int)this.KeyValue3;
			}
			if (this.strategy.WoodCount > 0)
			{
				this.strategy.TempWoodCount += (int)this.KeyValue3;
			}
			if (this.strategy.WaterCount > 0)
			{
				this.strategy.TempWaterCount += (int)this.KeyValue3;
			}
			if (this.strategy.FireCount > 0)
			{
				this.strategy.TempFireCount += (int)this.KeyValue3;
			}
			if (this.strategy.DustCount > 0)
			{
				this.strategy.TempDustCount += (int)this.KeyValue3;
				return;
			}
		}
		else
		{
			for (int i = 0; i < (int)this.KeyValue; i++)
			{
				this.AddRandomElement();
			}
		}
	}
}
