using System;

// Token: 0x02000166 RID: 358
public class RuleIceBreak : Rule
{
	// Token: 0x17000356 RID: 854
	// (get) Token: 0x0600094C RID: 2380 RVA: 0x00018C22 File Offset: 0x00016E22
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_ICEBREAK;
		}
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x00018C26 File Offset: 0x00016E26
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.IceBreak, false));
	}
}
