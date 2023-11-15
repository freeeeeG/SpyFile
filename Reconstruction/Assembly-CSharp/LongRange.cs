using System;

// Token: 0x02000086 RID: 134
public class LongRange : GlobalSkill
{
	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06000332 RID: 818 RVA: 0x00009ADA File Offset: 0x00007CDA
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.LongRange;
		}
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00009ADE File Offset: 0x00007CDE
	public override void Build()
	{
		base.Build();
		this.strategy.BaseFixRange += this.strategy.InitRange;
	}
}
