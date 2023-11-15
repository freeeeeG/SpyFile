using System;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class UIMouseTracker : MonoBehaviour
{
	// Token: 0x06000B70 RID: 2928 RVA: 0x0002C9B9 File Offset: 0x0002ABB9
	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.parentCanvas = base.GetComponentInParent<Canvas>();
		this.canvasSize = this.parentCanvas.GetComponent<RectTransform>().sizeDelta;
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0002C9EC File Offset: 0x0002ABEC
	private void Update()
	{
		Vector2 vector = Input.mousePosition;
		Vector2 vector2 = new Vector2(vector.x / (float)Screen.width, vector.y / (float)Screen.height);
		Vector2 anchoredPosition = new Vector2(vector2.x * this.canvasSize.x, vector2.y * this.canvasSize.y) - this.canvasSize * 0.5f;
		this.rectTransform.anchoredPosition = anchoredPosition;
	}

	// Token: 0x0400091D RID: 2333
	private RectTransform rectTransform;

	// Token: 0x0400091E RID: 2334
	private Canvas parentCanvas;

	// Token: 0x0400091F RID: 2335
	private Vector2 canvasSize;
}
