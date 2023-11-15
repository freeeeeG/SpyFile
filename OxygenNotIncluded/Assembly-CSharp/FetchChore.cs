using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x020003B3 RID: 947
public class FetchChore : Chore<FetchChore.StatesInstance>
{
	// Token: 0x17000067 RID: 103
	// (get) Token: 0x060013B5 RID: 5045 RVA: 0x00068C40 File Offset: 0x00066E40
	public float originalAmount
	{
		get
		{
			return base.smi.sm.requestedamount.Get(base.smi);
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x060013B6 RID: 5046 RVA: 0x00068C5D File Offset: 0x00066E5D
	// (set) Token: 0x060013B7 RID: 5047 RVA: 0x00068C7A File Offset: 0x00066E7A
	public float amount
	{
		get
		{
			return base.smi.sm.actualamount.Get(base.smi);
		}
		set
		{
			base.smi.sm.actualamount.Set(value, base.smi, false);
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x060013B8 RID: 5048 RVA: 0x00068C9A File Offset: 0x00066E9A
	// (set) Token: 0x060013B9 RID: 5049 RVA: 0x00068CB7 File Offset: 0x00066EB7
	public Pickupable fetchTarget
	{
		get
		{
			return base.smi.sm.chunk.Get<Pickupable>(base.smi);
		}
		set
		{
			base.smi.sm.chunk.Set(value, base.smi);
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060013BA RID: 5050 RVA: 0x00068CD5 File Offset: 0x00066ED5
	// (set) Token: 0x060013BB RID: 5051 RVA: 0x00068CF2 File Offset: 0x00066EF2
	public GameObject fetcher
	{
		get
		{
			return base.smi.sm.fetcher.Get(base.smi);
		}
		set
		{
			base.smi.sm.fetcher.Set(value, base.smi, false);
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060013BC RID: 5052 RVA: 0x00068D12 File Offset: 0x00066F12
	// (set) Token: 0x060013BD RID: 5053 RVA: 0x00068D1A File Offset: 0x00066F1A
	public Storage destination { get; private set; }

	// Token: 0x060013BE RID: 5054 RVA: 0x00068D24 File Offset: 0x00066F24
	public void FetchAreaBegin(Chore.Precondition.Context context, float amount_to_be_fetched)
	{
		this.amount = amount_to_be_fetched;
		base.smi.sm.fetcher.Set(context.consumerState.gameObject, base.smi, false);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, context.chore.choreType.Name, GameUtil.GetChoreName(this, context.data));
		base.Begin(context);
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x00068D94 File Offset: 0x00066F94
	public void FetchAreaEnd(ChoreDriver driver, Pickupable pickupable, bool is_success)
	{
		if (is_success)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, base.choreType.Name, GameUtil.GetChoreName(this, pickupable));
			this.fetchTarget = pickupable;
			base.driver = driver;
			this.fetcher = driver.gameObject;
			base.Succeed("FetchAreaEnd");
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().LogFetchChore(this.fetcher, base.choreType);
			return;
		}
		base.SetOverrideTarget(null);
		this.Fail("FetchAreaFail");
	}

	// Token: 0x060013C0 RID: 5056 RVA: 0x00068E1C File Offset: 0x0006701C
	public Pickupable FindFetchTarget(ChoreConsumerState consumer_state)
	{
		if (!(this.destination != null))
		{
			return null;
		}
		if (consumer_state.hasSolidTransferArm)
		{
			return consumer_state.solidTransferArm.FindFetchTarget(this.destination, this);
		}
		return Game.Instance.fetchManager.FindFetchTarget(this.destination, this);
	}

	// Token: 0x060013C1 RID: 5057 RVA: 0x00068E6C File Offset: 0x0006706C
	public override void Begin(Chore.Precondition.Context context)
	{
		Pickupable pickupable = (Pickupable)context.data;
		if (pickupable == null)
		{
			pickupable = this.FindFetchTarget(context.consumerState);
		}
		base.smi.sm.source.Set(pickupable.gameObject, base.smi, false);
		pickupable.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		base.Begin(context);
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x00068EE0 File Offset: 0x000670E0
	protected override void End(string reason)
	{
		Pickupable pickupable = base.smi.sm.source.Get<Pickupable>(base.smi);
		if (pickupable != null)
		{
			pickupable.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		}
		base.End(reason);
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x00068F30 File Offset: 0x00067130
	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.chunk.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x00068F60 File Offset: 0x00067160
	public override void PrepareChore(ref Chore.Precondition.Context context)
	{
		context.chore = new FetchAreaChore(context);
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x00068F73 File Offset: 0x00067173
	public float AmountWaitingToFetch()
	{
		if (this.fetcher == null)
		{
			return this.originalAmount;
		}
		return this.amount;
	}

	// Token: 0x060013C6 RID: 5062 RVA: 0x00068F90 File Offset: 0x00067190
	public FetchChore(ChoreType choreType, Storage destination, float amount, HashSet<Tag> tags, FetchChore.MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags = null, ChoreProvider chore_provider = null, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, Operational.State operational_requirement = Operational.State.Operational, int priority_mod = 0) : base(choreType, destination, chore_provider, run_until_complete, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.basic, 5, false, true, priority_mod, false, ReportManager.ReportType.WorkTime)
	{
		if (choreType == null)
		{
			global::Debug.LogError("You must specify a chore type for fetching!");
		}
		this.tagsFirst = ((tags.Count > 0) ? tags.First<Tag>() : Tag.Invalid);
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("Chore {0} is requesting {1} {2} to {3}", new object[]
				{
					choreType.Id,
					this.tagsFirst,
					amount,
					(destination != null) ? destination.name : "to nowhere"
				})
			});
		}
		base.SetPrioritizable((destination.prioritizable != null) ? destination.prioritizable : destination.GetComponent<Prioritizable>());
		base.smi = new FetchChore.StatesInstance(this);
		base.smi.sm.requestedamount.Set(amount, base.smi, false);
		this.destination = destination;
		DebugUtil.DevAssert(criteria != FetchChore.MatchCriteria.MatchTags || tags.Count <= 1, "For performance reasons fetch chores are limited to one tag when matching tags!", null);
		this.tags = tags;
		this.criteria = criteria;
		this.tagsHash = FetchChore.ComputeHashCodeForTags(tags);
		this.requiredTag = required_tag;
		this.forbiddenTags = ((forbidden_tags != null) ? forbidden_tags : new Tag[0]);
		this.forbidHash = FetchChore.ComputeHashCodeForTags(this.forbiddenTags);
		DebugUtil.DevAssert(!tags.Contains(GameTags.Preserved), "Fetch chore fetching invalid tags.", null);
		if (destination.GetOnlyFetchMarkedItems())
		{
			DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!", null);
			this.requiredTag = GameTags.Garbage;
		}
		base.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		base.AddPrecondition(ChorePreconditions.instance.CanMoveTo, destination);
		base.AddPrecondition(FetchChore.IsFetchTargetAvailable, null);
		Deconstructable component = base.target.GetComponent<Deconstructable>();
		if (component != null)
		{
			base.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component);
		}
		BuildingEnabledButton component2 = base.target.GetComponent<BuildingEnabledButton>();
		if (component2 != null)
		{
			base.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component2);
		}
		if (operational_requirement != Operational.State.None)
		{
			Operational component3 = destination.GetComponent<Operational>();
			if (component3 != null)
			{
				Chore.Precondition precondition = ChorePreconditions.instance.IsOperational;
				if (operational_requirement == Operational.State.Functional)
				{
					precondition = ChorePreconditions.instance.IsFunctional;
				}
				base.AddPrecondition(precondition, component3);
			}
		}
		this.partitionerEntry = GameScenePartitioner.Instance.Add(destination.name, this, Grid.PosToCell(destination), GameScenePartitioner.Instance.fetchChoreLayer, null);
		destination.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.automatable = destination.GetComponent<Automatable>();
		if (this.automatable)
		{
			base.AddPrecondition(ChorePreconditions.instance.IsAllowedByAutomation, this.automatable);
		}
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x0006927C File Offset: 0x0006747C
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		if (this.destination != null)
		{
			if (this.destination.GetOnlyFetchMarkedItems())
			{
				DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!", null);
				this.requiredTag = GameTags.Garbage;
				return;
			}
			this.requiredTag = Tag.Invalid;
		}
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x000692D4 File Offset: 0x000674D4
	private void OnMasterPriorityChanged(PriorityScreen.PriorityClass priorityClass, int priority_value)
	{
		this.masterPriority.priority_class = priorityClass;
		this.masterPriority.priority_value = priority_value;
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x000692EE File Offset: 0x000674EE
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x000692F0 File Offset: 0x000674F0
	public void CollectChoresFromGlobalChoreProvider(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		base.CollectChores(consumer_state, succeeded_contexts, failed_contexts, is_attempting_override);
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x00069300 File Offset: 0x00067500
	public override void Cleanup()
	{
		base.Cleanup();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.destination != null)
		{
			this.destination.Unsubscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		}
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x00069350 File Offset: 0x00067550
	public static int ComputeHashCodeForTags(IEnumerable<Tag> tags)
	{
		int num = 123137;
		foreach (Tag tag in new SortedSet<Tag>(tags))
		{
			num = ((num << 5) + num ^ tag.GetHash());
		}
		return num;
	}

	// Token: 0x04000AAC RID: 2732
	public HashSet<Tag> tags;

	// Token: 0x04000AAD RID: 2733
	public Tag tagsFirst;

	// Token: 0x04000AAE RID: 2734
	public FetchChore.MatchCriteria criteria;

	// Token: 0x04000AAF RID: 2735
	public int tagsHash;

	// Token: 0x04000AB0 RID: 2736
	public bool validateRequiredTagOnTagChange;

	// Token: 0x04000AB1 RID: 2737
	public Tag requiredTag;

	// Token: 0x04000AB2 RID: 2738
	public Tag[] forbiddenTags;

	// Token: 0x04000AB3 RID: 2739
	public int forbidHash;

	// Token: 0x04000AB4 RID: 2740
	public Automatable automatable;

	// Token: 0x04000AB5 RID: 2741
	public bool allowMultifetch = true;

	// Token: 0x04000AB6 RID: 2742
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04000AB7 RID: 2743
	public static readonly Chore.Precondition IsFetchTargetAvailable = new Chore.Precondition
	{
		id = "IsFetchTargetAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_FETCH_TARGET_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			FetchChore fetchChore = (FetchChore)context.chore;
			Pickupable pickupable = (Pickupable)context.data;
			bool flag;
			if (pickupable == null)
			{
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
				flag = (pickupable != null);
			}
			else
			{
				flag = FetchManager.IsFetchablePickup(pickupable, fetchChore, context.consumerState.storage);
			}
			if (flag)
			{
				if (pickupable == null)
				{
					global::Debug.Log(string.Format("Failed to find fetch target for {0}", fetchChore.destination));
					return false;
				}
				context.data = pickupable;
				int num;
				if (context.consumerState.consumer.GetNavigationCost(pickupable, out num))
				{
					context.cost += num;
					return true;
				}
			}
			return false;
		}
	};

	// Token: 0x02000FF8 RID: 4088
	public enum MatchCriteria
	{
		// Token: 0x040057BC RID: 22460
		MatchID,
		// Token: 0x040057BD RID: 22461
		MatchTags
	}

	// Token: 0x02000FF9 RID: 4089
	public class StatesInstance : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.GameInstance
	{
		// Token: 0x0600744D RID: 29773 RVA: 0x002C72DC File Offset: 0x002C54DC
		public StatesInstance(FetchChore master) : base(master)
		{
		}
	}

	// Token: 0x02000FFA RID: 4090
	public class States : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore>
	{
		// Token: 0x0600744E RID: 29774 RVA: 0x002C72E5 File Offset: 0x002C54E5
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
		}

		// Token: 0x040057BE RID: 22462
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter fetcher;

		// Token: 0x040057BF RID: 22463
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter source;

		// Token: 0x040057C0 RID: 22464
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter chunk;

		// Token: 0x040057C1 RID: 22465
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter requestedamount;

		// Token: 0x040057C2 RID: 22466
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter actualamount;
	}
}
