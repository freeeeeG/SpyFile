using System;

// Token: 0x0200014F RID: 335
public class MicroRule : Rule
{
	// Token: 0x17000336 RID: 822
	// (get) Token: 0x060008FE RID: 2302 RVA: 0x00018988 File Offset: 0x00016B88
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_MICRO;
		}
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0001898B File Offset: 0x00016B8B
	public override void BeforeGameLoad()
	{
		GameRes.GroundSize = 15;
	}
}
