using System;

// Token: 0x02000163 RID: 355
public class RuleFixFirerate : Rule
{
	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06000943 RID: 2371 RVA: 0x00018BD1 File Offset: 0x00016DD1
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_FIXFIRERATE;
		}
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00018BD5 File Offset: 0x00016DD5
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.FixFirerate, false));
	}
}
