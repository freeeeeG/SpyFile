using System;
using UnityEngine;

// Token: 0x020003B0 RID: 944
public class EntombedChore : Chore<EntombedChore.StatesInstance>
{
	// Token: 0x060013A8 RID: 5032 RVA: 0x00068818 File Offset: 0x00066A18
	public EntombedChore(IStateMachineTarget target, string entombedAnimOverride) : base(Db.Get().ChoreTypes.Entombed, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EntombedChore.StatesInstance(this, target.gameObject, entombedAnimOverride);
	}

	// Token: 0x02000FF2 RID: 4082
	public class StatesInstance : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.GameInstance
	{
		// Token: 0x0600742F RID: 29743 RVA: 0x002C5CEE File Offset: 0x002C3EEE
		public StatesInstance(EntombedChore master, GameObject entombable, string entombedAnimOverride) : base(master)
		{
			base.sm.entombable.Set(entombable, base.smi, false);
			this.entombedAnimOverride = entombedAnimOverride;
		}

		// Token: 0x06007430 RID: 29744 RVA: 0x002C5D18 File Offset: 0x002C3F18
		public void UpdateFaceEntombed()
		{
			int num = Grid.CellAbove(Grid.PosToCell(base.transform.GetPosition()));
			base.sm.isFaceEntombed.Set(Grid.IsValidCell(num) && Grid.Solid[num], base.smi, false);
		}

		// Token: 0x0400579A RID: 22426
		public string entombedAnimOverride;
	}

	// Token: 0x02000FF3 RID: 4083
	public class States : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore>
	{
		// Token: 0x06007431 RID: 29745 RVA: 0x002C5D6C File Offset: 0x002C3F6C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.entombedbody;
			base.Target(this.entombable);
			this.root.ToggleAnims((EntombedChore.StatesInstance smi) => smi.entombedAnimOverride).Update("IsFaceEntombed", delegate(EntombedChore.StatesInstance smi, float dt)
			{
				smi.UpdateFaceEntombed();
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.EntombedChore, null);
			this.entombedface.PlayAnim("entombed_ceiling", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isFaceEntombed, this.entombedbody, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsFalse);
			this.entombedbody.PlayAnim("entombed_floor", KAnim.PlayMode.Loop).StopMoving().ParamTransition<bool>(this.isFaceEntombed, this.entombedface, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsTrue);
		}

		// Token: 0x0400579B RID: 22427
		public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.BoolParameter isFaceEntombed;

		// Token: 0x0400579C RID: 22428
		public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.TargetParameter entombable;

		// Token: 0x0400579D RID: 22429
		public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedface;

		// Token: 0x0400579E RID: 22430
		public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedbody;
	}
}
