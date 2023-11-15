using System;

// Token: 0x02000069 RID: 105
public class Firerate : GlobalSkill
{
	// Token: 0x17000125 RID: 293
	// (get) Token: 0x0600029D RID: 669 RVA: 0x00008E6B File Offset: 0x0000706B
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.Firerate;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x0600029E RID: 670 RVA: 0x00008E6E File Offset: 0x0000706E
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00008E75 File Offset: 0x00007075
	public override void Build()
	{
		base.Build();
		this.strategy.FirerateIntensify += this.KeyValue;
	}
}
