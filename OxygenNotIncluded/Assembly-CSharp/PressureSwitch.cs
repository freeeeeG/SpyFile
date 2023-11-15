using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000674 RID: 1652
[SerializationConfig(MemberSerialization.OptIn)]
public class PressureSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06002BC0 RID: 11200 RVA: 0x000E8A10 File Offset: 0x000E6C10
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

	// Token: 0x06002BC1 RID: 11201 RVA: 0x000E8AD8 File Offset: 0x000E6CD8
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x000E8B2B File Offset: 0x000E6D2B
	// (set) Token: 0x06002BC3 RID: 11203 RVA: 0x000E8B33 File Offset: 0x000E6D33
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

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06002BC4 RID: 11204 RVA: 0x000E8B3C File Offset: 0x000E6D3C
	// (set) Token: 0x06002BC5 RID: 11205 RVA: 0x000E8B44 File Offset: 0x000E6D44
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

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06002BC6 RID: 11206 RVA: 0x000E8B50 File Offset: 0x000E6D50
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

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x000E8B81 File Offset: 0x000E6D81
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06002BC8 RID: 11208 RVA: 0x000E8B89 File Offset: 0x000E6D89
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x000E8B91 File Offset: 0x000E6D91
	public float GetRangeMinInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMin;
		}
		return this.rangeMin * 1000f;
	}

	// Token: 0x06002BCA RID: 11210 RVA: 0x000E8BAF File Offset: 0x000E6DAF
	public float GetRangeMaxInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMax;
		}
		return this.rangeMax * 1000f;
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06002BCB RID: 11211 RVA: 0x000E8BCD File Offset: 0x000E6DCD
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06002BCC RID: 11212 RVA: 0x000E8BD4 File Offset: 0x000E6DD4
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06002BCD RID: 11213 RVA: 0x000E8BDB File Offset: 0x000E6DDB
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06002BCE RID: 11214 RVA: 0x000E8BE7 File Offset: 0x000E6DE7
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002BCF RID: 11215 RVA: 0x000E8BF4 File Offset: 0x000E6DF4
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

	// Token: 0x06002BD0 RID: 11216 RVA: 0x000E8C20 File Offset: 0x000E6E20
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

	// Token: 0x06002BD1 RID: 11217 RVA: 0x000E8C4A File Offset: 0x000E6E4A
	public float ProcessedInputValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input /= 1000f;
		}
		return input;
	}

	// Token: 0x06002BD2 RID: 11218 RVA: 0x000E8C5F File Offset: 0x000E6E5F
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x000E8C6F File Offset: 0x000E6E6F
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000E8C72 File Offset: 0x000E6E72
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06002BD5 RID: 11221 RVA: 0x000E8C75 File Offset: 0x000E6E75
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x040019B1 RID: 6577
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x040019B2 RID: 6578
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x040019B3 RID: 6579
	public float rangeMin;

	// Token: 0x040019B4 RID: 6580
	public float rangeMax = 1f;

	// Token: 0x040019B5 RID: 6581
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x040019B6 RID: 6582
	private const int WINDOW_SIZE = 8;

	// Token: 0x040019B7 RID: 6583
	private float[] samples = new float[8];

	// Token: 0x040019B8 RID: 6584
	private int sampleIdx;
}
