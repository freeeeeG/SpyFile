using System;
using System.Collections;
using System.Globalization;
using Characters;
using Characters.Controllers;
using Data;
using FX;
using GameResources;
using Hardmode;
using Platforms;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Level
{
	// Token: 0x0200049B RID: 1179
	[CreateAssetMenu]
	public class Chapter : ScriptableObject
	{
		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x0004680C File Offset: 0x00044A0C
		public string chapterName
		{
			get
			{
				Chapter.Type type = this.type;
				if (Singleton<HardmodeManager>.Instance.hardmode && type >= Chapter.Type.HardmodeChapter1)
				{
					type = this.type - 6;
				}
				return Localization.GetLocalizedString(string.Format("map/{0}", type));
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001673 RID: 5747 RVA: 0x00046850 File Offset: 0x00044A50
		public string stageTag
		{
			get
			{
				Chapter.Type type = this.type;
				if (Singleton<HardmodeManager>.Instance.hardmode && type >= Chapter.Type.HardmodeChapter1)
				{
					type = this.type - 6;
				}
				return Localization.GetLocalizedString(string.Format("map/{0}/{1}/tag", type, this._stageIndex));
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x000468A0 File Offset: 0x00044AA0
		public string stageName
		{
			get
			{
				Chapter.Type type = this.type;
				if (Singleton<HardmodeManager>.Instance.hardmode && type >= Chapter.Type.HardmodeChapter1)
				{
					type = this.type - 6;
				}
				return Localization.GetLocalizedString(string.Format("map/{0}/{1}", type, this._stageIndex));
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x000468EE File Offset: 0x00044AEE
		public int smallPotionPrice
		{
			get
			{
				return this._smallPotionPrice;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001676 RID: 5750 RVA: 0x000468F6 File Offset: 0x00044AF6
		public int mediumPotionPrice
		{
			get
			{
				return this._mediumPotionPrice;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001677 RID: 5751 RVA: 0x000468FE File Offset: 0x00044AFE
		public int largePotionPrice
		{
			get
			{
				return this._largePotionPrice;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x00046906 File Offset: 0x00044B06
		public float adventurerGoldRewardMultiplier
		{
			get
			{
				return this._adventurerGoldRewardMultiplier;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x0004690E File Offset: 0x00044B0E
		public int[] collectorRefreshCosts
		{
			get
			{
				return this._collectorRefreshCosts;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x00046916 File Offset: 0x00044B16
		public Sprite gateWall
		{
			get
			{
				return this._gateWall;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x0004691E File Offset: 0x00044B1E
		public Sprite gateTable
		{
			get
			{
				return this._gateTable;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x00046926 File Offset: 0x00044B26
		public Sprite gateChoiceTable
		{
			get
			{
				return this._gateChoiceTable;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600167D RID: 5757 RVA: 0x0004692E File Offset: 0x00044B2E
		public Gate gatePrefab
		{
			get
			{
				return this._gatePrefab;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x0600167E RID: 5758 RVA: 0x00046936 File Offset: 0x00044B36
		// (set) Token: 0x0600167F RID: 5759 RVA: 0x0004693E File Offset: 0x00044B3E
		public Chapter.Type type { get; private set; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x00046947 File Offset: 0x00044B47
		// (set) Token: 0x06001681 RID: 5761 RVA: 0x0004694F File Offset: 0x00044B4F
		public IStageInfo currentStage { get; private set; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x00046958 File Offset: 0x00044B58
		public int stageIndex
		{
			get
			{
				return this._stageIndex;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001683 RID: 5763 RVA: 0x00046960 File Offset: 0x00044B60
		// (set) Token: 0x06001684 RID: 5764 RVA: 0x00046968 File Offset: 0x00044B68
		public Map map { get; private set; }

		// Token: 0x06001685 RID: 5765 RVA: 0x00046974 File Offset: 0x00044B74
		public void Initialize(Chapter.Type type)
		{
			this.type = type;
			for (int i = 0; i < this.stages.Length; i++)
			{
				if (this.stages[i] == null)
				{
					Debug.LogError(string.Format("[{0}] Stage is null : {1}, {2}, {3}", new object[]
					{
						"Chapter",
						type,
						i,
						this.stages[i]
					}));
				}
			}
			this._stageIndex = -1;
			this.currentStage = null;
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x000469F0 File Offset: 0x00044BF0
		private void ChangeStage(int stageIndex)
		{
			if (this._stageIndex == stageIndex)
			{
				return;
			}
			if (this.currentStage != null)
			{
				Resources.UnloadAsset(this.currentStage);
			}
			if (this._currentStageHandle.IsValid())
			{
				Addressables.Release<IStageInfo>(this._currentStageHandle);
			}
			this._stageIndex = stageIndex;
			this._currentStageHandle = this.stages[stageIndex].LoadAssetAsync<IStageInfo>();
			this.currentStage = this._currentStageHandle.WaitForCompletion();
			this.currentStage.Initialize();
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x00046A6E File Offset: 0x00044C6E
		public void Enter()
		{
			this.ChangeStage(0);
			this.LoadStage(0, 0);
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x00046A7F File Offset: 0x00044C7F
		public void Enter(int stageIndex, int pathIndex, int nodeIndex)
		{
			this.ChangeStage(stageIndex);
			this.LoadStage(pathIndex, nodeIndex);
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x00046A90 File Offset: 0x00044C90
		public bool Next(NodeIndex nodeIndex)
		{
			PathNode pathNode = this.currentStage.Next(nodeIndex);
			if (pathNode == null || pathNode.reference == null)
			{
				PoolObject.DespawnAllOrphans();
				PoolObject.ClearUnused();
				return this.NextStage();
			}
			this.ChangeMap(pathNode);
			return true;
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00046AD3 File Offset: 0x00044CD3
		public void Release()
		{
			this.Clear();
			if (this._currentStageHandle.IsValid())
			{
				Addressables.Release<IStageInfo>(this._currentStageHandle);
			}
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x00046AF4 File Offset: 0x00044CF4
		public void Clear()
		{
			Singleton<EffectPool>.Instance.ClearNonAttached();
			PoolObject.DespawnAllOrphans();
			Singleton<Service>.Instance.levelManager.ClearDrops();
			if (this.map != null)
			{
				UnityEngine.Object.Destroy(this.map.gameObject);
				this.map = null;
			}
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00046B44 File Offset: 0x00044D44
		public bool NextStage()
		{
			int num = this._stageIndex + 1;
			if (num == this.stages.Length)
			{
				return false;
			}
			this.ChangeStage(num);
			this.LoadStage(0, 0);
			return true;
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00046B78 File Offset: 0x00044D78
		private void LoadStage(int pathIndex = 0, int nodeIndex = 0)
		{
			Debug.Log(string.Format("[Load Stage] {0}, stage {1} index {2}, nodeIndex {3}", new object[]
			{
				this.type,
				this.stageIndex,
				pathIndex,
				nodeIndex
			}));
			if (this.currentStage.music == null)
			{
				PersistentSingleton<SoundManager>.Instance.FadeOutBackgroundMusic(1f);
			}
			else
			{
				PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this.currentStage.music, 1f, true, true, false);
			}
			this.currentStage.Reset();
			this.currentStage.pathIndex = pathIndex;
			PathNode item = this.currentStage.currentMapPath.Item1;
			PathNode item2 = this.currentStage.currentMapPath.Item2;
			PathNode pathNode = item;
			bool flag = item != null && item.reference != null;
			bool flag2 = item2 != null && item2.reference != null;
			if (!flag || (nodeIndex == 1 && flag2))
			{
				pathNode = item2;
			}
			LoadingScreen.LoadingScreenData value;
			if (!this.TryGetLoadingScreenData(out value))
			{
				CoroutineProxy.instance.StartCoroutine(this.CChangeMap(pathNode, null));
				return;
			}
			CoroutineProxy.instance.StartCoroutine(this.CChangeMap(pathNode, new LoadingScreen.LoadingScreenData?(value)));
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00046CB0 File Offset: 0x00044EB0
		private bool TryGetLoadingScreenData(out LoadingScreen.LoadingScreenData loadingScreenData)
		{
			if (this.currentStage.loadingScreenBackground == null)
			{
				loadingScreenData = default(LoadingScreen.LoadingScreenData);
				return false;
			}
			string description;
			if (MMMaths.RandomBool())
			{
				string[] localizedStringArray;
				if (!Localization.TryGetLocalizedStringArray(string.Format("loading/description/{0}/{1}", this.type, this._stageIndex), out localizedStringArray))
				{
					localizedStringArray = Localization.GetLocalizedStringArray("loading/description/common");
				}
				description = localizedStringArray.Random<string>();
			}
			else
			{
				description = Localization.GetLocalizedStringArray("loading/description/common").Random<string>();
			}
			AnimationClip walkClip = null;
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (player != null)
			{
				walkClip = player.playerComponents.inventory.weapon.polymorphOrCurrent.characterAnimation.walkClip;
			}
			if (this.type == Chapter.Type.Castle || (this.type == Chapter.Type.Chapter1 && this.stageIndex == 0))
			{
				loadingScreenData = new LoadingScreen.LoadingScreenData(this.currentStage.loadingScreenBackground, walkClip, this.stageName, description);
			}
			else
			{
				string currentTime = new TimeSpan(0, 0, GameData.Progress.playTime).ToString("hh\\ \\:\\ mm\\ \\:\\ ss", CultureInfo.InvariantCulture);
				string key = string.Format("{0}/{1}", this.type, this._stageIndex);
				bool bestTimeUpdated = GameData.Record.UpdateBestTime(key);
				string bestTime = new TimeSpan(0, 0, GameData.Record.GetBestTime(key)).ToString("hh\\ \\:\\ mm\\ \\:\\ ss", CultureInfo.InvariantCulture);
				loadingScreenData = new LoadingScreen.LoadingScreenData(this.currentStage.loadingScreenBackground, walkClip, this.stageName, description, currentTime, bestTime, bestTimeUpdated);
			}
			return true;
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00046E3C File Offset: 0x0004503C
		public void ChangeMap(PathNode pathNode)
		{
			CoroutineProxy.instance.StartCoroutine(this.CChangeMap(pathNode, null));
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00046E64 File Offset: 0x00045064
		private IEnumerator CChangeMap(PathNode pathNode, LoadingScreen.LoadingScreenData? loadingScreenData = null)
		{
			Chapter.<>c__DisplayClass63_0 CS$<>8__locals1 = new Chapter.<>c__DisplayClass63_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.levelManager = Singleton<Service>.Instance.levelManager;
			PlayerInput.blocked.Attach(this);
			if (CS$<>8__locals1.levelManager.player != null && CS$<>8__locals1.levelManager.player.invulnerable != null)
			{
				CS$<>8__locals1.levelManager.player.invulnerable.Attach(this);
			}
			if (this.map == null)
			{
				Chronometer.global.AttachTimeScale(this, 0f);
				Singleton<Service>.Instance.fadeInOut.FadeOutImmediately();
			}
			else
			{
				yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
				Chronometer.global.AttachTimeScale(this, 0f);
				this.Clear();
			}
			yield return Resources.UnloadUnusedAssets();
			if (SystemInfo.systemMemorySize <= 4096)
			{
				GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
				yield return Resources.UnloadUnusedAssets();
			}
			MapRequest newMapRequest = pathNode.reference.LoadAsync();
			do
			{
				yield return null;
			}
			while (!newMapRequest.isDone);
			if (pathNode.reference.darkEnemy)
			{
				DarkEnemySelector.instance.SetTargetCountInMap();
			}
			this.map = UnityEngine.Object.Instantiate<Map>(newMapRequest.asset, Vector3.zero, Quaternion.identity);
			ReleaseAddressableHandleOnDestroy.Reserve(this.map.gameObject, newMapRequest.handle);
			if (!string.IsNullOrWhiteSpace(this.chapterName) && !string.IsNullOrWhiteSpace(this.stageName) && this.map.displayStageName)
			{
				Scene<GameBase>.instance.uiManager.stageName.Show(this.chapterName, this.stageTag, this.stageName);
			}
			this.map.darkEnemy = pathNode.reference.darkEnemy;
			this.map.SetReward(pathNode.reward);
			this.map.SetExits(this.currentStage.nextMapPath.Item1, this.currentStage.nextMapPath.Item2);
			CS$<>8__locals1.levelManager.SpawnPlayerIfNotExist();
			CS$<>8__locals1.<CChangeMap>g__ResetPlayerPosition|0();
			CS$<>8__locals1.levelManager.ExcuteInNextFrame(new Action(CS$<>8__locals1.<CChangeMap>g__ResetPlayerPosition|0));
			GameBase instance = Scene<GameBase>.instance;
			Vector3 position = instance.cameraController.transform.position;
			(-this.map.backgroundOrigin).z = position.z;
			instance.cameraController.Move(this.map.playerOrigin);
			instance.minimapCameraController.Move(this.map.playerOrigin);
			instance.SetBackground((this.map.background == null) ? this.currentStage.background : this.map.background, this.map.playerOrigin.y - this.map.backgroundOrigin.y);
			GameData.Currency.SaveAll();
			GameData.Progress.SaveAll();
			GameData.HardmodeProgress.SaveAll();
			PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
			CS$<>8__locals1.levelManager.InvokeOnMapChanged();
			Singleton<Service>.Instance.fadeInOut.HideLoadingIcon();
			yield return Singleton<Service>.Instance.fadeInOut.CFadeIn();
			Chronometer.global.DetachTimeScale(this);
			PlayerInput.blocked.Detach(this);
			CS$<>8__locals1.levelManager.player.StartCoroutine(CS$<>8__locals1.<CChangeMap>g__CDetachInvulnerableInSecond|1());
			CS$<>8__locals1.levelManager.InvokeOnMapChangedAndFadeIn(this.map);
			yield break;
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00046E7C File Offset: 0x0004507C
		public void EnterOutTrack(Map newMap)
		{
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			levelManager.ClearDrops();
			if (this.map != null)
			{
				UnityEngine.Object.Destroy(this.map.gameObject);
			}
			this.map = UnityEngine.Object.Instantiate<Map>(newMap, Vector3.zero, Quaternion.identity);
			levelManager.player.transform.position = this.map.playerOrigin;
			levelManager.player.movement.controller.ResetBounds();
			Physics2D.SyncTransforms();
			GameBase instance = Scene<GameBase>.instance;
			Vector3 position = instance.cameraController.transform.position;
			(-this.map.backgroundOrigin).z = position.z;
			instance.cameraController.Move(this.map.playerOrigin);
			instance.minimapCameraController.Move(this.map.playerOrigin);
			instance.SetBackground((this.map.background == null) ? this.currentStage.background : this.map.background, this.map.playerOrigin.y - this.map.backgroundOrigin.y);
		}

		// Token: 0x040013B1 RID: 5041
		private Chapter.Type _chapterType;

		// Token: 0x040013B2 RID: 5042
		public AssetReference[] stages;

		// Token: 0x040013B3 RID: 5043
		[SerializeField]
		[Space]
		private int _smallPotionPrice;

		// Token: 0x040013B4 RID: 5044
		[SerializeField]
		private int _mediumPotionPrice;

		// Token: 0x040013B5 RID: 5045
		[SerializeField]
		private int _largePotionPrice;

		// Token: 0x040013B6 RID: 5046
		[SerializeField]
		private float _adventurerGoldRewardMultiplier;

		// Token: 0x040013B7 RID: 5047
		[Space]
		[SerializeField]
		private int[] _collectorRefreshCosts;

		// Token: 0x040013B8 RID: 5048
		[Space]
		[SerializeField]
		private Sprite _gateWall;

		// Token: 0x040013B9 RID: 5049
		[SerializeField]
		private Sprite _gateTable;

		// Token: 0x040013BA RID: 5050
		[SerializeField]
		private Sprite _gateChoiceTable;

		// Token: 0x040013BB RID: 5051
		[SerializeField]
		private Gate _gatePrefab;

		// Token: 0x040013BC RID: 5052
		private int _stageIndex;

		// Token: 0x040013BE RID: 5054
		private AsyncOperationHandle<IStageInfo> _currentStageHandle;

		// Token: 0x0200049C RID: 1180
		public enum Type
		{
			// Token: 0x040013C2 RID: 5058
			Test,
			// Token: 0x040013C3 RID: 5059
			Castle,
			// Token: 0x040013C4 RID: 5060
			Tutorial,
			// Token: 0x040013C5 RID: 5061
			Chapter1,
			// Token: 0x040013C6 RID: 5062
			Chapter2,
			// Token: 0x040013C7 RID: 5063
			Chapter3,
			// Token: 0x040013C8 RID: 5064
			Chapter4,
			// Token: 0x040013C9 RID: 5065
			Chapter5,
			// Token: 0x040013CA RID: 5066
			HardmodeCastle,
			// Token: 0x040013CB RID: 5067
			HardmodeChapter1,
			// Token: 0x040013CC RID: 5068
			HardmodeChapter2,
			// Token: 0x040013CD RID: 5069
			HardmodeChapter3,
			// Token: 0x040013CE RID: 5070
			HardmodeChapter4,
			// Token: 0x040013CF RID: 5071
			HardmodeChapter5
		}
	}
}
