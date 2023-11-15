using System;

// Token: 0x020001BA RID: 442
public class TechB : Technology
{
	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x06000B54 RID: 2900 RVA: 0x0001D4F8 File Offset: 0x0001B6F8
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHB;
		}
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06000B55 RID: 2901 RVA: 0x0001D4FC File Offset: 0x0001B6FC
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x0001D51C File Offset: 0x0001B71C
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechBBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
