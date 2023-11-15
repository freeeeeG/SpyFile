using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AAF RID: 2735
public class BuildingChoresPanel : TargetScreen
{
	// Token: 0x0600539A RID: 21402 RVA: 0x001E1C54 File Offset: 0x001DFE54
	public override bool IsValidForTarget(GameObject target)
	{
		KPrefabID component = target.GetComponent<KPrefabID>();
		return component != null && component.HasTag(GameTags.HasChores) && !component.IsPrefabID(GameTags.Minion);
	}

	// Token: 0x0600539B RID: 21403 RVA: 0x001E1C8E File Offset: 0x001DFE8E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreGroup = Util.KInstantiateUI<HierarchyReferences>(this.choreGroupPrefab, base.gameObject, false);
		this.choreGroup.gameObject.SetActive(true);
	}

	// Token: 0x0600539C RID: 21404 RVA: 0x001E1CBF File Offset: 0x001DFEBF
	private void Update()
	{
		this.Refresh();
	}

	// Token: 0x0600539D RID: 21405 RVA: 0x001E1CC7 File Offset: 0x001DFEC7
	public override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		this.Refresh();
	}

	// Token: 0x0600539E RID: 21406 RVA: 0x001E1CD6 File Offset: 0x001DFED6
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
	}

	// Token: 0x0600539F RID: 21407 RVA: 0x001E1CDF File Offset: 0x001DFEDF
	private void Refresh()
	{
		this.RefreshDetails();
	}

	// Token: 0x060053A0 RID: 21408 RVA: 0x001E1CE8 File Offset: 0x001DFEE8
	private void RefreshDetails()
	{
		int myParentWorldId = this.selectedTarget.GetMyParentWorldId();
		List<Chore> list = null;
		GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(myParentWorldId, out list);
		int num = 0;
		while (list != null && num < list.Count)
		{
			Chore chore = list[num];
			if (!chore.isNull && chore.gameObject == this.selectedTarget)
			{
				this.AddChoreEntry(chore);
			}
			num++;
		}
		List<FetchChore> list2 = null;
		GlobalChoreProvider.Instance.fetchMap.TryGetValue(myParentWorldId, out list2);
		int num2 = 0;
		while (list2 != null && num2 < list2.Count)
		{
			FetchChore fetchChore = list2[num2];
			if (!fetchChore.isNull && fetchChore.gameObject == this.selectedTarget)
			{
				this.AddChoreEntry(fetchChore);
			}
			num2++;
		}
		for (int i = this.activeDupeEntries; i < this.dupeEntries.Count; i++)
		{
			this.dupeEntries[i].gameObject.SetActive(false);
		}
		this.activeDupeEntries = 0;
		for (int j = this.activeChoreEntries; j < this.choreEntries.Count; j++)
		{
			this.choreEntries[j].gameObject.SetActive(false);
		}
		this.activeChoreEntries = 0;
	}

	// Token: 0x060053A1 RID: 21409 RVA: 0x001E1E30 File Offset: 0x001E0030
	private void AddChoreEntry(Chore chore)
	{
		HierarchyReferences choreEntry = this.GetChoreEntry(GameUtil.GetChoreName(chore, null), chore.choreType, this.choreGroup.GetReference<RectTransform>("EntriesContainer"));
		FetchChore fetchChore = chore as FetchChore;
		ListPool<Chore.Precondition.Context, BuildingChoresPanel>.PooledList pooledList = ListPool<Chore.Precondition.Context, BuildingChoresPanel>.Allocate();
		foreach (Component component in Components.LiveMinionIdentities.Items)
		{
			pooledList.Clear();
			ChoreConsumer component2 = component.GetComponent<ChoreConsumer>();
			Chore.Precondition.Context context = default(Chore.Precondition.Context);
			ChoreConsumer.PreconditionSnapshot lastPreconditionSnapshot = component2.GetLastPreconditionSnapshot();
			if (lastPreconditionSnapshot.doFailedContextsNeedSorting)
			{
				lastPreconditionSnapshot.failedContexts.Sort();
				lastPreconditionSnapshot.doFailedContextsNeedSorting = false;
			}
			pooledList.AddRange(lastPreconditionSnapshot.failedContexts);
			pooledList.AddRange(lastPreconditionSnapshot.succeededContexts);
			int num = -1;
			int num2 = 0;
			for (int i = pooledList.Count - 1; i >= 0; i--)
			{
				if (!(pooledList[i].chore.driver != null) || !(pooledList[i].chore.driver != component2.choreDriver))
				{
					bool flag = pooledList[i].IsPotentialSuccess();
					if (flag)
					{
						num2++;
					}
					FetchAreaChore fetchAreaChore = pooledList[i].chore as FetchAreaChore;
					if (pooledList[i].chore == chore || (fetchChore != null && fetchAreaChore != null && fetchAreaChore.smi.SameDestination(fetchChore)))
					{
						num = (flag ? num2 : int.MaxValue);
						context = pooledList[i];
						break;
					}
				}
			}
			if (num >= 0)
			{
				this.DupeEntryDatas.Add(new BuildingChoresPanel.DupeEntryData
				{
					consumer = component2,
					context = context,
					personalPriority = component2.GetPersonalPriority(chore.choreType),
					rank = num
				});
			}
		}
		pooledList.Recycle();
		this.DupeEntryDatas.Sort();
		foreach (BuildingChoresPanel.DupeEntryData data in this.DupeEntryDatas)
		{
			this.GetDupeEntry(data, choreEntry.GetReference<RectTransform>("DupeContainer"));
		}
		this.DupeEntryDatas.Clear();
	}

	// Token: 0x060053A2 RID: 21410 RVA: 0x001E20A0 File Offset: 0x001E02A0
	private HierarchyReferences GetChoreEntry(string label, ChoreType choreType, RectTransform parent)
	{
		HierarchyReferences hierarchyReferences;
		if (this.activeChoreEntries >= this.choreEntries.Count)
		{
			hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.chorePrefab, parent.gameObject, false);
			this.choreEntries.Add(hierarchyReferences);
		}
		else
		{
			hierarchyReferences = this.choreEntries[this.activeChoreEntries];
			hierarchyReferences.transform.SetParent(parent);
			hierarchyReferences.transform.SetAsLastSibling();
		}
		this.activeChoreEntries++;
		hierarchyReferences.GetReference<LocText>("ChoreLabel").text = label;
		hierarchyReferences.GetReference<LocText>("ChoreSubLabel").text = GameUtil.ChoreGroupsForChoreType(choreType);
		Image reference = hierarchyReferences.GetReference<Image>("Icon");
		if (choreType.groups.Length != 0)
		{
			Sprite sprite = Assets.GetSprite(choreType.groups[0].sprite);
			reference.sprite = sprite;
			reference.gameObject.SetActive(true);
			reference.GetComponent<ToolTip>().toolTip = string.Format(UI.DETAILTABS.BUILDING_CHORES.CHORE_TYPE_TOOLTIP, choreType.groups[0].Name);
		}
		else
		{
			reference.gameObject.SetActive(false);
		}
		Image reference2 = hierarchyReferences.GetReference<Image>("Icon2");
		if (choreType.groups.Length > 1)
		{
			Sprite sprite2 = Assets.GetSprite(choreType.groups[1].sprite);
			reference2.sprite = sprite2;
			reference2.gameObject.SetActive(true);
			reference2.GetComponent<ToolTip>().toolTip = string.Format(UI.DETAILTABS.BUILDING_CHORES.CHORE_TYPE_TOOLTIP, choreType.groups[1].Name);
		}
		else
		{
			reference2.gameObject.SetActive(false);
		}
		hierarchyReferences.gameObject.SetActive(true);
		return hierarchyReferences;
	}

	// Token: 0x060053A3 RID: 21411 RVA: 0x001E223C File Offset: 0x001E043C
	private BuildingChoresPanelDupeRow GetDupeEntry(BuildingChoresPanel.DupeEntryData data, RectTransform parent)
	{
		BuildingChoresPanelDupeRow buildingChoresPanelDupeRow;
		if (this.activeDupeEntries >= this.dupeEntries.Count)
		{
			buildingChoresPanelDupeRow = Util.KInstantiateUI<BuildingChoresPanelDupeRow>(this.dupePrefab.gameObject, parent.gameObject, false);
			this.dupeEntries.Add(buildingChoresPanelDupeRow);
		}
		else
		{
			buildingChoresPanelDupeRow = this.dupeEntries[this.activeDupeEntries];
			buildingChoresPanelDupeRow.transform.SetParent(parent);
			buildingChoresPanelDupeRow.transform.SetAsLastSibling();
		}
		this.activeDupeEntries++;
		buildingChoresPanelDupeRow.Init(data);
		buildingChoresPanelDupeRow.gameObject.SetActive(true);
		return buildingChoresPanelDupeRow;
	}

	// Token: 0x040037DC RID: 14300
	public GameObject choreGroupPrefab;

	// Token: 0x040037DD RID: 14301
	public GameObject chorePrefab;

	// Token: 0x040037DE RID: 14302
	public BuildingChoresPanelDupeRow dupePrefab;

	// Token: 0x040037DF RID: 14303
	private GameObject detailsPanel;

	// Token: 0x040037E0 RID: 14304
	private DetailsPanelDrawer drawer;

	// Token: 0x040037E1 RID: 14305
	private HierarchyReferences choreGroup;

	// Token: 0x040037E2 RID: 14306
	private List<HierarchyReferences> choreEntries = new List<HierarchyReferences>();

	// Token: 0x040037E3 RID: 14307
	private int activeChoreEntries;

	// Token: 0x040037E4 RID: 14308
	private List<BuildingChoresPanelDupeRow> dupeEntries = new List<BuildingChoresPanelDupeRow>();

	// Token: 0x040037E5 RID: 14309
	private int activeDupeEntries;

	// Token: 0x040037E6 RID: 14310
	private List<BuildingChoresPanel.DupeEntryData> DupeEntryDatas = new List<BuildingChoresPanel.DupeEntryData>();

	// Token: 0x020019C5 RID: 6597
	public class DupeEntryData : IComparable<BuildingChoresPanel.DupeEntryData>
	{
		// Token: 0x06009540 RID: 38208 RVA: 0x003396A0 File Offset: 0x003378A0
		public int CompareTo(BuildingChoresPanel.DupeEntryData other)
		{
			if (this.personalPriority != other.personalPriority)
			{
				return other.personalPriority.CompareTo(this.personalPriority);
			}
			if (this.rank != other.rank)
			{
				return this.rank.CompareTo(other.rank);
			}
			if (this.consumer.GetProperName() != other.consumer.GetProperName())
			{
				return this.consumer.GetProperName().CompareTo(other.consumer.GetProperName());
			}
			return this.consumer.GetInstanceID().CompareTo(other.consumer.GetInstanceID());
		}

		// Token: 0x0400774B RID: 30539
		public ChoreConsumer consumer;

		// Token: 0x0400774C RID: 30540
		public Chore.Precondition.Context context;

		// Token: 0x0400774D RID: 30541
		public int personalPriority;

		// Token: 0x0400774E RID: 30542
		public int rank;
	}
}
