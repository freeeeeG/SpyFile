using System;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Upgrades;
using Characters.Gear.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
	// Token: 0x0200042F RID: 1071
	public class GearOption : MonoBehaviour
	{
		// Token: 0x06001469 RID: 5225 RVA: 0x0003EB78 File Offset: 0x0003CD78
		public void Clear()
		{
			this._thumnailIcon.enabled = false;
			this._name.text = string.Empty;
			this._rarity.text = string.Empty;
			this._itemDiscardKey.gameObject.SetActive(false);
			this._skillSwapKey.SetActive(false);
			this._weaponOption.gameObject.SetActive(false);
			this._itemOption.gameObject.SetActive(false);
			this._essenceOption.gameObject.SetActive(false);
			this._upgradeOption.gameObject.SetActive(false);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0003EC12 File Offset: 0x0003CE12
		public void Set(Weapon weapon)
		{
			this.Clear();
			this._weaponOption.gameObject.SetActive(true);
			this._weaponOption.Set(weapon);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0003EC37 File Offset: 0x0003CE37
		public void Set(Item item)
		{
			this.Clear();
			this._itemOption.gameObject.SetActive(true);
			this._itemOption.Set(item);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0003EC5C File Offset: 0x0003CE5C
		public void Set(Quintessence essence)
		{
			this.Clear();
			this._essenceOption.gameObject.SetActive(true);
			this._essenceOption.Set(essence);
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0003EC81 File Offset: 0x0003CE81
		public void Set(UpgradeObject item)
		{
			this.Clear();
			this._upgradeOption.gameObject.SetActive(true);
			this._upgradeOption.Set(item);
		}

		// Token: 0x04001159 RID: 4441
		[SerializeField]
		private WeaponOption _weaponOption;

		// Token: 0x0400115A RID: 4442
		[SerializeField]
		private ItemOption _itemOption;

		// Token: 0x0400115B RID: 4443
		[SerializeField]
		private QuintessenceOption _essenceOption;

		// Token: 0x0400115C RID: 4444
		[SerializeField]
		private UpgradeOption _upgradeOption;

		// Token: 0x0400115D RID: 4445
		[SerializeField]
		[Space]
		private Image _thumnailIcon;

		// Token: 0x0400115E RID: 4446
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x0400115F RID: 4447
		[SerializeField]
		private TMP_Text _rarity;

		// Token: 0x04001160 RID: 4448
		[SerializeField]
		[Space]
		private PressingButton _itemDiscardKey;

		// Token: 0x04001161 RID: 4449
		[SerializeField]
		private TMP_Text _itemDiscardText;

		// Token: 0x04001162 RID: 4450
		[SerializeField]
		private GameObject _skillSwapKey;
	}
}
