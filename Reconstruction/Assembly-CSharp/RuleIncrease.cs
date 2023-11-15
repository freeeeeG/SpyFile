using System;

// Token: 0x0200015C RID: 348
public class RuleIncrease : Rule
{
	// Token: 0x17000348 RID: 840
	// (get) Token: 0x0600092A RID: 2346 RVA: 0x00018B10 File Offset: 0x00016D10
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x00018B13 File Offset: 0x00016D13
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_INCREASE;
		}
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00018B17 File Offset: 0x00016D17
	public override void OnGameLoad()
	{
		GameRes.IntentLineID = 2;
	}
}
