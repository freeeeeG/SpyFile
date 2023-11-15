using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005E0 RID: 1504
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitTemperatureSensor : ConduitThresholdSensor, IThresholdSwitch
{
	// Token: 0x06002568 RID: 9576 RVA: 0x000CB940 File Offset: 0x000C9B40
	private void GetContentsTemperature(out float temperature, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			temperature = contents.temperature;
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		if (pickupable != null && pickupable.PrimaryElement.Mass > 0f)
		{
			temperature = pickupable.PrimaryElement.Temperature;
			hasMass = true;
			return;
		}
		temperature = 0f;
		hasMass = false;
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06002569 RID: 9577 RVA: 0x000CB9E0 File Offset: 0x000C9BE0
	public override float CurrentValue
	{
		get
		{
			float num;
			bool flag;
			this.GetContentsTemperature(out num, out flag);
			if (flag)
			{
				this.lastValue = num;
			}
			return this.lastValue;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x0600256A RID: 9578 RVA: 0x000CBA07 File Offset: 0x000C9C07
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x0600256B RID: 9579 RVA: 0x000CBA0F File Offset: 0x000C9C0F
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x0600256C RID: 9580 RVA: 0x000CBA17 File Offset: 0x000C9C17
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x0600256D RID: 9581 RVA: 0x000CBA25 File Offset: 0x000C9C25
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x0600256E RID: 9582 RVA: 0x000CBA33 File Offset: 0x000C9C33
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x0600256F RID: 9583 RVA: 0x000CBA3A File Offset: 0x000C9C3A
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE;
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06002570 RID: 9584 RVA: 0x000CBA41 File Offset: 0x000C9C41
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06002571 RID: 9585 RVA: 0x000CBA4D File Offset: 0x000C9C4D
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002572 RID: 9586 RVA: 0x000CBA59 File Offset: 0x000C9C59
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
	}

	// Token: 0x06002573 RID: 9587 RVA: 0x000CBA65 File Offset: 0x000C9C65
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002574 RID: 9588 RVA: 0x000CBA6D File Offset: 0x000C9C6D
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x06002575 RID: 9589 RVA: 0x000CBA78 File Offset: 0x000C9C78
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

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06002576 RID: 9590 RVA: 0x000CBAB8 File Offset: 0x000C9CB8
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06002577 RID: 9591 RVA: 0x000CBABB File Offset: 0x000C9CBB
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06002578 RID: 9592 RVA: 0x000CBAC0 File Offset: 0x000C9CC0
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

	// Token: 0x04001568 RID: 5480
	public float rangeMin;

	// Token: 0x04001569 RID: 5481
	public float rangeMax = 373.15f;

	// Token: 0x0400156A RID: 5482
	[Serialize]
	private float lastValue;
}
