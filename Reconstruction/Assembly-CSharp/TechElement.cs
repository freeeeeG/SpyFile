using System;

// Token: 0x0200019D RID: 413
public class TechElement : Technology
{
	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0001C2C5 File Offset: 0x0001A4C5
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x06000A95 RID: 2709 RVA: 0x0001C2C8 File Offset: 0x0001A4C8
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHELEMENT;
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0001C2CC File Offset: 0x0001A4CC
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x06000A97 RID: 2711 RVA: 0x0001C2EC File Offset: 0x0001A4EC
	public override string DisplayValue2
	{
		get
		{
			return this.m_Skill.KeyValue2.ToString();
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0001C30C File Offset: 0x0001A50C
	public override string DisplayValue3
	{
		get
		{
			return this.m_Skill.KeyValue3.ToString();
		}
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0001C32C File Offset: 0x0001A52C
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechElementSkill, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x0001C357 File Offset: 0x0001A557
	public override void OnGet()
	{
		base.OnGet();
		if (this.IsAbnormal)
		{
			StrategyBase.MaxElementCount = (int)this.m_Skill.KeyValue2;
		}
		Singleton<GameManager>.Instance.TriggerDetectSkills();
	}
}
