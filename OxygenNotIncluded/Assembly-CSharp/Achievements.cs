using System;
using UnityEngine;

// Token: 0x02000582 RID: 1410
[AddComponentMenu("KMonoBehaviour/scripts/Achievements")]
public class Achievements : KMonoBehaviour
{
	// Token: 0x06002224 RID: 8740 RVA: 0x000BBA69 File Offset: 0x000B9C69
	public void Unlock(string id)
	{
		if (SteamAchievementService.Instance)
		{
			SteamAchievementService.Instance.Unlock(id);
		}
	}
}
