using System;

// Token: 0x02000699 RID: 1689
public class StorageLockerSmart : StorageLocker
{
	// Token: 0x06002D5F RID: 11615 RVA: 0x000F0CE5 File Offset: 0x000EEEE5
	protected override void OnPrefabInit()
	{
		base.Initialize(true);
	}

	// Token: 0x06002D60 RID: 11616 RVA: 0x000F0CF0 File Offset: 0x000EEEF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ports = base.gameObject.GetComponent<LogicPorts>();
		base.Subscribe<StorageLockerSmart>(-1697596308, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
		base.Subscribe<StorageLockerSmart>(-592767678, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
		this.UpdateLogicAndActiveState();
	}

	// Token: 0x06002D61 RID: 11617 RVA: 0x000F0D3C File Offset: 0x000EEF3C
	private void UpdateLogicCircuitCB(object data)
	{
		this.UpdateLogicAndActiveState();
	}

	// Token: 0x06002D62 RID: 11618 RVA: 0x000F0D44 File Offset: 0x000EEF44
	private void UpdateLogicAndActiveState()
	{
		bool flag = this.filteredStorage.IsFull();
		bool isOperational = this.operational.IsOperational;
		bool flag2 = flag && isOperational;
		this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, flag2 ? 1 : 0);
		this.filteredStorage.SetLogicMeter(flag2);
		this.operational.SetActive(isOperational, false);
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06002D63 RID: 11619 RVA: 0x000F0D9B File Offset: 0x000EEF9B
	// (set) Token: 0x06002D64 RID: 11620 RVA: 0x000F0DA3 File Offset: 0x000EEFA3
	public override float UserMaxCapacity
	{
		get
		{
			return base.UserMaxCapacity;
		}
		set
		{
			base.UserMaxCapacity = value;
			this.UpdateLogicAndActiveState();
		}
	}

	// Token: 0x04001AD9 RID: 6873
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x04001ADA RID: 6874
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001ADB RID: 6875
	private static readonly EventSystem.IntraObjectHandler<StorageLockerSmart> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<StorageLockerSmart>(delegate(StorageLockerSmart component, object data)
	{
		component.UpdateLogicCircuitCB(data);
	});
}
