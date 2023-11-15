using System;

// Token: 0x020001AF RID: 431
public class TechLaser : Technology
{
	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0001CF88 File Offset: 0x0001B188
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHLASER;
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06000B12 RID: 2834 RVA: 0x0001CF8C File Offset: 0x0001B18C
	public override RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.Laser;
		}
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0001CF90 File Offset: 0x0001B190
	public override string DisplayValue1
	{
		get
		{
			return (this.m_Skill.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x06000B14 RID: 2836 RVA: 0x0001CFC0 File Offset: 0x0001B1C0
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0001CFF0 File Offset: 0x0001B1F0
	public override string DisplayValue3
	{
		get
		{
			return (this.m_Skill.KeyValue3 * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0001D020 File Offset: 0x0001B220
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.LaserBuff, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
