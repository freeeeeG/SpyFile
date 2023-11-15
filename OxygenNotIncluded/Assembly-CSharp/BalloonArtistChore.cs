using System;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class BalloonArtistChore : Chore<BalloonArtistChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001389 RID: 5001 RVA: 0x00066F4C File Offset: 0x0006514C
	public BalloonArtistChore(IStateMachineTarget target)
	{
		Chore.Precondition hasBalloonStallCell = default(Chore.Precondition);
		hasBalloonStallCell.id = "HasBalloonStallCell";
		hasBalloonStallCell.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_BALLOON_STALL_CELL;
		hasBalloonStallCell.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((BalloonArtistChore)data).smi.HasBalloonStallCell();
		};
		this.HasBalloonStallCell = hasBalloonStallCell;
		base..ctor(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime);
		this.showAvailabilityInHoverText = false;
		base.smi = new BalloonArtistChore.StatesInstance(this, target.gameObject);
		base.AddPrecondition(this.HasBalloonStallCell, this);
		base.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		base.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		base.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x00067045 File Offset: 0x00065245
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000A71 RID: 2673
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x04000A72 RID: 2674
	private Chore.Precondition HasBalloonStallCell;

	// Token: 0x02000FDF RID: 4063
	public class States : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore>
	{
		// Token: 0x060073BD RID: 29629 RVA: 0x002C32D0 File Offset: 0x002C14D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.balloonArtist);
			this.root.EventTransition(GameHashes.ScheduleBlocksChanged, this.idle, (BalloonArtistChore.StatesInstance smi) => !smi.IsRecTime());
			this.idle.DoNothing();
			this.goToStand.Transition(null, (BalloonArtistChore.StatesInstance smi) => !smi.HasBalloonStallCell(), UpdateRate.SIM_200ms).MoveTo((BalloonArtistChore.StatesInstance smi) => smi.GetBalloonStallCell(), this.balloonStand, null, false);
			this.balloonStand.ToggleAnims("anim_interacts_balloon_artist_kanim", 0f, "").Enter(delegate(BalloonArtistChore.StatesInstance smi)
			{
				smi.SpawnBalloonStand();
			}).Enter(delegate(BalloonArtistChore.StatesInstance smi)
			{
				this.balloonArtist.GetSMI<BalloonArtist.Instance>(smi).Internal_InitBalloons();
			}).Exit(delegate(BalloonArtistChore.StatesInstance smi)
			{
				smi.DestroyBalloonStand();
			}).DefaultState(this.balloonStand.idle);
			this.balloonStand.idle.PlayAnim("working_pre").QueueAnim("working_loop", true, null).OnSignal(this.giveBalloonOut, this.balloonStand.giveBalloon);
			this.balloonStand.giveBalloon.PlayAnim("working_pst").OnAnimQueueComplete(this.balloonStand.idle);
		}

		// Token: 0x04005725 RID: 22309
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.TargetParameter balloonArtist;

		// Token: 0x04005726 RID: 22310
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.IntParameter balloonsGivenOut = new StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.IntParameter(0);

		// Token: 0x04005727 RID: 22311
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.Signal giveBalloonOut;

		// Token: 0x04005728 RID: 22312
		public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State idle;

		// Token: 0x04005729 RID: 22313
		public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State goToStand;

		// Token: 0x0400572A RID: 22314
		public BalloonArtistChore.States.BalloonStandStates balloonStand;

		// Token: 0x02001FC2 RID: 8130
		public class BalloonStandStates : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State
		{
			// Token: 0x04008EFC RID: 36604
			public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State idle;

			// Token: 0x04008EFD RID: 36605
			public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State giveBalloon;
		}
	}

	// Token: 0x02000FE0 RID: 4064
	public class StatesInstance : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.GameInstance
	{
		// Token: 0x060073C0 RID: 29632 RVA: 0x002C3495 File Offset: 0x002C1695
		public StatesInstance(BalloonArtistChore master, GameObject balloonArtist) : base(master)
		{
			this.balloonArtist = balloonArtist;
			base.sm.balloonArtist.Set(balloonArtist, base.smi, false);
		}

		// Token: 0x060073C1 RID: 29633 RVA: 0x002C34BE File Offset: 0x002C16BE
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x060073C2 RID: 29634 RVA: 0x002C34DF File Offset: 0x002C16DF
		public int GetBalloonStallCell()
		{
			return this.balloonArtistCellSensor.GetCell();
		}

		// Token: 0x060073C3 RID: 29635 RVA: 0x002C34EC File Offset: 0x002C16EC
		public int GetBalloonStallTargetCell()
		{
			return this.balloonArtistCellSensor.GetStandCell();
		}

		// Token: 0x060073C4 RID: 29636 RVA: 0x002C34F9 File Offset: 0x002C16F9
		public bool HasBalloonStallCell()
		{
			if (this.balloonArtistCellSensor == null)
			{
				this.balloonArtistCellSensor = base.GetComponent<Sensors>().GetSensor<BalloonStandCellSensor>();
			}
			return this.balloonArtistCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x060073C5 RID: 29637 RVA: 0x002C352C File Offset: 0x002C172C
		public bool IsSameRoom()
		{
			int cell = Grid.PosToCell(this.balloonArtist);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(this.GetBalloonStallCell());
			return cavityForCell != null && cavityForCell2 != null && cavityForCell.handle == cavityForCell2.handle;
		}

		// Token: 0x060073C6 RID: 29638 RVA: 0x002C3588 File Offset: 0x002C1788
		public void SpawnBalloonStand()
		{
			Vector3 vector = Grid.CellToPos(this.GetBalloonStallTargetCell());
			this.balloonArtist.GetComponent<Facing>().Face(vector);
			this.balloonStand = Util.KInstantiate(Assets.GetPrefab("BalloonStand"), vector, Quaternion.identity, null, null, true, 0);
			this.balloonStand.SetActive(true);
			this.balloonStand.GetComponent<GetBalloonWorkable>().SetBalloonArtist(base.smi);
		}

		// Token: 0x060073C7 RID: 29639 RVA: 0x002C35F8 File Offset: 0x002C17F8
		public void DestroyBalloonStand()
		{
			this.balloonStand.DeleteObject();
		}

		// Token: 0x060073C8 RID: 29640 RVA: 0x002C3605 File Offset: 0x002C1805
		public BalloonOverrideSymbol GetBalloonOverride()
		{
			return this.balloonArtist.GetSMI<BalloonArtist.Instance>().GetCurrentBalloonSymbolOverride();
		}

		// Token: 0x060073C9 RID: 29641 RVA: 0x002C3617 File Offset: 0x002C1817
		public void NextBalloonOverride()
		{
			this.balloonArtist.GetSMI<BalloonArtist.Instance>().ApplyNextBalloonSymbolOverride();
		}

		// Token: 0x060073CA RID: 29642 RVA: 0x002C362C File Offset: 0x002C182C
		public void GiveBalloon(BalloonOverrideSymbol balloonOverride)
		{
			BalloonArtist.Instance smi = this.balloonArtist.GetSMI<BalloonArtist.Instance>();
			smi.GiveBalloon();
			balloonOverride.ApplyTo(smi);
			base.smi.sm.giveBalloonOut.Trigger(base.smi);
		}

		// Token: 0x0400572B RID: 22315
		private BalloonStandCellSensor balloonArtistCellSensor;

		// Token: 0x0400572C RID: 22316
		private GameObject balloonArtist;

		// Token: 0x0400572D RID: 22317
		private GameObject balloonStand;
	}
}
