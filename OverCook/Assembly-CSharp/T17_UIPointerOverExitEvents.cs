using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000B5E RID: 2910
public class T17_UIPointerOverExitEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	// Token: 0x06003B1D RID: 15133 RVA: 0x00119B19 File Offset: 0x00117F19
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		if (this.m_OnPointerEnterEvent != null)
		{
			this.m_OnPointerEnterEvent.Invoke(eventData);
		}
	}

	// Token: 0x06003B1E RID: 15134 RVA: 0x00119B32 File Offset: 0x00117F32
	public virtual void OnPointerExit(PointerEventData eventData)
	{
		if (this.m_OnPointerExitEvent != null)
		{
			this.m_OnPointerExitEvent.Invoke(eventData);
		}
	}

	// Token: 0x0400301A RID: 12314
	public EventTrigger.TriggerEvent m_OnPointerEnterEvent = new EventTrigger.TriggerEvent();

	// Token: 0x0400301B RID: 12315
	public EventTrigger.TriggerEvent m_OnPointerExitEvent = new EventTrigger.TriggerEvent();
}
