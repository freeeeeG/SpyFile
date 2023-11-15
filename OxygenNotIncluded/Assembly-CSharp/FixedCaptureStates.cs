using System;
using STRINGS;

// Token: 0x020000C2 RID: 194
public class FixedCaptureStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>
{
	// Token: 0x0600037F RID: 895 RVA: 0x0001B680 File Offset: 0x00019880
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.capture;
		this.root.Exit("AbandonedCapturePoint", delegate(FixedCaptureStates.Instance smi)
		{
			smi.AbandonedCapturePoint();
		});
		this.capture.EventTransition(GameHashes.CapturePointNoLongerAvailable, null, null).DefaultState(this.capture.cheer);
		this.capture.cheer.DefaultState(this.capture.cheer.pre).ToggleStatusItem(CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME, CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.capture.cheer.pre.ScheduleGoTo(0.9f, this.capture.cheer.cheer);
		this.capture.cheer.cheer.Enter("FaceRancher", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetComponent<Facing>().Face(smi.GetCapturePoint().transform.GetPosition());
		}).PlayAnim("excited_loop").OnAnimQueueComplete(this.capture.cheer.pst);
		this.capture.cheer.pst.ScheduleGoTo(0.2f, this.capture.move);
		this.capture.move.DefaultState(this.capture.move.movetoranch).ToggleStatusItem(CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME, CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.capture.move.movetoranch.Enter("Speedup", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed * 1.25f;
		}).MoveTo(new Func<FixedCaptureStates.Instance, int>(FixedCaptureStates.GetTargetCaptureCell), this.capture.move.waitforranchertobeready, null, false).Exit("RestoreSpeed", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed;
		});
		this.capture.move.waitforranchertobeready.Enter("SetCreatureAtRanchingStation", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetCapturePoint().Trigger(-1992722293, null);
		}).EventTransition(GameHashes.RancherReadyAtCapturePoint, this.capture.ranching, null);
		this.capture.ranching.ToggleStatusItem(CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME, CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToGetCaptured, false);
	}

	// Token: 0x06000380 RID: 896 RVA: 0x0001B988 File Offset: 0x00019B88
	private static FixedCapturePoint.Instance GetCapturePoint(FixedCaptureStates.Instance smi)
	{
		return smi.GetSMI<FixedCapturableMonitor.Instance>().targetCapturePoint;
	}

	// Token: 0x06000381 RID: 897 RVA: 0x0001B998 File Offset: 0x00019B98
	private static int GetTargetCaptureCell(FixedCaptureStates.Instance smi)
	{
		FixedCapturePoint.Instance capturePoint = FixedCaptureStates.GetCapturePoint(smi);
		return capturePoint.def.getTargetCapturePoint(capturePoint);
	}

	// Token: 0x04000257 RID: 599
	private FixedCaptureStates.CaptureStates capture;

	// Token: 0x04000258 RID: 600
	private GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State behaviourcomplete;

	// Token: 0x02000ECC RID: 3788
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000ECD RID: 3789
	public new class Instance : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.GameInstance
	{
		// Token: 0x06007045 RID: 28741 RVA: 0x002BA7C1 File Offset: 0x002B89C1
		public Instance(Chore<FixedCaptureStates.Instance> chore, FixedCaptureStates.Def def) : base(chore, def)
		{
			this.originalSpeed = base.GetComponent<Navigator>().defaultSpeed;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToGetCaptured);
		}

		// Token: 0x06007046 RID: 28742 RVA: 0x002BA7F8 File Offset: 0x002B89F8
		public FixedCapturePoint.Instance GetCapturePoint()
		{
			FixedCapturableMonitor.Instance smi = this.GetSMI<FixedCapturableMonitor.Instance>();
			if (smi == null)
			{
				return null;
			}
			return smi.targetCapturePoint;
		}

		// Token: 0x06007047 RID: 28743 RVA: 0x002BA817 File Offset: 0x002B8A17
		public void AbandonedCapturePoint()
		{
			if (this.GetCapturePoint() != null)
			{
				this.GetCapturePoint().Trigger(-1000356449, null);
			}
		}

		// Token: 0x04005446 RID: 21574
		public float originalSpeed;
	}

	// Token: 0x02000ECE RID: 3790
	public class CaptureStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
	{
		// Token: 0x04005447 RID: 21575
		public FixedCaptureStates.CaptureStates.CheerStates cheer;

		// Token: 0x04005448 RID: 21576
		public FixedCaptureStates.CaptureStates.MoveStates move;

		// Token: 0x04005449 RID: 21577
		public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State ranching;

		// Token: 0x02001FA8 RID: 8104
		public class CheerStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
		{
			// Token: 0x04008EBC RID: 36540
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State pre;

			// Token: 0x04008EBD RID: 36541
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State cheer;

			// Token: 0x04008EBE RID: 36542
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State pst;
		}

		// Token: 0x02001FA9 RID: 8105
		public class MoveStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
		{
			// Token: 0x04008EBF RID: 36543
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State movetoranch;

			// Token: 0x04008EC0 RID: 36544
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State waitforranchertobeready;
		}
	}
}
