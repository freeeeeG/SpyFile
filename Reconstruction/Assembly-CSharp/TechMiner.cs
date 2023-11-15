using System;

// Token: 0x020001B1 RID: 433
public class TechMiner : Technology
{
	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x06000B1F RID: 2847 RVA: 0x0001D100 File Offset: 0x0001B300
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHMINER;
		}
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06000B20 RID: 2848 RVA: 0x0001D104 File Offset: 0x0001B304
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Miner;
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0001D108 File Offset: 0x0001B308
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06000B22 RID: 2850 RVA: 0x0001D138 File Offset: 0x0001B338
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06000B23 RID: 2851 RVA: 0x0001D168 File Offset: 0x0001B368
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0001D198 File Offset: 0x0001B398
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.MinerBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
