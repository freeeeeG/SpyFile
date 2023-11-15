using System;

// Token: 0x020001A4 RID: 420
public class TechSnow : Technology
{
	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0001C7E0 File Offset: 0x0001A9E0
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHSNOW;
		}
	}

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0001C7E4 File Offset: 0x0001A9E4
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Snow;
		}
	}

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0001C7E8 File Offset: 0x0001A9E8
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0001C818 File Offset: 0x0001AA18
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0001C848 File Offset: 0x0001AA48
	public override string DisplayValue3
	{
		get
		{
			return this.m_Skill.KeyValue3.ToString();
		}
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x0001C868 File Offset: 0x0001AA68
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.SnowBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
