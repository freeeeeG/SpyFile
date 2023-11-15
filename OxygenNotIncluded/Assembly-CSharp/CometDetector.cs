using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x020005CC RID: 1484
public class CometDetector : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>
{
	// Token: 0x0600249E RID: 9374 RVA: 0x000C7E68 File Offset: 0x000C6068
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(CometDetector.Instance smi)
		{
			smi.UpdateDetectionState(this.lastIsTargetDetected.Get(smi), true);
			smi.remainingSecondsToFreezeLogicSignal = 3f;
		}).Update(delegate(CometDetector.Instance smi, float deltaSeconds)
		{
			smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
			if (smi.remainingSecondsToFreezeLogicSignal < 0f)
			{
				smi.remainingSecondsToFreezeLogicSignal = 0f;
				return;
			}
			smi.SetLogicSignal(this.lastIsTargetDetected.Get(smi));
		}, UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (CometDetector.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning, null).Enter("ToggleActive", delegate(CometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		}).Exit("ToggleActive", delegate(CometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(false, false);
		});
		this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
		this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).TagTransition(GameTags.Detecting, this.on.working, false).Enter("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), false);
		}).Update("Scan Sky", delegate(CometDetector.Instance smi, float dt)
		{
			smi.ScanSky(false);
		}, UpdateRate.SIM_200ms, false);
		this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
		this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors, null).Enter("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.SetLogicSignal(true);
		}).Exit("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.SetLogicSignal(false);
		}).Update("Scan Sky", delegate(CometDetector.Instance smi, float dt)
		{
			smi.ScanSky(true);
		}, UpdateRate.SIM_200ms, false);
		this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
		this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Detecting, this.on.working.pst, true);
		this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop);
	}

	// Token: 0x040014FE RID: 5374
	public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State off;

	// Token: 0x040014FF RID: 5375
	public CometDetector.OnStates on;

	// Token: 0x04001500 RID: 5376
	public StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.BoolParameter lastIsTargetDetected;

	// Token: 0x02001269 RID: 4713
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200126A RID: 4714
	public class OnStates : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
	{
		// Token: 0x04005F9E RID: 24478
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;

		// Token: 0x04005F9F RID: 24479
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;

		// Token: 0x04005FA0 RID: 24480
		public CometDetector.WorkingStates working;

		// Token: 0x04005FA1 RID: 24481
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
	}

	// Token: 0x0200126B RID: 4715
	public class WorkingStates : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
	{
		// Token: 0x04005FA2 RID: 24482
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;

		// Token: 0x04005FA3 RID: 24483
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;

		// Token: 0x04005FA4 RID: 24484
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
	}

	// Token: 0x0200126C RID: 4716
	public new class Instance : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.GameInstance
	{
		// Token: 0x06007D41 RID: 32065 RVA: 0x002E3AA9 File Offset: 0x002E1CA9
		public Instance(IStateMachineTarget master, CometDetector.Def def) : base(master, def)
		{
			this.detectorNetworkDef = new DetectorNetwork.Def();
			this.targetCraft = new Ref<LaunchConditionManager>();
		}

		// Token: 0x06007D42 RID: 32066 RVA: 0x002E3AD4 File Offset: 0x002E1CD4
		public override void StartSM()
		{
			if (this.detectorNetwork == null)
			{
				this.detectorNetwork = (DetectorNetwork.Instance)this.detectorNetworkDef.CreateSMI(base.master);
			}
			this.detectorNetwork.StartSM();
			base.StartSM();
		}

		// Token: 0x06007D43 RID: 32067 RVA: 0x002E3B0B File Offset: 0x002E1D0B
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			this.detectorNetwork.StopSM(reason);
		}

		// Token: 0x06007D44 RID: 32068 RVA: 0x002E3B20 File Offset: 0x002E1D20
		public void UpdateDetectionState(bool currentDetection, bool expectedDetectionForState)
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (currentDetection)
			{
				component.AddTag(GameTags.Detecting, false);
			}
			else
			{
				component.RemoveTag(GameTags.Detecting);
			}
			if (currentDetection == expectedDetectionForState)
			{
				this.SetLogicSignal(currentDetection);
			}
		}

		// Token: 0x06007D45 RID: 32069 RVA: 0x002E3B5C File Offset: 0x002E1D5C
		public void ScanSky(bool expectedDetectionForState)
		{
			LaunchConditionManager launchConditionManager = this.targetCraft.Get();
			Option<SpaceScannerTarget> option;
			if (launchConditionManager == null)
			{
				option = SpaceScannerTarget.MeteorShower();
			}
			else if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.targetCraft.Get()).state == Spacecraft.MissionState.Destroyed)
			{
				option = Option.None;
			}
			else
			{
				option = SpaceScannerTarget.RocketBaseGame(launchConditionManager);
			}
			bool flag = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
			base.smi.sm.lastIsTargetDetected.Set(flag, this, false);
			this.UpdateDetectionState(flag, expectedDetectionForState);
		}

		// Token: 0x06007D46 RID: 32070 RVA: 0x002E3C0C File Offset: 0x002E1E0C
		public void SetLogicSignal(bool on)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
		}

		// Token: 0x06007D47 RID: 32071 RVA: 0x002E3C25 File Offset: 0x002E1E25
		public void SetTargetCraft(LaunchConditionManager target)
		{
			this.targetCraft.Set(target);
		}

		// Token: 0x06007D48 RID: 32072 RVA: 0x002E3C33 File Offset: 0x002E1E33
		public LaunchConditionManager GetTargetCraft()
		{
			return this.targetCraft.Get();
		}

		// Token: 0x04005FA5 RID: 24485
		public bool ShowWorkingStatus;

		// Token: 0x04005FA6 RID: 24486
		[Serialize]
		private Ref<LaunchConditionManager> targetCraft;

		// Token: 0x04005FA7 RID: 24487
		[NonSerialized]
		public float remainingSecondsToFreezeLogicSignal;

		// Token: 0x04005FA8 RID: 24488
		private DetectorNetwork.Def detectorNetworkDef;

		// Token: 0x04005FA9 RID: 24489
		private DetectorNetwork.Instance detectorNetwork;

		// Token: 0x04005FAA RID: 24490
		private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();
	}
}
