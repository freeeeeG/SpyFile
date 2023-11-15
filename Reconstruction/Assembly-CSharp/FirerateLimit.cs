using System;

// Token: 0x0200006C RID: 108
public class FirerateLimit : GlobalSkill
{
	// Token: 0x1700012E RID: 302
	// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000903A File Offset: 0x0000723A
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.FirerateLimit;
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000903E File Offset: 0x0000723E
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00009045 File Offset: 0x00007245
	public override void Build()
	{
		base.Build();
		this.strategy.MaxFireRate = this.KeyValue;
	}
}
