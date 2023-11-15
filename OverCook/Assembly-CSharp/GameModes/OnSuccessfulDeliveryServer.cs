using System;
using OrderController;

namespace GameModes
{
	// Token: 0x020006A3 RID: 1699
	// (Invoke) Token: 0x06002043 RID: 8259
	public delegate void OnSuccessfulDeliveryServer(OrderID orderID, RecipeList.Entry entry, float timePropRemainingPercentage, bool wasCombo, ServerPlateStation station);
}
