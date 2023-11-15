using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200013F RID: 319
public class MouseLineRenderer : MonoBehaviour
{
	// Token: 0x06000841 RID: 2113 RVA: 0x0001F3D1 File Offset: 0x0001D5D1
	private void OnEnable()
	{
		EventMgr.Register<bool>(eGameEvents.UI_TogglePlacementPointerArrow, new Action<bool>(this.OnTogglePlacementPointerArrow));
		EventMgr.Register<Transform, Transform>(eGameEvents.UI_SetPlacementPointerArrowTarget, new Action<Transform, Transform>(this.OnSetPlacementPointerArrowTarget));
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x0001F409 File Offset: 0x0001D609
	private void OnDisable()
	{
		EventMgr.Remove<bool>(eGameEvents.UI_TogglePlacementPointerArrow, new Action<bool>(this.OnTogglePlacementPointerArrow));
		EventMgr.Register<Transform, Transform>(eGameEvents.UI_SetPlacementPointerArrowTarget, new Action<Transform, Transform>(this.OnSetPlacementPointerArrowTarget));
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x0001F441 File Offset: 0x0001D641
	private void OnTogglePlacementPointerArrow(bool isOn)
	{
		this.lineRenderer.enabled = isOn;
		this.image_ArrowHead.enabled = isOn;
		this.doUpdate = isOn;
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x0001F462 File Offset: 0x0001D662
	private void OnSetPlacementPointerArrowTarget(Transform from, Transform arg2)
	{
		this.startPointTransform = from;
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0001F46C File Offset: 0x0001D66C
	private void Start()
	{
		if (this.lineRenderer == null)
		{
			this.lineRenderer = base.GetComponent<LineRenderer>();
		}
		if (this.canvas == null)
		{
			this.canvas = base.GetComponentInParent<Canvas>();
		}
		this.lineRenderer.positionCount = this.numOfPoints;
		this.lineRenderer.enabled = false;
		this.image_ArrowHead.enabled = false;
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x0001F4D8 File Offset: 0x0001D6D8
	private void Update()
	{
		if (!this.doUpdate)
		{
			return;
		}
		Vector3[] array = new Vector3[this.numOfPoints];
		Vector3 position = this.startPointTransform.position;
		Vector2 screenPoint = Input.mousePosition;
		Vector2 v;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvas.transform as RectTransform, screenPoint, this.canvas.worldCamera, out v);
		Vector3 end = this.canvas.transform.TransformPoint(v);
		for (int i = 0; i < this.numOfPoints; i++)
		{
			float num = (float)i / (float)(this.numOfPoints - 1);
			array[i] = this.BezierCurve(position, end, num);
			array[i] += this.offset * Mathf.Sin(num * 3.1415927f);
		}
		this.lineRenderer.SetPositions(array);
		this.image_ArrowHead.transform.position = array[array.Length - 1];
		this.image_ArrowHead.transform.up = array[array.Length - 1] - array[array.Length - 2];
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x0001F608 File Offset: 0x0001D808
	private Vector3 BezierCurve(Vector3 start, Vector3 end, float t)
	{
		Vector3 a = new Vector3(start.x, start.y, (start.z + end.z) / 2f);
		float num = 1f - t;
		return num * num * start + 2f * num * t * a + t * t * end;
	}

	// Token: 0x040006AA RID: 1706
	[SerializeField]
	private Vector3 offset = Vector3.zero;

	// Token: 0x040006AB RID: 1707
	[SerializeField]
	private Image image_ArrowHead;

	// Token: 0x040006AC RID: 1708
	public int numOfPoints = 10;

	// Token: 0x040006AD RID: 1709
	public LineRenderer lineRenderer;

	// Token: 0x040006AE RID: 1710
	public Canvas canvas;

	// Token: 0x040006AF RID: 1711
	private bool doUpdate;

	// Token: 0x040006B0 RID: 1712
	private Transform startPointTransform;
}
