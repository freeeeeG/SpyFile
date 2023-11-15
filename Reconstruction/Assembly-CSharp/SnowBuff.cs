using System;

// Token: 0x02000073 RID: 115
public class SnowBuff : GlobalSkill
{
	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060002D4 RID: 724 RVA: 0x00009329 File Offset: 0x00007529
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.SnowBuff;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000932D File Offset: 0x0000752D
	public override float KeyValue
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x060002D6 RID: 726 RVA: 0x00009334 File Offset: 0x00007534
	public override float KeyValue2
	{
		get
		{
			return 1.2f;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000933B File Offset: 0x0000753B
	public override float KeyValue3
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00009342 File Offset: 0x00007542
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Snow)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00009370 File Offset: 0x00007570
	public override void OnUnFrost()
	{
		base.OnUnFrost();
		if (this.intensifyValue < this.KeyValue2)
		{
			this.strategy.TurnFireRateIntensify += this.KeyValue;
			this.intensifyValue += this.KeyValue;
		}
	}

	// Token: 0x060002DA RID: 730 RVA: 0x000093BC File Offset: 0x000075BC
	public override void EndTurn()
	{
		base.EndTurn();
		this.intensifyValue = 0f;
	}

	// Token: 0x04000170 RID: 368
	private float intensifyValue;
}
