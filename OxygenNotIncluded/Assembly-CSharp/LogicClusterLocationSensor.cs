using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000629 RID: 1577
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicClusterLocationSensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x1700021D RID: 541
	// (get) Token: 0x060027FD RID: 10237 RVA: 0x000D942D File Offset: 0x000D762D
	public bool ActiveInSpace
	{
		get
		{
			return this.activeInSpace;
		}
	}

	// Token: 0x060027FE RID: 10238 RVA: 0x000D9435 File Offset: 0x000D7635
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicClusterLocationSensor>(-905833192, LogicClusterLocationSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060027FF RID: 10239 RVA: 0x000D9450 File Offset: 0x000D7650
	private void OnCopySettings(object data)
	{
		LogicClusterLocationSensor component = ((GameObject)data).GetComponent<LogicClusterLocationSensor>();
		if (component != null)
		{
			this.activeLocations.Clear();
			for (int i = 0; i < component.activeLocations.Count; i++)
			{
				this.SetLocationEnabled(component.activeLocations[i], true);
			}
			this.activeInSpace = component.activeInSpace;
		}
	}

	// Token: 0x06002800 RID: 10240 RVA: 0x000D94B2 File Offset: 0x000D76B2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002801 RID: 10241 RVA: 0x000D94E5 File Offset: 0x000D76E5
	public void SetLocationEnabled(AxialI location, bool setting)
	{
		if (!setting)
		{
			this.activeLocations.Remove(location);
			return;
		}
		if (!this.activeLocations.Contains(location))
		{
			this.activeLocations.Add(location);
		}
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000D9512 File Offset: 0x000D7712
	public void SetSpaceEnabled(bool setting)
	{
		this.activeInSpace = setting;
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x000D951C File Offset: 0x000D771C
	public void Sim200ms(float dt)
	{
		bool state = this.CheckCurrentLocationSelected();
		this.SetState(state);
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x000D9538 File Offset: 0x000D7738
	private bool CheckCurrentLocationSelected()
	{
		AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
		return this.activeLocations.Contains(myWorldLocation) || (this.activeInSpace && this.CheckInEmptySpace());
	}

	// Token: 0x06002805 RID: 10245 RVA: 0x000D9574 File Offset: 0x000D7774
	private bool CheckInEmptySpace()
	{
		bool result = true;
		AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (!worldContainer.IsModuleInterior && worldContainer.GetMyWorldLocation() == myWorldLocation)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x000D95E8 File Offset: 0x000D77E8
	public bool CheckLocationSelected(AxialI location)
	{
		return this.activeLocations.Contains(location);
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x000D95F6 File Offset: 0x000D77F6
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x000D9605 File Offset: 0x000D7805
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002809 RID: 10249 RVA: 0x000D9624 File Offset: 0x000D7824
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
			bool flag = true;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior && worldContainer.GetMyWorldLocation() == myWorldLocation)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				component.Play(this.switchedOn ? "on_space_pre" : "on_space_pst", KAnim.PlayMode.Once, 1f, 0f);
				component.Queue(this.switchedOn ? "on_space" : "off_space", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			component.Play(this.switchedOn ? "on_asteroid_pre" : "on_asteroid_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on_asteroid" : "off_asteroid", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x0600280A RID: 10250 RVA: 0x000D9770 File Offset: 0x000D7970
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001740 RID: 5952
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001741 RID: 5953
	[Serialize]
	private List<AxialI> activeLocations = new List<AxialI>();

	// Token: 0x04001742 RID: 5954
	[Serialize]
	private bool activeInSpace = true;

	// Token: 0x04001743 RID: 5955
	private bool wasOn;

	// Token: 0x04001744 RID: 5956
	private static readonly EventSystem.IntraObjectHandler<LogicClusterLocationSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicClusterLocationSensor>(delegate(LogicClusterLocationSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
