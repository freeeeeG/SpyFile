using System;

// Token: 0x02000158 RID: 344
public class RuleTileRecover : Rule
{
	// Token: 0x17000341 RID: 833
	// (get) Token: 0x0600091B RID: 2331 RVA: 0x00018A8E File Offset: 0x00016C8E
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_TILERECOVER;
		}
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00018A92 File Offset: 0x00016C92
	public override void OnGameLoad()
	{
		EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.TileRecover, 1, false));
	}
}
