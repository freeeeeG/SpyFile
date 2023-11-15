using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001AC RID: 428
public class UI_ScaleOnMouseOver : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000B77 RID: 2935 RVA: 0x0002CFBE File Offset: 0x0002B1BE
	private void Awake()
	{
		this.originalScale = base.transform.localScale;
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x0002CFD1 File Offset: 0x0002B1D1
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.cardMouseOverTweener != null && this.cardMouseOverTweener.IsActive())
		{
			this.cardMouseOverTweener.Complete();
		}
		this.cardMouseOverTweener = base.transform.DOScale(this.targetScale, this.duration);
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x0002D010 File Offset: 0x0002B210
	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.cardMouseOverTweener != null && this.cardMouseOverTweener.IsActive())
		{
			this.cardMouseOverTweener.Complete();
		}
		this.cardMouseOverTweener = base.transform.DOScale(this.originalScale, this.duration);
	}

	// Token: 0x04000928 RID: 2344
	[SerializeField]
	private Vector3 targetScale = Vector3.one * 1.2f;

	// Token: 0x04000929 RID: 2345
	[SerializeField]
	private float duration = 0.15f;

	// Token: 0x0400092A RID: 2346
	private Vector3 originalScale = Vector3.one;

	// Token: 0x0400092B RID: 2347
	private Tweener cardMouseOverTweener;
}
