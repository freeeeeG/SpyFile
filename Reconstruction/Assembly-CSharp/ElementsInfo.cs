using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000294 RID: 660
public class ElementsInfo : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600103A RID: 4154 RVA: 0x0002BD9F File Offset: 0x00029F9F
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.benefitPanel.InitializePanel(this.m_Strategy);
		this.benefitPanel.Show();
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x0002BDBD File Offset: 0x00029FBD
	public void OnPointerExit(PointerEventData eventData)
	{
		this.benefitPanel.Hide();
	}

	// Token: 0x04000885 RID: 2181
	public StrategyBase m_Strategy;

	// Token: 0x04000886 RID: 2182
	[SerializeField]
	private ElementBenefitPanel benefitPanel;
}
