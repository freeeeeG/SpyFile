using System;

// Token: 0x02000196 RID: 406
public class TechFire : Technology
{
	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0001BE89 File Offset: 0x0001A089
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0001BE8C File Offset: 0x0001A08C
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHFIRE;
		}
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x06000A5F RID: 2655 RVA: 0x0001BE90 File Offset: 0x0001A090
	public override string DisplayValue1
	{
		get
		{
			return this.m_Skill.KeyValue.ToString();
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0001BEB0 File Offset: 0x0001A0B0
	public override string DisplayValue2
	{
		get
		{
			return (this.m_Skill.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0001BEE0 File Offset: 0x0001A0E0
	public override string DisplayValue3
	{
		get
		{
			return this.m_Skill.KeyValue3.ToString();
		}
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0001BF00 File Offset: 0x0001A100
	public override void InitializeTech()
	{
		base.InitializeTech();
		this.m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechFireSkill, this.IsAbnormal);
		this.m_Skill = TurretSkillFactory.GetGlobalSkill(this.m_SkillInfo);
	}
}
