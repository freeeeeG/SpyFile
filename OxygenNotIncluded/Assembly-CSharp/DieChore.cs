using System;

// Token: 0x020003AD RID: 941
public class DieChore : Chore<DieChore.StatesInstance>
{
	// Token: 0x0600139C RID: 5020 RVA: 0x000683AC File Offset: 0x000665AC
	public DieChore(IStateMachineTarget master, Death death) : base(Db.Get().ChoreTypes.Die, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new DieChore.StatesInstance(this, death);
	}

	// Token: 0x02000FEB RID: 4075
	public class StatesInstance : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.GameInstance
	{
		// Token: 0x06007418 RID: 29720 RVA: 0x002C5414 File Offset: 0x002C3614
		public StatesInstance(DieChore master, Death death) : base(master)
		{
			base.sm.death.Set(death, base.smi, false);
		}

		// Token: 0x06007419 RID: 29721 RVA: 0x002C5438 File Offset: 0x002C3638
		public void PlayPreAnim()
		{
			string preAnim = base.sm.death.Get(base.smi).preAnim;
			base.GetComponent<KAnimControllerBase>().Play(preAnim, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x02000FEC RID: 4076
	public class States : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore>
	{
		// Token: 0x0600741A RID: 29722 RVA: 0x002C5480 File Offset: 0x002C3680
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.dying;
			this.dying.OnAnimQueueComplete(this.dead).Enter("PlayAnim", delegate(DieChore.StatesInstance smi)
			{
				smi.PlayPreAnim();
			});
			this.dead.ReturnSuccess();
		}

		// Token: 0x04005786 RID: 22406
		public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dying;

		// Token: 0x04005787 RID: 22407
		public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dead;

		// Token: 0x04005788 RID: 22408
		public StateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.ResourceParameter<Death> death;
	}
}
