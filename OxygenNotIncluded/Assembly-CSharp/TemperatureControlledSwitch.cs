using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006A5 RID: 1701
[SerializationConfig(MemberSerialization.OptIn)]
public class TemperatureControlledSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06002E02 RID: 11778 RVA: 0x000F3534 File Offset: 0x000F1734
	public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

	// Token: 0x06002E03 RID: 11779 RVA: 0x000F3559 File Offset: 0x000F1759
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
	}

	// Token: 0x06002E04 RID: 11780 RVA: 0x000F3578 File Offset: 0x000F1778
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8)
		{
			this.temperatures[this.simUpdateCounter] = Grid.Temperature[Grid.PosToCell(this)];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.averageTemp = 0f;
		for (int i = 0; i < 8; i++)
		{
			this.averageTemp += this.temperatures[i];
		}
		this.averageTemp /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageTemp > this.thresholdTemperature && !base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageTemp > this.thresholdTemperature && base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06002E05 RID: 11781 RVA: 0x000F366C File Offset: 0x000F186C
	public float GetTemperature()
	{
		return this.averageTemp;
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06002E06 RID: 11782 RVA: 0x000F3674 File Offset: 0x000F1874
	// (set) Token: 0x06002E07 RID: 11783 RVA: 0x000F367C File Offset: 0x000F187C
	public float Threshold
	{
		get
		{
			return this.thresholdTemperature;
		}
		set
		{
			this.thresholdTemperature = value;
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06002E08 RID: 11784 RVA: 0x000F3685 File Offset: 0x000F1885
	// (set) Token: 0x06002E09 RID: 11785 RVA: 0x000F368D File Offset: 0x000F188D
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x06002E0A RID: 11786 RVA: 0x000F3696 File Offset: 0x000F1896
	public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x06002E0B RID: 11787 RVA: 0x000F369E File Offset: 0x000F189E
	public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x06002E0C RID: 11788 RVA: 0x000F36A6 File Offset: 0x000F18A6
	public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

	// Token: 0x06002E0D RID: 11789 RVA: 0x000F36AE File Offset: 0x000F18AE
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06002E0E RID: 11790 RVA: 0x000F36BC File Offset: 0x000F18BC
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x06002E0F RID: 11791 RVA: 0x000F36CA File Offset: 0x000F18CA
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06002E10 RID: 11792 RVA: 0x000F36D1 File Offset: 0x000F18D1
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06002E11 RID: 11793 RVA: 0x000F36D8 File Offset: 0x000F18D8
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x06002E12 RID: 11794 RVA: 0x000F36E4 File Offset: 0x000F18E4
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002E13 RID: 11795 RVA: 0x000F36F0 File Offset: 0x000F18F0
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
	}

	// Token: 0x06002E14 RID: 11796 RVA: 0x000F36FC File Offset: 0x000F18FC
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002E15 RID: 11797 RVA: 0x000F3704 File Offset: 0x000F1904
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x06002E16 RID: 11798 RVA: 0x000F370C File Offset: 0x000F190C
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

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06002E17 RID: 11799 RVA: 0x000F374C File Offset: 0x000F194C
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.InputField;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06002E18 RID: 11800 RVA: 0x000F374F File Offset: 0x000F194F
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06002E19 RID: 11801 RVA: 0x000F3752 File Offset: 0x000F1952
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04001B19 RID: 6937
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001B1A RID: 6938
	private int simUpdateCounter;

	// Token: 0x04001B1B RID: 6939
	[Serialize]
	public float thresholdTemperature = 280f;

	// Token: 0x04001B1C RID: 6940
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04001B1D RID: 6941
	public float minTemp;

	// Token: 0x04001B1E RID: 6942
	public float maxTemp = 373.15f;

	// Token: 0x04001B1F RID: 6943
	private const int NumFrameDelay = 8;

	// Token: 0x04001B20 RID: 6944
	private float[] temperatures = new float[8];

	// Token: 0x04001B21 RID: 6945
	private float averageTemp;
}
