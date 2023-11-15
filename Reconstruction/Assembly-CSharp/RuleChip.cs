using System;

// Token: 0x02000167 RID: 359
public class RuleChip : Rule
{
	// Token: 0x17000357 RID: 855
	// (get) Token: 0x0600094F RID: 2383 RVA: 0x00018C3C File Offset: 0x00016E3C
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_CHIP;
		}
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00018C40 File Offset: 0x00016E40
	public override void OnGameLoad()
	{
		GameRes.SkillChipInterval -= 10;
	}
}
