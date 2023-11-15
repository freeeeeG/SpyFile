using System;

// Token: 0x02000160 RID: 352
public class RuleDistanceDamage : Rule
{
	// Token: 0x17000350 RID: 848
	// (get) Token: 0x0600093A RID: 2362 RVA: 0x00018B80 File Offset: 0x00016D80
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_DISTANCEDAMAGE;
		}
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00018B84 File Offset: 0x00016D84
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.TileBaseDamage, false));
	}
}
