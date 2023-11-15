using System;

// Token: 0x02000155 RID: 341
public class AmountRule : Rule
{
	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06000912 RID: 2322 RVA: 0x00018A33 File Offset: 0x00016C33
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_AMOUNT;
		}
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x00018A36 File Offset: 0x00016C36
	public override void OnGameLoad()
	{
		GameRes.EnemyAmoundAdjust += 1f;
	}
}
