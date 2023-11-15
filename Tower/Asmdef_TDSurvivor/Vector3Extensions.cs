using System;
using UnityEngine;

// Token: 0x020000FA RID: 250
public static class Vector3Extensions
{
	// Token: 0x0600062F RID: 1583 RVA: 0x00017ADE File Offset: 0x00015CDE
	public static Vector3 Flattened(this Vector3 vector)
	{
		return new Vector3(vector.x, 0f, vector.z);
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00017AF6 File Offset: 0x00015CF6
	public static float DistanceFlat(this Vector3 origin, Vector3 destination)
	{
		return Vector3.Distance(origin.Flattened(), destination.Flattened());
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x00017B09 File Offset: 0x00015D09
	public static Vector2 ToVector2(this Vector3 v)
	{
		return new Vector2(v.x, v.y);
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x00017B1C File Offset: 0x00015D1C
	public static Vector3Int ToVector3Int(this Vector3 v)
	{
		return Vector3Int.RoundToInt(v);
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00017B24 File Offset: 0x00015D24
	public static Vector3 WithX(this Vector3 v, float x)
	{
		return new Vector3(x, v.y, v.z);
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x00017B38 File Offset: 0x00015D38
	public static Vector3 WithY(this Vector3 v, float y)
	{
		return new Vector3(v.x, y, v.z);
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x00017B4C File Offset: 0x00015D4C
	public static Vector3 WithZ(this Vector3 v, float z)
	{
		return new Vector3(v.x, v.y, z);
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x00017B60 File Offset: 0x00015D60
	public static Vector2 WithX(this Vector2 v, float x)
	{
		return new Vector2(x, v.y);
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x00017B6E File Offset: 0x00015D6E
	public static Vector2 WithY(this Vector2 v, float y)
	{
		return new Vector2(v.x, y);
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x00017B7C File Offset: 0x00015D7C
	public static Vector3 WithZ(this Vector2 v, float z)
	{
		return new Vector3(v.x, v.y, z);
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x00017B90 File Offset: 0x00015D90
	public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
	{
		if (!isNormalized)
		{
			axisDirection.Normalize();
		}
		float d = Vector3.Dot(point, axisDirection);
		return axisDirection * d;
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x00017BB8 File Offset: 0x00015DB8
	public static Vector3 NearestPointOnLine(this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
	{
		if (!isNormalized)
		{
			lineDirection.Normalize();
		}
		float d = Vector3.Dot(point - pointOnLine, lineDirection);
		return pointOnLine + lineDirection * d;
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00017BEA File Offset: 0x00015DEA
	public static Vector2[] ToVector2Array(this Vector3[] v3)
	{
		return Array.ConvertAll<Vector3, Vector2>(v3, new Converter<Vector3, Vector2>(Vector3Extensions.GetV3fromV2));
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00017BFE File Offset: 0x00015DFE
	public static Vector2 GetV3fromV2(Vector3 v3)
	{
		return new Vector2(v3.x, v3.y);
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00017C11 File Offset: 0x00015E11
	public static Vector3[] ToVector3Array(this Vector2[] v2)
	{
		return Array.ConvertAll<Vector2, Vector3>(v2, new Converter<Vector2, Vector3>(Vector3Extensions.GetV2fromV3));
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00017C25 File Offset: 0x00015E25
	public static Vector3 GetV2fromV3(Vector2 v2)
	{
		return new Vector3(v2.x, v2.y, 0f);
	}
}
