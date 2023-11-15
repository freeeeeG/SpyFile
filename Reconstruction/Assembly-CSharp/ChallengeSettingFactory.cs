using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000014 RID: 20
[CreateAssetMenu(menuName = "Attribute/ChallengeSetting", fileName = "ChallengeSetting")]
public class ChallengeSettingFactory : ScriptableObject
{
	// Token: 0x0600007C RID: 124 RVA: 0x000043A4 File Offset: 0x000025A4
	public ChallengeChoice GetRandomChoice(int level)
	{
		switch (level)
		{
		case 0:
			return this.WhiteChoices[Random.Range(0, this.WhiteChoices.Count)];
		case 1:
			return this.BlueChoices[Random.Range(0, this.BlueChoices.Count)];
		case 2:
			return this.PurpleChoices[Random.Range(0, this.PurpleChoices.Count)];
		case 3:
			return this.GoldChoices[Random.Range(0, this.GoldChoices.Count)];
		default:
			return null;
		}
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00004440 File Offset: 0x00002640
	public List<ChallengeChoice> GetRandomChoices(int level, int amount)
	{
		List<ChallengeChoice> list = new List<ChallengeChoice>();
		List<ChallengeChoice> list2 = null;
		int total = 0;
		switch (level)
		{
		case 0:
			list2 = this.WhiteChoices;
			total = this.WhiteChoices.Count;
			break;
		case 1:
			list2 = this.BlueChoices;
			total = this.BlueChoices.Count;
			break;
		case 2:
			list2 = this.PurpleChoices;
			total = this.PurpleChoices.Count;
			break;
		case 3:
			list2 = this.GoldChoices;
			total = this.GoldChoices.Count;
			break;
		}
		List<int> list3 = StaticData.SelectNoRepeat(total, amount);
		for (int i = 0; i < list3.Count; i++)
		{
			list.Add(list2[list3[i]]);
		}
		return list;
	}

	// Token: 0x04000050 RID: 80
	public List<ChallengeChoice> WhiteChoices;

	// Token: 0x04000051 RID: 81
	public List<ChallengeChoice> BlueChoices;

	// Token: 0x04000052 RID: 82
	public List<ChallengeChoice> PurpleChoices;

	// Token: 0x04000053 RID: 83
	public List<ChallengeChoice> GoldChoices;
}
