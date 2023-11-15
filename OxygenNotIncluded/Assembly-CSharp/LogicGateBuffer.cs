using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000633 RID: 1587
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateBuffer : LogicGate, ISingleSliderControl, ISliderControl
{
	// Token: 0x17000244 RID: 580
	// (get) Token: 0x060028B7 RID: 10423 RVA: 0x000DD617 File Offset: 0x000DB817
	// (set) Token: 0x060028B8 RID: 10424 RVA: 0x000DD620 File Offset: 0x000DB820
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

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x060028B9 RID: 10425 RVA: 0x000DD64B File Offset: 0x000DB84B
	private int DelayAmountTicks
	{
		get
		{
			return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x060028BA RID: 10426 RVA: 0x000DD65E File Offset: 0x000DB85E
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x060028BB RID: 10427 RVA: 0x000DD665 File Offset: 0x000DB865
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.SECOND;
		}
	}

	// Token: 0x060028BC RID: 10428 RVA: 0x000DD671 File Offset: 0x000DB871
	public int SliderDecimalPlaces(int index)
	{
		return 1;
	}

	// Token: 0x060028BD RID: 10429 RVA: 0x000DD674 File Offset: 0x000DB874
	public float GetSliderMin(int index)
	{
		return 0.1f;
	}

	// Token: 0x060028BE RID: 10430 RVA: 0x000DD67B File Offset: 0x000DB87B
	public float GetSliderMax(int index)
	{
		return 200f;
	}

	// Token: 0x060028BF RID: 10431 RVA: 0x000DD682 File Offset: 0x000DB882
	public float GetSliderValue(int index)
	{
		return this.DelayAmount;
	}

	// Token: 0x060028C0 RID: 10432 RVA: 0x000DD68A File Offset: 0x000DB88A
	public void SetSliderValue(float value, int index)
	{
		this.DelayAmount = value;
	}

	// Token: 0x060028C1 RID: 10433 RVA: 0x000DD693 File Offset: 0x000DB893
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x060028C2 RID: 10434 RVA: 0x000DD69A File Offset: 0x000DB89A
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP"), this.DelayAmount);
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x000DD6BB File Offset: 0x000DB8BB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicGateBuffer>(-905833192, LogicGateBuffer.OnCopySettingsDelegate);
	}

	// Token: 0x060028C4 RID: 10436 RVA: 0x000DD6D4 File Offset: 0x000DB8D4
	private void OnCopySettings(object data)
	{
		LogicGateBuffer component = ((GameObject)data).GetComponent<LogicGateBuffer>();
		if (component != null)
		{
			this.DelayAmount = component.DelayAmount;
		}
	}

	// Token: 0x060028C5 RID: 10437 RVA: 0x000DD704 File Offset: 0x000DB904
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.meter.SetPositionPercent(1f);
	}

	// Token: 0x060028C6 RID: 10438 RVA: 0x000DD750 File Offset: 0x000DB950
	private void Update()
	{
		float positionPercent;
		if (this.input_was_previously_positive)
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

	// Token: 0x060028C7 RID: 10439 RVA: 0x000DD7A7 File Offset: 0x000DB9A7
	public override void LogicTick()
	{
		if (!this.input_was_previously_positive && this.delayTicksRemaining > 0)
		{
			this.delayTicksRemaining--;
			if (this.delayTicksRemaining <= 0)
			{
				this.OnDelay();
			}
		}
	}

	// Token: 0x060028C8 RID: 10440 RVA: 0x000DD7D8 File Offset: 0x000DB9D8
	protected override int GetCustomValue(int val1, int val2)
	{
		if (val1 != 0)
		{
			this.input_was_previously_positive = true;
			this.delayTicksRemaining = 0;
			this.meter.SetPositionPercent(0f);
		}
		else if (this.delayTicksRemaining <= 0)
		{
			if (this.input_was_previously_positive)
			{
				this.delayTicksRemaining = this.DelayAmountTicks;
			}
			this.input_was_previously_positive = false;
		}
		if (val1 == 0 && this.delayTicksRemaining <= 0)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060028C9 RID: 10441 RVA: 0x000DD83C File Offset: 0x000DBA3C
	private void OnDelay()
	{
		if (this.cleaningUp)
		{
			return;
		}
		this.delayTicksRemaining = 0;
		this.meter.SetPositionPercent(1f);
		if (this.outputValueOne == 0)
		{
			return;
		}
		int outputCellOne = base.OutputCellOne;
		if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
		{
			return;
		}
		this.outputValueOne = 0;
		base.RefreshAnimation();
	}

	// Token: 0x040017F7 RID: 6135
	[Serialize]
	private bool input_was_previously_positive;

	// Token: 0x040017F8 RID: 6136
	[Serialize]
	private float delayAmount = 5f;

	// Token: 0x040017F9 RID: 6137
	[Serialize]
	private int delayTicksRemaining;

	// Token: 0x040017FA RID: 6138
	private MeterController meter;

	// Token: 0x040017FB RID: 6139
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040017FC RID: 6140
	private static readonly EventSystem.IntraObjectHandler<LogicGateBuffer> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateBuffer>(delegate(LogicGateBuffer component, object data)
	{
		component.OnCopySettings(data);
	});
}
