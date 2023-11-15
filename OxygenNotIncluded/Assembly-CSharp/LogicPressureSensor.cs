using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200063B RID: 1595
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicPressureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06002952 RID: 10578 RVA: 0x000DF26B File Offset: 0x000DD46B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicPressureSensor>(-905833192, LogicPressureSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002953 RID: 10579 RVA: 0x000DF284 File Offset: 0x000DD484
	private void OnCopySettings(object data)
	{
		LogicPressureSensor component = ((GameObject)data).GetComponent<LogicPressureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002954 RID: 10580 RVA: 0x000DF2BE File Offset: 0x000DD4BE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x000DF2F4 File Offset: 0x000DD4F4
	public void Sim200ms(float dt)
	{
		int num = Grid.PosToCell(this);
		if (this.sampleIdx < 8)
		{
			float num2 = Grid.Element[num].IsState(this.desiredState) ? Grid.Mass[num] : 0f;
			this.samples[this.sampleIdx] = num2;
			this.sampleIdx++;
			return;
		}
		this.sampleIdx = 0;
		float currentValue = this.CurrentValue;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x000DF3BC File Offset: 0x000DD5BC
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06002957 RID: 10583 RVA: 0x000DF3CB File Offset: 0x000DD5CB
	// (set) Token: 0x06002958 RID: 10584 RVA: 0x000DF3D3 File Offset: 0x000DD5D3
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06002959 RID: 10585 RVA: 0x000DF3DC File Offset: 0x000DD5DC
	// (set) Token: 0x0600295A RID: 10586 RVA: 0x000DF3E4 File Offset: 0x000DD5E4
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x0600295B RID: 10587 RVA: 0x000DF3F0 File Offset: 0x000DD5F0
	public float CurrentValue
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += this.samples[i];
			}
			return num / 8f;
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x0600295C RID: 10588 RVA: 0x000DF421 File Offset: 0x000DD621
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x0600295D RID: 10589 RVA: 0x000DF429 File Offset: 0x000DD629
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x0600295E RID: 10590 RVA: 0x000DF431 File Offset: 0x000DD631
	public float GetRangeMinInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMin;
		}
		return this.rangeMin * 1000f;
	}

	// Token: 0x0600295F RID: 10591 RVA: 0x000DF44F File Offset: 0x000DD64F
	public float GetRangeMaxInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMax;
		}
		return this.rangeMax * 1000f;
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06002960 RID: 10592 RVA: 0x000DF46D File Offset: 0x000DD66D
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06002961 RID: 10593 RVA: 0x000DF474 File Offset: 0x000DD674
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06002962 RID: 10594 RVA: 0x000DF480 File Offset: 0x000DD680
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002963 RID: 10595 RVA: 0x000DF48C File Offset: 0x000DD68C
	public string Format(float value, bool units)
	{
		GameUtil.MetricMassFormat massFormat;
		if (this.desiredState == Element.State.Gas)
		{
			massFormat = GameUtil.MetricMassFormat.Gram;
		}
		else
		{
			massFormat = GameUtil.MetricMassFormat.Kilogram;
		}
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, massFormat, units, "{0:0.#}");
	}

	// Token: 0x06002964 RID: 10596 RVA: 0x000DF4B8 File Offset: 0x000DD6B8
	public float ProcessedSliderValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input = Mathf.Round(input * 1000f) / 1000f;
		}
		else
		{
			input = Mathf.Round(input);
		}
		return input;
	}

	// Token: 0x06002965 RID: 10597 RVA: 0x000DF4E2 File Offset: 0x000DD6E2
	public float ProcessedInputValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input /= 1000f;
		}
		return input;
	}

	// Token: 0x06002966 RID: 10598 RVA: 0x000DF4F7 File Offset: 0x000DD6F7
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06002967 RID: 10599 RVA: 0x000DF507 File Offset: 0x000DD707
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06002968 RID: 10600 RVA: 0x000DF50E File Offset: 0x000DD70E
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06002969 RID: 10601 RVA: 0x000DF511 File Offset: 0x000DD711
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x0600296A RID: 10602 RVA: 0x000DF514 File Offset: 0x000DD714
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x0600296B RID: 10603 RVA: 0x000DF521 File Offset: 0x000DD721
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x0600296C RID: 10604 RVA: 0x000DF540 File Offset: 0x000DD740
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x000DF5C8 File Offset: 0x000DD7C8
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001844 RID: 6212
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04001845 RID: 6213
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04001846 RID: 6214
	private bool wasOn;

	// Token: 0x04001847 RID: 6215
	public float rangeMin;

	// Token: 0x04001848 RID: 6216
	public float rangeMax = 1f;

	// Token: 0x04001849 RID: 6217
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x0400184A RID: 6218
	private const int WINDOW_SIZE = 8;

	// Token: 0x0400184B RID: 6219
	private float[] samples = new float[8];

	// Token: 0x0400184C RID: 6220
	private int sampleIdx;

	// Token: 0x0400184D RID: 6221
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400184E RID: 6222
	private static readonly EventSystem.IntraObjectHandler<LogicPressureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicPressureSensor>(delegate(LogicPressureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
