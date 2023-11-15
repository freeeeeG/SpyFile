using System;

// Token: 0x02000068 RID: 104
public class LowTech : GlobalSkill
{
	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000298 RID: 664 RVA: 0x00008DDD File Offset: 0x00006FDD
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.LowTech;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000299 RID: 665 RVA: 0x00008DE0 File Offset: 0x00006FE0
	public override float KeyValue
	{
		get
		{
			if (!this.IsAbnormal)
			{
				return 0.5f;
			}
			return 0.8f;
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x0600029A RID: 666 RVA: 0x00008DF5 File Offset: 0x00006FF5
	public override float KeyValue2
	{
		get
		{
			if (!this.IsAbnormal)
			{
				return 0f;
			}
			return 0.5f;
		}
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00008E0C File Offset: 0x0000700C
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.Rare <= 3)
		{
			this.strategy.BaseFixDamageIntensify += this.KeyValue;
			return;
		}
		this.strategy.BaseFixDamageIntensify -= this.KeyValue2;
	}
}
