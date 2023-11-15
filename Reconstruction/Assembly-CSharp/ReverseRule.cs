using System;

// Token: 0x02000153 RID: 339
public class ReverseRule : Rule
{
	// Token: 0x1700033C RID: 828
	// (get) Token: 0x0600090C RID: 2316 RVA: 0x00018A00 File Offset: 0x00016C00
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_REVERSE;
		}
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x00018A03 File Offset: 0x00016C03
	public override void OnGameLoad()
	{
		GameRes.Reverse = true;
	}
}
