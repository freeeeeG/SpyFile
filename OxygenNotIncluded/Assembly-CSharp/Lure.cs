using System;

// Token: 0x02000B54 RID: 2900
public class Lure : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>
{
	// Token: 0x0600598D RID: 22925 RVA: 0x0020BFFC File Offset: 0x0020A1FC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.off;
		this.off.DoNothing();
		this.on.Enter(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.AddToScenePartitioner)).Exit(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.RemoveFromScenePartitioner));
	}

	// Token: 0x0600598E RID: 22926 RVA: 0x0020C050 File Offset: 0x0020A250
	private void AddToScenePartitioner(Lure.Instance smi)
	{
		Extents extents = new Extents(smi.cell, smi.def.radius);
		smi.partitionerEntry = GameScenePartitioner.Instance.Add(this.name, smi, extents, GameScenePartitioner.Instance.lure, null);
	}

	// Token: 0x0600598F RID: 22927 RVA: 0x0020C098 File Offset: 0x0020A298
	private void RemoveFromScenePartitioner(Lure.Instance smi)
	{
		GameScenePartitioner.Instance.Free(ref smi.partitionerEntry);
	}

	// Token: 0x04003C9E RID: 15518
	public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State off;

	// Token: 0x04003C9F RID: 15519
	public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State on;

	// Token: 0x02001A64 RID: 6756
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007966 RID: 31078
		public CellOffset[] defaultLurePoints = new CellOffset[1];

		// Token: 0x04007967 RID: 31079
		public int radius = 50;

		// Token: 0x04007968 RID: 31080
		public Tag[] initialLures;
	}

	// Token: 0x02001A65 RID: 6757
	public new class Instance : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.GameInstance
	{
		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060096F4 RID: 38644 RVA: 0x0033E34C File Offset: 0x0033C54C
		public int cell
		{
			get
			{
				if (this._cell == -1)
				{
					this._cell = Grid.PosToCell(base.transform.GetPosition());
				}
				return this._cell;
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060096F5 RID: 38645 RVA: 0x0033E373 File Offset: 0x0033C573
		// (set) Token: 0x060096F6 RID: 38646 RVA: 0x0033E38F File Offset: 0x0033C58F
		public CellOffset[] LurePoints
		{
			get
			{
				if (this._lurePoints == null)
				{
					return base.def.defaultLurePoints;
				}
				return this._lurePoints;
			}
			set
			{
				this._lurePoints = value;
			}
		}

		// Token: 0x060096F7 RID: 38647 RVA: 0x0033E398 File Offset: 0x0033C598
		public Instance(IStateMachineTarget master, Lure.Def def) : base(master, def)
		{
		}

		// Token: 0x060096F8 RID: 38648 RVA: 0x0033E3A9 File Offset: 0x0033C5A9
		public override void StartSM()
		{
			base.StartSM();
			if (base.def.initialLures != null)
			{
				this.SetActiveLures(base.def.initialLures);
			}
		}

		// Token: 0x060096F9 RID: 38649 RVA: 0x0033E3D0 File Offset: 0x0033C5D0
		public void ChangeLureCellPosition(int newCell)
		{
			bool flag = base.IsInsideState(base.sm.on);
			if (flag)
			{
				this.GoTo(base.sm.off);
			}
			this.LurePoints = new CellOffset[]
			{
				Grid.GetOffset(Grid.PosToCell(base.smi.transform.GetPosition()), newCell)
			};
			this._cell = newCell;
			if (flag)
			{
				this.GoTo(base.sm.on);
			}
		}

		// Token: 0x060096FA RID: 38650 RVA: 0x0033E44C File Offset: 0x0033C64C
		public void SetActiveLures(Tag[] lures)
		{
			this.lures = lures;
			if (lures == null || lures.Length == 0)
			{
				this.GoTo(base.sm.off);
				return;
			}
			this.GoTo(base.sm.on);
		}

		// Token: 0x060096FB RID: 38651 RVA: 0x0033E47F File Offset: 0x0033C67F
		public bool IsActive()
		{
			return this.GetCurrentState() == base.sm.on;
		}

		// Token: 0x060096FC RID: 38652 RVA: 0x0033E494 File Offset: 0x0033C694
		public bool HasAnyLure(Tag[] creature_lures)
		{
			if (this.lures == null || creature_lures == null)
			{
				return false;
			}
			foreach (Tag a in creature_lures)
			{
				foreach (Tag b in this.lures)
				{
					if (a == b)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04007969 RID: 31081
		private int _cell = -1;

		// Token: 0x0400796A RID: 31082
		private Tag[] lures;

		// Token: 0x0400796B RID: 31083
		public HandleVector<int>.Handle partitionerEntry;

		// Token: 0x0400796C RID: 31084
		private CellOffset[] _lurePoints;
	}
}
