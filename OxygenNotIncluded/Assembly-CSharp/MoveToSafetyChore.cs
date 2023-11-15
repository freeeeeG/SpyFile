using System;
using UnityEngine;

// Token: 0x020003BD RID: 957
public class MoveToSafetyChore : Chore<MoveToSafetyChore.StatesInstance>
{
	// Token: 0x060013E7 RID: 5095 RVA: 0x0006A118 File Offset: 0x00068318
	public MoveToSafetyChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.MoveToSafety, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.idle, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveToSafetyChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001011 RID: 4113
	public class StatesInstance : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.GameInstance
	{
		// Token: 0x06007496 RID: 29846 RVA: 0x002C8F24 File Offset: 0x002C7124
		public StatesInstance(MoveToSafetyChore master, GameObject mover) : base(master)
		{
			base.sm.mover.Set(mover, base.smi, false);
			this.sensor = base.sm.mover.Get<Sensors>(base.smi).GetSensor<SafeCellSensor>();
			this.targetCell = this.sensor.GetSensorCell();
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x002C8F83 File Offset: 0x002C7183
		public void UpdateTargetCell()
		{
			this.targetCell = this.sensor.GetSensorCell();
		}

		// Token: 0x04005812 RID: 22546
		private SafeCellSensor sensor;

		// Token: 0x04005813 RID: 22547
		public int targetCell;
	}

	// Token: 0x02001012 RID: 4114
	public class States : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore>
	{
		// Token: 0x06007498 RID: 29848 RVA: 0x002C8F98 File Offset: 0x002C7198
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.move;
			base.Target(this.mover);
			this.root.ToggleTag(GameTags.Idle);
			this.move.Enter("UpdateLocatorPosition", delegate(MoveToSafetyChore.StatesInstance smi)
			{
				smi.UpdateTargetCell();
			}).Update("UpdateLocatorPosition", delegate(MoveToSafetyChore.StatesInstance smi, float dt)
			{
				smi.UpdateTargetCell();
			}, UpdateRate.SIM_200ms, false).MoveTo((MoveToSafetyChore.StatesInstance smi) => smi.targetCell, null, null, true);
		}

		// Token: 0x04005814 RID: 22548
		public StateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.TargetParameter mover;

		// Token: 0x04005815 RID: 22549
		public GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.State move;
	}
}
