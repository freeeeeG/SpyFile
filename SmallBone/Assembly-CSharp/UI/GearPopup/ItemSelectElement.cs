using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.GearPopup
{
	// Token: 0x02000455 RID: 1109
	public class ItemSelectElement : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x06001521 RID: 5409 RVA: 0x000428C4 File Offset: 0x00040AC4
		public void OnSelect(BaseEventData eventData)
		{
			Action action = this.onSelected;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x000428D6 File Offset: 0x00040AD6
		public void SetIcon(Sprite sprite)
		{
			this._icon.enabled = true;
			this._icon.sprite = sprite;
			this._icon.SetNativeSize();
		}

		// Token: 0x04001275 RID: 4725
		[SerializeField]
		private Image _icon;

		// Token: 0x04001276 RID: 4726
		public Action onSelected;
	}
}
