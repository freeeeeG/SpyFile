using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000231 RID: 561
	public class SelectorImage : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
	{
		// Token: 0x06000C5E RID: 3166 RVA: 0x0002D76F File Offset: 0x0002B96F
		public void OnSelect(BaseEventData eventData)
		{
			this.img.enabled = true;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0002D77D File Offset: 0x0002B97D
		public void OnDeselect(BaseEventData eventData)
		{
			this.img.enabled = false;
		}

		// Token: 0x040008A5 RID: 2213
		[SerializeField]
		private Image img;
	}
}
