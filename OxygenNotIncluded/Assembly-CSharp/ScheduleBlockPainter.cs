using System;
using UnityEngine;

// Token: 0x02000BE2 RID: 3042
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockPainter")]
public class ScheduleBlockPainter : KMonoBehaviour
{
	// Token: 0x06006042 RID: 24642 RVA: 0x00239495 File Offset: 0x00237695
	public void Setup(Action<float> blockPaintHandler)
	{
		this.blockPaintHandler = blockPaintHandler;
		this.button.onPointerDown += this.OnPointerDown;
		this.button.onDrag += this.OnDrag;
	}

	// Token: 0x06006043 RID: 24643 RVA: 0x002394CC File Offset: 0x002376CC
	private void OnPointerDown()
	{
		this.Transmit();
	}

	// Token: 0x06006044 RID: 24644 RVA: 0x002394D4 File Offset: 0x002376D4
	private void OnDrag()
	{
		this.Transmit();
	}

	// Token: 0x06006045 RID: 24645 RVA: 0x002394DC File Offset: 0x002376DC
	private void Transmit()
	{
		float obj = (base.transform.InverseTransformPoint(KInputManager.GetMousePos()).x - this.rectTransform.rect.x) / this.rectTransform.rect.width;
		this.blockPaintHandler(obj);
	}

	// Token: 0x04004189 RID: 16777
	[SerializeField]
	private KButtonDrag button;

	// Token: 0x0400418A RID: 16778
	private Action<float> blockPaintHandler;

	// Token: 0x0400418B RID: 16779
	[MyCmpGet]
	private RectTransform rectTransform;
}
