using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200063C RID: 1596
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicRadiationSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06002970 RID: 10608 RVA: 0x000DF664 File Offset: 0x000DD864
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRadiationSensor>(-905833192, LogicRadiationSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002971 RID: 10609 RVA: 0x000DF680 File Offset: 0x000DD880
	private void OnCopySettings(object data)
	{
		LogicRadiationSensor component = ((GameObject)data).GetComponent<LogicRadiationSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002972 RID: 10610 RVA: 0x000DF6BA File Offset: 0x000DD8BA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002973 RID: 10611 RVA: 0x000DF6F0 File Offset: 0x000DD8F0
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8 && !this.dirty)
		{
			int i = Grid.PosToCell(this);
			this.radHistory[this.simUpdateCounter] = Grid.Radiation[i];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.dirty = false;
		this.averageRads = 0f;
		for (int j = 0; j < 8; j++)
		{
			this.averageRads += this.radHistory[j];
		}
		this.averageRads /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageRads > this.thresholdRads && !base.IsSwitchedOn) || (this.averageRads <= this.thresholdRads && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageRads >= this.thresholdRads && base.IsSwitchedOn) || (this.averageRads < this.thresholdRads && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x000DF7F5 File Offset: 0x000DD9F5
	public float GetAverageRads()
	{
		return this.averageRads;
	}

	// Token: 0x06002975 RID: 10613 RVA: 0x000DF7FD File Offset: 0x000DD9FD
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002976 RID: 10614 RVA: 0x000DF80C File Offset: 0x000DDA0C
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x000DF82C File Offset: 0x000DDA2C
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

	// Token: 0x06002978 RID: 10616 RVA: 0x000DF8B4 File Offset: 0x000DDAB4
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06002979 RID: 10617 RVA: 0x000DF907 File Offset: 0x000DDB07
	// (set) Token: 0x0600297A RID: 10618 RVA: 0x000DF90F File Offset: 0x000DDB0F
	public float Threshold
	{
		get
		{
			return this.thresholdRads;
		}
		set
		{
			this.thresholdRads = value;
			this.dirty = true;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x0600297B RID: 10619 RVA: 0x000DF91F File Offset: 0x000DDB1F
	// (set) Token: 0x0600297C RID: 10620 RVA: 0x000DF927 File Offset: 0x000DDB27
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

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x0600297D RID: 10621 RVA: 0x000DF937 File Offset: 0x000DDB37
	public float CurrentValue
	{
		get
		{
			return this.GetAverageRads();
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x0600297E RID: 10622 RVA: 0x000DF93F File Offset: 0x000DDB3F
	public float RangeMin
	{
		get
		{
			return this.minRads;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x0600297F RID: 10623 RVA: 0x000DF947 File Offset: 0x000DDB47
	public float RangeMax
	{
		get
		{
			return this.maxRads;
		}
	}

	// Token: 0x06002980 RID: 10624 RVA: 0x000DF94F File Offset: 0x000DDB4F
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x000DF95D File Offset: 0x000DDB5D
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06002982 RID: 10626 RVA: 0x000DF96B File Offset: 0x000DDB6B
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.RADIATIONSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06002983 RID: 10627 RVA: 0x000DF972 File Offset: 0x000DDB72
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION;
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06002984 RID: 10628 RVA: 0x000DF979 File Offset: 0x000DDB79
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06002985 RID: 10629 RVA: 0x000DF985 File Offset: 0x000DDB85
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002986 RID: 10630 RVA: 0x000DF991 File Offset: 0x000DDB91
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}

	// Token: 0x06002987 RID: 10631 RVA: 0x000DF99A File Offset: 0x000DDB9A
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002988 RID: 10632 RVA: 0x000DF9A2 File Offset: 0x000DDBA2
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06002989 RID: 10633 RVA: 0x000DF9A5 File Offset: 0x000DDBA5
	public LocString ThresholdValueUnits()
	{
		return "";
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x0600298A RID: 10634 RVA: 0x000DF9B1 File Offset: 0x000DDBB1
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x0600298B RID: 10635 RVA: 0x000DF9B4 File Offset: 0x000DDBB4
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x0600298C RID: 10636 RVA: 0x000DF9B8 File Offset: 0x000DDBB8
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(50f, 200f),
				new NonLinearSlider.Range(25f, 1000f),
				new NonLinearSlider.Range(25f, 5000f)
			};
		}
	}

	// Token: 0x0400184F RID: 6223
	private int simUpdateCounter;

	// Token: 0x04001850 RID: 6224
	[Serialize]
	public float thresholdRads = 280f;

	// Token: 0x04001851 RID: 6225
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04001852 RID: 6226
	[Serialize]
	private bool dirty = true;

	// Token: 0x04001853 RID: 6227
	public float minRads;

	// Token: 0x04001854 RID: 6228
	public float maxRads = 5000f;

	// Token: 0x04001855 RID: 6229
	private const int NumFrameDelay = 8;

	// Token: 0x04001856 RID: 6230
	private float[] radHistory = new float[8];

	// Token: 0x04001857 RID: 6231
	private float averageRads;

	// Token: 0x04001858 RID: 6232
	private bool wasOn;

	// Token: 0x04001859 RID: 6233
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400185A RID: 6234
	private static readonly EventSystem.IntraObjectHandler<LogicRadiationSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRadiationSensor>(delegate(LogicRadiationSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
