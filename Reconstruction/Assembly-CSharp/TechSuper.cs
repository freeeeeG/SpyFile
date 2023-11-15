using System;

// Token: 0x020001A7 RID: 423
public class TechSuper : Technology
{
	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0001CA24 File Offset: 0x0001AC24
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHSUPER;
		}
	}

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0001CA28 File Offset: 0x0001AC28
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Super;
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x06000ADE RID: 2782 RVA: 0x0001CA2C File Offset: 0x0001AC2C
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0001CA4C File Offset: 0x0001AC4C
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0001CA7C File Offset: 0x0001AC7C
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0001CAAC File Offset: 0x0001ACAC
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.SuperBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
