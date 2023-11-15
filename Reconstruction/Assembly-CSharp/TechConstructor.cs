using System;

// Token: 0x0200019F RID: 415
public class TechConstructor : Technology
{
	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x0001C483 File Offset: 0x0001A683
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHCONSTRUCTOR;
		}
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x0001C487 File Offset: 0x0001A687
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Constructor;
		}
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x0001C48C File Offset: 0x0001A68C
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x0001C4BC File Offset: 0x0001A6BC
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0001C4EC File Offset: 0x0001A6EC
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.ConstructorBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
