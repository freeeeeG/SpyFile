using System;

// Token: 0x02000081 RID: 129
public class TechBBuff : GlobalSkill
{
	// Token: 0x17000168 RID: 360
	// (get) Token: 0x0600031E RID: 798 RVA: 0x000099C6 File Offset: 0x00007BC6
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.TechBBuff;
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x0600031F RID: 799 RVA: 0x000099CA File Offset: 0x00007BCA
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06000320 RID: 800 RVA: 0x000099D1 File Offset: 0x00007BD1
	public override void Build()
	{
		base.Build();
		this.strategy.BaseWoodCount += (int)this.KeyValue;
	}
}
