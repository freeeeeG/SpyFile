using System;

// Token: 0x02000165 RID: 357
public class RuleFixSplash : Rule
{
	// Token: 0x17000355 RID: 853
	// (get) Token: 0x06000949 RID: 2377 RVA: 0x00018C07 File Offset: 0x00016E07
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_FIXSPLASH;
		}
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x00018C0B File Offset: 0x00016E0B
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.FixSplash, false));
	}
}
