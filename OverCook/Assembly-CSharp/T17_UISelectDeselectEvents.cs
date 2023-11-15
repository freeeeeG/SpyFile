using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000B5F RID: 2911
public class T17_UISelectDeselectEvents : MonoBehaviour, ISelectHandler, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x06003B20 RID: 15136 RVA: 0x00119B69 File Offset: 0x00117F69
	public virtual void OnSelect(BaseEventData eventData)
	{
		if (this.m_OnSelectEvent != null)
		{
			this.m_OnSelectEvent.Invoke(eventData);
		}
	}

	// Token: 0x06003B21 RID: 15137 RVA: 0x00119B82 File Offset: 0x00117F82
	public virtual void OnDeselect(BaseEventData eventData)
	{
		if (this.m_OnDeselectEvent != null)
		{
			this.m_OnDeselectEvent.Invoke(eventData);
		}
	}

	// Token: 0x0400301C RID: 12316
	public EventTrigger.TriggerEvent m_OnSelectEvent = new EventTrigger.TriggerEvent();

	// Token: 0x0400301D RID: 12317
	public EventTrigger.TriggerEvent m_OnDeselectEvent = new EventTrigger.TriggerEvent();
}
