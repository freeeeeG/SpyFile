using System;

// Token: 0x02000168 RID: 360
public class RuleElement : Rule
{
	// Token: 0x17000358 RID: 856
	// (get) Token: 0x06000952 RID: 2386 RVA: 0x00018C57 File Offset: 0x00016E57
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_ELEMENT;
		}
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x00018C5B File Offset: 0x00016E5B
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.AllElement, false));
	}
}
