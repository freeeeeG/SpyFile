using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000518 RID: 1304
public class TrackerTool : KMonoBehaviour
{
	// Token: 0x06001F4E RID: 8014 RVA: 0x000A7198 File Offset: 0x000A5398
	protected override void OnSpawn()
	{
		TrackerTool.Instance = this;
		base.OnSpawn();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			this.AddNewWorldTrackers(worldContainer.id);
		}
		foreach (object obj in Components.LiveMinionIdentities)
		{
			this.AddMinionTrackers((MinionIdentity)obj);
		}
		Components.LiveMinionIdentities.OnAdd += this.AddMinionTrackers;
		ClusterManager.Instance.Subscribe(-1280433810, new Action<object>(this.Refresh));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorld));
	}

	// Token: 0x06001F4F RID: 8015 RVA: 0x000A7294 File Offset: 0x000A5494
	protected override void OnForcedCleanUp()
	{
		TrackerTool.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06001F50 RID: 8016 RVA: 0x000A72A4 File Offset: 0x000A54A4
	private void AddMinionTrackers(MinionIdentity identity)
	{
		this.minionTrackers.Add(identity, new List<MinionTracker>());
		identity.Subscribe(1969584890, delegate(object data)
		{
			this.minionTrackers.Remove(identity);
		});
	}

	// Token: 0x06001F51 RID: 8017 RVA: 0x000A72F8 File Offset: 0x000A54F8
	private void Refresh(object data)
	{
		int worldID = (int)data;
		this.AddNewWorldTrackers(worldID);
	}

	// Token: 0x06001F52 RID: 8018 RVA: 0x000A7314 File Offset: 0x000A5514
	private void RemoveWorld(object data)
	{
		int world_id = (int)data;
		this.worldTrackers.RemoveAll((WorldTracker match) => match.WorldID == world_id);
	}

	// Token: 0x06001F53 RID: 8019 RVA: 0x000A734B File Offset: 0x000A554B
	public bool IsRocketInterior(int worldID)
	{
		return ClusterManager.Instance.GetWorld(worldID).IsModuleInterior;
	}

	// Token: 0x06001F54 RID: 8020 RVA: 0x000A7360 File Offset: 0x000A5560
	private void AddNewWorldTrackers(int worldID)
	{
		this.worldTrackers.Add(new StressTracker(worldID));
		this.worldTrackers.Add(new KCalTracker(worldID));
		this.worldTrackers.Add(new IdleTracker(worldID));
		this.worldTrackers.Add(new BreathabilityTracker(worldID));
		this.worldTrackers.Add(new PowerUseTracker(worldID));
		this.worldTrackers.Add(new BatteryTracker(worldID));
		this.worldTrackers.Add(new CropTracker(worldID));
		this.worldTrackers.Add(new WorkingToiletTracker(worldID));
		this.worldTrackers.Add(new RadiationTracker(worldID));
		if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
		{
			this.worldTrackers.Add(new RocketFuelTracker(worldID));
			this.worldTrackers.Add(new RocketOxidizerTracker(worldID));
		}
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			this.worldTrackers.Add(new WorkTimeTracker(worldID, Db.Get().ChoreGroups[i]));
			this.worldTrackers.Add(new ChoreCountTracker(worldID, Db.Get().ChoreGroups[i]));
		}
		this.worldTrackers.Add(new AllChoresCountTracker(worldID));
		this.worldTrackers.Add(new AllWorkTimeTracker(worldID));
		foreach (Tag tag in GameTags.CalorieCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag));
			foreach (GameObject gameObject in Assets.GetPrefabsWithTag(tag))
			{
				this.AddResourceTracker(worldID, gameObject.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag2 in GameTags.UnitCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag2));
			foreach (GameObject gameObject2 in Assets.GetPrefabsWithTag(tag2))
			{
				this.AddResourceTracker(worldID, gameObject2.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag3 in GameTags.MaterialCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag3));
			foreach (GameObject gameObject3 in Assets.GetPrefabsWithTag(tag3))
			{
				this.AddResourceTracker(worldID, gameObject3.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag4 in GameTags.OtherEntityTags)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag4));
			foreach (GameObject gameObject4 in Assets.GetPrefabsWithTag(tag4))
			{
				this.AddResourceTracker(worldID, gameObject4.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (GameObject gameObject5 in Assets.GetPrefabsWithTag(GameTags.CookingIngredient))
		{
			this.AddResourceTracker(worldID, gameObject5.GetComponent<KPrefabID>().PrefabTag);
		}
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			this.AddResourceTracker(worldID, foodInfo.Id);
		}
		foreach (Element element in ElementLoader.elements)
		{
			this.AddResourceTracker(worldID, element.tag);
		}
	}

	// Token: 0x06001F55 RID: 8021 RVA: 0x000A7808 File Offset: 0x000A5A08
	private void AddResourceTracker(int worldID, Tag tag)
	{
		if (this.worldTrackers.Find((WorldTracker match) => match is ResourceTracker && ((ResourceTracker)match).WorldID == worldID && ((ResourceTracker)match).tag == tag) != null)
		{
			return;
		}
		this.worldTrackers.Add(new ResourceTracker(worldID, tag));
	}

	// Token: 0x06001F56 RID: 8022 RVA: 0x000A7860 File Offset: 0x000A5A60
	public ResourceTracker GetResourceStatistic(int worldID, Tag tag)
	{
		return (ResourceTracker)this.worldTrackers.Find((WorldTracker match) => match is ResourceTracker && ((ResourceTracker)match).WorldID == worldID && ((ResourceTracker)match).tag == tag);
	}

	// Token: 0x06001F57 RID: 8023 RVA: 0x000A78A0 File Offset: 0x000A5AA0
	public WorldTracker GetWorldTracker<T>(int worldID) where T : WorldTracker
	{
		return (T)((object)this.worldTrackers.Find((WorldTracker match) => match is T && ((T)((object)match)).WorldID == worldID));
	}

	// Token: 0x06001F58 RID: 8024 RVA: 0x000A78DC File Offset: 0x000A5ADC
	public ChoreCountTracker GetChoreGroupTracker(int worldID, ChoreGroup choreGroup)
	{
		return (ChoreCountTracker)this.worldTrackers.Find((WorldTracker match) => match is ChoreCountTracker && ((ChoreCountTracker)match).WorldID == worldID && ((ChoreCountTracker)match).choreGroup == choreGroup);
	}

	// Token: 0x06001F59 RID: 8025 RVA: 0x000A791C File Offset: 0x000A5B1C
	public WorkTimeTracker GetWorkTimeTracker(int worldID, ChoreGroup choreGroup)
	{
		return (WorkTimeTracker)this.worldTrackers.Find((WorldTracker match) => match is WorkTimeTracker && ((WorkTimeTracker)match).WorldID == worldID && ((WorkTimeTracker)match).choreGroup == choreGroup);
	}

	// Token: 0x06001F5A RID: 8026 RVA: 0x000A7959 File Offset: 0x000A5B59
	public MinionTracker GetMinionTracker<T>(MinionIdentity identity) where T : MinionTracker
	{
		return (T)((object)this.minionTrackers[identity].Find((MinionTracker match) => match is T));
	}

	// Token: 0x06001F5B RID: 8027 RVA: 0x000A7998 File Offset: 0x000A5B98
	public void Update()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		if (!this.trackerActive)
		{
			return;
		}
		if (this.minionTrackers.Count > 0)
		{
			this.updatingMinionTracker++;
			if (this.updatingMinionTracker >= this.minionTrackers.Count)
			{
				this.updatingMinionTracker = 0;
			}
			KeyValuePair<MinionIdentity, List<MinionTracker>> keyValuePair = this.minionTrackers.ElementAt(this.updatingMinionTracker);
			for (int i = 0; i < keyValuePair.Value.Count; i++)
			{
				keyValuePair.Value[i].UpdateData();
			}
		}
		if (this.worldTrackers.Count > 0)
		{
			for (int j = 0; j < this.numUpdatesPerFrame; j++)
			{
				this.updatingWorldTracker++;
				if (this.updatingWorldTracker >= this.worldTrackers.Count)
				{
					this.updatingWorldTracker = 0;
				}
				this.worldTrackers[this.updatingWorldTracker].UpdateData();
			}
		}
	}

	// Token: 0x04001199 RID: 4505
	public static TrackerTool Instance;

	// Token: 0x0400119A RID: 4506
	private List<WorldTracker> worldTrackers = new List<WorldTracker>();

	// Token: 0x0400119B RID: 4507
	private Dictionary<MinionIdentity, List<MinionTracker>> minionTrackers = new Dictionary<MinionIdentity, List<MinionTracker>>();

	// Token: 0x0400119C RID: 4508
	private int updatingWorldTracker;

	// Token: 0x0400119D RID: 4509
	private int updatingMinionTracker;

	// Token: 0x0400119E RID: 4510
	public bool trackerActive = true;

	// Token: 0x0400119F RID: 4511
	private int numUpdatesPerFrame = 50;
}
