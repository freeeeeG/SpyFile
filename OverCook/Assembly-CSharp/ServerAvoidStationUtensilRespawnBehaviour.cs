using System;

// Token: 0x020005EC RID: 1516
public class ServerAvoidStationUtensilRespawnBehaviour : ServerUtensilRespawnBehaviour
{
	// Token: 0x06001CDA RID: 7386 RVA: 0x0008DC46 File Offset: 0x0008C046
	protected override bool CanRespawnOnStation(ServerAttachStation _attachStation)
	{
		return !_attachStation.CompareTag("CookingStation");
	}
}
