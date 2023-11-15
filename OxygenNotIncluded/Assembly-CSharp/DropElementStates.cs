using System;
using STRINGS;

// Token: 0x020000BD RID: 189
public class DropElementStates : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>
{
	// Token: 0x0600036D RID: 877 RVA: 0x0001AF74 File Offset: 0x00019174
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.dropping;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.EXPELLING_GAS.NAME, CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.dropping.PlayAnim("dirty").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.Enter("DropElement", delegate(DropElementStates.Instance smi)
		{
			smi.GetSMI<ElementDropperMonitor.Instance>().DropPeriodicElement();
		}).QueueAnim("idle_loop", true, null).BehaviourComplete(GameTags.Creatures.WantsToDropElements, false);
	}

	// Token: 0x04000249 RID: 585
	public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State dropping;

	// Token: 0x0400024A RID: 586
	public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State behaviourcomplete;

	// Token: 0x02000EBD RID: 3773
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EBE RID: 3774
	public new class Instance : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.GameInstance
	{
		// Token: 0x0600702C RID: 28716 RVA: 0x002BA610 File Offset: 0x002B8810
		public Instance(Chore<DropElementStates.Instance> chore, DropElementStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToDropElements);
		}
	}
}
