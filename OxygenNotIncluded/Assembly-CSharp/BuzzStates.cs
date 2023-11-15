using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class BuzzStates : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>
{
	// Token: 0x06000320 RID: 800 RVA: 0x00018DC8 File Offset: 0x00016FC8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.root.Exit("StopNavigator", delegate(BuzzStates.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		}).ToggleStatusItem(CREATURES.STATUSITEMS.IDLE.NAME, CREATURES.STATUSITEMS.IDLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).ToggleTag(GameTags.Idle);
		this.idle.Enter(new StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State.Callback(this.PlayIdle)).ToggleScheduleCallback("DoBuzz", (BuzzStates.Instance smi) => (float)UnityEngine.Random.Range(3, 10), delegate(BuzzStates.Instance smi)
		{
			this.numMoves.Set(UnityEngine.Random.Range(4, 6), smi, false);
			smi.GoTo(this.buzz.move);
		});
		this.buzz.ParamTransition<int>(this.numMoves, this.idle, (BuzzStates.Instance smi, int p) => p <= 0);
		this.buzz.move.Enter(new StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State.Callback(this.MoveToNewCell)).EventTransition(GameHashes.DestinationReached, this.buzz.pause, null).EventTransition(GameHashes.NavigationFailed, this.buzz.pause, null);
		this.buzz.pause.Enter(delegate(BuzzStates.Instance smi)
		{
			this.numMoves.Set(this.numMoves.Get(smi) - 1, smi, false);
			smi.GoTo(this.buzz.move);
		});
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00018F44 File Offset: 0x00017144
	public void MoveToNewCell(BuzzStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		BuzzStates.MoveCellQuery moveCellQuery = new BuzzStates.MoveCellQuery(component.CurrentNavType);
		moveCellQuery.allowLiquid = smi.gameObject.HasTag(GameTags.Amphibious);
		component.RunQuery(moveCellQuery);
		component.GoTo(moveCellQuery.GetResultCell(), null);
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00018F90 File Offset: 0x00017190
	public void PlayIdle(BuzzStates.Instance smi)
	{
		KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
		Navigator component2 = smi.GetComponent<Navigator>();
		NavType nav_type = component2.CurrentNavType;
		if (smi.GetComponent<Facing>().GetFacing())
		{
			nav_type = NavGrid.MirrorNavType(nav_type);
		}
		if (smi.def.customIdleAnim != null)
		{
			HashedString invalid = HashedString.Invalid;
			HashedString hashedString = smi.def.customIdleAnim(smi, ref invalid);
			if (hashedString != HashedString.Invalid)
			{
				if (invalid != HashedString.Invalid)
				{
					component.Play(invalid, KAnim.PlayMode.Once, 1f, 0f);
				}
				component.Queue(hashedString, KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		HashedString idleAnim = component2.NavGrid.GetIdleAnim(nav_type);
		component.Play(idleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0400021C RID: 540
	private StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.IntParameter numMoves;

	// Token: 0x0400021D RID: 541
	private BuzzStates.BuzzingStates buzz;

	// Token: 0x0400021E RID: 542
	public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State idle;

	// Token: 0x0400021F RID: 543
	public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State move;

	// Token: 0x02000E8E RID: 3726
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040053CD RID: 21453
		public BuzzStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x02001FA6 RID: 8102
		// (Invoke) Token: 0x0600A310 RID: 41744
		public delegate HashedString IdleAnimCallback(BuzzStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x02000E8F RID: 3727
	public new class Instance : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.GameInstance
	{
		// Token: 0x06006FBE RID: 28606 RVA: 0x002B9C25 File Offset: 0x002B7E25
		public Instance(Chore<BuzzStates.Instance> chore, BuzzStates.Def def) : base(chore, def)
		{
		}
	}

	// Token: 0x02000E90 RID: 3728
	public class BuzzingStates : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State
	{
		// Token: 0x040053CE RID: 21454
		public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State move;

		// Token: 0x040053CF RID: 21455
		public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State pause;
	}

	// Token: 0x02000E91 RID: 3729
	public class MoveCellQuery : PathFinderQuery
	{
		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06006FC0 RID: 28608 RVA: 0x002B9C37 File Offset: 0x002B7E37
		// (set) Token: 0x06006FC1 RID: 28609 RVA: 0x002B9C3F File Offset: 0x002B7E3F
		public bool allowLiquid { get; set; }

		// Token: 0x06006FC2 RID: 28610 RVA: 0x002B9C48 File Offset: 0x002B7E48
		public MoveCellQuery(NavType navType)
		{
			this.navType = navType;
			this.maxIterations = UnityEngine.Random.Range(5, 25);
		}

		// Token: 0x06006FC3 RID: 28611 RVA: 0x002B9C70 File Offset: 0x002B7E70
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			bool flag = this.navType != NavType.Swim;
			bool flag2 = this.navType == NavType.Swim || this.allowLiquid;
			bool flag3 = Grid.IsSubstantialLiquid(cell, 0.35f);
			if (flag3 && !flag2)
			{
				return false;
			}
			if (!flag3 && !flag)
			{
				return false;
			}
			this.targetCell = cell;
			int num = this.maxIterations - 1;
			this.maxIterations = num;
			return num <= 0;
		}

		// Token: 0x06006FC4 RID: 28612 RVA: 0x002B9CE1 File Offset: 0x002B7EE1
		public override int GetResultCell()
		{
			return this.targetCell;
		}

		// Token: 0x040053D0 RID: 21456
		private NavType navType;

		// Token: 0x040053D1 RID: 21457
		private int targetCell = Grid.InvalidCell;

		// Token: 0x040053D2 RID: 21458
		private int maxIterations;
	}
}
