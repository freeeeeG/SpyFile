using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000BE7 RID: 3047
public class ScheduleScreenColumnEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler
{
	// Token: 0x0600606A RID: 24682 RVA: 0x0023A154 File Offset: 0x00238354
	public void OnPointerEnter(PointerEventData event_data)
	{
		this.RunCallbacks();
	}

	// Token: 0x0600606B RID: 24683 RVA: 0x0023A15C File Offset: 0x0023835C
	private void RunCallbacks()
	{
		if (Input.GetMouseButton(0) && this.onLeftClick != null)
		{
			this.onLeftClick();
		}
	}

	// Token: 0x0600606C RID: 24684 RVA: 0x0023A179 File Offset: 0x00238379
	public void OnPointerDown(PointerEventData event_data)
	{
		this.RunCallbacks();
	}

	// Token: 0x040041A7 RID: 16807
	public Image image;

	// Token: 0x040041A8 RID: 16808
	public System.Action onLeftClick;
}
