using System;

// Token: 0x020001A8 RID: 424
public class TechBoomerang : Technology
{
	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0001CAE0 File Offset: 0x0001ACE0
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHBOOMERANG;
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x0001CAE4 File Offset: 0x0001ACE4
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Boomerrang;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0001CAE8 File Offset: 0x0001ACE8
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0001CB18 File Offset: 0x0001AD18
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0001CB48 File Offset: 0x0001AD48
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0001CB78 File Offset: 0x0001AD78
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.BoomerrangBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
