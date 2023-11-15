using System;

// Token: 0x02000082 RID: 130
public class TechCBuff : GlobalSkill
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000322 RID: 802 RVA: 0x000099FA File Offset: 0x00007BFA
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.TechCBuff;
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06000323 RID: 803 RVA: 0x000099FE File Offset: 0x00007BFE
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00009A05 File Offset: 0x00007C05
	public override void Build()
	{
		base.Build();
		this.strategy.BaseWaterCount += (int)this.KeyValue;
	}
}
