using System;
using System.Collections.Generic;
using System.Diagnostics;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020003D5 RID: 981
[AddComponentMenu("KMonoBehaviour/scripts/ChoreConsumer")]
public class ChoreConsumer : KMonoBehaviour, IPersonalPriorityManager
{
	// Token: 0x0600146B RID: 5227 RVA: 0x0006C1B1 File Offset: 0x0006A3B1
	public List<ChoreProvider> GetProviders()
	{
		return this.providers;
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x0006C1B9 File Offset: 0x0006A3B9
	public ChoreConsumer.PreconditionSnapshot GetLastPreconditionSnapshot()
	{
		return this.preconditionSnapshot;
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x0006C1C1 File Offset: 0x0006A3C1
	public List<Chore.Precondition.Context> GetSuceededPreconditionContexts()
	{
		return this.lastSuccessfulPreconditionSnapshot.succeededContexts;
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x0006C1CE File Offset: 0x0006A3CE
	public List<Chore.Precondition.Context> GetFailedPreconditionContexts()
	{
		return this.lastSuccessfulPreconditionSnapshot.failedContexts;
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x0006C1DB File Offset: 0x0006A3DB
	public ChoreConsumer.PreconditionSnapshot GetLastSuccessfulPreconditionSnapshot()
	{
		return this.lastSuccessfulPreconditionSnapshot;
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x0006C1E4 File Offset: 0x0006A3E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (ChoreGroupManager.instance != null)
		{
			foreach (KeyValuePair<Tag, int> keyValuePair in ChoreGroupManager.instance.DefaultChorePermission)
			{
				bool flag = false;
				foreach (HashedString hashedString in this.userDisabledChoreGroups)
				{
					if (hashedString.HashValue == keyValuePair.Key.GetHashCode())
					{
						flag = true;
						break;
					}
				}
				if (!flag && keyValuePair.Value == 0)
				{
					this.userDisabledChoreGroups.Add(new HashedString(keyValuePair.Key.GetHashCode()));
				}
			}
		}
		this.providers.Add(this.choreProvider);
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x0006C2F4 File Offset: 0x0006A4F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (this.choreTable != null)
		{
			this.choreTableInstance = new ChoreTable.Instance(this.choreTable, component);
		}
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			int personalPriority = this.GetPersonalPriority(choreGroup);
			this.UpdateChoreTypePriorities(choreGroup, personalPriority);
			this.SetPermittedByUser(choreGroup, personalPriority != 0);
		}
		this.consumerState = new ChoreConsumerState(this);
	}

	// Token: 0x06001472 RID: 5234 RVA: 0x0006C398 File Offset: 0x0006A598
	protected override void OnForcedCleanUp()
	{
		if (this.consumerState != null)
		{
			this.consumerState.navigator = null;
		}
		this.navigator = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06001473 RID: 5235 RVA: 0x0006C3BB File Offset: 0x0006A5BB
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.choreTableInstance != null)
		{
			this.choreTableInstance.OnCleanUp(base.GetComponent<KPrefabID>());
			this.choreTableInstance = null;
		}
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x0006C3E3 File Offset: 0x0006A5E3
	public bool IsPermittedByUser(ChoreGroup chore_group)
	{
		return chore_group == null || !this.userDisabledChoreGroups.Contains(chore_group.IdHash);
	}

	// Token: 0x06001475 RID: 5237 RVA: 0x0006C400 File Offset: 0x0006A600
	public void SetPermittedByUser(ChoreGroup chore_group, bool is_allowed)
	{
		if (is_allowed)
		{
			if (this.userDisabledChoreGroups.Remove(chore_group.IdHash))
			{
				this.choreRulesChanged.Signal();
				return;
			}
		}
		else if (!this.userDisabledChoreGroups.Contains(chore_group.IdHash))
		{
			this.userDisabledChoreGroups.Add(chore_group.IdHash);
			this.choreRulesChanged.Signal();
		}
	}

	// Token: 0x06001476 RID: 5238 RVA: 0x0006C45E File Offset: 0x0006A65E
	public bool IsPermittedByTraits(ChoreGroup chore_group)
	{
		return chore_group == null || !this.traitDisabledChoreGroups.Contains(chore_group.IdHash);
	}

	// Token: 0x06001477 RID: 5239 RVA: 0x0006C47C File Offset: 0x0006A67C
	public void SetPermittedByTraits(ChoreGroup chore_group, bool is_enabled)
	{
		if (is_enabled)
		{
			if (this.traitDisabledChoreGroups.Remove(chore_group.IdHash))
			{
				this.choreRulesChanged.Signal();
				return;
			}
		}
		else if (!this.traitDisabledChoreGroups.Contains(chore_group.IdHash))
		{
			this.traitDisabledChoreGroups.Add(chore_group.IdHash);
			this.choreRulesChanged.Signal();
		}
	}

	// Token: 0x06001478 RID: 5240 RVA: 0x0006C4DC File Offset: 0x0006A6DC
	private bool ChooseChore(ref Chore.Precondition.Context out_context, List<Chore.Precondition.Context> succeeded_contexts)
	{
		if (succeeded_contexts.Count == 0)
		{
			return false;
		}
		Chore currentChore = this.choreDriver.GetCurrentChore();
		if (currentChore == null)
		{
			for (int i = succeeded_contexts.Count - 1; i >= 0; i--)
			{
				Chore.Precondition.Context context = succeeded_contexts[i];
				if (context.IsSuccess())
				{
					out_context = context;
					return true;
				}
			}
		}
		else
		{
			int interruptPriority = Db.Get().ChoreTypes.TopPriority.interruptPriority;
			int num = (currentChore.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority) ? interruptPriority : currentChore.choreType.interruptPriority;
			for (int j = succeeded_contexts.Count - 1; j >= 0; j--)
			{
				Chore.Precondition.Context context2 = succeeded_contexts[j];
				if (context2.IsSuccess() && ((context2.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority) ? interruptPriority : context2.interruptPriority) > num && !currentChore.choreType.interruptExclusion.Overlaps(context2.chore.choreType.tags))
				{
					out_context = context2;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x0006C5DC File Offset: 0x0006A7DC
	public bool FindNextChore(ref Chore.Precondition.Context out_context)
	{
		this.preconditionSnapshot.Clear();
		this.consumerState.Refresh();
		if (this.consumerState.hasSolidTransferArm)
		{
			global::Debug.Assert(this.stationaryReach > 0);
			CellOffset offset = Grid.GetOffset(Grid.PosToCell(this));
			Extents extents = new Extents(offset.x, offset.y, this.stationaryReach);
			ListPool<ScenePartitionerEntry, ChoreConsumer>.PooledList pooledList = ListPool<ScenePartitionerEntry, ChoreConsumer>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.fetchChoreLayer, pooledList);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				if (scenePartitionerEntry.obj == null)
				{
					DebugUtil.Assert(false, "FindNextChore found an entry that was null");
				}
				else
				{
					FetchChore fetchChore = scenePartitionerEntry.obj as FetchChore;
					if (fetchChore == null)
					{
						DebugUtil.Assert(false, "FindNextChore found an entry that wasn't a FetchChore");
					}
					else if (fetchChore.target == null)
					{
						DebugUtil.Assert(false, "FindNextChore found an entry with a null target");
					}
					else if (fetchChore.isNull)
					{
						global::Debug.LogWarning("FindNextChore found an entry that isNull");
					}
					else
					{
						int cell = Grid.PosToCell(fetchChore.gameObject);
						if (this.consumerState.solidTransferArm.IsCellReachable(cell))
						{
							fetchChore.CollectChoresFromGlobalChoreProvider(this.consumerState, this.preconditionSnapshot.succeededContexts, this.preconditionSnapshot.failedContexts, false);
						}
					}
				}
			}
			pooledList.Recycle();
		}
		else
		{
			for (int i = 0; i < this.providers.Count; i++)
			{
				this.providers[i].CollectChores(this.consumerState, this.preconditionSnapshot.succeededContexts, this.preconditionSnapshot.failedContexts);
			}
		}
		this.preconditionSnapshot.succeededContexts.Sort();
		List<Chore.Precondition.Context> succeededContexts = this.preconditionSnapshot.succeededContexts;
		bool flag = this.ChooseChore(ref out_context, succeededContexts);
		if (flag)
		{
			this.preconditionSnapshot.CopyTo(this.lastSuccessfulPreconditionSnapshot);
		}
		return flag;
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x0006C7D4 File Offset: 0x0006A9D4
	public void AddProvider(ChoreProvider provider)
	{
		DebugUtil.Assert(provider != null);
		this.providers.Add(provider);
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x0006C7EE File Offset: 0x0006A9EE
	public void RemoveProvider(ChoreProvider provider)
	{
		this.providers.Remove(provider);
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x0006C7FD File Offset: 0x0006A9FD
	public void AddUrge(Urge urge)
	{
		DebugUtil.Assert(urge != null);
		this.urges.Add(urge);
		base.Trigger(-736698276, urge);
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x0006C820 File Offset: 0x0006AA20
	public void RemoveUrge(Urge urge)
	{
		this.urges.Remove(urge);
		base.Trigger(231622047, urge);
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x0006C83B File Offset: 0x0006AA3B
	public bool HasUrge(Urge urge)
	{
		return this.urges.Contains(urge);
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x0006C849 File Offset: 0x0006AA49
	public List<Urge> GetUrges()
	{
		return this.urges;
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x0006C851 File Offset: 0x0006AA51
	[Conditional("ENABLE_LOGGER")]
	public void Log(string evt, string param)
	{
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x0006C854 File Offset: 0x0006AA54
	public bool IsPermittedOrEnabled(ChoreType chore_type, Chore chore)
	{
		if (chore_type.groups.Length == 0)
		{
			return true;
		}
		bool flag = false;
		bool flag2 = true;
		for (int i = 0; i < chore_type.groups.Length; i++)
		{
			ChoreGroup chore_group = chore_type.groups[i];
			if (!this.IsPermittedByTraits(chore_group))
			{
				flag2 = false;
			}
			if (this.IsPermittedByUser(chore_group))
			{
				flag = true;
			}
		}
		return flag && flag2;
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x0006C8A5 File Offset: 0x0006AAA5
	public void SetReach(int reach)
	{
		this.stationaryReach = reach;
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x0006C8B0 File Offset: 0x0006AAB0
	public bool GetNavigationCost(IApproachable approachable, out int cost)
	{
		if (this.navigator)
		{
			cost = this.navigator.GetNavigationCost(approachable);
			if (cost != -1)
			{
				return true;
			}
		}
		else if (this.consumerState.hasSolidTransferArm)
		{
			int cell = approachable.GetCell();
			if (this.consumerState.solidTransferArm.IsCellReachable(cell))
			{
				cost = Grid.GetCellRange(this.NaturalBuildingCell(), cell);
				return true;
			}
		}
		cost = 0;
		return false;
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x0006C91C File Offset: 0x0006AB1C
	public bool GetNavigationCost(int cell, out int cost)
	{
		if (this.navigator)
		{
			cost = this.navigator.GetNavigationCost(cell);
			if (cost != -1)
			{
				return true;
			}
		}
		else if (this.consumerState.hasSolidTransferArm && this.consumerState.solidTransferArm.IsCellReachable(cell))
		{
			cost = Grid.GetCellRange(this.NaturalBuildingCell(), cell);
			return true;
		}
		cost = 0;
		return false;
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x0006C980 File Offset: 0x0006AB80
	public bool CanReach(IApproachable approachable)
	{
		if (this.navigator)
		{
			return this.navigator.CanReach(approachable);
		}
		if (this.consumerState.hasSolidTransferArm)
		{
			int cell = approachable.GetCell();
			return this.consumerState.solidTransferArm.IsCellReachable(cell);
		}
		return false;
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x0006C9D0 File Offset: 0x0006ABD0
	public bool IsWithinReach(IApproachable approachable)
	{
		if (this.navigator)
		{
			return !(this == null) && !(base.gameObject == null) && Grid.IsCellOffsetOf(Grid.PosToCell(this), approachable.GetCell(), approachable.GetOffsets());
		}
		return this.consumerState.hasSolidTransferArm && this.consumerState.solidTransferArm.IsCellReachable(approachable.GetCell());
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x0006CA40 File Offset: 0x0006AC40
	public void ShowHoverTextOnHoveredItem(Chore.Precondition.Context context, KSelectable hover_obj, HoverTextDrawer drawer, SelectToolHoverTextCard hover_text_card)
	{
		if (context.chore.target.isNull || context.chore.target.gameObject != hover_obj.gameObject)
		{
			return;
		}
		drawer.NewLine(26);
		drawer.AddIndent(36);
		drawer.DrawText(context.chore.choreType.Name, hover_text_card.Styles_BodyText.Standard);
		if (!context.IsSuccess())
		{
			Chore.PreconditionInstance preconditionInstance = context.chore.GetPreconditions()[context.failedPreconditionId];
			string text = preconditionInstance.description;
			if (string.IsNullOrEmpty(text))
			{
				text = preconditionInstance.id;
			}
			if (context.chore.driver != null)
			{
				text = text.Replace("{Assignee}", context.chore.driver.GetProperName());
			}
			text = text.Replace("{Selected}", this.GetProperName());
			drawer.DrawText(" (" + text + ")", hover_text_card.Styles_BodyText.Standard);
		}
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x0006CB4C File Offset: 0x0006AD4C
	public void ShowHoverTextOnHoveredItem(KSelectable hover_obj, HoverTextDrawer drawer, SelectToolHoverTextCard hover_text_card)
	{
		bool flag = false;
		foreach (Chore.Precondition.Context context in this.preconditionSnapshot.succeededContexts)
		{
			if (context.chore.showAvailabilityInHoverText && !context.chore.target.isNull && !(context.chore.target.gameObject != hover_obj.gameObject))
			{
				if (!flag)
				{
					drawer.NewLine(26);
					drawer.DrawText(DUPLICANTS.CHORES.PRECONDITIONS.HEADER.ToString().Replace("{Selected}", this.GetProperName()), hover_text_card.Styles_BodyText.Standard);
					flag = true;
				}
				this.ShowHoverTextOnHoveredItem(context, hover_obj, drawer, hover_text_card);
			}
		}
		foreach (Chore.Precondition.Context context2 in this.preconditionSnapshot.failedContexts)
		{
			if (context2.chore.showAvailabilityInHoverText && !context2.chore.target.isNull && !(context2.chore.target.gameObject != hover_obj.gameObject))
			{
				if (!flag)
				{
					drawer.NewLine(26);
					drawer.DrawText(DUPLICANTS.CHORES.PRECONDITIONS.HEADER.ToString().Replace("{Selected}", this.GetProperName()), hover_text_card.Styles_BodyText.Standard);
					flag = true;
				}
				this.ShowHoverTextOnHoveredItem(context2, hover_obj, drawer, hover_text_card);
			}
		}
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x0006CCE8 File Offset: 0x0006AEE8
	public int GetPersonalPriority(ChoreType chore_type)
	{
		int num;
		if (!this.choreTypePriorities.TryGetValue(chore_type.IdHash, out num))
		{
			num = 3;
		}
		num = Mathf.Clamp(num, 0, 5);
		return num;
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x0006CD18 File Offset: 0x0006AF18
	public int GetPersonalPriority(ChoreGroup group)
	{
		int value = 3;
		ChoreConsumer.PriorityInfo priorityInfo;
		if (this.choreGroupPriorities.TryGetValue(group.IdHash, out priorityInfo))
		{
			value = priorityInfo.priority;
		}
		return Mathf.Clamp(value, 0, 5);
	}

	// Token: 0x0600148B RID: 5259 RVA: 0x0006CD50 File Offset: 0x0006AF50
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
		if (group.choreTypes == null)
		{
			return;
		}
		value = Mathf.Clamp(value, 0, 5);
		ChoreConsumer.PriorityInfo priorityInfo;
		if (!this.choreGroupPriorities.TryGetValue(group.IdHash, out priorityInfo))
		{
			priorityInfo.priority = 3;
		}
		this.choreGroupPriorities[group.IdHash] = new ChoreConsumer.PriorityInfo
		{
			priority = value
		};
		this.UpdateChoreTypePriorities(group, value);
		this.SetPermittedByUser(group, value != 0);
	}

	// Token: 0x0600148C RID: 5260 RVA: 0x0006CDC2 File Offset: 0x0006AFC2
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return (int)this.GetAttributes().GetValue(group.attribute.Id);
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x0006CDDC File Offset: 0x0006AFDC
	private void UpdateChoreTypePriorities(ChoreGroup group, int value)
	{
		ChoreGroups choreGroups = Db.Get().ChoreGroups;
		foreach (ChoreType choreType in group.choreTypes)
		{
			int num = 0;
			foreach (ChoreGroup choreGroup in choreGroups.resources)
			{
				if (choreGroup.choreTypes != null)
				{
					using (List<ChoreType>.Enumerator enumerator3 = choreGroup.choreTypes.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.IdHash == choreType.IdHash)
							{
								int personalPriority = this.GetPersonalPriority(choreGroup);
								num = Mathf.Max(num, personalPriority);
							}
						}
					}
				}
			}
			this.choreTypePriorities[choreType.IdHash] = num;
		}
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x0006CEF4 File Offset: 0x0006B0F4
	public void ResetPersonalPriorities()
	{
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x0006CEF8 File Offset: 0x0006B0F8
	public bool RunBehaviourPrecondition(Tag tag)
	{
		ChoreConsumer.BehaviourPrecondition behaviourPrecondition = default(ChoreConsumer.BehaviourPrecondition);
		return this.behaviourPreconditions.TryGetValue(tag, out behaviourPrecondition) && behaviourPrecondition.cb(behaviourPrecondition.arg);
	}

	// Token: 0x06001490 RID: 5264 RVA: 0x0006CF30 File Offset: 0x0006B130
	public void AddBehaviourPrecondition(Tag tag, Func<object, bool> precondition, object arg)
	{
		DebugUtil.Assert(!this.behaviourPreconditions.ContainsKey(tag));
		this.behaviourPreconditions[tag] = new ChoreConsumer.BehaviourPrecondition
		{
			cb = precondition,
			arg = arg
		};
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x0006CF76 File Offset: 0x0006B176
	public void RemoveBehaviourPrecondition(Tag tag, Func<object, bool> precondition, object arg)
	{
		this.behaviourPreconditions.Remove(tag);
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x0006CF88 File Offset: 0x0006B188
	public bool IsChoreEqualOrAboveCurrentChorePriority<StateMachineType>()
	{
		Chore currentChore = this.choreDriver.GetCurrentChore();
		return currentChore == null || currentChore.choreType.priority <= this.choreTable.GetChorePriority<StateMachineType>(this);
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x0006CFC4 File Offset: 0x0006B1C4
	public bool IsChoreGroupDisabled(ChoreGroup chore_group)
	{
		bool result = false;
		Traits component = base.gameObject.GetComponent<Traits>();
		if (component != null && component.IsChoreGroupDisabled(chore_group))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x0006CFF4 File Offset: 0x0006B1F4
	public Dictionary<HashedString, ChoreConsumer.PriorityInfo> GetChoreGroupPriorities()
	{
		return this.choreGroupPriorities;
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x0006CFFC File Offset: 0x0006B1FC
	public void SetChoreGroupPriorities(Dictionary<HashedString, ChoreConsumer.PriorityInfo> priorities)
	{
		this.choreGroupPriorities = priorities;
	}

	// Token: 0x04000B0D RID: 2829
	public const int DEFAULT_PERSONAL_CHORE_PRIORITY = 3;

	// Token: 0x04000B0E RID: 2830
	public const int MIN_PERSONAL_PRIORITY = 0;

	// Token: 0x04000B0F RID: 2831
	public const int MAX_PERSONAL_PRIORITY = 5;

	// Token: 0x04000B10 RID: 2832
	public const int PRIORITY_DISABLED = 0;

	// Token: 0x04000B11 RID: 2833
	public const int PRIORITY_VERYLOW = 1;

	// Token: 0x04000B12 RID: 2834
	public const int PRIORITY_LOW = 2;

	// Token: 0x04000B13 RID: 2835
	public const int PRIORITY_FLAT = 3;

	// Token: 0x04000B14 RID: 2836
	public const int PRIORITY_HIGH = 4;

	// Token: 0x04000B15 RID: 2837
	public const int PRIORITY_VERYHIGH = 5;

	// Token: 0x04000B16 RID: 2838
	[MyCmpAdd]
	public ChoreProvider choreProvider;

	// Token: 0x04000B17 RID: 2839
	[MyCmpAdd]
	public ChoreDriver choreDriver;

	// Token: 0x04000B18 RID: 2840
	[MyCmpGet]
	public Navigator navigator;

	// Token: 0x04000B19 RID: 2841
	[MyCmpGet]
	public MinionResume resume;

	// Token: 0x04000B1A RID: 2842
	[MyCmpAdd]
	private User user;

	// Token: 0x04000B1B RID: 2843
	public System.Action choreRulesChanged;

	// Token: 0x04000B1C RID: 2844
	private List<ChoreProvider> providers = new List<ChoreProvider>();

	// Token: 0x04000B1D RID: 2845
	private List<Urge> urges = new List<Urge>();

	// Token: 0x04000B1E RID: 2846
	public ChoreTable choreTable;

	// Token: 0x04000B1F RID: 2847
	private ChoreTable.Instance choreTableInstance;

	// Token: 0x04000B20 RID: 2848
	public ChoreConsumerState consumerState;

	// Token: 0x04000B21 RID: 2849
	private Dictionary<Tag, ChoreConsumer.BehaviourPrecondition> behaviourPreconditions = new Dictionary<Tag, ChoreConsumer.BehaviourPrecondition>();

	// Token: 0x04000B22 RID: 2850
	private ChoreConsumer.PreconditionSnapshot preconditionSnapshot = new ChoreConsumer.PreconditionSnapshot();

	// Token: 0x04000B23 RID: 2851
	private ChoreConsumer.PreconditionSnapshot lastSuccessfulPreconditionSnapshot = new ChoreConsumer.PreconditionSnapshot();

	// Token: 0x04000B24 RID: 2852
	[Serialize]
	private Dictionary<HashedString, ChoreConsumer.PriorityInfo> choreGroupPriorities = new Dictionary<HashedString, ChoreConsumer.PriorityInfo>();

	// Token: 0x04000B25 RID: 2853
	private Dictionary<HashedString, int> choreTypePriorities = new Dictionary<HashedString, int>();

	// Token: 0x04000B26 RID: 2854
	private List<HashedString> traitDisabledChoreGroups = new List<HashedString>();

	// Token: 0x04000B27 RID: 2855
	private List<HashedString> userDisabledChoreGroups = new List<HashedString>();

	// Token: 0x04000B28 RID: 2856
	private int stationaryReach = -1;

	// Token: 0x02001042 RID: 4162
	private struct BehaviourPrecondition
	{
		// Token: 0x0400589D RID: 22685
		public Func<object, bool> cb;

		// Token: 0x0400589E RID: 22686
		public object arg;
	}

	// Token: 0x02001043 RID: 4163
	public class PreconditionSnapshot
	{
		// Token: 0x06007526 RID: 29990 RVA: 0x002CC2F1 File Offset: 0x002CA4F1
		public void CopyTo(ChoreConsumer.PreconditionSnapshot snapshot)
		{
			snapshot.Clear();
			snapshot.succeededContexts.AddRange(this.succeededContexts);
			snapshot.failedContexts.AddRange(this.failedContexts);
			snapshot.doFailedContextsNeedSorting = true;
		}

		// Token: 0x06007527 RID: 29991 RVA: 0x002CC322 File Offset: 0x002CA522
		public void Clear()
		{
			this.succeededContexts.Clear();
			this.failedContexts.Clear();
			this.doFailedContextsNeedSorting = true;
		}

		// Token: 0x0400589F RID: 22687
		public List<Chore.Precondition.Context> succeededContexts = new List<Chore.Precondition.Context>();

		// Token: 0x040058A0 RID: 22688
		public List<Chore.Precondition.Context> failedContexts = new List<Chore.Precondition.Context>();

		// Token: 0x040058A1 RID: 22689
		public bool doFailedContextsNeedSorting = true;
	}

	// Token: 0x02001044 RID: 4164
	public struct PriorityInfo
	{
		// Token: 0x040058A2 RID: 22690
		public int priority;
	}
}
