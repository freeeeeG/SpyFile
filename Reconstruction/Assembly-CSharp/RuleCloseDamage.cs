using System;

// Token: 0x0200016A RID: 362
public class RuleCloseDamage : Rule
{
	// Token: 0x1700035A RID: 858
	// (get) Token: 0x06000958 RID: 2392 RVA: 0x00018C8D File Offset: 0x00016E8D
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_CLOSEDAMAGE;
		}
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x00018C91 File Offset: 0x00016E91
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.CloseDamage, false));
	}
}
