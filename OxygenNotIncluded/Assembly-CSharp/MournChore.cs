using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020003B9 RID: 953
public class MournChore : Chore<MournChore.StatesInstance>
{
	// Token: 0x060013DC RID: 5084 RVA: 0x00069B04 File Offset: 0x00067D04
	private static int GetStandableCell(int cell, Navigator navigator)
	{
		foreach (CellOffset offset in MournChore.ValidStandingOffsets)
		{
			if (Grid.IsCellOffsetValid(cell, offset))
			{
				int num = Grid.OffsetCell(cell, offset);
				if (!Grid.Reserved[num] && navigator.NavGrid.NavTable.IsValid(num, NavType.Floor) && navigator.GetNavigationCost(num) != -1)
				{
					return num;
				}
			}
		}
		return -1;
	}

	// Token: 0x060013DD RID: 5085 RVA: 0x00069B6C File Offset: 0x00067D6C
	public MournChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.Mourn, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MournChore.StatesInstance(this);
		base.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		base.AddPrecondition(ChorePreconditions.instance.NoDeadBodies, null);
		base.AddPrecondition(MournChore.HasValidMournLocation, master);
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x00069BDC File Offset: 0x00067DDC
	public static Grave FindGraveToMournAt()
	{
		Grave result = null;
		float num = -1f;
		foreach (object obj in Components.Graves)
		{
			Grave grave = (Grave)obj;
			if (grave.burialTime > num)
			{
				num = grave.burialTime;
				result = grave;
			}
		}
		return result;
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x00069C4C File Offset: 0x00067E4C
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("MournChore null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("MournChore null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("MournChore null smi.sm");
			return;
		}
		if (MournChore.FindGraveToMournAt() == null)
		{
			global::Debug.LogError("MournChore no grave");
			return;
		}
		base.smi.sm.mourner.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04000ABD RID: 2749
	private static readonly CellOffset[] ValidStandingOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04000ABE RID: 2750
	private static readonly Chore.Precondition HasValidMournLocation = new Chore.Precondition
	{
		id = "HasPlaceToStand",
		description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_PLACE_TO_STAND,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Navigator component = ((IStateMachineTarget)data).GetComponent<Navigator>();
			bool result = false;
			Grave grave = MournChore.FindGraveToMournAt();
			if (grave != null && Grid.IsValidCell(MournChore.GetStandableCell(Grid.PosToCell(grave), component)))
			{
				result = true;
			}
			return result;
		}
	};

	// Token: 0x02001008 RID: 4104
	public class StatesInstance : GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.GameInstance
	{
		// Token: 0x0600747A RID: 29818 RVA: 0x002C85F7 File Offset: 0x002C67F7
		public StatesInstance(MournChore master) : base(master)
		{
		}

		// Token: 0x0600747B RID: 29819 RVA: 0x002C8608 File Offset: 0x002C6808
		public void CreateLocator()
		{
			int cell = Grid.PosToCell(MournChore.FindGraveToMournAt().transform.GetPosition());
			Navigator component = base.master.GetComponent<Navigator>();
			int standableCell = MournChore.GetStandableCell(cell, component);
			if (standableCell < 0)
			{
				base.smi.GoTo(null);
				return;
			}
			Grid.Reserved[standableCell] = true;
			Vector3 pos = Grid.CellToPosCBC(standableCell, Grid.SceneLayer.Move);
			GameObject value = ChoreHelpers.CreateLocator("MournLocator", pos);
			base.smi.sm.locator.Set(value, base.smi, false);
			this.locatorCell = standableCell;
			base.smi.GoTo(base.sm.moveto);
		}

		// Token: 0x0600747C RID: 29820 RVA: 0x002C86AC File Offset: 0x002C68AC
		public void DestroyLocator()
		{
			if (this.locatorCell >= 0)
			{
				Grid.Reserved[this.locatorCell] = false;
				ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
				base.sm.locator.Set(null, this);
				this.locatorCell = -1;
			}
		}

		// Token: 0x040057F4 RID: 22516
		private int locatorCell = -1;
	}

	// Token: 0x02001009 RID: 4105
	public class States : GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore>
	{
		// Token: 0x0600747D RID: 29821 RVA: 0x002C8704 File Offset: 0x002C6904
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.findOffset;
			base.Target(this.mourner);
			this.root.ToggleAnims("anim_react_mourning_kanim", 0f, "").Exit("DestroyLocator", delegate(MournChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			});
			this.findOffset.Enter("CreateLocator", delegate(MournChore.StatesInstance smi)
			{
				smi.CreateLocator();
			});
			this.moveto.InitializeStates(this.mourner, this.locator, this.mourn, null, null, null);
			this.mourn.PlayAnims((MournChore.StatesInstance smi) => MournChore.States.WORK_ANIMS, KAnim.PlayMode.Loop).ScheduleGoTo(10f, this.completed);
			this.completed.PlayAnim("working_pst").OnAnimQueueComplete(null).Exit(delegate(MournChore.StatesInstance smi)
			{
				this.mourner.Get<Effects>(smi).Remove(Db.Get().effects.Get("Mourning"));
			});
		}

		// Token: 0x040057F5 RID: 22517
		public StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.TargetParameter mourner;

		// Token: 0x040057F6 RID: 22518
		public StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.TargetParameter locator;

		// Token: 0x040057F7 RID: 22519
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State findOffset;

		// Token: 0x040057F8 RID: 22520
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.ApproachSubState<IApproachable> moveto;

		// Token: 0x040057F9 RID: 22521
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State mourn;

		// Token: 0x040057FA RID: 22522
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State completed;

		// Token: 0x040057FB RID: 22523
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};
	}
}
