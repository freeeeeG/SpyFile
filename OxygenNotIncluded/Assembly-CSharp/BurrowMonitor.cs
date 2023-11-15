using System;
using UnityEngine;

// Token: 0x02000494 RID: 1172
public class BurrowMonitor : GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>
{
	// Token: 0x06001A79 RID: 6777 RVA: 0x0008D2D0 File Offset: 0x0008B4D0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.openair;
		this.openair.ToggleBehaviour(GameTags.Creatures.WantsToEnterBurrow, (BurrowMonitor.Instance smi) => smi.ShouldBurrow() && smi.timeinstate > smi.def.minimumAwakeTime, delegate(BurrowMonitor.Instance smi)
		{
			smi.BurrowComplete();
		}).Transition(this.entombed, (BurrowMonitor.Instance smi) => smi.IsEntombed() && !smi.HasTag(GameTags.Creatures.Bagged), UpdateRate.SIM_200ms).Enter("SetCollider", delegate(BurrowMonitor.Instance smi)
		{
			smi.SetCollider(true);
		});
		this.entombed.Enter("SetCollider", delegate(BurrowMonitor.Instance smi)
		{
			smi.SetCollider(false);
		}).Transition(this.openair, (BurrowMonitor.Instance smi) => !smi.IsEntombed(), UpdateRate.SIM_200ms).TagTransition(GameTags.Creatures.Bagged, this.openair, false).ToggleBehaviour(GameTags.Creatures.Burrowed, (BurrowMonitor.Instance smi) => smi.IsEntombed(), delegate(BurrowMonitor.Instance smi)
		{
			smi.GoTo(this.openair);
		}).ToggleBehaviour(GameTags.Creatures.WantsToExitBurrow, (BurrowMonitor.Instance smi) => smi.EmergeIsClear() && GameClock.Instance.IsNighttime(), delegate(BurrowMonitor.Instance smi)
		{
			smi.ExitBurrowComplete();
		});
	}

	// Token: 0x04000EB5 RID: 3765
	public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State openair;

	// Token: 0x04000EB6 RID: 3766
	public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State entombed;

	// Token: 0x0200111B RID: 4379
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005B4C RID: 23372
		public float burrowHardnessLimit = 20f;

		// Token: 0x04005B4D RID: 23373
		public float minimumAwakeTime = 24f;

		// Token: 0x04005B4E RID: 23374
		public Vector2 moundColliderSize = new Vector2f(1f, 1.5f);

		// Token: 0x04005B4F RID: 23375
		public Vector2 moundColliderOffset = new Vector2(0f, -0.25f);
	}

	// Token: 0x0200111C RID: 4380
	public new class Instance : GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.GameInstance
	{
		// Token: 0x0600784B RID: 30795 RVA: 0x002D600C File Offset: 0x002D420C
		public Instance(IStateMachineTarget master, BurrowMonitor.Def def) : base(master, def)
		{
			KBoxCollider2D component = master.GetComponent<KBoxCollider2D>();
			this.originalColliderSize = component.size;
			this.originalColliderOffset = component.offset;
		}

		// Token: 0x0600784C RID: 30796 RVA: 0x002D6040 File Offset: 0x002D4240
		public bool EmergeIsClear()
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(Grid.CellAbove(cell)))
			{
				return false;
			}
			int i = Grid.CellAbove(cell);
			return !Grid.Solid[i] && !Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.9f);
		}

		// Token: 0x0600784D RID: 30797 RVA: 0x002D609B File Offset: 0x002D429B
		public bool ShouldBurrow()
		{
			return !GameClock.Instance.IsNighttime() && this.CanBurrowInto(Grid.CellBelow(Grid.PosToCell(base.gameObject))) && !base.HasTag(GameTags.Creatures.Bagged);
		}

		// Token: 0x0600784E RID: 30798 RVA: 0x002D60D8 File Offset: 0x002D42D8
		public bool CanBurrowInto(int cell)
		{
			return Grid.IsValidCell(cell) && Grid.Solid[cell] && !Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.35f) && !(Grid.Objects[cell, 1] != null) && (float)Grid.Element[cell].hardness <= base.def.burrowHardnessLimit && !Grid.Foundation[cell];
		}

		// Token: 0x0600784F RID: 30799 RVA: 0x002D6154 File Offset: 0x002D4354
		public bool IsEntombed()
		{
			int num = Grid.PosToCell(base.smi);
			return Grid.IsValidCell(num) && Grid.Solid[num];
		}

		// Token: 0x06007850 RID: 30800 RVA: 0x002D6182 File Offset: 0x002D4382
		public void ExitBurrowComplete()
		{
			base.smi.GetComponent<KBatchedAnimController>().Play("idle_loop", KAnim.PlayMode.Once, 1f, 0f);
			this.GoTo(base.sm.openair);
		}

		// Token: 0x06007851 RID: 30801 RVA: 0x002D61BC File Offset: 0x002D43BC
		public void BurrowComplete()
		{
			base.smi.transform.SetPosition(Grid.CellToPosCBC(Grid.CellBelow(Grid.PosToCell(base.transform.GetPosition())), Grid.SceneLayer.Creatures));
			base.smi.GetComponent<KBatchedAnimController>().Play("idle_mound", KAnim.PlayMode.Once, 1f, 0f);
			this.GoTo(base.sm.entombed);
		}

		// Token: 0x06007852 RID: 30802 RVA: 0x002D622C File Offset: 0x002D442C
		public void SetCollider(bool original_size)
		{
			KBoxCollider2D component = base.master.GetComponent<KBoxCollider2D>();
			AnimEventHandler component2 = base.master.GetComponent<AnimEventHandler>();
			if (original_size)
			{
				component.size = this.originalColliderSize;
				component.offset = this.originalColliderOffset;
				component2.baseOffset = this.originalColliderOffset;
				return;
			}
			component.size = base.def.moundColliderSize;
			component.offset = base.def.moundColliderOffset;
			component2.baseOffset = base.def.moundColliderOffset;
		}

		// Token: 0x04005B50 RID: 23376
		private Vector2 originalColliderSize;

		// Token: 0x04005B51 RID: 23377
		private Vector2 originalColliderOffset;
	}
}
