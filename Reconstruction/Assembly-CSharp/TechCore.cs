using System;

// Token: 0x020001AA RID: 426
public class TechCore : Technology
{
	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0001CC68 File Offset: 0x0001AE68
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHCORE;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x0001CC6C File Offset: 0x0001AE6C
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Core;
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0001CC70 File Offset: 0x0001AE70
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0001CC90 File Offset: 0x0001AE90
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x0001CCC0 File Offset: 0x0001AEC0
	public override string DisplayValue3
	{
		get
		{
			return this.m_Skill.KeyValue3.ToString();
		}
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0001CCE0 File Offset: 0x0001AEE0
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.CoreBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
