using System;

// Token: 0x02000195 RID: 405
public class TechCombat : Technology
{
	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0001BDAD File Offset: 0x00019FAD
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0001BDB0 File Offset: 0x00019FB0
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHCOMBAT;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0001BDB4 File Offset: 0x00019FB4
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0001BDE4 File Offset: 0x00019FE4
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0001BE14 File Offset: 0x0001A014
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0001BE44 File Offset: 0x0001A044
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechCombatSkill, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0001BE6F File Offset: 0x0001A06F
	public override void OnGet()
	{
		base.OnGet();
		Singleton<GameManager>.Instance.TriggerDetectSkills();
	}
}
