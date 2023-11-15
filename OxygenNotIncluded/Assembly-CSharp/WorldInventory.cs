using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x02000A36 RID: 2614
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/WorldInventory")]
public class WorldInventory : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x06004EA9 RID: 20137 RVA: 0x001B9337 File Offset: 0x001B7537
	public WorldContainer WorldContainer
	{
		get
		{
			if (this.m_worldContainer == null)
			{
				this.m_worldContainer = base.GetComponent<WorldContainer>();
			}
			return this.m_worldContainer;
		}
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06004EAA RID: 20138 RVA: 0x001B9359 File Offset: 0x001B7559
	public bool HasValidCount
	{
		get
		{
			return this.hasValidCount;
		}
	}

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06004EAB RID: 20139 RVA: 0x001B9364 File Offset: 0x001B7564
	private int worldId
	{
		get
		{
			WorldContainer worldContainer = this.WorldContainer;
			if (!(worldContainer != null))
			{
				return -1;
			}
			return worldContainer.id;
		}
	}

	// Token: 0x06004EAC RID: 20140 RVA: 0x001B938C File Offset: 0x001B758C
	protected override void OnPrefabInit()
	{
		base.Subscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
		base.Subscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
		base.Subscribe<WorldInventory>(631075836, WorldInventory.OnNewDayDelegate);
		this.m_worldContainer = base.GetComponent<WorldContainer>();
	}

	// Token: 0x06004EAD RID: 20141 RVA: 0x001B93FC File Offset: 0x001B75FC
	protected override void OnCleanUp()
	{
		base.Unsubscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
		base.Unsubscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
		base.OnCleanUp();
	}

	// Token: 0x06004EAE RID: 20142 RVA: 0x001B9454 File Offset: 0x001B7654
	private void GenerateInventoryReport(object data)
	{
		int num = 0;
		int num2 = 0;
		foreach (Brain brain in Components.Brains.GetWorldItems(this.worldId, false))
		{
			CreatureBrain creatureBrain = brain as CreatureBrain;
			if (creatureBrain != null)
			{
				if (creatureBrain.HasTag(GameTags.Creatures.Wild))
				{
					num++;
					ReportManager.Instance.ReportValue(ReportManager.ReportType.WildCritters, 1f, creatureBrain.GetProperName(), creatureBrain.GetProperName());
				}
				else
				{
					num2++;
					ReportManager.Instance.ReportValue(ReportManager.ReportType.DomesticatedCritters, 1f, creatureBrain.GetProperName(), creatureBrain.GetProperName());
				}
			}
		}
		if (DlcManager.IsExpansion1Active())
		{
			WorldContainer component = base.GetComponent<WorldContainer>();
			if (component != null && component.IsModuleInterior)
			{
				Clustercraft clustercraft = component.GetComponent<ClusterGridEntity>() as Clustercraft;
				if (clustercraft != null && clustercraft.Status != Clustercraft.CraftStatus.Grounded)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, clustercraft.Name, null);
					return;
				}
			}
		}
		else
		{
			foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
			{
				if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, spacecraft.rocketName, null);
				}
			}
		}
	}

	// Token: 0x06004EAF RID: 20143 RVA: 0x001B95E4 File Offset: 0x001B77E4
	protected override void OnSpawn()
	{
		this.Prober = MinionGroupProber.Get();
		base.StartCoroutine(this.InitialRefresh());
	}

	// Token: 0x06004EB0 RID: 20144 RVA: 0x001B95FE File Offset: 0x001B77FE
	private IEnumerator InitialRefresh()
	{
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		for (int j = 0; j < Components.Pickupables.Count; j++)
		{
			Pickupable pickupable = Components.Pickupables[j];
			if (pickupable != null)
			{
				ReachabilityMonitor.Instance smi = pickupable.GetSMI<ReachabilityMonitor.Instance>();
				if (smi != null)
				{
					smi.UpdateReachability();
				}
			}
		}
		yield break;
	}

	// Token: 0x06004EB1 RID: 20145 RVA: 0x001B9606 File Offset: 0x001B7806
	public bool IsReachable(Pickupable pickupable)
	{
		return this.Prober.IsReachable(pickupable);
	}

	// Token: 0x06004EB2 RID: 20146 RVA: 0x001B9614 File Offset: 0x001B7814
	public float GetTotalAmount(Tag tag, bool includeRelatedWorlds)
	{
		float result = 0f;
		this.accessibleAmounts.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06004EB3 RID: 20147 RVA: 0x001B9638 File Offset: 0x001B7838
	public ICollection<Pickupable> GetPickupables(Tag tag, bool includeRelatedWorlds = false)
	{
		if (!includeRelatedWorlds)
		{
			HashSet<Pickupable> result = null;
			this.Inventory.TryGetValue(tag, out result);
			return result;
		}
		return ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
	}

	// Token: 0x06004EB4 RID: 20148 RVA: 0x001B9664 File Offset: 0x001B7864
	public List<Pickupable> CreatePickupablesList(Tag tag)
	{
		HashSet<Pickupable> hashSet = null;
		this.Inventory.TryGetValue(tag, out hashSet);
		if (hashSet == null)
		{
			return null;
		}
		return hashSet.ToList<Pickupable>();
	}

	// Token: 0x06004EB5 RID: 20149 RVA: 0x001B9690 File Offset: 0x001B7890
	public float GetAmount(Tag tag, bool includeRelatedWorlds)
	{
		float num;
		if (!includeRelatedWorlds)
		{
			num = this.GetTotalAmount(tag, includeRelatedWorlds);
			num -= MaterialNeeds.GetAmount(tag, this.worldId, includeRelatedWorlds);
		}
		else
		{
			num = ClusterUtil.GetAmountFromRelatedWorlds(this, tag);
		}
		return Mathf.Max(num, 0f);
	}

	// Token: 0x06004EB6 RID: 20150 RVA: 0x001B96D8 File Offset: 0x001B78D8
	public int GetCountWithAdditionalTag(Tag tag, Tag additionalTag, bool includeRelatedWorlds = false)
	{
		ICollection<Pickupable> collection;
		if (!includeRelatedWorlds)
		{
			collection = this.GetPickupables(tag, false);
		}
		else
		{
			ICollection<Pickupable> pickupablesFromRelatedWorlds = ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
			collection = pickupablesFromRelatedWorlds;
		}
		ICollection<Pickupable> collection2 = collection;
		int num = 0;
		if (collection2 != null)
		{
			if (additionalTag.IsValid)
			{
				using (IEnumerator<Pickupable> enumerator = collection2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasTag(additionalTag))
						{
							num++;
						}
					}
					return num;
				}
			}
			num = collection2.Count;
		}
		return num;
	}

	// Token: 0x06004EB7 RID: 20151 RVA: 0x001B9754 File Offset: 0x001B7954
	public float GetAmountWithoutTag(Tag tag, bool includeRelatedWorlds = false, Tag[] forbiddenTags = null)
	{
		if (forbiddenTags == null)
		{
			return this.GetAmount(tag, includeRelatedWorlds);
		}
		float num = 0f;
		ICollection<Pickupable> collection;
		if (!includeRelatedWorlds)
		{
			collection = this.GetPickupables(tag, false);
		}
		else
		{
			ICollection<Pickupable> pickupablesFromRelatedWorlds = ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
			collection = pickupablesFromRelatedWorlds;
		}
		ICollection<Pickupable> collection2 = collection;
		if (collection2 != null)
		{
			foreach (Pickupable pickupable in collection2)
			{
				if (pickupable != null && !pickupable.HasTag(GameTags.StoredPrivate) && !pickupable.HasAnyTags(forbiddenTags))
				{
					num += pickupable.TotalAmount;
				}
			}
		}
		return num;
	}

	// Token: 0x06004EB8 RID: 20152 RVA: 0x001B97F0 File Offset: 0x001B79F0
	private void Update()
	{
		int num = 0;
		Dictionary<Tag, HashSet<Pickupable>>.Enumerator enumerator = this.Inventory.GetEnumerator();
		int worldId = this.worldId;
		while (enumerator.MoveNext())
		{
			KeyValuePair<Tag, HashSet<Pickupable>> keyValuePair = enumerator.Current;
			if (num == this.accessibleUpdateIndex || this.firstUpdate)
			{
				Tag key = keyValuePair.Key;
				IEnumerable<Pickupable> value = keyValuePair.Value;
				float num2 = 0f;
				foreach (Pickupable pickupable in value)
				{
					if (pickupable != null && pickupable.GetMyWorldId() == worldId && !pickupable.HasTag(GameTags.StoredPrivate))
					{
						num2 += pickupable.TotalAmount;
					}
				}
				if (!this.hasValidCount && this.accessibleUpdateIndex + 1 >= this.Inventory.Count)
				{
					this.hasValidCount = true;
					if (this.worldId == ClusterManager.Instance.activeWorldId)
					{
						this.hasValidCount = true;
						PinnedResourcesPanel.Instance.ClearExcessiveNewItems();
						PinnedResourcesPanel.Instance.Refresh();
					}
				}
				this.accessibleAmounts[key] = num2;
				this.accessibleUpdateIndex = (this.accessibleUpdateIndex + 1) % this.Inventory.Count;
				break;
			}
			num++;
		}
		this.firstUpdate = false;
	}

	// Token: 0x06004EB9 RID: 20153 RVA: 0x001B9944 File Offset: 0x001B7B44
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x06004EBA RID: 20154 RVA: 0x001B994C File Offset: 0x001B7B4C
	private void OnAddedFetchable(object data)
	{
		GameObject gameObject = (GameObject)data;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		if (component.HasAnyTags(WorldInventory.NonCritterEntitiesTags))
		{
			return;
		}
		Pickupable component2 = gameObject.GetComponent<Pickupable>();
		if (component2.GetMyWorldId() != this.worldId)
		{
			return;
		}
		Tag tag = component.PrefabID();
		if (!this.Inventory.ContainsKey(tag))
		{
			Tag categoryForEntity = DiscoveredResources.GetCategoryForEntity(component);
			DebugUtil.DevAssertArgs(categoryForEntity.IsValid, new object[]
			{
				component2.name,
				"was found by worldinventory but doesn't have a category! Add it to the element definition."
			});
			DiscoveredResources.Instance.Discover(tag, categoryForEntity);
		}
		HashSet<Pickupable> hashSet;
		if (!this.Inventory.TryGetValue(tag, out hashSet))
		{
			hashSet = new HashSet<Pickupable>();
			this.Inventory[tag] = hashSet;
		}
		hashSet.Add(component2);
		foreach (Tag key in component.Tags)
		{
			if (!this.Inventory.TryGetValue(key, out hashSet))
			{
				hashSet = new HashSet<Pickupable>();
				this.Inventory[key] = hashSet;
			}
			hashSet.Add(component2);
		}
	}

	// Token: 0x06004EBB RID: 20155 RVA: 0x001B9A78 File Offset: 0x001B7C78
	private void OnRemovedFetchable(object data)
	{
		Pickupable component = ((GameObject)data).GetComponent<Pickupable>();
		KPrefabID kprefabID = component.KPrefabID;
		HashSet<Pickupable> hashSet;
		if (this.Inventory.TryGetValue(kprefabID.PrefabTag, out hashSet))
		{
			hashSet.Remove(component);
		}
		foreach (Tag key in kprefabID.Tags)
		{
			if (this.Inventory.TryGetValue(key, out hashSet))
			{
				hashSet.Remove(component);
			}
		}
	}

	// Token: 0x06004EBC RID: 20156 RVA: 0x001B9B10 File Offset: 0x001B7D10
	public Dictionary<Tag, float> GetAccessibleAmounts()
	{
		return this.accessibleAmounts;
	}

	// Token: 0x0400332C RID: 13100
	private WorldContainer m_worldContainer;

	// Token: 0x0400332D RID: 13101
	[Serialize]
	public List<Tag> pinnedResources = new List<Tag>();

	// Token: 0x0400332E RID: 13102
	[Serialize]
	public List<Tag> notifyResources = new List<Tag>();

	// Token: 0x0400332F RID: 13103
	private Dictionary<Tag, HashSet<Pickupable>> Inventory = new Dictionary<Tag, HashSet<Pickupable>>();

	// Token: 0x04003330 RID: 13104
	private MinionGroupProber Prober;

	// Token: 0x04003331 RID: 13105
	private Dictionary<Tag, float> accessibleAmounts = new Dictionary<Tag, float>();

	// Token: 0x04003332 RID: 13106
	private bool hasValidCount;

	// Token: 0x04003333 RID: 13107
	private static readonly EventSystem.IntraObjectHandler<WorldInventory> OnNewDayDelegate = new EventSystem.IntraObjectHandler<WorldInventory>(delegate(WorldInventory component, object data)
	{
		component.GenerateInventoryReport(data);
	});

	// Token: 0x04003334 RID: 13108
	private int accessibleUpdateIndex;

	// Token: 0x04003335 RID: 13109
	private bool firstUpdate = true;

	// Token: 0x04003336 RID: 13110
	private static Tag[] NonCritterEntitiesTags = new Tag[]
	{
		GameTags.DupeBrain,
		GameTags.Robot
	};
}
