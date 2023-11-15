using System;

// Token: 0x02000087 RID: 135
public class FixRange : GlobalSkill
{
	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000335 RID: 821 RVA: 0x00009B0B File Offset: 0x00007D0B
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.FixRange;
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06000336 RID: 822 RVA: 0x00009B0F File Offset: 0x00007D0F
	public override float KeyValue
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00009B16 File Offset: 0x00007D16
	public override void Build()
	{
		base.Build();
		this.strategy.BaseFixRange += (int)this.KeyValue;
		this.strategy.MaxRange = (int)this.KeyValue;
	}
}
