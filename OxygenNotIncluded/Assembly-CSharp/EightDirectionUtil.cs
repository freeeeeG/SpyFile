using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000781 RID: 1921
[SerializationConfig(MemberSerialization.OptIn)]
public class EightDirectionUtil
{
	// Token: 0x0600351A RID: 13594 RVA: 0x0011FCC8 File Offset: 0x0011DEC8
	public static int GetDirectionIndex(EightDirection direction)
	{
		return (int)direction;
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x0011FCCB File Offset: 0x0011DECB
	public static EightDirection AngleToDirection(int angle)
	{
		return (EightDirection)Mathf.Floor((float)angle / 45f);
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x0011FCDB File Offset: 0x0011DEDB
	public static Vector3 GetNormal(EightDirection direction)
	{
		return EightDirectionUtil.normals[EightDirectionUtil.GetDirectionIndex(direction)];
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x0011FCED File Offset: 0x0011DEED
	public static float GetAngle(EightDirection direction)
	{
		return (float)(45 * EightDirectionUtil.GetDirectionIndex(direction));
	}

	// Token: 0x04002039 RID: 8249
	public static readonly Vector3[] normals = new Vector3[]
	{
		Vector3.up,
		(Vector3.up + Vector3.left).normalized,
		Vector3.left,
		(Vector3.down + Vector3.left).normalized,
		Vector3.down,
		(Vector3.down + Vector3.right).normalized,
		Vector3.right,
		(Vector3.up + Vector3.right).normalized
	};
}
