using System;

// Token: 0x02000076 RID: 118
public class SuperBuff : GlobalSkill
{
	// Token: 0x1700014C RID: 332
	// (get) Token: 0x060002E6 RID: 742 RVA: 0x00009455 File Offset: 0x00007655
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.SuperBuff;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x060002E7 RID: 743 RVA: 0x00009459 File Offset: 0x00007659
	public override float KeyValue
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x060002E8 RID: 744 RVA: 0x00009460 File Offset: 0x00007660
	public override float KeyValue2
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00009468 File Offset: 0x00007668
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Super)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
		this.strategy.AttackIntensify -= this.KeyValue2;
		((SuperSkill)this.strategy.TurretSkills[0]).BounceTime += (int)this.KeyValue;
	}
}
