using System;

// Token: 0x02000080 RID: 128
public class TechABuff : GlobalSkill
{
	// Token: 0x17000166 RID: 358
	// (get) Token: 0x0600031A RID: 794 RVA: 0x00009992 File Offset: 0x00007B92
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.TechABuff;
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x0600031B RID: 795 RVA: 0x00009996 File Offset: 0x00007B96
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0000999D File Offset: 0x00007B9D
	public override void Build()
	{
		base.Build();
		this.strategy.BaseGoldCount += (int)this.KeyValue;
	}
}
