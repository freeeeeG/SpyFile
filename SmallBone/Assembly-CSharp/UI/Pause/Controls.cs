using System;
using Data;
using GameResources;
using InControl;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.Pause
{
	// Token: 0x02000415 RID: 1045
	public class Controls : Dialogue
	{
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x00018EC5 File Offset: 0x000170C5
		public override bool closeWithPauseKey
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0003C80C File Offset: 0x0003AA0C
		private void Awake()
		{
			this._reset.onClick.AddListener(delegate
			{
				KeyMapper.Map.ResetToDefault();
			});
			this._return.onClick.AddListener(delegate
			{
				this._panel.state = Panel.State.Menu;
			});
			this._up.Initialize(KeyMapper.Map.Up, this._pressNewKey);
			this._down.Initialize(KeyMapper.Map.Down, this._pressNewKey);
			this._left.Initialize(KeyMapper.Map.Left, this._pressNewKey);
			this._right.Initialize(KeyMapper.Map.Right, this._pressNewKey);
			this._attack.Initialize(KeyMapper.Map.Attack, this._pressNewKey);
			this._jump.Initialize(KeyMapper.Map.Jump, this._pressNewKey);
			this._dash.Initialize(KeyMapper.Map.Dash, this._pressNewKey);
			this.ArrowDashText();
			this._arrowDash.value = (GameData.Settings.arrowDashEnabled ? 1 : 0);
			this._arrowDash.onValueChanged += delegate(int v)
			{
				GameData.Settings.arrowDashEnabled = (v == 1);
			};
			this._swap.Initialize(KeyMapper.Map.Swap, this._pressNewKey);
			this._skill1.Initialize(KeyMapper.Map.Skill1, this._pressNewKey);
			this._skill2.Initialize(KeyMapper.Map.Skill2, this._pressNewKey);
			this._quintessence.Initialize(KeyMapper.Map.Quintessence, this._pressNewKey);
			this._inventory.Initialize(KeyMapper.Map.Inventory, this._pressNewKey);
			this._interaction.Initialize(KeyMapper.Map.Interaction, this._pressNewKey);
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0003CA0C File Offset: 0x0003AC0C
		protected override void OnEnable()
		{
			base.OnEnable();
			this._up.UpdateKeyImageAndBindingSource();
			this._down.UpdateKeyImageAndBindingSource();
			this._left.UpdateKeyImageAndBindingSource();
			this._right.UpdateKeyImageAndBindingSource();
			this._attack.UpdateKeyImageAndBindingSource();
			this._jump.UpdateKeyImageAndBindingSource();
			this._dash.UpdateKeyImageAndBindingSource();
			this.ArrowDashText();
			this._swap.UpdateKeyImageAndBindingSource();
			this._skill1.UpdateKeyImageAndBindingSource();
			this._skill2.UpdateKeyImageAndBindingSource();
			this._quintessence.UpdateKeyImageAndBindingSource();
			this._inventory.UpdateKeyImageAndBindingSource();
			this._interaction.UpdateKeyImageAndBindingSource();
			KeyMapper.Map.OnSimplifiedLastInputTypeChanged += this.OnSimplifiedLastInputTypeChanged;
			this.OnSimplifiedLastInputTypeChanged(KeyMapper.Map.SimplifiedLastInputType);
			this.SetInitialFocus();
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0003CAE0 File Offset: 0x0003ACE0
		protected override void OnDisable()
		{
			base.OnDisable();
			GameData.Settings.keyBindings = KeyMapper.Map.Save();
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0003CAF8 File Offset: 0x0003ACF8
		private void SetInitialFocus()
		{
			if (KeyMapper.Map.SimplifiedLastInputType == BindingSourceType.KeyBindingSource)
			{
				base.Focus(this._up.button);
				return;
			}
			this._up.gameObject.SetActive(false);
			this._up.gameObject.SetActive(true);
			this._down.gameObject.SetActive(false);
			this._down.gameObject.SetActive(true);
			this._left.gameObject.SetActive(false);
			this._left.gameObject.SetActive(true);
			this._right.gameObject.SetActive(false);
			this._right.gameObject.SetActive(true);
			this._inventory.gameObject.SetActive(false);
			this._inventory.gameObject.SetActive(true);
			base.Focus(this._arrowDash);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0003CBDC File Offset: 0x0003ADDC
		private void OnSimplifiedLastInputTypeChanged(BindingSourceType bindingSourceType)
		{
			if (bindingSourceType == BindingSourceType.KeyBindingSource)
			{
				this._up.button.interactable = true;
				this._down.button.interactable = true;
				this._left.button.interactable = true;
				this._right.button.interactable = true;
				this._inventory.button.interactable = true;
				base.Focus(this._up.button);
				return;
			}
			this._up.button.interactable = false;
			this._down.button.interactable = false;
			this._left.button.interactable = false;
			this._right.button.interactable = false;
			this._inventory.button.interactable = false;
			this._up.gameObject.SetActive(false);
			this._up.gameObject.SetActive(true);
			this._down.gameObject.SetActive(false);
			this._down.gameObject.SetActive(true);
			this._left.gameObject.SetActive(false);
			this._left.gameObject.SetActive(true);
			this._right.gameObject.SetActive(false);
			this._right.gameObject.SetActive(true);
			this._inventory.gameObject.SetActive(false);
			this._inventory.gameObject.SetActive(true);
			base.Focus(this._arrowDash);
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0003CD5F File Offset: 0x0003AF5F
		private void ArrowDashText()
		{
			this._arrowDash.SetTexts(Localization.GetLocalizedStrings(new string[]
			{
				"label/pause/settings/off",
				"label/pause/settings/on"
			}));
		}

		// Token: 0x040010CF RID: 4303
		[SerializeField]
		private Panel _panel;

		// Token: 0x040010D0 RID: 4304
		[SerializeField]
		private PressNewKey _pressNewKey;

		// Token: 0x040010D1 RID: 4305
		[SerializeField]
		private KeyBinder _up;

		// Token: 0x040010D2 RID: 4306
		[SerializeField]
		private KeyBinder _down;

		// Token: 0x040010D3 RID: 4307
		[SerializeField]
		private KeyBinder _left;

		// Token: 0x040010D4 RID: 4308
		[SerializeField]
		private KeyBinder _right;

		// Token: 0x040010D5 RID: 4309
		[SerializeField]
		private KeyBinder _attack;

		// Token: 0x040010D6 RID: 4310
		[SerializeField]
		private KeyBinder _jump;

		// Token: 0x040010D7 RID: 4311
		[SerializeField]
		private KeyBinder _dash;

		// Token: 0x040010D8 RID: 4312
		[SerializeField]
		private Toggle _arrowDash;

		// Token: 0x040010D9 RID: 4313
		[SerializeField]
		private KeyBinder _swap;

		// Token: 0x040010DA RID: 4314
		[SerializeField]
		private KeyBinder _skill1;

		// Token: 0x040010DB RID: 4315
		[SerializeField]
		private KeyBinder _skill2;

		// Token: 0x040010DC RID: 4316
		[SerializeField]
		private KeyBinder _quintessence;

		// Token: 0x040010DD RID: 4317
		[SerializeField]
		private KeyBinder _inventory;

		// Token: 0x040010DE RID: 4318
		[SerializeField]
		private KeyBinder _interaction;

		// Token: 0x040010DF RID: 4319
		[SerializeField]
		private Button _reset;

		// Token: 0x040010E0 RID: 4320
		[SerializeField]
		private Button _return;
	}
}
