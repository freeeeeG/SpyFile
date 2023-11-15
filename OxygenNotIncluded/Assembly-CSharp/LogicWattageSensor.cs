using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000644 RID: 1604
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicWattageSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06002A03 RID: 10755 RVA: 0x000E10B0 File Offset: 0x000DF2B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicWattageSensor>(-905833192, LogicWattageSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002A04 RID: 10756 RVA: 0x000E10CC File Offset: 0x000DF2CC
	private void OnCopySettings(object data)
	{
		LogicWattageSensor component = ((GameObject)data).GetComponent<LogicWattageSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x000E1106 File Offset: 0x000DF306
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002A06 RID: 10758 RVA: 0x000E113C File Offset: 0x000DF33C
	public void Sim200ms(float dt)
	{
		this.currentWattage = Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager.GetCircuitID(Grid.PosToCell(this)));
		this.currentWattage = Mathf.Max(0f, this.currentWattage);
		if (this.activateOnHigherThan)
		{
			if ((this.currentWattage > this.thresholdWattage && !base.IsSwitchedOn) || (this.currentWattage <= this.thresholdWattage && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.currentWattage >= this.thresholdWattage && base.IsSwitchedOn) || (this.currentWattage < this.thresholdWattage && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06002A07 RID: 10759 RVA: 0x000E11F6 File Offset: 0x000DF3F6
	public float GetWattageUsed()
	{
		return this.currentWattage;
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x000E11FE File Offset: 0x000DF3FE
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x000E120D File Offset: 0x000DF40D
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x000E122C File Offset: 0x000DF42C
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

	// Token: 0x06002A0B RID: 10763 RVA: 0x000E12B4 File Offset: 0x000DF4B4
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06002A0C RID: 10764 RVA: 0x000E1307 File Offset: 0x000DF507
	// (set) Token: 0x06002A0D RID: 10765 RVA: 0x000E130F File Offset: 0x000DF50F
	public float Threshold
	{
		get
		{
			return this.thresholdWattage;
		}
		set
		{
			this.thresholdWattage = value;
			this.dirty = true;
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06002A0E RID: 10766 RVA: 0x000E131F File Offset: 0x000DF51F
	// (set) Token: 0x06002A0F RID: 10767 RVA: 0x000E1327 File Offset: 0x000DF527
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnHigherThan;
		}
		set
		{
			this.activateOnHigherThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06002A10 RID: 10768 RVA: 0x000E1337 File Offset: 0x000DF537
	public float CurrentValue
	{
		get
		{
			return this.GetWattageUsed();
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06002A11 RID: 10769 RVA: 0x000E133F File Offset: 0x000DF53F
	public float RangeMin
	{
		get
		{
			return this.minWattage;
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06002A12 RID: 10770 RVA: 0x000E1347 File Offset: 0x000DF547
	public float RangeMax
	{
		get
		{
			return this.maxWattage;
		}
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x000E134F File Offset: 0x000DF54F
	public float GetRangeMinInputField()
	{
		return this.minWattage;
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x000E1357 File Offset: 0x000DF557
	public float GetRangeMaxInputField()
	{
		return this.maxWattage;
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06002A15 RID: 10773 RVA: 0x000E135F File Offset: 0x000DF55F
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.WATTAGESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06002A16 RID: 10774 RVA: 0x000E1366 File Offset: 0x000DF566
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE;
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06002A17 RID: 10775 RVA: 0x000E136D File Offset: 0x000DF56D
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06002A18 RID: 10776 RVA: 0x000E1379 File Offset: 0x000DF579
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002A19 RID: 10777 RVA: 0x000E1385 File Offset: 0x000DF585
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Watts, units);
	}

	// Token: 0x06002A1A RID: 10778 RVA: 0x000E138F File Offset: 0x000DF58F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x000E1397 File Offset: 0x000DF597
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x000E139A File Offset: 0x000DF59A
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.ELECTRICAL.WATT;
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06002A1D RID: 10781 RVA: 0x000E13A1 File Offset: 0x000DF5A1
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06002A1E RID: 10782 RVA: 0x000E13A4 File Offset: 0x000DF5A4
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06002A1F RID: 10783 RVA: 0x000E13A8 File Offset: 0x000DF5A8
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(5f, 5f),
				new NonLinearSlider.Range(35f, 1000f),
				new NonLinearSlider.Range(50f, 3000f),
				new NonLinearSlider.Range(10f, this.maxWattage)
			};
		}
	}

	// Token: 0x0400189C RID: 6300
	[Serialize]
	public float thresholdWattage;

	// Token: 0x0400189D RID: 6301
	[Serialize]
	public bool activateOnHigherThan;

	// Token: 0x0400189E RID: 6302
	[Serialize]
	public bool dirty = true;

	// Token: 0x0400189F RID: 6303
	private readonly float minWattage;

	// Token: 0x040018A0 RID: 6304
	private readonly float maxWattage = 1.5f * Wire.GetMaxWattageAsFloat(Wire.WattageRating.Max50000);

	// Token: 0x040018A1 RID: 6305
	private float currentWattage;

	// Token: 0x040018A2 RID: 6306
	private bool wasOn;

	// Token: 0x040018A3 RID: 6307
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040018A4 RID: 6308
	private static readonly EventSystem.IntraObjectHandler<LogicWattageSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicWattageSensor>(delegate(LogicWattageSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
