using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ImGuiNET;
using KSerialization;
using UnityEngine;

// Token: 0x02000431 RID: 1073
public class StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine where StateMachineInstanceType : StateMachine.Instance where MasterType : IStateMachineTarget
{
	// Token: 0x06001695 RID: 5781 RVA: 0x00075888 File Offset: 0x00073A88
	public override string[] GetStateNames()
	{
		List<string> list = new List<string>();
		foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
		{
			list.Add(state.name);
		}
		return list.ToArray();
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x000758EC File Offset: 0x00073AEC
	public void Target(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target)
	{
		this.stateTarget = target;
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x000758F8 File Offset: 0x00073AF8
	public void BindState(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, string state_name)
	{
		if (parent_state != null)
		{
			state_name = parent_state.name + "." + state_name;
		}
		state.name = state_name;
		state.longName = this.name + "." + state_name;
		state.debugPushName = "PuS: " + state.longName;
		state.debugPopName = "PoS: " + state.longName;
		state.debugExecuteName = "EA: " + state.longName;
		List<StateMachine.BaseState> list;
		if (parent_state != null)
		{
			list = new List<StateMachine.BaseState>(parent_state.branch);
		}
		else
		{
			list = new List<StateMachine.BaseState>();
		}
		list.Add(state);
		state.parent = parent_state;
		state.branch = list.ToArray();
		this.maxDepth = Math.Max(state.branch.Length, this.maxDepth);
		this.states.Add(state);
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x000759D4 File Offset: 0x00073BD4
	public void BindStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state, object state_machine)
	{
		foreach (FieldInfo fieldInfo in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.BaseState)))
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)fieldInfo.GetValue(state_machine);
				if (state != parent_state)
				{
					string name = fieldInfo.Name;
					this.BindState(parent_state, state, name);
					this.BindStates(state, state);
				}
			}
		}
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x00075A43 File Offset: 0x00073C43
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x00075A4C File Offset: 0x00073C4C
	public override void BindStates()
	{
		this.BindStates(null, this);
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x00075A56 File Offset: 0x00073C56
	public override Type GetStateMachineInstanceType()
	{
		return typeof(StateMachineInstanceType);
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x00075A64 File Offset: 0x00073C64
	public override StateMachine.BaseState GetState(string state_name)
	{
		foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
		{
			if (state.name == state_name)
			{
				return state;
			}
		}
		return null;
	}

	// Token: 0x0600169D RID: 5789 RVA: 0x00075AC8 File Offset: 0x00073CC8
	public override void FreeResources()
	{
		for (int i = 0; i < this.states.Count; i++)
		{
			this.states[i].FreeResources();
		}
		this.states.Clear();
		base.FreeResources();
	}

	// Token: 0x04000C9D RID: 3229
	private List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State> states = new List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State>();

	// Token: 0x04000C9E RID: 3230
	public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter masterTarget;

	// Token: 0x04000C9F RID: 3231
	[StateMachine.DoNotAutoCreate]
	protected StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter stateTarget;

	// Token: 0x020010B4 RID: 4276
	public class GenericInstance : StateMachine.Instance
	{
		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06007722 RID: 30498 RVA: 0x002D27CF File Offset: 0x002D09CF
		// (set) Token: 0x06007723 RID: 30499 RVA: 0x002D27D7 File Offset: 0x002D09D7
		public StateMachineType sm { get; private set; }

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06007724 RID: 30500 RVA: 0x002D27E0 File Offset: 0x002D09E0
		protected StateMachineInstanceType smi
		{
			get
			{
				return (StateMachineInstanceType)((object)this);
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06007725 RID: 30501 RVA: 0x002D27E8 File Offset: 0x002D09E8
		// (set) Token: 0x06007726 RID: 30502 RVA: 0x002D27F0 File Offset: 0x002D09F0
		public MasterType master { get; private set; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06007727 RID: 30503 RVA: 0x002D27F9 File Offset: 0x002D09F9
		// (set) Token: 0x06007728 RID: 30504 RVA: 0x002D2801 File Offset: 0x002D0A01
		public DefType def { get; set; }

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06007729 RID: 30505 RVA: 0x002D280A File Offset: 0x002D0A0A
		public bool isMasterNull
		{
			get
			{
				return this.internalSm.masterTarget.IsNull((StateMachineInstanceType)((object)this));
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x0600772A RID: 30506 RVA: 0x002D2822 File Offset: 0x002D0A22
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> internalSm
		{
			get
			{
				return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>)((object)this.sm);
			}
		}

		// Token: 0x0600772B RID: 30507 RVA: 0x002D2834 File Offset: 0x002D0A34
		protected virtual void OnCleanUp()
		{
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x0600772C RID: 30508 RVA: 0x002D2836 File Offset: 0x002D0A36
		public override float timeinstate
		{
			get
			{
				return Time.time - this.stateEnterTime;
			}
		}

		// Token: 0x0600772D RID: 30509 RVA: 0x002D2844 File Offset: 0x002D0A44
		public override void FreeResources()
		{
			this.updateHandle.FreeResources();
			this.updateHandle = default(SchedulerHandle);
			this.controller = null;
			if (this.gotoStack != null)
			{
				this.gotoStack.Clear();
			}
			this.gotoStack = null;
			if (this.transitionStack != null)
			{
				this.transitionStack.Clear();
			}
			this.transitionStack = null;
			if (this.currentSchedulerGroup != null)
			{
				this.currentSchedulerGroup.FreeResources();
			}
			this.currentSchedulerGroup = null;
			if (this.stateStack != null)
			{
				for (int i = 0; i < this.stateStack.Length; i++)
				{
					if (this.stateStack[i].schedulerGroup != null)
					{
						this.stateStack[i].schedulerGroup.FreeResources();
					}
				}
			}
			this.stateStack = null;
			base.FreeResources();
		}

		// Token: 0x0600772E RID: 30510 RVA: 0x002D2910 File Offset: 0x002D0B10
		public GenericInstance(MasterType master) : base((StateMachine)((object)Singleton<StateMachineManager>.Instance.CreateStateMachine<StateMachineType>()), master)
		{
			this.master = master;
			this.stateStack = new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[this.stateMachine.GetMaxDepth()];
			for (int i = 0; i < this.stateStack.Length; i++)
			{
				this.stateStack[i].schedulerGroup = Singleton<StateMachineManager>.Instance.CreateSchedulerGroup();
			}
			this.sm = (StateMachineType)((object)this.stateMachine);
			this.dataTable = new object[base.GetStateMachine().dataTableSize];
			this.updateTable = new StateMachine.Instance.UpdateTableEntry[base.GetStateMachine().updateTableSize];
			this.controller = master.GetComponent<StateMachineController>();
			if (this.controller == null)
			{
				this.controller = master.gameObject.AddComponent<StateMachineController>();
			}
			this.internalSm.masterTarget.Set(master.gameObject, this.smi, false);
			this.controller.AddStateMachineInstance(this);
		}

		// Token: 0x0600772F RID: 30511 RVA: 0x002D2A4C File Offset: 0x002D0C4C
		public override IStateMachineTarget GetMaster()
		{
			return this.master;
		}

		// Token: 0x06007730 RID: 30512 RVA: 0x002D2A5C File Offset: 0x002D0C5C
		private void PushEvent(StateEvent evt)
		{
			StateEvent.Context item = evt.Subscribe(this);
			this.subscribedEvents.Push(item);
		}

		// Token: 0x06007731 RID: 30513 RVA: 0x002D2A80 File Offset: 0x002D0C80
		private void PopEvent()
		{
			StateEvent.Context context = this.subscribedEvents.Pop();
			context.stateEvent.Unsubscribe(this, context);
		}

		// Token: 0x06007732 RID: 30514 RVA: 0x002D2AA8 File Offset: 0x002D0CA8
		private bool TryEvaluateTransitions(StateMachine.BaseState state, int goto_id)
		{
			if (state.transitions == null)
			{
				return true;
			}
			bool result = true;
			for (int i = 0; i < state.transitions.Count; i++)
			{
				StateMachine.BaseTransition baseTransition = state.transitions[i];
				if (goto_id != this.gotoId)
				{
					result = false;
					break;
				}
				baseTransition.Evaluate(this.smi);
			}
			return result;
		}

		// Token: 0x06007733 RID: 30515 RVA: 0x002D2B04 File Offset: 0x002D0D04
		private void PushTransitions(StateMachine.BaseState state)
		{
			if (state.transitions == null)
			{
				return;
			}
			for (int i = 0; i < state.transitions.Count; i++)
			{
				StateMachine.BaseTransition transition = state.transitions[i];
				this.PushTransition(transition);
			}
		}

		// Token: 0x06007734 RID: 30516 RVA: 0x002D2B44 File Offset: 0x002D0D44
		private void PushTransition(StateMachine.BaseTransition transition)
		{
			StateMachine.BaseTransition.Context item = transition.Register(this.smi);
			this.transitionStack.Push(item);
		}

		// Token: 0x06007735 RID: 30517 RVA: 0x002D2B70 File Offset: 0x002D0D70
		private void PopTransition(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			StateMachine.BaseTransition.Context context = this.transitionStack.Pop();
			state.transitions[context.idx].Unregister(this.smi, context);
		}

		// Token: 0x06007736 RID: 30518 RVA: 0x002D2BAC File Offset: 0x002D0DAC
		private void PushState(StateMachine.BaseState state)
		{
			int num = this.gotoId;
			this.currentActionIdx = -1;
			if (state.events != null)
			{
				foreach (StateEvent evt in state.events)
				{
					this.PushEvent(evt);
				}
			}
			this.PushTransitions(state);
			if (state.updateActions != null)
			{
				for (int i = 0; i < state.updateActions.Count; i++)
				{
					StateMachine.UpdateAction updateAction = state.updateActions[i];
					int updateTableIdx = updateAction.updateTableIdx;
					int nextBucketIdx = updateAction.nextBucketIdx;
					updateAction.nextBucketIdx = (updateAction.nextBucketIdx + 1) % updateAction.buckets.Length;
					UpdateBucketWithUpdater<StateMachineInstanceType> updateBucketWithUpdater = (UpdateBucketWithUpdater<StateMachineInstanceType>)updateAction.buckets[nextBucketIdx];
					this.smi.updateTable[updateTableIdx].bucket = updateBucketWithUpdater;
					this.smi.updateTable[updateTableIdx].handle = updateBucketWithUpdater.Add(this.smi, Singleton<StateMachineUpdater>.Instance.GetFrameTime(updateAction.updateRate, updateBucketWithUpdater.frame), (UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater)updateAction.updater);
					state.updateActions[i] = updateAction;
				}
			}
			this.stateEnterTime = Time.time;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] array = this.stateStack;
			int stackSize = this.stackSize;
			this.stackSize = stackSize + 1;
			array[stackSize].state = state;
			this.currentSchedulerGroup = this.stateStack[this.stackSize - 1].schedulerGroup;
			if (!this.TryEvaluateTransitions(state, num))
			{
				return;
			}
			if (num != this.gotoId)
			{
				return;
			}
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.enterActions);
			int num2 = this.gotoId;
		}

		// Token: 0x06007737 RID: 30519 RVA: 0x002D2D88 File Offset: 0x002D0F88
		private void ExecuteActions(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, List<StateMachine.Action> actions)
		{
			if (actions == null)
			{
				return;
			}
			int num = this.gotoId;
			this.currentActionIdx++;
			while (this.currentActionIdx < actions.Count && num == this.gotoId)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback)actions[this.currentActionIdx].callback;
				try
				{
					callback(this.smi);
				}
				catch (Exception e)
				{
					if (!StateMachine.Instance.error)
					{
						base.Error();
						string text = "(NULL).";
						IStateMachineTarget master = this.GetMaster();
						if (!master.isNull)
						{
							KPrefabID component = master.GetComponent<KPrefabID>();
							if (component != null)
							{
								text = "(" + component.PrefabTag.ToString() + ").";
							}
							else
							{
								text = "(" + base.gameObject.name + ").";
							}
						}
						string text2 = string.Concat(new string[]
						{
							"Exception in: ",
							text,
							this.stateMachine.ToString(),
							".",
							state.name,
							"."
						});
						if (this.currentActionIdx > 0 && this.currentActionIdx < actions.Count)
						{
							text2 += actions[this.currentActionIdx].name;
						}
						DebugUtil.LogException(this.controller, text2, e);
					}
				}
				this.currentActionIdx++;
			}
			this.currentActionIdx = 2147483646;
		}

		// Token: 0x06007738 RID: 30520 RVA: 0x002D2F1C File Offset: 0x002D111C
		private void PopState()
		{
			this.currentActionIdx = -1;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] array = this.stateStack;
			int num = this.stackSize - 1;
			this.stackSize = num;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry stackEntry = array[num];
			StateMachine.BaseState state = stackEntry.state;
			int num2 = 0;
			while (state.transitions != null && num2 < state.transitions.Count)
			{
				this.PopTransition((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state);
				num2++;
			}
			if (state.events != null)
			{
				for (int i = 0; i < state.events.Count; i++)
				{
					this.PopEvent();
				}
			}
			if (state.updateActions != null)
			{
				foreach (StateMachine.UpdateAction updateAction in state.updateActions)
				{
					int updateTableIdx = updateAction.updateTableIdx;
					StateMachineUpdater.BaseUpdateBucket baseUpdateBucket = (UpdateBucketWithUpdater<StateMachineInstanceType>)this.smi.updateTable[updateTableIdx].bucket;
					this.smi.updateTable[updateTableIdx].bucket = null;
					baseUpdateBucket.Remove(this.smi.updateTable[updateTableIdx].handle);
				}
			}
			stackEntry.schedulerGroup.Reset();
			this.currentSchedulerGroup = stackEntry.schedulerGroup;
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.exitActions);
		}

		// Token: 0x06007739 RID: 30521 RVA: 0x002D3080 File Offset: 0x002D1280
		public override SchedulerHandle Schedule(float time, Action<object> callback, object callback_data = null)
		{
			string name = null;
			return Singleton<StateMachineManager>.Instance.Schedule(name, time, callback, callback_data, this.currentSchedulerGroup);
		}

		// Token: 0x0600773A RID: 30522 RVA: 0x002D30A4 File Offset: 0x002D12A4
		public override SchedulerHandle ScheduleNextFrame(Action<object> callback, object callback_data = null)
		{
			string name = null;
			return Singleton<StateMachineManager>.Instance.ScheduleNextFrame(name, callback, callback_data, this.currentSchedulerGroup);
		}

		// Token: 0x0600773B RID: 30523 RVA: 0x002D30C6 File Offset: 0x002D12C6
		public override void StartSM()
		{
			if (this.controller != null && !this.controller.HasStateMachineInstance(this))
			{
				this.controller.AddStateMachineInstance(this);
			}
			base.StartSM();
		}

		// Token: 0x0600773C RID: 30524 RVA: 0x002D30F8 File Offset: 0x002D12F8
		public override void StopSM(string reason)
		{
			if (StateMachine.Instance.error)
			{
				return;
			}
			if (this.controller != null)
			{
				this.controller.RemoveStateMachineInstance(this);
			}
			if (!base.IsRunning())
			{
				return;
			}
			this.gotoId++;
			while (this.stackSize > 0)
			{
				this.PopState();
			}
			if (this.master != null && this.controller != null)
			{
				this.controller.RemoveStateMachineInstance(this);
			}
			if (this.status == StateMachine.Status.Running)
			{
				base.SetStatus(StateMachine.Status.Failed);
			}
			if (this.OnStop != null)
			{
				this.OnStop(reason, this.status);
			}
			for (int i = 0; i < this.parameterContexts.Length; i++)
			{
				this.parameterContexts[i].Cleanup();
			}
			this.OnCleanUp();
		}

		// Token: 0x0600773D RID: 30525 RVA: 0x002D31C6 File Offset: 0x002D13C6
		private void FinishStateInProgress(StateMachine.BaseState state)
		{
			if (state.enterActions == null)
			{
				return;
			}
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.enterActions);
		}

		// Token: 0x0600773E RID: 30526 RVA: 0x002D31E4 File Offset: 0x002D13E4
		public override void GoTo(StateMachine.BaseState base_state)
		{
			if (App.IsExiting)
			{
				return;
			}
			if (StateMachine.Instance.error)
			{
				return;
			}
			if (this.isMasterNull)
			{
				return;
			}
			if (this.smi.IsNullOrDestroyed())
			{
				return;
			}
			try
			{
				if (base.IsBreakOnGoToEnabled())
				{
					Debugger.Break();
				}
				if (base_state != null)
				{
					while (base_state.defaultState != null)
					{
						base_state = base_state.defaultState;
					}
				}
				if (this.GetCurrentState() == null)
				{
					base.SetStatus(StateMachine.Status.Running);
				}
				if (this.gotoStack.Count > 100)
				{
					string text = "Potential infinite transition loop detected in state machine: " + this.ToString() + "\nGoto stack:\n";
					foreach (StateMachine.BaseState baseState in this.gotoStack)
					{
						text = text + "\n" + baseState.name;
					}
					global::Debug.LogError(text);
					base.Error();
				}
				else
				{
					this.gotoStack.Push(base_state);
					if (base_state == null)
					{
						this.StopSM("StateMachine.GoTo(null)");
						this.gotoStack.Pop();
					}
					else
					{
						int num = this.gotoId + 1;
						this.gotoId = num;
						int num2 = num;
						StateMachine.BaseState[] branch = (base_state as StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State).branch;
						int num3 = 0;
						while (num3 < this.stackSize && num3 < branch.Length && this.stateStack[num3].state == branch[num3])
						{
							num3++;
						}
						int num4 = this.stackSize - 1;
						if (num4 >= 0 && num4 == num3 - 1)
						{
							this.FinishStateInProgress(this.stateStack[num4].state);
						}
						while (this.stackSize > num3 && num2 == this.gotoId)
						{
							this.PopState();
						}
						int num5 = num3;
						while (num5 < branch.Length && num2 == this.gotoId)
						{
							this.PushState(branch[num5]);
							num5++;
						}
						this.gotoStack.Pop();
					}
				}
			}
			catch (Exception ex)
			{
				if (!StateMachine.Instance.error)
				{
					base.Error();
					string text2 = "(Stop)";
					if (base_state != null)
					{
						text2 = base_state.name;
					}
					string text3 = "(NULL).";
					if (!this.GetMaster().isNull)
					{
						text3 = "(" + base.gameObject.name + ").";
					}
					string str = string.Concat(new string[]
					{
						"Exception in: ",
						text3,
						this.stateMachine.ToString(),
						".GoTo(",
						text2,
						")"
					});
					DebugUtil.LogErrorArgs(this.controller, new object[]
					{
						str + "\n" + ex.ToString()
					});
				}
			}
		}

		// Token: 0x0600773F RID: 30527 RVA: 0x002D34B0 File Offset: 0x002D16B0
		public override StateMachine.BaseState GetCurrentState()
		{
			if (this.stackSize > 0)
			{
				return this.stateStack[this.stackSize - 1].state;
			}
			return null;
		}

		// Token: 0x040059F3 RID: 23027
		private float stateEnterTime;

		// Token: 0x040059F4 RID: 23028
		private int gotoId;

		// Token: 0x040059F5 RID: 23029
		private int currentActionIdx = -1;

		// Token: 0x040059F6 RID: 23030
		private SchedulerHandle updateHandle;

		// Token: 0x040059F7 RID: 23031
		private Stack<StateMachine.BaseState> gotoStack = new Stack<StateMachine.BaseState>();

		// Token: 0x040059F8 RID: 23032
		protected Stack<StateMachine.BaseTransition.Context> transitionStack = new Stack<StateMachine.BaseTransition.Context>();

		// Token: 0x040059FC RID: 23036
		protected StateMachineController controller;

		// Token: 0x040059FD RID: 23037
		private SchedulerGroup currentSchedulerGroup;

		// Token: 0x040059FE RID: 23038
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] stateStack;

		// Token: 0x0200207A RID: 8314
		public struct StackEntry
		{
			// Token: 0x04009171 RID: 37233
			public StateMachine.BaseState state;

			// Token: 0x04009172 RID: 37234
			public SchedulerGroup schedulerGroup;
		}
	}

	// Token: 0x020010B5 RID: 4277
	public class State : StateMachine.BaseState
	{
		// Token: 0x040059FF RID: 23039
		protected StateMachineType sm;

		// Token: 0x0200207B RID: 8315
		// (Invoke) Token: 0x0600A5B3 RID: 42419
		public delegate void Callback(StateMachineInstanceType smi);
	}

	// Token: 0x020010B6 RID: 4278
	public new abstract class ParameterTransition : StateMachine.ParameterTransition
	{
		// Token: 0x06007741 RID: 30529 RVA: 0x002D34DD File Offset: 0x002D16DD
		public ParameterTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state) : base(idx, name, source_state, target_state)
		{
		}
	}

	// Token: 0x020010B7 RID: 4279
	public class Transition : StateMachine.BaseTransition
	{
		// Token: 0x06007742 RID: 30530 RVA: 0x002D34EA File Offset: 0x002D16EA
		public Transition(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition) : base(idx, name, source_state, target_state)
		{
			this.condition = condition;
		}

		// Token: 0x06007743 RID: 30531 RVA: 0x002D34FF File Offset: 0x002D16FF
		public override string ToString()
		{
			if (this.targetState != null)
			{
				return this.name + "->" + this.targetState.name;
			}
			return this.name + "->(Stop)";
		}

		// Token: 0x04005A00 RID: 23040
		public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition;

		// Token: 0x0200207C RID: 8316
		// (Invoke) Token: 0x0600A5B7 RID: 42423
		public delegate bool ConditionCallback(StateMachineInstanceType smi);
	}

	// Token: 0x020010B8 RID: 4280
	public abstract class Parameter<ParameterType> : StateMachine.Parameter
	{
		// Token: 0x06007744 RID: 30532 RVA: 0x002D3535 File Offset: 0x002D1735
		public Parameter()
		{
		}

		// Token: 0x06007745 RID: 30533 RVA: 0x002D353D File Offset: 0x002D173D
		public Parameter(ParameterType default_value)
		{
			this.defaultValue = default_value;
		}

		// Token: 0x06007746 RID: 30534 RVA: 0x002D354C File Offset: 0x002D174C
		public ParameterType Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
		{
			((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this)).Set(value, smi, silenceEvents);
			return value;
		}

		// Token: 0x06007747 RID: 30535 RVA: 0x002D3568 File Offset: 0x002D1768
		public ParameterType Get(StateMachineInstanceType smi)
		{
			return ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this)).value;
		}

		// Token: 0x06007748 RID: 30536 RVA: 0x002D3580 File Offset: 0x002D1780
		public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context GetContext(StateMachineInstanceType smi)
		{
			return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this);
		}

		// Token: 0x04005A01 RID: 23041
		public ParameterType defaultValue;

		// Token: 0x04005A02 RID: 23042
		public bool isSignal;

		// Token: 0x0200207D RID: 8317
		// (Invoke) Token: 0x0600A5BB RID: 42427
		public delegate bool Callback(StateMachineInstanceType smi, ParameterType p);

		// Token: 0x0200207E RID: 8318
		public class Transition : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition
		{
			// Token: 0x0600A5BE RID: 42430 RVA: 0x0036CA52 File Offset: 0x0036AC52
			public Transition(int idx, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback) : base(idx, parameter.name, null, state)
			{
				this.parameter = parameter;
				this.callback = callback;
			}

			// Token: 0x0600A5BF RID: 42431 RVA: 0x0036CA74 File Offset: 0x0036AC74
			public override void Evaluate(StateMachine.Instance smi)
			{
				StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
				global::Debug.Assert(stateMachineInstanceType != null);
				if (this.parameter.isSignal && this.callback == null)
				{
					return;
				}
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)stateMachineInstanceType.GetParameterContext(this.parameter);
				if (this.callback(stateMachineInstanceType, context.value))
				{
					stateMachineInstanceType.GoTo(this.targetState);
				}
			}

			// Token: 0x0600A5C0 RID: 42432 RVA: 0x0036CAED File Offset: 0x0036ACED
			private void Trigger(StateMachineInstanceType smi)
			{
				smi.GoTo(this.targetState);
			}

			// Token: 0x0600A5C1 RID: 42433 RVA: 0x0036CB00 File Offset: 0x0036AD00
			public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this.parameter);
				if (this.parameter.isSignal && this.callback == null)
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context2 = context;
					context2.onDirty = (Action<StateMachineInstanceType>)Delegate.Combine(context2.onDirty, new Action<StateMachineInstanceType>(this.Trigger));
				}
				else
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context3 = context;
					context3.onDirty = (Action<StateMachineInstanceType>)Delegate.Combine(context3.onDirty, new Action<StateMachineInstanceType>(this.Evaluate));
				}
				return new StateMachine.BaseTransition.Context(this);
			}

			// Token: 0x0600A5C2 RID: 42434 RVA: 0x0036CB84 File Offset: 0x0036AD84
			public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context transitionContext)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this.parameter);
				if (this.parameter.isSignal && this.callback == null)
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context2 = context;
					context2.onDirty = (Action<StateMachineInstanceType>)Delegate.Remove(context2.onDirty, new Action<StateMachineInstanceType>(this.Trigger));
					return;
				}
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context3 = context;
				context3.onDirty = (Action<StateMachineInstanceType>)Delegate.Remove(context3.onDirty, new Action<StateMachineInstanceType>(this.Evaluate));
			}

			// Token: 0x0600A5C3 RID: 42435 RVA: 0x0036CBFE File Offset: 0x0036ADFE
			public override string ToString()
			{
				if (this.targetState != null)
				{
					return this.parameter.name + "->" + this.targetState.name;
				}
				return this.parameter.name + "->(Stop)";
			}

			// Token: 0x04009173 RID: 37235
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter;

			// Token: 0x04009174 RID: 37236
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback;
		}

		// Token: 0x0200207F RID: 8319
		public new abstract class Context : StateMachine.Parameter.Context
		{
			// Token: 0x0600A5C4 RID: 42436 RVA: 0x0036CC3E File Offset: 0x0036AE3E
			public Context(StateMachine.Parameter parameter, ParameterType default_value) : base(parameter)
			{
				this.value = default_value;
			}

			// Token: 0x0600A5C5 RID: 42437 RVA: 0x0036CC4E File Offset: 0x0036AE4E
			public virtual void Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				if (!EqualityComparer<ParameterType>.Default.Equals(value, this.value))
				{
					this.value = value;
					if (!silenceEvents && this.onDirty != null)
					{
						this.onDirty(smi);
					}
				}
			}

			// Token: 0x04009175 RID: 37237
			public ParameterType value;

			// Token: 0x04009176 RID: 37238
			public Action<StateMachineInstanceType> onDirty;
		}
	}

	// Token: 0x020010B9 RID: 4281
	public class BoolParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>
	{
		// Token: 0x06007749 RID: 30537 RVA: 0x002D3593 File Offset: 0x002D1793
		public BoolParameter()
		{
		}

		// Token: 0x0600774A RID: 30538 RVA: 0x002D359B File Offset: 0x002D179B
		public BoolParameter(bool default_value) : base(default_value)
		{
		}

		// Token: 0x0600774B RID: 30539 RVA: 0x002D35A4 File Offset: 0x002D17A4
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.BoolParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002080 RID: 8320
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Context
		{
			// Token: 0x0600A5C6 RID: 42438 RVA: 0x0036CC81 File Offset: 0x0036AE81
			public Context(StateMachine.Parameter parameter, bool default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600A5C7 RID: 42439 RVA: 0x0036CC8B File Offset: 0x0036AE8B
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value ? 1 : 0);
			}

			// Token: 0x0600A5C8 RID: 42440 RVA: 0x0036CCA0 File Offset: 0x0036AEA0
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = (reader.ReadByte() > 0);
			}

			// Token: 0x0600A5C9 RID: 42441 RVA: 0x0036CCB1 File Offset: 0x0036AEB1
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600A5CA RID: 42442 RVA: 0x0036CCB4 File Offset: 0x0036AEB4
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				bool value = this.value;
				if (ImGui.Checkbox(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020010BA RID: 4282
	public class Vector3Parameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>
	{
		// Token: 0x0600774C RID: 30540 RVA: 0x002D35B2 File Offset: 0x002D17B2
		public Vector3Parameter()
		{
		}

		// Token: 0x0600774D RID: 30541 RVA: 0x002D35BA File Offset: 0x002D17BA
		public Vector3Parameter(Vector3 default_value) : base(default_value)
		{
		}

		// Token: 0x0600774E RID: 30542 RVA: 0x002D35C3 File Offset: 0x002D17C3
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Vector3Parameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002081 RID: 8321
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>.Context
		{
			// Token: 0x0600A5CB RID: 42443 RVA: 0x0036CCEC File Offset: 0x0036AEEC
			public Context(StateMachine.Parameter parameter, Vector3 default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600A5CC RID: 42444 RVA: 0x0036CCF6 File Offset: 0x0036AEF6
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.x);
				writer.Write(this.value.y);
				writer.Write(this.value.z);
			}

			// Token: 0x0600A5CD RID: 42445 RVA: 0x0036CD2B File Offset: 0x0036AF2B
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value.x = reader.ReadSingle();
				this.value.y = reader.ReadSingle();
				this.value.z = reader.ReadSingle();
			}

			// Token: 0x0600A5CE RID: 42446 RVA: 0x0036CD60 File Offset: 0x0036AF60
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600A5CF RID: 42447 RVA: 0x0036CD64 File Offset: 0x0036AF64
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				Vector3 value = this.value;
				if (ImGui.InputFloat3(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020010BB RID: 4283
	public class EnumParameter<EnumType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>
	{
		// Token: 0x0600774F RID: 30543 RVA: 0x002D35D1 File Offset: 0x002D17D1
		public EnumParameter(EnumType default_value) : base(default_value)
		{
		}

		// Token: 0x06007750 RID: 30544 RVA: 0x002D35DA File Offset: 0x002D17DA
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EnumParameter<EnumType>.Context(this, this.defaultValue);
		}

		// Token: 0x02002082 RID: 8322
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>.Context
		{
			// Token: 0x0600A5D0 RID: 42448 RVA: 0x0036CD9C File Offset: 0x0036AF9C
			public Context(StateMachine.Parameter parameter, EnumType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600A5D1 RID: 42449 RVA: 0x0036CDA6 File Offset: 0x0036AFA6
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write((int)((object)this.value));
			}

			// Token: 0x0600A5D2 RID: 42450 RVA: 0x0036CDBE File Offset: 0x0036AFBE
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = (EnumType)((object)reader.ReadInt32());
			}

			// Token: 0x0600A5D3 RID: 42451 RVA: 0x0036CDD6 File Offset: 0x0036AFD6
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600A5D4 RID: 42452 RVA: 0x0036CDD8 File Offset: 0x0036AFD8
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string[] names = Enum.GetNames(typeof(EnumType));
				Array values = Enum.GetValues(typeof(EnumType));
				int index = Array.IndexOf(values, this.value);
				if (ImGui.Combo(this.parameter.name, ref index, names, names.Length))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set((EnumType)((object)values.GetValue(index)), smi, false);
				}
			}
		}
	}

	// Token: 0x020010BC RID: 4284
	public class FloatParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>
	{
		// Token: 0x06007751 RID: 30545 RVA: 0x002D35E8 File Offset: 0x002D17E8
		public FloatParameter()
		{
		}

		// Token: 0x06007752 RID: 30546 RVA: 0x002D35F0 File Offset: 0x002D17F0
		public FloatParameter(float default_value) : base(default_value)
		{
		}

		// Token: 0x06007753 RID: 30547 RVA: 0x002D35FC File Offset: 0x002D17FC
		public float Delta(float delta_value, StateMachineInstanceType smi)
		{
			float num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06007754 RID: 30548 RVA: 0x002D3620 File Offset: 0x002D1820
		public float DeltaClamp(float delta_value, float min_value, float max_value, StateMachineInstanceType smi)
		{
			float num = base.Get(smi);
			num += delta_value;
			num = Mathf.Clamp(num, min_value, max_value);
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06007755 RID: 30549 RVA: 0x002D364F File Offset: 0x002D184F
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002083 RID: 8323
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Context
		{
			// Token: 0x0600A5D5 RID: 42453 RVA: 0x0036CE4A File Offset: 0x0036B04A
			public Context(StateMachine.Parameter parameter, float default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600A5D6 RID: 42454 RVA: 0x0036CE54 File Offset: 0x0036B054
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x0600A5D7 RID: 42455 RVA: 0x0036CE62 File Offset: 0x0036B062
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadSingle();
			}

			// Token: 0x0600A5D8 RID: 42456 RVA: 0x0036CE70 File Offset: 0x0036B070
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600A5D9 RID: 42457 RVA: 0x0036CE74 File Offset: 0x0036B074
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				float value = this.value;
				if (ImGui.InputFloat(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020010BD RID: 4285
	public class IntParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>
	{
		// Token: 0x06007756 RID: 30550 RVA: 0x002D365D File Offset: 0x002D185D
		public IntParameter()
		{
		}

		// Token: 0x06007757 RID: 30551 RVA: 0x002D3665 File Offset: 0x002D1865
		public IntParameter(int default_value) : base(default_value)
		{
		}

		// Token: 0x06007758 RID: 30552 RVA: 0x002D3670 File Offset: 0x002D1870
		public int Delta(int delta_value, StateMachineInstanceType smi)
		{
			int num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06007759 RID: 30553 RVA: 0x002D3694 File Offset: 0x002D1894
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.IntParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002084 RID: 8324
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Context
		{
			// Token: 0x0600A5DA RID: 42458 RVA: 0x0036CEAC File Offset: 0x0036B0AC
			public Context(StateMachine.Parameter parameter, int default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600A5DB RID: 42459 RVA: 0x0036CEB6 File Offset: 0x0036B0B6
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x0600A5DC RID: 42460 RVA: 0x0036CEC4 File Offset: 0x0036B0C4
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadInt32();
			}

			// Token: 0x0600A5DD RID: 42461 RVA: 0x0036CED2 File Offset: 0x0036B0D2
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600A5DE RID: 42462 RVA: 0x0036CED4 File Offset: 0x0036B0D4
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				int value = this.value;
				if (ImGui.InputInt(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020010BE RID: 4286
	public class ResourceParameter<ResourceType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType> where ResourceType : Resource
	{
		// Token: 0x0600775A RID: 30554 RVA: 0x002D36A4 File Offset: 0x002D18A4
		public ResourceParameter() : base(default(ResourceType))
		{
		}

		// Token: 0x0600775B RID: 30555 RVA: 0x002D36C0 File Offset: 0x002D18C0
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ResourceParameter<ResourceType>.Context(this, this.defaultValue);
		}

		// Token: 0x02002085 RID: 8325
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType>.Context
		{
			// Token: 0x0600A5DF RID: 42463 RVA: 0x0036CF0C File Offset: 0x0036B10C
			public Context(StateMachine.Parameter parameter, ResourceType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600A5E0 RID: 42464 RVA: 0x0036CF18 File Offset: 0x0036B118
			public override void Serialize(BinaryWriter writer)
			{
				string str = "";
				if (this.value != null)
				{
					if (this.value.Guid == null)
					{
						global::Debug.LogError("Cannot serialize resource with invalid guid: " + this.value.Id);
					}
					else
					{
						str = this.value.Guid.Guid;
					}
				}
				writer.WriteKleiString(str);
			}

			// Token: 0x0600A5E1 RID: 42465 RVA: 0x0036CF90 File Offset: 0x0036B190
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				string text = reader.ReadKleiString();
				if (text != "")
				{
					ResourceGuid guid = new ResourceGuid(text, null);
					this.value = Db.Get().GetResource<ResourceType>(guid);
				}
			}

			// Token: 0x0600A5E2 RID: 42466 RVA: 0x0036CFCA File Offset: 0x0036B1CA
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600A5E3 RID: 42467 RVA: 0x0036CFCC File Offset: 0x0036B1CC
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string fmt = "None";
				if (this.value != null)
				{
					fmt = this.value.ToString();
				}
				ImGui.LabelText(this.parameter.name, fmt);
			}
		}
	}

	// Token: 0x020010BF RID: 4287
	public class TagParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>
	{
		// Token: 0x0600775C RID: 30556 RVA: 0x002D36CE File Offset: 0x002D18CE
		public TagParameter()
		{
		}

		// Token: 0x0600775D RID: 30557 RVA: 0x002D36D6 File Offset: 0x002D18D6
		public TagParameter(Tag default_value) : base(default_value)
		{
		}

		// Token: 0x0600775E RID: 30558 RVA: 0x002D36DF File Offset: 0x002D18DF
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002086 RID: 8326
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>.Context
		{
			// Token: 0x0600A5E4 RID: 42468 RVA: 0x0036D00E File Offset: 0x0036B20E
			public Context(StateMachine.Parameter parameter, Tag default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600A5E5 RID: 42469 RVA: 0x0036D018 File Offset: 0x0036B218
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.GetHash());
			}

			// Token: 0x0600A5E6 RID: 42470 RVA: 0x0036D02B File Offset: 0x0036B22B
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = new Tag(reader.ReadInt32());
			}

			// Token: 0x0600A5E7 RID: 42471 RVA: 0x0036D03E File Offset: 0x0036B23E
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600A5E8 RID: 42472 RVA: 0x0036D040 File Offset: 0x0036B240
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				ImGui.LabelText(this.parameter.name, this.value.ToString());
			}
		}
	}

	// Token: 0x020010C0 RID: 4288
	public class ObjectParameter<ObjectType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType> where ObjectType : class
	{
		// Token: 0x0600775F RID: 30559 RVA: 0x002D36F0 File Offset: 0x002D18F0
		public ObjectParameter() : base(default(ObjectType))
		{
		}

		// Token: 0x06007760 RID: 30560 RVA: 0x002D370C File Offset: 0x002D190C
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ObjectParameter<ObjectType>.Context(this, this.defaultValue);
		}

		// Token: 0x02002087 RID: 8327
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType>.Context
		{
			// Token: 0x0600A5E9 RID: 42473 RVA: 0x0036D063 File Offset: 0x0036B263
			public Context(StateMachine.Parameter parameter, ObjectType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600A5EA RID: 42474 RVA: 0x0036D06D File Offset: 0x0036B26D
			public override void Serialize(BinaryWriter writer)
			{
				DebugUtil.DevLogError("ObjectParameter cannot be serialized");
			}

			// Token: 0x0600A5EB RID: 42475 RVA: 0x0036D079 File Offset: 0x0036B279
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				DebugUtil.DevLogError("ObjectParameter cannot be serialized");
			}

			// Token: 0x0600A5EC RID: 42476 RVA: 0x0036D085 File Offset: 0x0036B285
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600A5ED RID: 42477 RVA: 0x0036D088 File Offset: 0x0036B288
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string fmt = "None";
				if (this.value != null)
				{
					fmt = this.value.ToString();
				}
				ImGui.LabelText(this.parameter.name, fmt);
			}
		}
	}

	// Token: 0x020010C1 RID: 4289
	public class TargetParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>
	{
		// Token: 0x06007761 RID: 30561 RVA: 0x002D371A File Offset: 0x002D191A
		public TargetParameter() : base(null)
		{
		}

		// Token: 0x06007762 RID: 30562 RVA: 0x002D3724 File Offset: 0x002D1924
		public SMT GetSMI<SMT>(StateMachineInstanceType smi) where SMT : StateMachine.Instance
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				SMT smi2 = gameObject.GetSMI<SMT>();
				if (smi2 != null)
				{
					return smi2;
				}
				global::Debug.LogError(gameObject.name + " does not have state machine " + typeof(StateMachineType).Name);
			}
			return default(SMT);
		}

		// Token: 0x06007763 RID: 30563 RVA: 0x002D3780 File Offset: 0x002D1980
		public bool IsNull(StateMachineInstanceType smi)
		{
			return base.Get(smi) == null;
		}

		// Token: 0x06007764 RID: 30564 RVA: 0x002D3790 File Offset: 0x002D1990
		public ComponentType Get<ComponentType>(StateMachineInstanceType smi)
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				ComponentType component = gameObject.GetComponent<ComponentType>();
				if (component != null)
				{
					return component;
				}
				global::Debug.LogError(gameObject.name + " does not have component " + typeof(ComponentType).Name);
			}
			return default(ComponentType);
		}

		// Token: 0x06007765 RID: 30565 RVA: 0x002D37EC File Offset: 0x002D19EC
		public ComponentType AddOrGet<ComponentType>(StateMachineInstanceType smi) where ComponentType : Component
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				ComponentType componentType = gameObject.GetComponent<ComponentType>();
				if (componentType == null)
				{
					componentType = gameObject.AddComponent<ComponentType>();
				}
				return componentType;
			}
			return default(ComponentType);
		}

		// Token: 0x06007766 RID: 30566 RVA: 0x002D3834 File Offset: 0x002D1A34
		public void Set(KMonoBehaviour value, StateMachineInstanceType smi)
		{
			GameObject value2 = null;
			if (value != null)
			{
				value2 = value.gameObject;
			}
			base.Set(value2, smi, false);
		}

		// Token: 0x06007767 RID: 30567 RVA: 0x002D385D File Offset: 0x002D1A5D
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002088 RID: 8328
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Context
		{
			// Token: 0x0600A5EE RID: 42478 RVA: 0x0036D0CA File Offset: 0x0036B2CA
			public Context(StateMachine.Parameter parameter, GameObject default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600A5EF RID: 42479 RVA: 0x0036D0D4 File Offset: 0x0036B2D4
			public override void Serialize(BinaryWriter writer)
			{
				if (this.value != null)
				{
					int instanceID = this.value.GetComponent<KPrefabID>().InstanceID;
					writer.Write(instanceID);
					return;
				}
				writer.Write(0);
			}

			// Token: 0x0600A5F0 RID: 42480 RVA: 0x0036D110 File Offset: 0x0036B310
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				try
				{
					int num = reader.ReadInt32();
					if (num != 0)
					{
						KPrefabID instance = KPrefabIDTracker.Get().GetInstance(num);
						if (instance != null)
						{
							this.value = instance.gameObject;
							this.objectDestroyedHandler = instance.Subscribe(1969584890, new Action<object>(this.OnObjectDestroyed));
						}
						this.m_smi = (StateMachineInstanceType)((object)smi);
					}
				}
				catch (Exception ex)
				{
					if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
					{
						global::Debug.LogWarning("Missing statemachine target params. " + ex.Message);
					}
				}
			}

			// Token: 0x0600A5F1 RID: 42481 RVA: 0x0036D1B4 File Offset: 0x0036B3B4
			public override void Cleanup()
			{
				base.Cleanup();
				if (this.value != null)
				{
					this.value.GetComponent<KMonoBehaviour>().Unsubscribe(this.objectDestroyedHandler);
					this.objectDestroyedHandler = 0;
				}
			}

			// Token: 0x0600A5F2 RID: 42482 RVA: 0x0036D1E8 File Offset: 0x0036B3E8
			public override void Set(GameObject value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				this.m_smi = smi;
				if (this.value != null)
				{
					this.value.GetComponent<KMonoBehaviour>().Unsubscribe(this.objectDestroyedHandler);
					this.objectDestroyedHandler = 0;
				}
				if (value != null)
				{
					this.objectDestroyedHandler = value.GetComponent<KMonoBehaviour>().Subscribe(1969584890, new Action<object>(this.OnObjectDestroyed));
				}
				base.Set(value, smi, silenceEvents);
			}

			// Token: 0x0600A5F3 RID: 42483 RVA: 0x0036D25B File Offset: 0x0036B45B
			private void OnObjectDestroyed(object data)
			{
				this.Set(null, this.m_smi, false);
			}

			// Token: 0x0600A5F4 RID: 42484 RVA: 0x0036D26B File Offset: 0x0036B46B
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600A5F5 RID: 42485 RVA: 0x0036D270 File Offset: 0x0036B470
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				if (this.value != null)
				{
					ImGui.LabelText(this.parameter.name, this.value.name);
					return;
				}
				ImGui.LabelText(this.parameter.name, "null");
			}

			// Token: 0x04009177 RID: 37239
			private StateMachineInstanceType m_smi;

			// Token: 0x04009178 RID: 37240
			private int objectDestroyedHandler;
		}
	}

	// Token: 0x020010C2 RID: 4290
	public class SignalParameter
	{
	}

	// Token: 0x020010C3 RID: 4291
	public class Signal : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>
	{
		// Token: 0x06007769 RID: 30569 RVA: 0x002D3873 File Offset: 0x002D1A73
		public Signal() : base(null)
		{
			this.isSignal = true;
		}

		// Token: 0x0600776A RID: 30570 RVA: 0x002D3883 File Offset: 0x002D1A83
		public void Trigger(StateMachineInstanceType smi)
		{
			((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context)smi.GetParameterContext(this)).Set(null, smi, false);
		}

		// Token: 0x0600776B RID: 30571 RVA: 0x002D389E File Offset: 0x002D1A9E
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context(this, this.defaultValue);
		}

		// Token: 0x02002089 RID: 8329
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>.Context
		{
			// Token: 0x0600A5F6 RID: 42486 RVA: 0x0036D2BC File Offset: 0x0036B4BC
			public Context(StateMachine.Parameter parameter, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600A5F7 RID: 42487 RVA: 0x0036D2C6 File Offset: 0x0036B4C6
			public override void Serialize(BinaryWriter writer)
			{
			}

			// Token: 0x0600A5F8 RID: 42488 RVA: 0x0036D2C8 File Offset: 0x0036B4C8
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
			}

			// Token: 0x0600A5F9 RID: 42489 RVA: 0x0036D2CA File Offset: 0x0036B4CA
			public override void Set(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				if (!silenceEvents && this.onDirty != null)
				{
					this.onDirty(smi);
				}
			}

			// Token: 0x0600A5FA RID: 42490 RVA: 0x0036D2E3 File Offset: 0x0036B4E3
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600A5FB RID: 42491 RVA: 0x0036D2E8 File Offset: 0x0036B4E8
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				if (ImGui.Button(this.parameter.name))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(null, smi, false);
				}
			}
		}
	}
}
