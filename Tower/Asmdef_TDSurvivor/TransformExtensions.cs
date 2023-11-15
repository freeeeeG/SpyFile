using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public static class TransformExtensions
{
	// Token: 0x0600062A RID: 1578 RVA: 0x00017A5F File Offset: 0x00015C5F
	public static void CopyPosition(this Transform transform, Transform from)
	{
		transform.position = from.position;
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x00017A6D File Offset: 0x00015C6D
	public static void CopyRotation(this Transform transform, Transform from)
	{
		transform.rotation = from.rotation;
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x00017A7B File Offset: 0x00015C7B
	public static void CopyPosAndRot(this Transform transform, Transform from)
	{
		transform.position = from.position;
		transform.rotation = from.rotation;
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x00017A95 File Offset: 0x00015C95
	public static void CopyTransform(this Transform transform, Transform from)
	{
		transform.position = from.position;
		transform.rotation = from.rotation;
		transform.localScale = from.localScale;
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x00017ABB File Offset: 0x00015CBB
	public static void ResetLocalTransform(this Transform transform)
	{
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}
}
