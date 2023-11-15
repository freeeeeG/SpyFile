using System;

// Token: 0x0200015D RID: 349
public class RuleDecrease : Rule
{
	// Token: 0x1700034A RID: 842
	// (get) Token: 0x0600092E RID: 2350 RVA: 0x00018B27 File Offset: 0x00016D27
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x00018B2A File Offset: 0x00016D2A
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_DECREASE;
		}
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x00018B2E File Offset: 0x00016D2E
	public override void OnGameLoad()
	{
		GameRes.IntentLineID = 1;
	}
}
