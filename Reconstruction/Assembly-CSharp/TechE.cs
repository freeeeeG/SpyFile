using System;

// Token: 0x020001BD RID: 445
public class TechE : Technology
{
	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0001D600 File Offset: 0x0001B800
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHE;
		}
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0001D604 File Offset: 0x0001B804
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x0001D624 File Offset: 0x0001B824
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechEBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
