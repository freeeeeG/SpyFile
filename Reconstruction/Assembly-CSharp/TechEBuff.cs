using System;

// Token: 0x02000084 RID: 132
public class TechEBuff : GlobalSkill
{
	// Token: 0x1700016E RID: 366
	// (get) Token: 0x0600032A RID: 810 RVA: 0x00009A62 File Offset: 0x00007C62
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.TechEBuff;
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x0600032B RID: 811 RVA: 0x00009A66 File Offset: 0x00007C66
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00009A6D File Offset: 0x00007C6D
	public override void Build()
	{
		base.Build();
		this.strategy.BaseDustCount += (int)this.KeyValue;
	}
}
