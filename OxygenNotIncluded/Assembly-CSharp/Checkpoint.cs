using System;
using STRINGS;
using UnityEngine;

// Token: 0x020005C6 RID: 1478
public class Checkpoint : StateMachineComponent<Checkpoint.SMInstance>
{
	// Token: 0x170001CB RID: 459
	// (get) Token: 0x0600246E RID: 9326 RVA: 0x000C6B4A File Offset: 0x000C4D4A
	private bool RedLightDesiredState
	{
		get
		{
			return this.hasLogicWire && !this.hasInputHigh && this.operational.IsOperational;
		}
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x000C6B6C File Offset: 0x000C4D6C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Checkpoint>(-801688580, Checkpoint.OnLogicValueChangedDelegate);
		base.Subscribe<Checkpoint>(-592767678, Checkpoint.OnOperationalChangedDelegate);
		base.smi.StartSM();
		if (Checkpoint.infoStatusItem_Logic == null)
		{
			Checkpoint.infoStatusItem_Logic = new StatusItem("CheckpointLogic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Checkpoint.infoStatusItem_Logic.resolveStringCallback = new Func<string, object, string>(Checkpoint.ResolveInfoStatusItem_Logic);
		}
		this.Refresh(this.redLight);
	}

	// Token: 0x06002470 RID: 9328 RVA: 0x000C6BFD File Offset: 0x000C4DFD
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.ClearReactable();
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x000C6C0B File Offset: 0x000C4E0B
	public void RefreshLight()
	{
		if (this.redLight != this.RedLightDesiredState)
		{
			this.Refresh(this.RedLightDesiredState);
			this.statusDirty = true;
		}
		if (this.statusDirty)
		{
			this.RefreshStatusItem();
		}
	}

	// Token: 0x06002472 RID: 9330 RVA: 0x000C6C3C File Offset: 0x000C4E3C
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(Checkpoint.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x06002473 RID: 9331 RVA: 0x000C6C6A File Offset: 0x000C4E6A
	private static string ResolveInfoStatusItem_Logic(string format_str, object data)
	{
		return ((Checkpoint)data).RedLight ? BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_CLOSED : BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_OPEN;
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000C6C8A File Offset: 0x000C4E8A
	private void CreateNewReactable()
	{
		if (this.reactable == null)
		{
			this.reactable = new Checkpoint.CheckpointReactable(this);
		}
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x000C6CA0 File Offset: 0x000C4EA0
	private void OrphanReactable()
	{
		this.reactable = null;
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x000C6CA9 File Offset: 0x000C4EA9
	private void ClearReactable()
	{
		if (this.reactable != null)
		{
			this.reactable.Cleanup();
			this.reactable = null;
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06002477 RID: 9335 RVA: 0x000C6CC5 File Offset: 0x000C4EC5
	public bool RedLight
	{
		get
		{
			return this.redLight;
		}
	}

	// Token: 0x06002478 RID: 9336 RVA: 0x000C6CD0 File Offset: 0x000C4ED0
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == Checkpoint.PORT_ID)
		{
			this.hasInputHigh = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.hasLogicWire = (this.GetNetwork() != null);
			this.statusDirty = true;
		}
	}

	// Token: 0x06002479 RID: 9337 RVA: 0x000C6D1E File Offset: 0x000C4F1E
	private void OnOperationalChanged(object data)
	{
		this.statusDirty = true;
	}

	// Token: 0x0600247A RID: 9338 RVA: 0x000C6D28 File Offset: 0x000C4F28
	private void RefreshStatusItem()
	{
		bool on = this.operational.IsOperational && this.hasLogicWire;
		this.selectable.ToggleStatusItem(Checkpoint.infoStatusItem_Logic, on, this);
		this.statusDirty = false;
	}

	// Token: 0x0600247B RID: 9339 RVA: 0x000C6D68 File Offset: 0x000C4F68
	private void Refresh(bool redLightState)
	{
		this.redLight = redLightState;
		this.operational.SetActive(this.operational.IsOperational && this.redLight, false);
		base.smi.sm.redLight.Set(this.redLight, base.smi, false);
		if (this.redLight)
		{
			this.CreateNewReactable();
			return;
		}
		this.ClearReactable();
	}

	// Token: 0x040014E2 RID: 5346
	[MyCmpReq]
	public Operational operational;

	// Token: 0x040014E3 RID: 5347
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040014E4 RID: 5348
	private static StatusItem infoStatusItem_Logic;

	// Token: 0x040014E5 RID: 5349
	private Checkpoint.CheckpointReactable reactable;

	// Token: 0x040014E6 RID: 5350
	public static readonly HashedString PORT_ID = "Checkpoint";

	// Token: 0x040014E7 RID: 5351
	private bool hasLogicWire;

	// Token: 0x040014E8 RID: 5352
	private bool hasInputHigh;

	// Token: 0x040014E9 RID: 5353
	private bool redLight;

	// Token: 0x040014EA RID: 5354
	private bool statusDirty = true;

	// Token: 0x040014EB RID: 5355
	private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>(delegate(Checkpoint component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040014EC RID: 5356
	private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>(delegate(Checkpoint component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x02001254 RID: 4692
	private class CheckpointReactable : Reactable
	{
		// Token: 0x06007CBA RID: 31930 RVA: 0x002E21F4 File Offset: 0x002E03F4
		public CheckpointReactable(Checkpoint checkpoint) : base(checkpoint.gameObject, "CheckpointReactable", Db.Get().ChoreTypes.Checkpoint, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.checkpoint = checkpoint;
			this.rotated = this.gameObject.GetComponent<Rotatable>().IsRotated;
			this.preventChoreInterruption = false;
		}

		// Token: 0x06007CBB RID: 31931 RVA: 0x002E2264 File Offset: 0x002E0464
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.checkpoint == null)
			{
				base.Cleanup();
				return false;
			}
			if (!this.checkpoint.RedLight)
			{
				return false;
			}
			if (this.rotated)
			{
				return transition.x < 0;
			}
			return transition.x > 0;
		}

		// Token: 0x06007CBC RID: 31932 RVA: 0x002E22C4 File Offset: 0x002E04C4
		protected override void InternalBegin()
		{
			this.reactor_navigator = this.reactor.GetComponent<Navigator>();
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			this.checkpoint.OrphanReactable();
			this.checkpoint.CreateNewReactable();
		}

		// Token: 0x06007CBD RID: 31933 RVA: 0x002E2354 File Offset: 0x002E0554
		public override void Update(float dt)
		{
			if (this.checkpoint == null || !this.checkpoint.RedLight || this.reactor_navigator == null)
			{
				base.Cleanup();
				return;
			}
			this.reactor_navigator.AdvancePath(false);
			if (!this.reactor_navigator.path.IsValid())
			{
				base.Cleanup();
				return;
			}
			NavGrid.Transition nextTransition = this.reactor_navigator.GetNextTransition();
			if (!(this.rotated ? (nextTransition.x < 0) : (nextTransition.x > 0)))
			{
				base.Cleanup();
			}
		}

		// Token: 0x06007CBE RID: 31934 RVA: 0x002E23E6 File Offset: 0x002E05E6
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		// Token: 0x06007CBF RID: 31935 RVA: 0x002E2415 File Offset: 0x002E0615
		protected override void InternalCleanup()
		{
		}

		// Token: 0x04005F2A RID: 24362
		private Checkpoint checkpoint;

		// Token: 0x04005F2B RID: 24363
		private Navigator reactor_navigator;

		// Token: 0x04005F2C RID: 24364
		private bool rotated;
	}

	// Token: 0x02001255 RID: 4693
	public class SMInstance : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.GameInstance
	{
		// Token: 0x06007CC0 RID: 31936 RVA: 0x002E2417 File Offset: 0x002E0617
		public SMInstance(Checkpoint master) : base(master)
		{
		}
	}

	// Token: 0x02001256 RID: 4694
	public class States : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint>
	{
		// Token: 0x06007CC1 RID: 31937 RVA: 0x002E2420 File Offset: 0x002E0620
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.go;
			this.root.Update("RefreshLight", delegate(Checkpoint.SMInstance smi, float dt)
			{
				smi.master.RefreshLight();
			}, UpdateRate.SIM_200ms, false);
			this.stop.ParamTransition<bool>(this.redLight, this.go, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsFalse).PlayAnim("red_light");
			this.go.ParamTransition<bool>(this.redLight, this.stop, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsTrue).PlayAnim("green_light");
		}

		// Token: 0x04005F2D RID: 24365
		public StateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.BoolParameter redLight;

		// Token: 0x04005F2E RID: 24366
		public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State stop;

		// Token: 0x04005F2F RID: 24367
		public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State go;
	}
}
