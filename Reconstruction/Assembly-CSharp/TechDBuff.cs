using System;

// Token: 0x02000083 RID: 131
public class TechDBuff : GlobalSkill
{
	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06000326 RID: 806 RVA: 0x00009A2E File Offset: 0x00007C2E
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.TechDBuff;
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000327 RID: 807 RVA: 0x00009A32 File Offset: 0x00007C32
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00009A39 File Offset: 0x00007C39
	public override void Build()
	{
		base.Build();
		this.strategy.BaseFireCount += (int)this.KeyValue;
	}
}
