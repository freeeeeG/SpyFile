using System;

// Token: 0x020001A2 RID: 418
public class TechCoordinator : Technology
{
	// Token: 0x170003CE RID: 974
	// (get) Token: 0x06000ABA RID: 2746 RVA: 0x0001C698 File Offset: 0x0001A898
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHCOORDINATOR;
		}
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0001C69C File Offset: 0x0001A89C
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Coordinator;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0001C6A0 File Offset: 0x0001A8A0
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0001C6D0 File Offset: 0x0001A8D0
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.CoordinatorBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x0001C6FC File Offset: 0x0001A8FC
	public override void OnGet()
	{
		base.OnGet();
		StrategyBase.CoordinatorMaxIntensify += this.m_Skill.KeyValue;
	}
}
