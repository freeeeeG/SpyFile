using System;

// Token: 0x020001AB RID: 427
public class TechRadpid : Technology
{
	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x0001CD14 File Offset: 0x0001AF14
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x0001CD17 File Offset: 0x0001AF17
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHRAPID;
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06000AFA RID: 2810 RVA: 0x0001CD1C File Offset: 0x0001AF1C
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06000AFB RID: 2811 RVA: 0x0001CD4C File Offset: 0x0001AF4C
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06000AFC RID: 2812 RVA: 0x0001CD7C File Offset: 0x0001AF7C
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0001CDAC File Offset: 0x0001AFAC
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.RapidBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
