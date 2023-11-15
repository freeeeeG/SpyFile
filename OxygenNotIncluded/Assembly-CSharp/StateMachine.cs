using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using KSerialization;
using UnityEngine;

// Token: 0x02000430 RID: 1072
public abstract class StateMachine
{
	// Token: 0x06001685 RID: 5765 RVA: 0x0007566B File Offset: 0x0007386B
	public StateMachine()
	{
		this.name = base.GetType().FullName;
	}

	// Token: 0x06001686 RID: 5766 RVA: 0x00075690 File Offset: 0x00073890
	public virtual void FreeResources()
	{
		this.name = null;
		if (this.defaultState != null)
		{
			this.defaultState.FreeResources();
		}
		this.defaultState = null;
		this.parameters = null;
	}

	// Token: 0x06001687 RID: 5767
	public abstract string[] GetStateNames();

	// Token: 0x06001688 RID: 5768
	public abstract StateMachine.BaseState GetState(string name);

	// Token: 0x06001689 RID: 5769
	public abstract void BindStates();

	// Token: 0x0600168A RID: 5770
	public abstract Type GetStateMachineInstanceType();

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600168B RID: 5771 RVA: 0x000756BA File Offset: 0x000738BA
	// (set) Token: 0x0600168C RID: 5772 RVA: 0x000756C2 File Offset: 0x000738C2
	public int version { get; protected set; }

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x0600168D RID: 5773 RVA: 0x000756CB File Offset: 0x000738CB
	// (set) Token: 0x0600168E RID: 5774 RVA: 0x000756D3 File Offset: 0x000738D3
	public StateMachine.SerializeType serializable { get; protected set; }

	// Token: 0x0600168F RID: 5775 RVA: 0x000756DC File Offset: 0x000738DC
	public virtual void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = null;
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x000756E4 File Offset: 0x000738E4
	public void InitializeStateMachine()
	{
		this.debugSettings = StateMachineDebuggerSettings.Get().CreateEntry(base.GetType());
		StateMachine.BaseState baseState = null;
		this.InitializeStates(out baseState);
		DebugUtil.Assert(baseState != null);
		this.defaultState = baseState;
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x00075724 File Offset: 0x00073924
	public void CreateStates(object state_machine)
	{
		foreach (FieldInfo fieldInfo in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			bool flag = false;
			object[] customAttributes = fieldInfo.GetCustomAttributes(false);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				if (customAttributes[j].GetType() == typeof(StateMachine.DoNotAutoCreate))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.BaseState)))
				{
					StateMachine.BaseState baseState = (StateMachine.BaseState)Activator.CreateInstance(fieldInfo.FieldType);
					this.CreateStates(baseState);
					fieldInfo.SetValue(state_machine, baseState);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.Parameter)))
				{
					StateMachine.Parameter parameter = (StateMachine.Parameter)fieldInfo.GetValue(state_machine);
					if (parameter == null)
					{
						parameter = (StateMachine.Parameter)Activator.CreateInstance(fieldInfo.FieldType);
						fieldInfo.SetValue(state_machine, parameter);
					}
					parameter.name = fieldInfo.Name;
					parameter.idx = this.parameters.Length;
					this.parameters = this.parameters.Append(parameter);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine)))
				{
					fieldInfo.SetValue(state_machine, this);
				}
			}
		}
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x0007586D File Offset: 0x00073A6D
	public StateMachine.BaseState GetDefaultState()
	{
		return this.defaultState;
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x00075875 File Offset: 0x00073A75
	public int GetMaxDepth()
	{
		return this.maxDepth;
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x0007587D File Offset: 0x00073A7D
	public override string ToString()
	{
		return this.name;
	}

	// Token: 0x04000C93 RID: 3219
	protected string name;

	// Token: 0x04000C94 RID: 3220
	protected int maxDepth;

	// Token: 0x04000C95 RID: 3221
	protected StateMachine.BaseState defaultState;

	// Token: 0x04000C96 RID: 3222
	protected StateMachine.Parameter[] parameters = new StateMachine.Parameter[0];

	// Token: 0x04000C97 RID: 3223
	public int dataTableSize;

	// Token: 0x04000C98 RID: 3224
	public int updateTableSize;

	// Token: 0x04000C9B RID: 3227
	public StateMachineDebuggerSettings.Entry debugSettings;

	// Token: 0x04000C9C RID: 3228
	public bool saveHistory;

	// Token: 0x020010A8 RID: 4264
	public sealed class DoNotAutoCreate : Attribute
	{
	}

	// Token: 0x020010A9 RID: 4265
	public enum Status
	{
		// Token: 0x040059C1 RID: 22977
		Initialized,
		// Token: 0x040059C2 RID: 22978
		Running,
		// Token: 0x040059C3 RID: 22979
		Failed,
		// Token: 0x040059C4 RID: 22980
		Success
	}

	// Token: 0x020010AA RID: 4266
	public class BaseDef
	{
		// Token: 0x060076EC RID: 30444 RVA: 0x002D22E9 File Offset: 0x002D04E9
		public StateMachine.Instance CreateSMI(IStateMachineTarget master)
		{
			return Singleton<StateMachineManager>.Instance.CreateSMIFromDef(master, this);
		}

		// Token: 0x060076ED RID: 30445 RVA: 0x002D22F7 File Offset: 0x002D04F7
		public Type GetStateMachineType()
		{
			return base.GetType().DeclaringType;
		}

		// Token: 0x060076EE RID: 30446 RVA: 0x002D2304 File Offset: 0x002D0504
		public virtual void Configure(GameObject prefab)
		{
		}
	}

	// Token: 0x020010AB RID: 4267
	public class Category : Resource
	{
		// Token: 0x060076F0 RID: 30448 RVA: 0x002D230E File Offset: 0x002D050E
		public Category(string id) : base(id, null, null)
		{
		}
	}

	// Token: 0x020010AC RID: 4268
	[SerializationConfig(MemberSerialization.OptIn)]
	public abstract class Instance
	{
		// Token: 0x060076F1 RID: 30449
		public abstract StateMachine.BaseState GetCurrentState();

		// Token: 0x060076F2 RID: 30450
		public abstract void GoTo(StateMachine.BaseState state);

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x060076F3 RID: 30451
		public abstract float timeinstate { get; }

		// Token: 0x060076F4 RID: 30452
		public abstract IStateMachineTarget GetMaster();

		// Token: 0x060076F5 RID: 30453
		public abstract void StopSM(string reason);

		// Token: 0x060076F6 RID: 30454
		public abstract SchedulerHandle Schedule(float time, Action<object> callback, object callback_data = null);

		// Token: 0x060076F7 RID: 30455
		public abstract SchedulerHandle ScheduleNextFrame(Action<object> callback, object callback_data = null);

		// Token: 0x060076F8 RID: 30456 RVA: 0x002D2319 File Offset: 0x002D0519
		public virtual void FreeResources()
		{
			this.stateMachine = null;
			if (this.subscribedEvents != null)
			{
				this.subscribedEvents.Clear();
			}
			this.subscribedEvents = null;
			this.parameterContexts = null;
			this.dataTable = null;
			this.updateTable = null;
		}

		// Token: 0x060076F9 RID: 30457 RVA: 0x002D2351 File Offset: 0x002D0551
		public Instance(StateMachine state_machine, IStateMachineTarget master)
		{
			this.stateMachine = state_machine;
			this.CreateParameterContexts();
			this.log = new LoggerFSSSS(this.stateMachine.name, 35);
		}

		// Token: 0x060076FA RID: 30458 RVA: 0x002D2389 File Offset: 0x002D0589
		public bool IsRunning()
		{
			return this.GetCurrentState() != null;
		}

		// Token: 0x060076FB RID: 30459 RVA: 0x002D2394 File Offset: 0x002D0594
		public void GoTo(string state_name)
		{
			DebugUtil.DevAssert(!KMonoBehaviour.isLoadingScene, "Using Goto while scene was loaded", null);
			StateMachine.BaseState state = this.stateMachine.GetState(state_name);
			this.GoTo(state);
		}

		// Token: 0x060076FC RID: 30460 RVA: 0x002D23C8 File Offset: 0x002D05C8
		public int GetStackSize()
		{
			return this.stackSize;
		}

		// Token: 0x060076FD RID: 30461 RVA: 0x002D23D0 File Offset: 0x002D05D0
		public StateMachine GetStateMachine()
		{
			return this.stateMachine;
		}

		// Token: 0x060076FE RID: 30462 RVA: 0x002D23D8 File Offset: 0x002D05D8
		[Conditional("UNITY_EDITOR")]
		public void Log(string a, string b = "", string c = "", string d = "")
		{
		}

		// Token: 0x060076FF RID: 30463 RVA: 0x002D23DA File Offset: 0x002D05DA
		public bool IsConsoleLoggingEnabled()
		{
			return this.enableConsoleLogging || this.stateMachine.debugSettings.enableConsoleLogging;
		}

		// Token: 0x06007700 RID: 30464 RVA: 0x002D23F6 File Offset: 0x002D05F6
		public bool IsBreakOnGoToEnabled()
		{
			return this.breakOnGoTo || this.stateMachine.debugSettings.breakOnGoTo;
		}

		// Token: 0x06007701 RID: 30465 RVA: 0x002D2412 File Offset: 0x002D0612
		public LoggerFSSSS GetLog()
		{
			return this.log;
		}

		// Token: 0x06007702 RID: 30466 RVA: 0x002D241A File Offset: 0x002D061A
		public StateMachine.Parameter.Context[] GetParameterContexts()
		{
			return this.parameterContexts;
		}

		// Token: 0x06007703 RID: 30467 RVA: 0x002D2422 File Offset: 0x002D0622
		public StateMachine.Parameter.Context GetParameterContext(StateMachine.Parameter parameter)
		{
			return this.parameterContexts[parameter.idx];
		}

		// Token: 0x06007704 RID: 30468 RVA: 0x002D2431 File Offset: 0x002D0631
		public StateMachine.Status GetStatus()
		{
			return this.status;
		}

		// Token: 0x06007705 RID: 30469 RVA: 0x002D2439 File Offset: 0x002D0639
		public void SetStatus(StateMachine.Status status)
		{
			this.status = status;
		}

		// Token: 0x06007706 RID: 30470 RVA: 0x002D2442 File Offset: 0x002D0642
		public void Error()
		{
			if (!StateMachine.Instance.error)
			{
				this.isCrashed = true;
				StateMachine.Instance.error = true;
				RestartWarning.ShouldWarn = true;
			}
		}

		// Token: 0x06007707 RID: 30471 RVA: 0x002D2460 File Offset: 0x002D0660
		public override string ToString()
		{
			string str = "";
			if (this.GetCurrentState() != null)
			{
				str = this.GetCurrentState().name;
			}
			else if (this.GetStatus() != StateMachine.Status.Initialized)
			{
				str = this.GetStatus().ToString();
			}
			return this.stateMachine.ToString() + "(" + str + ")";
		}

		// Token: 0x06007708 RID: 30472 RVA: 0x002D24C4 File Offset: 0x002D06C4
		public virtual void StartSM()
		{
			if (!this.IsRunning())
			{
				StateMachineController component = this.GetComponent<StateMachineController>();
				MyAttributes.OnStart(this, component);
				StateMachine.BaseState defaultState = this.stateMachine.GetDefaultState();
				DebugUtil.Assert(defaultState != null);
				if (!component.Restore(this))
				{
					this.GoTo(defaultState);
				}
			}
		}

		// Token: 0x06007709 RID: 30473 RVA: 0x002D250C File Offset: 0x002D070C
		public bool HasTag(Tag tag)
		{
			return this.GetComponent<KPrefabID>().HasTag(tag);
		}

		// Token: 0x0600770A RID: 30474 RVA: 0x002D251C File Offset: 0x002D071C
		public bool IsInsideState(StateMachine.BaseState state)
		{
			StateMachine.BaseState currentState = this.GetCurrentState();
			if (currentState == null)
			{
				return false;
			}
			bool flag = state == currentState;
			int num = 0;
			while (!flag && num < currentState.branch.Length && !(flag = (state == currentState.branch[num])))
			{
				num++;
			}
			return flag;
		}

		// Token: 0x0600770B RID: 30475 RVA: 0x002D2560 File Offset: 0x002D0760
		public void ScheduleGoTo(float time, StateMachine.BaseState state)
		{
			if (this.scheduleGoToCallback == null)
			{
				this.scheduleGoToCallback = delegate(object d)
				{
					this.GoTo((StateMachine.BaseState)d);
				};
			}
			this.Schedule(time, this.scheduleGoToCallback, state);
		}

		// Token: 0x0600770C RID: 30476 RVA: 0x002D258B File Offset: 0x002D078B
		public void Subscribe(int hash, Action<object> handler)
		{
			this.GetMaster().Subscribe(hash, handler);
		}

		// Token: 0x0600770D RID: 30477 RVA: 0x002D259B File Offset: 0x002D079B
		public void Unsubscribe(int hash, Action<object> handler)
		{
			this.GetMaster().Unsubscribe(hash, handler);
		}

		// Token: 0x0600770E RID: 30478 RVA: 0x002D25AA File Offset: 0x002D07AA
		public void Trigger(int hash, object data = null)
		{
			this.GetMaster().GetComponent<KPrefabID>().Trigger(hash, data);
		}

		// Token: 0x0600770F RID: 30479 RVA: 0x002D25BE File Offset: 0x002D07BE
		public ComponentType Get<ComponentType>()
		{
			return this.GetComponent<ComponentType>();
		}

		// Token: 0x06007710 RID: 30480 RVA: 0x002D25C6 File Offset: 0x002D07C6
		public ComponentType GetComponent<ComponentType>()
		{
			return this.GetMaster().GetComponent<ComponentType>();
		}

		// Token: 0x06007711 RID: 30481 RVA: 0x002D25D4 File Offset: 0x002D07D4
		private void CreateParameterContexts()
		{
			this.parameterContexts = new StateMachine.Parameter.Context[this.stateMachine.parameters.Length];
			for (int i = 0; i < this.stateMachine.parameters.Length; i++)
			{
				this.parameterContexts[i] = this.stateMachine.parameters[i].CreateContext();
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06007712 RID: 30482 RVA: 0x002D262B File Offset: 0x002D082B
		public GameObject gameObject
		{
			get
			{
				return this.GetMaster().gameObject;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06007713 RID: 30483 RVA: 0x002D2638 File Offset: 0x002D0838
		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x040059C5 RID: 22981
		public string serializationSuffix;

		// Token: 0x040059C6 RID: 22982
		protected LoggerFSSSS log;

		// Token: 0x040059C7 RID: 22983
		protected StateMachine.Status status;

		// Token: 0x040059C8 RID: 22984
		protected StateMachine stateMachine;

		// Token: 0x040059C9 RID: 22985
		protected Stack<StateEvent.Context> subscribedEvents = new Stack<StateEvent.Context>();

		// Token: 0x040059CA RID: 22986
		protected int stackSize;

		// Token: 0x040059CB RID: 22987
		protected StateMachine.Parameter.Context[] parameterContexts;

		// Token: 0x040059CC RID: 22988
		public object[] dataTable;

		// Token: 0x040059CD RID: 22989
		public StateMachine.Instance.UpdateTableEntry[] updateTable;

		// Token: 0x040059CE RID: 22990
		private Action<object> scheduleGoToCallback;

		// Token: 0x040059CF RID: 22991
		public Action<string, StateMachine.Status> OnStop;

		// Token: 0x040059D0 RID: 22992
		public bool breakOnGoTo;

		// Token: 0x040059D1 RID: 22993
		public bool enableConsoleLogging;

		// Token: 0x040059D2 RID: 22994
		public bool isCrashed;

		// Token: 0x040059D3 RID: 22995
		public static bool error;

		// Token: 0x02002077 RID: 8311
		public struct UpdateTableEntry
		{
			// Token: 0x0400916C RID: 37228
			public HandleVector<int>.Handle handle;

			// Token: 0x0400916D RID: 37229
			public StateMachineUpdater.BaseUpdateBucket bucket;
		}
	}

	// Token: 0x020010AD RID: 4269
	[DebuggerDisplay("{longName}")]
	public class BaseState
	{
		// Token: 0x06007715 RID: 30485 RVA: 0x002D2653 File Offset: 0x002D0853
		public BaseState()
		{
			this.branch = new StateMachine.BaseState[1];
			this.branch[0] = this;
		}

		// Token: 0x06007716 RID: 30486 RVA: 0x002D2670 File Offset: 0x002D0870
		public void FreeResources()
		{
			if (this.name == null)
			{
				return;
			}
			this.name = null;
			if (this.defaultState != null)
			{
				this.defaultState.FreeResources();
			}
			this.defaultState = null;
			this.events = null;
			int num = 0;
			while (this.transitions != null && num < this.transitions.Count)
			{
				this.transitions[num].Clear();
				num++;
			}
			this.transitions = null;
			this.enterActions = null;
			this.exitActions = null;
			if (this.branch != null)
			{
				for (int i = 0; i < this.branch.Length; i++)
				{
					this.branch[i].FreeResources();
				}
			}
			this.branch = null;
			this.parent = null;
		}

		// Token: 0x06007717 RID: 30487 RVA: 0x002D2728 File Offset: 0x002D0928
		public int GetStateCount()
		{
			return this.branch.Length;
		}

		// Token: 0x06007718 RID: 30488 RVA: 0x002D2732 File Offset: 0x002D0932
		public StateMachine.BaseState GetState(int idx)
		{
			return this.branch[idx];
		}

		// Token: 0x040059D4 RID: 22996
		public string name;

		// Token: 0x040059D5 RID: 22997
		public string longName;

		// Token: 0x040059D6 RID: 22998
		public string debugPushName;

		// Token: 0x040059D7 RID: 22999
		public string debugPopName;

		// Token: 0x040059D8 RID: 23000
		public string debugExecuteName;

		// Token: 0x040059D9 RID: 23001
		public StateMachine.BaseState defaultState;

		// Token: 0x040059DA RID: 23002
		public List<StateEvent> events;

		// Token: 0x040059DB RID: 23003
		public List<StateMachine.BaseTransition> transitions;

		// Token: 0x040059DC RID: 23004
		public List<StateMachine.UpdateAction> updateActions;

		// Token: 0x040059DD RID: 23005
		public List<StateMachine.Action> enterActions;

		// Token: 0x040059DE RID: 23006
		public List<StateMachine.Action> exitActions;

		// Token: 0x040059DF RID: 23007
		public StateMachine.BaseState[] branch;

		// Token: 0x040059E0 RID: 23008
		public StateMachine.BaseState parent;
	}

	// Token: 0x020010AE RID: 4270
	public class BaseTransition
	{
		// Token: 0x06007719 RID: 30489 RVA: 0x002D273C File Offset: 0x002D093C
		public BaseTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state)
		{
			this.idx = idx;
			this.name = name;
			this.sourceState = source_state;
			this.targetState = target_state;
		}

		// Token: 0x0600771A RID: 30490 RVA: 0x002D2761 File Offset: 0x002D0961
		public virtual void Evaluate(StateMachine.Instance smi)
		{
		}

		// Token: 0x0600771B RID: 30491 RVA: 0x002D2763 File Offset: 0x002D0963
		public virtual StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			return new StateMachine.BaseTransition.Context(this);
		}

		// Token: 0x0600771C RID: 30492 RVA: 0x002D276B File Offset: 0x002D096B
		public virtual void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
		}

		// Token: 0x0600771D RID: 30493 RVA: 0x002D276D File Offset: 0x002D096D
		public void Clear()
		{
			this.name = null;
			if (this.sourceState != null)
			{
				this.sourceState.FreeResources();
			}
			this.sourceState = null;
			if (this.targetState != null)
			{
				this.targetState.FreeResources();
			}
			this.targetState = null;
		}

		// Token: 0x040059E1 RID: 23009
		public int idx;

		// Token: 0x040059E2 RID: 23010
		public string name;

		// Token: 0x040059E3 RID: 23011
		public StateMachine.BaseState sourceState;

		// Token: 0x040059E4 RID: 23012
		public StateMachine.BaseState targetState;

		// Token: 0x02002078 RID: 8312
		public struct Context
		{
			// Token: 0x0600A5AB RID: 42411 RVA: 0x0036CA2C File Offset: 0x0036AC2C
			public Context(StateMachine.BaseTransition transition)
			{
				this.idx = transition.idx;
				this.handlerId = 0;
			}

			// Token: 0x0400916E RID: 37230
			public int idx;

			// Token: 0x0400916F RID: 37231
			public int handlerId;
		}
	}

	// Token: 0x020010AF RID: 4271
	public struct UpdateAction
	{
		// Token: 0x040059E5 RID: 23013
		public int updateTableIdx;

		// Token: 0x040059E6 RID: 23014
		public UpdateRate updateRate;

		// Token: 0x040059E7 RID: 23015
		public int nextBucketIdx;

		// Token: 0x040059E8 RID: 23016
		public StateMachineUpdater.BaseUpdateBucket[] buckets;

		// Token: 0x040059E9 RID: 23017
		public object updater;
	}

	// Token: 0x020010B0 RID: 4272
	public struct Action
	{
		// Token: 0x0600771E RID: 30494 RVA: 0x002D27AA File Offset: 0x002D09AA
		public Action(string name, object callback)
		{
			this.name = name;
			this.callback = callback;
		}

		// Token: 0x040059EA RID: 23018
		public string name;

		// Token: 0x040059EB RID: 23019
		public object callback;
	}

	// Token: 0x020010B1 RID: 4273
	public class ParameterTransition : StateMachine.BaseTransition
	{
		// Token: 0x0600771F RID: 30495 RVA: 0x002D27BA File Offset: 0x002D09BA
		public ParameterTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state) : base(idx, name, source_state, target_state)
		{
		}
	}

	// Token: 0x020010B2 RID: 4274
	public abstract class Parameter
	{
		// Token: 0x06007720 RID: 30496
		public abstract StateMachine.Parameter.Context CreateContext();

		// Token: 0x040059EC RID: 23020
		public string name;

		// Token: 0x040059ED RID: 23021
		public int idx;

		// Token: 0x02002079 RID: 8313
		public abstract class Context
		{
			// Token: 0x0600A5AC RID: 42412 RVA: 0x0036CA41 File Offset: 0x0036AC41
			public Context(StateMachine.Parameter parameter)
			{
				this.parameter = parameter;
			}

			// Token: 0x0600A5AD RID: 42413
			public abstract void Serialize(BinaryWriter writer);

			// Token: 0x0600A5AE RID: 42414
			public abstract void Deserialize(IReader reader, StateMachine.Instance smi);

			// Token: 0x0600A5AF RID: 42415 RVA: 0x0036CA50 File Offset: 0x0036AC50
			public virtual void Cleanup()
			{
			}

			// Token: 0x0600A5B0 RID: 42416
			public abstract void ShowEditor(StateMachine.Instance base_smi);

			// Token: 0x0600A5B1 RID: 42417
			public abstract void ShowDevTool(StateMachine.Instance base_smi);

			// Token: 0x04009170 RID: 37232
			public StateMachine.Parameter parameter;
		}
	}

	// Token: 0x020010B3 RID: 4275
	public enum SerializeType
	{
		// Token: 0x040059EF RID: 23023
		Never,
		// Token: 0x040059F0 RID: 23024
		ParamsOnly,
		// Token: 0x040059F1 RID: 23025
		CurrentStateOnly_DEPRECATED,
		// Token: 0x040059F2 RID: 23026
		Both_DEPRECATED
	}
}
