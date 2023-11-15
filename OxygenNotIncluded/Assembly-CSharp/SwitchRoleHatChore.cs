﻿using System;
using UnityEngine;

// Token: 0x020003CA RID: 970
public class SwitchRoleHatChore : Chore<SwitchRoleHatChore.StatesInstance>
{
	// Token: 0x06001408 RID: 5128 RVA: 0x0006AD70 File Offset: 0x00068F70
	public SwitchRoleHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SwitchRoleHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200102F RID: 4143
	public class StatesInstance : GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.GameInstance
	{
		// Token: 0x060074F9 RID: 29945 RVA: 0x002CB390 File Offset: 0x002C9590
		public StatesInstance(SwitchRoleHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x02001030 RID: 4144
	public class States : GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore>
	{
		// Token: 0x060074FA RID: 29946 RVA: 0x002CB3B4 File Offset: 0x002C95B4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.start;
			base.Target(this.duplicant);
			this.start.Enter(delegate(SwitchRoleHatChore.StatesInstance smi)
			{
				if (this.duplicant.Get(smi).GetComponent<MinionResume>().CurrentHat == null)
				{
					smi.GoTo(this.delay);
					return;
				}
				smi.GoTo(this.remove_hat);
			});
			this.remove_hat.ToggleAnims("anim_hat_kanim", 0f, "").PlayAnim("hat_off").OnAnimQueueComplete(this.delay);
			this.delay.ToggleThought(Db.Get().Thoughts.NewRole, null).ToggleExpression(Db.Get().Expressions.Happy, null).ToggleAnims("anim_selfish_kanim", 0f, "").QueueAnim("working_pre", false, null).QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.applyHat_pre);
			this.applyHat_pre.ToggleAnims("anim_hat_kanim", 0f, "").Enter(delegate(SwitchRoleHatChore.StatesInstance smi)
			{
				this.duplicant.Get(smi).GetComponent<MinionResume>().ApplyTargetHat();
			}).PlayAnim("hat_first").OnAnimQueueComplete(this.applyHat);
			this.applyHat.ToggleAnims("anim_hat_kanim", 0f, "").PlayAnim("working_pst").OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x04005862 RID: 22626
		public StateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.TargetParameter duplicant;

		// Token: 0x04005863 RID: 22627
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State remove_hat;

		// Token: 0x04005864 RID: 22628
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State start;

		// Token: 0x04005865 RID: 22629
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State delay;

		// Token: 0x04005866 RID: 22630
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State delay_pst;

		// Token: 0x04005867 RID: 22631
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State applyHat_pre;

		// Token: 0x04005868 RID: 22632
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State applyHat;

		// Token: 0x04005869 RID: 22633
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State complete;
	}
}
