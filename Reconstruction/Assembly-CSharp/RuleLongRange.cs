using System;

// Token: 0x02000162 RID: 354
public class RuleLongRange : Rule
{
	// Token: 0x17000352 RID: 850
	// (get) Token: 0x06000940 RID: 2368 RVA: 0x00018BB6 File Offset: 0x00016DB6
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_LONGRANGE;
		}
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00018BBA File Offset: 0x00016DBA
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.LongRange, false));
	}
}
