using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000638 RID: 1592
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicMassSensor : Switch, ISaveLoadable, IThresholdSwitch
{
	// Token: 0x06002924 RID: 10532 RVA: 0x000DE818 File Offset: 0x000DCA18
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicMassSensor>(-905833192, LogicMassSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x000DE834 File Offset: 0x000DCA34
	private void OnCopySettings(object data)
	{
		LogicMassSensor component = ((GameObject)data).GetComponent<LogicMassSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002926 RID: 10534 RVA: 0x000DE870 File Offset: 0x000DCA70
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateVisualState(true);
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		this.solidChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SolidChanged", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.PickupablesChanged", base.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupablesChanged));
		this.floorSwitchActivatorChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SwitchActivatorChanged", base.gameObject, cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, new Action<object>(this.OnActivatorsChanged));
		base.OnToggle += this.SwitchToggled;
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x000DE93E File Offset: 0x000DCB3E
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.solidChangedEntry);
		GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
		GameScenePartitioner.Instance.Free(ref this.floorSwitchActivatorChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002928 RID: 10536 RVA: 0x000DE978 File Offset: 0x000DCB78
	private void Update()
	{
		this.toggleCooldown = Mathf.Max(0f, this.toggleCooldown - Time.deltaTime);
		if (this.toggleCooldown == 0f)
		{
			float currentValue = this.CurrentValue;
			if ((this.activateAboveThreshold ? (currentValue > this.threshold) : (currentValue < this.threshold)) != base.IsSwitchedOn)
			{
				this.Toggle();
				this.toggleCooldown = 0.15f;
			}
			this.UpdateVisualState(false);
		}
	}

	// Token: 0x06002929 RID: 10537 RVA: 0x000DE9F4 File Offset: 0x000DCBF4
	private void OnSolidChanged(object data)
	{
		int i = Grid.CellAbove(this.NaturalBuildingCell());
		if (Grid.Solid[i])
		{
			this.massSolid = Grid.Mass[i];
			return;
		}
		this.massSolid = 0f;
	}

	// Token: 0x0600292A RID: 10538 RVA: 0x000DEA38 File Offset: 0x000DCC38
	private void OnPickupablesChanged(object data)
	{
		float num = 0f;
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			if (!(pickupable == null) && !pickupable.wasAbsorbed)
			{
				KPrefabID component = pickupable.GetComponent<KPrefabID>();
				if (!component.HasTag(GameTags.Creature) || (component.HasTag(GameTags.Creatures.Walker) || component.HasTag(GameTags.Creatures.Hoverer) || component.HasTag(GameTags.Creatures.Flopping)))
				{
					num += pickupable.PrimaryElement.Mass;
				}
			}
		}
		pooledList.Recycle();
		this.massPickupables = num;
	}

	// Token: 0x0600292B RID: 10539 RVA: 0x000DEB24 File Offset: 0x000DCD24
	private void OnActivatorsChanged(object data)
	{
		float num = 0f;
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.floorSwitchActivatorLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			FloorSwitchActivator floorSwitchActivator = pooledList[i].obj as FloorSwitchActivator;
			if (!(floorSwitchActivator == null))
			{
				num += floorSwitchActivator.PrimaryElement.Mass;
			}
		}
		pooledList.Recycle();
		this.massActivators = num;
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x0600292C RID: 10540 RVA: 0x000DEBC0 File Offset: 0x000DCDC0
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x0600292D RID: 10541 RVA: 0x000DEBC7 File Offset: 0x000DCDC7
	// (set) Token: 0x0600292E RID: 10542 RVA: 0x000DEBCF File Offset: 0x000DCDCF
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

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x0600292F RID: 10543 RVA: 0x000DEBD8 File Offset: 0x000DCDD8
	// (set) Token: 0x06002930 RID: 10544 RVA: 0x000DEBE0 File Offset: 0x000DCDE0
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

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06002931 RID: 10545 RVA: 0x000DEBE9 File Offset: 0x000DCDE9
	public float CurrentValue
	{
		get
		{
			return this.massSolid + this.massPickupables + this.massActivators;
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06002932 RID: 10546 RVA: 0x000DEBFF File Offset: 0x000DCDFF
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06002933 RID: 10547 RVA: 0x000DEC07 File Offset: 0x000DCE07
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x000DEC0F File Offset: 0x000DCE0F
	public float GetRangeMinInputField()
	{
		return this.rangeMin;
	}

	// Token: 0x06002935 RID: 10549 RVA: 0x000DEC17 File Offset: 0x000DCE17
	public float GetRangeMaxInputField()
	{
		return this.rangeMax;
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06002936 RID: 10550 RVA: 0x000DEC1F File Offset: 0x000DCE1F
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06002937 RID: 10551 RVA: 0x000DEC26 File Offset: 0x000DCE26
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06002938 RID: 10552 RVA: 0x000DEC32 File Offset: 0x000DCE32
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002939 RID: 10553 RVA: 0x000DEC40 File Offset: 0x000DCE40
	public string Format(float value, bool units)
	{
		GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.Kilogram;
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, massFormat, units, "{0:0.#}");
	}

	// Token: 0x0600293A RID: 10554 RVA: 0x000DEC5F File Offset: 0x000DCE5F
	public float ProcessedSliderValue(float input)
	{
		input = Mathf.Round(input);
		return input;
	}

	// Token: 0x0600293B RID: 10555 RVA: 0x000DEC6A File Offset: 0x000DCE6A
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x0600293C RID: 10556 RVA: 0x000DEC6D File Offset: 0x000DCE6D
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(false);
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x0600293D RID: 10557 RVA: 0x000DEC75 File Offset: 0x000DCE75
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x0600293E RID: 10558 RVA: 0x000DEC78 File Offset: 0x000DCE78
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x0600293F RID: 10559 RVA: 0x000DEC7B File Offset: 0x000DCE7B
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06002940 RID: 10560 RVA: 0x000DEC88 File Offset: 0x000DCE88
	private void SwitchToggled(bool toggled_on)
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, toggled_on ? 1 : 0);
	}

	// Token: 0x06002941 RID: 10561 RVA: 0x000DECA4 File Offset: 0x000DCEA4
	private void UpdateVisualState(bool force = false)
	{
		bool flag = this.CurrentValue > this.threshold;
		if (flag != this.was_pressed || this.was_on != base.IsSwitchedOn || force)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (flag)
			{
				if (force)
				{
					component.Play(base.IsSwitchedOn ? "on_down" : "off_down", KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(base.IsSwitchedOn ? "on_down_pre" : "off_down_pre", KAnim.PlayMode.Once, 1f, 0f);
					component.Queue(base.IsSwitchedOn ? "on_down" : "off_down", KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			else if (force)
			{
				component.Play(base.IsSwitchedOn ? "on_up" : "off_up", KAnim.PlayMode.Once, 1f, 0f);
			}
			else
			{
				component.Play(base.IsSwitchedOn ? "on_up_pre" : "off_up_pre", KAnim.PlayMode.Once, 1f, 0f);
				component.Queue(base.IsSwitchedOn ? "on_up" : "off_up", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.was_pressed = flag;
			this.was_on = base.IsSwitchedOn;
		}
	}

	// Token: 0x06002942 RID: 10562 RVA: 0x000DEE14 File Offset: 0x000DD014
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001826 RID: 6182
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04001827 RID: 6183
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04001828 RID: 6184
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04001829 RID: 6185
	private bool was_pressed;

	// Token: 0x0400182A RID: 6186
	private bool was_on;

	// Token: 0x0400182B RID: 6187
	public float rangeMin;

	// Token: 0x0400182C RID: 6188
	public float rangeMax = 1f;

	// Token: 0x0400182D RID: 6189
	[Serialize]
	private float massSolid;

	// Token: 0x0400182E RID: 6190
	[Serialize]
	private float massPickupables;

	// Token: 0x0400182F RID: 6191
	[Serialize]
	private float massActivators;

	// Token: 0x04001830 RID: 6192
	private const float MIN_TOGGLE_TIME = 0.15f;

	// Token: 0x04001831 RID: 6193
	private float toggleCooldown = 0.15f;

	// Token: 0x04001832 RID: 6194
	private HandleVector<int>.Handle solidChangedEntry;

	// Token: 0x04001833 RID: 6195
	private HandleVector<int>.Handle pickupablesChangedEntry;

	// Token: 0x04001834 RID: 6196
	private HandleVector<int>.Handle floorSwitchActivatorChangedEntry;

	// Token: 0x04001835 RID: 6197
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001836 RID: 6198
	private static readonly EventSystem.IntraObjectHandler<LogicMassSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicMassSensor>(delegate(LogicMassSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
