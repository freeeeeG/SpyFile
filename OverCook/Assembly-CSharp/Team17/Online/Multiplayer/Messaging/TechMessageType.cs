using System;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008CE RID: 2254
	public enum TechMessageType
	{
		// Token: 0x040022FE RID: 8958
		ReliableMessageBatch,
		// Token: 0x040022FF RID: 8959
		UnreliableMessageBatch,
		// Token: 0x04002300 RID: 8960
		ReliableGameMessage,
		// Token: 0x04002301 RID: 8961
		UnreliableGameMessage,
		// Token: 0x04002302 RID: 8962
		ReliableMultiPart,
		// Token: 0x04002303 RID: 8963
		Disconnect
	}
}
