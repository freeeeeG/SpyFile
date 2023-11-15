using System;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public class LayEggStates : GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>
{
	// Token: 0x060003C8 RID: 968 RVA: 0x0001D578 File Offset: 0x0001B778
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.layeggpre;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.LAYINGANEGG.NAME, CREATURES.STATUSITEMS.LAYINGANEGG.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.layeggpre.Enter(new StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback(LayEggStates.LayEgg)).Exit(new StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback(LayEggStates.ShowEgg)).PlayAnim("lay_egg_pre").OnAnimQueueComplete(this.layeggpst);
		this.layeggpst.PlayAnim("lay_egg_pst").OnAnimQueueComplete(this.moveaside);
		this.moveaside.MoveTo(new Func<LayEggStates.Instance, int>(LayEggStates.GetMoveAsideCell), this.lookategg, this.behaviourcomplete, false);
		this.lookategg.Enter(new StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback(LayEggStates.FaceEgg)).GoTo(this.behaviourcomplete);
		this.behaviourcomplete.QueueAnim("idle_loop", true, null).BehaviourComplete(GameTags.Creatures.Fertile, false);
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x0001D694 File Offset: 0x0001B894
	private static void LayEgg(LayEggStates.Instance smi)
	{
		smi.eggPos = smi.transform.GetPosition();
		smi.GetSMI<FertilityMonitor.Instance>().LayEgg();
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0001D6B4 File Offset: 0x0001B8B4
	private static void ShowEgg(LayEggStates.Instance smi)
	{
		FertilityMonitor.Instance smi2 = smi.GetSMI<FertilityMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.ShowEgg();
		}
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0001D6D1 File Offset: 0x0001B8D1
	private static void FaceEgg(LayEggStates.Instance smi)
	{
		smi.Get<Facing>().Face(smi.eggPos);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0001D6E4 File Offset: 0x0001B8E4
	private static int GetMoveAsideCell(LayEggStates.Instance smi)
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

	// Token: 0x0400027F RID: 639
	public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State layeggpre;

	// Token: 0x04000280 RID: 640
	public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State layeggpst;

	// Token: 0x04000281 RID: 641
	public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State moveaside;

	// Token: 0x04000282 RID: 642
	public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State lookategg;

	// Token: 0x04000283 RID: 643
	public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State behaviourcomplete;

	// Token: 0x02000F01 RID: 3841
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F02 RID: 3842
	public new class Instance : GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.GameInstance
	{
		// Token: 0x060070C6 RID: 28870 RVA: 0x002BB662 File Offset: 0x002B9862
		public Instance(Chore<LayEggStates.Instance> chore, LayEggStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Fertile);
		}

		// Token: 0x040054B9 RID: 21689
		public Vector3 eggPos;
	}
}
