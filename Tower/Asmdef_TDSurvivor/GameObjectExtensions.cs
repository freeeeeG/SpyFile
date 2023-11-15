using System;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public static class GameObjectExtensions
{
	// Token: 0x0600061D RID: 1565 RVA: 0x00017789 File Offset: 0x00015989
	public static void SetActiveIfNot(this GameObject gameObject, bool isOn)
	{
		if (gameObject.activeSelf != isOn)
		{
			gameObject.SetActive(isOn);
		}
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0001779C File Offset: 0x0001599C
	public static void SetLayer(this GameObject parent, int layer, bool includeChildren = true)
	{
		parent.layer = layer;
		if (includeChildren)
		{
			Transform[] componentsInChildren = parent.transform.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layer;
			}
		}
	}
}
