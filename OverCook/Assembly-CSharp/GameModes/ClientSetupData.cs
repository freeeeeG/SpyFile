using System;

namespace GameModes
{
	// Token: 0x020006AE RID: 1710
	public struct ClientSetupData
	{
		// Token: 0x040018E2 RID: 6370
		public IClientRoundTimer m_roundTimer;

		// Token: 0x040018E3 RID: 6371
		public ClientOrderControllerBuilder m_orderControllerBuilder;

		// Token: 0x040018E4 RID: 6372
		public OnSessionConfigChanged m_onSessionConfigChangedCallback;

		// Token: 0x040018E5 RID: 6373
		public OnOrderAddedClient m_onOrderAdded;

		// Token: 0x040018E6 RID: 6374
		public OnOrderExpiredClient m_onOrderExpired;

		// Token: 0x040018E7 RID: 6375
		public OnSuccessfulDeliveryClient m_onSuccessfulDelivery;

		// Token: 0x040018E8 RID: 6376
		public OnFailedDeliveryClient m_onFailedDelivery;

		// Token: 0x040018E9 RID: 6377
		public OnOutroClient m_onOutro;
	}
}
