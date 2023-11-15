using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000237 RID: 567
	[RequireComponent(typeof(Toggle))]
	public class ToggleOnSelect : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x06000C7C RID: 3196 RVA: 0x0002DCC8 File Offset: 0x0002BEC8
		private void Start()
		{
			this.toggle = base.GetComponent<Toggle>();
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0002DCD6 File Offset: 0x0002BED6
		public void OnSelect(BaseEventData eventData)
		{
			this.toggle.isOn = true;
		}

		// Token: 0x040008BE RID: 2238
		private Toggle toggle;
	}
}
