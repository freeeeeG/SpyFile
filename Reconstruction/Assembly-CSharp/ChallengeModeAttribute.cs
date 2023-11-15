using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000010 RID: 16
[CreateAssetMenu(menuName = "Attribute/ChallengeModeAttribute", fileName = "ChallengeModeAttribute")]
public class ChallengeModeAttribute : ScriptableObject
{
	// Token: 0x06000078 RID: 120 RVA: 0x00004180 File Offset: 0x00002380
	private void SaveCurrentPath()
	{
		this.Paths.Clear();
		foreach (PathPoint pathPoint in BoardSystem.shortestPoints)
		{
			this.Paths.Add(pathPoint.PathPos);
		}
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000041E8 File Offset: 0x000023E8
	private void GenerateRandomChoice()
	{
		ChallengeChoice challengeChoice = new ChallengeChoice();
		challengeChoice.Choices = new List<Choice>();
		ChallengeChoiceType challengeChoiceType = (Random.value > 0.75f) ? ChallengeChoiceType.Trap : ChallengeChoiceType.Turret;
		if (challengeChoiceType != ChallengeChoiceType.Turret)
		{
			if (challengeChoiceType == ChallengeChoiceType.Trap)
			{
				List<int> list = StaticData.SelectNoRepeat(Singleton<StaticData>.Instance.ContentFactory.TrapNames.Count, 3);
				for (int i = 0; i < 3; i++)
				{
					Choice item = default(Choice);
					item.ChoiceType = challengeChoiceType;
					item.Value1 = Singleton<StaticData>.Instance.ContentFactory.TrapNames[list[i]];
					challengeChoice.Choices.Add(item);
				}
			}
		}
		else
		{
			List<int> list2 = StaticData.SelectNoRepeat(Singleton<StaticData>.Instance.ContentFactory.RefactorTurretNames.Count, 3);
			for (int j = 0; j < 3; j++)
			{
				Choice choice = default(Choice);
				choice.ChoiceType = challengeChoiceType;
				choice.Value1 = Singleton<StaticData>.Instance.ContentFactory.RefactorTurretNames[list2[j]];
				choice.Elements = null;
				while (choice.Elements == null || (choice.Elements[0] == 0 && choice.Elements[1] == 2 && choice.Elements[2] == 4))
				{
					choice.Elements = new List<int>();
					for (int k = 0; k < 3; k++)
					{
						int element = (int)Singleton<StaticData>.Instance.ContentFactory.GetRandomElementAttribute().element;
						choice.Elements.Add(element);
					}
				}
				challengeChoice.Choices.Add(choice);
			}
		}
		this.Choices.Add(challengeChoice);
	}

	// Token: 0x04000045 RID: 69
	public int[] WaveRequired;

	// Token: 0x04000046 RID: 70
	public List<Vector2> Paths;

	// Token: 0x04000047 RID: 71
	public List<ChallengeChoice> Choices;
}
