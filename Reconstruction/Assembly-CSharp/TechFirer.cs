using System;

// Token: 0x020001AE RID: 430
public class TechFirer : Technology
{
	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x06000B0A RID: 2826 RVA: 0x0001CEBC File Offset: 0x0001B0BC
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHFIRER;
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0001CEC0 File Offset: 0x0001B0C0
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Firer;
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06000B0C RID: 2828 RVA: 0x0001CEC4 File Offset: 0x0001B0C4
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0001CEF4 File Offset: 0x0001B0F4
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06000B0E RID: 2830 RVA: 0x0001CF24 File Offset: 0x0001B124
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0001CF54 File Offset: 0x0001B154
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.FirerBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
