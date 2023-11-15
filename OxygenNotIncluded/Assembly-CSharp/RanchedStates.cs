using System;
using STRINGS;

// Token: 0x020000D9 RID: 217
public class RanchedStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>
{
	// Token: 0x060003DB RID: 987 RVA: 0x0001DB90 File Offset: 0x0001BD90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.ranch;
		this.root.Exit("AbandonedRanchStation", delegate(RanchedStates.Instance smi)
		{
			if (smi.Monitor.TargetRanchStation != null)
			{
				if (smi.Monitor.TargetRanchStation.IsCritterInQueue(smi.Monitor))
				{
					Debug.LogWarning("Why are we exiting RanchedStates while in the queue?");
					smi.Monitor.TargetRanchStation.Abandon(smi.Monitor);
				}
				smi.Monitor.TargetRanchStation = null;
			}
			smi.sm.ranchTarget.Set(null, smi);
		});
		this.ranch.EnterTransition(this.ranch.Cheer, (RanchedStates.Instance smi) => RanchedStates.IsCrittersTurn(smi)).EventHandler(GameHashes.RanchStationNoLongerAvailable, delegate(RanchedStates.Instance smi)
		{
			smi.GoTo(null);
		}).BehaviourComplete(GameTags.Creatures.WantsToGetRanched, true).Update(delegate(RanchedStates.Instance smi, float deltaSeconds)
		{
			RanchStation.Instance ranchStation = smi.GetRanchStation();
			if (ranchStation.IsNullOrDestroyed())
			{
				smi.StopSM("No more target ranch station.");
				return;
			}
			Option<CavityInfo> option = Option.Maybe<CavityInfo>(Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi)));
			Option<CavityInfo> cavityInfo = ranchStation.GetCavityInfo();
			if (option.IsNone() || cavityInfo.IsNone())
			{
				smi.StopSM("No longer in any cavity.");
				return;
			}
			if (option.Unwrap() != cavityInfo.Unwrap())
			{
				smi.StopSM("Critter is in a different cavity");
				return;
			}
		}, UpdateRate.SIM_200ms, false).EventHandler(GameHashes.RancherReadyAtRanchStation, delegate(RanchedStates.Instance smi)
		{
			smi.UpdateWaitingState();
		}).Exit(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.ClearLayerOverride));
		this.ranch.Cheer.ToggleStatusItem(CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.NAME, CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter("FaceRancher", delegate(RanchedStates.Instance smi)
		{
			smi.GetComponent<Facing>().Face(smi.GetRanchStation().transform.GetPosition());
		}).PlayAnim("excited_loop").OnAnimQueueComplete(this.ranch.Cheer.Pst);
		this.ranch.Cheer.Pst.ScheduleGoTo(0.2f, this.ranch.Move);
		this.ranch.Move.DefaultState(this.ranch.Move.MoveToRanch).Enter("Speedup", delegate(RanchedStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.OriginalSpeed * 1.25f;
		}).ToggleStatusItem(CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.NAME, CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Exit("RestoreSpeed", delegate(RanchedStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.OriginalSpeed;
		});
		this.ranch.Move.MoveToRanch.EnterTransition(this.ranch.Wait.WaitInLine, GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Not(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback(RanchedStates.IsCrittersTurn))).MoveTo(new Func<RanchedStates.Instance, int>(RanchedStates.GetRanchNavTarget), this.ranch.Wait.WaitInLine, null, false).Target(this.ranchTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, this.ranch.Wait.WaitInLine, (RanchedStates.Instance smi) => !RanchedStates.IsCrittersTurn(smi));
		this.ranch.Wait.WaitInLine.EnterTransition(this.ranch.Ranching, new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback(RanchedStates.IsCrittersTurn)).Enter(delegate(RanchedStates.Instance smi)
		{
			smi.EnterQueue();
		}).EventTransition(GameHashes.DestinationReached, this.ranch.Wait.Waiting, null);
		this.ranch.Wait.Waiting.Face(this.ranchTarget, 0f).PlayAnim((RanchedStates.Instance smi) => smi.def.StartWaitingAnim, KAnim.PlayMode.Once).QueueAnim((RanchedStates.Instance smi) => smi.def.WaitingAnim, true, null);
		this.ranch.Wait.DoneWaiting.PlayAnim((RanchedStates.Instance smi) => smi.def.EndWaitingAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.ranch.Move.MoveToRanch);
		this.ranch.Ranching.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.GetOnTable)).Enter("SetCreatureAtRanchingStation", delegate(RanchedStates.Instance smi)
		{
			smi.GetRanchStation().MessageCreatureArrived(smi);
			smi.AnimController.SetSceneLayer(Grid.SceneLayer.BuildingUse);
		}).EventTransition(GameHashes.RanchingComplete, this.ranch.Wavegoodbye, null).ToggleMainStatusItem(delegate(RanchedStates.Instance smi)
		{
			RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
			if (ranchStation != null)
			{
				return ranchStation.def.CreatureRanchingStatusItem;
			}
			return Db.Get().CreatureStatusItems.GettingRanched;
		}, null);
		this.ranch.Wavegoodbye.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.ClearLayerOverride)).OnAnimQueueComplete(this.ranch.Runaway).ToggleStatusItem(CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME, CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.ranch.Runaway.MoveTo(new Func<RanchedStates.Instance, int>(RanchedStates.GetRunawayCell), null, null, false).ToggleStatusItem(CREATURES.STATUSITEMS.IDLE.NAME, CREATURES.STATUSITEMS.IDLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0001E11B File Offset: 0x0001C31B
	private static void ClearLayerOverride(RanchedStates.Instance smi)
	{
		smi.AnimController.SetSceneLayer(Grid.SceneLayer.Creatures);
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0001E12A File Offset: 0x0001C32A
	private static RanchStation.Instance GetRanchStation(RanchedStates.Instance smi)
	{
		return smi.GetRanchStation();
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0001E134 File Offset: 0x0001C334
	private static void GetOnTable(RanchedStates.Instance smi)
	{
		Navigator navigator = smi.Get<Navigator>();
		if (navigator.IsValidNavType(NavType.Floor))
		{
			navigator.SetCurrentNavType(NavType.Floor);
		}
		smi.Get<Facing>().SetFacing(false);
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0001E164 File Offset: 0x0001C364
	private static bool IsCrittersTurn(RanchedStates.Instance smi)
	{
		RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
		return ranchStation != null && ranchStation.IsRancherReady && ranchStation.TryGetRanched(smi);
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x0001E190 File Offset: 0x0001C390
	private static int GetRanchNavTarget(RanchedStates.Instance smi)
	{
		RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
		return smi.ModifyNavTargetForCritter(ranchStation.GetRanchNavTarget());
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0001E1B0 File Offset: 0x0001C3B0
	private static int GetRunawayCell(RanchedStates.Instance smi)
	{
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		int num = Grid.OffsetCell(cell, 2, 0);
		if (Grid.Solid[num])
		{
			num = Grid.OffsetCell(cell, -2, 0);
		}
		return num;
	}

	// Token: 0x0400028F RID: 655
	private RanchedStates.RanchStates ranch;

	// Token: 0x04000290 RID: 656
	private StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.TargetParameter ranchTarget;

	// Token: 0x02000F0E RID: 3854
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040054C8 RID: 21704
		public string StartWaitingAnim = "queue_pre";

		// Token: 0x040054C9 RID: 21705
		public string WaitingAnim = "queue_loop";

		// Token: 0x040054CA RID: 21706
		public string EndWaitingAnim = "queue_pst";

		// Token: 0x040054CB RID: 21707
		public int WaitCellOffset = 1;
	}

	// Token: 0x02000F0F RID: 3855
	public new class Instance : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.GameInstance
	{
		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x060070E1 RID: 28897 RVA: 0x002BBB18 File Offset: 0x002B9D18
		public RanchableMonitor.Instance Monitor
		{
			get
			{
				if (this.ranchMonitor == null)
				{
					this.ranchMonitor = this.GetSMI<RanchableMonitor.Instance>();
				}
				return this.ranchMonitor;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x060070E2 RID: 28898 RVA: 0x002BBB34 File Offset: 0x002B9D34
		public KBatchedAnimController AnimController
		{
			get
			{
				return this.animController;
			}
		}

		// Token: 0x060070E3 RID: 28899 RVA: 0x002BBB3C File Offset: 0x002B9D3C
		public Instance(Chore<RanchedStates.Instance> chore, RanchedStates.Def def) : base(chore, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			this.OriginalSpeed = this.Monitor.NavComponent.defaultSpeed;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToGetRanched);
		}

		// Token: 0x060070E4 RID: 28900 RVA: 0x002BBB8D File Offset: 0x002B9D8D
		public RanchStation.Instance GetRanchStation()
		{
			if (this.Monitor != null)
			{
				return this.Monitor.TargetRanchStation;
			}
			return null;
		}

		// Token: 0x060070E5 RID: 28901 RVA: 0x002BBBA4 File Offset: 0x002B9DA4
		public void EnterQueue()
		{
			if (this.GetRanchStation() != null)
			{
				this.InitializeWaitCell();
				this.Monitor.NavComponent.GoTo(this.waitCell, null);
			}
		}

		// Token: 0x060070E6 RID: 28902 RVA: 0x002BBBCC File Offset: 0x002B9DCC
		public void AbandonRanchStation()
		{
			if (this.Monitor.TargetRanchStation == null || this.status == StateMachine.Status.Failed)
			{
				return;
			}
			this.StopSM("Abandoned Ranch");
		}

		// Token: 0x060070E7 RID: 28903 RVA: 0x002BBBF0 File Offset: 0x002B9DF0
		public void SetRanchStation(RanchStation.Instance ranch_station)
		{
			if (this.Monitor.TargetRanchStation != null && this.Monitor.TargetRanchStation != ranch_station)
			{
				this.Monitor.TargetRanchStation.Abandon(base.smi.Monitor);
			}
			base.smi.sm.ranchTarget.Set(ranch_station.gameObject, base.smi, false);
			this.Monitor.TargetRanchStation = ranch_station;
		}

		// Token: 0x060070E8 RID: 28904 RVA: 0x002BBC62 File Offset: 0x002B9E62
		public int ModifyNavTargetForCritter(int navCell)
		{
			if (base.smi.HasTag(GameTags.Creatures.Flyer))
			{
				return Grid.CellAbove(navCell);
			}
			return navCell;
		}

		// Token: 0x060070E9 RID: 28905 RVA: 0x002BBC80 File Offset: 0x002B9E80
		private void InitializeWaitCell()
		{
			if (this.GetRanchStation() == null)
			{
				return;
			}
			int cell = 0;
			Extents stationExtents = this.Monitor.TargetRanchStation.StationExtents;
			int cell2 = this.ModifyNavTargetForCritter(Grid.XYToCell(stationExtents.x, stationExtents.y));
			int num = 0;
			int num2;
			if (Grid.Raycast(cell2, new Vector2I(-1, 0), out num2, base.def.WaitCellOffset, ~(Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable)))
			{
				num = 1 + base.def.WaitCellOffset - num2;
				cell = this.ModifyNavTargetForCritter(Grid.XYToCell(stationExtents.x + 1, stationExtents.y));
			}
			int num3 = 0;
			int num4;
			if (num != 0 && Grid.Raycast(cell, new Vector2I(1, 0), out num4, base.def.WaitCellOffset, ~(Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable)))
			{
				num3 = base.def.WaitCellOffset - num4;
			}
			int x = (base.def.WaitCellOffset - num) * -1;
			if (num == base.def.WaitCellOffset)
			{
				x = 1 + base.def.WaitCellOffset - num3;
			}
			CellOffset offset = new CellOffset(x, 0);
			this.waitCell = Grid.OffsetCell(cell2, offset);
		}

		// Token: 0x060070EA RID: 28906 RVA: 0x002BBD90 File Offset: 0x002B9F90
		public void UpdateWaitingState()
		{
			if (!RanchedStates.IsCrittersTurn(base.smi))
			{
				base.smi.GoTo(base.smi.sm.ranch.Wait.WaitInLine);
				return;
			}
			if (base.smi.IsInsideState(base.sm.ranch.Wait.Waiting))
			{
				base.smi.GoTo(base.smi.sm.ranch.Wait.DoneWaiting);
				return;
			}
			base.smi.GoTo(base.smi.sm.ranch.Cheer);
		}

		// Token: 0x040054CC RID: 21708
		public float OriginalSpeed;

		// Token: 0x040054CD RID: 21709
		private int waitCell;

		// Token: 0x040054CE RID: 21710
		private KBatchedAnimController animController;

		// Token: 0x040054CF RID: 21711
		private RanchableMonitor.Instance ranchMonitor;
	}

	// Token: 0x02000F10 RID: 3856
	public class RanchStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040054D0 RID: 21712
		public RanchedStates.CheerStates Cheer;

		// Token: 0x040054D1 RID: 21713
		public RanchedStates.MoveStates Move;

		// Token: 0x040054D2 RID: 21714
		public RanchedStates.WaitStates Wait;

		// Token: 0x040054D3 RID: 21715
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Ranching;

		// Token: 0x040054D4 RID: 21716
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Wavegoodbye;

		// Token: 0x040054D5 RID: 21717
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Runaway;
	}

	// Token: 0x02000F11 RID: 3857
	public class CheerStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040054D6 RID: 21718
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Cheer;

		// Token: 0x040054D7 RID: 21719
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Pst;
	}

	// Token: 0x02000F12 RID: 3858
	public class MoveStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040054D8 RID: 21720
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State MoveToRanch;
	}

	// Token: 0x02000F13 RID: 3859
	public class WaitStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040054D9 RID: 21721
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State WaitInLine;

		// Token: 0x040054DA RID: 21722
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Waiting;

		// Token: 0x040054DB RID: 21723
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State DoneWaiting;
	}
}
