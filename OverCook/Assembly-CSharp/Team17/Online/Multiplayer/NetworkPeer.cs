using System;
using Team17.Online.Multiplayer.Connection;
using Team17.Online.Multiplayer.Messaging;

namespace Team17.Online.Multiplayer
{
	// Token: 0x020008EF RID: 2287
	public interface NetworkPeer
	{
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06002C73 RID: 11379
		// (remove) Token: 0x06002C74 RID: 11380
		event GenericVoid<IOnlineMultiplayerSessionUserId, MessageType, Serialisable, uint, bool> OnMessageReceived;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06002C75 RID: 11381
		// (remove) Token: 0x06002C76 RID: 11382
		event GenericVoid OnLeftSession;

		// Token: 0x06002C77 RID: 11383
		void HandleReceivedBytesFromConnection(NetworkConnection connection, byte[] data, int size);

		// Token: 0x06002C78 RID: 11384
		void HandleConnectionLost(IOnlineMultiplayerSessionUserId sessionUserId, NetworkConnection connection);

		// Token: 0x06002C79 RID: 11385
		void HandleLocalLoopbackConnectionLost(NetworkConnection connection);

		// Token: 0x06002C7A RID: 11386
		void HandleDisconnectMessage(NetworkConnection connection);

		// Token: 0x06002C7B RID: 11387
		void Dispatch();

		// Token: 0x06002C7C RID: 11388
		ConnectionStats GetConnectionStats(bool bReliable);

		// Token: 0x06002C7D RID: 11389
		void SetLatencyTestPaused(bool paused);

		// Token: 0x06002C7E RID: 11390
		NetworkMessageTracker GetTracker();
	}
}
