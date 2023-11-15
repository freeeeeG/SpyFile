using System;

// Token: 0x020009E9 RID: 2537
public class Splat : GameStateMachine<Splat, Splat.StatesInstance>
{
	// Token: 0x06004BBE RID: 19390 RVA: 0x001A940C File Offset: 0x001A760C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleChore((Splat.StatesInstance smi) => new WorkChore<SplatWorkable>(Db.Get().ChoreTypes.Mop, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true), this.complete);
		this.complete.Enter(delegate(Splat.StatesInstance smi)
		{
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x0400316C RID: 12652
	public GameStateMachine<Splat, Splat.StatesInstance, IStateMachineTarget, object>.State complete;

	// Token: 0x02001869 RID: 6249
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200186A RID: 6250
	public class StatesInstance : GameStateMachine<Splat, Splat.StatesInstance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060091BE RID: 37310 RVA: 0x0032A4D0 File Offset: 0x003286D0
		public StatesInstance(IStateMachineTarget master, Splat.Def def) : base(master, def)
		{
		}
	}
}
