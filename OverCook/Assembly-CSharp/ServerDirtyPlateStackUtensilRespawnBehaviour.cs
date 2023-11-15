using System;
using UnityEngine;

// Token: 0x02000603 RID: 1539
public class ServerDirtyPlateStackUtensilRespawnBehaviour : ServerUtensilRespawnBehaviour
{
	// Token: 0x06001D35 RID: 7477 RVA: 0x0008F820 File Offset: 0x0008DC20
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		ServerPlateStackBase serverPlateStackBase = base.gameObject.RequestComponent<ServerPlateStackBase>();
		if (serverPlateStackBase != null)
		{
			this.m_platingStepData = serverPlateStackBase.GetPlatingStep();
		}
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x0008F858 File Offset: 0x0008DC58
	protected override bool CanRespawnOnStation(ServerAttachStation _attachStation)
	{
		ServerPlateReturnStation serverPlateReturnStation = _attachStation.gameObject.RequestComponent<ServerPlateReturnStation>();
		if (serverPlateReturnStation != null)
		{
			PlateReturnStation plateReturnStation = _attachStation.gameObject.RequireComponent<PlateReturnStation>();
			if (plateReturnStation.m_stackPrefab != null && plateReturnStation.m_stackPrefab.RequestComponent<DirtyPlateStack>() != null)
			{
				return (this.m_platingStepData == null || serverPlateReturnStation.GetPlatingStep() == this.m_platingStepData) && serverPlateReturnStation.CanReturnPlate();
			}
		}
		return false;
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x0008F8E4 File Offset: 0x0008DCE4
	protected override ServerAttachStation GetFreeAttachStation()
	{
		if (this.m_IdealSpawnLocation != null && this.m_IdealSpawnLocation.enabled && this.m_IdealSpawnLocation.gameObject.activeInHierarchy && base.IsInLevelBounds(this.m_IdealSpawnLocation, null) && this.CanRespawnOnStation(this.m_IdealSpawnLocation))
		{
			return this.m_IdealSpawnLocation;
		}
		return base.GetFreeAttachStation();
	}

	// Token: 0x06001D38 RID: 7480 RVA: 0x0008F957 File Offset: 0x0008DD57
	protected override ServerAttachStation[] GetRespawnStations()
	{
		return UnityEngine.Object.FindObjectsOfType<ServerPlateReturnStation>().ConvertAll((ServerPlateReturnStation _station) => _station.gameObject.RequireComponent<ServerAttachStation>());
	}

	// Token: 0x06001D39 RID: 7481 RVA: 0x0008F980 File Offset: 0x0008DD80
	protected override void AddItemToStation(ServerAttachStation _station)
	{
		ServerPlateReturnStation serverPlateReturnStation = _station.gameObject.RequestComponent<ServerPlateReturnStation>();
		if (serverPlateReturnStation == null)
		{
			base.AddItemToStation(_station);
		}
		else
		{
			ServerDirtyPlateStack serverDirtyPlateStack = base.gameObject.RequireComponent<ServerDirtyPlateStack>();
			for (int i = 0; i < serverDirtyPlateStack.GetSize(); i++)
			{
				serverPlateReturnStation.ReturnPlate();
			}
			NetworkUtils.DestroyObjectsRecursive(base.gameObject);
		}
	}

	// Token: 0x040016B2 RID: 5810
	private PlatingStepData m_platingStepData;
}
