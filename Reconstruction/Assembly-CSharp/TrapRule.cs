using System;

// Token: 0x02000152 RID: 338
public class TrapRule : Rule
{
	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06000909 RID: 2313 RVA: 0x000189E3 File Offset: 0x00016BE3
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_TRAP;
		}
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x000189E6 File Offset: 0x00016BE6
	public override void OnGameLoad()
	{
		GameRes.TrapItensify += 0.5f;
	}
}
