using System;

namespace GameModes
{
	// Token: 0x020006AD RID: 1709
	public struct ServerSetupData
	{
		// Token: 0x040018DA RID: 6362
		public IServerRoundTimer m_roundTimer;

		// Token: 0x040018DB RID: 6363
		public ServerOrderControllerBuilder m_orderControllerBuilder;

		// Token: 0x040018DC RID: 6364
		public OnSessionConfigChanged m_onSessionConfigChangedCallback;

		// Token: 0x040018DD RID: 6365
		public OnOrderAddedServer m_onOrderAdded;

		// Token: 0x040018DE RID: 6366
		public OnOrderExpiredServer m_onOrderExpired;

		// Token: 0x040018DF RID: 6367
		public OnSuccessfulDeliveryServer m_onSuccessfulDelivery;

		// Token: 0x040018E0 RID: 6368
		public OnFailedDeliveryServer m_onFailedDelivery;

		// Token: 0x040018E1 RID: 6369
		public OnOutroSceneServer m_onOutroScene;
	}
}
