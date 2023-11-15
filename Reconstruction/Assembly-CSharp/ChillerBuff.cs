using System;

// Token: 0x0200007A RID: 122
public class ChillerBuff : GlobalSkill
{
	// Token: 0x17000156 RID: 342
	// (get) Token: 0x060002F9 RID: 761 RVA: 0x0000962F File Offset: 0x0000782F
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.ChillerBuff;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x060002FA RID: 762 RVA: 0x00009633 File Offset: 0x00007833
	public override float KeyValue
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x060002FB RID: 763 RVA: 0x0000963C File Offset: 0x0000783C
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Chiller)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
		this.strategy.BaseFixFrostResist += this.KeyValue;
	}
}
