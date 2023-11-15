using System;
using OrderController;

namespace GameModes
{
	// Token: 0x020006A8 RID: 1704
	// (Invoke) Token: 0x06002057 RID: 8279
	public delegate void OnSuccessfulDeliveryClient(TeamID teamId, OrderID orderId, float timePropRemainingPercentage, int tip, bool wasCombo, ClientPlateStation station);
}
