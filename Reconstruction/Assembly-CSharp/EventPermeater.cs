using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000245 RID: 581
public class EventPermeater : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerClickHandler
{
	// Token: 0x06000EEA RID: 3818 RVA: 0x000279D0 File Offset: 0x00025BD0
	public void SetTarget(GameObject target)
	{
		if (target != null)
		{
			this.m_Target = target;
			this.eventPermeaterMask.SetActive(true);
			return;
		}
		this.eventPermeaterMask.SetActive(false);
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x000279FB File Offset: 0x00025BFB
	public void OnPointerDown(PointerEventData eventData)
	{
		this.PassEvent<IPointerDownHandler>(eventData, ExecuteEvents.pointerDownHandler);
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x00027A09 File Offset: 0x00025C09
	public void OnPointerUp(PointerEventData eventData)
	{
		this.PassEvent<IPointerUpHandler>(eventData, ExecuteEvents.pointerUpHandler);
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x00027A17 File Offset: 0x00025C17
	public void OnPointerClick(PointerEventData eventData)
	{
		this.PassEvent<IPointerClickHandler>(eventData, ExecuteEvents.pointerClickHandler);
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x00027A28 File Offset: 0x00025C28
	public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
	{
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(data, list);
		GameObject gameObject = data.pointerCurrentRaycast.gameObject;
		for (int i = 0; i < list.Count; i++)
		{
			if (this.m_Target == list[i].gameObject)
			{
				ExecuteEvents.Execute<T>(list[i].gameObject, data, function);
				return;
			}
		}
	}

	// Token: 0x0400075E RID: 1886
	[SerializeField]
	private GameObject m_Target;

	// Token: 0x0400075F RID: 1887
	[SerializeField]
	private GameObject eventPermeaterMask;
}
