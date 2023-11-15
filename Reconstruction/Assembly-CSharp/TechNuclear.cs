using System;

// Token: 0x020001B2 RID: 434
public class TechNuclear : Technology
{
	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0001D1CC File Offset: 0x0001B3CC
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHNUCLEAR;
		}
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06000B27 RID: 2855 RVA: 0x0001D1D0 File Offset: 0x0001B3D0
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Nuclear;
		}
	}

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06000B28 RID: 2856 RVA: 0x0001D1D4 File Offset: 0x0001B3D4
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06000B29 RID: 2857 RVA: 0x0001D204 File Offset: 0x0001B404
	public override string DisplayValue2
	{
		get
		{
			return this.m_Skill.KeyValue2.ToString();
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06000B2A RID: 2858 RVA: 0x0001D224 File Offset: 0x0001B424
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x0001D254 File Offset: 0x0001B454
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.NuclearBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
