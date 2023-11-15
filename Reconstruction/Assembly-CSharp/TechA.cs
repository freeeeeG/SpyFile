using System;

// Token: 0x020001B9 RID: 441
public class TechA : Technology
{
	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06000B50 RID: 2896 RVA: 0x0001D4A0 File Offset: 0x0001B6A0
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHA;
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0001D4A4 File Offset: 0x0001B6A4
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x0001D4C4 File Offset: 0x0001B6C4
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechABuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
