using System;

// Token: 0x020001A0 RID: 416
public class TechRapider : Technology
{
	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06000AAC RID: 2732 RVA: 0x0001C520 File Offset: 0x0001A720
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHRAPIDER;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0001C524 File Offset: 0x0001A724
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Rapider;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0001C528 File Offset: 0x0001A728
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0001C548 File Offset: 0x0001A748
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0001C578 File Offset: 0x0001A778
	public override string DisplayValue3
	{
		get
		{
			return this.m_Skill.KeyValue3.ToString();
		}
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x0001C598 File Offset: 0x0001A798
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.RapiderBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
