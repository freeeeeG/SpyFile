using System;

// Token: 0x020001AD RID: 429
public class TechChiller : Technology
{
	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0001CE4D File Offset: 0x0001B04D
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHCHILLER;
		}
	}

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06000B06 RID: 2822 RVA: 0x0001CE51 File Offset: 0x0001B051
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Chiller;
		}
	}

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0001CE58 File Offset: 0x0001B058
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0001CE88 File Offset: 0x0001B088
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.ChillerBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
