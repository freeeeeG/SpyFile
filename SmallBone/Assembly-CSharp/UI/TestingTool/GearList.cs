using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Gear;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Gear.Upgrades;
using Characters.Player;
using GameResources;
using InControl;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TestingTool
{
	// Token: 0x020003FD RID: 1021
	public class GearList : MonoBehaviour
	{
		// Token: 0x06001346 RID: 4934 RVA: 0x00039FC0 File Offset: 0x000381C0
		private void Awake()
		{
			this.selected = new List<ItemReference>();
			foreach (WeaponReference gearReference in GearResource.instance.weapons)
			{
				GearListElement gearListElement = UnityEngine.Object.Instantiate<GearListElement>(this._gearListElementPrefab, this._gridContainer);
				gearListElement.Set(gearReference);
				this._gearListElements.Add(gearListElement);
			}
			foreach (ItemReference gearReference2 in GearResource.instance.items)
			{
				GearListElement gearListElement2 = UnityEngine.Object.Instantiate<GearListElement>(this._gearListElementPrefab, this._gridContainer);
				gearListElement2.Set(gearReference2);
				this._gearListElements.Add(gearListElement2);
			}
			foreach (EssenceReference gearReference3 in GearResource.instance.essences)
			{
				GearListElement gearListElement3 = UnityEngine.Object.Instantiate<GearListElement>(this._gearListElementPrefab, this._gridContainer);
				gearListElement3.Set(gearReference3);
				this._gearListElements.Add(gearListElement3);
			}
			this._head.onClick.AddListener(delegate
			{
				this.SetFilter(GearList.Filter.Weapon);
			});
			this._item.onClick.AddListener(delegate
			{
				this.SetFilter(GearList.Filter.Item);
			});
			this._essence.onClick.AddListener(delegate
			{
				this.SetFilter(GearList.Filter.Essence);
			});
			this._upgrade.onClick.AddListener(delegate
			{
				this.SetFilter(GearList.Filter.Upgrade);
			});
			Character player = Singleton<Service>.Instance.levelManager.player;
			using (List<UpgradeResource.Reference>.Enumerator enumerator4 = UpgradeResource.instance.upgradeReferences.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					UpgradeResource.Reference upgrade = enumerator4.Current;
					Button button = UnityEngine.Object.Instantiate<Button>(this._upgradeListElement, this._gridContainer);
					Text componentInChildren = button.GetComponentInChildren<Text>();
					button.onClick.AddListener(delegate
					{
						UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
						if (!upgrade.Has(upgrade))
						{
							if (!upgrade.TryEquip(upgrade))
							{
								Debug.LogError("획득 실패");
								return;
							}
						}
						else
						{
							int index = upgrade.IndexOf(upgrade);
							upgrade.upgrades[index].LevelUpOrigin();
						}
					});
					if (componentInChildren != null)
					{
						componentInChildren.text = upgrade.displayName;
					}
					this._upgradeListElements.Add(button);
				}
			}
			using (IEnumerator enumerator5 = Enum.GetValues(typeof(Inscription.Key)).GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					Inscription.Key inscription = (Inscription.Key)enumerator5.Current;
					Button button2 = UnityEngine.Object.Instantiate<Button>(this._inscriptionElementPrefab, this._gridContainer);
					Text componentInChildren2 = button2.GetComponentInChildren<Text>();
					button2.onClick.AddListener(delegate
					{
						this.SetInscription(inscription);
					});
					if (componentInChildren2 != null)
					{
						componentInChildren2.text = inscription.ToString();
					}
					this._inscriptionListElements.Add(button2);
				}
			}
			this._inputField.onValueChanged.AddListener(delegate(string _)
			{
				this.FilterGearList(null);
			});
			this.SetFilter(GearList.Filter.Weapon);
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0003A328 File Offset: 0x00038528
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			if (!activeDevice.LeftBumper.WasPressed && !activeDevice.LeftTrigger.WasPressed)
			{
				if (activeDevice.RightBumper.WasPressed || activeDevice.RightTrigger.WasPressed)
				{
					if (this._currentFilter == EnumValues<GearList.Filter>.Values.Last<GearList.Filter>())
					{
						this.SetFilter(EnumValues<GearList.Filter>.Values.First<GearList.Filter>());
						return;
					}
					this.SetFilter(this._currentFilter + 1);
				}
				return;
			}
			if (this._currentFilter == GearList.Filter.Weapon)
			{
				this.SetFilter(EnumValues<GearList.Filter>.Values.Last<GearList.Filter>());
				return;
			}
			this.SetFilter(this._currentFilter - 1);
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0003A3C8 File Offset: 0x000385C8
		private void SetFilter(GearList.Filter filter)
		{
			this._currentFilter = filter;
			switch (filter)
			{
			case GearList.Filter.Weapon:
				this.FilterGearList(new Gear.Type?(Gear.Type.Weapon));
				return;
			case GearList.Filter.Item:
				this.FilterGearList(new Gear.Type?(Gear.Type.Item));
				return;
			case GearList.Filter.Essence:
				this.FilterGearList(new Gear.Type?(Gear.Type.Quintessence));
				return;
			case GearList.Filter.Upgrade:
				this.FilterUpgrade();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0003A420 File Offset: 0x00038620
		private void FilterGearList(Gear.Type? type = null)
		{
			string value = this._inputField.text.Trim().ToUpperInvariant();
			foreach (Button button in this._upgradeListElements)
			{
				button.gameObject.SetActive(false);
			}
			foreach (Button button2 in this._inscriptionListElements)
			{
				GameObject gameObject = button2.gameObject;
				Gear.Type? type2 = type;
				Gear.Type type3 = Gear.Type.Item;
				gameObject.SetActive(type2.GetValueOrDefault() == type3 & type2 != null);
			}
			foreach (GearListElement gearListElement in this._gearListElements)
			{
				bool flag = string.IsNullOrEmpty(value) || gearListElement.text.ToUpperInvariant().Contains(value) || gearListElement.gearReference.name.ToUpperInvariant().Contains(value);
				if (type != null)
				{
					bool flag2 = flag;
					Gear.Type type4 = gearListElement.type;
					Gear.Type? type2 = type;
					flag = (flag2 & (type4 == type2.GetValueOrDefault() & type2 != null));
				}
				gearListElement.gameObject.SetActive(flag);
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0003A594 File Offset: 0x00038794
		private void FilterUpgrade()
		{
			string value = this._inputField.text.Trim().ToUpperInvariant();
			foreach (Button button in this._inscriptionListElements)
			{
				button.gameObject.SetActive(false);
			}
			foreach (GearListElement gearListElement in this._gearListElements)
			{
				gearListElement.gameObject.SetActive(false);
			}
			foreach (Button button2 in this._upgradeListElements)
			{
				bool active = string.IsNullOrEmpty(value) || button2.name.ToUpperInvariant().Contains(value);
				button2.gameObject.SetActive(active);
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0003A6AC File Offset: 0x000388AC
		private void SetInscription(Inscription.Key keyword)
		{
			this.selected.Clear();
			foreach (ItemReference itemReference in GearResource.instance.items)
			{
				if (itemReference.prefabKeyword1 == keyword || itemReference.prefabKeyword2 == keyword)
				{
					this.selected.Add(itemReference);
				}
			}
			foreach (GearListElement gearListElement in this._gearListElements)
			{
				bool active = false;
				if (gearListElement.gearReference.type == Gear.Type.Item)
				{
					foreach (ItemReference itemReference2 in this.selected)
					{
						if (gearListElement.gearReference.name.Equals(itemReference2.name, StringComparison.OrdinalIgnoreCase))
						{
							active = true;
							break;
						}
					}
				}
				gearListElement.gameObject.SetActive(active);
			}
		}

		// Token: 0x0400102C RID: 4140
		[SerializeField]
		private GearListElement _gearListElementPrefab;

		// Token: 0x0400102D RID: 4141
		[SerializeField]
		private Button _upgradeListElement;

		// Token: 0x0400102E RID: 4142
		[SerializeField]
		private Button _inscriptionElementPrefab;

		// Token: 0x0400102F RID: 4143
		[SerializeField]
		private Button _head;

		// Token: 0x04001030 RID: 4144
		[SerializeField]
		private Button _item;

		// Token: 0x04001031 RID: 4145
		[SerializeField]
		private Button _essence;

		// Token: 0x04001032 RID: 4146
		[SerializeField]
		private Button _upgrade;

		// Token: 0x04001033 RID: 4147
		[SerializeField]
		private TMP_InputField _inputField;

		// Token: 0x04001034 RID: 4148
		[SerializeField]
		private Transform _gridContainer;

		// Token: 0x04001035 RID: 4149
		private GearList.Filter _currentFilter;

		// Token: 0x04001036 RID: 4150
		private readonly List<GearListElement> _gearListElements = new List<GearListElement>();

		// Token: 0x04001037 RID: 4151
		private readonly List<Button> _upgradeListElements = new List<Button>();

		// Token: 0x04001038 RID: 4152
		private readonly List<Button> _inscriptionListElements = new List<Button>();

		// Token: 0x04001039 RID: 4153
		private List<ItemReference> selected;

		// Token: 0x020003FE RID: 1022
		private enum Filter
		{
			// Token: 0x0400103B RID: 4155
			Weapon,
			// Token: 0x0400103C RID: 4156
			Item,
			// Token: 0x0400103D RID: 4157
			Essence,
			// Token: 0x0400103E RID: 4158
			Upgrade
		}
	}
}
