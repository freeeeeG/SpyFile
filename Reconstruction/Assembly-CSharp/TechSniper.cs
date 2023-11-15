using System;

// Token: 0x020001A6 RID: 422
public class TechSniper : Technology
{
	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0001C968 File Offset: 0x0001AB68
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHSNIPER;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0001C96C File Offset: 0x0001AB6C
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Sniper;
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0001C970 File Offset: 0x0001AB70
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x0001C990 File Offset: 0x0001AB90
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0001C9C0 File Offset: 0x0001ABC0
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x0001C9F0 File Offset: 0x0001ABF0
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.SniperBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
