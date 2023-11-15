using System;
using UnityEngine;

// Token: 0x0200043B RID: 1083
public class DropUnusedInventoryChore : Chore<DropUnusedInventoryChore.StatesInstance>
{
	// Token: 0x060016E3 RID: 5859 RVA: 0x00076890 File Offset: 0x00074A90
	public DropUnusedInventoryChore(ChoreType chore_type, IStateMachineTarget target) : base(chore_type, target, target.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new DropUnusedInventoryChore.StatesInstance(this);
	}

	// Token: 0x020010CA RID: 4298
	public class StatesInstance : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.GameInstance
	{
		// Token: 0x0600777D RID: 30589 RVA: 0x002D3E8F File Offset: 0x002D208F
		public StatesInstance(DropUnusedInventoryChore master) : base(master)
		{
		}
	}

	// Token: 0x020010CB RID: 4299
	public class States : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore>
	{
		// Token: 0x0600777E RID: 30590 RVA: 0x002D3E98 File Offset: 0x002D2098
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.dropping;
			this.dropping.Enter(delegate(DropUnusedInventoryChore.StatesInstance smi)
			{
				smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
			}).GoTo(this.success);
			this.success.ReturnSuccess();
		}

		// Token: 0x04005A16 RID: 23062
		public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State dropping;

		// Token: 0x04005A17 RID: 23063
		public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State success;
	}
}
