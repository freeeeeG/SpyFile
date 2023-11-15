using System;

// Token: 0x02000169 RID: 361
public class RuleFarDamage : Rule
{
	// Token: 0x17000359 RID: 857
	// (get) Token: 0x06000955 RID: 2389 RVA: 0x00018C72 File Offset: 0x00016E72
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_FARDAMAGE;
		}
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x00018C76 File Offset: 0x00016E76
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.FarDamage, false));
	}
}
