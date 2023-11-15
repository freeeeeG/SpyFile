using System;

// Token: 0x0200015A RID: 346
public class RuleFrost : Rule
{
	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06000922 RID: 2338 RVA: 0x00018ACF File Offset: 0x00016CCF
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06000923 RID: 2339 RVA: 0x00018AD2 File Offset: 0x00016CD2
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_FROST;
		}
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00018AD6 File Offset: 0x00016CD6
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.RuleFrostBuff, false));
	}
}
