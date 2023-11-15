using System;

// Token: 0x02000601 RID: 1537
public class ServerCookingUtensilRespawnBehaviour : ServerUtensilRespawnBehaviour
{
	// Token: 0x06001D2F RID: 7471 RVA: 0x0008F69F File Offset: 0x0008DA9F
	private void Awake()
	{
		this.m_cookingHandler = base.gameObject.RequestComponent<CookingHandler>();
		this.m_mixingHandler = base.gameObject.RequestComponent<MixingHandler>();
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x0008F6C3 File Offset: 0x0008DAC3
	public override float GetStationRespawnPriority(ServerAttachStation _station)
	{
		return base.GetRespawnDistance(_station.transform.position) + ((!_station.CompareTag("CookingStation")) ? 1000f : 0f);
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x0008F6F8 File Offset: 0x0008DAF8
	protected override bool CanRespawnOnStation(ServerAttachStation _attachStation)
	{
		return !_attachStation.CompareTag("PlateReturn") && !_attachStation.CompareTag("PlateStation") && !(_attachStation.gameObject.RequestComponent<RubbishBin>() != null) && !(_attachStation.gameObject.RequestComponent<ConveyorStation>() != null) && !(_attachStation.gameObject.RequestComponent<WashingStation>() != null) && (!_attachStation.CompareTag("CookingStation") || !this.InvalidStationFilter(_attachStation)) && _attachStation.CanAttachToSelf(base.gameObject, default(PlacementContext));
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x0008F79C File Offset: 0x0008DB9C
	private bool InvalidStationFilter(ServerAttachStation _station)
	{
		if (this.m_mixingHandler != null)
		{
			return _station.gameObject.RequestComponent<MixingStation>() == null;
		}
		if (this.m_cookingHandler != null)
		{
			CookingStation cookingStation = _station.gameObject.RequestComponent<CookingStation>();
			return cookingStation == null || cookingStation.m_stationType != this.m_cookingHandler.m_stationType;
		}
		return false;
	}

	// Token: 0x040016AF RID: 5807
	private const float c_incorrectStationModifier = 1000f;

	// Token: 0x040016B0 RID: 5808
	private CookingHandler m_cookingHandler;

	// Token: 0x040016B1 RID: 5809
	private MixingHandler m_mixingHandler;
}
