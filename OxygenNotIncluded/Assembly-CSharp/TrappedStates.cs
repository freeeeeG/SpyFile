using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class TrappedStates : GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>
{
	// Token: 0x06000410 RID: 1040 RVA: 0x0001F820 File Offset: 0x0001DA20
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.trapped;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.TRAPPED.NAME, CREATURES.STATUSITEMS.TRAPPED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.trapped.Enter(delegate(TrappedStates.Instance smi)
		{
			Navigator component = smi.GetComponent<Navigator>();
			if (component.IsValidNavType(NavType.Floor))
			{
				component.SetCurrentNavType(NavType.Floor);
			}
		}).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim(new Func<TrappedStates.Instance, string>(TrappedStates.GetTrappedAnimName), KAnim.PlayMode.Loop).TagTransition(GameTags.Trapped, null, true);
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0001F8D0 File Offset: 0x0001DAD0
	public static string GetTrappedAnimName(TrappedStates.Instance smi)
	{
		string result = "trapped";
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		Pickupable component = smi.gameObject.GetComponent<Pickupable>();
		GameObject gameObject = (component != null) ? component.storage.gameObject : Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			if (gameObject.GetComponent<TrappedStates.ITrapStateAnimationInstructions>() != null)
			{
				string trappedAnimationName = gameObject.GetComponent<TrappedStates.ITrapStateAnimationInstructions>().GetTrappedAnimationName();
				if (trappedAnimationName != null)
				{
					return trappedAnimationName;
				}
			}
			if (gameObject.GetSMI<TrappedStates.ITrapStateAnimationInstructions>() != null)
			{
				string trappedAnimationName2 = gameObject.GetSMI<TrappedStates.ITrapStateAnimationInstructions>().GetTrappedAnimationName();
				if (trappedAnimationName2 != null)
				{
					return trappedAnimationName2;
				}
			}
		}
		Trappable component2 = smi.gameObject.GetComponent<Trappable>();
		if (component2 != null && component2.HasTag(GameTags.Creatures.Swimmer) && Grid.IsValidCell(cell) && !Grid.IsLiquid(cell))
		{
			result = "trapped_onLand";
		}
		return result;
	}

	// Token: 0x040002A6 RID: 678
	public const string DEFAULT_TRAPPED_ANIM_NAME = "trapped";

	// Token: 0x040002A7 RID: 679
	private GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State trapped;

	// Token: 0x02000F29 RID: 3881
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F2A RID: 3882
	public interface ITrapStateAnimationInstructions
	{
		// Token: 0x06007135 RID: 28981
		string GetTrappedAnimationName();
	}

	// Token: 0x02000F2B RID: 3883
	public new class Instance : GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.GameInstance
	{
		// Token: 0x06007136 RID: 28982 RVA: 0x002BCB7E File Offset: 0x002BAD7E
		public Instance(Chore<TrappedStates.Instance> chore, TrappedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(TrappedStates.Instance.IsTrapped, null);
		}

		// Token: 0x0400552E RID: 21806
		public static readonly Chore.Precondition IsTrapped = new Chore.Precondition
		{
			id = "IsTrapped",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Trapped);
			}
		};
	}
}
