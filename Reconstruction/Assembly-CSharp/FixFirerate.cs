using System;

// Token: 0x02000085 RID: 133
public class FixFirerate : GlobalSkill
{
	// Token: 0x17000170 RID: 368
	// (get) Token: 0x0600032E RID: 814 RVA: 0x00009A96 File Offset: 0x00007C96
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.FixFirerate;
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x0600032F RID: 815 RVA: 0x00009A9A File Offset: 0x00007C9A
	public override float KeyValue
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00009AA1 File Offset: 0x00007CA1
	public override void Build()
	{
		base.Build();
		this.strategy.BaseFixFirerate += this.KeyValue;
		this.strategy.MaxFireRate = this.KeyValue;
	}
}
