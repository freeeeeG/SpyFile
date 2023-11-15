using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200062C RID: 1580
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicDiseaseSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x0600283A RID: 10298 RVA: 0x000DA10D File Offset: 0x000D830D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicDiseaseSensor>(-905833192, LogicDiseaseSensor.OnCopySettingsDelegate);
	}

	// Token: 0x0600283B RID: 10299 RVA: 0x000DA128 File Offset: 0x000D8328
	private void OnCopySettings(object data)
	{
		LogicDiseaseSensor component = ((GameObject)data).GetComponent<LogicDiseaseSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x0600283C RID: 10300 RVA: 0x000DA162 File Offset: 0x000D8362
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600283D RID: 10301 RVA: 0x000DA1A4 File Offset: 0x000D83A4
	public void Sim200ms(float dt)
	{
		if (this.sampleIdx < 8)
		{
			int i = Grid.PosToCell(this);
			if (Grid.Mass[i] > 0f)
			{
				this.samples[this.sampleIdx] = Grid.DiseaseCount[i];
				this.sampleIdx++;
			}
			return;
		}
		this.sampleIdx = 0;
		float currentValue = this.CurrentValue;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
		this.animController.SetSymbolVisiblity(LogicDiseaseSensor.TINT_SYMBOL, currentValue > 0f);
	}

	// Token: 0x0600283E RID: 10302 RVA: 0x000DA27F File Offset: 0x000D847F
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x0600283F RID: 10303 RVA: 0x000DA28E File Offset: 0x000D848E
	// (set) Token: 0x06002840 RID: 10304 RVA: 0x000DA296 File Offset: 0x000D8496
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

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06002841 RID: 10305 RVA: 0x000DA29F File Offset: 0x000D849F
	// (set) Token: 0x06002842 RID: 10306 RVA: 0x000DA2A7 File Offset: 0x000D84A7
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

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06002843 RID: 10307 RVA: 0x000DA2B0 File Offset: 0x000D84B0
	public float CurrentValue
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += (float)this.samples[i];
			}
			return num / 8f;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06002844 RID: 10308 RVA: 0x000DA2E2 File Offset: 0x000D84E2
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06002845 RID: 10309 RVA: 0x000DA2E9 File Offset: 0x000D84E9
	public float RangeMax
	{
		get
		{
			return 100000f;
		}
	}

	// Token: 0x06002846 RID: 10310 RVA: 0x000DA2F0 File Offset: 0x000D84F0
	public float GetRangeMinInputField()
	{
		return 0f;
	}

	// Token: 0x06002847 RID: 10311 RVA: 0x000DA2F7 File Offset: 0x000D84F7
	public float GetRangeMaxInputField()
	{
		return 100000f;
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06002848 RID: 10312 RVA: 0x000DA2FE File Offset: 0x000D84FE
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06002849 RID: 10313 RVA: 0x000DA305 File Offset: 0x000D8505
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x0600284A RID: 10314 RVA: 0x000DA311 File Offset: 0x000D8511
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x0600284B RID: 10315 RVA: 0x000DA31D File Offset: 0x000D851D
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
	}

	// Token: 0x0600284C RID: 10316 RVA: 0x000DA328 File Offset: 0x000D8528
	public float ProcessedSliderValue(float input)
	{
		return input;
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x000DA32B File Offset: 0x000D852B
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x0600284E RID: 10318 RVA: 0x000DA32E File Offset: 0x000D852E
	public LocString ThresholdValueUnits()
	{
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x0600284F RID: 10319 RVA: 0x000DA335 File Offset: 0x000D8535
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06002850 RID: 10320 RVA: 0x000DA338 File Offset: 0x000D8538
	public int IncrementScale
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06002851 RID: 10321 RVA: 0x000DA33C File Offset: 0x000D853C
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06002852 RID: 10322 RVA: 0x000DA349 File Offset: 0x000D8549
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x000DA368 File Offset: 0x000D8568
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(LogicDiseaseSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				int i = Grid.PosToCell(this);
				byte b = Grid.DiseaseIdx[i];
				Color32 c = Color.white;
				if (b != 255)
				{
					Disease disease = Db.Get().Diseases[(int)b];
					c = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				}
				this.animController.SetSymbolTint(LogicDiseaseSensor.TINT_SYMBOL, c);
				return;
			}
			this.animController.Play(LogicDiseaseSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x06002854 RID: 10324 RVA: 0x000DA42C File Offset: 0x000D862C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06002855 RID: 10325 RVA: 0x000DA47F File Offset: 0x000D867F
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
		}
	}

	// Token: 0x04001762 RID: 5986
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04001763 RID: 5987
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04001764 RID: 5988
	private KBatchedAnimController animController;

	// Token: 0x04001765 RID: 5989
	private bool wasOn;

	// Token: 0x04001766 RID: 5990
	private const float rangeMin = 0f;

	// Token: 0x04001767 RID: 5991
	private const float rangeMax = 100000f;

	// Token: 0x04001768 RID: 5992
	private const int WINDOW_SIZE = 8;

	// Token: 0x04001769 RID: 5993
	private int[] samples = new int[8];

	// Token: 0x0400176A RID: 5994
	private int sampleIdx;

	// Token: 0x0400176B RID: 5995
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400176C RID: 5996
	private static readonly EventSystem.IntraObjectHandler<LogicDiseaseSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicDiseaseSensor>(delegate(LogicDiseaseSensor component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400176D RID: 5997
	private static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on_loop"
	};

	// Token: 0x0400176E RID: 5998
	private static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};

	// Token: 0x0400176F RID: 5999
	private static readonly HashedString TINT_SYMBOL = "germs";
}
