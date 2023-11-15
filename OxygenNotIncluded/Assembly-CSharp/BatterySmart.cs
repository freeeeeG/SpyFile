using System;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005BD RID: 1469
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
public class BatterySmart : Battery, IActivationRangeTarget
{
	// Token: 0x06002421 RID: 9249 RVA: 0x000C5597 File Offset: 0x000C3797
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<BatterySmart>(-905833192, BatterySmart.OnCopySettingsDelegate);
	}

	// Token: 0x06002422 RID: 9250 RVA: 0x000C55B0 File Offset: 0x000C37B0
	private void OnCopySettings(object data)
	{
		BatterySmart component = ((GameObject)data).GetComponent<BatterySmart>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x000C55EA File Offset: 0x000C37EA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CreateLogicMeter();
		base.Subscribe<BatterySmart>(-801688580, BatterySmart.OnLogicValueChangedDelegate);
		base.Subscribe<BatterySmart>(-592767678, BatterySmart.UpdateLogicCircuitDelegate);
	}

	// Token: 0x06002424 RID: 9252 RVA: 0x000C561A File Offset: 0x000C381A
	private void CreateLogicMeter()
	{
		this.logicMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06002425 RID: 9253 RVA: 0x000C563F File Offset: 0x000C383F
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		this.UpdateLogicCircuit(null);
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x000C5650 File Offset: 0x000C3850
	private void UpdateLogicCircuit(object data)
	{
		float num = (float)Mathf.RoundToInt(base.PercentFull * 100f);
		if (this.activated)
		{
			if (num >= (float)this.deactivateValue)
			{
				this.activated = false;
			}
		}
		else if (num <= (float)this.activateValue)
		{
			this.activated = true;
		}
		bool isOperational = this.operational.IsOperational;
		bool flag = this.activated && isOperational;
		this.logicPorts.SendSignal(BatterySmart.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x000C56C8 File Offset: 0x000C38C8
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == BatterySmart.PORT_ID)
		{
			this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
		}
	}

	// Token: 0x06002428 RID: 9256 RVA: 0x000C5700 File Offset: 0x000C3900
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06002429 RID: 9257 RVA: 0x000C5724 File Offset: 0x000C3924
	// (set) Token: 0x0600242A RID: 9258 RVA: 0x000C572D File Offset: 0x000C392D
	public float ActivateValue
	{
		get
		{
			return (float)this.deactivateValue;
		}
		set
		{
			this.deactivateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x0600242B RID: 9259 RVA: 0x000C573E File Offset: 0x000C393E
	// (set) Token: 0x0600242C RID: 9260 RVA: 0x000C5747 File Offset: 0x000C3947
	public float DeactivateValue
	{
		get
		{
			return (float)this.activateValue;
		}
		set
		{
			this.activateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x0600242D RID: 9261 RVA: 0x000C5758 File Offset: 0x000C3958
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x0600242E RID: 9262 RVA: 0x000C575F File Offset: 0x000C395F
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x0600242F RID: 9263 RVA: 0x000C5766 File Offset: 0x000C3966
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06002430 RID: 9264 RVA: 0x000C5769 File Offset: 0x000C3969
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06002431 RID: 9265 RVA: 0x000C5775 File Offset: 0x000C3975
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06002432 RID: 9266 RVA: 0x000C5781 File Offset: 0x000C3981
	public string ActivationRangeTitleText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_TITLE;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06002433 RID: 9267 RVA: 0x000C578D File Offset: 0x000C398D
	public string ActivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_DEACTIVATE;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06002434 RID: 9268 RVA: 0x000C5799 File Offset: 0x000C3999
	public string DeactivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_ACTIVATE;
		}
	}

	// Token: 0x040014BC RID: 5308
	public static readonly HashedString PORT_ID = "BatterySmartLogicPort";

	// Token: 0x040014BD RID: 5309
	[Serialize]
	private int activateValue;

	// Token: 0x040014BE RID: 5310
	[Serialize]
	private int deactivateValue = 100;

	// Token: 0x040014BF RID: 5311
	[Serialize]
	private bool activated;

	// Token: 0x040014C0 RID: 5312
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x040014C1 RID: 5313
	private MeterController logicMeter;

	// Token: 0x040014C2 RID: 5314
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040014C3 RID: 5315
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040014C4 RID: 5316
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040014C5 RID: 5317
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.UpdateLogicCircuit(data);
	});
}
