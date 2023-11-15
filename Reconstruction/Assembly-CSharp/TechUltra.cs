using System;

// Token: 0x020001A9 RID: 425
public class TechUltra : Technology
{
	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0001CBAC File Offset: 0x0001ADAC
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHULTRA;
		}
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0001CBB0 File Offset: 0x0001ADB0
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Ultra;
		}
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0001CBB4 File Offset: 0x0001ADB4
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x06000AED RID: 2797 RVA: 0x0001CBD4 File Offset: 0x0001ADD4
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x06000AEE RID: 2798 RVA: 0x0001CC04 File Offset: 0x0001AE04
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0001CC34 File Offset: 0x0001AE34
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.UltraBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
