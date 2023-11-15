using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Data;
using FX;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Level
{
	// Token: 0x02000545 RID: 1349
	public sealed class LevelManager : MonoBehaviour
	{
		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06001A89 RID: 6793 RVA: 0x000530F0 File Offset: 0x000512F0
		// (remove) Token: 0x06001A8A RID: 6794 RVA: 0x00053128 File Offset: 0x00051328
		public event Action onMapLoaded;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06001A8B RID: 6795 RVA: 0x00053160 File Offset: 0x00051360
		// (remove) Token: 0x06001A8C RID: 6796 RVA: 0x00053198 File Offset: 0x00051398
		public event Action onMapLoadedAndFadedIn;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06001A8D RID: 6797 RVA: 0x000531D0 File Offset: 0x000513D0
		// (remove) Token: 0x06001A8E RID: 6798 RVA: 0x00053208 File Offset: 0x00051408
		public event LevelManager.OnMapChangedDelegate onMapChangedAndFadedIn;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06001A8F RID: 6799 RVA: 0x00053240 File Offset: 0x00051440
		// (remove) Token: 0x06001A90 RID: 6800 RVA: 0x00053278 File Offset: 0x00051478
		public event Action onChapterLoaded;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06001A91 RID: 6801 RVA: 0x000532B0 File Offset: 0x000514B0
		// (remove) Token: 0x06001A92 RID: 6802 RVA: 0x000532E8 File Offset: 0x000514E8
		public event Action onActivateMapReward;

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001A93 RID: 6803 RVA: 0x0005331D File Offset: 0x0005151D
		public int prevChapterIndex
		{
			get
			{
				return this._prevChapterIndex;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001A94 RID: 6804 RVA: 0x00053325 File Offset: 0x00051525
		// (set) Token: 0x06001A95 RID: 6805 RVA: 0x0005332D File Offset: 0x0005152D
		public bool skulSpawnAnimaionEnable { get; set; } = true;

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001A96 RID: 6806 RVA: 0x00053336 File Offset: 0x00051536
		// (set) Token: 0x06001A97 RID: 6807 RVA: 0x0005333E File Offset: 0x0005153E
		public bool skulPortalUsed { get; set; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001A98 RID: 6808 RVA: 0x00053347 File Offset: 0x00051547
		// (set) Token: 0x06001A99 RID: 6809 RVA: 0x0005334F File Offset: 0x0005154F
		public Character player { get; private set; }

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x00053358 File Offset: 0x00051558
		// (set) Token: 0x06001A9B RID: 6811 RVA: 0x00053360 File Offset: 0x00051560
		public Chapter currentChapter { get; private set; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001A9C RID: 6812 RVA: 0x00053369 File Offset: 0x00051569
		// (set) Token: 0x06001A9D RID: 6813 RVA: 0x00053371 File Offset: 0x00051571
		public bool clearing { get; private set; }

		// Token: 0x06001A9E RID: 6814 RVA: 0x0005337A File Offset: 0x0005157A
		public void ClearEvents()
		{
			this.onMapLoaded = null;
			this.onMapLoadedAndFadedIn = null;
			this.onMapChangedAndFadedIn = null;
			this.onChapterLoaded = null;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00053398 File Offset: 0x00051598
		public void ClearDrops()
		{
			this.clearing = true;
			for (int i = this._drops.Count - 1; i >= 0; i--)
			{
				this._drops[i].Despawn();
			}
			this._drops.Clear();
			this.clearing = false;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x000533E7 File Offset: 0x000515E7
		public void RegisterDrop(PoolObject poolObject)
		{
			this._drops.Add(poolObject);
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x000533F5 File Offset: 0x000515F5
		public void DeregisterDrop(PoolObject poolObject)
		{
			if (this.clearing)
			{
				return;
			}
			this._drops.Remove(poolObject);
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x0005340D File Offset: 0x0005160D
		public void DropGold(int amount, int count)
		{
			this.DropGold(amount, count, this.player.transform.position);
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00053427 File Offset: 0x00051627
		public void DropGold(int amount, int count, Vector3 position)
		{
			this.DropCurrency(GameData.Currency.Type.Gold, amount, count, position);
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x00053433 File Offset: 0x00051633
		public void DropGold(int amount, int count, Vector3 position, Vector2 force)
		{
			this.DropCurrency(GameData.Currency.Type.Gold, amount, count, position, force);
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00053441 File Offset: 0x00051641
		public void DropDarkQuartz(int amount)
		{
			this.DropDarkQuartz(amount, this.player.transform.position);
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x0005345A File Offset: 0x0005165A
		public void DropDarkQuartz(int amount, Vector3 position)
		{
			this.DropDarkQuartz(amount, Mathf.Min(amount, 20), position);
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x0005346C File Offset: 0x0005166C
		public void DropDarkQuartz(int amount, Vector3 position, Vector2 force)
		{
			this.DropDarkQuartz(amount, Mathf.Min(amount, 20), position, force);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x0005347F File Offset: 0x0005167F
		public void DropDarkQuartz(int amount, int count, Vector3 position)
		{
			this.DropCurrency(GameData.Currency.Type.DarkQuartz, amount, count, position);
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x0005348B File Offset: 0x0005168B
		public void DropDarkQuartz(int amount, int count, Vector3 position, Vector2 force)
		{
			this.DropCurrency(GameData.Currency.Type.DarkQuartz, amount, count, position, force);
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x00053499 File Offset: 0x00051699
		public void DropBone(int amount, int count)
		{
			this.DropBone(amount, count, this.player.transform.position);
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x000534B3 File Offset: 0x000516B3
		public void DropBone(int amount, int count, Vector3 position)
		{
			this.DropCurrency(GameData.Currency.Type.Bone, amount, count, position);
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x000534BF File Offset: 0x000516BF
		public void DropBone(int amount, int count, Vector3 position, Vector2 force)
		{
			this.DropCurrency(GameData.Currency.Type.Bone, amount, count, position, force);
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x000534D0 File Offset: 0x000516D0
		public void DropCurrency(GameData.Currency.Type type, int amount, int count, Vector3 position)
		{
			if (count == 0)
			{
				return;
			}
			int currencyAmount = amount / count;
			PoolObject currencyParticle = CommonResource.instance.GetCurrencyParticle(type);
			for (int i = 0; i < count; i++)
			{
				CurrencyParticle component = currencyParticle.Spawn(position, true).GetComponent<CurrencyParticle>();
				component.currencyType = type;
				component.currencyAmount = currencyAmount;
				if (i == 0)
				{
					component.currencyAmount += amount % count;
				}
			}
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x00053530 File Offset: 0x00051730
		public void DropCurrencyBag(GameData.Currency.Type type, Rarity rarity, int amount, int count, Vector3 position)
		{
			if (count == 0)
			{
				return;
			}
			CurrencyBag currencyBag = CurrencyBagResource.instance.GetCurrencyBag(type, rarity);
			CurrencyBag currencyBag2 = UnityEngine.Object.Instantiate<CurrencyBag>(currencyBag, position, Quaternion.identity, Map.Instance.transform);
			currencyBag2.name = currencyBag.name;
			currencyBag2.amount = amount;
			currencyBag2.count = count;
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x00053580 File Offset: 0x00051780
		public void DropCurrency(GameData.Currency.Type type, int amount, int count, Vector3 position, Vector2 force)
		{
			if (count == 0)
			{
				return;
			}
			int currencyAmount = amount / count;
			PoolObject currencyParticle = CommonResource.instance.GetCurrencyParticle(type);
			for (int i = 0; i < count; i++)
			{
				CurrencyParticle component = currencyParticle.Spawn(position, true).GetComponent<CurrencyParticle>();
				component.currencyType = type;
				component.currencyAmount = currencyAmount;
				component.SetForce(force);
				if (i == 0)
				{
					component.currencyAmount += amount % count;
				}
			}
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x000535E5 File Offset: 0x000517E5
		public Weapon DropWeapon(WeaponRequest gearRequest, Vector3 position)
		{
			Weapon weapon = this.DropWeapon(gearRequest.asset, position);
			ReleaseAddressableHandleOnDestroy.Reserve(weapon.gameObject, gearRequest.handle);
			gearRequest.SetReleaseReserved();
			return weapon;
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x0005360B File Offset: 0x0005180B
		public Weapon DropWeapon(Weapon weapon, Vector3 position)
		{
			if (weapon == null)
			{
				return null;
			}
			Weapon weapon2 = UnityEngine.Object.Instantiate<Weapon>(weapon, position, Quaternion.identity);
			weapon2.name = weapon.name;
			weapon2.transform.parent = Map.Instance.transform;
			weapon2.Initialize();
			return weapon2;
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x0005364B File Offset: 0x0005184B
		public Item DropItem(ItemRequest gearRequest, Vector3 position)
		{
			Item item = this.DropItem(gearRequest.asset, position);
			ReleaseAddressableHandleOnDestroy.Reserve(item.gameObject, gearRequest.handle);
			gearRequest.SetReleaseReserved();
			return item;
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x00053671 File Offset: 0x00051871
		public Item DropItem(Item item, Vector3 position)
		{
			if (item == null)
			{
				return null;
			}
			Item item2 = UnityEngine.Object.Instantiate<Item>(item, position, Quaternion.identity);
			item2.name = item.name;
			item2.transform.parent = Map.Instance.transform;
			item2.Initialize();
			return item2;
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x000536B1 File Offset: 0x000518B1
		public Quintessence DropQuintessence(EssenceRequest gearRequest, Vector3 position)
		{
			Quintessence quintessence = this.DropQuintessence(gearRequest.asset, position);
			ReleaseAddressableHandleOnDestroy.Reserve(quintessence.gameObject, gearRequest.handle);
			gearRequest.SetReleaseReserved();
			return quintessence;
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000536D7 File Offset: 0x000518D7
		public Quintessence DropQuintessence(Quintessence quintessence, Vector3 position)
		{
			if (quintessence == null)
			{
				return null;
			}
			Quintessence quintessence2 = UnityEngine.Object.Instantiate<Quintessence>(quintessence, position, Quaternion.identity);
			quintessence2.name = quintessence.name;
			quintessence2.transform.parent = Map.Instance.transform;
			quintessence2.Initialize();
			return quintessence2;
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00053717 File Offset: 0x00051917
		public Gear DropGear(GearRequest gearRequest, Vector3 position)
		{
			Gear gear = this.DropGear(gearRequest.asset, position);
			ReleaseAddressableHandleOnDestroy.Reserve(gear.gameObject, gearRequest.handle);
			gearRequest.SetReleaseReserved();
			return gear;
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x0005373D File Offset: 0x0005193D
		public Gear DropGear(Gear gear, Vector3 position)
		{
			Gear gear2 = UnityEngine.Object.Instantiate<Gear>(gear, position, Quaternion.identity);
			gear2.name = gear.name;
			gear2.transform.parent = Map.Instance.transform;
			gear2.Initialize();
			return gear2;
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x00053772 File Offset: 0x00051972
		public Potion DropPotion(Potion potion)
		{
			return this.DropPotion(potion, this.player.transform.position);
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x0005378B File Offset: 0x0005198B
		public Potion DropPotion(Potion potion, Vector3 position)
		{
			Potion potion2 = UnityEngine.Object.Instantiate<Potion>(potion, position, Quaternion.identity);
			potion2.name = potion.name;
			potion2.transform.parent = Map.Instance.transform;
			potion2.Initialize();
			return potion2;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000537C0 File Offset: 0x000519C0
		private void ChangeChapter(int chapterIndex)
		{
			if (this._currentChapterIndex == chapterIndex)
			{
				return;
			}
			if (this.currentChapter != null)
			{
				this.currentChapter.Release();
				Resources.UnloadAsset(this.currentChapter);
			}
			if (this._currentChapterHandle.IsValid())
			{
				Addressables.Release<Chapter>(this._currentChapterHandle);
			}
			this._prevChapterIndex = this._currentChapterIndex;
			this._currentChapterIndex = chapterIndex;
			AssetReference chapter = LevelResource.instance.GetChapter(chapterIndex);
			this._currentChapterHandle = chapter.LoadAssetAsync<Chapter>();
			this.currentChapter = this._currentChapterHandle.WaitForCompletion();
			this.currentChapter.Initialize((Chapter.Type)chapterIndex);
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x0005385B File Offset: 0x00051A5B
		public void Load(Chapter.Type chapter)
		{
			this.Load((int)chapter);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x00053864 File Offset: 0x00051A64
		private void Load(int chapterIndex)
		{
			if (chapterIndex == 2)
			{
				GameData.Generic.tutorial.Start();
			}
			if (chapterIndex == 1 || chapterIndex == 2 || chapterIndex == 8)
			{
				GameData.Save.instance.ResetAll();
				GameData.Currency.gold.ResetNonpermaAll();
				GameData.Currency.bone.ResetNonpermaAll();
				GameData.Currency.heartQuartz.ResetNonpermaAll();
				GameData.Currency.darkQuartz.income = 0;
				GameData.HardmodeProgress.ResetNonpermaAll();
				GameData.Progress.ResetNonpermaAll();
				GameData.Buff.ResetAll();
			}
			GameData.HardmodeProgress.hardmode = (chapterIndex >= 8);
			base.transform.Empty();
			this.ChangeChapter(chapterIndex);
			if (Scene<GameBase>.instance == null)
			{
				SceneManager.LoadSceneAsync("gameBase", LoadSceneMode.Single).completed += delegate(AsyncOperation operation)
				{
					this.currentChapter.Enter();
				};
			}
			else
			{
				this.currentChapter.Enter();
			}
			Action action = this.onChapterLoaded;
			if (action != null)
			{
				action();
			}
			this.Save();
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x0005393C File Offset: 0x00051B3C
		public void LoadGame()
		{
			if (GameData.Save.instance.hasSave)
			{
				this.LoadFromSave();
				return;
			}
			Chapter.Type chapter = GameData.Generic.tutorial.isPlayed() ? Chapter.Type.Castle : Chapter.Type.Tutorial;
			this.Load(chapter);
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x00053974 File Offset: 0x00051B74
		public void LoadFromSave()
		{
			LevelManager.<>c__DisplayClass76_0 CS$<>8__locals1 = new LevelManager.<>c__DisplayClass76_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.save = GameData.Save.instance;
			this.ChangeChapter(CS$<>8__locals1.save.chapterIndex);
			if (Scene<GameBase>.instance == null)
			{
				SceneManager.LoadSceneAsync("gameBase", LoadSceneMode.Single).completed += delegate(AsyncOperation operation)
				{
					base.<LoadFromSave>g__Load|0();
				};
				return;
			}
			CS$<>8__locals1.<LoadFromSave>g__Load|0();
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x000539DC File Offset: 0x00051BDC
		private void Save()
		{
			Chapter.Type currentChapterIndex = (Chapter.Type)this._currentChapterIndex;
			if (currentChapterIndex == Chapter.Type.Castle || currentChapterIndex == Chapter.Type.Tutorial || currentChapterIndex == Chapter.Type.Test)
			{
				return;
			}
			if (this.player == null)
			{
				return;
			}
			this.player.playerComponents.inventory.Save();
			GameData.Save instance = GameData.Save.instance;
			instance.hasSave = true;
			instance.health = (int)this.player.health.currentHealth;
			instance.chapterIndex = this._currentChapterIndex;
			instance.stageIndex = this.currentChapter.stageIndex;
			instance.pathIndex = this.currentChapter.currentStage.pathIndex;
			instance.nodeIndex = (int)this.currentChapter.currentStage.nodeIndex;
			List<AbilityBuff> instancesByInstanceType = this.player.ability.GetInstancesByInstanceType<AbilityBuff>();
			instance.abilityBuffs = string.Join<AbilityBuff>(",", instancesByInstanceType);
			instance.SaveAll();
			this.player.playerComponents.savableAbilityManager.SaveAll();
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x00053AC8 File Offset: 0x00051CC8
		private void Reset()
		{
			Singleton<EffectPool>.Instance.Clear();
			PoolObject.Clear();
			Scene<GameBase>.instance.poolObjectContainer.DespawnAll();
			Scene<GameBase>.instance.cameraController.shake.Clear();
			Singleton<Service>.Instance.controllerVibation.Stop();
			GameBase instance = Scene<GameBase>.instance;
			instance.gameFadeInOut.Deactivate();
			instance.cameraController.Zoom(1f, 1f);
			instance.cameraController.StopTrack();
			instance.uiManager.curseOfLightVignette.Hide();
			this.DestroyPlayer();
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x00053B5B File Offset: 0x00051D5B
		public void Unload()
		{
			this.Reset();
			base.transform.Empty();
			this.currentChapter.Clear();
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x00053B79 File Offset: 0x00051D79
		public void ResetGame()
		{
			this._prevChapterIndex = this._currentChapterIndex;
			this.Reset();
			if (!GameData.Generic.tutorial.isPlayed())
			{
				this.Load(Chapter.Type.Tutorial);
				return;
			}
			if (GameData.HardmodeProgress.hardmode)
			{
				this.Load(Chapter.Type.HardmodeCastle);
				return;
			}
			this.Load(Chapter.Type.Castle);
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x00053BB7 File Offset: 0x00051DB7
		public void ResetGame(Chapter.Type chapter)
		{
			this._prevChapterIndex = this._currentChapterIndex;
			this.Reset();
			this.Load(chapter);
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x00053BD4 File Offset: 0x00051DD4
		public void SpawnPlayerIfNotExist()
		{
			GameBase instance = Scene<GameBase>.instance;
			if (this.player == null)
			{
				this.player = UnityEngine.Object.Instantiate<Character>(CommonResource.instance.player, instance.transform);
				instance.uiManager.headupDisplay.Initialize(this.player);
				instance.cameraController.StartTrack(this.player.transform);
				instance.minimapCameraController.StartTrack(this.player.transform);
			}
			UnityEngine.Object.DontDestroyOnLoad(this.player);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x00053C5D File Offset: 0x00051E5D
		public void LoadNextStage()
		{
			this.currentChapter.NextStage();
			this.Save();
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x00053C71 File Offset: 0x00051E71
		public void LoadNextMap(NodeIndex nodeIndex = NodeIndex.Node1)
		{
			base.StartCoroutine(this.CLoadNextMap(nodeIndex));
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x00053C81 File Offset: 0x00051E81
		private IEnumerator CLoadNextMap(NodeIndex nodeIndex)
		{
			Chapter chapter = this.currentChapter;
			this._oldMap = chapter.map;
			Singleton<Service>.Instance.gearManager.DestroyDroppedInstaces();
			if (!chapter.Next(nodeIndex))
			{
				yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
				chapter.Clear();
				this.Load(this._currentChapterIndex + 1);
			}
			this.Save();
			yield break;
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x00053C97 File Offset: 0x00051E97
		public void InvokeOnActivateMapReward()
		{
			Action action = this.onActivateMapReward;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x00053CA9 File Offset: 0x00051EA9
		public void InvokeOnMapChanged()
		{
			Action action = this.onMapLoaded;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00053CBB File Offset: 0x00051EBB
		public void InvokeOnMapChangedAndFadeIn(Map newMap)
		{
			Action action = this.onMapLoadedAndFadedIn;
			if (action != null)
			{
				action();
			}
			LevelManager.OnMapChangedDelegate onMapChangedDelegate = this.onMapChangedAndFadedIn;
			if (onMapChangedDelegate == null)
			{
				return;
			}
			onMapChangedDelegate(this._oldMap, newMap);
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00053CE8 File Offset: 0x00051EE8
		public void EnterOutTrack(Map map, bool playerReset = false)
		{
			Chapter currentChapter = this.currentChapter;
			this._oldMap = currentChapter.map;
			if (playerReset)
			{
				this.DestroyPlayer();
				this.SpawnPlayerIfNotExist();
			}
			currentChapter.EnterOutTrack(map);
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00053D1E File Offset: 0x00051F1E
		public void DestroyPlayer()
		{
			if (this.player != null)
			{
				UnityEngine.Object.Destroy(this.player.gameObject);
				this.player = null;
			}
		}

		// Token: 0x04001717 RID: 5911
		[SerializeField]
		private AbilityBuffList _abilityBuffList;

		// Token: 0x04001718 RID: 5912
		private Map _oldMap;

		// Token: 0x04001719 RID: 5913
		private List<PoolObject> _drops = new List<PoolObject>();

		// Token: 0x0400171A RID: 5914
		private int _prevChapterIndex = -1;

		// Token: 0x0400171B RID: 5915
		private int _currentChapterIndex = -1;

		// Token: 0x0400171F RID: 5919
		private AsyncOperationHandle<Chapter> _currentChapterHandle;

		// Token: 0x02000546 RID: 1350
		// (Invoke) Token: 0x06001AD0 RID: 6864
		public delegate void OnMapChangedDelegate(Map old, Map @new);
	}
}
