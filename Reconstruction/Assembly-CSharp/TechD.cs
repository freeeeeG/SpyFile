using System;

// Token: 0x020001BC RID: 444
public class TechD : Technology
{
	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0001D5A8 File Offset: 0x0001B7A8
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHD;
		}
	}

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0001D5AC File Offset: 0x0001B7AC
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x0001D5CC File Offset: 0x0001B7CC
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechDBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
