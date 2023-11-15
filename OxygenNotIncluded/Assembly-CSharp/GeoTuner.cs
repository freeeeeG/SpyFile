using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x0200060B RID: 1547
public class GeoTuner : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>
{
	// Token: 0x060026E4 RID: 9956 RVA: 0x000D2DDC File Offset: 0x000D0FDC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.operational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType));
		this.nonOperational.DefaultState(this.nonOperational.off).OnSignal(this.geyserSwitchSignal, this.nonOperational.switchingGeyser).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).TagTransition(GameTags.Operational, this.operational, false);
		this.nonOperational.off.PlayAnim("off");
		this.nonOperational.switchingGeyser.QueueAnim("geyser_down", false, null).OnAnimQueueComplete(this.nonOperational.down);
		this.nonOperational.down.PlayAnim("geyser_up").QueueAnim("off", false, null).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange));
		this.operational.PlayAnim("on").Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).DefaultState(this.operational.idle).TagTransition(GameTags.Operational, this.nonOperational, true);
		this.operational.idle.ParamTransition<GameObject>(this.AssignedGeyser, this.operational.geyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNotNull).ParamTransition<GameObject>(this.AssignedGeyser, this.operational.noGeyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNull);
		this.operational.noGeyserSelected.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerNoGeyserSelected, null).ParamTransition<GameObject>(this.AssignedGeyser, this.operational.geyserSelected.switchingGeyser, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNotNull).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.DropStorage)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshStorageRequirements)).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).QueueAnim("geyser_down", false, null).OnAnimQueueComplete(this.operational.noGeyserSelected.idle);
		this.operational.noGeyserSelected.idle.PlayAnim("geyser_up").QueueAnim("on", false, null).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange));
		this.operational.geyserSelected.DefaultState(this.operational.geyserSelected.idle).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoTunerGeyserStatus, null).ParamTransition<GameObject>(this.AssignedGeyser, this.operational.noGeyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNull).OnSignal(this.geyserSwitchSignal, this.operational.geyserSelected.switchingGeyser).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		});
		this.operational.geyserSelected.idle.ParamTransition<bool>(this.hasBeenWorkedByResearcher, this.operational.geyserSelected.broadcasting.active, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsTrue).ParamTransition<bool>(this.hasBeenWorkedByResearcher, this.operational.geyserSelected.researcherInteractionNeeded, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsFalse);
		this.operational.geyserSelected.switchingGeyser.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.DropStorageIfNotMatching)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshStorageRequirements)).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).QueueAnim("geyser_down", false, null).OnAnimQueueComplete(this.operational.geyserSelected.switchingGeyser.down);
		this.operational.geyserSelected.switchingGeyser.down.QueueAnim("geyser_up", false, null).QueueAnim("on", false, null).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange)).ScheduleActionNextFrame("Switch Animation Completed", delegate(GeoTuner.Instance smi)
		{
			smi.GoTo(this.operational.geyserSelected.idle);
		});
		this.operational.geyserSelected.researcherInteractionNeeded.EventTransition(GameHashes.UpdateRoom, this.operational.geyserSelected.researcherInteractionNeeded.blocked, (GeoTuner.Instance smi) => !GeoTuner.WorkRequirementsMet(smi)).EventTransition(GameHashes.UpdateRoom, this.operational.geyserSelected.researcherInteractionNeeded.available, new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Transition.ConditionCallback(GeoTuner.WorkRequirementsMet)).EventTransition(GameHashes.OnStorageChange, this.operational.geyserSelected.researcherInteractionNeeded.blocked, (GeoTuner.Instance smi) => !GeoTuner.WorkRequirementsMet(smi)).EventTransition(GameHashes.OnStorageChange, this.operational.geyserSelected.researcherInteractionNeeded.available, new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Transition.ConditionCallback(GeoTuner.WorkRequirementsMet)).ParamTransition<bool>(this.hasBeenWorkedByResearcher, this.operational.geyserSelected.broadcasting.active, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsTrue).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer));
		this.operational.geyserSelected.researcherInteractionNeeded.blocked.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchNeeded, null).DoNothing();
		this.operational.geyserSelected.researcherInteractionNeeded.available.DefaultState(this.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe).ToggleRecurringChore(new Func<GeoTuner.Instance, Chore>(this.CreateResearchChore), null).WorkableCompleteTransition((GeoTuner.Instance smi) => smi.workable, this.operational.geyserSelected.researcherInteractionNeeded.completed);
		this.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchNeeded, null).WorkableStartTransition((GeoTuner.Instance smi) => smi.workable, this.operational.geyserSelected.researcherInteractionNeeded.available.inProgress);
		this.operational.geyserSelected.researcherInteractionNeeded.available.inProgress.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchInProgress, null).WorkableStopTransition((GeoTuner.Instance smi) => smi.workable, this.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe);
		this.operational.geyserSelected.researcherInteractionNeeded.completed.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.OnResearchCompleted));
		this.operational.geyserSelected.broadcasting.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerBroadcasting, null).Toggle("Tuning", new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ApplyTuning), new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RemoveTuning));
		this.operational.geyserSelected.broadcasting.onHold.PlayAnim("on").UpdateTransition(this.operational.geyserSelected.broadcasting.active, (GeoTuner.Instance smi, float dt) => !GeoTuner.GeyserExitEruptionTransition(smi, dt), UpdateRate.SIM_200ms, false);
		this.operational.geyserSelected.broadcasting.active.Toggle("EnergyConsumption", delegate(GeoTuner.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}, delegate(GeoTuner.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).Toggle("BroadcastingAnimations", new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.PlayBroadcastingAnimation), new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.StopPlayingBroadcastingAnimation)).Update(new Action<GeoTuner.Instance, float>(GeoTuner.ExpirationTimerUpdate), UpdateRate.SIM_200ms, false).UpdateTransition(this.operational.geyserSelected.broadcasting.onHold, new Func<GeoTuner.Instance, float, bool>(GeoTuner.GeyserExitEruptionTransition), UpdateRate.SIM_200ms, false).ParamTransition<float>(this.expirationTimer, this.operational.geyserSelected.broadcasting.expired, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsLTEZero);
		this.operational.geyserSelected.broadcasting.expired.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).ScheduleActionNextFrame("Expired", delegate(GeoTuner.Instance smi)
		{
			smi.GoTo(this.operational.geyserSelected.researcherInteractionNeeded);
		});
	}

	// Token: 0x060026E5 RID: 9957 RVA: 0x000D3728 File Offset: 0x000D1928
	private static void TriggerSoundsForGeyserChange(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			EventInstance instance = default(EventInstance);
			switch (assignedGeyser.configuration.geyserType.shape)
			{
			case GeyserConfigurator.GeyserShape.Gas:
				instance = SoundEvent.BeginOneShot(GeoTuner.gasGeyserTuningSoundPath, smi.transform.GetPosition(), 1f, false);
				break;
			case GeyserConfigurator.GeyserShape.Liquid:
				instance = SoundEvent.BeginOneShot(GeoTuner.liquidGeyserTuningSoundPath, smi.transform.GetPosition(), 1f, false);
				break;
			case GeyserConfigurator.GeyserShape.Molten:
				instance = SoundEvent.BeginOneShot(GeoTuner.metalGeyserTuningSoundPath, smi.transform.GetPosition(), 1f, false);
				break;
			}
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x060026E6 RID: 9958 RVA: 0x000D37D4 File Offset: 0x000D19D4
	private static void RefreshStorageRequirements(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser == null)
		{
			smi.storage.capacityKg = 0f;
			smi.storage.storageFilters = null;
			smi.manualDelivery.capacity = 0f;
			smi.manualDelivery.refillMass = 0f;
			smi.manualDelivery.RequestedItemTag = null;
			smi.manualDelivery.AbortDelivery("No geyser is selected for tuning");
			return;
		}
		GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = smi.def.GetSettingsForGeyser(assignedGeyser);
		smi.storage.capacityKg = settingsForGeyser.quantity;
		smi.storage.storageFilters = new List<Tag>
		{
			settingsForGeyser.material
		};
		smi.manualDelivery.AbortDelivery("Switching to new delivery request");
		smi.manualDelivery.capacity = settingsForGeyser.quantity;
		smi.manualDelivery.refillMass = settingsForGeyser.quantity;
		smi.manualDelivery.MinimumMass = settingsForGeyser.quantity;
		smi.manualDelivery.RequestedItemTag = settingsForGeyser.material;
	}

	// Token: 0x060026E7 RID: 9959 RVA: 0x000D38E0 File Offset: 0x000D1AE0
	private static void DropStorage(GeoTuner.Instance smi)
	{
		smi.storage.DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x060026E8 RID: 9960 RVA: 0x000D3908 File Offset: 0x000D1B08
	private static void DropStorageIfNotMatching(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = smi.def.GetSettingsForGeyser(assignedGeyser);
			List<GameObject> items = smi.storage.GetItems();
			if (smi.storage.GetItems() != null && items.Count > 0)
			{
				Tag tag = items[0].PrefabID();
				PrimaryElement component = items[0].GetComponent<PrimaryElement>();
				if (tag != settingsForGeyser.material)
				{
					smi.storage.DropAll(false, false, default(Vector3), true, null);
					return;
				}
				float num = component.Mass - settingsForGeyser.quantity;
				if (num > 0f)
				{
					smi.storage.DropSome(tag, num, false, false, default(Vector3), true, false);
					return;
				}
			}
		}
		else
		{
			smi.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x060026E9 RID: 9961 RVA: 0x000D39F0 File Offset: 0x000D1BF0
	private static bool GeyserExitEruptionTransition(GeoTuner.Instance smi, float dt)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		return assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && assignedGeyser.smi.GetCurrentState().parent != assignedGeyser.smi.sm.erupt;
	}

	// Token: 0x060026EA RID: 9962 RVA: 0x000D3A41 File Offset: 0x000D1C41
	public static void OnResearchCompleted(GeoTuner.Instance smi)
	{
		smi.storage.ConsumeAllIgnoringDisease();
		smi.sm.hasBeenWorkedByResearcher.Set(true, smi, false);
	}

	// Token: 0x060026EB RID: 9963 RVA: 0x000D3A62 File Offset: 0x000D1C62
	public static void PlayBroadcastingAnimation(GeoTuner.Instance smi)
	{
		smi.animController.Play("broadcasting", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060026EC RID: 9964 RVA: 0x000D3A84 File Offset: 0x000D1C84
	public static void StopPlayingBroadcastingAnimation(GeoTuner.Instance smi)
	{
		smi.animController.Play("broadcasting", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060026ED RID: 9965 RVA: 0x000D3AA6 File Offset: 0x000D1CA6
	public static void RefreshAnimationGeyserSymbolType(GeoTuner.Instance smi)
	{
		smi.RefreshGeyserSymbol();
	}

	// Token: 0x060026EE RID: 9966 RVA: 0x000D3AAE File Offset: 0x000D1CAE
	public static float GetRemainingExpiraionTime(GeoTuner.Instance smi)
	{
		return smi.sm.expirationTimer.Get(smi);
	}

	// Token: 0x060026EF RID: 9967 RVA: 0x000D3AC4 File Offset: 0x000D1CC4
	private static void ExpirationTimerUpdate(GeoTuner.Instance smi, float dt)
	{
		float num = GeoTuner.GetRemainingExpiraionTime(smi);
		num -= dt;
		smi.sm.expirationTimer.Set(num, smi, false);
	}

	// Token: 0x060026F0 RID: 9968 RVA: 0x000D3AF0 File Offset: 0x000D1CF0
	private static void ResetExpirationTimer(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			smi.sm.expirationTimer.Set(smi.def.GetSettingsForGeyser(assignedGeyser).duration, smi, false);
			return;
		}
		smi.sm.expirationTimer.Set(0f, smi, false);
	}

	// Token: 0x060026F1 RID: 9969 RVA: 0x000D3B4A File Offset: 0x000D1D4A
	private static void ForgetWorkDoneByDupe(GeoTuner.Instance smi)
	{
		smi.sm.hasBeenWorkedByResearcher.Set(false, smi, false);
		smi.workable.WorkTimeRemaining = smi.workable.GetWorkTime();
	}

	// Token: 0x060026F2 RID: 9970 RVA: 0x000D3B78 File Offset: 0x000D1D78
	private Chore CreateResearchChore(GeoTuner.Instance smi)
	{
		return new WorkChore<GeoTunerWorkable>(Db.Get().ChoreTypes.Research, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x060026F3 RID: 9971 RVA: 0x000D3BB0 File Offset: 0x000D1DB0
	private static void ApplyTuning(GeoTuner.Instance smi)
	{
		smi.GetAssignedGeyser().AddModification(smi.currentGeyserModification);
	}

	// Token: 0x060026F4 RID: 9972 RVA: 0x000D3BC4 File Offset: 0x000D1DC4
	private static void RemoveTuning(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			assignedGeyser.RemoveModification(smi.currentGeyserModification);
		}
	}

	// Token: 0x060026F5 RID: 9973 RVA: 0x000D3BED File Offset: 0x000D1DED
	public static bool WorkRequirementsMet(GeoTuner.Instance smi)
	{
		return GeoTuner.IsInLabRoom(smi) && smi.storage.MassStored() == smi.storage.capacityKg;
	}

	// Token: 0x060026F6 RID: 9974 RVA: 0x000D3C11 File Offset: 0x000D1E11
	public static bool IsInLabRoom(GeoTuner.Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x0400164B RID: 5707
	private StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Signal geyserSwitchSignal;

	// Token: 0x0400164C RID: 5708
	private GeoTuner.NonOperationalState nonOperational;

	// Token: 0x0400164D RID: 5709
	private GeoTuner.OperationalState operational;

	// Token: 0x0400164E RID: 5710
	private StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.TargetParameter FutureGeyser;

	// Token: 0x0400164F RID: 5711
	private StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.TargetParameter AssignedGeyser;

	// Token: 0x04001650 RID: 5712
	public StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.BoolParameter hasBeenWorkedByResearcher;

	// Token: 0x04001651 RID: 5713
	public StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.FloatParameter expirationTimer;

	// Token: 0x04001652 RID: 5714
	public static string liquidGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Geyser", false);

	// Token: 0x04001653 RID: 5715
	public static string gasGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Vent", false);

	// Token: 0x04001654 RID: 5716
	public static string metalGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Volcano", false);

	// Token: 0x04001655 RID: 5717
	public const string anim_switchGeyser_down = "geyser_down";

	// Token: 0x04001656 RID: 5718
	public const string anim_switchGeyser_up = "geyser_up";

	// Token: 0x04001657 RID: 5719
	private const string BroadcastingOnHoldAnimationName = "on";

	// Token: 0x04001658 RID: 5720
	private const string OnAnimName = "on";

	// Token: 0x04001659 RID: 5721
	private const string OffAnimName = "off";

	// Token: 0x0400165A RID: 5722
	private const string BroadcastingAnimationName = "broadcasting";

	// Token: 0x020012B6 RID: 4790
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007E45 RID: 32325 RVA: 0x002E7DB8 File Offset: 0x002E5FB8
		public GeoTunerConfig.GeotunedGeyserSettings GetSettingsForGeyser(Geyser geyser)
		{
			GeoTunerConfig.GeotunedGeyserSettings result;
			if (!this.geotunedGeyserSettings.TryGetValue(geyser.configuration.typeId, out result))
			{
				DebugUtil.DevLogError(string.Format("Geyser {0} is missing a Geotuner setting, using default", geyser.configuration.typeId));
				return this.defaultSetting;
			}
			return result;
		}

		// Token: 0x04006072 RID: 24690
		public string OUTPUT_LOGIC_PORT_ID;

		// Token: 0x04006073 RID: 24691
		public Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings> geotunedGeyserSettings;

		// Token: 0x04006074 RID: 24692
		public GeoTunerConfig.GeotunedGeyserSettings defaultSetting;
	}

	// Token: 0x020012B7 RID: 4791
	public class BroadcastingState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04006075 RID: 24693
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State active;

		// Token: 0x04006076 RID: 24694
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State onHold;

		// Token: 0x04006077 RID: 24695
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State expired;
	}

	// Token: 0x020012B8 RID: 4792
	public class ResearchProgress : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04006078 RID: 24696
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State waitingForDupe;

		// Token: 0x04006079 RID: 24697
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State inProgress;
	}

	// Token: 0x020012B9 RID: 4793
	public class ResearchState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x0400607A RID: 24698
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State blocked;

		// Token: 0x0400607B RID: 24699
		public GeoTuner.ResearchProgress available;

		// Token: 0x0400607C RID: 24700
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State completed;
	}

	// Token: 0x020012BA RID: 4794
	public class SwitchingGeyser : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x0400607D RID: 24701
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State down;
	}

	// Token: 0x020012BB RID: 4795
	public class GeyserSelectedState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x0400607E RID: 24702
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State idle;

		// Token: 0x0400607F RID: 24703
		public GeoTuner.SwitchingGeyser switchingGeyser;

		// Token: 0x04006080 RID: 24704
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State resourceNeeded;

		// Token: 0x04006081 RID: 24705
		public GeoTuner.ResearchState researcherInteractionNeeded;

		// Token: 0x04006082 RID: 24706
		public GeoTuner.BroadcastingState broadcasting;
	}

	// Token: 0x020012BC RID: 4796
	public class SimpleIdleState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04006083 RID: 24707
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State idle;
	}

	// Token: 0x020012BD RID: 4797
	public class NonOperationalState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04006084 RID: 24708
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State off;

		// Token: 0x04006085 RID: 24709
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State switchingGeyser;

		// Token: 0x04006086 RID: 24710
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State down;
	}

	// Token: 0x020012BE RID: 4798
	public class OperationalState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04006087 RID: 24711
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State idle;

		// Token: 0x04006088 RID: 24712
		public GeoTuner.SimpleIdleState noGeyserSelected;

		// Token: 0x04006089 RID: 24713
		public GeoTuner.GeyserSelectedState geyserSelected;
	}

	// Token: 0x020012BF RID: 4799
	public enum GeyserAnimTypeSymbols
	{
		// Token: 0x0400608B RID: 24715
		meter_gas,
		// Token: 0x0400608C RID: 24716
		meter_metal,
		// Token: 0x0400608D RID: 24717
		meter_liquid,
		// Token: 0x0400608E RID: 24718
		meter_board
	}

	// Token: 0x020012C0 RID: 4800
	public new class Instance : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.GameInstance
	{
		// Token: 0x06007E4F RID: 32335 RVA: 0x002E7E50 File Offset: 0x002E6050
		public Instance(IStateMachineTarget master, GeoTuner.Def def) : base(master, def)
		{
			this.originID = UI.StripLinkFormatting("GeoTuner") + " [" + base.gameObject.GetInstanceID().ToString() + "]";
			this.switchGeyserMeter = new MeterController(this.animController, "geyser_target", this.GetAnimationSymbol().ToString(), Meter.Offset.Behind, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x06007E50 RID: 32336 RVA: 0x002E7ECC File Offset: 0x002E60CC
		public override void StartSM()
		{
			base.StartSM();
			Components.GeoTuners.Add(base.smi.GetMyWorldId(), this);
			Geyser assignedGeyser = this.GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				assignedGeyser.Subscribe(-593169791, new Action<object>(this.OnEruptionStateChanged));
				this.RefreshModification();
			}
			this.RefreshLogicOutput();
			this.AssignFutureGeyser(this.GetFutureGeyser());
			base.gameObject.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
		}

		// Token: 0x06007E51 RID: 32337 RVA: 0x002E7F52 File Offset: 0x002E6152
		public Geyser GetFutureGeyser()
		{
			if (base.smi.sm.FutureGeyser.IsNull(this))
			{
				return null;
			}
			return base.sm.FutureGeyser.Get(this).GetComponent<Geyser>();
		}

		// Token: 0x06007E52 RID: 32338 RVA: 0x002E7F84 File Offset: 0x002E6184
		public Geyser GetAssignedGeyser()
		{
			if (base.smi.sm.AssignedGeyser.IsNull(this))
			{
				return null;
			}
			return base.sm.AssignedGeyser.Get(this).GetComponent<Geyser>();
		}

		// Token: 0x06007E53 RID: 32339 RVA: 0x002E7FB8 File Offset: 0x002E61B8
		public void AssignFutureGeyser(Geyser newFutureGeyser)
		{
			bool flag = newFutureGeyser != this.GetFutureGeyser();
			bool flag2 = this.GetAssignedGeyser() != newFutureGeyser;
			base.sm.FutureGeyser.Set(newFutureGeyser, this);
			if (flag)
			{
				if (flag2)
				{
					this.RecreateSwitchGeyserChore();
					return;
				}
				if (this.switchGeyserChore != null)
				{
					this.AbortSwitchGeyserChore("Future Geyser was set to current Geyser");
					return;
				}
			}
			else if (this.switchGeyserChore == null && flag2)
			{
				this.RecreateSwitchGeyserChore();
			}
		}

		// Token: 0x06007E54 RID: 32340 RVA: 0x002E8028 File Offset: 0x002E6228
		private void AbortSwitchGeyserChore(string reason = "Aborting Switch Geyser Chore")
		{
			if (this.switchGeyserChore != null)
			{
				Chore chore = this.switchGeyserChore;
				chore.onComplete = (Action<Chore>)Delegate.Remove(chore.onComplete, new Action<Chore>(this.OnSwitchGeyserChoreCompleted));
				this.switchGeyserChore.Cancel(reason);
				this.switchGeyserChore = null;
			}
			this.switchGeyserChore = null;
		}

		// Token: 0x06007E55 RID: 32341 RVA: 0x002E8080 File Offset: 0x002E6280
		private Chore RecreateSwitchGeyserChore()
		{
			this.AbortSwitchGeyserChore("Recreating Chore");
			this.switchGeyserChore = new WorkChore<GeoTunerSwitchGeyserWorkable>(Db.Get().ChoreTypes.Toggle, this.switchGeyserWorkable, null, true, null, new Action<Chore>(this.ShowSwitchingGeyserStatusItem), new Action<Chore>(this.HideSwitchingGeyserStatusItem), true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			Chore chore = this.switchGeyserChore;
			chore.onComplete = (Action<Chore>)Delegate.Combine(chore.onComplete, new Action<Chore>(this.OnSwitchGeyserChoreCompleted));
			return this.switchGeyserChore;
		}

		// Token: 0x06007E56 RID: 32342 RVA: 0x002E810C File Offset: 0x002E630C
		private void ShowSwitchingGeyserStatusItem(Chore chore)
		{
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, null);
		}

		// Token: 0x06007E57 RID: 32343 RVA: 0x002E812F File Offset: 0x002E632F
		private void HideSwitchingGeyserStatusItem(Chore chore)
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
		}

		// Token: 0x06007E58 RID: 32344 RVA: 0x002E8154 File Offset: 0x002E6354
		private void OnSwitchGeyserChoreCompleted(Chore chore)
		{
			this.GetCurrentState();
			GeoTuner.NonOperationalState nonOperational = base.sm.nonOperational;
			Geyser futureGeyser = this.GetFutureGeyser();
			bool flag = this.GetAssignedGeyser() != futureGeyser;
			if (chore.isComplete && flag)
			{
				this.AssignGeyser(futureGeyser);
			}
			base.Trigger(1980521255, null);
		}

		// Token: 0x06007E59 RID: 32345 RVA: 0x002E81A8 File Offset: 0x002E63A8
		public void AssignGeyser(Geyser geyser)
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			if (assignedGeyser != null && assignedGeyser != geyser)
			{
				GeoTuner.RemoveTuning(base.smi);
				assignedGeyser.Unsubscribe(-593169791, new Action<object>(base.smi.OnEruptionStateChanged));
			}
			Geyser geyser2 = assignedGeyser;
			base.sm.AssignedGeyser.Set(geyser, this);
			this.RefreshModification();
			if (geyser2 != geyser)
			{
				if (geyser != null)
				{
					geyser.Subscribe(-593169791, new Action<object>(this.OnEruptionStateChanged));
					geyser.Trigger(1763323737, null);
				}
				if (geyser2 != null)
				{
					geyser2.Trigger(1763323737, null);
				}
				base.sm.geyserSwitchSignal.Trigger(this);
			}
		}

		// Token: 0x06007E5A RID: 32346 RVA: 0x002E826C File Offset: 0x002E646C
		private void RefreshModification()
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = base.def.GetSettingsForGeyser(assignedGeyser);
				this.currentGeyserModification = settingsForGeyser.template;
				this.currentGeyserModification.originID = this.originID;
				this.enhancementDuration = settingsForGeyser.duration;
				assignedGeyser.Trigger(1763323737, null);
			}
			GeoTuner.RefreshStorageRequirements(this);
			GeoTuner.DropStorageIfNotMatching(this);
		}

		// Token: 0x06007E5B RID: 32347 RVA: 0x002E82D8 File Offset: 0x002E64D8
		public void RefreshGeyserSymbol()
		{
			this.switchGeyserMeter.meterController.Play(this.GetAnimationSymbol().ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06007E5C RID: 32348 RVA: 0x002E831C File Offset: 0x002E651C
		private GeoTuner.GeyserAnimTypeSymbols GetAnimationSymbol()
		{
			GeoTuner.GeyserAnimTypeSymbols result = GeoTuner.GeyserAnimTypeSymbols.meter_board;
			Geyser assignedGeyser = base.smi.GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				switch (assignedGeyser.configuration.geyserType.shape)
				{
				case GeyserConfigurator.GeyserShape.Gas:
					result = GeoTuner.GeyserAnimTypeSymbols.meter_gas;
					break;
				case GeyserConfigurator.GeyserShape.Liquid:
					result = GeoTuner.GeyserAnimTypeSymbols.meter_liquid;
					break;
				case GeyserConfigurator.GeyserShape.Molten:
					result = GeoTuner.GeyserAnimTypeSymbols.meter_metal;
					break;
				}
			}
			return result;
		}

		// Token: 0x06007E5D RID: 32349 RVA: 0x002E8370 File Offset: 0x002E6570
		public void OnEruptionStateChanged(object data)
		{
			bool flag = (bool)data;
			this.RefreshLogicOutput();
		}

		// Token: 0x06007E5E RID: 32350 RVA: 0x002E8380 File Offset: 0x002E6580
		public void RefreshLogicOutput()
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			bool flag = this.GetCurrentState() != base.smi.sm.nonOperational;
			bool flag2 = assignedGeyser != null && this.GetCurrentState() != base.smi.sm.operational.noGeyserSelected;
			bool flag3 = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && (assignedGeyser.smi.GetCurrentState() == assignedGeyser.smi.sm.erupt || assignedGeyser.smi.GetCurrentState().parent == assignedGeyser.smi.sm.erupt);
			bool flag4 = flag && flag2 && flag3;
			this.logicPorts.SendSignal(base.def.OUTPUT_LOGIC_PORT_ID, flag4 ? 1 : 0);
			this.switchGeyserMeter.meterController.SetSymbolVisiblity("light_bloom", flag4);
		}

		// Token: 0x06007E5F RID: 32351 RVA: 0x002E847C File Offset: 0x002E667C
		public void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject != null)
			{
				GeoTuner.Instance smi = gameObject.GetSMI<GeoTuner.Instance>();
				if (smi != null && smi.GetFutureGeyser() != this.GetFutureGeyser())
				{
					Geyser futureGeyser = smi.GetFutureGeyser();
					if (futureGeyser != null && futureGeyser.GetAmountOfGeotunersPointingOrWillPointAtThisGeyser() < 5)
					{
						this.AssignFutureGeyser(smi.GetFutureGeyser());
					}
				}
			}
		}

		// Token: 0x06007E60 RID: 32352 RVA: 0x002E84DC File Offset: 0x002E66DC
		protected override void OnCleanUp()
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			Components.GeoTuners.Remove(base.smi.GetMyWorldId(), this);
			if (assignedGeyser != null)
			{
				assignedGeyser.Unsubscribe(-593169791, new Action<object>(base.smi.OnEruptionStateChanged));
			}
			GeoTuner.RemoveTuning(this);
		}

		// Token: 0x0400608F RID: 24719
		[MyCmpReq]
		public Operational operational;

		// Token: 0x04006090 RID: 24720
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04006091 RID: 24721
		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		// Token: 0x04006092 RID: 24722
		[MyCmpReq]
		public GeoTunerWorkable workable;

		// Token: 0x04006093 RID: 24723
		[MyCmpReq]
		public GeoTunerSwitchGeyserWorkable switchGeyserWorkable;

		// Token: 0x04006094 RID: 24724
		[MyCmpReq]
		public LogicPorts logicPorts;

		// Token: 0x04006095 RID: 24725
		[MyCmpReq]
		public RoomTracker roomTracker;

		// Token: 0x04006096 RID: 24726
		[MyCmpReq]
		public KBatchedAnimController animController;

		// Token: 0x04006097 RID: 24727
		public MeterController switchGeyserMeter;

		// Token: 0x04006098 RID: 24728
		public string originID;

		// Token: 0x04006099 RID: 24729
		public float enhancementDuration;

		// Token: 0x0400609A RID: 24730
		public Geyser.GeyserModification currentGeyserModification;

		// Token: 0x0400609B RID: 24731
		private Chore switchGeyserChore;
	}
}
