using System;
using KSerialization;
using STRINGS;

// Token: 0x020000DB RID: 219
public class SameSpotPoopStates : GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>
{
	// Token: 0x060003E5 RID: 997 RVA: 0x0001E29C File Offset: 0x0001C49C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtopoop;
		this.root.Enter("SetTarget", delegate(SameSpotPoopStates.Instance smi)
		{
			this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi, false);
		});
		this.goingtopoop.MoveTo((SameSpotPoopStates.Instance smi) => smi.GetLastPoopCell(), this.pooping, this.updatepoopcell, false);
		this.pooping.PlayAnim("poop").ToggleStatusItem(CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).OnAnimQueueComplete(this.behaviourcomplete);
		this.updatepoopcell.Enter(delegate(SameSpotPoopStates.Instance smi)
		{
			smi.SetLastPoopCell();
		}).GoTo(this.pooping);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop, false);
	}

	// Token: 0x04000293 RID: 659
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State goingtopoop;

	// Token: 0x04000294 RID: 660
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State pooping;

	// Token: 0x04000295 RID: 661
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State behaviourcomplete;

	// Token: 0x04000296 RID: 662
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State updatepoopcell;

	// Token: 0x04000297 RID: 663
	public StateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.IntParameter targetCell;

	// Token: 0x02000F17 RID: 3863
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F18 RID: 3864
	public new class Instance : GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.GameInstance
	{
		// Token: 0x06007103 RID: 28931 RVA: 0x002BC082 File Offset: 0x002BA282
		public Instance(Chore<SameSpotPoopStates.Instance> chore, SameSpotPoopStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Poop);
		}

		// Token: 0x06007104 RID: 28932 RVA: 0x002BC0AD File Offset: 0x002BA2AD
		public int GetLastPoopCell()
		{
			if (this.lastPoopCell == -1)
			{
				this.SetLastPoopCell();
			}
			return this.lastPoopCell;
		}

		// Token: 0x06007105 RID: 28933 RVA: 0x002BC0C4 File Offset: 0x002BA2C4
		public void SetLastPoopCell()
		{
			this.lastPoopCell = Grid.PosToCell(this);
		}

		// Token: 0x040054EC RID: 21740
		[Serialize]
		private int lastPoopCell = -1;
	}
}
