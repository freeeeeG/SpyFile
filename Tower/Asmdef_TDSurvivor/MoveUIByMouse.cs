using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001A0 RID: 416
public class MoveUIByMouse : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
{
	// Token: 0x06000B1D RID: 2845 RVA: 0x0002B0D6 File Offset: 0x000292D6
	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.targetAnchorPos = this.rectTransform.anchoredPosition;
		this.initialPos = this.rectTransform.anchoredPosition;
		this.totalOffset = Vector2.zero;
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0002B114 File Offset: 0x00029314
	private void Update()
	{
		this.rectTransform.anchoredPosition = Vector2.Lerp(this.rectTransform.anchoredPosition, this.targetAnchorPos, Time.deltaTime * 10f);
		this.totalOffset = this.initialPos - this.rectTransform.anchoredPosition;
		this.totalOffset.x = this.totalOffset.x / 1920f;
		this.totalOffset.y = this.totalOffset.y / 1080f;
		this.mat_Map.SetVector("_MapOffset", this.totalOffset);
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0002B1BC File Offset: 0x000293BC
	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
		{
			this.isDragging = true;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, eventData.position, eventData.pressEventCamera, out this.pointerOffset);
		}
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x0002B1F4 File Offset: 0x000293F4
	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
		{
			this.isDragging = false;
		}
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x0002B210 File Offset: 0x00029410
	public void OnDrag(PointerEventData eventData)
	{
		if (this.isDragging)
		{
			Vector2 a;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out a);
			this.targetAnchorPos = (a - this.pointerOffset) * this.scaleMultiplier;
		}
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x0002B266 File Offset: 0x00029466
	public void CenterToPosition(Vector3 position, Vector3 offset)
	{
		this.targetAnchorPos = -1f * base.transform.InverseTransformPoint(position) + offset;
	}

	// Token: 0x040008EF RID: 2287
	private RectTransform rectTransform;

	// Token: 0x040008F0 RID: 2288
	[SerializeField]
	private Canvas canvas;

	// Token: 0x040008F1 RID: 2289
	[SerializeField]
	private Material mat_Map;

	// Token: 0x040008F2 RID: 2290
	[SerializeField]
	private float materialOffsetScale = 1f;

	// Token: 0x040008F3 RID: 2291
	[SerializeField]
	private float scaleMultiplierMin = 0.5f;

	// Token: 0x040008F4 RID: 2292
	[SerializeField]
	private float scaleMultiplierMax = 1f;

	// Token: 0x040008F5 RID: 2293
	[SerializeField]
	private float mouseScrollFactor = 0.1f;

	// Token: 0x040008F6 RID: 2294
	private bool isDragging;

	// Token: 0x040008F7 RID: 2295
	private Vector2 pointerOffset;

	// Token: 0x040008F8 RID: 2296
	private Vector2 targetAnchorPos;

	// Token: 0x040008F9 RID: 2297
	private Vector2 initialPos;

	// Token: 0x040008FA RID: 2298
	private Vector2 totalOffset;

	// Token: 0x040008FB RID: 2299
	private float scaleMultiplier = 1f;
}
