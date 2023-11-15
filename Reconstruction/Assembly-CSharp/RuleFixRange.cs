using System;

// Token: 0x02000164 RID: 356
public class RuleFixRange : Rule
{
	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06000946 RID: 2374 RVA: 0x00018BEC File Offset: 0x00016DEC
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_FIXRANGE;
		}
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00018BF0 File Offset: 0x00016DF0
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.FixRange, false));
	}
}
