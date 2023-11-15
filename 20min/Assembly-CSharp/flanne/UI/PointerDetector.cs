using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace flanne.UI
{
	// Token: 0x0200022E RID: 558
	public class PointerDetector : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
	{
		// Token: 0x06000C52 RID: 3154 RVA: 0x0002D6E7 File Offset: 0x0002B8E7
		public void OnPointerEnter(PointerEventData pointerEventData)
		{
			this.onEnter.Invoke();
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0002D6F4 File Offset: 0x0002B8F4
		public void OnPointerExit(PointerEventData pointerEventData)
		{
			this.onExit.Invoke();
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0002D701 File Offset: 0x0002B901
		public void OnSelect(BaseEventData eventData)
		{
			this.onSelect.Invoke();
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0002D70E File Offset: 0x0002B90E
		public void OnDeselect(BaseEventData eventData)
		{
			this.onDeselect.Invoke();
		}

		// Token: 0x0400089E RID: 2206
		public UnityEvent onEnter;

		// Token: 0x0400089F RID: 2207
		public UnityEvent onExit;

		// Token: 0x040008A0 RID: 2208
		public UnityEvent onSelect;

		// Token: 0x040008A1 RID: 2209
		public UnityEvent onDeselect;
	}
}
