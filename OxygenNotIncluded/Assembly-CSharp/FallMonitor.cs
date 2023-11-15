using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
public class FallMonitor : GameStateMachine<FallMonitor, FallMonitor.Instance>
{
	// Token: 0x06001B52 RID: 6994 RVA: 0x00092BA8 File Offset: 0x00090DA8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.standing;
		this.root.TagTransition(GameTags.Stored, this.instorage, false).Update("CheckLanded", delegate(FallMonitor.Instance smi, float dt)
		{
			smi.UpdateFalling();
		}, UpdateRate.SIM_33ms, true);
		this.standing.ParamTransition<bool>(this.isEntombed, this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).ParamTransition<bool>(this.isFalling, this.falling_pre, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue);
		this.falling_pre.Enter("StopNavigator", delegate(FallMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		}).Enter("AttemptInitialRecovery", delegate(FallMonitor.Instance smi)
		{
			smi.AttemptInitialRecovery();
		}).GoTo(this.falling).ToggleBrain("falling_pre");
		this.falling.ToggleBrain("falling").PlayAnim("fall_pre").QueueAnim("fall_loop", true, null).ParamTransition<bool>(this.isEntombed, this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).Transition(this.recoverladder, (FallMonitor.Instance smi) => smi.CanRecoverToLadder(), UpdateRate.SIM_33ms).Transition(this.recoverpole, (FallMonitor.Instance smi) => smi.CanRecoverToPole(), UpdateRate.SIM_33ms).ToggleGravity(this.landfloor);
		this.recoverinitialfall.ToggleBrain("recoverinitialfall").Enter("Recover", delegate(FallMonitor.Instance smi)
		{
			smi.Recover();
		}).EventTransition(GameHashes.DestinationReached, this.standing, null).EventTransition(GameHashes.NavigationFailed, this.standing, null).Exit(delegate(FallMonitor.Instance smi)
		{
			smi.RecoverEmote();
		});
		this.landfloor.Enter("Land", delegate(FallMonitor.Instance smi)
		{
			smi.LandFloor();
		}).GoTo(this.standing);
		this.recoverladder.ToggleBrain("recoverladder").PlayAnim("floor_ladder_0_0").Enter("MountLadder", delegate(FallMonitor.Instance smi)
		{
			smi.MountLadder();
		}).OnAnimQueueComplete(this.standing);
		this.recoverpole.ToggleBrain("recoverpole").PlayAnim("floor_pole_0_0").Enter("MountPole", delegate(FallMonitor.Instance smi)
		{
			smi.MountPole();
		}).OnAnimQueueComplete(this.standing);
		this.instorage.TagTransition(GameTags.Stored, this.standing, true);
		this.entombed.DefaultState(this.entombed.recovering);
		this.entombed.recovering.Enter("TryEntombedEscape", delegate(FallMonitor.Instance smi)
		{
			smi.TryEntombedEscape();
		});
		this.entombed.stuck.Enter("StopNavigator", delegate(FallMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		}).ToggleChore((FallMonitor.Instance smi) => new EntombedChore(smi.master, smi.entombedAnimOverride), this.standing).ParamTransition<bool>(this.isEntombed, this.standing, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsFalse);
	}

	// Token: 0x04000F2E RID: 3886
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State standing;

	// Token: 0x04000F2F RID: 3887
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling_pre;

	// Token: 0x04000F30 RID: 3888
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling;

	// Token: 0x04000F31 RID: 3889
	public FallMonitor.EntombedStates entombed;

	// Token: 0x04000F32 RID: 3890
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverladder;

	// Token: 0x04000F33 RID: 3891
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverpole;

	// Token: 0x04000F34 RID: 3892
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverinitialfall;

	// Token: 0x04000F35 RID: 3893
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State landfloor;

	// Token: 0x04000F36 RID: 3894
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State instorage;

	// Token: 0x04000F37 RID: 3895
	public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isEntombed;

	// Token: 0x04000F38 RID: 3896
	public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isFalling;

	// Token: 0x02001163 RID: 4451
	public class EntombedStates : GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005C1A RID: 23578
		public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recovering;

		// Token: 0x04005C1B RID: 23579
		public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State stuck;
	}

	// Token: 0x02001164 RID: 4452
	public new class Instance : GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007950 RID: 31056 RVA: 0x002D8950 File Offset: 0x002D6B50
		public Instance(IStateMachineTarget master, bool shouldPlayEmotes, string entombedAnimOverride = null) : base(master)
		{
			this.navigator = base.GetComponent<Navigator>();
			this.shouldPlayEmotes = shouldPlayEmotes;
			this.entombedAnimOverride = entombedAnimOverride;
			Pathfinding.Instance.FlushNavGridsOnLoad();
			base.Subscribe(915392638, new Action<object>(this.OnCellChanged));
			base.Subscribe(1027377649, new Action<object>(this.OnMovementStateChanged));
			base.Subscribe(387220196, new Action<object>(this.OnDestinationReached));
		}

		// Token: 0x06007951 RID: 31057 RVA: 0x002D8A50 File Offset: 0x002D6C50
		private void OnDestinationReached(object data)
		{
			int item = Grid.PosToCell(base.transform.GetPosition());
			if (!this.safeCells.Contains(item))
			{
				this.safeCells.Add(item);
				if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
				{
					this.safeCells.RemoveAt(0);
				}
			}
		}

		// Token: 0x06007952 RID: 31058 RVA: 0x002D8AA8 File Offset: 0x002D6CA8
		private void OnMovementStateChanged(object data)
		{
			if ((GameHashes)data == GameHashes.ObjectMovementWakeUp)
			{
				int item = Grid.PosToCell(base.transform.GetPosition());
				if (!this.safeCells.Contains(item))
				{
					this.safeCells.Add(item);
					if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
					{
						this.safeCells.RemoveAt(0);
					}
				}
			}
		}

		// Token: 0x06007953 RID: 31059 RVA: 0x002D8B0C File Offset: 0x002D6D0C
		private void OnCellChanged(object data)
		{
			int item = (int)data;
			if (!this.safeCells.Contains(item))
			{
				this.safeCells.Add(item);
				if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
				{
					this.safeCells.RemoveAt(0);
				}
			}
		}

		// Token: 0x06007954 RID: 31060 RVA: 0x002D8B5C File Offset: 0x002D6D5C
		public void Recover()
		{
			int cell = Grid.PosToCell(this.navigator);
			foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
			{
				if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
				{
					int num = transition.IsValid(cell, this.navigator.NavGrid.NavTable);
					if (Grid.InvalidCell != num)
					{
						Vector2I vector2I = Grid.CellToXY(cell);
						Vector2I vector2I2 = Grid.CellToXY(num);
						this.flipRecoverEmote = (vector2I2.x < vector2I.x);
						this.navigator.BeginTransition(transition);
						return;
					}
				}
			}
		}

		// Token: 0x06007955 RID: 31061 RVA: 0x002D8C14 File Offset: 0x002D6E14
		public void RecoverEmote()
		{
			if (!this.shouldPlayEmotes)
			{
				return;
			}
			if (UnityEngine.Random.Range(0, 9) == 8)
			{
				new EmoteChore(base.master.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.CloseCall_Fall, KAnim.PlayMode.Once, 1, this.flipRecoverEmote);
			}
		}

		// Token: 0x06007956 RID: 31062 RVA: 0x002D8C71 File Offset: 0x002D6E71
		public void LandFloor()
		{
			this.navigator.SetCurrentNavType(NavType.Floor);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x06007957 RID: 31063 RVA: 0x002D8CA4 File Offset: 0x002D6EA4
		public void AttemptInitialRecovery()
		{
			if (base.gameObject.HasTag(GameTags.Incapacitated))
			{
				return;
			}
			int cell = Grid.PosToCell(this.navigator);
			foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
			{
				if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
				{
					int num = transition.IsValid(cell, this.navigator.NavGrid.NavTable);
					if (Grid.InvalidCell != num)
					{
						base.smi.GoTo(base.smi.sm.recoverinitialfall);
						return;
					}
				}
			}
		}

		// Token: 0x06007958 RID: 31064 RVA: 0x002D8D54 File Offset: 0x002D6F54
		public bool CanRecoverToLadder()
		{
			int cell = Grid.PosToCell(base.master.transform.GetPosition());
			return this.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !base.gameObject.HasTag(GameTags.Incapacitated);
		}

		// Token: 0x06007959 RID: 31065 RVA: 0x002D8DA5 File Offset: 0x002D6FA5
		public void MountLadder()
		{
			this.navigator.SetCurrentNavType(NavType.Ladder);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x0600795A RID: 31066 RVA: 0x002D8DD8 File Offset: 0x002D6FD8
		public bool CanRecoverToPole()
		{
			int cell = Grid.PosToCell(base.master.transform.GetPosition());
			return this.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole) && !base.gameObject.HasTag(GameTags.Incapacitated);
		}

		// Token: 0x0600795B RID: 31067 RVA: 0x002D8E29 File Offset: 0x002D7029
		public void MountPole()
		{
			this.navigator.SetCurrentNavType(NavType.Pole);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x0600795C RID: 31068 RVA: 0x002D8E5C File Offset: 0x002D705C
		public void UpdateFalling()
		{
			bool value = false;
			bool flag = false;
			if (!this.navigator.IsMoving() && this.navigator.CurrentNavType != NavType.Tube)
			{
				int num = Grid.PosToCell(base.transform.GetPosition());
				int num2 = Grid.CellAbove(num);
				bool flag2 = Grid.IsValidCell(num);
				bool flag3 = Grid.IsValidCell(num2);
				bool flag4 = this.IsValidNavCell(num);
				flag4 = (flag4 && (!base.gameObject.HasTag(GameTags.Incapacitated) || (this.navigator.CurrentNavType != NavType.Ladder && this.navigator.CurrentNavType != NavType.Pole)));
				flag = ((!flag4 && flag2 && Grid.Solid[num] && !Grid.DupePassable[num]) || (flag3 && Grid.Solid[num2] && !Grid.DupePassable[num2]) || (flag2 && Grid.DupeImpassable[num]) || (flag3 && Grid.DupeImpassable[num2]));
				value = (!flag4 && !flag);
				if ((!flag2 && flag3) || (flag3 && Grid.WorldIdx[num] != Grid.WorldIdx[num2] && Grid.IsWorldValidCell(num2)))
				{
					this.TeleportInWorld(num);
				}
			}
			base.sm.isFalling.Set(value, base.smi, false);
			base.sm.isEntombed.Set(flag, base.smi, false);
		}

		// Token: 0x0600795D RID: 31069 RVA: 0x002D8FDC File Offset: 0x002D71DC
		private void TeleportInWorld(int cell)
		{
			int num = Grid.CellAbove(cell);
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
			if (world != null)
			{
				int safeCell = world.GetSafeCell();
				global::Debug.Log(string.Format("Teleporting {0} to {1}", this.navigator.name, safeCell));
				this.MoveToCell(safeCell, false);
				return;
			}
			global::Debug.LogError(string.Format("Unable to teleport {0} stuck on {1}", this.navigator.name, cell));
		}

		// Token: 0x0600795E RID: 31070 RVA: 0x002D905B File Offset: 0x002D725B
		private bool IsValidNavCell(int cell)
		{
			return this.navigator.NavGrid.NavTable.IsValid(cell, this.navigator.CurrentNavType) && !Grid.DupeImpassable[cell];
		}

		// Token: 0x0600795F RID: 31071 RVA: 0x002D9090 File Offset: 0x002D7290
		public void TryEntombedEscape()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			int backCell = base.GetComponent<Facing>().GetBackCell();
			int num2 = Grid.CellAbove(backCell);
			int num3 = Grid.CellBelow(backCell);
			foreach (int num4 in new int[]
			{
				backCell,
				num2,
				num3
			})
			{
				if (this.IsValidNavCell(num4) && !Grid.HasDoor[num4])
				{
					this.MoveToCell(num4, false);
					return;
				}
			}
			int cell = Grid.PosToCell(base.transform.GetPosition());
			foreach (CellOffset offset in this.entombedEscapeOffsets)
			{
				if (Grid.IsCellOffsetValid(cell, offset))
				{
					int num5 = Grid.OffsetCell(cell, offset);
					if (this.IsValidNavCell(num5) && !Grid.HasDoor[num5])
					{
						this.MoveToCell(num5, false);
						return;
					}
				}
			}
			for (int k = this.safeCells.Count - 1; k >= 0; k--)
			{
				int num6 = this.safeCells[k];
				if (num6 != num && this.IsValidNavCell(num6) && !Grid.HasDoor[num6])
				{
					this.MoveToCell(num6, false);
					return;
				}
			}
			foreach (CellOffset offset2 in this.entombedEscapeOffsets)
			{
				if (Grid.IsCellOffsetValid(cell, offset2))
				{
					int num7 = Grid.OffsetCell(cell, offset2);
					int num8 = Grid.CellAbove(num7);
					if (Grid.IsValidCell(num8) && !Grid.Solid[num7] && !Grid.Solid[num8] && !Grid.DupeImpassable[num7] && !Grid.DupeImpassable[num8] && !Grid.HasDoor[num7] && !Grid.HasDoor[num8])
					{
						this.MoveToCell(num7, true);
						return;
					}
				}
			}
			this.GoTo(base.sm.entombed.stuck);
		}

		// Token: 0x06007960 RID: 31072 RVA: 0x002D92A4 File Offset: 0x002D74A4
		private void MoveToCell(int cell, bool forceFloorNav = false)
		{
			base.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
			base.transform.GetComponent<Navigator>().Stop(false, true);
			if (base.gameObject.HasTag(GameTags.Incapacitated) || forceFloorNav)
			{
				base.transform.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
			}
			this.UpdateFalling();
			if (base.sm.isEntombed.Get(base.smi))
			{
				this.GoTo(base.sm.entombed.stuck);
				return;
			}
			this.GoTo(base.sm.standing);
		}

		// Token: 0x04005C1C RID: 23580
		private CellOffset[] entombedEscapeOffsets = new CellOffset[]
		{
			new CellOffset(0, 1),
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(1, 1),
			new CellOffset(-1, 1),
			new CellOffset(1, -1),
			new CellOffset(-1, -1)
		};

		// Token: 0x04005C1D RID: 23581
		private Navigator navigator;

		// Token: 0x04005C1E RID: 23582
		private bool shouldPlayEmotes;

		// Token: 0x04005C1F RID: 23583
		public string entombedAnimOverride;

		// Token: 0x04005C20 RID: 23584
		private List<int> safeCells = new List<int>();

		// Token: 0x04005C21 RID: 23585
		private int MAX_CELLS_TRACKED = 3;

		// Token: 0x04005C22 RID: 23586
		private bool flipRecoverEmote;
	}
}
