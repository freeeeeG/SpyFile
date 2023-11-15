using System;

// Token: 0x0200015B RID: 347
public class RuleAirborne : Rule
{
	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06000926 RID: 2342 RVA: 0x00018AEC File Offset: 0x00016CEC
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06000927 RID: 2343 RVA: 0x00018AEF File Offset: 0x00016CEF
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_AIRBORNE;
		}
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00018AF3 File Offset: 0x00016CF3
	public override void OnGameLoad()
	{
		EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.RuleAirborneBuff, 1, false));
	}
}
