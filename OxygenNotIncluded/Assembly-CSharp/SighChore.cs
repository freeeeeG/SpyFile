using System;
using UnityEngine;

// Token: 0x020003C6 RID: 966
public class SighChore : Chore<SighChore.StatesInstance>
{
	// Token: 0x060013FF RID: 5119 RVA: 0x0006AA94 File Offset: 0x00068C94
	public SighChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Sigh, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SighChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001026 RID: 4134
	public class StatesInstance : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.GameInstance
	{
		// Token: 0x060074DB RID: 29915 RVA: 0x002CA79E File Offset: 0x002C899E
		public StatesInstance(SighChore master, GameObject sigher) : base(master)
		{
			base.sm.sigher.Set(sigher, base.smi, false);
		}
	}

	// Token: 0x02001027 RID: 4135
	public class States : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore>
	{
		// Token: 0x060074DC RID: 29916 RVA: 0x002CA7C0 File Offset: 0x002C89C0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.sigher);
			this.root.PlayAnim("emote_depressed").OnAnimQueueComplete(null);
		}

		// Token: 0x04005847 RID: 22599
		public StateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.TargetParameter sigher;
	}
}
