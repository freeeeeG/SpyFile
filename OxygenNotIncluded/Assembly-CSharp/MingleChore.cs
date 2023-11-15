using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B8 RID: 952
public class MingleChore : Chore<MingleChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x060013D9 RID: 5081 RVA: 0x000699EC File Offset: 0x00067BEC
	public MingleChore(IStateMachineTarget target)
	{
		Chore.Precondition hasMingleCell = default(Chore.Precondition);
		hasMingleCell.id = "HasMingleCell";
		hasMingleCell.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_MINGLE_CELL;
		hasMingleCell.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((MingleChore)data).smi.HasMingleCell();
		};
		this.HasMingleCell = hasMingleCell;
		base..ctor(Db.Get().ChoreTypes.Relax, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime);
		this.showAvailabilityInHoverText = false;
		base.smi = new MingleChore.StatesInstance(this, target.gameObject);
		base.AddPrecondition(this.HasMingleCell, this);
		base.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		base.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		base.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x00069AE5 File Offset: 0x00067CE5
	protected override StatusItem GetStatusItem()
	{
		return Db.Get().DuplicantStatusItems.Mingling;
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x00069AF6 File Offset: 0x00067CF6
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000ABB RID: 2747
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x04000ABC RID: 2748
	private Chore.Precondition HasMingleCell;

	// Token: 0x02001005 RID: 4101
	public class States : GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore>
	{
		// Token: 0x06007470 RID: 29808 RVA: 0x002C8300 File Offset: 0x002C6500
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.mingle;
			base.Target(this.mingler);
			this.root.EventTransition(GameHashes.ScheduleBlocksChanged, null, (MingleChore.StatesInstance smi) => !smi.IsRecTime());
			this.mingle.Transition(this.walk, (MingleChore.StatesInstance smi) => smi.IsSameRoom(), UpdateRate.SIM_200ms).Transition(this.move, (MingleChore.StatesInstance smi) => !smi.IsSameRoom(), UpdateRate.SIM_200ms);
			this.move.Transition(null, (MingleChore.StatesInstance smi) => !smi.HasMingleCell(), UpdateRate.SIM_200ms).MoveTo((MingleChore.StatesInstance smi) => smi.GetMingleCell(), this.onfloor, null, false);
			this.walk.Transition(null, (MingleChore.StatesInstance smi) => !smi.HasMingleCell(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_walk_kanim", 0f, "").MoveTo((MingleChore.StatesInstance smi) => smi.GetMingleCell(), this.onfloor, null, false);
			this.onfloor.ToggleAnims("anim_generic_convo_kanim", 0f, "").PlayAnim("idle", KAnim.PlayMode.Loop).ScheduleGoTo((MingleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 10), this.success).ToggleTag(GameTags.AlwaysConverse);
			this.success.ReturnSuccess();
		}

		// Token: 0x040057EA RID: 22506
		public StateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.TargetParameter mingler;

		// Token: 0x040057EB RID: 22507
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State mingle;

		// Token: 0x040057EC RID: 22508
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State move;

		// Token: 0x040057ED RID: 22509
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State walk;

		// Token: 0x040057EE RID: 22510
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State onfloor;

		// Token: 0x040057EF RID: 22511
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State success;
	}

	// Token: 0x02001006 RID: 4102
	public class StatesInstance : GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.GameInstance
	{
		// Token: 0x06007472 RID: 29810 RVA: 0x002C84F9 File Offset: 0x002C66F9
		public StatesInstance(MingleChore master, GameObject mingler) : base(master)
		{
			this.mingler = mingler;
			base.sm.mingler.Set(mingler, base.smi, false);
			this.mingleCellSensor = base.GetComponent<Sensors>().GetSensor<MingleCellSensor>();
		}

		// Token: 0x06007473 RID: 29811 RVA: 0x002C8533 File Offset: 0x002C6733
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06007474 RID: 29812 RVA: 0x002C8554 File Offset: 0x002C6754
		public int GetMingleCell()
		{
			return this.mingleCellSensor.GetCell();
		}

		// Token: 0x06007475 RID: 29813 RVA: 0x002C8561 File Offset: 0x002C6761
		public bool HasMingleCell()
		{
			return this.mingleCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x06007476 RID: 29814 RVA: 0x002C8578 File Offset: 0x002C6778
		public bool IsSameRoom()
		{
			int cell = Grid.PosToCell(this.mingler);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(this.GetMingleCell());
			return cavityForCell != null && cavityForCell2 != null && cavityForCell.handle == cavityForCell2.handle;
		}

		// Token: 0x040057F0 RID: 22512
		private MingleCellSensor mingleCellSensor;

		// Token: 0x040057F1 RID: 22513
		private GameObject mingler;
	}
}
