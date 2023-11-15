using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000637 RID: 1591
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicLightSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06002909 RID: 10505 RVA: 0x000DE4FF File Offset: 0x000DC6FF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x000DE534 File Offset: 0x000DC734
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 4)
		{
			this.levels[this.simUpdateCounter] = (float)Grid.LightIntensity[Grid.PosToCell(this)];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.averageBrightness = 0f;
		for (int i = 0; i < 4; i++)
		{
			this.averageBrightness += this.levels[i];
		}
		this.averageBrightness /= 4f;
		if (this.activateOnBrighterThan)
		{
			if ((this.averageBrightness > this.thresholdBrightness && !base.IsSwitchedOn) || (this.averageBrightness < this.thresholdBrightness && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageBrightness > this.thresholdBrightness && base.IsSwitchedOn) || (this.averageBrightness < this.thresholdBrightness && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x000DE629 File Offset: 0x000DC829
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x000DE638 File Offset: 0x000DC838
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x0600290D RID: 10509 RVA: 0x000DE658 File Offset: 0x000DC858
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

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x0600290E RID: 10510 RVA: 0x000DE6DF File Offset: 0x000DC8DF
	// (set) Token: 0x0600290F RID: 10511 RVA: 0x000DE6E7 File Offset: 0x000DC8E7
	public float Threshold
	{
		get
		{
			return this.thresholdBrightness;
		}
		set
		{
			this.thresholdBrightness = value;
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06002910 RID: 10512 RVA: 0x000DE6F0 File Offset: 0x000DC8F0
	// (set) Token: 0x06002911 RID: 10513 RVA: 0x000DE6F8 File Offset: 0x000DC8F8
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnBrighterThan;
		}
		set
		{
			this.activateOnBrighterThan = value;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06002912 RID: 10514 RVA: 0x000DE701 File Offset: 0x000DC901
	public float CurrentValue
	{
		get
		{
			return this.averageBrightness;
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06002913 RID: 10515 RVA: 0x000DE709 File Offset: 0x000DC909
	public float RangeMin
	{
		get
		{
			return this.minBrightness;
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06002914 RID: 10516 RVA: 0x000DE711 File Offset: 0x000DC911
	public float RangeMax
	{
		get
		{
			return this.maxBrightness;
		}
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x000DE719 File Offset: 0x000DC919
	public float GetRangeMinInputField()
	{
		return this.RangeMin;
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x000DE721 File Offset: 0x000DC921
	public float GetRangeMaxInputField()
	{
		return this.RangeMax;
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06002917 RID: 10519 RVA: 0x000DE729 File Offset: 0x000DC929
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.BRIGHTNESSSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06002918 RID: 10520 RVA: 0x000DE730 File Offset: 0x000DC930
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS;
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06002919 RID: 10521 RVA: 0x000DE737 File Offset: 0x000DC937
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x0600291A RID: 10522 RVA: 0x000DE743 File Offset: 0x000DC943
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_BELOW;
		}
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x000DE74F File Offset: 0x000DC94F
	public string Format(float value, bool units)
	{
		if (units)
		{
			return GameUtil.GetFormattedLux((int)value);
		}
		return string.Format("{0}", (int)value);
	}

	// Token: 0x0600291C RID: 10524 RVA: 0x000DE76D File Offset: 0x000DC96D
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x0600291D RID: 10525 RVA: 0x000DE775 File Offset: 0x000DC975
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x000DE778 File Offset: 0x000DC978
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.LIGHT.LUX;
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x0600291F RID: 10527 RVA: 0x000DE77F File Offset: 0x000DC97F
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06002920 RID: 10528 RVA: 0x000DE782 File Offset: 0x000DC982
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06002921 RID: 10529 RVA: 0x000DE785 File Offset: 0x000DC985
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06002922 RID: 10530 RVA: 0x000DE794 File Offset: 0x000DC994
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x0400181D RID: 6173
	private int simUpdateCounter;

	// Token: 0x0400181E RID: 6174
	[Serialize]
	public float thresholdBrightness = 280f;

	// Token: 0x0400181F RID: 6175
	[Serialize]
	public bool activateOnBrighterThan = true;

	// Token: 0x04001820 RID: 6176
	public float minBrightness;

	// Token: 0x04001821 RID: 6177
	public float maxBrightness = 15000f;

	// Token: 0x04001822 RID: 6178
	private const int NumFrameDelay = 4;

	// Token: 0x04001823 RID: 6179
	private float[] levels = new float[4];

	// Token: 0x04001824 RID: 6180
	private float averageBrightness;

	// Token: 0x04001825 RID: 6181
	private bool wasOn;
}
