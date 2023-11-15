using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000643 RID: 1603
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimerSensor : Switch, ISaveLoadable, ISim33ms
{
	// Token: 0x060029F8 RID: 10744 RVA: 0x000E0E22 File Offset: 0x000DF022
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTimerSensor>(-905833192, LogicTimerSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x000E0E3C File Offset: 0x000DF03C
	private void OnCopySettings(object data)
	{
		LogicTimerSensor component = ((GameObject)data).GetComponent<LogicTimerSensor>();
		if (component != null)
		{
			this.onDuration = component.onDuration;
			this.offDuration = component.offDuration;
			this.timeElapsedInCurrentState = component.timeElapsedInCurrentState;
			this.displayCyclesMode = component.displayCyclesMode;
		}
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x000E0E8E File Offset: 0x000DF08E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x000E0EC4 File Offset: 0x000DF0C4
	public void Sim33ms(float dt)
	{
		if (this.onDuration == 0f && this.offDuration == 0f)
		{
			return;
		}
		this.timeElapsedInCurrentState += dt;
		bool flag = base.IsSwitchedOn;
		if (flag)
		{
			if (this.timeElapsedInCurrentState >= this.onDuration)
			{
				flag = false;
				this.timeElapsedInCurrentState -= this.onDuration;
			}
		}
		else if (this.timeElapsedInCurrentState >= this.offDuration)
		{
			flag = true;
			this.timeElapsedInCurrentState -= this.offDuration;
		}
		this.SetState(flag);
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x000E0F53 File Offset: 0x000DF153
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060029FD RID: 10749 RVA: 0x000E0F62 File Offset: 0x000DF162
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060029FE RID: 10750 RVA: 0x000E0F80 File Offset: 0x000DF180
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

	// Token: 0x060029FF RID: 10751 RVA: 0x000E1008 File Offset: 0x000DF208
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x06002A00 RID: 10752 RVA: 0x000E105B File Offset: 0x000DF25B
	public void ResetTimer()
	{
		this.SetState(true);
		this.OnSwitchToggled(true);
		this.timeElapsedInCurrentState = 0f;
	}

	// Token: 0x04001895 RID: 6293
	[Serialize]
	public float onDuration = 10f;

	// Token: 0x04001896 RID: 6294
	[Serialize]
	public float offDuration = 10f;

	// Token: 0x04001897 RID: 6295
	[Serialize]
	public bool displayCyclesMode;

	// Token: 0x04001898 RID: 6296
	private bool wasOn;

	// Token: 0x04001899 RID: 6297
	[SerializeField]
	[Serialize]
	public float timeElapsedInCurrentState;

	// Token: 0x0400189A RID: 6298
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400189B RID: 6299
	private static readonly EventSystem.IntraObjectHandler<LogicTimerSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimerSensor>(delegate(LogicTimerSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
