using System;

// Token: 0x02000156 RID: 342
public class HeavyRule : Rule
{
	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06000915 RID: 2325 RVA: 0x00018A50 File Offset: 0x00016C50
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_HEAVY;
		}
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00018A53 File Offset: 0x00016C53
	public override void OnGameLoad()
	{
		GameRes.EnemyIntensifyAdjust += 1f;
	}
}
