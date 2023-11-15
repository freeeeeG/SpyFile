using System;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
public static class HandlePlacementUtils
{
	// Token: 0x0600168B RID: 5771 RVA: 0x00076B30 File Offset: 0x00074F30
	public static T GetHighestPriority<T>(T[] _iHandlePlacements) where T : IBaseHandlePlacement
	{
		T t = default(T);
		foreach (T t2 in _iHandlePlacements)
		{
			if ((!(t2 is MonoBehaviour) || (t2 as MonoBehaviour).enabled) && (t == null || t2.GetPlacementPriority() > t.GetPlacementPriority()))
			{
				t = t2;
			}
		}
		return t;
	}
}
