using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000641 RID: 1601
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTemperatureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x1700028E RID: 654
	// (get) Token: 0x060029CE RID: 10702 RVA: 0x000E0768 File Offset: 0x000DE968
	public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

	// Token: 0x060029CF RID: 10703 RVA: 0x000E078D File Offset: 0x000DE98D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTemperatureSensor>(-905833192, LogicTemperatureSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060029D0 RID: 10704 RVA: 0x000E07A8 File Offset: 0x000DE9A8
	private void OnCopySettings(object data)
	{
		LogicTemperatureSensor component = ((GameObject)data).GetComponent<LogicTemperatureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060029D1 RID: 10705 RVA: 0x000E07E4 File Offset: 0x000DE9E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060029D2 RID: 10706 RVA: 0x000E0838 File Offset: 0x000DEA38
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8 && !this.dirty)
		{
			int i = Grid.PosToCell(this);
			if (Grid.Mass[i] > 0f)
			{
				this.temperatures[this.simUpdateCounter] = Grid.Temperature[i];
				this.simUpdateCounter++;
			}
			return;
		}
		this.simUpdateCounter = 0;
		this.dirty = false;
		this.averageTemp = 0f;
		for (int j = 0; j < 8; j++)
		{
			this.averageTemp += this.temperatures[j];
		}
		this.averageTemp /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageTemp > this.thresholdTemperature && !base.IsSwitchedOn) || (this.averageTemp <= this.thresholdTemperature && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageTemp >= this.thresholdTemperature && base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x060029D3 RID: 10707 RVA: 0x000E094F File Offset: 0x000DEB4F
	public float GetTemperature()
	{
		return this.averageTemp;
	}

	// Token: 0x060029D4 RID: 10708 RVA: 0x000E0957 File Offset: 0x000DEB57
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x000E0966 File Offset: 0x000DEB66
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x000E0984 File Offset: 0x000DEB84
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

	// Token: 0x060029D7 RID: 10711 RVA: 0x000E0A0C File Offset: 0x000DEC0C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x060029D8 RID: 10712 RVA: 0x000E0A5F File Offset: 0x000DEC5F
	// (set) Token: 0x060029D9 RID: 10713 RVA: 0x000E0A67 File Offset: 0x000DEC67
	public float Threshold
	{
		get
		{
			return this.thresholdTemperature;
		}
		set
		{
			this.thresholdTemperature = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x060029DA RID: 10714 RVA: 0x000E0A77 File Offset: 0x000DEC77
	// (set) Token: 0x060029DB RID: 10715 RVA: 0x000E0A7F File Offset: 0x000DEC7F
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x060029DC RID: 10716 RVA: 0x000E0A8F File Offset: 0x000DEC8F
	public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x060029DD RID: 10717 RVA: 0x000E0A97 File Offset: 0x000DEC97
	public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x060029DE RID: 10718 RVA: 0x000E0A9F File Offset: 0x000DEC9F
	public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x000E0AA7 File Offset: 0x000DECA7
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x060029E0 RID: 10720 RVA: 0x000E0AB5 File Offset: 0x000DECB5
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x060029E1 RID: 10721 RVA: 0x000E0AC3 File Offset: 0x000DECC3
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x060029E2 RID: 10722 RVA: 0x000E0ACA File Offset: 0x000DECCA
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x060029E3 RID: 10723 RVA: 0x000E0AD1 File Offset: 0x000DECD1
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x060029E4 RID: 10724 RVA: 0x000E0ADD File Offset: 0x000DECDD
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x000E0AE9 File Offset: 0x000DECE9
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, true);
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x000E0AF5 File Offset: 0x000DECF5
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x000E0AFD File Offset: 0x000DECFD
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x000E0B08 File Offset: 0x000DED08
	public LocString ThresholdValueUnits()
	{
		LocString result = null;
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			result = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
			break;
		case GameUtil.TemperatureUnit.Fahrenheit:
			result = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
			break;
		case GameUtil.TemperatureUnit.Kelvin:
			result = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
			break;
		}
		return result;
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x060029E9 RID: 10729 RVA: 0x000E0B48 File Offset: 0x000DED48
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x060029EA RID: 10730 RVA: 0x000E0B4B File Offset: 0x000DED4B
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x060029EB RID: 10731 RVA: 0x000E0B50 File Offset: 0x000DED50
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(25f, 260f),
				new NonLinearSlider.Range(50f, 400f),
				new NonLinearSlider.Range(12f, 1500f),
				new NonLinearSlider.Range(13f, 10000f)
			};
		}
	}

	// Token: 0x04001883 RID: 6275
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001884 RID: 6276
	private int simUpdateCounter;

	// Token: 0x04001885 RID: 6277
	[Serialize]
	public float thresholdTemperature = 280f;

	// Token: 0x04001886 RID: 6278
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04001887 RID: 6279
	[Serialize]
	private bool dirty = true;

	// Token: 0x04001888 RID: 6280
	public float minTemp;

	// Token: 0x04001889 RID: 6281
	public float maxTemp = 373.15f;

	// Token: 0x0400188A RID: 6282
	private const int NumFrameDelay = 8;

	// Token: 0x0400188B RID: 6283
	private float[] temperatures = new float[8];

	// Token: 0x0400188C RID: 6284
	private float averageTemp;

	// Token: 0x0400188D RID: 6285
	private bool wasOn;

	// Token: 0x0400188E RID: 6286
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400188F RID: 6287
	private static readonly EventSystem.IntraObjectHandler<LogicTemperatureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTemperatureSensor>(delegate(LogicTemperatureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
