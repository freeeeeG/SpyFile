using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003CC RID: 972
	public class SelectionSpriteSwapper : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
	{
		// Token: 0x06001219 RID: 4633 RVA: 0x00035682 File Offset: 0x00033882
		private void Awake()
		{
			this._image.sprite = this._normal;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00035682 File Offset: 0x00033882
		private void OnDisable()
		{
			this._image.sprite = this._normal;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00035695 File Offset: 0x00033895
		public void OnSelect(BaseEventData eventData)
		{
			this._image.sprite = this._selected;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00035682 File Offset: 0x00033882
		public void OnDeselect(BaseEventData eventData)
		{
			this._image.sprite = this._normal;
		}

		// Token: 0x04000EFD RID: 3837
		[SerializeField]
		private Image _image;

		// Token: 0x04000EFE RID: 3838
		[SerializeField]
		private Sprite _normal;

		// Token: 0x04000EFF RID: 3839
		[SerializeField]
		private Sprite _selected;
	}
}
