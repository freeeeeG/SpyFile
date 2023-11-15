using System;
using System.Collections.Generic;
using Database;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

// Token: 0x020009F1 RID: 2545
[SerializationConfig(MemberSerialization.OptIn)]
public class StoryManager : KMonoBehaviour
{
	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06004BE7 RID: 19431 RVA: 0x001A9DA0 File Offset: 0x001A7FA0
	// (set) Token: 0x06004BE8 RID: 19432 RVA: 0x001A9DA7 File Offset: 0x001A7FA7
	public static StoryManager Instance { get; private set; }

	// Token: 0x06004BE9 RID: 19433 RVA: 0x001A9DAF File Offset: 0x001A7FAF
	public static IReadOnlyList<StoryManager.StoryTelemetry> GetTelemetry()
	{
		return StoryManager.storyTelemetry;
	}

	// Token: 0x06004BEA RID: 19434 RVA: 0x001A9DB8 File Offset: 0x001A7FB8
	protected override void OnPrefabInit()
	{
		StoryManager.Instance = this;
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDayStarted));
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(instance.OnLoad, new Action<Game.GameSaveData>(this.OnGameLoaded));
	}

	// Token: 0x06004BEB RID: 19435 RVA: 0x001A9E10 File Offset: 0x001A8010
	protected override void OnCleanUp()
	{
		GameClock.Instance.Unsubscribe(631075836, new Action<object>(this.OnNewDayStarted));
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(instance.OnLoad, new Action<Game.GameSaveData>(this.OnGameLoaded));
	}

	// Token: 0x06004BEC RID: 19436 RVA: 0x001A9E60 File Offset: 0x001A8060
	public void InitialSaveSetup()
	{
		this.highestStoryCoordinateWhenGenerated = Db.Get().Stories.GetHighestCoordinateOffset();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			foreach (string storyTraitTemplate in worldContainer.StoryTraitIds)
			{
				Story storyFromStoryTrait = Db.Get().Stories.GetStoryFromStoryTrait(storyTraitTemplate);
				this.CreateStory(storyFromStoryTrait, worldContainer.id);
			}
		}
		this.LogInitialSaveSetup();
	}

	// Token: 0x06004BED RID: 19437 RVA: 0x001A9F24 File Offset: 0x001A8124
	public StoryInstance CreateStory(string id, int worldId)
	{
		Story story = Db.Get().Stories.Get(id);
		return this.CreateStory(story, worldId);
	}

	// Token: 0x06004BEE RID: 19438 RVA: 0x001A9F4C File Offset: 0x001A814C
	public StoryInstance CreateStory(Story story, int worldId)
	{
		StoryInstance storyInstance = new StoryInstance(story, worldId);
		this._stories.Add(story.HashId, storyInstance);
		StoryManager.InitTelemetry(storyInstance);
		if (story.autoStart)
		{
			this.BeginStoryEvent(story);
		}
		return storyInstance;
	}

	// Token: 0x06004BEF RID: 19439 RVA: 0x001A9F8C File Offset: 0x001A818C
	public StoryInstance GetStoryInstance(int hash)
	{
		StoryInstance result;
		this._stories.TryGetValue(hash, out result);
		return result;
	}

	// Token: 0x06004BF0 RID: 19440 RVA: 0x001A9FA9 File Offset: 0x001A81A9
	public Dictionary<int, StoryInstance> GetStoryInstances()
	{
		return this._stories;
	}

	// Token: 0x06004BF1 RID: 19441 RVA: 0x001A9FB1 File Offset: 0x001A81B1
	public int GetHighestCoordinate()
	{
		return this.highestStoryCoordinateWhenGenerated;
	}

	// Token: 0x06004BF2 RID: 19442 RVA: 0x001A9FB9 File Offset: 0x001A81B9
	private string GetCompleteUnlockId(string id)
	{
		return id + "_STORY_COMPLETE";
	}

	// Token: 0x06004BF3 RID: 19443 RVA: 0x001A9FC6 File Offset: 0x001A81C6
	public void ForceCreateStory(Story story, int worldId)
	{
		if (this.GetStoryInstance(story.HashId) == null)
		{
			this.CreateStory(story, worldId);
		}
	}

	// Token: 0x06004BF4 RID: 19444 RVA: 0x001A9FE0 File Offset: 0x001A81E0
	public void DiscoverStoryEvent(Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || this.CheckState(StoryInstance.State.DISCOVERED, story))
		{
			return;
		}
		storyInstance.CurrentState = StoryInstance.State.DISCOVERED;
	}

	// Token: 0x06004BF5 RID: 19445 RVA: 0x001AA010 File Offset: 0x001A8210
	public void BeginStoryEvent(Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || this.CheckState(StoryInstance.State.IN_PROGRESS, story))
		{
			return;
		}
		storyInstance.CurrentState = StoryInstance.State.IN_PROGRESS;
	}

	// Token: 0x06004BF6 RID: 19446 RVA: 0x001AA03F File Offset: 0x001A823F
	public void CompleteStoryEvent(Story story, MonoBehaviour keepsakeSpawnTarget, FocusTargetSequence.Data sequenceData)
	{
		if (this.GetStoryInstance(story.HashId) == null || this.CheckState(StoryInstance.State.COMPLETE, story))
		{
			return;
		}
		FocusTargetSequence.Start(keepsakeSpawnTarget, sequenceData);
	}

	// Token: 0x06004BF7 RID: 19447 RVA: 0x001AA064 File Offset: 0x001A8264
	public void CompleteStoryEvent(Story story, Vector3 keepsakeSpawnPosition)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null)
		{
			return;
		}
		GameObject prefab = Assets.GetPrefab(storyInstance.GetStory().keepsakePrefabId);
		if (prefab != null)
		{
			keepsakeSpawnPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			GameObject gameObject = Util.KInstantiate(prefab, keepsakeSpawnPosition);
			gameObject.SetActive(true);
			new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
		}
		storyInstance.CurrentState = StoryInstance.State.COMPLETE;
		Game.Instance.unlocks.Unlock(this.GetCompleteUnlockId(story.Id), true);
	}

	// Token: 0x06004BF8 RID: 19448 RVA: 0x001AA104 File Offset: 0x001A8304
	public bool CheckState(StoryInstance.State state, Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		return storyInstance != null && storyInstance.CurrentState >= state;
	}

	// Token: 0x06004BF9 RID: 19449 RVA: 0x001AA12F File Offset: 0x001A832F
	public bool IsStoryComplete(Story story)
	{
		return this.CheckState(StoryInstance.State.COMPLETE, story);
	}

	// Token: 0x06004BFA RID: 19450 RVA: 0x001AA139 File Offset: 0x001A8339
	public bool IsStoryCompleteGlobal(Story story)
	{
		return Game.Instance.unlocks.IsUnlocked(this.GetCompleteUnlockId(story.Id));
	}

	// Token: 0x06004BFB RID: 19451 RVA: 0x001AA158 File Offset: 0x001A8358
	public StoryInstance DisplayPopup(Story story, StoryManager.PopupInfo info, System.Action popupCB = null, Notification.ClickCallback notificationCB = null)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || storyInstance.HasDisplayedPopup(info.PopupType))
		{
			return null;
		}
		EventInfoData eventInfoData = EventInfoDataHelper.GenerateStoryTraitData(info.Title, info.Description, info.CloseButtonText, info.TextureName, info.PopupType, info.CloseButtonToolTip, info.Minions, popupCB);
		Notification notification = null;
		if (!info.DisplayImmediate)
		{
			notification = EventInfoScreen.CreateNotification(eventInfoData, notificationCB);
		}
		storyInstance.SetPopupData(info, eventInfoData, notification);
		return storyInstance;
	}

	// Token: 0x06004BFC RID: 19452 RVA: 0x001AA1D4 File Offset: 0x001A83D4
	public bool HasDisplayedPopup(Story story, EventInfoDataHelper.PopupType type)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		return storyInstance != null && storyInstance.HasDisplayedPopup(type);
	}

	// Token: 0x06004BFD RID: 19453 RVA: 0x001AA1FC File Offset: 0x001A83FC
	private void LogInitialSaveSetup()
	{
		int num = 0;
		StoryManager.StoryCreationTelemetry[] array = new StoryManager.StoryCreationTelemetry[CustomGameSettings.Instance.CurrentStoryLevelsBySetting.Count];
		foreach (KeyValuePair<string, string> keyValuePair in CustomGameSettings.Instance.CurrentStoryLevelsBySetting)
		{
			array[num] = new StoryManager.StoryCreationTelemetry
			{
				StoryId = keyValuePair.Key,
				Enabled = CustomGameSettings.Instance.IsStoryActive(keyValuePair.Key, keyValuePair.Value)
			};
			num++;
		}
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "StoryTraitsCreation", array);
	}

	// Token: 0x06004BFE RID: 19454 RVA: 0x001AA2A0 File Offset: 0x001A84A0
	private void OnNewDayStarted(object _)
	{
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "SavedHighestStoryCoordinate", this.highestStoryCoordinateWhenGenerated);
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "StoryTraits", StoryManager.storyTelemetry);
	}

	// Token: 0x06004BFF RID: 19455 RVA: 0x001AA2C8 File Offset: 0x001A84C8
	private static void InitTelemetry(StoryInstance story)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(story.worldId);
		if (world == null)
		{
			return;
		}
		story.Telemetry.StoryId = story.storyId;
		story.Telemetry.WorldId = world.worldName;
		StoryManager.storyTelemetry.Add(story.Telemetry);
	}

	// Token: 0x06004C00 RID: 19456 RVA: 0x001AA324 File Offset: 0x001A8524
	private void OnGameLoaded(object _)
	{
		StoryManager.storyTelemetry.Clear();
		foreach (KeyValuePair<int, StoryInstance> keyValuePair in this._stories)
		{
			StoryManager.InitTelemetry(keyValuePair.Value);
		}
		CustomGameSettings.Instance.DisableAllStories();
		foreach (KeyValuePair<int, StoryInstance> keyValuePair2 in this._stories)
		{
			SettingConfig config;
			if (keyValuePair2.Value.Telemetry.Retrofitted < 0f && CustomGameSettings.Instance.StorySettings.TryGetValue(keyValuePair2.Value.storyId, out config))
			{
				CustomGameSettings.Instance.SetStorySetting(config, true);
			}
		}
	}

	// Token: 0x06004C01 RID: 19457 RVA: 0x001AA410 File Offset: 0x001A8610
	public static void DestroyInstance()
	{
		StoryManager.storyTelemetry.Clear();
		StoryManager.Instance = null;
	}

	// Token: 0x04003196 RID: 12694
	public const int BEFORE_STORIES = -2;

	// Token: 0x04003198 RID: 12696
	private static List<StoryManager.StoryTelemetry> storyTelemetry = new List<StoryManager.StoryTelemetry>();

	// Token: 0x04003199 RID: 12697
	[Serialize]
	private Dictionary<int, StoryInstance> _stories = new Dictionary<int, StoryInstance>();

	// Token: 0x0400319A RID: 12698
	[Serialize]
	private int highestStoryCoordinateWhenGenerated = -2;

	// Token: 0x0400319B RID: 12699
	private const string STORY_TRAIT_KEY = "StoryTraits";

	// Token: 0x0400319C RID: 12700
	private const string STORY_CREATION_KEY = "StoryTraitsCreation";

	// Token: 0x0400319D RID: 12701
	private const string STORY_COORDINATE_KEY = "SavedHighestStoryCoordinate";

	// Token: 0x02001873 RID: 6259
	public struct PopupInfo
	{
		// Token: 0x04007208 RID: 29192
		public string Title;

		// Token: 0x04007209 RID: 29193
		public string Description;

		// Token: 0x0400720A RID: 29194
		public string CloseButtonText;

		// Token: 0x0400720B RID: 29195
		public string CloseButtonToolTip;

		// Token: 0x0400720C RID: 29196
		public string TextureName;

		// Token: 0x0400720D RID: 29197
		public GameObject[] Minions;

		// Token: 0x0400720E RID: 29198
		public bool DisplayImmediate;

		// Token: 0x0400720F RID: 29199
		public EventInfoDataHelper.PopupType PopupType;
	}

	// Token: 0x02001874 RID: 6260
	[SerializationConfig(MemberSerialization.OptIn)]
	public class StoryTelemetry : ISaveLoadable
	{
		// Token: 0x060091CD RID: 37325 RVA: 0x0032A7BC File Offset: 0x003289BC
		public void LogStateChange(StoryInstance.State state, float time)
		{
			switch (state)
			{
			case StoryInstance.State.RETROFITTED:
				this.Retrofitted = ((this.Retrofitted >= 0f) ? this.Retrofitted : time);
				return;
			case StoryInstance.State.NOT_STARTED:
				break;
			case StoryInstance.State.DISCOVERED:
				this.Discovered = ((this.Discovered >= 0f) ? this.Discovered : time);
				return;
			case StoryInstance.State.IN_PROGRESS:
				this.InProgress = ((this.InProgress >= 0f) ? this.InProgress : time);
				return;
			case StoryInstance.State.COMPLETE:
				this.Completed = ((this.Completed >= 0f) ? this.Completed : time);
				break;
			default:
				return;
			}
		}

		// Token: 0x04007210 RID: 29200
		public string StoryId;

		// Token: 0x04007211 RID: 29201
		public string WorldId;

		// Token: 0x04007212 RID: 29202
		[Serialize]
		public float Retrofitted = -1f;

		// Token: 0x04007213 RID: 29203
		[Serialize]
		public float Discovered = -1f;

		// Token: 0x04007214 RID: 29204
		[Serialize]
		public float InProgress = -1f;

		// Token: 0x04007215 RID: 29205
		[Serialize]
		public float Completed = -1f;
	}

	// Token: 0x02001875 RID: 6261
	public class StoryCreationTelemetry
	{
		// Token: 0x04007216 RID: 29206
		public string StoryId;

		// Token: 0x04007217 RID: 29207
		public bool Enabled;
	}
}
