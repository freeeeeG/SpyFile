using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000642 RID: 1602
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimeOfDaySensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x060029EE RID: 10734 RVA: 0x000E0C08 File Offset: 0x000DEE08
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTimeOfDaySensor>(-905833192, LogicTimeOfDaySensor.OnCopySettingsDelegate);
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x000E0C24 File Offset: 0x000DEE24
	private void OnCopySettings(object data)
	{
		LogicTimeOfDaySensor component = ((GameObject)data).GetComponent<LogicTimeOfDaySensor>();
		if (component != null)
		{
			this.startTime = component.startTime;
			this.duration = component.duration;
		}
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x000E0C5E File Offset: 0x000DEE5E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060029F1 RID: 10737 RVA: 0x000E0C94 File Offset: 0x000DEE94
	public void Sim200ms(float dt)
	{
		float currentCycleAsPercentage = GameClock.Instance.GetCurrentCycleAsPercentage();
		bool state = false;
		if (currentCycleAsPercentage >= this.startTime && currentCycleAsPercentage < this.startTime + this.duration)
		{
			state = true;
		}
		if (currentCycleAsPercentage < this.startTime + this.duration - 1f)
		{
			state = true;
		}
		this.SetState(state);
	}

	// Token: 0x060029F2 RID: 10738 RVA: 0x000E0CE8 File Offset: 0x000DEEE8
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060029F3 RID: 10739 RVA: 0x000E0CF7 File Offset: 0x000DEEF7
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x000E0D18 File Offset: 0x000DEF18
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

	// Token: 0x060029F5 RID: 10741 RVA: 0x000E0DA0 File Offset: 0x000DEFA0
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001890 RID: 6288
	[SerializeField]
	[Serialize]
	public float startTime;

	// Token: 0x04001891 RID: 6289
	[SerializeField]
	[Serialize]
	public float duration = 1f;

	// Token: 0x04001892 RID: 6290
	private bool wasOn;

	// Token: 0x04001893 RID: 6291
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001894 RID: 6292
	private static readonly EventSystem.IntraObjectHandler<LogicTimeOfDaySensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimeOfDaySensor>(delegate(LogicTimeOfDaySensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
