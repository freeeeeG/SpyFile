using System;

// Token: 0x020003AC RID: 940
public class DeliverFoodChore : Chore<DeliverFoodChore.StatesInstance>
{
	// Token: 0x0600139A RID: 5018 RVA: 0x00068290 File Offset: 0x00066490
	public DeliverFoodChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.DeliverFood, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new DeliverFoodChore.StatesInstance(this);
		base.AddPrecondition(ChorePreconditions.instance.IsChattable, target);
	}

	// Token: 0x0600139B RID: 5019 RVA: 0x000682E4 File Offset: 0x000664E4
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.requestedrationcount.Set(base.smi.GetComponent<StateMachineController>().GetSMI<RationMonitor.Instance>().GetRationsRemaining(), base.smi, false);
		base.smi.sm.ediblesource.Set(context.consumerState.gameObject.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().GetEdible(), base.smi);
		base.smi.sm.deliverypoint.Set(this.gameObject, base.smi, false);
		base.smi.sm.deliverer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x02000FE9 RID: 4073
	public class StatesInstance : GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.GameInstance
	{
		// Token: 0x06007415 RID: 29717 RVA: 0x002C536A File Offset: 0x002C356A
		public StatesInstance(DeliverFoodChore master) : base(master)
		{
		}
	}

	// Token: 0x02000FEA RID: 4074
	public class States : GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore>
	{
		// Token: 0x06007416 RID: 29718 RVA: 0x002C5374 File Offset: 0x002C3574
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			this.fetch.InitializeStates(this.deliverer, this.ediblesource, this.ediblechunk, this.requestedrationcount, this.actualrationcount, this.movetodeliverypoint, null);
			this.movetodeliverypoint.InitializeStates(this.deliverer, this.deliverypoint, this.drop, null, null, null);
			this.drop.InitializeStates(this.deliverer, this.ediblechunk, this.deliverypoint, this.success, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x0400577C RID: 22396
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter deliverer;

		// Token: 0x0400577D RID: 22397
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter ediblesource;

		// Token: 0x0400577E RID: 22398
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter ediblechunk;

		// Token: 0x0400577F RID: 22399
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter deliverypoint;

		// Token: 0x04005780 RID: 22400
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FloatParameter requestedrationcount;

		// Token: 0x04005781 RID: 22401
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FloatParameter actualrationcount;

		// Token: 0x04005782 RID: 22402
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FetchSubState fetch;

		// Token: 0x04005783 RID: 22403
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.ApproachSubState<Chattable> movetodeliverypoint;

		// Token: 0x04005784 RID: 22404
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.DropSubState drop;

		// Token: 0x04005785 RID: 22405
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.State success;
	}
}
