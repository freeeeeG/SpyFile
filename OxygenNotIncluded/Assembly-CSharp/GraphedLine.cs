using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

// Token: 0x02000B10 RID: 2832
[AddComponentMenu("KMonoBehaviour/scripts/GraphedLine")]
[Serializable]
public class GraphedLine : KMonoBehaviour
{
	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06005751 RID: 22353 RVA: 0x001FED11 File Offset: 0x001FCF11
	public int PointCount
	{
		get
		{
			return this.points.Length;
		}
	}

	// Token: 0x06005752 RID: 22354 RVA: 0x001FED1B File Offset: 0x001FCF1B
	public void SetPoints(Vector2[] points)
	{
		this.points = points;
		this.UpdatePoints();
	}

	// Token: 0x06005753 RID: 22355 RVA: 0x001FED2C File Offset: 0x001FCF2C
	private void UpdatePoints()
	{
		Vector2[] array = new Vector2[this.points.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.layer.graph.GetRelativePosition(this.points[i]);
		}
		this.line_renderer.Points = array;
	}

	// Token: 0x06005754 RID: 22356 RVA: 0x001FED84 File Offset: 0x001FCF84
	public Vector2 GetClosestDataToPointOnXAxis(Vector2 toPoint)
	{
		float num = toPoint.x / this.layer.graph.rectTransform().sizeDelta.x;
		float num2 = this.layer.graph.axis_x.min_value + this.layer.graph.axis_x.range * num;
		Vector2 vector = Vector2.zero;
		foreach (Vector2 vector2 in this.points)
		{
			if (Mathf.Abs(vector2.x - num2) < Mathf.Abs(vector.x - num2))
			{
				vector = vector2;
			}
		}
		return vector;
	}

	// Token: 0x06005755 RID: 22357 RVA: 0x001FEE2B File Offset: 0x001FD02B
	public void HidePointHighlight()
	{
		if (this.highlightPoint != null)
		{
			this.highlightPoint.SetActive(false);
		}
	}

	// Token: 0x06005756 RID: 22358 RVA: 0x001FEE48 File Offset: 0x001FD048
	public void SetPointHighlight(Vector2 point)
	{
		if (this.highlightPoint == null)
		{
			return;
		}
		this.highlightPoint.SetActive(true);
		Vector2 relativePosition = this.layer.graph.GetRelativePosition(point);
		this.highlightPoint.rectTransform().SetLocalPosition(new Vector2(relativePosition.x * this.layer.graph.rectTransform().sizeDelta.x - this.layer.graph.rectTransform().sizeDelta.x / 2f, relativePosition.y * this.layer.graph.rectTransform().sizeDelta.y - this.layer.graph.rectTransform().sizeDelta.y / 2f));
		ToolTip component = this.layer.graph.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		component.tooltipPositionOffset = new Vector2(this.highlightPoint.rectTransform().localPosition.x, this.layer.graph.rectTransform().rect.height / 2f - 12f);
		component.SetSimpleTooltip(string.Concat(new string[]
		{
			this.layer.graph.axis_x.name,
			" ",
			point.x.ToString(),
			", ",
			Mathf.RoundToInt(point.y).ToString(),
			" ",
			this.layer.graph.axis_y.name
		}));
		ToolTipScreen.Instance.SetToolTip(component);
	}

	// Token: 0x04003AF8 RID: 15096
	public UILineRenderer line_renderer;

	// Token: 0x04003AF9 RID: 15097
	public LineLayer layer;

	// Token: 0x04003AFA RID: 15098
	private Vector2[] points = new Vector2[0];

	// Token: 0x04003AFB RID: 15099
	[SerializeField]
	private GameObject highlightPoint;
}
