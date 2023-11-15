using System;
using GameResources;
using InControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserInput;

namespace UI.Pause
{
	// Token: 0x0200041A RID: 1050
	public class KeyBinder : MonoBehaviour
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x0003CE62 File Offset: 0x0003B062
		public Button button
		{
			get
			{
				return this._button;
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0003CE6C File Offset: 0x0003B06C
		public void Initialize(PlayerAction action, PressNewKey pressNewKey)
		{
			KeyBinder.<>c__DisplayClass6_0 CS$<>8__locals1 = new KeyBinder.<>c__DisplayClass6_0();
			CS$<>8__locals1.pressNewKey = pressNewKey;
			CS$<>8__locals1.action = action;
			CS$<>8__locals1.<>4__this = this;
			this._button.onClick.AddListener(new UnityAction(CS$<>8__locals1.<Initialize>g__OnClick|0));
			this._action = CS$<>8__locals1.action;
			this._action.OnBindingsChanged += this.UpdateKeyImageAndBindingSource;
			this.UpdateKeyImageAndBindingSource();
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0003CED9 File Offset: 0x0003B0D9
		private void OnDisable()
		{
			this._action.OnBindingsChanged -= this.UpdateKeyImageAndBindingSource;
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0003CEF2 File Offset: 0x0003B0F2
		private void Update()
		{
			this.UpdateKeyImageAndBindingSource();
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0003CEFC File Offset: 0x0003B0FC
		public void UpdateKeyImageAndBindingSource()
		{
			foreach (BindingSource bindingSource in this._action.Bindings)
			{
				if (KeyMap.SimplifyBindingSourceType(bindingSource.BindingSourceType) == KeyMapper.Map.SimplifiedLastInputType)
				{
					this._bindingSource = bindingSource;
					break;
				}
			}
			Sprite sprite;
			if (CommonResource.instance.TryGetKeyIcon(this._bindingSource, out sprite, true))
			{
				this._image.sprite = sprite;
				this._image.SetNativeSize();
			}
		}

		// Token: 0x040010E7 RID: 4327
		[SerializeField]
		private Button _button;

		// Token: 0x040010E8 RID: 4328
		[SerializeField]
		private Image _image;

		// Token: 0x040010E9 RID: 4329
		private PlayerAction _action;

		// Token: 0x040010EA RID: 4330
		private BindingSource _bindingSource;
	}
}
