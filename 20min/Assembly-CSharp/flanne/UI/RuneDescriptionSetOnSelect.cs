using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace flanne.UI
{
	// Token: 0x0200020F RID: 527
	public class RuneDescriptionSetOnSelect : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x06000BDB RID: 3035 RVA: 0x0002C1ED File Offset: 0x0002A3ED
		public void OnSelect(BaseEventData eventData)
		{
			this.runeDescription.data = this.runeIcon.data;
		}

		// Token: 0x04000847 RID: 2119
		[SerializeField]
		private RuneIcon runeIcon;

		// Token: 0x04000848 RID: 2120
		[SerializeField]
		private RuneDescription runeDescription;
	}
}
