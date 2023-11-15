using System;

// Token: 0x02000154 RID: 340
public class StrikeRule : Rule
{
	// Token: 0x1700033D RID: 829
	// (get) Token: 0x0600090F RID: 2319 RVA: 0x00018A13 File Offset: 0x00016C13
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_STRIKE;
		}
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00018A16 File Offset: 0x00016C16
	public override void OnGameLoad()
	{
		EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.RuleStrikeBuff, 1, false));
	}
}
