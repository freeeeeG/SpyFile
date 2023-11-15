using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000230 RID: 560
	[RequireComponent(typeof(Selectable))]
	public class SelectOnHover : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
	{
		// Token: 0x06000C5B RID: 3163 RVA: 0x0002D754 File Offset: 0x0002B954
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.selectable.Select();
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0002D761 File Offset: 0x0002B961
		private void Start()
		{
			this.selectable = base.GetComponent<Selectable>();
		}

		// Token: 0x040008A4 RID: 2212
		private Selectable selectable;
	}
}
