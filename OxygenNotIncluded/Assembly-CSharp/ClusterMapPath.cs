using System;
using System.Collections.Generic;
using System.Linq;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000AB5 RID: 2741
public class ClusterMapPath : MonoBehaviour
{
	// Token: 0x060053CD RID: 21453 RVA: 0x001E3140 File Offset: 0x001E1340
	public void Init()
	{
		this.lineRenderer = base.gameObject.GetComponentInChildren<UILineRenderer>();
		base.gameObject.SetActive(true);
	}

	// Token: 0x060053CE RID: 21454 RVA: 0x001E315F File Offset: 0x001E135F
	public void Init(List<Vector2> nodes, Color color)
	{
		this.m_nodes = nodes;
		this.m_color = color;
		this.lineRenderer = base.gameObject.GetComponentInChildren<UILineRenderer>();
		this.UpdateColor();
		this.UpdateRenderer();
		base.gameObject.SetActive(true);
	}

	// Token: 0x060053CF RID: 21455 RVA: 0x001E3198 File Offset: 0x001E1398
	public void SetColor(Color color)
	{
		this.m_color = color;
		this.UpdateColor();
	}

	// Token: 0x060053D0 RID: 21456 RVA: 0x001E31A7 File Offset: 0x001E13A7
	private void UpdateColor()
	{
		this.lineRenderer.color = this.m_color;
		this.pathStart.color = this.m_color;
		this.pathEnd.color = this.m_color;
	}

	// Token: 0x060053D1 RID: 21457 RVA: 0x001E31DC File Offset: 0x001E13DC
	public void SetPoints(List<Vector2> points)
	{
		this.m_nodes = points;
		this.UpdateRenderer();
	}

	// Token: 0x060053D2 RID: 21458 RVA: 0x001E31EC File Offset: 0x001E13EC
	private void UpdateRenderer()
	{
		HashSet<Vector2> pointsOnCatmullRomSpline = ProcGen.Util.GetPointsOnCatmullRomSpline(this.m_nodes, 10);
		this.lineRenderer.Points = pointsOnCatmullRomSpline.ToArray<Vector2>();
		if (this.lineRenderer.Points.Length > 1)
		{
			this.pathStart.transform.localPosition = this.lineRenderer.Points[0];
			this.pathStart.gameObject.SetActive(true);
			Vector2 vector = this.lineRenderer.Points[this.lineRenderer.Points.Length - 1];
			Vector2 b = this.lineRenderer.Points[this.lineRenderer.Points.Length - 2];
			this.pathEnd.transform.localPosition = vector;
			Vector2 v = vector - b;
			this.pathEnd.transform.rotation = Quaternion.LookRotation(Vector3.forward, v);
			this.pathEnd.gameObject.SetActive(true);
			return;
		}
		this.pathStart.gameObject.SetActive(false);
		this.pathEnd.gameObject.SetActive(false);
	}

	// Token: 0x060053D3 RID: 21459 RVA: 0x001E3314 File Offset: 0x001E1514
	public float GetRotationForNextSegment()
	{
		if (this.m_nodes.Count > 1)
		{
			Vector2 b = this.m_nodes[0];
			Vector2 to = this.m_nodes[1] - b;
			return Vector2.SignedAngle(Vector2.up, to);
		}
		return 0f;
	}

	// Token: 0x04003802 RID: 14338
	private List<Vector2> m_nodes;

	// Token: 0x04003803 RID: 14339
	private Color m_color;

	// Token: 0x04003804 RID: 14340
	public UILineRenderer lineRenderer;

	// Token: 0x04003805 RID: 14341
	public Image pathStart;

	// Token: 0x04003806 RID: 14342
	public Image pathEnd;
}
