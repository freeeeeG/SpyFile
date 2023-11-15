using System;

// Token: 0x0200006D RID: 109
public class RangeLimit : GlobalSkill
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x060002B4 RID: 692 RVA: 0x00009066 File Offset: 0x00007266
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.RangeLimit;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000906A File Offset: 0x0000726A
	public override float KeyValue
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00009071 File Offset: 0x00007271
	public override void Build()
	{
		base.Build();
		this.strategy.MaxRange = (int)this.KeyValue;
	}
}
