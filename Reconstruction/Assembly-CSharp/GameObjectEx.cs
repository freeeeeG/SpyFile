using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
public static class GameObjectEx
{
	// Token: 0x060006E7 RID: 1767 RVA: 0x00012E5C File Offset: 0x0001105C
	public static void DrawCircle(this GameObject container, float radius, float lineWidth, Color lineColor)
	{
		int num = 360;
		LineRenderer lineRenderer = container.GetComponent<LineRenderer>();
		if (lineRenderer == null)
		{
			lineRenderer = container.AddComponent<LineRenderer>();
		}
		lineRenderer.useWorldSpace = false;
		lineRenderer.startWidth = lineWidth;
		lineRenderer.endWidth = lineWidth;
		lineRenderer.positionCount = num + 1;
		lineRenderer.startColor = lineColor;
		lineRenderer.endColor = lineColor;
		int num2 = num + 1;
		Vector3[] array = new Vector3[num2];
		for (int i = 0; i < num2; i++)
		{
			float f = 0.017453292f * ((float)i * 360f / (float)num);
			array[i] = new Vector3(Mathf.Sin(f) * radius, Mathf.Cos(f) * radius, 0f);
		}
		lineRenderer.SetPositions(array);
		lineRenderer.enabled = true;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x00012F14 File Offset: 0x00011114
	public static void HideCircle(this GameObject container)
	{
		LineRenderer component = container.GetComponent<LineRenderer>();
		if (component != null)
		{
			component.enabled = false;
		}
	}
}
