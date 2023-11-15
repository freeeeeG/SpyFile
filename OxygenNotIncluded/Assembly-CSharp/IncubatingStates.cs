using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000D2 RID: 210
public class IncubatingStates : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>
{
	// Token: 0x060003BE RID: 958 RVA: 0x0001D08C File Offset: 0x0001B28C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.incubator;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.IN_INCUBATOR.NAME, CREATURES.STATUSITEMS.IN_INCUBATOR.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.incubator.DefaultState(this.incubator.idle).ToggleTag(GameTags.Creatures.Deliverable).TagTransition(GameTags.Creatures.InIncubator, null, true);
		this.incubator.idle.Enter("VariantUpdate", new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State.Callback(IncubatingStates.VariantUpdate)).PlayAnim("incubator_idle_loop").OnAnimQueueComplete(this.incubator.choose);
		this.incubator.choose.Transition(this.incubator.variant, new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant), UpdateRate.SIM_200ms).Transition(this.incubator.idle, GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Not(new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant)), UpdateRate.SIM_200ms);
		this.incubator.variant.PlayAnim("incubator_variant").OnAnimQueueComplete(this.incubator.idle);
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001D1C2 File Offset: 0x0001B3C2
	public static bool DoVariant(IncubatingStates.Instance smi)
	{
		return smi.variant_time == 0;
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0001D1CD File Offset: 0x0001B3CD
	public static void VariantUpdate(IncubatingStates.Instance smi)
	{
		if (smi.variant_time <= 0)
		{
			smi.variant_time = UnityEngine.Random.Range(3, 7);
			return;
		}
		smi.variant_time--;
	}

	// Token: 0x0400027A RID: 634
	public IncubatingStates.IncubatorStates incubator;

	// Token: 0x02000EFA RID: 3834
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EFB RID: 3835
	public new class Instance : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.GameInstance
	{
		// Token: 0x060070B0 RID: 28848 RVA: 0x002BB326 File Offset: 0x002B9526
		public Instance(Chore<IncubatingStates.Instance> chore, IncubatingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(IncubatingStates.Instance.IsInIncubator, null);
		}

		// Token: 0x04005498 RID: 21656
		public int variant_time = 3;

		// Token: 0x04005499 RID: 21657
		public static readonly Chore.Precondition IsInIncubator = new Chore.Precondition
		{
			id = "IsInIncubator",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.InIncubator);
			}
		};
	}

	// Token: 0x02000EFC RID: 3836
	public class IncubatorStates : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State
	{
		// Token: 0x0400549A RID: 21658
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State idle;

		// Token: 0x0400549B RID: 21659
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State choose;

		// Token: 0x0400549C RID: 21660
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State variant;
	}
}
