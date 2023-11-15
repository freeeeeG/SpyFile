using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class MoveToLureStates : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>
{
	// Token: 0x060003D4 RID: 980 RVA: 0x0001D8F4 File Offset: 0x0001BAF4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.move;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.CONSIDERINGLURE.NAME, CREATURES.STATUSITEMS.CONSIDERINGLURE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.move.MoveTo(new Func<MoveToLureStates.Instance, int>(MoveToLureStates.GetLureCell), new Func<MoveToLureStates.Instance, CellOffset[]>(MoveToLureStates.GetLureOffsets), this.arrive_at_lure, this.behaviourcomplete, false);
		this.arrive_at_lure.Enter(delegate(MoveToLureStates.Instance smi)
		{
			Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
			if (targetLure != null && targetLure.HasTag(GameTags.OneTimeUseLure))
			{
				targetLure.GetComponent<KPrefabID>().AddTag(GameTags.LureUsed, false);
			}
		}).GoTo(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.MoveToLure, false);
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0001D9CC File Offset: 0x0001BBCC
	private static Lure.Instance GetTargetLure(MoveToLureStates.Instance smi)
	{
		GameObject targetLure = smi.GetSMI<LureableMonitor.Instance>().GetTargetLure();
		if (targetLure == null)
		{
			return null;
		}
		return targetLure.GetSMI<Lure.Instance>();
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0001D9F8 File Offset: 0x0001BBF8
	private static int GetLureCell(MoveToLureStates.Instance smi)
	{
		Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
		if (targetLure == null)
		{
			return Grid.InvalidCell;
		}
		return Grid.PosToCell(targetLure);
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0001DA1C File Offset: 0x0001BC1C
	private static CellOffset[] GetLureOffsets(MoveToLureStates.Instance smi)
	{
		Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
		if (targetLure == null)
		{
			return null;
		}
		return targetLure.LurePoints;
	}

	// Token: 0x04000288 RID: 648
	public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State move;

	// Token: 0x04000289 RID: 649
	public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State arrive_at_lure;

	// Token: 0x0400028A RID: 650
	public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State behaviourcomplete;

	// Token: 0x02000F08 RID: 3848
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F09 RID: 3849
	public new class Instance : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.GameInstance
	{
		// Token: 0x060070CF RID: 28879 RVA: 0x002BB6E2 File Offset: 0x002B98E2
		public Instance(Chore<MoveToLureStates.Instance> chore, MoveToLureStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.MoveToLure);
		}
	}
}
