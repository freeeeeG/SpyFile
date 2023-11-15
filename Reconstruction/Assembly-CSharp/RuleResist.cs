using System;

// Token: 0x0200015F RID: 351
public class RuleResist : Rule
{
	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06000936 RID: 2358 RVA: 0x00018B5C File Offset: 0x00016D5C
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x00018B5F File Offset: 0x00016D5F
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_RESTORE;
		}
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x00018B63 File Offset: 0x00016D63
	public override void OnGameLoad()
	{
		EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.RuleRestoreBuff, 1, false));
	}
}
