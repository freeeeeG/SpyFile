using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019F RID: 415
public static class MathUtils
{
	// Token: 0x06000B1B RID: 2843 RVA: 0x0002AFA4 File Offset: 0x000291A4
	private static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		return Mathf.Pow(1f - t, 3f) * p0 + 3f * Mathf.Pow(1f - t, 2f) * t * p1 + 3f * (1f - t) * Mathf.Pow(t, 2f) * p2 + Mathf.Pow(t, 3f) * p3;
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x0002B02C File Offset: 0x0002922C
	public static List<Vector3> CreateCurveLine(Vector3 start, Vector3 end, float curvature, int numPoints)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 vector = end - start;
		Vector3 normalized = Vector3.Cross(vector, Vector3.forward).normalized;
		Vector3 p = start + vector * 0.33f + normalized * curvature;
		Vector3 p2 = start + vector * 0.66f + normalized * curvature;
		for (int i = 0; i < numPoints; i++)
		{
			float t = (float)i / (float)(numPoints - 1);
			Vector3 item = MathUtils.Bezier(start, p, p2, end, t);
			item.z = 0f;
			list.Add(item);
		}
		return list;
	}
}
