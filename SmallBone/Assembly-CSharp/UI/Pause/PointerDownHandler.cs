using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Pause
{
	// Token: 0x02000424 RID: 1060
	internal class PointerDownHandler : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x06001410 RID: 5136 RVA: 0x0003D43E File Offset: 0x0003B63E
		public void OnPointerDown(PointerEventData eventData)
		{
			Action action = this.onPointerDown;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04001107 RID: 4359
		internal Action onPointerDown;
	}
}
