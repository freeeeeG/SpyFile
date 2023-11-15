using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000034 RID: 52
[AddComponentMenu("KMonoBehaviour/scripts/CargoBay")]
public class CargoBayConduit : KMonoBehaviour
{
	// Token: 0x060000E9 RID: 233 RVA: 0x000076B8 File Offset: 0x000058B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (CargoBayConduit.connectedPortStatus == null)
		{
			CargoBayConduit.connectedPortStatus = new StatusItem("CONNECTED_ROCKET_PORT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null);
			CargoBayConduit.connectedWrongPortStatus = new StatusItem("CONNECTED_ROCKET_WRONG_PORT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID, true, 129022, null);
			CargoBayConduit.connectedNoPortStatus = new StatusItem("CONNECTED_ROCKET_NO_PORT", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.Bad, true, OverlayModes.None.ID, true, 129022, null);
		}
		if (base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad != null)
		{
			this.OnLaunchpadChainChanged(null);
			base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Subscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
		}
		base.Subscribe<CargoBayConduit>(-1277991738, CargoBayConduit.OnLaunchDelegate);
		base.Subscribe<CargoBayConduit>(-887025858, CargoBayConduit.OnLandDelegate);
		this.storageType = base.GetComponent<CargoBay>().storageType;
		this.UpdateStatusItems();
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000077CC File Offset: 0x000059CC
	protected override void OnCleanUp()
	{
		LaunchPad currentPad = base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
		if (currentPad != null)
		{
			currentPad.Unsubscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
		}
		base.OnCleanUp();
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00007810 File Offset: 0x00005A10
	public void OnLaunch(object data)
	{
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			component.conduitType = ConduitType.None;
		}
		base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Unsubscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0000785C File Offset: 0x00005A5C
	public void OnLand(object data)
	{
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			CargoBay.CargoType cargoType = this.storageType;
			if (cargoType != CargoBay.CargoType.Liquids)
			{
				if (cargoType == CargoBay.CargoType.Gasses)
				{
					component.conduitType = ConduitType.Gas;
				}
				else
				{
					component.conduitType = ConduitType.None;
				}
			}
			else
			{
				component.conduitType = ConduitType.Liquid;
			}
		}
		base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Subscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
		this.UpdateStatusItems();
	}

	// Token: 0x060000ED RID: 237 RVA: 0x000078CE File Offset: 0x00005ACE
	private void OnLaunchpadChainChanged(object data)
	{
		this.UpdateStatusItems();
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000078D8 File Offset: 0x00005AD8
	private void UpdateStatusItems()
	{
		bool flag;
		bool flag2;
		this.HasMatchingConduitPort(out flag, out flag2);
		KSelectable component = base.GetComponent<KSelectable>();
		if (flag)
		{
			this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedPortStatus, this);
			return;
		}
		if (flag2)
		{
			this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedWrongPortStatus, this);
			return;
		}
		this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedNoPortStatus, this);
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00007948 File Offset: 0x00005B48
	private void HasMatchingConduitPort(out bool hasMatch, out bool hasAny)
	{
		hasMatch = false;
		hasAny = false;
		LaunchPad currentPad = base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
		if (currentPad == null)
		{
			return;
		}
		ChainedBuilding.StatesInstance smi = currentPad.GetSMI<ChainedBuilding.StatesInstance>();
		if (smi == null)
		{
			return;
		}
		HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
		smi.GetLinkedBuildings(ref pooledHashSet);
		foreach (ChainedBuilding.StatesInstance statesInstance in pooledHashSet)
		{
			IConduitDispenser component = statesInstance.GetComponent<IConduitDispenser>();
			if (component != null)
			{
				hasAny = true;
				if (CargoBayConduit.ElementToCargoMap[component.ConduitType] == this.storageType)
				{
					hasMatch = true;
					break;
				}
			}
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x04000086 RID: 134
	public static Dictionary<ConduitType, CargoBay.CargoType> ElementToCargoMap = new Dictionary<ConduitType, CargoBay.CargoType>
	{
		{
			ConduitType.Solid,
			CargoBay.CargoType.Solids
		},
		{
			ConduitType.Liquid,
			CargoBay.CargoType.Liquids
		},
		{
			ConduitType.Gas,
			CargoBay.CargoType.Gasses
		}
	};

	// Token: 0x04000087 RID: 135
	private static readonly EventSystem.IntraObjectHandler<CargoBayConduit> OnLaunchDelegate = new EventSystem.IntraObjectHandler<CargoBayConduit>(delegate(CargoBayConduit component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x04000088 RID: 136
	private static readonly EventSystem.IntraObjectHandler<CargoBayConduit> OnLandDelegate = new EventSystem.IntraObjectHandler<CargoBayConduit>(delegate(CargoBayConduit component, object data)
	{
		component.OnLand(data);
	});

	// Token: 0x04000089 RID: 137
	private static StatusItem connectedPortStatus;

	// Token: 0x0400008A RID: 138
	private static StatusItem connectedWrongPortStatus;

	// Token: 0x0400008B RID: 139
	private static StatusItem connectedNoPortStatus;

	// Token: 0x0400008C RID: 140
	private CargoBay.CargoType storageType;

	// Token: 0x0400008D RID: 141
	private Guid connectedConduitPortStatusItem;
}
