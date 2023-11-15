using System;

// Token: 0x020001BB RID: 443
public class TechC : Technology
{
	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0001D550 File Offset: 0x0001B750
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHC;
		}
	}

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0001D554 File Offset: 0x0001B754
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x0001D574 File Offset: 0x0001B774
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechCBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
