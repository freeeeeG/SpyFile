using System;

// Token: 0x02000150 RID: 336
public class MacroRule : Rule
{
	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06000901 RID: 2305 RVA: 0x0001899C File Offset: 0x00016B9C
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06000902 RID: 2306 RVA: 0x0001899F File Offset: 0x00016B9F
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_MACRO;
		}
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x000189A2 File Offset: 0x00016BA2
	public override void OnGameLoad()
	{
		GameRes.GroundSize = 35;
	}
}
