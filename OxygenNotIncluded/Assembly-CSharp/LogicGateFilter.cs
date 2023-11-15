using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000634 RID: 1588
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateFilter : LogicGate, ISingleSliderControl, ISliderControl
{
	// Token: 0x17000248 RID: 584
	// (get) Token: 0x060028CC RID: 10444 RVA: 0x000DD8CD File Offset: 0x000DBACD
	// (set) Token: 0x060028CD RID: 10445 RVA: 0x000DD8D8 File Offset: 0x000DBAD8
	public float DelayAmount
	{
		get
		{
			return this.delayAmount;
		}
		set
		{
			this.delayAmount = value;
			int delayAmountTicks = this.DelayAmountTicks;
			if (this.delayTicksRemaining > delayAmountTicks)
			{
				this.delayTicksRemaining = delayAmountTicks;
			}
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x060028CE RID: 10446 RVA: 0x000DD903 File Offset: 0x000DBB03
	private int DelayAmountTicks
	{
		get
		{
			return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x060028CF RID: 10447 RVA: 0x000DD916 File Offset: 0x000DBB16
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x060028D0 RID: 10448 RVA: 0x000DD91D File Offset: 0x000DBB1D
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.SECOND;
		}
	}

	// Token: 0x060028D1 RID: 10449 RVA: 0x000DD929 File Offset: 0x000DBB29
	public int SliderDecimalPlaces(int index)
	{
		return 1;
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x000DD92C File Offset: 0x000DBB2C
	public float GetSliderMin(int index)
	{
		return 0.1f;
	}

	// Token: 0x060028D3 RID: 10451 RVA: 0x000DD933 File Offset: 0x000DBB33
	public float GetSliderMax(int index)
	{
		return 200f;
	}

	// Token: 0x060028D4 RID: 10452 RVA: 0x000DD93A File Offset: 0x000DBB3A
	public float GetSliderValue(int index)
	{
		return this.DelayAmount;
	}

	// Token: 0x060028D5 RID: 10453 RVA: 0x000DD942 File Offset: 0x000DBB42
	public void SetSliderValue(float value, int index)
	{
		this.DelayAmount = value;
	}

	// Token: 0x060028D6 RID: 10454 RVA: 0x000DD94B File Offset: 0x000DBB4B
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x060028D7 RID: 10455 RVA: 0x000DD952 File Offset: 0x000DBB52
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TOOLTIP"), this.DelayAmount);
	}

	// Token: 0x060028D8 RID: 10456 RVA: 0x000DD973 File Offset: 0x000DBB73
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicGateFilter>(-905833192, LogicGateFilter.OnCopySettingsDelegate);
	}

	// Token: 0x060028D9 RID: 10457 RVA: 0x000DD98C File Offset: 0x000DBB8C
	private void OnCopySettings(object data)
	{
		LogicGateFilter component = ((GameObject)data).GetComponent<LogicGateFilter>();
		if (component != null)
		{
			this.DelayAmount = component.DelayAmount;
		}
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x000DD9BC File Offset: 0x000DBBBC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.meter.SetPositionPercent(0f);
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x000DDA08 File Offset: 0x000DBC08
	private void Update()
	{
		float positionPercent;
		if (this.input_was_previously_negative)
		{
			positionPercent = 0f;
		}
		else if (this.delayTicksRemaining > 0)
		{
			positionPercent = (float)(this.DelayAmountTicks - this.delayTicksRemaining) / (float)this.DelayAmountTicks;
		}
		else
		{
			positionPercent = 1f;
		}
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x000DDA5F File Offset: 0x000DBC5F
	public override void LogicTick()
	{
		if (!this.input_was_previously_negative && this.delayTicksRemaining > 0)
		{
			this.delayTicksRemaining--;
			if (this.delayTicksRemaining <= 0)
			{
				this.OnDelay();
			}
		}
	}

	// Token: 0x060028DD RID: 10461 RVA: 0x000DDA90 File Offset: 0x000DBC90
	protected override int GetCustomValue(int val1, int val2)
	{
		if (val1 == 0)
		{
			this.input_was_previously_negative = true;
			this.delayTicksRemaining = 0;
			this.meter.SetPositionPercent(1f);
		}
		else if (this.delayTicksRemaining <= 0)
		{
			if (this.input_was_previously_negative)
			{
				this.delayTicksRemaining = this.DelayAmountTicks;
			}
			this.input_was_previously_negative = false;
		}
		if (val1 != 0 && this.delayTicksRemaining <= 0)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060028DE RID: 10462 RVA: 0x000DDAF4 File Offset: 0x000DBCF4
	private void OnDelay()
	{
		if (this.cleaningUp)
		{
			return;
		}
		this.delayTicksRemaining = 0;
		this.meter.SetPositionPercent(0f);
		if (this.outputValueOne == 1)
		{
			return;
		}
		int outputCellOne = base.OutputCellOne;
		if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
		{
			return;
		}
		this.outputValueOne = 1;
		base.RefreshAnimation();
	}

	// Token: 0x040017FD RID: 6141
	[Serialize]
	private bool input_was_previously_negative;

	// Token: 0x040017FE RID: 6142
	[Serialize]
	private float delayAmount = 5f;

	// Token: 0x040017FF RID: 6143
	[Serialize]
	private int delayTicksRemaining;

	// Token: 0x04001800 RID: 6144
	private MeterController meter;

	// Token: 0x04001801 RID: 6145
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001802 RID: 6146
	private static readonly EventSystem.IntraObjectHandler<LogicGateFilter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateFilter>(delegate(LogicGateFilter component, object data)
	{
		component.OnCopySettings(data);
	});
}
