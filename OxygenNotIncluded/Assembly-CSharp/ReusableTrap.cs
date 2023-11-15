using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000939 RID: 2361
public class ReusableTrap : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>
{
	// Token: 0x06004494 RID: 17556 RVA: 0x00181E80 File Offset: 0x00180080
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.operational;
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).DefaultState(this.noOperational.idle);
		this.noOperational.idle.EnterTransition(this.noOperational.releasing, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ParamTransition<bool>(this.IsArmed, this.noOperational.disarming, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).PlayAnim("off");
		this.noOperational.releasing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.Release)).PlayAnim("release").OnAnimQueueComplete(this.noOperational.idle);
		this.noOperational.disarming.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).PlayAnim("abort_armed").OnAnimQueueComplete(this.noOperational.idle);
		this.operational.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.unarmed);
		this.operational.unarmed.ParamTransition<bool>(this.IsArmed, this.operational.armed, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapNeedsArming, null).PlayAnim("unarmed").Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.StartArmTrapWorkChore)).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.CancelArmTrapWorkChore)).WorkableCompleteTransition(new Func<ReusableTrap.Instance, Workable>(ReusableTrap.GetWorkable), this.operational.armed);
		this.operational.armed.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsArmed)).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).PlayAnim("armed", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapArmed, null).Toggle("Enable/Disable Trap Trigger", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.EnableTrapTrigger), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Toggle("Enable/Disable Lure", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.ActivateLure), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableLure)).EventHandlerTransition(GameHashes.TrapTriggered, this.operational.capture.capturing, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.HasCritter_OnTrapTriggered));
		this.operational.capture.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).ToggleTag(GameTags.Trapped).DefaultState(this.operational.capture.capturing).EventHandlerTransition(GameHashes.OnStorageChange, this.operational.capture.release, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.OnStorageEmptied));
		this.operational.capture.capturing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.SetupCapturingAnimations)).Update(new Action<ReusableTrap.Instance, float>(ReusableTrap.OptionalCapturingAnimationUpdate), UpdateRate.RENDER_EVERY_TICK, false).PlayAnim("capture").OnAnimQueueComplete(this.operational.capture.idle).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.UnsetCapturingAnimations));
		this.operational.capture.idle.TriggerOnEnter(GameHashes.TrapCaptureCompleted, null).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapHasCritter, (ReusableTrap.Instance smi) => smi.CapturedCritter).PlayAnim("capture_idle");
		this.operational.capture.release.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).QueueAnim("release", false, null).OnAnimQueueComplete(this.operational.unarmed);
	}

	// Token: 0x06004495 RID: 17557 RVA: 0x001822D8 File Offset: 0x001804D8
	public static void RefreshLogicOutput(ReusableTrap.Instance smi)
	{
		smi.RefreshLogicOutput();
	}

	// Token: 0x06004496 RID: 17558 RVA: 0x001822E0 File Offset: 0x001804E0
	public static void Release(ReusableTrap.Instance smi)
	{
		smi.Release();
	}

	// Token: 0x06004497 RID: 17559 RVA: 0x001822E8 File Offset: 0x001804E8
	public static void StartArmTrapWorkChore(ReusableTrap.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06004498 RID: 17560 RVA: 0x001822F0 File Offset: 0x001804F0
	public static void CancelArmTrapWorkChore(ReusableTrap.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06004499 RID: 17561 RVA: 0x001822F8 File Offset: 0x001804F8
	public static bool OnStorageEmptied(ReusableTrap.Instance smi, object obj)
	{
		return !smi.HasCritter;
	}

	// Token: 0x0600449A RID: 17562 RVA: 0x00182303 File Offset: 0x00180503
	public static bool HasCritter_OnTrapTriggered(ReusableTrap.Instance smi, object capturedItem)
	{
		return smi.HasCritter;
	}

	// Token: 0x0600449B RID: 17563 RVA: 0x0018230B File Offset: 0x0018050B
	public static bool StorageContainsCritter(ReusableTrap.Instance smi)
	{
		return smi.HasCritter;
	}

	// Token: 0x0600449C RID: 17564 RVA: 0x00182313 File Offset: 0x00180513
	public static bool StorageIsEmpty(ReusableTrap.Instance smi)
	{
		return !smi.HasCritter;
	}

	// Token: 0x0600449D RID: 17565 RVA: 0x0018231E File Offset: 0x0018051E
	public static void EnableTrapTrigger(ReusableTrap.Instance smi)
	{
		smi.SetTrapTriggerActiveState(true);
	}

	// Token: 0x0600449E RID: 17566 RVA: 0x00182327 File Offset: 0x00180527
	public static void DisableTrapTrigger(ReusableTrap.Instance smi)
	{
		smi.SetTrapTriggerActiveState(false);
	}

	// Token: 0x0600449F RID: 17567 RVA: 0x00182330 File Offset: 0x00180530
	public static ArmTrapWorkable GetWorkable(ReusableTrap.Instance smi)
	{
		return smi.GetWorkable();
	}

	// Token: 0x060044A0 RID: 17568 RVA: 0x00182338 File Offset: 0x00180538
	public static void ActivateLure(ReusableTrap.Instance smi)
	{
		smi.SetLureActiveState(true);
	}

	// Token: 0x060044A1 RID: 17569 RVA: 0x00182341 File Offset: 0x00180541
	public static void DisableLure(ReusableTrap.Instance smi)
	{
		smi.SetLureActiveState(false);
	}

	// Token: 0x060044A2 RID: 17570 RVA: 0x0018234A File Offset: 0x0018054A
	public static void SetupCapturingAnimations(ReusableTrap.Instance smi)
	{
		smi.SetupCapturingAnimations();
	}

	// Token: 0x060044A3 RID: 17571 RVA: 0x00182352 File Offset: 0x00180552
	public static void UnsetCapturingAnimations(ReusableTrap.Instance smi)
	{
		smi.UnsetCapturingAnimations();
	}

	// Token: 0x060044A4 RID: 17572 RVA: 0x0018235C File Offset: 0x0018055C
	public static void OptionalCapturingAnimationUpdate(ReusableTrap.Instance smi, float dt)
	{
		if (smi.def.usingSymbolChaseCapturingAnimations && smi.lastCritterCapturedAnimController != null)
		{
			if (smi.lastCritterCapturedAnimController.currentAnim != smi.CAPTURING_CRITTER_ANIMATION_NAME)
			{
				smi.lastCritterCapturedAnimController.Play(smi.CAPTURING_CRITTER_ANIMATION_NAME, KAnim.PlayMode.Once, 1f, 0f);
			}
			bool flag;
			Vector3 position = smi.animController.GetSymbolTransform(smi.CAPTURING_SYMBOL_NAME, out flag).GetColumn(3);
			smi.lastCritterCapturedAnimController.transform.SetPosition(position);
		}
	}

	// Token: 0x060044A5 RID: 17573 RVA: 0x001823FE File Offset: 0x001805FE
	public static void MarkAsArmed(ReusableTrap.Instance smi)
	{
		smi.sm.IsArmed.Set(true, smi, false);
		smi.gameObject.AddTag(GameTags.TrapArmed);
	}

	// Token: 0x060044A6 RID: 17574 RVA: 0x00182424 File Offset: 0x00180624
	public static void MarkAsUnarmed(ReusableTrap.Instance smi)
	{
		smi.sm.IsArmed.Set(false, smi, false);
		smi.gameObject.RemoveTag(GameTags.TrapArmed);
	}

	// Token: 0x04002D6A RID: 11626
	public const string CAPTURE_ANIMATION_NAME = "capture";

	// Token: 0x04002D6B RID: 11627
	public const string CAPTURE_IDLE_ANIMATION_NAME = "capture_idle";

	// Token: 0x04002D6C RID: 11628
	public const string CAPTURE_RELEASE_ANIMATION_NAME = "release";

	// Token: 0x04002D6D RID: 11629
	public const string UNARMED_ANIMATION_NAME = "unarmed";

	// Token: 0x04002D6E RID: 11630
	public const string ARMED_ANIMATION_NAME = "armed";

	// Token: 0x04002D6F RID: 11631
	public const string ABORT_ARMED_ANIMATION = "abort_armed";

	// Token: 0x04002D70 RID: 11632
	public StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.BoolParameter IsArmed;

	// Token: 0x04002D71 RID: 11633
	public ReusableTrap.NonOperationalStates noOperational;

	// Token: 0x04002D72 RID: 11634
	public ReusableTrap.OperationalStates operational;

	// Token: 0x02001778 RID: 6008
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06008E60 RID: 36448 RVA: 0x0031F45D File Offset: 0x0031D65D
		public bool usingLure
		{
			get
			{
				return this.lures != null && this.lures.Length != 0;
			}
		}

		// Token: 0x04006EE6 RID: 28390
		public string OUTPUT_LOGIC_PORT_ID;

		// Token: 0x04006EE7 RID: 28391
		public Tag[] lures;

		// Token: 0x04006EE8 RID: 28392
		public CellOffset releaseCellOffset = CellOffset.none;

		// Token: 0x04006EE9 RID: 28393
		public bool usingSymbolChaseCapturingAnimations;

		// Token: 0x04006EEA RID: 28394
		public Func<string> getTrappedAnimationNameCallback;
	}

	// Token: 0x02001779 RID: 6009
	public class CaptureStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04006EEB RID: 28395
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State capturing;

		// Token: 0x04006EEC RID: 28396
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;

		// Token: 0x04006EED RID: 28397
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State release;
	}

	// Token: 0x0200177A RID: 6010
	public class OperationalStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04006EEE RID: 28398
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State unarmed;

		// Token: 0x04006EEF RID: 28399
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State armed;

		// Token: 0x04006EF0 RID: 28400
		public ReusableTrap.CaptureStates capture;
	}

	// Token: 0x0200177B RID: 6011
	public class NonOperationalStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04006EF1 RID: 28401
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;

		// Token: 0x04006EF2 RID: 28402
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State releasing;

		// Token: 0x04006EF3 RID: 28403
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State disarming;
	}

	// Token: 0x0200177C RID: 6012
	public new class Instance : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.GameInstance, TrappedStates.ITrapStateAnimationInstructions
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06008E65 RID: 36453 RVA: 0x0031F49E File Offset: 0x0031D69E
		public bool HasCritter
		{
			get
			{
				return !this.storage.IsEmpty();
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06008E66 RID: 36454 RVA: 0x0031F4AE File Offset: 0x0031D6AE
		public GameObject CapturedCritter
		{
			get
			{
				if (!this.HasCritter)
				{
					return null;
				}
				return this.storage.items[0];
			}
		}

		// Token: 0x06008E67 RID: 36455 RVA: 0x0031F4CB File Offset: 0x0031D6CB
		public ArmTrapWorkable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x06008E68 RID: 36456 RVA: 0x0031F4D4 File Offset: 0x0031D6D4
		public void RefreshLogicOutput()
		{
			bool flag = base.IsInsideState(base.sm.operational) && this.HasCritter;
			this.logicPorts.SendSignal(base.def.OUTPUT_LOGIC_PORT_ID, flag ? 1 : 0);
		}

		// Token: 0x06008E69 RID: 36457 RVA: 0x0031F520 File Offset: 0x0031D720
		public Instance(IStateMachineTarget master, ReusableTrap.Def def) : base(master, def)
		{
		}

		// Token: 0x06008E6A RID: 36458 RVA: 0x0031F540 File Offset: 0x0031D740
		public override void StartSM()
		{
			base.StartSM();
			ArmTrapWorkable armTrapWorkable = this.workable;
			armTrapWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(armTrapWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent));
		}

		// Token: 0x06008E6B RID: 36459 RVA: 0x0031F570 File Offset: 0x0031D770
		private void OnWorkEvent(Workable workable, Workable.WorkableEvent state)
		{
			if (state == Workable.WorkableEvent.WorkStopped && workable.GetPercentComplete() < 1f && workable.GetPercentComplete() != 0f && base.IsInsideState(base.sm.operational.unarmed))
			{
				this.animController.Play("unarmed", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06008E6C RID: 36460 RVA: 0x0031F5D3 File Offset: 0x0031D7D3
		public void SetTrapTriggerActiveState(bool active)
		{
			this.trapTrigger.enabled = active;
		}

		// Token: 0x06008E6D RID: 36461 RVA: 0x0031F5E4 File Offset: 0x0031D7E4
		public void SetLureActiveState(bool activate)
		{
			if (base.def.usingLure)
			{
				Lure.Instance smi = base.gameObject.GetSMI<Lure.Instance>();
				if (smi != null)
				{
					smi.SetActiveLures(activate ? base.def.lures : null);
				}
			}
		}

		// Token: 0x06008E6E RID: 36462 RVA: 0x0031F624 File Offset: 0x0031D824
		public void SetupCapturingAnimations()
		{
			if (this.HasCritter)
			{
				this.lastCritterCapturedAnimController = this.CapturedCritter.GetComponent<KBatchedAnimController>();
			}
		}

		// Token: 0x06008E6F RID: 36463 RVA: 0x0031F640 File Offset: 0x0031D840
		public void UnsetCapturingAnimations()
		{
			this.trapTrigger.SetStoredPosition(this.CapturedCritter);
			if (base.def.usingSymbolChaseCapturingAnimations && this.lastCritterCapturedAnimController != null)
			{
				this.lastCritterCapturedAnimController.Play("trapped", KAnim.PlayMode.Loop, 1f, 0f);
			}
			this.lastCritterCapturedAnimController = null;
		}

		// Token: 0x06008E70 RID: 36464 RVA: 0x0031F6A0 File Offset: 0x0031D8A0
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<ArmTrapWorkable>(Db.Get().ChoreTypes.ArmTrap, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06008E71 RID: 36465 RVA: 0x0031F6E6 File Offset: 0x0031D8E6
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("GroundTrap.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x06008E72 RID: 36466 RVA: 0x0031F708 File Offset: 0x0031D908
		public void Release()
		{
			if (this.HasCritter)
			{
				Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(base.smi.transform.GetPosition()), base.def.releaseCellOffset), Grid.SceneLayer.Creatures);
				List<GameObject> list = new List<GameObject>();
				this.storage.DropAll(false, false, default(Vector3), true, list);
				foreach (GameObject gameObject in list)
				{
					gameObject.transform.SetPosition(position);
					KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						component.SetSceneLayer(Grid.SceneLayer.Creatures);
					}
				}
			}
		}

		// Token: 0x06008E73 RID: 36467 RVA: 0x0031F7CC File Offset: 0x0031D9CC
		public string GetTrappedAnimationName()
		{
			if (base.def.getTrappedAnimationNameCallback != null)
			{
				return base.def.getTrappedAnimationNameCallback();
			}
			return null;
		}

		// Token: 0x04006EF4 RID: 28404
		public string CAPTURING_CRITTER_ANIMATION_NAME = "caught_loop";

		// Token: 0x04006EF5 RID: 28405
		public string CAPTURING_SYMBOL_NAME = "creatureSymbol";

		// Token: 0x04006EF6 RID: 28406
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04006EF7 RID: 28407
		[MyCmpGet]
		private ArmTrapWorkable workable;

		// Token: 0x04006EF8 RID: 28408
		[MyCmpGet]
		private TrapTrigger trapTrigger;

		// Token: 0x04006EF9 RID: 28409
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x04006EFA RID: 28410
		[MyCmpGet]
		public LogicPorts logicPorts;

		// Token: 0x04006EFB RID: 28411
		public KBatchedAnimController lastCritterCapturedAnimController;

		// Token: 0x04006EFC RID: 28412
		private Chore chore;
	}
}
