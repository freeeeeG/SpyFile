using System;

// Token: 0x020001A5 RID: 421
public class TechMortar : Technology
{
	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06000ACE RID: 2766 RVA: 0x0001C89C File Offset: 0x0001AA9C
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHMORTAR;
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0001C8A0 File Offset: 0x0001AAA0
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Mortar;
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x0001C8A4 File Offset: 0x0001AAA4
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x0001C8D4 File Offset: 0x0001AAD4
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x0001C904 File Offset: 0x0001AB04
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x0001C934 File Offset: 0x0001AB34
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.MortarBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
