using System;

// Token: 0x0200008B RID: 139
public class AllElement : GlobalSkill
{
	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000346 RID: 838 RVA: 0x00009C25 File Offset: 0x00007E25
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.AllElement;
		}
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000347 RID: 839 RVA: 0x00009C29 File Offset: 0x00007E29
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06000348 RID: 840 RVA: 0x00009C30 File Offset: 0x00007E30
	public override void Build()
	{
		base.Build();
		this.strategy.BaseGoldCount += (int)this.KeyValue;
		this.strategy.BaseWoodCount += (int)this.KeyValue;
		this.strategy.BaseWaterCount += (int)this.KeyValue;
		this.strategy.BaseFireCount += (int)this.KeyValue;
		this.strategy.BaseDustCount += (int)this.KeyValue;
	}
}
