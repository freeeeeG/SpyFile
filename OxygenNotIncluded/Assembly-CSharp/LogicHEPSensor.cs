using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000635 RID: 1589
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicHEPSensor : Switch, ISaveLoadable, IThresholdSwitch, ISimEveryTick
{
	// Token: 0x060028E1 RID: 10465 RVA: 0x000DDB86 File Offset: 0x000DBD86
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicHEPSensor>(-905833192, LogicHEPSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060028E2 RID: 10466 RVA: 0x000DDBA0 File Offset: 0x000DBDA0
	private void OnCopySettings(object data)
	{
		LogicHEPSensor component = ((GameObject)data).GetComponent<LogicHEPSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060028E3 RID: 10467 RVA: 0x000DDBDC File Offset: 0x000DBDDC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
	}

	// Token: 0x060028E4 RID: 10468 RVA: 0x000DDC45 File Offset: 0x000DBE45
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.OnCleanUp();
	}

	// Token: 0x060028E5 RID: 10469 RVA: 0x000DDC78 File Offset: 0x000DBE78
	public void SimEveryTick(float dt)
	{
		if (this.waitForLogicTick)
		{
			return;
		}
		Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
		ListPool<ScenePartitionerEntry, LogicHEPSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicHEPSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
		float num = 0f;
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			HighEnergyParticle component = (scenePartitionerEntry.obj as KCollider2D).gameObject.GetComponent<HighEnergyParticle>();
			if (!(component == null) && component.isCollideable)
			{
				num += component.payload;
			}
		}
		pooledList.Recycle();
		this.foundPayload = num;
		bool flag = (this.activateOnHigherThan && num > this.thresholdPayload) || (!this.activateOnHigherThan && num < this.thresholdPayload);
		if (flag != this.switchedOn)
		{
			this.waitForLogicTick = true;
		}
		this.SetState(flag);
	}

	// Token: 0x060028E6 RID: 10470 RVA: 0x000DDD84 File Offset: 0x000DBF84
	private void LogicTick()
	{
		this.waitForLogicTick = false;
	}

	// Token: 0x060028E7 RID: 10471 RVA: 0x000DDD8D File Offset: 0x000DBF8D
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060028E8 RID: 10472 RVA: 0x000DDD9C File Offset: 0x000DBF9C
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060028E9 RID: 10473 RVA: 0x000DDDBC File Offset: 0x000DBFBC
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

	// Token: 0x060028EA RID: 10474 RVA: 0x000DDE44 File Offset: 0x000DC044
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x060028EB RID: 10475 RVA: 0x000DDE97 File Offset: 0x000DC097
	// (set) Token: 0x060028EC RID: 10476 RVA: 0x000DDE9F File Offset: 0x000DC09F
	public float Threshold
	{
		get
		{
			return this.thresholdPayload;
		}
		set
		{
			this.thresholdPayload = value;
			this.dirty = true;
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x060028ED RID: 10477 RVA: 0x000DDEAF File Offset: 0x000DC0AF
	// (set) Token: 0x060028EE RID: 10478 RVA: 0x000DDEB7 File Offset: 0x000DC0B7
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

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x060028EF RID: 10479 RVA: 0x000DDEC7 File Offset: 0x000DC0C7
	public float CurrentValue
	{
		get
		{
			return this.foundPayload;
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x060028F0 RID: 10480 RVA: 0x000DDECF File Offset: 0x000DC0CF
	public float RangeMin
	{
		get
		{
			return this.minPayload;
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x060028F1 RID: 10481 RVA: 0x000DDED7 File Offset: 0x000DC0D7
	public float RangeMax
	{
		get
		{
			return this.maxPayload;
		}
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x000DDEDF File Offset: 0x000DC0DF
	public float GetRangeMinInputField()
	{
		return this.minPayload;
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x000DDEE7 File Offset: 0x000DC0E7
	public float GetRangeMaxInputField()
	{
		return this.maxPayload;
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x060028F4 RID: 10484 RVA: 0x000DDEEF File Offset: 0x000DC0EF
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.HEPSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x060028F5 RID: 10485 RVA: 0x000DDEF6 File Offset: 0x000DC0F6
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x060028F6 RID: 10486 RVA: 0x000DDEFD File Offset: 0x000DC0FD
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x060028F7 RID: 10487 RVA: 0x000DDF09 File Offset: 0x000DC109
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060028F8 RID: 10488 RVA: 0x000DDF15 File Offset: 0x000DC115
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedHighEnergyParticles(value, GameUtil.TimeSlice.None, units);
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x000DDF1F File Offset: 0x000DC11F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x000DDF27 File Offset: 0x000DC127
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x000DDF2A File Offset: 0x000DC12A
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x060028FC RID: 10492 RVA: 0x000DDF31 File Offset: 0x000DC131
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x060028FD RID: 10493 RVA: 0x000DDF34 File Offset: 0x000DC134
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x060028FE RID: 10494 RVA: 0x000DDF38 File Offset: 0x000DC138
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(30f, 50f),
				new NonLinearSlider.Range(30f, 200f),
				new NonLinearSlider.Range(40f, 500f)
			};
		}
	}

	// Token: 0x04001803 RID: 6147
	[Serialize]
	public float thresholdPayload;

	// Token: 0x04001804 RID: 6148
	[Serialize]
	public bool activateOnHigherThan;

	// Token: 0x04001805 RID: 6149
	[Serialize]
	public bool dirty = true;

	// Token: 0x04001806 RID: 6150
	private readonly float minPayload;

	// Token: 0x04001807 RID: 6151
	private readonly float maxPayload = 500f;

	// Token: 0x04001808 RID: 6152
	private float foundPayload;

	// Token: 0x04001809 RID: 6153
	private bool waitForLogicTick;

	// Token: 0x0400180A RID: 6154
	private bool wasOn;

	// Token: 0x0400180B RID: 6155
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400180C RID: 6156
	private static readonly EventSystem.IntraObjectHandler<LogicHEPSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicHEPSensor>(delegate(LogicHEPSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
