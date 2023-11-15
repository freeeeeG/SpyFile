using System;

// Token: 0x0200015E RID: 350
public class RuleForbid : Rule
{
	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06000932 RID: 2354 RVA: 0x00018B3E File Offset: 0x00016D3E
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06000933 RID: 2355 RVA: 0x00018B41 File Offset: 0x00016D41
	public override RuleName RuleName
	{
		get
		{
			return RuleName.RULE_FORBID;
		}
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00018B45 File Offset: 0x00016D45
	public override void OnGameLoad()
	{
		TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.FirerateLimit, false));
	}
}
