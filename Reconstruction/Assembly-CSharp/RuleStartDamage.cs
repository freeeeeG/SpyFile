using System;

// Token: 0x02000161 RID: 353
public class RuleStartDamage : Rule
{
	// Token: 0x17000351 RID: 849
	// (get) Token: 0x0600093D RID: 2365 RVA: 0x00018B9B File Offset: 0x00016D9B
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_STARTDAMAGE;
		}
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x00018B9F File Offset: 0x00016D9F
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.StartTileDamage, false));
	}
}
