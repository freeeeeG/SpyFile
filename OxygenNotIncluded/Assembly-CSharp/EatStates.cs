using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class EatStates : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>
{
	// Token: 0x06000372 RID: 882 RVA: 0x0001B170 File Offset: 0x00019370
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtoeat;
		this.root.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.SetTarget)).Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.ReserveEdible)).Exit(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.UnreserveEdible));
		this.goingtoeat.MoveTo(new Func<EatStates.Instance, int>(EatStates.GetEdibleCell), this.eating, null, false).ToggleStatusItem(CREATURES.STATUSITEMS.LOOKINGFORFOOD.NAME, CREATURES.STATUSITEMS.LOOKINGFORFOOD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.eating.DefaultState(this.eating.pre).ToggleStatusItem(CREATURES.STATUSITEMS.EATING.NAME, CREATURES.STATUSITEMS.EATING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.eating.pre.QueueAnim("eat_pre", false, null).OnAnimQueueComplete(this.eating.loop);
		this.eating.loop.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.EatComplete)).QueueAnim("eat_loop", true, null).ScheduleGoTo(3f, this.eating.pst);
		this.eating.pst.QueueAnim("eat_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.WantsToEat, false);
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0001B31C File Offset: 0x0001951C
	private static void SetTarget(EatStates.Instance smi)
	{
		smi.sm.target.Set(smi.GetSMI<SolidConsumerMonitor.Instance>().targetEdible, smi, false);
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0001B33C File Offset: 0x0001953C
	private static void ReserveEdible(EatStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0001B384 File Offset: 0x00019584
	private static void UnreserveEdible(EatStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			if (gameObject.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(smi.gameObject, "{0} UnreserveEdible but it wasn't reserved: {1}", new object[]
			{
				smi.gameObject,
				gameObject
			});
		}
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0001B3E8 File Offset: 0x000195E8
	private static void EatComplete(EatStates.Instance smi)
	{
		PrimaryElement primaryElement = smi.sm.target.Get<PrimaryElement>(smi);
		if (primaryElement != null)
		{
			smi.lastMealElement = primaryElement.Element;
		}
		smi.Trigger(1386391852, smi.sm.target.Get<KPrefabID>(smi));
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0001B438 File Offset: 0x00019638
	private static int GetEdibleCell(EatStates.Instance smi)
	{
		return Grid.PosToCell(smi.sm.target.Get(smi));
	}

	// Token: 0x0400024E RID: 590
	public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.ApproachSubState<Pickupable> goingtoeat;

	// Token: 0x0400024F RID: 591
	public EatStates.EatingState eating;

	// Token: 0x04000250 RID: 592
	public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State behaviourcomplete;

	// Token: 0x04000251 RID: 593
	public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.TargetParameter target;

	// Token: 0x02000EC4 RID: 3780
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EC5 RID: 3781
	public new class Instance : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.GameInstance
	{
		// Token: 0x06007038 RID: 28728 RVA: 0x002BA6CD File Offset: 0x002B88CD
		public Instance(Chore<EatStates.Instance> chore, EatStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToEat);
		}

		// Token: 0x06007039 RID: 28729 RVA: 0x002BA6F1 File Offset: 0x002B88F1
		public Element GetLatestMealElement()
		{
			return this.lastMealElement;
		}

		// Token: 0x0400543D RID: 21565
		public Element lastMealElement;
	}

	// Token: 0x02000EC6 RID: 3782
	public class EatingState : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State
	{
		// Token: 0x0400543E RID: 21566
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pre;

		// Token: 0x0400543F RID: 21567
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State loop;

		// Token: 0x04005440 RID: 21568
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pst;
	}
}
