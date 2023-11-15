using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000621 RID: 1569
[SerializationConfig(MemberSerialization.OptIn)]
public class LimitValve : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06002796 RID: 10134 RVA: 0x000D6CE8 File Offset: 0x000D4EE8
	public float RemainingCapacity
	{
		get
		{
			return Mathf.Max(0f, this.m_limit - this.m_amount);
		}
	}

	// Token: 0x06002797 RID: 10135 RVA: 0x000D6D01 File Offset: 0x000D4F01
	public NonLinearSlider.Range[] GetRanges()
	{
		if (this.sliderRanges != null && this.sliderRanges.Length != 0)
		{
			return this.sliderRanges;
		}
		return NonLinearSlider.GetDefaultRange(this.maxLimitKg);
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06002798 RID: 10136 RVA: 0x000D6D26 File Offset: 0x000D4F26
	// (set) Token: 0x06002799 RID: 10137 RVA: 0x000D6D2E File Offset: 0x000D4F2E
	public float Limit
	{
		get
		{
			return this.m_limit;
		}
		set
		{
			this.m_limit = value;
			this.Refresh();
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x0600279A RID: 10138 RVA: 0x000D6D3D File Offset: 0x000D4F3D
	// (set) Token: 0x0600279B RID: 10139 RVA: 0x000D6D45 File Offset: 0x000D4F45
	public float Amount
	{
		get
		{
			return this.m_amount;
		}
		set
		{
			this.m_amount = value;
			base.Trigger(-1722241721, this.Amount);
			this.Refresh();
		}
	}

	// Token: 0x0600279C RID: 10140 RVA: 0x000D6D6A File Offset: 0x000D4F6A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LimitValve>(-905833192, LimitValve.OnCopySettingsDelegate);
	}

	// Token: 0x0600279D RID: 10141 RVA: 0x000D6D84 File Offset: 0x000D4F84
	protected override void OnSpawn()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.Subscribe<LimitValve>(-801688580, LimitValve.OnLogicValueChangedDelegate);
		if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
		{
			ConduitBridge conduitBridge = this.conduitBridge;
			conduitBridge.desiredMassTransfer = (ConduitBridgeBase.DesiredMassTransfer)Delegate.Combine(conduitBridge.desiredMassTransfer, new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer));
			ConduitBridge conduitBridge2 = this.conduitBridge;
			conduitBridge2.OnMassTransfer = (ConduitBridgeBase.ConduitBridgeEvent)Delegate.Combine(conduitBridge2.OnMassTransfer, new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer));
		}
		else if (this.conduitType == ConduitType.Solid)
		{
			SolidConduitBridge solidConduitBridge = this.solidConduitBridge;
			solidConduitBridge.desiredMassTransfer = (ConduitBridgeBase.DesiredMassTransfer)Delegate.Combine(solidConduitBridge.desiredMassTransfer, new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer));
			SolidConduitBridge solidConduitBridge2 = this.solidConduitBridge;
			solidConduitBridge2.OnMassTransfer = (ConduitBridgeBase.ConduitBridgeEvent)Delegate.Combine(solidConduitBridge2.OnMassTransfer, new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer));
		}
		if (this.limitMeter == null)
		{
			this.limitMeter = new MeterController(this.controller, "meter_target_counter", "meter_counter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target_counter"
			});
		}
		this.Refresh();
		base.OnSpawn();
	}

	// Token: 0x0600279E RID: 10142 RVA: 0x000D6EC6 File Offset: 0x000D50C6
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.OnCleanUp();
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x000D6EF9 File Offset: 0x000D50F9
	private void LogicTick()
	{
		if (this.m_resetRequested)
		{
			this.ResetAmount();
		}
	}

	// Token: 0x060027A0 RID: 10144 RVA: 0x000D6F09 File Offset: 0x000D5109
	public void ResetAmount()
	{
		this.m_resetRequested = false;
		this.Amount = 0f;
	}

	// Token: 0x060027A1 RID: 10145 RVA: 0x000D6F20 File Offset: 0x000D5120
	private float DesiredMassTransfer(float dt, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable)
	{
		if (!this.operational.IsOperational)
		{
			return 0f;
		}
		if (this.conduitType == ConduitType.Solid && pickupable != null && GameTags.DisplayAsUnits.Contains(pickupable.PrefabID()))
		{
			float num = pickupable.PrimaryElement.Units;
			if (this.RemainingCapacity < num)
			{
				num = (float)Mathf.FloorToInt(this.RemainingCapacity);
			}
			return num * pickupable.PrimaryElement.MassPerUnit;
		}
		return Mathf.Min(mass, this.RemainingCapacity);
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x000D6FA4 File Offset: 0x000D51A4
	private void OnMassTransfer(SimHashes element, float transferredMass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable)
	{
		if (!LogicCircuitNetwork.IsBitActive(0, this.ports.GetInputValue(LimitValve.RESET_PORT_ID)))
		{
			if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
			{
				this.Amount += transferredMass;
			}
			else if (this.conduitType == ConduitType.Solid && pickupable != null)
			{
				this.Amount += transferredMass / pickupable.PrimaryElement.MassPerUnit;
			}
		}
		this.operational.SetActive(this.operational.IsOperational && transferredMass > 0f, false);
		this.Refresh();
	}

	// Token: 0x060027A3 RID: 10147 RVA: 0x000D7044 File Offset: 0x000D5244
	private void Refresh()
	{
		if (this.operational == null)
		{
			return;
		}
		this.ports.SendSignal(LimitValve.OUTPUT_PORT_ID, (this.RemainingCapacity <= 0f) ? 1 : 0);
		this.operational.SetFlag(LimitValve.limitNotReached, this.RemainingCapacity > 0f);
		if (this.RemainingCapacity > 0f)
		{
			this.limitMeter.meterController.Play("meter_counter", KAnim.PlayMode.Paused, 1f, 0f);
			this.limitMeter.SetPositionPercent(this.Amount / this.Limit);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitNotReached, this);
			return;
		}
		this.limitMeter.meterController.Play("meter_on", KAnim.PlayMode.Paused, 1f, 0f);
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitReached, this);
	}

	// Token: 0x060027A4 RID: 10148 RVA: 0x000D7164 File Offset: 0x000D5364
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LimitValve.RESET_PORT_ID && LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue))
		{
			this.ResetAmount();
		}
	}

	// Token: 0x060027A5 RID: 10149 RVA: 0x000D71A0 File Offset: 0x000D53A0
	private void OnCopySettings(object data)
	{
		LimitValve component = ((GameObject)data).GetComponent<LimitValve>();
		if (component != null)
		{
			this.Limit = component.Limit;
		}
	}

	// Token: 0x040016E6 RID: 5862
	public static readonly HashedString RESET_PORT_ID = new HashedString("LimitValveReset");

	// Token: 0x040016E7 RID: 5863
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LimitValveOutput");

	// Token: 0x040016E8 RID: 5864
	public static readonly Operational.Flag limitNotReached = new Operational.Flag("limitNotReached", Operational.Flag.Type.Requirement);

	// Token: 0x040016E9 RID: 5865
	public ConduitType conduitType;

	// Token: 0x040016EA RID: 5866
	public float maxLimitKg = 100f;

	// Token: 0x040016EB RID: 5867
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040016EC RID: 5868
	[MyCmpReq]
	private LogicPorts ports;

	// Token: 0x040016ED RID: 5869
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x040016EE RID: 5870
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040016EF RID: 5871
	[MyCmpGet]
	private ConduitBridge conduitBridge;

	// Token: 0x040016F0 RID: 5872
	[MyCmpGet]
	private SolidConduitBridge solidConduitBridge;

	// Token: 0x040016F1 RID: 5873
	[Serialize]
	[SerializeField]
	private float m_limit;

	// Token: 0x040016F2 RID: 5874
	[Serialize]
	private float m_amount;

	// Token: 0x040016F3 RID: 5875
	[Serialize]
	private bool m_resetRequested;

	// Token: 0x040016F4 RID: 5876
	private MeterController limitMeter;

	// Token: 0x040016F5 RID: 5877
	public bool displayUnitsInsteadOfMass;

	// Token: 0x040016F6 RID: 5878
	public NonLinearSlider.Range[] sliderRanges;

	// Token: 0x040016F7 RID: 5879
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040016F8 RID: 5880
	private static readonly EventSystem.IntraObjectHandler<LimitValve> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LimitValve>(delegate(LimitValve component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040016F9 RID: 5881
	private static readonly EventSystem.IntraObjectHandler<LimitValve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LimitValve>(delegate(LimitValve component, object data)
	{
		component.OnCopySettings(data);
	});
}
