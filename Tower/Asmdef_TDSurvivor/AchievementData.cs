using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AchievementData", order = 1)]
public class AchievementData : ScriptableObject
{
	// Token: 0x06000006 RID: 6 RVA: 0x00002078 File Offset: 0x00000278
	public string GetDescription(eAchievementType type)
	{
		return this.dic_Achievements[type];
	}

	// Token: 0x04000012 RID: 18
	[SerializeField]
	private AchievementData.AchievementDescriptionDic dic_Achievements;

	// Token: 0x020001D0 RID: 464
	[Serializable]
	public class AchievementDescriptionDic : SerializableDictionary<eAchievementType, string>
	{
	}
}
