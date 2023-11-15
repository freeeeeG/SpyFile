using System;

// Token: 0x02000157 RID: 343
public class RuleFast : Rule
{
	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06000918 RID: 2328 RVA: 0x00018A6D File Offset: 0x00016C6D
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_FAST;
		}
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x00018A71 File Offset: 0x00016C71
	public override void OnGameLoad()
	{
		EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.RuleFastBuff, 1, false));
	}
}
