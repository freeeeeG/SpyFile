using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
	// Token: 0x0200042E RID: 1070
	public class GearElement : Button
	{
		// Token: 0x06001462 RID: 5218 RVA: 0x0003EA1A File Offset: 0x0003CC1A
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			Action action = this.onSelected;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0003EA34 File Offset: 0x0003CC34
		public void SetIcon(Sprite sprite)
		{
			this._placeholder.color = new Color(1f, 1f, 1f, 0f);
			this._icon.enabled = true;
			this._icon.sprite = sprite;
			this._icon.SetNativeSize();
			this._shadowIcon.enabled = true;
			this._shadowIcon.sprite = sprite;
			this._shadowIcon.SetNativeSize();
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0003EAAB File Offset: 0x0003CCAB
		public void SetSetImage(Sprite image)
		{
			this._setImage.enabled = true;
			this._setImage.sprite = image;
			this._setImage.SetNativeSize();
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0003EAD0 File Offset: 0x0003CCD0
		public void SetSetAnimator(RuntimeAnimatorController animatorController)
		{
			this._setAnimator.enabled = true;
			this._setAnimator.runtimeAnimatorController = animatorController;
			this._setAnimator.Update(0f);
			this._setImage.SetNativeSize();
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0003EB05 File Offset: 0x0003CD05
		public void DisableSetEffect()
		{
			this._setImage.enabled = false;
			this._setAnimator.enabled = false;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0003EB20 File Offset: 0x0003CD20
		public void Deactivate()
		{
			this._placeholder.color = Color.white;
			this._icon.enabled = false;
			this._shadowIcon.enabled = false;
			this._setImage.enabled = false;
			this._setAnimator.enabled = false;
		}

		// Token: 0x04001152 RID: 4434
		public Action onSelected;

		// Token: 0x04001153 RID: 4435
		[SerializeField]
		private Image _placeholder;

		// Token: 0x04001154 RID: 4436
		[SerializeField]
		private Image _icon;

		// Token: 0x04001155 RID: 4437
		[SerializeField]
		private Image _shadowIcon;

		// Token: 0x04001156 RID: 4438
		[SerializeField]
		private Image _setImage;

		// Token: 0x04001157 RID: 4439
		[SerializeField]
		private Animator _setAnimator;

		// Token: 0x04001158 RID: 4440
		[SerializeField]
		private Shadow _shadow;
	}
}
