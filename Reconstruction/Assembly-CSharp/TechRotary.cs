using System;

// Token: 0x020001A3 RID: 419
public class TechRotary : Technology
{
	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0001C722 File Offset: 0x0001A922
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHROTARY;
		}
	}

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0001C726 File Offset: 0x0001A926
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Rotary;
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0001C72C File Offset: 0x0001A92C
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0001C75C File Offset: 0x0001A95C
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0001C78C File Offset: 0x0001A98C
	public override string DisplayValue3
	{
		get
		{
			return this.m_Skill.KeyValue3.ToString();
		}
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0001C7AC File Offset: 0x0001A9AC
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.RotaryBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
