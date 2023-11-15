using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace flanne.UI
{
	// Token: 0x02000222 RID: 546
	public class DoubleClickDetector : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06000C12 RID: 3090 RVA: 0x0002CAD8 File Offset: 0x0002ACD8
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.clickCount == 2)
			{
				UnityEvent unityEvent = this.onDoubleClick;
				if (unityEvent == null)
				{
					return;
				}
				unityEvent.Invoke();
			}
		}

		// Token: 0x04000877 RID: 2167
		public UnityEvent onDoubleClick;
	}
}
