using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200017E RID: 382
public class ElementSelectPreview : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060009D9 RID: 2521 RVA: 0x0001AC53 File Offset: 0x00018E53
	public void SetStrategy(StrategyBase strategy)
	{
		this.FrameSprite.color = this.NormalColor;
		this.m_Strategy = strategy;
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x0001AC6D File Offset: 0x00018E6D
	public void OnPointerEnter(PointerEventData eventData)
	{
		Singleton<GameManager>.Instance.PreviewComposition(true, this.m_Strategy.Attribute.element, this.m_Strategy.Quality);
		this.FrameSprite.color = this.HandleColor;
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x0001ACA6 File Offset: 0x00018EA6
	public void OnPointerExit(PointerEventData eventData)
	{
		Singleton<GameManager>.Instance.PreviewComposition(false, ElementType.DUST, 1);
		this.FrameSprite.color = this.NormalColor;
	}

	// Token: 0x04000523 RID: 1315
	[SerializeField]
	private Image FrameSprite;

	// Token: 0x04000524 RID: 1316
	[SerializeField]
	private Color NormalColor;

	// Token: 0x04000525 RID: 1317
	[SerializeField]
	private Color HandleColor;

	// Token: 0x04000526 RID: 1318
	private StrategyBase m_Strategy;
}
