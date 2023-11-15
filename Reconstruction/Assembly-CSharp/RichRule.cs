using System;

// Token: 0x02000159 RID: 345
public class RichRule : Rule
{
	// Token: 0x17000342 RID: 834
	// (get) Token: 0x0600091E RID: 2334 RVA: 0x00018AAF File Offset: 0x00016CAF
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x0600091F RID: 2335 RVA: 0x00018AB2 File Offset: 0x00016CB2
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_RICH;
		}
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00018AB5 File Offset: 0x00016CB5
	public override void OnGameLoad()
	{
		GameRes.CoinAdjust += 1f;
	}
}
