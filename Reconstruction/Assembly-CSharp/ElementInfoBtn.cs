using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000235 RID: 565
public class ElementInfoBtn : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000E9F RID: 3743 RVA: 0x00025C68 File Offset: 0x00023E68
	public void SetStrategy(StrategyBase strategy)
	{
		this.m_Strategy = strategy;
		this.m_Txt.text = StaticData.FormElementName(this.m_Strategy.Attribute.element, this.m_Strategy.Quality);
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x00025C9C File Offset: 0x00023E9C
	public void OnPointerEnter(PointerEventData eventData)
	{
		Singleton<GameManager>.Instance.PreviewComposition(true, this.m_Strategy.Attribute.element, this.m_Strategy.Quality);
		Singleton<TipsManager>.Instance.ShowTurreTips(this.m_Strategy, StaticData.LeftMidTipsPos, 2);
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x00025CDA File Offset: 0x00023EDA
	public void OnPointerExit(PointerEventData eventData)
	{
		Singleton<GameManager>.Instance.PreviewComposition(false, ElementType.DUST, 1);
		Singleton<TipsManager>.Instance.HideTips();
	}

	// Token: 0x04000706 RID: 1798
	private StrategyBase m_Strategy;

	// Token: 0x04000707 RID: 1799
	[SerializeField]
	private TextMeshProUGUI m_Txt;
}
