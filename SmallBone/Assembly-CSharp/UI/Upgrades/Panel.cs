using System;
using Data;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInput;

namespace UI.Upgrades
{
	// Token: 0x020003EE RID: 1006
	public sealed class Panel : Dialogue
	{
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x000388F5 File Offset: 0x00036AF5
		public UpgradableContainer upgradableContainer
		{
			get
			{
				return this._upgradableList;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool closeWithPauseKey
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x000388FD File Offset: 0x00036AFD
		private void Awake()
		{
			this._upgradableList.Initialize(this);
			this._current.Initialize(this);
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00038918 File Offset: 0x00036B18
		protected override void OnEnable()
		{
			base.OnEnable();
			this.UpdateAll();
			Selectable selectable = this._upgradableList.GetDefaultFocusTarget().selectable;
			EventSystem.current.SetSelectedGameObject(selectable.gameObject);
			selectable.Select();
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00038958 File Offset: 0x00036B58
		protected override void OnDisable()
		{
			base.OnDisable();
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00038970 File Offset: 0x00036B70
		protected override void Update()
		{
			base.Update();
			if (this._currencyCache != GameData.Currency.heartQuartz.balance)
			{
				this._currency.text = GameData.Currency.heartQuartz.balance.ToString();
				this._currencyCache = GameData.Currency.heartQuartz.balance;
			}
			if (this._goldCache != GameData.Currency.gold.balance)
			{
				this._gold.text = GameData.Currency.gold.balance.ToString();
				this._goldCache = GameData.Currency.gold.balance;
			}
			if (KeyMapper.Map.Cancel.WasPressed && Dialogue.GetCurrent() == this)
			{
				base.Close();
				return;
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00038A26 File Offset: 0x00036C26
		private void UpdateAll()
		{
			this.UpdateUpgradedList();
			this._upgradableList.UpdateElements();
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00038A39 File Offset: 0x00036C39
		public void UpdateUpgradedList()
		{
			this._upgradedList.Set(this);
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x00038A47 File Offset: 0x00036C47
		public void AppendToUpgradedList()
		{
			this._upgradedList.Append(this);
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x00038A55 File Offset: 0x00036C55
		public void UpdateCurrentOption(UpgradeElement element)
		{
			this._current.Set(element);
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00038A63 File Offset: 0x00036C63
		public void UpdateCurrentOption(UpgradedElement element)
		{
			this._current.Set(element);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00038A74 File Offset: 0x00036C74
		public void SetFocusOnRemoved(int removedIndex)
		{
			UpgradedElement focusElementOnRemoved = this._upgradedList.GetFocusElementOnRemoved(removedIndex);
			if (focusElementOnRemoved == null)
			{
				this.SetFocusToDefault();
			}
			else
			{
				base.Focus(focusElementOnRemoved.selectable);
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this.upgradableContainer.clearSoundInfo, base.gameObject.transform.position);
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00038AD1 File Offset: 0x00036CD1
		public void SetFocusToDefault()
		{
			this._defaultFocus = this._upgradableList.GetDefaultFocusTarget().selectable;
			base.Focus();
		}

		// Token: 0x04000FE2 RID: 4066
		[SerializeField]
		private UpgradedContainer _upgradedList;

		// Token: 0x04000FE3 RID: 4067
		[SerializeField]
		private UpgradableContainer _upgradableList;

		// Token: 0x04000FE4 RID: 4068
		[SerializeField]
		private Option _current;

		// Token: 0x04000FE5 RID: 4069
		[Header("Left Bottom")]
		[SerializeField]
		private TMP_Text _currency;

		// Token: 0x04000FE6 RID: 4070
		[SerializeField]
		private TMP_Text _gold;

		// Token: 0x04000FE7 RID: 4071
		private int _currencyCache;

		// Token: 0x04000FE8 RID: 4072
		private int _goldCache;
	}
}
