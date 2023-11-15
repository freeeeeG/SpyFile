using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class IdleStates : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>
{
	// Token: 0x060003B7 RID: 951 RVA: 0x0001CCF8 File Offset: 0x0001AEF8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.root.Exit("StopNavigator", delegate(IdleStates.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		}).ToggleStatusItem(CREATURES.STATUSITEMS.IDLE.NAME, CREATURES.STATUSITEMS.IDLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).ToggleTag(GameTags.Idle);
		this.loop.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(this.PlayIdle)).ToggleScheduleCallback("IdleMove", (IdleStates.Instance smi) => (float)UnityEngine.Random.Range(3, 10), delegate(IdleStates.Instance smi)
		{
			smi.GoTo(this.move);
		});
		this.move.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(this.MoveToNewCell)).EventTransition(GameHashes.DestinationReached, this.loop, null).EventTransition(GameHashes.NavigationFailed, this.loop, null);
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0001CE10 File Offset: 0x0001B010
	public void MoveToNewCell(IdleStates.Instance smi)
	{
		if (smi.HasTag(GameTags.StationaryIdling))
		{
			smi.GoTo(smi.sm.loop);
			return;
		}
		Navigator component = smi.GetComponent<Navigator>();
		IdleStates.MoveCellQuery moveCellQuery = new IdleStates.MoveCellQuery(component.CurrentNavType);
		moveCellQuery.allowLiquid = smi.gameObject.HasTag(GameTags.Amphibious);
		moveCellQuery.submerged = smi.gameObject.HasTag(GameTags.Creatures.Submerged);
		int num = Grid.PosToCell(component);
		if (component.CurrentNavType == NavType.Hover && CellSelectionObject.IsExposedToSpace(num))
		{
			int num2 = 0;
			int cell = num;
			for (int i = 0; i < 10; i++)
			{
				cell = Grid.CellBelow(cell);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || !CellSelectionObject.IsExposedToSpace(cell))
				{
					break;
				}
				num2++;
			}
			moveCellQuery.lowerCellBias = (num2 == 10);
		}
		component.RunQuery(moveCellQuery);
		component.GoTo(moveCellQuery.GetResultCell(), null);
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0001CEF4 File Offset: 0x0001B0F4
	public void PlayIdle(IdleStates.Instance smi)
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

	// Token: 0x04000275 RID: 629
	private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State loop;

	// Token: 0x04000276 RID: 630
	private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State move;

	// Token: 0x02000EF4 RID: 3828
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400548E RID: 21646
		public IdleStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x02001FAB RID: 8107
		// (Invoke) Token: 0x0600A31D RID: 41757
		public delegate HashedString IdleAnimCallback(IdleStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x02000EF5 RID: 3829
	public new class Instance : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.GameInstance
	{
		// Token: 0x0600709F RID: 28831 RVA: 0x002BB14C File Offset: 0x002B934C
		public Instance(Chore<IdleStates.Instance> chore, IdleStates.Def def) : base(chore, def)
		{
		}
	}

	// Token: 0x02000EF6 RID: 3830
	public class MoveCellQuery : PathFinderQuery
	{
		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x060070A0 RID: 28832 RVA: 0x002BB156 File Offset: 0x002B9356
		// (set) Token: 0x060070A1 RID: 28833 RVA: 0x002BB15E File Offset: 0x002B935E
		public bool allowLiquid { get; set; }

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x060070A2 RID: 28834 RVA: 0x002BB167 File Offset: 0x002B9367
		// (set) Token: 0x060070A3 RID: 28835 RVA: 0x002BB16F File Offset: 0x002B936F
		public bool submerged { get; set; }

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x060070A4 RID: 28836 RVA: 0x002BB178 File Offset: 0x002B9378
		// (set) Token: 0x060070A5 RID: 28837 RVA: 0x002BB180 File Offset: 0x002B9380
		public bool lowerCellBias { get; set; }

		// Token: 0x060070A6 RID: 28838 RVA: 0x002BB189 File Offset: 0x002B9389
		public MoveCellQuery(NavType navType)
		{
			this.navType = navType;
			this.maxIterations = UnityEngine.Random.Range(5, 25);
		}

		// Token: 0x060070A7 RID: 28839 RVA: 0x002BB1B4 File Offset: 0x002B93B4
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			GameObject gameObject;
			Grid.ObjectLayers[1].TryGetValue(cell, out gameObject);
			if (gameObject != null)
			{
				BuildingUnderConstruction component = gameObject.GetComponent<BuildingUnderConstruction>();
				if (component != null && (component.Def.IsFoundation || component.HasTag(GameTags.NoCreatureIdling)))
				{
					return false;
				}
			}
			this.submerged = (this.submerged || Grid.IsSubstantialLiquid(cell, 0.35f));
			bool flag = this.navType != NavType.Swim;
			bool flag2 = this.navType == NavType.Swim || this.allowLiquid;
			if (this.submerged && !flag2)
			{
				return false;
			}
			if (!this.submerged && !flag)
			{
				return false;
			}
			if (this.targetCell == Grid.InvalidCell || !this.lowerCellBias)
			{
				this.targetCell = cell;
			}
			else
			{
				int num = Grid.CellRow(this.targetCell);
				if (Grid.CellRow(cell) < num)
				{
					this.targetCell = cell;
				}
			}
			int num2 = this.maxIterations - 1;
			this.maxIterations = num2;
			return num2 <= 0;
		}

		// Token: 0x060070A8 RID: 28840 RVA: 0x002BB2BC File Offset: 0x002B94BC
		public override int GetResultCell()
		{
			return this.targetCell;
		}

		// Token: 0x0400548F RID: 21647
		private NavType navType;

		// Token: 0x04005490 RID: 21648
		private int targetCell = Grid.InvalidCell;

		// Token: 0x04005491 RID: 21649
		private int maxIterations;
	}
}
