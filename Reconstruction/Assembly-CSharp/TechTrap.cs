using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class TechTrap : Technology
{
	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0001D728 File Offset: 0x0001B928
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0001D72B File Offset: 0x0001B92B
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHTRAP;
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0001D72F File Offset: 0x0001B92F
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0001D738 File Offset: 0x0001B938
	public override string DisplayValue1
	{
		get
		{
			return this.KeyValue.ToString();
		}
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x0001D753 File Offset: 0x0001B953
	public override bool OnGet2()
	{
		Singleton<GameManager>.Instance.StartCoroutine(this.ShowNewChoice());
		return false;
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x0001D767 File Offset: 0x0001B967
	private IEnumerator ShowNewChoice()
	{
		yield return new WaitForSeconds(0.5f);
		ChallengeChoice challengeChoice = new ChallengeChoice();
		challengeChoice.Choices = new List<Choice>();
		List<int> list = StaticData.SelectNoRepeat(Singleton<StaticData>.Instance.ContentFactory.TrapNames.Count, 3);
		for (int i = 0; i < 3; i++)
		{
			Choice item = default(Choice);
			item.ChoiceType = ChallengeChoiceType.Trap;
			item.Value1 = Singleton<StaticData>.Instance.ContentFactory.TrapNames[list[i]];
			challengeChoice.Choices.Add(item);
		}
		Singleton<GameManager>.Instance.ShowNormalChoices(true, challengeChoice);
		yield break;
	}
}
