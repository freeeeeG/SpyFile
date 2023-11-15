using System;
using UnityEngine;

// Token: 0x02000283 RID: 643
public static class RectTransformUtils
{
	// Token: 0x06000BD3 RID: 3027 RVA: 0x0003DC58 File Offset: 0x0003C058
	public static Vector3 GetSize(this RectTransform _target)
	{
		Vector3[] array = new Vector3[4];
		_target.GetWorldCorners(array);
		Bounds bounds = new Bounds(array[0], Vector3.zero);
		for (int i = 1; i < 4; i++)
		{
			bounds.Encapsulate(array[i]);
		}
		return bounds.size;
	}
}
