using System;
using UnityEngine;

// Token: 0x020005FC RID: 1532
public class ServerCleanPlateUtensilRespawnBehaviour : ServerUtensilRespawnBehaviour
{
	// Token: 0x06001D22 RID: 7458 RVA: 0x0008F458 File Offset: 0x0008D858
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		ServerPlateStackBase serverPlateStackBase = base.gameObject.RequestComponent<ServerPlateStackBase>();
		if (serverPlateStackBase != null)
		{
			this.m_platingStepData = serverPlateStackBase.GetPlatingStep();
		}
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x0008F490 File Offset: 0x0008D890
	protected override bool CanRespawnOnStation(ServerAttachStation _attachStation)
	{
		if (_attachStation.CompareTag("CookingStation") || _attachStation.CompareTag("PlateStation") || _attachStation.gameObject.RequestComponent<RubbishBin>() != null || _attachStation.gameObject.RequestComponent<ConveyorStation>() != null || _attachStation.gameObject.RequestComponent<WashingStation>() != null)
		{
			return false;
		}
		ServerPlateReturnStation serverPlateReturnStation = _attachStation.gameObject.RequestComponent<ServerPlateReturnStation>();
		if (serverPlateReturnStation != null)
		{
			PlateReturnStation plateReturnStation = _attachStation.gameObject.RequireComponent<PlateReturnStation>();
			if (plateReturnStation.m_stackPrefab != null && plateReturnStation.m_stackPrefab.RequestComponent<CleanPlateStack>() != null)
			{
				return (this.m_platingStepData == null || serverPlateReturnStation.GetPlatingStep() == this.m_platingStepData) && serverPlateReturnStation.CanReturnPlate();
			}
		}
		return _attachStation.CanAttachToSelf(base.gameObject, default(PlacementContext));
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x0008F594 File Offset: 0x0008D994
	protected override void AddItemToStation(ServerAttachStation _station)
	{
		ServerPlateReturnStation serverPlateReturnStation = _station.gameObject.RequestComponent<ServerPlateReturnStation>();
		if (serverPlateReturnStation == null)
		{
			base.AddItemToStation(_station);
		}
		else
		{
			ServerPlate x = base.gameObject.RequestComponent<ServerPlate>();
			if (x != null)
			{
				serverPlateReturnStation.ReturnPlate();
			}
			NetworkUtils.DestroyObjectsRecursive(base.gameObject);
		}
	}

	// Token: 0x040016AD RID: 5805
	private PlatingStepData m_platingStepData;
}
