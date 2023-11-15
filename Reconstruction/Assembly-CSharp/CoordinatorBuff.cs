using System;

// Token: 0x02000071 RID: 113
public class CoordinatorBuff : GlobalSkill
{
	// Token: 0x1700013D RID: 317
	// (get) Token: 0x060002C9 RID: 713 RVA: 0x00009201 File Offset: 0x00007401
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.CoordinatorBuff;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x060002CA RID: 714 RVA: 0x00009205 File Offset: 0x00007405
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x060002CB RID: 715 RVA: 0x0000920C File Offset: 0x0000740C
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Coordinator)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
	}
}
