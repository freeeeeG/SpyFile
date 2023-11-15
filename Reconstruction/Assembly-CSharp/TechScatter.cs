using System;

// Token: 0x020001A1 RID: 417
public class TechScatter : Technology
{
	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0001C5CC File Offset: 0x0001A7CC
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHSCATTER;
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0001C5D0 File Offset: 0x0001A7D0
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Scatter;
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0001C5D4 File Offset: 0x0001A7D4
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0001C604 File Offset: 0x0001A804
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0001C634 File Offset: 0x0001A834
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x0001C664 File Offset: 0x0001A864
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.ScatterBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
