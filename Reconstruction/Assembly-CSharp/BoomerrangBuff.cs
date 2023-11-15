using System;

// Token: 0x02000077 RID: 119
public class BoomerrangBuff : GlobalSkill
{
	// Token: 0x1700014F RID: 335
	// (get) Token: 0x060002EB RID: 747 RVA: 0x000094EB File Offset: 0x000076EB
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.BoomerrangBuff;
		}
	}

	// Token: 0x060002EC RID: 748 RVA: 0x000094EF File Offset: 0x000076EF
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Boomerrang)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
		((BoomerangStrategy)this.strategy).UnfrostEffect = true;
	}
}
