using System;

// Token: 0x02000088 RID: 136
public class FixSplash : GlobalSkill
{
	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000339 RID: 825 RVA: 0x00009B51 File Offset: 0x00007D51
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.FixSplash;
		}
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x0600033A RID: 826 RVA: 0x00009B55 File Offset: 0x00007D55
	public override float KeyValue
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00009B5C File Offset: 0x00007D5C
	public override void Build()
	{
		base.Build();
		this.strategy.BaseFixSplash += this.KeyValue;
		this.strategy.MaxSplash = this.KeyValue;
	}
}
