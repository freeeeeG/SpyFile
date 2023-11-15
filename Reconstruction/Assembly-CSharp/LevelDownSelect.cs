using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200017F RID: 383
public class LevelDownSelect : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060009DD RID: 2525 RVA: 0x0001ACCE File Offset: 0x00018ECE
	public void SetStrategy(StrategyBase strategy)
	{
		this.m_Strategy = new StrategyBase(strategy.Attribute, strategy.Quality - 1);
		this.m_Strategy.SetQualityValue();
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x0001ACF4 File Offset: 0x00018EF4
	public void OnPointerEnter(PointerEventData eventData)
	{
		Singleton<GameManager>.Instance.PreviewComposition(true, this.m_Strategy.Attribute.element, this.m_Strategy.Quality);
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x0001AD1C File Offset: 0x00018F1C
	public void OnPointerExit(PointerEventData eventData)
	{
		Singleton<GameManager>.Instance.PreviewComposition(false, ElementType.DUST, 1);
	}

	// Token: 0x04000527 RID: 1319
	public StrategyBase m_Strategy;
}
