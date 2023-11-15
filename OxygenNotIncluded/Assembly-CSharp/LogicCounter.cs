using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200062A RID: 1578
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicCounter : Switch, ISaveLoadable
{
	// Token: 0x0600280D RID: 10253 RVA: 0x000D97F9 File Offset: 0x000D79F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicCounter>(-905833192, LogicCounter.OnCopySettingsDelegate);
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x000D9814 File Offset: 0x000D7A14
	private void OnCopySettings(object data)
	{
		LogicCounter component = ((GameObject)data).GetComponent<LogicCounter>();
		if (component != null)
		{
			this.maxCount = component.maxCount;
			this.resetCountAtMax = component.resetCountAtMax;
			this.advancedMode = component.advancedMode;
		}
	}

	// Token: 0x0600280F RID: 10255 RVA: 0x000D985C File Offset: 0x000D7A5C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		if (this.maxCount == 0)
		{
			this.maxCount = 10;
		}
		base.Subscribe<LogicCounter>(-801688580, LogicCounter.OnLogicValueChangedDelegate);
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", component.FlipY ? "meter_dn" : "meter_up", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.UpdateMeter();
	}

	// Token: 0x06002810 RID: 10256 RVA: 0x000D9921 File Offset: 0x000D7B21
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
	}

	// Token: 0x06002811 RID: 10257 RVA: 0x000D994E File Offset: 0x000D7B4E
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002812 RID: 10258 RVA: 0x000D995D File Offset: 0x000D7B5D
	public void UpdateLogicCircuit()
	{
		if (this.receivedFirstSignal)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicCounter.OUTPUT_PORT_ID, this.switchedOn ? 1 : 0);
		}
	}

	// Token: 0x06002813 RID: 10259 RVA: 0x000D9984 File Offset: 0x000D7B84
	public void UpdateMeter()
	{
		float num = (float)(this.advancedMode ? (this.currentCount % this.maxCount) : this.currentCount);
		if (num == 10f)
		{
			num = 0f;
		}
		this.meter.SetPositionPercent(num / 10f);
	}

	// Token: 0x06002814 RID: 10260 RVA: 0x000D99D0 File Offset: 0x000D7BD0
	public void UpdateVisualState(bool force = false)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (!this.receivedFirstSignal)
		{
			component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (this.wasOn != this.switchedOn || force)
		{
			int num = (this.switchedOn ? 4 : 0) + (this.wasResetting ? 2 : 0) + (this.wasIncrementing ? 1 : 0);
			this.wasOn = this.switchedOn;
			component.Play("on_" + num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002815 RID: 10261 RVA: 0x000D9A78 File Offset: 0x000D7C78
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LogicCounter.INPUT_PORT_ID)
		{
			int newValue = logicValueChanged.newValue;
			this.receivedFirstSignal = true;
			if (LogicCircuitNetwork.IsBitActive(0, newValue))
			{
				if (!this.wasIncrementing)
				{
					this.wasIncrementing = true;
					if (!this.wasResetting)
					{
						if (this.currentCount == this.maxCount || this.currentCount >= 10)
						{
							this.currentCount = 0;
						}
						this.currentCount++;
						this.UpdateMeter();
						this.SetCounterState();
						if (this.currentCount == this.maxCount && this.resetCountAtMax)
						{
							this.resetRequested = true;
						}
					}
				}
			}
			else
			{
				this.wasIncrementing = false;
			}
		}
		else
		{
			if (!(logicValueChanged.portID == LogicCounter.RESET_PORT_ID))
			{
				return;
			}
			int newValue2 = logicValueChanged.newValue;
			this.receivedFirstSignal = true;
			if (LogicCircuitNetwork.IsBitActive(0, newValue2))
			{
				if (!this.wasResetting)
				{
					this.wasResetting = true;
					this.ResetCounter();
				}
			}
			else
			{
				this.wasResetting = false;
			}
		}
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x000D9B90 File Offset: 0x000D7D90
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x000D9BE3 File Offset: 0x000D7DE3
	public void ResetCounter()
	{
		this.resetRequested = false;
		this.currentCount = 0;
		this.SetCounterState();
		this.UpdateVisualState(true);
		this.UpdateMeter();
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x000D9C0C File Offset: 0x000D7E0C
	public void LogicTick()
	{
		if (this.resetRequested)
		{
			this.ResetCounter();
		}
		if (this.pulsingActive)
		{
			this.pulseTicksRemaining--;
			if (this.pulseTicksRemaining <= 0)
			{
				this.pulsingActive = false;
				this.SetState(false);
				this.UpdateVisualState(false);
				this.UpdateMeter();
				this.UpdateLogicCircuit();
			}
		}
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x000D9C68 File Offset: 0x000D7E68
	public void SetCounterState()
	{
		this.SetState(this.advancedMode ? (this.currentCount % this.maxCount == 0) : (this.currentCount == this.maxCount));
		if (this.advancedMode && this.currentCount % this.maxCount == 0)
		{
			this.pulsingActive = true;
			this.pulseTicksRemaining = 2;
		}
	}

	// Token: 0x04001745 RID: 5957
	[Serialize]
	public int maxCount;

	// Token: 0x04001746 RID: 5958
	[Serialize]
	public int currentCount;

	// Token: 0x04001747 RID: 5959
	[Serialize]
	public bool resetCountAtMax;

	// Token: 0x04001748 RID: 5960
	[Serialize]
	public bool advancedMode;

	// Token: 0x04001749 RID: 5961
	private bool wasOn;

	// Token: 0x0400174A RID: 5962
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400174B RID: 5963
	private static readonly EventSystem.IntraObjectHandler<LogicCounter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicCounter>(delegate(LogicCounter component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400174C RID: 5964
	private static readonly EventSystem.IntraObjectHandler<LogicCounter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicCounter>(delegate(LogicCounter component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x0400174D RID: 5965
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicCounterInput");

	// Token: 0x0400174E RID: 5966
	public static readonly HashedString RESET_PORT_ID = new HashedString("LogicCounterReset");

	// Token: 0x0400174F RID: 5967
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicCounterOutput");

	// Token: 0x04001750 RID: 5968
	private bool resetRequested;

	// Token: 0x04001751 RID: 5969
	[Serialize]
	private bool wasResetting;

	// Token: 0x04001752 RID: 5970
	[Serialize]
	private bool wasIncrementing;

	// Token: 0x04001753 RID: 5971
	[Serialize]
	public bool receivedFirstSignal;

	// Token: 0x04001754 RID: 5972
	private bool pulsingActive;

	// Token: 0x04001755 RID: 5973
	private const int pulseLength = 1;

	// Token: 0x04001756 RID: 5974
	private int pulseTicksRemaining;

	// Token: 0x04001757 RID: 5975
	private MeterController meter;
}
