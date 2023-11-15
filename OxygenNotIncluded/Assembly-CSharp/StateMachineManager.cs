using System;
using System.Collections.Generic;

// Token: 0x02000439 RID: 1081
public class StateMachineManager : Singleton<StateMachineManager>, IScheduler
{
	// Token: 0x060016D0 RID: 5840 RVA: 0x000763D1 File Offset: 0x000745D1
	public void RegisterScheduler(Scheduler scheduler)
	{
		this.scheduler = scheduler;
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x000763DA File Offset: 0x000745DA
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x000763EE File Offset: 0x000745EE
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x00076405 File Offset: 0x00074605
	public SchedulerGroup CreateSchedulerGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x00076414 File Offset: 0x00074614
	public StateMachine CreateStateMachine(Type type)
	{
		StateMachine stateMachine = null;
		if (!this.stateMachines.TryGetValue(type, out stateMachine))
		{
			stateMachine = (StateMachine)Activator.CreateInstance(type);
			stateMachine.CreateStates(stateMachine);
			stateMachine.BindStates();
			stateMachine.InitializeStateMachine();
			this.stateMachines[type] = stateMachine;
			List<Action<StateMachine>> list;
			if (this.stateMachineCreatedCBs.TryGetValue(type, out list))
			{
				foreach (Action<StateMachine> action in list)
				{
					action(stateMachine);
				}
			}
		}
		return stateMachine;
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x000764B0 File Offset: 0x000746B0
	public T CreateStateMachine<T>()
	{
		return (T)((object)this.CreateStateMachine(typeof(T)));
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x000764C8 File Offset: 0x000746C8
	public static void ResetParameters()
	{
		for (int i = 0; i < StateMachineManager.parameters.Length; i++)
		{
			StateMachineManager.parameters[i] = null;
		}
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x000764EF File Offset: 0x000746EF
	public StateMachine.Instance CreateSMIFromDef(IStateMachineTarget master, StateMachine.BaseDef def)
	{
		StateMachineManager.parameters[0] = master;
		StateMachineManager.parameters[1] = def;
		return (StateMachine.Instance)Activator.CreateInstance(Singleton<StateMachineManager>.Instance.CreateStateMachine(def.GetStateMachineType()).GetStateMachineInstanceType(), StateMachineManager.parameters);
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x00076525 File Offset: 0x00074725
	public void Clear()
	{
		if (this.scheduler != null)
		{
			this.scheduler.FreeResources();
		}
		if (this.stateMachines != null)
		{
			this.stateMachines.Clear();
		}
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x00076550 File Offset: 0x00074750
	public void AddStateMachineCreatedCallback(Type sm_type, Action<StateMachine> cb)
	{
		List<Action<StateMachine>> list;
		if (!this.stateMachineCreatedCBs.TryGetValue(sm_type, out list))
		{
			list = new List<Action<StateMachine>>();
			this.stateMachineCreatedCBs[sm_type] = list;
		}
		list.Add(cb);
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x00076588 File Offset: 0x00074788
	public void RemoveStateMachineCreatedCallback(Type sm_type, Action<StateMachine> cb)
	{
		List<Action<StateMachine>> list;
		if (this.stateMachineCreatedCBs.TryGetValue(sm_type, out list))
		{
			list.Remove(cb);
		}
	}

	// Token: 0x04000CAA RID: 3242
	private Scheduler scheduler;

	// Token: 0x04000CAB RID: 3243
	private Dictionary<Type, StateMachine> stateMachines = new Dictionary<Type, StateMachine>();

	// Token: 0x04000CAC RID: 3244
	private Dictionary<Type, List<Action<StateMachine>>> stateMachineCreatedCBs = new Dictionary<Type, List<Action<StateMachine>>>();

	// Token: 0x04000CAD RID: 3245
	private static object[] parameters = new object[2];
}
