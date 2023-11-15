using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Controllers;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Upgrades;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using FX;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UserInput;

namespace UI.Inventory
{
	// Token: 0x02000439 RID: 1081
	public class Panel : Dialogue
	{
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool closeWithPauseKey
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0003F75C File Offset: 0x0003D95C
		private void Awake()
		{
			EventSystem.current.sendNavigationEvents = true;
			Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.AddComponent<PlaySoundOnSelected>().soundInfo = this._selectSound;
			}
			this._gearDiscardButton.onPressed += delegate()
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(GlobalSoundSettings.instance.gearDestroying, base.transform.position);
				Action discardGear = this._discardGear;
				if (discardGear == null)
				{
					return;
				}
				discardGear();
			};
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0003F7BC File Offset: 0x0003D9BC
		protected override void OnEnable()
		{
			base.OnEnable();
			if (GameData.HardmodeProgress.hardmode)
			{
				this._scroll.sprite = this._hardmodeScrollSprite;
				this._harmodeObjects.gameObject.SetActive(true);
				this._items[6].navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnDown = this._upgrades[0],
					selectOnUp = this._items[3],
					selectOnLeft = this._items[8],
					selectOnRight = this._items[7]
				};
				this._items[7].navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnDown = this._upgrades[1],
					selectOnUp = this._items[4],
					selectOnLeft = this._items[6],
					selectOnRight = this._items[8]
				};
				this._items[8].navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnDown = this._upgrades[2],
					selectOnUp = this._items[5],
					selectOnLeft = this._items[7],
					selectOnRight = this._items[6]
				};
				this._weapons[0].navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnDown = this._quintessences[0],
					selectOnUp = this._upgrades[0],
					selectOnLeft = this._weapons[1],
					selectOnRight = this._weapons[1]
				};
				this._weapons[1].navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnDown = this._quintessences[0],
					selectOnUp = this._upgrades[1],
					selectOnLeft = this._weapons[0],
					selectOnRight = this._weapons[0]
				};
			}
			else
			{
				this._scroll.sprite = this._normalmodeScrollSprite;
				this._harmodeObjects.gameObject.SetActive(false);
				this._items[6].navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnDown = this._weapons[0],
					selectOnUp = this._items[3],
					selectOnLeft = this._items[8],
					selectOnRight = this._items[7]
				};
				this._items[7].navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnDown = this._weapons[0],
					selectOnUp = this._items[4],
					selectOnLeft = this._items[6],
					selectOnRight = this._items[8]
				};
				this._items[8].navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnDown = this._weapons[1],
					selectOnUp = this._items[5],
					selectOnLeft = this._items[7],
					selectOnRight = this._items[6]
				};
				this._weapons[0].navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnDown = this._quintessences[0],
					selectOnUp = this._items[6],
					selectOnLeft = this._weapons[1],
					selectOnRight = this._weapons[1]
				};
				this._weapons[1].navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit,
					selectOnDown = this._quintessences[0],
					selectOnUp = this._items[7],
					selectOnLeft = this._weapons[0],
					selectOnRight = this._weapons[0]
				};
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._openSound, Vector3.zero);
			PlayerInput.blocked.Attach(this);
			Chronometer.global.AttachTimeScale(this, 0f);
			WeaponInventory weapon = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
			this.UpdateGearInfo();
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0003FBFC File Offset: 0x0003DDFC
		private void ClearOption()
		{
			this._gearOption.Clear();
			this._discardGear = null;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0003FC10 File Offset: 0x0003DE10
		private void UpdateGearInfo()
		{
			this._keywordDisplay.UpdateElements();
			Character player = Singleton<Service>.Instance.levelManager.player;
			WeaponInventory weapon2 = player.playerComponents.inventory.weapon;
			QuintessenceInventory quintessenceInventory = player.playerComponents.inventory.quintessence;
			UpgradeInventory upgrade2 = player.playerComponents.inventory.upgrade;
			ItemInventory itemInventory = player.playerComponents.inventory.item;
			for (int i = 0; i < this._weapons.Length; i++)
			{
				Weapon weapon;
				if (i == 0)
				{
					weapon = weapon2.polymorphOrCurrent;
				}
				else
				{
					weapon = weapon2.next;
				}
				if (weapon != null)
				{
					Sprite icon = weapon.icon;
					this._weapons[i].SetIcon(icon);
					Action <>9__3;
					this._weapons[i].onSelected = delegate()
					{
						this._gearOption.gameObject.SetActive(true);
						this._gearOption.Set(weapon);
						this._discardGear = null;
						Panel <>4__this = this;
						Action swapSkill;
						if ((swapSkill = <>9__3) == null)
						{
							swapSkill = (<>9__3 = delegate()
							{
								weapon.SwapSkillOrder();
								this._gearOption.Set(weapon);
							});
						}
						<>4__this._swapSkill = swapSkill;
					};
				}
				else
				{
					this._weapons[i].Deactivate();
					this._weapons[i].onSelected = new Action(this.ClearOption);
				}
			}
			for (int j = 0; j < this._quintessences.Length; j++)
			{
				Quintessence quintessence = quintessenceInventory.items[j];
				if (quintessence != null)
				{
					Sprite icon2 = quintessence.icon;
					this._quintessences[j].SetIcon(icon2);
					int cachedIndex = j;
					Action <>9__5;
					this._quintessences[j].onSelected = delegate()
					{
						this._gearOption.gameObject.SetActive(true);
						this._gearOption.Set(quintessence);
						Panel <>4__this = this;
						Action discardGear;
						if ((discardGear = <>9__5) == null)
						{
							discardGear = (<>9__5 = delegate()
							{
								if (quintessenceInventory.Discard(cachedIndex))
								{
									this._quintessences[cachedIndex].Deactivate();
									this.UpdateGearInfo();
									this._quintessences[cachedIndex].onSelected();
								}
							});
						}
						<>4__this._discardGear = discardGear;
						this._swapSkill = null;
					};
				}
				else
				{
					this._quintessences[j].Deactivate();
					this._quintessences[j].onSelected = new Action(this.ClearOption);
				}
			}
			string[] itemKeys = (from item in itemInventory.items
			where item != null
			select item.name).ToArray<string>();
			Func<string, bool> <>9__6;
			for (int k = 0; k < this._items.Length; k++)
			{
				Item item = itemInventory.items[k];
				GearElement gearElement = this._items[k];
				if (item != null)
				{
					Sprite icon3 = item.icon;
					gearElement.SetIcon(icon3);
					IEnumerable<string> setItemKeys = item.setItemKeys;
					Func<string, bool> predicate;
					if ((predicate = <>9__6) == null)
					{
						predicate = (<>9__6 = ((string setKey) => itemKeys.Contains(setKey, StringComparer.OrdinalIgnoreCase)));
					}
					if (setItemKeys.Any(predicate))
					{
						if (item.setItemImage != null)
						{
							gearElement.SetSetImage(item.setItemImage);
						}
						if (item.setItemAnimator != null)
						{
							gearElement.SetSetAnimator(item.setItemAnimator);
						}
					}
					else
					{
						gearElement.DisableSetEffect();
					}
					int cachedIndex = k;
					Action <>9__8;
					gearElement.onSelected = delegate()
					{
						this._gearOption.gameObject.SetActive(true);
						this._gearOption.Set(item);
						Panel <>4__this = this;
						Action discardGear;
						if ((discardGear = <>9__8) == null)
						{
							discardGear = (<>9__8 = delegate()
							{
								if (itemInventory.Discard(cachedIndex))
								{
									itemInventory.Trim();
									this._items[cachedIndex].Deactivate();
									this.UpdateGearInfo();
									this._items[cachedIndex].onSelected();
								}
							});
						}
						<>4__this._discardGear = discardGear;
						this._swapSkill = null;
					};
				}
				else
				{
					gearElement.Deactivate();
					gearElement.onSelected = new Action(this.ClearOption);
				}
			}
			for (int l = 0; l < this._upgrades.Length; l++)
			{
				UpgradeObject upgrade = upgrade2.upgrades[l];
				if (upgrade != null)
				{
					Sprite icon4 = upgrade.icon;
					this._upgrades[l].SetIcon(icon4);
					this._upgrades[l].onSelected = delegate()
					{
						this._gearOption.gameObject.SetActive(true);
						this._gearOption.Set(upgrade);
						this._discardGear = null;
						this._swapSkill = null;
					};
				}
				else
				{
					this._upgrades[l].Deactivate();
					this._upgrades[l].onSelected = new Action(this.ClearOption);
				}
			}
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0004007C File Offset: 0x0003E27C
		protected override void OnDisable()
		{
			base.OnDisable();
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._closeSound, Vector3.zero);
			PlayerInput.blocked.Detach(this);
			Chronometer.global.DetachTimeScale(this);
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x000400B4 File Offset: 0x0003E2B4
		protected override void Update()
		{
			base.Update();
			if (!base.focused)
			{
				return;
			}
			if (KeyMapper.Map.Inventory.WasPressed)
			{
				base.Close();
				return;
			}
			if (KeyMapper.Map.Cancel.WasPressed)
			{
				base.Close();
				return;
			}
			if (KeyMapper.Map.UiInteraction1.WasPressed && this._discardGear == null)
			{
				Action swapSkill = this._swapSkill;
				if (swapSkill != null)
				{
					swapSkill();
				}
			}
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject == null)
			{
				return;
			}
			this._focus.rectTransform.position = currentSelectedGameObject.transform.position;
		}

		// Token: 0x040011A0 RID: 4512
		[Header("하드모드")]
		[SerializeField]
		private Sprite _normalmodeScrollSprite;

		// Token: 0x040011A1 RID: 4513
		[SerializeField]
		private Sprite _hardmodeScrollSprite;

		// Token: 0x040011A2 RID: 4514
		[SerializeField]
		private Image _scroll;

		// Token: 0x040011A3 RID: 4515
		[SerializeField]
		private GameObject _harmodeObjects;

		// Token: 0x040011A4 RID: 4516
		[Space]
		[SerializeField]
		private GearOption _gearOption;

		// Token: 0x040011A5 RID: 4517
		[SerializeField]
		private Image _focus;

		// Token: 0x040011A6 RID: 4518
		[Space]
		[SerializeField]
		private KeywordDisplay _keywordDisplay;

		// Token: 0x040011A7 RID: 4519
		[Space]
		[FormerlySerializedAs("_itemDiscardButton")]
		[SerializeField]
		private PressingButton _gearDiscardButton;

		// Token: 0x040011A8 RID: 4520
		[Space]
		[SerializeField]
		private GearElement[] _weapons;

		// Token: 0x040011A9 RID: 4521
		[SerializeField]
		private GearElement[] _quintessences;

		// Token: 0x040011AA RID: 4522
		[SerializeField]
		private GearElement[] _items;

		// Token: 0x040011AB RID: 4523
		[SerializeField]
		private GearElement[] _upgrades;

		// Token: 0x040011AC RID: 4524
		[SerializeField]
		[Space]
		private SoundInfo _openSound;

		// Token: 0x040011AD RID: 4525
		[SerializeField]
		private SoundInfo _closeSound;

		// Token: 0x040011AE RID: 4526
		[SerializeField]
		private SoundInfo _selectSound;

		// Token: 0x040011AF RID: 4527
		private Action _swapSkill;

		// Token: 0x040011B0 RID: 4528
		private Action _discardGear;
	}
}
