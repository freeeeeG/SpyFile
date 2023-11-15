using System;

// Token: 0x02000151 RID: 337
public class ColdRule : Rule
{
	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06000905 RID: 2309 RVA: 0x000189B3 File Offset: 0x00016BB3
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06000906 RID: 2310 RVA: 0x000189B6 File Offset: 0x00016BB6
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_COLD;
		}
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x000189B9 File Offset: 0x00016BB9
	public override void OnGameLoad()
	{
		GameRes.EnemyFrostResist -= 1f;
		GameRes.TurretFrostResist -= 1f;
	}
}
