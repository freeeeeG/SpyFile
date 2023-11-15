using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class InhaleStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>
{
	// Token: 0x060003C2 RID: 962 RVA: 0x0001D1FC File Offset: 0x0001B3FC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtoeat;
		this.root.Enter("SetTarget", delegate(InhaleStates.Instance smi)
		{
			this.targetCell.Set(smi.monitor.targetCell, smi, false);
		});
		this.goingtoeat.MoveTo((InhaleStates.Instance smi) => this.targetCell.Get(smi), this.inhaling, null, false).ToggleMainStatusItem(new Func<InhaleStates.Instance, StatusItem>(InhaleStates.GetMovingStatusItem), null);
		this.inhaling.DefaultState(this.inhaling.inhale).ToggleStatusItem(CREATURES.STATUSITEMS.INHALING.NAME, CREATURES.STATUSITEMS.INHALING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.inhaling.inhale.PlayAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimPre, KAnim.PlayMode.Once).QueueAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimLoop, true, null).Enter("ComputeInhaleAmount", delegate(InhaleStates.Instance smi)
		{
			smi.ComputeInhaleAmounts();
		}).Update("Consume", delegate(InhaleStates.Instance smi, float dt)
		{
			smi.monitor.Consume(dt * smi.consumptionMult);
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.ElementNoLongerAvailable, this.inhaling.pst, null).Enter("StartInhaleSound", delegate(InhaleStates.Instance smi)
		{
			smi.StartInhaleSound();
		}).Exit("StopInhaleSound", delegate(InhaleStates.Instance smi)
		{
			smi.StopInhaleSound();
		}).ScheduleGoTo((InhaleStates.Instance smi) => smi.inhaleTime, this.inhaling.pst);
		this.inhaling.pst.Transition(this.inhaling.full, (InhaleStates.Instance smi) => smi.def.alwaysPlayPstAnim || InhaleStates.IsFull(smi), UpdateRate.SIM_200ms).Transition(this.behaviourcomplete, GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Not(new StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback(InhaleStates.IsFull)), UpdateRate.SIM_200ms);
		this.inhaling.full.QueueAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimPst, false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((InhaleStates.Instance smi) => smi.def.behaviourTag, false);
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x0001D4C6 File Offset: 0x0001B6C6
	private static StatusItem GetMovingStatusItem(InhaleStates.Instance smi)
	{
		if (smi.def.useStorage)
		{
			return smi.def.storageStatusItem;
		}
		return Db.Get().CreatureStatusItems.LookingForFood;
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0001D4F0 File Offset: 0x0001B6F0
	private static bool IsFull(InhaleStates.Instance smi)
	{
		if (smi.def.useStorage)
		{
			if (smi.storage != null)
			{
				return smi.storage.IsFull();
			}
		}
		else
		{
			CreatureCalorieMonitor.Instance smi2 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
			if (smi2 != null)
			{
				return smi2.stomach.GetFullness() >= 1f;
			}
		}
		return false;
	}

	// Token: 0x0400027B RID: 635
	public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State goingtoeat;

	// Token: 0x0400027C RID: 636
	public InhaleStates.InhalingStates inhaling;

	// Token: 0x0400027D RID: 637
	public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State behaviourcomplete;

	// Token: 0x0400027E RID: 638
	public StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.IntParameter targetCell;

	// Token: 0x02000EFD RID: 3837
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400549D RID: 21661
		public string inhaleSound;

		// Token: 0x0400549E RID: 21662
		public float inhaleTime = 3f;

		// Token: 0x0400549F RID: 21663
		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		// Token: 0x040054A0 RID: 21664
		public bool useStorage;

		// Token: 0x040054A1 RID: 21665
		public string inhaleAnimPre = "inhale_pre";

		// Token: 0x040054A2 RID: 21666
		public string inhaleAnimLoop = "inhale_loop";

		// Token: 0x040054A3 RID: 21667
		public string inhaleAnimPst = "inhale_pst";

		// Token: 0x040054A4 RID: 21668
		public bool alwaysPlayPstAnim;

		// Token: 0x040054A5 RID: 21669
		public StatusItem storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas;
	}

	// Token: 0x02000EFE RID: 3838
	public new class Instance : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.GameInstance
	{
		// Token: 0x060070B4 RID: 28852 RVA: 0x002BB3EB File Offset: 0x002B95EB
		public Instance(Chore<InhaleStates.Instance> chore, InhaleStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.behaviourTag);
			this.inhaleSound = GlobalAssets.GetSound(def.inhaleSound, false);
		}

		// Token: 0x060070B5 RID: 28853 RVA: 0x002BB424 File Offset: 0x002B9624
		public void StartInhaleSound()
		{
			LoopingSounds component = base.GetComponent<LoopingSounds>();
			if (component != null && base.smi.inhaleSound != null)
			{
				component.StartSound(base.smi.inhaleSound);
			}
		}

		// Token: 0x060070B6 RID: 28854 RVA: 0x002BB460 File Offset: 0x002B9660
		public void StopInhaleSound()
		{
			LoopingSounds component = base.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.smi.inhaleSound);
			}
		}

		// Token: 0x060070B7 RID: 28855 RVA: 0x002BB490 File Offset: 0x002B9690
		public void ComputeInhaleAmounts()
		{
			float num = base.def.inhaleTime;
			this.inhaleTime = num;
			this.consumptionMult = 1f;
			if (!base.def.useStorage && this.monitor.def.diet != null)
			{
				Diet.Info dietInfo = base.smi.monitor.def.diet.GetDietInfo(base.smi.monitor.GetTargetElement().tag);
				if (dietInfo != null)
				{
					CreatureCalorieMonitor.Instance smi = base.smi.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
					float num2 = Mathf.Clamp01(smi.GetCalories0to1() / 0.9f);
					float num3 = 1f - num2;
					float consumptionRate = base.smi.monitor.def.consumptionRate;
					float num4 = dietInfo.ConvertConsumptionMassToCalories(consumptionRate);
					float num5 = num * num4 + 0.8f * smi.calories.GetMax() * num3 * num3 * num3;
					float num6 = num5 / num4;
					if (num6 > 5f * num)
					{
						this.inhaleTime = 5f * num;
						this.consumptionMult = num5 / (this.inhaleTime * num4);
						return;
					}
					this.inhaleTime = num6;
				}
			}
		}

		// Token: 0x040054A6 RID: 21670
		public string inhaleSound;

		// Token: 0x040054A7 RID: 21671
		public float inhaleTime;

		// Token: 0x040054A8 RID: 21672
		public float consumptionMult;

		// Token: 0x040054A9 RID: 21673
		[MySmiGet]
		public GasAndLiquidConsumerMonitor.Instance monitor;

		// Token: 0x040054AA RID: 21674
		[MyCmpGet]
		public Storage storage;
	}

	// Token: 0x02000EFF RID: 3839
	public class InhalingStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State
	{
		// Token: 0x040054AB RID: 21675
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State inhale;

		// Token: 0x040054AC RID: 21676
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State pst;

		// Token: 0x040054AD RID: 21677
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State full;
	}
}
