using System;
using System.Runtime.CompilerServices;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Gear.Upgrades;
using Characters.Gear.Weapons;
using Data;
using Hardmode;
using Hardmode.Darktech;
using Singletons;

namespace Characters.Player
{
	// Token: 0x020007ED RID: 2029
	public class Inventory
	{
		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06002912 RID: 10514 RVA: 0x0007CE20 File Offset: 0x0007B020
		// (remove) Token: 0x06002913 RID: 10515 RVA: 0x0007CE58 File Offset: 0x0007B058
		public event Action onUpdated;

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06002914 RID: 10516 RVA: 0x0007CE90 File Offset: 0x0007B090
		// (remove) Token: 0x06002915 RID: 10517 RVA: 0x0007CEC8 File Offset: 0x0007B0C8
		public event Action onUpdatedKeywordCounts;

		// Token: 0x06002916 RID: 10518 RVA: 0x0007CF00 File Offset: 0x0007B100
		public Inventory(Character character)
		{
			this._character = character;
			this.synergy = character.GetComponent<Synergy>();
			this.weapon = character.GetComponent<WeaponInventory>();
			this.item = character.GetComponent<ItemInventory>();
			this.quintessence = character.GetComponent<QuintessenceInventory>();
			this.upgrade = character.GetComponent<UpgradeInventory>();
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x0007CF58 File Offset: 0x0007B158
		public void Initialize()
		{
			this.synergy.Initialize(this._character);
			this.onUpdated += this.UpdateSynergy;
			this.weapon.onChanged += delegate(Weapon old, Weapon @new)
			{
				this.onUpdated();
			};
			this.item.onChanged += delegate()
			{
				this.onUpdated();
			};
			this.quintessence.onChanged += delegate()
			{
				this.onUpdated();
			};
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x0007CFD0 File Offset: 0x0007B1D0
		public void UpdateSynergy()
		{
			foreach (Inscription inscription in this.synergy.inscriptions)
			{
				inscription.count = 0;
			}
			foreach (Item item in this.item.items)
			{
				if (!(item == null))
				{
					this.synergy.inscriptions[item.keyword1].count++;
					this.synergy.inscriptions[item.keyword2].count++;
				}
			}
			foreach (Item item2 in this.item.items)
			{
				if (!(item2 == null))
				{
					foreach (Item.BonusKeyword bonusKeyword2 in item2.bonusKeyword)
					{
						bonusKeyword2.Evaluate();
						EnumArray<Inscription.Key, int> values = bonusKeyword2.Values;
						foreach (Inscription.Key key in Inscription.keys)
						{
							this.synergy.inscriptions[key].count += values[key];
						}
					}
				}
			}
			foreach (Inscription.Key key2 in Inscription.keys)
			{
				this.synergy.inscriptions[key2].count += this.synergy.inscriptions[key2].bonusCount;
			}
			Action action = this.onUpdatedKeywordCounts;
			if (action != null)
			{
				action();
			}
			this.synergy.UpdateBonus();
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x0007D21C File Offset: 0x0007B41C
		public void Save()
		{
			GameData.Save instance = GameData.Save.instance;
			instance.currentWeapon = this.weapon.current.name;
			instance.currentWeaponSkill1 = this.weapon.current.GetSkillWithoutSkillChanges(0).key;
			if (this.weapon.current.currentSkills.Count <= 1)
			{
				instance.currentWeaponSkill2 = string.Empty;
			}
			else
			{
				instance.currentWeaponSkill2 = this.weapon.current.GetSkillWithoutSkillChanges(1).key;
			}
			IStackable componentInChildren = this.weapon.current.GetComponentInChildren<IStackable>();
			if (componentInChildren == null)
			{
				instance.currentWeaponStack = 0f;
			}
			else
			{
				instance.currentWeaponStack = componentInChildren.stack;
			}
			if (this.weapon.next == null)
			{
				instance.nextWeapon = string.Empty;
				instance.nextWeaponSkill1 = string.Empty;
				instance.nextWeaponSkill1 = string.Empty;
			}
			else
			{
				this.weapon.next.UnapplyAllSkillChanges();
				instance.nextWeapon = this.weapon.next.name;
				instance.nextWeaponSkill1 = this.weapon.next.currentSkills[0].key;
				if (this.weapon.next.currentSkills.Count <= 1)
				{
					instance.nextWeaponSkill2 = string.Empty;
				}
				else
				{
					instance.nextWeaponSkill2 = this.weapon.next.currentSkills[1].key;
				}
				this.weapon.next.ApplyAllSkillChanges();
				IStackable componentInChildren2 = this.weapon.next.GetComponentInChildren<IStackable>();
				if (componentInChildren2 == null)
				{
					instance.nextWeaponStack = 0f;
				}
				else
				{
					instance.nextWeaponStack = componentInChildren2.stack;
				}
			}
			if (this.quintessence.items[0] == null)
			{
				instance.essence = string.Empty;
			}
			else
			{
				Quintessence quintessence = this.quintessence.items[0];
				instance.essence = quintessence.name;
				IStackable componentInChildren3 = quintessence.GetComponentInChildren<IStackable>();
				if (componentInChildren3 == null)
				{
					instance.essenceStack = 0f;
				}
				else
				{
					instance.essenceStack = componentInChildren3.stack;
				}
			}
			for (int i = 0; i < instance.items.length; i++)
			{
				Item item = this.item.items[i];
				if (item == null)
				{
					instance.items[i] = string.Empty;
					instance.itemStacks[i] = 0f;
					instance.itemKeywords1[i] = 0;
					instance.itemKeywords2[i] = 0;
				}
				else
				{
					instance.items[i] = item.name;
					IStackable componentInChildren4 = item.GetComponentInChildren<IStackable>();
					if (componentInChildren4 == null)
					{
						instance.itemStacks[i] = 0f;
					}
					else
					{
						instance.itemStacks[i] = componentInChildren4.stack;
					}
					instance.itemKeywords1[i] = (int)item.keyword1;
					instance.itemKeywords2[i] = (int)item.keyword2;
				}
			}
			if (!GameData.HardmodeProgress.hardmode)
			{
				return;
			}
			for (int j = 0; j < instance.upgrades.length; j++)
			{
				UpgradeObject upgradeObject = this.upgrade.upgrades[j];
				if (upgradeObject == null)
				{
					instance.upgrades[j] = string.Empty;
					instance.upgradeLevels[j] = 0;
					instance.upgradeStacks[j] = 0f;
				}
				else
				{
					instance.upgrades[j] = upgradeObject.name;
					instance.upgradeLevels[j] = upgradeObject.level;
					IStackable componentInChildren5 = upgradeObject.GetOrigin().GetCurrentAbility().GetComponentInChildren<IStackable>();
					if (componentInChildren5 == null)
					{
						instance.upgradeStacks[j] = 0f;
					}
					else
					{
						instance.upgradeStacks[j] = componentInChildren5.stack;
					}
				}
			}
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x0007D610 File Offset: 0x0007B810
		public void LoadFromSave()
		{
			GameData.Save instance = GameData.Save.instance;
			GameResourceLoader instance2 = GameResourceLoader.instance;
			this.LoadSavedWeaponFromPreloader(instance, instance2);
			this.LoadSavedEssenceFromPreloader(instance, instance2);
			this.LoadSavedItemsFromPreloader(instance, instance2);
			if (!Singleton<HardmodeManager>.Instance.hardmode)
			{
				return;
			}
			this.LoadSavedUpgradeFromPreloader(instance);
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x0007D658 File Offset: 0x0007B858
		private void LoadSavedUpgradeFromPreloader(GameData.Save save)
		{
			for (int i = 0; i < GameData.HardmodeProgress.InscriptionSynthesisEquipment.count; i++)
			{
				int value = GameData.HardmodeProgress.inscriptionSynthesisEquipment[i].value;
				if (value != -1)
				{
					this.synergy.inscriptions[(Inscription.Key)value].bonusCount += InscriptionSynthesisEquipment.increasement;
				}
			}
			this.UpdateSynergy();
			for (int j = 0; j < save.upgrades.length; j++)
			{
				string text = save.upgrades[j];
				if (!string.IsNullOrEmpty(text))
				{
					UpgradeResource.Reference reference = Singleton<UpgradeManager>.Instance.FindByName(text);
					this.upgrade.EquipAt(reference, j);
					int level = save.upgradeLevels[j];
					UpgradeObject upgradeObject = this.upgrade.upgrades[j];
					upgradeObject.SetLevel(level);
					UpgradeAbility currentAbility = upgradeObject.GetCurrentAbility();
					if (currentAbility != null)
					{
						IStackable componentInChildren = currentAbility.GetComponentInChildren<IStackable>();
						if (componentInChildren != null)
						{
							componentInChildren.stack = save.upgradeStacks[j];
						}
					}
				}
			}
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x0007D758 File Offset: 0x0007B958
		private void LoadSavedWeaponFromPreloader(GameData.Save save, GameResourceLoader preloader)
		{
			this.weapon.LoseAll();
			Weapon weaponInstance = preloader.TakeWeapon2();
			int index = 1;
			string text = save.nextWeaponSkill1;
			string text2 = save.nextWeaponSkill2;
			this.<LoadSavedWeaponFromPreloader>g__LoadWeapon|18_0(weaponInstance, index, text, text2, save.nextWeaponStack);
			Weapon weaponInstance2 = preloader.TakeWeapon1();
			int index2 = 0;
			text = save.currentWeaponSkill1;
			text2 = save.currentWeaponSkill2;
			this.<LoadSavedWeaponFromPreloader>g__LoadWeapon|18_0(weaponInstance2, index2, text, text2, save.currentWeaponStack);
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x0007D7BC File Offset: 0x0007B9BC
		private void LoadSavedEssenceFromPreloader(GameData.Save save, GameResourceLoader preloader)
		{
			Quintessence quintessence = preloader.TakeEssence();
			if (quintessence != null)
			{
				this.quintessence.EquipAt(quintessence, 0);
				if (save.essenceStack > 0f)
				{
					IStackable componentInChildren = quintessence.GetComponentInChildren<IStackable>();
					if (componentInChildren != null)
					{
						componentInChildren.stack = save.essenceStack;
					}
				}
			}
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x0007D80C File Offset: 0x0007BA0C
		private void LoadSavedItemsFromPreloader(GameData.Save save, GameResourceLoader preloader)
		{
			for (int i = 0; i < save.items.length; i++)
			{
				Item item = preloader.TakeItem(i);
				if (!(item == null))
				{
					Inscription.Key key = (Inscription.Key)save.itemKeywords1[i];
					Inscription.Key key2 = (Inscription.Key)save.itemKeywords2[i];
					if (key != Inscription.Key.None)
					{
						item.keyword1 = key;
					}
					if (key2 != Inscription.Key.None)
					{
						item.keyword2 = key2;
					}
					this.item.EquipAt(item, i);
					if (save.itemStacks[i] > 0f)
					{
						IStackable componentInChildren = item.GetComponentInChildren<IStackable>();
						if (componentInChildren != null)
						{
							componentInChildren.stack = save.itemStacks[i];
						}
					}
				}
			}
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x0007D8C0 File Offset: 0x0007BAC0
		[CompilerGenerated]
		private void <LoadSavedWeaponFromPreloader>g__LoadWeapon|18_0(Weapon weaponInstance, int index, in string skill1, in string skill2, float stack)
		{
			if (weaponInstance == null)
			{
				return;
			}
			this.weapon.ForceEquipAt(weaponInstance, index);
			if (string.IsNullOrEmpty(skill2))
			{
				weaponInstance.SetSkills(new string[]
				{
					skill1
				}, false);
			}
			else
			{
				weaponInstance.SetSkills(new string[]
				{
					skill1,
					skill2
				}, false);
			}
			if (stack > 0f)
			{
				IStackable componentInChildren = weaponInstance.GetComponentInChildren<IStackable>();
				if (componentInChildren != null)
				{
					componentInChildren.stack = stack;
				}
			}
		}

		// Token: 0x04002374 RID: 9076
		public readonly Synergy synergy;

		// Token: 0x04002375 RID: 9077
		public readonly WeaponInventory weapon;

		// Token: 0x04002376 RID: 9078
		public readonly ItemInventory item;

		// Token: 0x04002377 RID: 9079
		public readonly QuintessenceInventory quintessence;

		// Token: 0x04002378 RID: 9080
		public readonly UpgradeInventory upgrade;

		// Token: 0x04002379 RID: 9081
		private Character _character;
	}
}
