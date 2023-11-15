using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200062B RID: 1579
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicCritterCountSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x0600281C RID: 10268 RVA: 0x000D9D3E File Offset: 0x000D7F3E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectable = base.GetComponent<KSelectable>();
		base.Subscribe<LogicCritterCountSensor>(-905833192, LogicCritterCountSensor.OnCopySettingsDelegate);
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x000D9D64 File Offset: 0x000D7F64
	private void OnCopySettings(object data)
	{
		LogicCritterCountSensor component = ((GameObject)data).GetComponent<LogicCritterCountSensor>();
		if (component != null)
		{
			this.countThreshold = component.countThreshold;
			this.activateOnGreaterThan = component.activateOnGreaterThan;
			this.countCritters = component.countCritters;
			this.countEggs = component.countEggs;
		}
	}

	// Token: 0x0600281E RID: 10270 RVA: 0x000D9DB6 File Offset: 0x000D7FB6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600281F RID: 10271 RVA: 0x000D9DEC File Offset: 0x000D7FEC
	public void Sim200ms(float dt)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			this.currentCount = 0;
			if (this.countCritters)
			{
				this.currentCount += roomOfGameObject.cavity.creatures.Count;
			}
			if (this.countEggs)
			{
				this.currentCount += roomOfGameObject.cavity.eggs.Count;
			}
			bool state = this.activateOnGreaterThan ? (this.currentCount > this.countThreshold) : (this.currentCount < this.countThreshold);
			this.SetState(state);
			if (this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
			{
				this.selectable.RemoveStatusItem(this.roomStatusGUID, false);
				return;
			}
		}
		else
		{
			if (!this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
			{
				this.roomStatusGUID = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom, null);
			}
			this.SetState(false);
		}
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x000D9F08 File Offset: 0x000D8108
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x000D9F17 File Offset: 0x000D8117
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x000D9F38 File Offset: 0x000D8138
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			if (this.switchedOn)
			{
				component.Queue("on", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			component.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002823 RID: 10275 RVA: 0x000D9FD8 File Offset: 0x000D81D8
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06002824 RID: 10276 RVA: 0x000DA02B File Offset: 0x000D822B
	// (set) Token: 0x06002825 RID: 10277 RVA: 0x000DA034 File Offset: 0x000D8234
	public float Threshold
	{
		get
		{
			return (float)this.countThreshold;
		}
		set
		{
			this.countThreshold = (int)value;
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06002826 RID: 10278 RVA: 0x000DA03E File Offset: 0x000D823E
	// (set) Token: 0x06002827 RID: 10279 RVA: 0x000DA046 File Offset: 0x000D8246
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnGreaterThan;
		}
		set
		{
			this.activateOnGreaterThan = value;
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06002828 RID: 10280 RVA: 0x000DA04F File Offset: 0x000D824F
	public float CurrentValue
	{
		get
		{
			return (float)this.currentCount;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06002829 RID: 10281 RVA: 0x000DA058 File Offset: 0x000D8258
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x0600282A RID: 10282 RVA: 0x000DA05F File Offset: 0x000D825F
	public float RangeMax
	{
		get
		{
			return 64f;
		}
	}

	// Token: 0x0600282B RID: 10283 RVA: 0x000DA066 File Offset: 0x000D8266
	public float GetRangeMinInputField()
	{
		return this.RangeMin;
	}

	// Token: 0x0600282C RID: 10284 RVA: 0x000DA06E File Offset: 0x000D826E
	public float GetRangeMaxInputField()
	{
		return this.RangeMax;
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x0600282D RID: 10285 RVA: 0x000DA076 File Offset: 0x000D8276
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TITLE;
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x0600282E RID: 10286 RVA: 0x000DA07D File Offset: 0x000D827D
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.VALUE_NAME;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x0600282F RID: 10287 RVA: 0x000DA084 File Offset: 0x000D8284
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06002830 RID: 10288 RVA: 0x000DA090 File Offset: 0x000D8290
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002831 RID: 10289 RVA: 0x000DA09C File Offset: 0x000D829C
	public string Format(float value, bool units)
	{
		return value.ToString();
	}

	// Token: 0x06002832 RID: 10290 RVA: 0x000DA0A5 File Offset: 0x000D82A5
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002833 RID: 10291 RVA: 0x000DA0AD File Offset: 0x000D82AD
	public float ProcessedInputValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002834 RID: 10292 RVA: 0x000DA0B5 File Offset: 0x000D82B5
	public LocString ThresholdValueUnits()
	{
		return "";
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06002835 RID: 10293 RVA: 0x000DA0C1 File Offset: 0x000D82C1
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06002836 RID: 10294 RVA: 0x000DA0C4 File Offset: 0x000D82C4
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06002837 RID: 10295 RVA: 0x000DA0C7 File Offset: 0x000D82C7
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04001758 RID: 5976
	private bool wasOn;

	// Token: 0x04001759 RID: 5977
	[Serialize]
	public bool countEggs = true;

	// Token: 0x0400175A RID: 5978
	[Serialize]
	public bool countCritters = true;

	// Token: 0x0400175B RID: 5979
	[Serialize]
	public int countThreshold;

	// Token: 0x0400175C RID: 5980
	[Serialize]
	public bool activateOnGreaterThan = true;

	// Token: 0x0400175D RID: 5981
	private int currentCount;

	// Token: 0x0400175E RID: 5982
	private KSelectable selectable;

	// Token: 0x0400175F RID: 5983
	private Guid roomStatusGUID;

	// Token: 0x04001760 RID: 5984
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001761 RID: 5985
	private static readonly EventSystem.IntraObjectHandler<LogicCritterCountSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicCritterCountSensor>(delegate(LogicCritterCountSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
