using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000191 RID: 401
public class TechItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0001BA25 File Offset: 0x00019C25
	public Technology MyTech
	{
		get
		{
			return this.m_Tech;
		}
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0001BA30 File Offset: 0x00019C30
	public void SetTechItem(Technology tech)
	{
		this.m_Tech = tech;
		this.techAtt = Singleton<StaticData>.Instance.ContentFactory.GetTechAtt(tech.TechnologyName);
		this.techIcon.sprite = this.techAtt.Icon;
		this.abnormalIcon.SetActive(this.m_Tech.IsAbnormal);
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0001BA8B File Offset: 0x00019C8B
	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		Singleton<TipsManager>.Instance.ShowTechInfoTips(this.MyTech, StaticData.LeftTipsPos, false);
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0001BAA3 File Offset: 0x00019CA3
	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		Singleton<TipsManager>.Instance.HideTips();
	}

	// Token: 0x04000563 RID: 1379
	[SerializeField]
	private Image techIcon;

	// Token: 0x04000564 RID: 1380
	private Technology m_Tech;

	// Token: 0x04000565 RID: 1381
	[SerializeField]
	private GameObject abnormalIcon;

	// Token: 0x04000566 RID: 1382
	private TechAttribute techAtt;
}
