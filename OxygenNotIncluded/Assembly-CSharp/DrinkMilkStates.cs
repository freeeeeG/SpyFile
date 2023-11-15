using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class DrinkMilkStates : GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>
{
	// Token: 0x0600035E RID: 862 RVA: 0x0001AA54 File Offset: 0x00018C54
	private static void SetSceneLayer(DrinkMilkStates.Instance smi, Grid.SceneLayer layer)
	{
		SegmentedCreature.Instance smi2 = smi.GetSMI<SegmentedCreature.Instance>();
		if (smi2 != null && smi2.segments != null)
		{
			using (IEnumerator<SegmentedCreature.CreatureSegment> enumerator = smi2.segments.Reverse<SegmentedCreature.CreatureSegment>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SegmentedCreature.CreatureSegment creatureSegment = enumerator.Current;
					creatureSegment.animController.SetSceneLayer(layer);
				}
				return;
			}
		}
		smi.GetComponent<KBatchedAnimController>().SetSceneLayer(layer);
	}

	// Token: 0x0600035F RID: 863 RVA: 0x0001AAC8 File Offset: 0x00018CC8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingToDrink;
		this.root.Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.SetTarget)).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.CheckIfCramped)).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.ReserveMilkFeeder)).Exit(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.UnreserveMilkFeeder)).Transition(this.behaviourComplete, delegate(DrinkMilkStates.Instance smi)
		{
			MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
			if (instance.IsNullOrDestroyed() || !instance.IsOperational())
			{
				smi.GetComponent<KAnimControllerBase>().Queue("idle_loop", KAnim.PlayMode.Loop, 1f, 0f);
				return true;
			}
			return false;
		}, UpdateRate.SIM_200ms);
		this.goingToDrink.MoveTo(new Func<DrinkMilkStates.Instance, int>(DrinkMilkStates.GetCellToDrinkFrom), this.drink, null, false).ToggleStatusItem(CREATURES.STATUSITEMS.LOOKINGFORMILK.NAME, CREATURES.STATUSITEMS.LOOKINGFORMILK.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.drink.DefaultState(this.drink.pre).Enter("FaceMilkFeeder", new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.FaceMilkFeeder)).ToggleStatusItem(CREATURES.STATUSITEMS.DRINKINGMILK.NAME, CREATURES.STATUSITEMS.DRINKINGMILK.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter(delegate(DrinkMilkStates.Instance smi)
		{
			DrinkMilkStates.SetSceneLayer(smi, smi.def.shouldBeBehindMilkTank ? Grid.SceneLayer.BuildingUse : Grid.SceneLayer.Creatures);
		}).Exit(delegate(DrinkMilkStates.Instance smi)
		{
			DrinkMilkStates.SetSceneLayer(smi, Grid.SceneLayer.Creatures);
		});
		this.drink.pre.QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkPre), false, null).OnAnimQueueComplete(this.drink.loop);
		this.drink.loop.QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkLoop), true, null).Enter(delegate(DrinkMilkStates.Instance smi)
		{
			MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
			if (instance != null)
			{
				instance.RequestToStartFeeding(smi);
				return;
			}
			smi.GoTo(this.drink.pst);
		}).OnSignal(this.requestedToStopFeeding, this.drink.pst);
		this.drink.pst.QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkPst), false, null).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.DrinkMilkComplete)).OnAnimQueueComplete(this.behaviourComplete);
		this.behaviourComplete.QueueAnim("idle_loop", true, null).BehaviourComplete(GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder, false);
	}

	// Token: 0x06000360 RID: 864 RVA: 0x0001AD38 File Offset: 0x00018F38
	private static MilkFeeder.Instance GetTargetMilkFeeder(DrinkMilkStates.Instance smi)
	{
		if (smi.sm.targetMilkFeeder.IsNullOrDestroyed())
		{
			return null;
		}
		GameObject gameObject = smi.sm.targetMilkFeeder.Get(smi);
		if (gameObject.IsNullOrDestroyed())
		{
			return null;
		}
		MilkFeeder.Instance smi2 = gameObject.GetSMI<MilkFeeder.Instance>();
		if (gameObject.IsNullOrDestroyed() || smi2.IsNullOrStopped())
		{
			return null;
		}
		return smi2;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x0001AD8F File Offset: 0x00018F8F
	private static void SetTarget(DrinkMilkStates.Instance smi)
	{
		smi.sm.targetMilkFeeder.Set(smi.GetSMI<DrinkMilkMonitor.Instance>().targetMilkFeeder.gameObject, smi, false);
	}

	// Token: 0x06000362 RID: 866 RVA: 0x0001ADB4 File Offset: 0x00018FB4
	private static void CheckIfCramped(DrinkMilkStates.Instance smi)
	{
		smi.isGassyMooCramped = smi.GetSMI<DrinkMilkMonitor.Instance>().doesTargetMilkFeederHaveSpaceForGassyMoo;
	}

	// Token: 0x06000363 RID: 867 RVA: 0x0001ADC8 File Offset: 0x00018FC8
	private static void ReserveMilkFeeder(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		instance.SetReserved(true);
	}

	// Token: 0x06000364 RID: 868 RVA: 0x0001ADE8 File Offset: 0x00018FE8
	private static void UnreserveMilkFeeder(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		instance.SetReserved(false);
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0001AE08 File Offset: 0x00019008
	private static void DrinkMilkComplete(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		smi.GetSMI<DrinkMilkMonitor.Instance>().NotifyFinishedDrinkingMilkFrom(instance);
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0001AE2C File Offset: 0x0001902C
	private static int GetCellToDrinkFrom(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return Grid.InvalidCell;
		}
		return smi.GetSMI<DrinkMilkMonitor.Instance>().GetDrinkCellOf(instance, smi.isGassyMooCramped);
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0001AE5B File Offset: 0x0001905B
	private static string GetAnimDrinkPre(DrinkMilkStates.Instance smi)
	{
		if (smi.isGassyMooCramped)
		{
			return "drink_cramped_pre";
		}
		return "drink_pre";
	}

	// Token: 0x06000368 RID: 872 RVA: 0x0001AE70 File Offset: 0x00019070
	private static string GetAnimDrinkLoop(DrinkMilkStates.Instance smi)
	{
		if (smi.isGassyMooCramped)
		{
			return "drink_cramped_loop";
		}
		return "drink_loop";
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0001AE85 File Offset: 0x00019085
	private static string GetAnimDrinkPst(DrinkMilkStates.Instance smi)
	{
		if (smi.isGassyMooCramped)
		{
			return "drink_cramped_pst";
		}
		return "drink_pst";
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0001AE9C File Offset: 0x0001909C
	private static void FaceMilkFeeder(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		float num;
		if (smi.def.isGassyMoo)
		{
			bool isRotated = instance.GetComponent<Rotatable>().IsRotated;
			if (smi.isGassyMooCramped)
			{
				if (isRotated)
				{
					num = -20f;
				}
				else
				{
					num = 20f;
				}
			}
			else if (isRotated)
			{
				num = 20f;
			}
			else
			{
				num = -20f;
			}
		}
		else
		{
			num = 0f;
		}
		IApproachable approachable = smi.sm.targetMilkFeeder.Get<IApproachable>(smi);
		if (approachable == null)
		{
			return;
		}
		float target_x = approachable.transform.GetPosition().x + num;
		smi.GetComponent<Facing>().Face(target_x);
	}

	// Token: 0x04000244 RID: 580
	public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State goingToDrink;

	// Token: 0x04000245 RID: 581
	public DrinkMilkStates.EatingState drink;

	// Token: 0x04000246 RID: 582
	public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State behaviourComplete;

	// Token: 0x04000247 RID: 583
	public StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.TargetParameter targetMilkFeeder;

	// Token: 0x04000248 RID: 584
	public StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.Signal requestedToStopFeeding;

	// Token: 0x02000EB9 RID: 3769
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400542D RID: 21549
		public bool shouldBeBehindMilkTank = true;

		// Token: 0x0400542E RID: 21550
		public bool isGassyMoo;
	}

	// Token: 0x02000EBA RID: 3770
	public new class Instance : GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.GameInstance
	{
		// Token: 0x06007023 RID: 28707 RVA: 0x002BA542 File Offset: 0x002B8742
		public Instance(Chore<DrinkMilkStates.Instance> chore, DrinkMilkStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder);
		}

		// Token: 0x06007024 RID: 28708 RVA: 0x002BA566 File Offset: 0x002B8766
		public void RequestToStopFeeding()
		{
			base.sm.requestedToStopFeeding.Trigger(base.smi);
		}

		// Token: 0x0400542F RID: 21551
		public bool isGassyMooCramped;
	}

	// Token: 0x02000EBB RID: 3771
	public class EatingState : GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State
	{
		// Token: 0x04005430 RID: 21552
		public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State pre;

		// Token: 0x04005431 RID: 21553
		public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State loop;

		// Token: 0x04005432 RID: 21554
		public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State pst;
	}
}
