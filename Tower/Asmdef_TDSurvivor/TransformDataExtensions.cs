using System;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public static class TransformDataExtensions
{
	// Token: 0x06000B6F RID: 2927 RVA: 0x0002C993 File Offset: 0x0002AB93
	public static void ReadFromData(this Transform transform, TransformData data)
	{
		transform.localPosition = data.Position;
		transform.localRotation = data.Rotation;
		transform.localScale = data.Scale;
	}
}
