using System;

// Token: 0x020000C7 RID: 199
public class HiveEatingMonitor : GameStateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>
{
	// Token: 0x06000393 RID: 915 RVA: 0x0001C0D3 File Offset: 0x0001A2D3
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToEat, new StateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>.Transition.ConditionCallback(HiveEatingMonitor.ShouldEat), null);
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0001C0FB File Offset: 0x0001A2FB
	public static bool ShouldEat(HiveEatingMonitor.Instance smi)
	{
		return smi.storage.FindFirst(smi.def.consumedOre) != null;
	}

	// Token: 0x02000EDC RID: 3804
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005461 RID: 21601
		public Tag consumedOre;
	}

	// Token: 0x02000EDD RID: 3805
	public new class Instance : GameStateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>.GameInstance
	{
		// Token: 0x0600706B RID: 28779 RVA: 0x002BACDB File Offset: 0x002B8EDB
		public Instance(IStateMachineTarget master, HiveEatingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x04005462 RID: 21602
		[MyCmpReq]
		public Storage storage;
	}
}
