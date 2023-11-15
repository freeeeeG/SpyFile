using System;
using UnityEngine;

// Token: 0x020003C0 RID: 960
public class PutOnHatChore : Chore<PutOnHatChore.StatesInstance>
{
	// Token: 0x060013ED RID: 5101 RVA: 0x0006A308 File Offset: 0x00068508
	public PutOnHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new PutOnHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001017 RID: 4119
	public class StatesInstance : GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.GameInstance
	{
		// Token: 0x060074A4 RID: 29860 RVA: 0x002C942E File Offset: 0x002C762E
		public StatesInstance(PutOnHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x02001018 RID: 4120
	public class States : GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore>
	{
		// Token: 0x060074A5 RID: 29861 RVA: 0x002C9450 File Offset: 0x002C7650
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.applyHat_pre;
			base.Target(this.duplicant);
			this.applyHat_pre.ToggleAnims("anim_hat_kanim", 0f, "").Enter(delegate(PutOnHatChore.StatesInstance smi)
			{
				this.duplicant.Get(smi).GetComponent<MinionResume>().ApplyTargetHat();
			}).PlayAnim("hat_first").OnAnimQueueComplete(this.applyHat);
			this.applyHat.ToggleAnims("anim_hat_kanim", 0f, "").PlayAnim("working_pst").OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x04005821 RID: 22561
		public StateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.TargetParameter duplicant;

		// Token: 0x04005822 RID: 22562
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State applyHat_pre;

		// Token: 0x04005823 RID: 22563
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State applyHat;

		// Token: 0x04005824 RID: 22564
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State complete;
	}
}
