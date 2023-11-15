using System;
using UnityEngine;

// Token: 0x0200049D RID: 1181
public class FlopMonitor : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>
{
	// Token: 0x06001AA0 RID: 6816 RVA: 0x0008E4AA File Offset: 0x0008C6AA
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Flopping, (FlopMonitor.Instance smi) => smi.ShouldBeginFlopping(), null);
	}

	// Token: 0x0200113B RID: 4411
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200113C RID: 4412
	public new class Instance : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>.GameInstance
	{
		// Token: 0x060078C9 RID: 30921 RVA: 0x002D7805 File Offset: 0x002D5A05
		public Instance(IStateMachineTarget master, FlopMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060078CA RID: 30922 RVA: 0x002D7810 File Offset: 0x002D5A10
		public bool ShouldBeginFlopping()
		{
			Vector3 position = base.transform.GetPosition();
			position.y += CreatureFallMonitor.FLOOR_DISTANCE;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			int num = Grid.PosToCell(position);
			return Grid.IsValidCell(num) && Grid.Solid[num] && !Grid.IsSubstantialLiquid(cell, 0.35f) && !Grid.IsLiquid(Grid.CellAbove(cell));
		}
	}
}
