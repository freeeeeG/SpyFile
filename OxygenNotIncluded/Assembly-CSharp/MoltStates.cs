using System;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class MoltStates : GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>
{
	// Token: 0x060003D0 RID: 976 RVA: 0x0001D7A8 File Offset: 0x0001B9A8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moltpre;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.MOLTING.NAME, CREATURES.STATUSITEMS.MOLTING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.moltpre.Enter(new StateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State.Callback(MoltStates.Molt)).QueueAnim("lay_egg_pre", false, null).OnAnimQueueComplete(this.moltpst);
		this.moltpst.QueueAnim("lay_egg_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.ScalesGrown, false);
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0001D863 File Offset: 0x0001BA63
	private static void Molt(MoltStates.Instance smi)
	{
		smi.eggPos = smi.transform.GetPosition();
		smi.GetSMI<ScaleGrowthMonitor.Instance>().Shear();
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0001D884 File Offset: 0x0001BA84
	private static int GetMoveAsideCell(MoltStates.Instance smi)
	{
		int num = 1;
		if (GenericGameSettings.instance.acceleratedLifecycle)
		{
			num = 8;
		}
		int cell = Grid.PosToCell(smi);
		if (Grid.IsValidCell(cell))
		{
			int num2 = Grid.OffsetCell(cell, num, 0);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				return num2;
			}
			int num3 = Grid.OffsetCell(cell, -num, 0);
			if (Grid.IsValidCell(num3))
			{
				return num3;
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x04000285 RID: 645
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpre;

	// Token: 0x04000286 RID: 646
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpst;

	// Token: 0x04000287 RID: 647
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State behaviourcomplete;

	// Token: 0x02000F06 RID: 3846
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F07 RID: 3847
	public new class Instance : GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.GameInstance
	{
		// Token: 0x060070CD RID: 28877 RVA: 0x002BB6B6 File Offset: 0x002B98B6
		public Instance(Chore<MoltStates.Instance> chore, MoltStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.ScalesGrown);
		}

		// Token: 0x040054BC RID: 21692
		public Vector3 eggPos;
	}
}
