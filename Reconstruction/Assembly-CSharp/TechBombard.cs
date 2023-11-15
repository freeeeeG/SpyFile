using System;

// Token: 0x020001B0 RID: 432
public class TechBombard : Technology
{
	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x06000B18 RID: 2840 RVA: 0x0001D054 File Offset: 0x0001B254
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHBOMBARD;
		}
	}

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0001D058 File Offset: 0x0001B258
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Bombard;
		}
	}

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x06000B1A RID: 2842 RVA: 0x0001D05C File Offset: 0x0001B25C
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0001D07C File Offset: 0x0001B27C
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x06000B1C RID: 2844 RVA: 0x0001D0AC File Offset: 0x0001B2AC
	public override string DisplayValue3
	{
		get
		{
			return this.m_Skill.KeyValue3.ToString();
		}
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x0001D0CC File Offset: 0x0001B2CC
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.BombardBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
