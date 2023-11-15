using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020003D1 RID: 977
public abstract class Chore
{
	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06001419 RID: 5145 RVA: 0x0006B558 File Offset: 0x00069758
	// (set) Token: 0x0600141A RID: 5146 RVA: 0x0006B560 File Offset: 0x00069760
	public int id { get; private set; }

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x0600141B RID: 5147 RVA: 0x0006B569 File Offset: 0x00069769
	// (set) Token: 0x0600141C RID: 5148 RVA: 0x0006B571 File Offset: 0x00069771
	public ChoreDriver driver { get; set; }

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x0600141D RID: 5149 RVA: 0x0006B57A File Offset: 0x0006977A
	// (set) Token: 0x0600141E RID: 5150 RVA: 0x0006B582 File Offset: 0x00069782
	public ChoreDriver lastDriver { get; set; }

	// Token: 0x0600141F RID: 5151
	protected abstract StateMachine.Instance GetSMI();

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06001420 RID: 5152 RVA: 0x0006B58B File Offset: 0x0006978B
	// (set) Token: 0x06001421 RID: 5153 RVA: 0x0006B593 File Offset: 0x00069793
	public ChoreType choreType { get; set; }

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06001422 RID: 5154 RVA: 0x0006B59C File Offset: 0x0006979C
	// (set) Token: 0x06001423 RID: 5155 RVA: 0x0006B5A4 File Offset: 0x000697A4
	public ChoreProvider provider { get; set; }

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06001424 RID: 5156 RVA: 0x0006B5AD File Offset: 0x000697AD
	// (set) Token: 0x06001425 RID: 5157 RVA: 0x0006B5B5 File Offset: 0x000697B5
	public ChoreConsumer overrideTarget { get; private set; }

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06001426 RID: 5158 RVA: 0x0006B5BE File Offset: 0x000697BE
	// (set) Token: 0x06001427 RID: 5159 RVA: 0x0006B5C6 File Offset: 0x000697C6
	public bool isComplete { get; protected set; }

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06001428 RID: 5160 RVA: 0x0006B5CF File Offset: 0x000697CF
	// (set) Token: 0x06001429 RID: 5161 RVA: 0x0006B5D7 File Offset: 0x000697D7
	public IStateMachineTarget target { get; protected set; }

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x0600142A RID: 5162 RVA: 0x0006B5E0 File Offset: 0x000697E0
	// (set) Token: 0x0600142B RID: 5163 RVA: 0x0006B5E8 File Offset: 0x000697E8
	public bool runUntilComplete { get; set; }

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x0600142C RID: 5164 RVA: 0x0006B5F1 File Offset: 0x000697F1
	// (set) Token: 0x0600142D RID: 5165 RVA: 0x0006B5F9 File Offset: 0x000697F9
	public int priorityMod { get; set; }

	// Token: 0x0600142E RID: 5166 RVA: 0x0006B602 File Offset: 0x00069802
	public bool InProgress()
	{
		return this.driver != null;
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x0600142F RID: 5167
	public abstract GameObject gameObject { get; }

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06001430 RID: 5168
	public abstract bool isNull { get; }

	// Token: 0x06001431 RID: 5169 RVA: 0x0006B610 File Offset: 0x00069810
	public bool IsValid()
	{
		return this.provider != null && this.gameObject.GetMyWorldId() != -1;
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06001432 RID: 5170 RVA: 0x0006B633 File Offset: 0x00069833
	// (set) Token: 0x06001433 RID: 5171 RVA: 0x0006B63B File Offset: 0x0006983B
	public bool IsPreemptable { get; protected set; }

	// Token: 0x06001434 RID: 5172 RVA: 0x0006B644 File Offset: 0x00069844
	public Chore(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete, Action<Chore> on_complete, Action<Chore> on_begin, Action<Chore> on_end, PriorityScreen.PriorityClass priority_class, int priority_value, bool is_preemptable, bool allow_in_context_menu, int priority_mod, bool add_to_daily_report, ReportManager.ReportType report_type)
	{
		this.target = target;
		if (priority_value == 2147483647)
		{
			priority_class = PriorityScreen.PriorityClass.topPriority;
			priority_value = 2;
		}
		if (priority_value < 1 || priority_value > 9)
		{
			global::Debug.LogErrorFormat("Priority Value Out Of Range: {0}", new object[]
			{
				priority_value
			});
		}
		this.masterPriority = new PrioritySetting(priority_class, priority_value);
		this.priorityMod = priority_mod;
		this.id = ++Chore.nextId;
		if (chore_provider == null)
		{
			chore_provider = GlobalChoreProvider.Instance;
			DebugUtil.Assert(chore_provider != null);
		}
		this.choreType = chore_type;
		this.runUntilComplete = run_until_complete;
		this.onComplete = on_complete;
		this.onEnd = on_end;
		this.onBegin = on_begin;
		this.IsPreemptable = is_preemptable;
		this.AddPrecondition(ChorePreconditions.instance.IsValid, null);
		this.AddPrecondition(ChorePreconditions.instance.IsPermitted, null);
		this.AddPrecondition(ChorePreconditions.instance.IsPreemptable, null);
		this.AddPrecondition(ChorePreconditions.instance.HasUrge, null);
		this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingEarly, null);
		this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingLate, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOverrideTargetNullOrMe, null);
		chore_provider.AddChore(this);
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x0006B797 File Offset: 0x00069997
	public virtual void Cleanup()
	{
		this.ClearPrioritizable();
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x0006B79F File Offset: 0x0006999F
	public void SetPriorityMod(int priorityMod)
	{
		this.priorityMod = priorityMod;
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x0006B7A8 File Offset: 0x000699A8
	public List<Chore.PreconditionInstance> GetPreconditions()
	{
		if (this.arePreconditionsDirty)
		{
			this.preconditions.Sort((Chore.PreconditionInstance x, Chore.PreconditionInstance y) => x.sortOrder.CompareTo(y.sortOrder));
			this.arePreconditionsDirty = false;
		}
		return this.preconditions;
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x0006B7F4 File Offset: 0x000699F4
	protected void SetPrioritizable(Prioritizable prioritizable)
	{
		if (prioritizable != null && prioritizable.IsPrioritizable())
		{
			this.prioritizable = prioritizable;
			this.masterPriority = prioritizable.GetMasterPriority();
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnMasterPriorityChanged));
		}
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x0006B847 File Offset: 0x00069A47
	private void ClearPrioritizable()
	{
		if (this.prioritizable != null)
		{
			Prioritizable prioritizable = this.prioritizable;
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Remove(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnMasterPriorityChanged));
		}
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x0006B87E File Offset: 0x00069A7E
	private void OnMasterPriorityChanged(PrioritySetting priority)
	{
		this.masterPriority = priority;
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x0006B887 File Offset: 0x00069A87
	public void SetOverrideTarget(ChoreConsumer chore_consumer)
	{
		if (chore_consumer != null)
		{
			string name = chore_consumer.name;
		}
		this.overrideTarget = chore_consumer;
		this.Fail("New override target");
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x0006B8AC File Offset: 0x00069AAC
	public void AddPrecondition(Chore.Precondition precondition, object data = null)
	{
		this.arePreconditionsDirty = true;
		this.preconditions.Add(new Chore.PreconditionInstance
		{
			id = precondition.id,
			description = precondition.description,
			sortOrder = precondition.sortOrder,
			fn = precondition.fn,
			data = data
		});
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x0006B910 File Offset: 0x00069B10
	public virtual void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		Chore.Precondition.Context item = new Chore.Precondition.Context(this, consumer_state, is_attempting_override, null);
		item.RunPreconditions();
		if (item.IsSuccess())
		{
			succeeded_contexts.Add(item);
			return;
		}
		failed_contexts.Add(item);
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x0006B948 File Offset: 0x00069B48
	public bool SatisfiesUrge(Urge urge)
	{
		return urge == this.choreType.urge;
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x0006B958 File Offset: 0x00069B58
	public ReportManager.ReportType GetReportType()
	{
		return this.reportType;
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x0006B960 File Offset: 0x00069B60
	public virtual void PrepareChore(ref Chore.Precondition.Context context)
	{
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x0006B962 File Offset: 0x00069B62
	public virtual string ResolveString(string str)
	{
		return str;
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x0006B968 File Offset: 0x00069B68
	public virtual void Begin(Chore.Precondition.Context context)
	{
		if (this.driver != null)
		{
			global::Debug.LogErrorFormat("Chore.Begin driver already set {0} {1} {2}, provider {3}, driver {4} -> {5}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver,
				context.consumerState.choreDriver
			});
		}
		if (this.provider == null)
		{
			global::Debug.LogErrorFormat("Chore.Begin provider is null {0} {1} {2}, provider {3}, driver {4}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver
			});
		}
		this.driver = context.consumerState.choreDriver;
		StateMachine.Instance smi = this.GetSMI();
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStateMachineStop));
		KSelectable component = this.driver.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.Main, this.GetStatusItem(), this);
		}
		smi.StartSM();
		if (this.onBegin != null)
		{
			this.onBegin(this);
		}
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x0006BAB4 File Offset: 0x00069CB4
	protected virtual void End(string reason)
	{
		if (this.driver != null)
		{
			KSelectable component = this.driver.GetComponent<KSelectable>();
			if (component != null)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
			}
		}
		StateMachine.Instance smi = this.GetSMI();
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Remove(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStateMachineStop));
		smi.StopSM(reason);
		if (this.driver == null)
		{
			return;
		}
		this.lastDriver = this.driver;
		this.driver = null;
		if (this.onEnd != null)
		{
			this.onEnd(this);
		}
		if (this.onExit != null)
		{
			this.onExit(this);
		}
		this.driver = null;
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x0006BB7C File Offset: 0x00069D7C
	protected void Succeed(string reason)
	{
		if (!this.RemoveFromProvider())
		{
			return;
		}
		this.isComplete = true;
		if (this.onComplete != null)
		{
			this.onComplete(this);
		}
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, null));
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().LogSuitChore((this.driver != null) ? this.driver : this.lastDriver);
		}
		this.End(reason);
		this.Cleanup();
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x0006BC0F File Offset: 0x00069E0F
	protected virtual StatusItem GetStatusItem()
	{
		return this.choreType.statusItem;
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x0006BC1C File Offset: 0x00069E1C
	public virtual void Fail(string reason)
	{
		if (this.provider == null)
		{
			return;
		}
		if (this.driver == null)
		{
			return;
		}
		if (!this.runUntilComplete)
		{
			this.Cancel(reason);
			return;
		}
		this.End(reason);
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x0006BC54 File Offset: 0x00069E54
	public void Cancel(string reason)
	{
		if (!this.RemoveFromProvider())
		{
			return;
		}
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, null));
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().LogSuitChore((this.driver != null) ? this.driver : this.lastDriver);
		}
		this.End(reason);
		this.Cleanup();
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x0006BCCC File Offset: 0x00069ECC
	protected virtual void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (status == StateMachine.Status.Success)
		{
			this.Succeed(reason);
			return;
		}
		this.Fail(reason);
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x0006BCE1 File Offset: 0x00069EE1
	private bool RemoveFromProvider()
	{
		if (this.provider != null)
		{
			this.provider.RemoveChore(this);
			return true;
		}
		return false;
	}

	// Token: 0x0600144A RID: 5194 RVA: 0x0006BD00 File Offset: 0x00069F00
	public virtual bool CanPreempt(Chore.Precondition.Context context)
	{
		return this.IsPreemptable;
	}

	// Token: 0x0600144B RID: 5195 RVA: 0x0006BD08 File Offset: 0x00069F08
	protected virtual void ShowCustomEditor(string filter, int width)
	{
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x0006BD0A File Offset: 0x00069F0A
	public virtual string GetReportName(string context = null)
	{
		if (context == null || this.choreType.reportName == null)
		{
			return this.choreType.Name;
		}
		return string.Format(this.choreType.reportName, context);
	}

	// Token: 0x04000AD3 RID: 2771
	private static int nextId;

	// Token: 0x04000ADA RID: 2778
	public bool isExpanded;

	// Token: 0x04000ADC RID: 2780
	public bool showAvailabilityInHoverText = true;

	// Token: 0x04000ADF RID: 2783
	public PrioritySetting masterPriority;

	// Token: 0x04000AE1 RID: 2785
	public Action<Chore> onExit;

	// Token: 0x04000AE2 RID: 2786
	public Action<Chore> onComplete;

	// Token: 0x04000AE3 RID: 2787
	private Action<Chore> onBegin;

	// Token: 0x04000AE4 RID: 2788
	private Action<Chore> onEnd;

	// Token: 0x04000AE5 RID: 2789
	public Action<Chore> onCleanup;

	// Token: 0x04000AE6 RID: 2790
	private List<Chore.PreconditionInstance> preconditions = new List<Chore.PreconditionInstance>();

	// Token: 0x04000AE7 RID: 2791
	private bool arePreconditionsDirty;

	// Token: 0x04000AE8 RID: 2792
	public bool addToDailyReport;

	// Token: 0x04000AE9 RID: 2793
	public ReportManager.ReportType reportType;

	// Token: 0x04000AEB RID: 2795
	private Prioritizable prioritizable;

	// Token: 0x04000AEC RID: 2796
	public const int MAX_PLAYER_BASIC_PRIORITY = 9;

	// Token: 0x04000AED RID: 2797
	public const int MIN_PLAYER_BASIC_PRIORITY = 1;

	// Token: 0x04000AEE RID: 2798
	public const int MAX_PLAYER_HIGH_PRIORITY = 0;

	// Token: 0x04000AEF RID: 2799
	public const int MIN_PLAYER_HIGH_PRIORITY = 0;

	// Token: 0x04000AF0 RID: 2800
	public const int MAX_PLAYER_EMERGENCY_PRIORITY = 1;

	// Token: 0x04000AF1 RID: 2801
	public const int MIN_PLAYER_EMERGENCY_PRIORITY = 1;

	// Token: 0x04000AF2 RID: 2802
	public const int DEFAULT_BASIC_PRIORITY = 5;

	// Token: 0x04000AF3 RID: 2803
	public const int MAX_BASIC_PRIORITY = 10;

	// Token: 0x04000AF4 RID: 2804
	public const int MIN_BASIC_PRIORITY = 0;

	// Token: 0x04000AF5 RID: 2805
	public static bool ENABLE_PERSONAL_PRIORITIES = true;

	// Token: 0x04000AF6 RID: 2806
	public static PrioritySetting DefaultPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);

	// Token: 0x0200103E RID: 4158
	// (Invoke) Token: 0x06007520 RID: 29984
	public delegate bool PreconditionFn(ref Chore.Precondition.Context context, object data);

	// Token: 0x0200103F RID: 4159
	public struct PreconditionInstance
	{
		// Token: 0x04005892 RID: 22674
		public string id;

		// Token: 0x04005893 RID: 22675
		public string description;

		// Token: 0x04005894 RID: 22676
		public int sortOrder;

		// Token: 0x04005895 RID: 22677
		public Chore.PreconditionFn fn;

		// Token: 0x04005896 RID: 22678
		public object data;
	}

	// Token: 0x02001040 RID: 4160
	public struct Precondition
	{
		// Token: 0x04005897 RID: 22679
		public string id;

		// Token: 0x04005898 RID: 22680
		public string description;

		// Token: 0x04005899 RID: 22681
		public int sortOrder;

		// Token: 0x0400589A RID: 22682
		public Chore.PreconditionFn fn;

		// Token: 0x02001FFC RID: 8188
		[DebuggerDisplay("{chore.GetType()}, {chore.gameObject.name}")]
		public struct Context : IComparable<Chore.Precondition.Context>, IEquatable<Chore.Precondition.Context>
		{
			// Token: 0x0600A44A RID: 42058 RVA: 0x00369264 File Offset: 0x00367464
			public Context(Chore chore, ChoreConsumerState consumer_state, bool is_attempting_override, object data = null)
			{
				this.masterPriority = chore.masterPriority;
				this.personalPriority = consumer_state.consumer.GetPersonalPriority(chore.choreType);
				this.priority = 0;
				this.priorityMod = chore.priorityMod;
				this.consumerPriority = 0;
				this.interruptPriority = 0;
				this.cost = 0;
				this.chore = chore;
				this.consumerState = consumer_state;
				this.failedPreconditionId = -1;
				this.isAttemptingOverride = is_attempting_override;
				this.data = data;
				this.choreTypeForPermission = chore.choreType;
				this.skipMoreSatisfyingEarlyPrecondition = (RootMenu.Instance != null && RootMenu.Instance.IsBuildingChorePanelActive());
				this.SetPriority(chore);
			}

			// Token: 0x0600A44B RID: 42059 RVA: 0x00369314 File Offset: 0x00367514
			public void Set(Chore chore, ChoreConsumerState consumer_state, bool is_attempting_override, object data = null)
			{
				this.masterPriority = chore.masterPriority;
				this.priority = 0;
				this.priorityMod = chore.priorityMod;
				this.consumerPriority = 0;
				this.interruptPriority = 0;
				this.cost = 0;
				this.chore = chore;
				this.consumerState = consumer_state;
				this.failedPreconditionId = -1;
				this.isAttemptingOverride = is_attempting_override;
				this.data = data;
				this.choreTypeForPermission = chore.choreType;
				this.SetPriority(chore);
			}

			// Token: 0x0600A44C RID: 42060 RVA: 0x0036938C File Offset: 0x0036758C
			public void SetPriority(Chore chore)
			{
				this.priority = (Game.Instance.advancedPersonalPriorities ? chore.choreType.explicitPriority : chore.choreType.priority);
				this.priorityMod = chore.priorityMod;
				this.interruptPriority = chore.choreType.interruptPriority;
			}

			// Token: 0x0600A44D RID: 42061 RVA: 0x003693E0 File Offset: 0x003675E0
			public bool IsSuccess()
			{
				return this.failedPreconditionId == -1;
			}

			// Token: 0x0600A44E RID: 42062 RVA: 0x003693EC File Offset: 0x003675EC
			public bool IsPotentialSuccess()
			{
				if (this.IsSuccess())
				{
					return true;
				}
				if (this.chore.driver == this.consumerState.choreDriver)
				{
					return true;
				}
				if (this.failedPreconditionId != -1)
				{
					if (this.failedPreconditionId >= 0 && this.failedPreconditionId < this.chore.preconditions.Count)
					{
						return this.chore.preconditions[this.failedPreconditionId].id == ChorePreconditions.instance.IsMoreSatisfyingLate.id;
					}
					DebugUtil.DevLogErrorFormat("failedPreconditionId out of range {0}/{1}", new object[]
					{
						this.failedPreconditionId,
						this.chore.preconditions.Count
					});
				}
				return false;
			}

			// Token: 0x0600A44F RID: 42063 RVA: 0x003694B8 File Offset: 0x003676B8
			public void RunPreconditions()
			{
				if (this.chore.arePreconditionsDirty)
				{
					this.chore.preconditions.Sort((Chore.PreconditionInstance x, Chore.PreconditionInstance y) => x.sortOrder.CompareTo(y.sortOrder));
					this.chore.arePreconditionsDirty = false;
				}
				for (int i = 0; i < this.chore.preconditions.Count; i++)
				{
					Chore.PreconditionInstance preconditionInstance = this.chore.preconditions[i];
					if (!preconditionInstance.fn(ref this, preconditionInstance.data))
					{
						this.failedPreconditionId = i;
						return;
					}
				}
			}

			// Token: 0x0600A450 RID: 42064 RVA: 0x00369558 File Offset: 0x00367758
			public int CompareTo(Chore.Precondition.Context obj)
			{
				bool flag = this.failedPreconditionId != -1;
				bool flag2 = obj.failedPreconditionId != -1;
				if (flag == flag2)
				{
					int num = this.masterPriority.priority_class - obj.masterPriority.priority_class;
					if (num != 0)
					{
						return num;
					}
					int num2 = this.personalPriority - obj.personalPriority;
					if (num2 != 0)
					{
						return num2;
					}
					int num3 = this.masterPriority.priority_value - obj.masterPriority.priority_value;
					if (num3 != 0)
					{
						return num3;
					}
					int num4 = this.priority - obj.priority;
					if (num4 != 0)
					{
						return num4;
					}
					int num5 = this.priorityMod - obj.priorityMod;
					if (num5 != 0)
					{
						return num5;
					}
					int num6 = this.consumerPriority - obj.consumerPriority;
					if (num6 != 0)
					{
						return num6;
					}
					int num7 = obj.cost - this.cost;
					if (num7 != 0)
					{
						return num7;
					}
					if (this.chore == null && obj.chore == null)
					{
						return 0;
					}
					if (this.chore == null)
					{
						return -1;
					}
					if (obj.chore == null)
					{
						return 1;
					}
					return this.chore.id - obj.chore.id;
				}
				else
				{
					if (!flag)
					{
						return 1;
					}
					return -1;
				}
			}

			// Token: 0x0600A451 RID: 42065 RVA: 0x00369674 File Offset: 0x00367874
			public override bool Equals(object obj)
			{
				Chore.Precondition.Context obj2 = (Chore.Precondition.Context)obj;
				return this.CompareTo(obj2) == 0;
			}

			// Token: 0x0600A452 RID: 42066 RVA: 0x00369692 File Offset: 0x00367892
			public bool Equals(Chore.Precondition.Context other)
			{
				return this.CompareTo(other) == 0;
			}

			// Token: 0x0600A453 RID: 42067 RVA: 0x0036969E File Offset: 0x0036789E
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x0600A454 RID: 42068 RVA: 0x003696B0 File Offset: 0x003678B0
			public static bool operator ==(Chore.Precondition.Context x, Chore.Precondition.Context y)
			{
				return x.CompareTo(y) == 0;
			}

			// Token: 0x0600A455 RID: 42069 RVA: 0x003696BD File Offset: 0x003678BD
			public static bool operator !=(Chore.Precondition.Context x, Chore.Precondition.Context y)
			{
				return x.CompareTo(y) != 0;
			}

			// Token: 0x0600A456 RID: 42070 RVA: 0x003696CA File Offset: 0x003678CA
			public static bool ShouldFilter(string filter, string text)
			{
				return !string.IsNullOrEmpty(filter) && (string.IsNullOrEmpty(text) || text.ToLower().IndexOf(filter) < 0);
			}

			// Token: 0x04008FFA RID: 36858
			public PrioritySetting masterPriority;

			// Token: 0x04008FFB RID: 36859
			public int personalPriority;

			// Token: 0x04008FFC RID: 36860
			public int priority;

			// Token: 0x04008FFD RID: 36861
			public int priorityMod;

			// Token: 0x04008FFE RID: 36862
			public int interruptPriority;

			// Token: 0x04008FFF RID: 36863
			public int cost;

			// Token: 0x04009000 RID: 36864
			public int consumerPriority;

			// Token: 0x04009001 RID: 36865
			public Chore chore;

			// Token: 0x04009002 RID: 36866
			public ChoreConsumerState consumerState;

			// Token: 0x04009003 RID: 36867
			public int failedPreconditionId;

			// Token: 0x04009004 RID: 36868
			public object data;

			// Token: 0x04009005 RID: 36869
			public bool isAttemptingOverride;

			// Token: 0x04009006 RID: 36870
			public ChoreType choreTypeForPermission;

			// Token: 0x04009007 RID: 36871
			public bool skipMoreSatisfyingEarlyPrecondition;
		}
	}
}
