using System;
using UnityEngine;

// Token: 0x020003BC RID: 956
public class MoveToQuarantineChore : Chore<MoveToQuarantineChore.StatesInstance>
{
	// Token: 0x060013E6 RID: 5094 RVA: 0x0006A0AC File Offset: 0x000682AC
	public MoveToQuarantineChore(IStateMachineTarget target, KMonoBehaviour quarantine_area) : base(Db.Get().ChoreTypes.MoveToQuarantine, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveToQuarantineChore.StatesInstance(this, target.gameObject);
		base.smi.sm.locator.Set(quarantine_area.gameObject, base.smi, false);
	}

	// Token: 0x0200100F RID: 4111
	public class StatesInstance : GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.GameInstance
	{
		// Token: 0x06007493 RID: 29843 RVA: 0x002C8EC1 File Offset: 0x002C70C1
		public StatesInstance(MoveToQuarantineChore master, GameObject quarantined) : base(master)
		{
			base.sm.quarantined.Set(quarantined, base.smi, false);
		}
	}

	// Token: 0x02001010 RID: 4112
	public class States : GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore>
	{
		// Token: 0x06007494 RID: 29844 RVA: 0x002C8EE3 File Offset: 0x002C70E3
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			this.approach.InitializeStates(this.quarantined, this.locator, this.success, null, null, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x0400580E RID: 22542
		public StateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.TargetParameter locator;

		// Token: 0x0400580F RID: 22543
		public StateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.TargetParameter quarantined;

		// Token: 0x04005810 RID: 22544
		public GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x04005811 RID: 22545
		public GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.State success;
	}
}
