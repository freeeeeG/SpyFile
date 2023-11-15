using System;

// Token: 0x02000198 RID: 408
public class TechMass : Technology
{
	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0001BFBC File Offset: 0x0001A1BC
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0001BFBF File Offset: 0x0001A1BF
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHMASS;
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0001BFC4 File Offset: 0x0001A1C4
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0001BFF4 File Offset: 0x0001A1F4
	public override string DisplayValue2
	{
		get
		{
			return this.m_Skill.KeyValue2.ToString();
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0001C014 File Offset: 0x0001A214
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0001C044 File Offset: 0x0001A244
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechMassSkill, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0001C06F File Offset: 0x0001A26F
	public override void OnGet()
	{
		base.OnGet();
		Singleton<GameManager>.Instance.TriggerDetectSkills();
	}
}
