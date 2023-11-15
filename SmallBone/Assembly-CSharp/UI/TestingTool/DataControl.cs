using System;
using System.Collections;
using Characters.Gear;
using Characters.Gear.Upgrades;
using CutScenes;
using Data;
using GameResources;
using Level;
using Level.Npc;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.TestingTool
{
	// Token: 0x020003F8 RID: 1016
	public sealed class DataControl : MonoBehaviour
	{
		// Token: 0x0600132D RID: 4909 RVA: 0x00039898 File Offset: 0x00037A98
		private void Awake()
		{
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			this._resetSeed.onClick.AddListener(delegate
			{
				GameData.Save.instance.ResetRandomSeed();
			});
			this._firstClear.onClick.AddListener(new UnityAction(this.ClearFirst));
			this._resetProgress.onClick.AddListener(delegate
			{
				GameData.Generic.ResetAll();
				GameData.Currency.ResetAll();
				GameData.Progress.ResetAll();
				GameData.Gear.ResetAll();
				GameData.Save.instance.ResetAll();
				GameData.HardmodeProgress.ResetAll();
				levelManager.player.playerComponents.savableAbilityManager.ResetAll();
			});
			this._randomItem.onClick.AddListener(new UnityAction(this.EquipRandomItem));
			this._unlockAllItems.onClick.AddListener(new UnityAction(this.UnlockAllItems));
			this._unlockAllUpgrades.onClick.AddListener(new UnityAction(this.UnlockAllUpgrades));
			this._lockAllUpgrades.onClick.AddListener(new UnityAction(this.LockAllUpgrades));
			this._itemReset.onClick.AddListener(delegate
			{
				levelManager.player.playerComponents.inventory.item.RemoveAll();
			});
			this._upgradeReset.onClick.AddListener(delegate
			{
				levelManager.player.playerComponents.inventory.upgrade.RemoveAll();
			});
			this._allGearReset.onClick.AddListener(delegate
			{
				levelManager.player.playerComponents.inventory.weapon.LoseAll();
				levelManager.player.playerComponents.inventory.quintessence.Remove(0);
				levelManager.player.playerComponents.inventory.item.RemoveAll();
				levelManager.player.playerComponents.inventory.upgrade.RemoveAll();
			});
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x000399E8 File Offset: 0x00037BE8
		private void UnlockAllItems()
		{
			string typeName = Gear.Type.Item.ToString();
			foreach (ItemReference itemReference in GearResource.instance.items)
			{
				if (itemReference.needUnlock)
				{
					GameData.Gear.SetUnlocked(typeName, itemReference.name, true);
				}
			}
			typeName = Gear.Type.Weapon.ToString();
			foreach (WeaponReference weaponReference in GearResource.instance.weapons)
			{
				if (weaponReference.needUnlock)
				{
					GameData.Gear.SetUnlocked(typeName, weaponReference.name, true);
				}
			}
			typeName = Gear.Type.Quintessence.ToString();
			foreach (EssenceReference essenceReference in GearResource.instance.essences)
			{
				if (essenceReference.needUnlock)
				{
					GameData.Gear.SetUnlocked(typeName, essenceReference.name, true);
				}
			}
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00039B24 File Offset: 0x00037D24
		private void UnlockAllUpgrades()
		{
			string typeName = Gear.Type.Upgrade.ToString();
			foreach (UpgradeResource.Reference reference in UpgradeResource.instance.upgradeReferences)
			{
				GameData.Gear.SetUnlocked(typeName, reference.name, true);
			}
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00039B94 File Offset: 0x00037D94
		private void LockAllUpgrades()
		{
			string typeName = Gear.Type.Upgrade.ToString();
			foreach (UpgradeResource.Reference reference in UpgradeResource.instance.upgradeReferences)
			{
				if (reference.needUnlock)
				{
					GameData.Gear.SetUnlocked(typeName, reference.name, false);
					GameData.Gear.SetFounded(typeName, reference.name, false);
				}
			}
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00039C18 File Offset: 0x00037E18
		private void ClearFirst()
		{
			GameData.Generic.tutorial.End();
			GameData.Progress.cutscene.SetData(Key.rookieHero, true);
			GameData.Progress.cutscene.SetData(Key.veterantHero, true);
			GameData.Progress.cutscene.SetData(Key.yggdrasill_Outro, true);
			GameData.Progress.cutscene.SetData(Key.leiana_Intro, true);
			GameData.Progress.cutscene.SetData(Key.leiana_Outro, true);
			GameData.Progress.cutscene.SetData(Key.chimera_Intro, true);
			GameData.Progress.cutscene.SetData(Key.chimera_Outro, true);
			GameData.Progress.cutscene.SetData(Key.pope_Intro, true);
			GameData.Progress.cutscene.SetData(Key.pope2Phase_Intro, true);
			GameData.Progress.cutscene.SetData(Key.pope_Outro, true);
			GameData.Progress.cutscene.SetData(Key.firstHero_Intro, true);
			GameData.Progress.cutscene.SetData(Key.firstHero2Phase_Intro, true);
			GameData.Progress.cutscene.SetData(Key.firstHero3Phase_Intro, true);
			GameData.Progress.cutscene.SetData(Key.firstHero_Outro, true);
			GameData.Progress.cutscene.SetData(Key.ending, true);
			GameData.Progress.cutscene.SetData(Key.masteryTutorial, true);
			GameData.Progress.cutscene.SetData(Key.strangeCat, true);
			GameData.Progress.cutscene.SetData(Key.arachne, true);
			GameData.Progress.skulstory.SetDataAll(true);
			GameData.Progress.SetRescued(NpcType.Fox, true);
			GameData.Progress.SetRescued(NpcType.Ogre, true);
			GameData.Progress.SetRescued(NpcType.Druid, true);
			GameData.Progress.SetRescued(NpcType.DeathKnight, true);
			GameData.Progress.arachneTutorial = true;
			GameData.Generic.normalEnding = true;
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00039D7A File Offset: 0x00037F7A
		private void EquipRandomItem()
		{
			base.StartCoroutine(this.CEquip());
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00039D89 File Offset: 0x00037F89
		private IEnumerator CEquip()
		{
			ItemReference itemReference = GearResource.instance.items.Random<ItemReference>();
			ItemRequest request = itemReference.LoadAsync();
			while (!request.isDone)
			{
				yield return null;
			}
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			levelManager.DropItem(request, levelManager.player.transform.position);
			yield break;
		}

		// Token: 0x04001019 RID: 4121
		[SerializeField]
		private Button _resetSeed;

		// Token: 0x0400101A RID: 4122
		[SerializeField]
		private Button _firstClear;

		// Token: 0x0400101B RID: 4123
		[SerializeField]
		private Button _resetProgress;

		// Token: 0x0400101C RID: 4124
		[SerializeField]
		private Button _randomItem;

		// Token: 0x0400101D RID: 4125
		[SerializeField]
		private Button _unlockAllItems;

		// Token: 0x0400101E RID: 4126
		[SerializeField]
		private Button _unlockAllUpgrades;

		// Token: 0x0400101F RID: 4127
		[SerializeField]
		private Button _lockAllUpgrades;

		// Token: 0x04001020 RID: 4128
		[SerializeField]
		private Button _itemReset;

		// Token: 0x04001021 RID: 4129
		[SerializeField]
		private Button _upgradeReset;

		// Token: 0x04001022 RID: 4130
		[SerializeField]
		private Button _allGearReset;
	}
}
