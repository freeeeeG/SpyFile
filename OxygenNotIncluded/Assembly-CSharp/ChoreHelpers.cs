using System;
using UnityEngine;

// Token: 0x020003AB RID: 939
public static class ChoreHelpers
{
	// Token: 0x06001397 RID: 5015 RVA: 0x00068205 File Offset: 0x00066405
	public static GameObject CreateLocator(string name, Vector3 pos)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(ApproachableLocator.ID), null, null);
		gameObject.name = name;
		gameObject.transform.SetPosition(pos);
		gameObject.gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x0006823D File Offset: 0x0006643D
	public static GameObject CreateSleepLocator(Vector3 pos)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(SleepLocator.ID), null, null);
		gameObject.name = "SLeepLocator";
		gameObject.transform.SetPosition(pos);
		gameObject.gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x00068279 File Offset: 0x00066479
	public static void DestroyLocator(GameObject locator)
	{
		if (locator != null)
		{
			locator.gameObject.DeleteObject();
		}
	}
}
