using System;

namespace Team17.Online.Multiplayer.Connection
{
	// Token: 0x0200087D RID: 2173
	public interface NetworkConnection
	{
		// Token: 0x06002A0C RID: 10764
		IOnlineMultiplayerSessionUserId GetRemoteSessionUserId();

		// Token: 0x06002A0D RID: 10765
		bool SendMessage(byte[] data, int size, bool bReliable);

		// Token: 0x06002A0E RID: 10766
		void HandleReceivedBytes(byte[] data, int size);

		// Token: 0x06002A0F RID: 10767
		void Dispatch();

		// Token: 0x06002A10 RID: 10768
		void Disconnect();

		// Token: 0x06002A11 RID: 10769
		ConnectionStats GetConnectionStats(bool bReliable);

		// Token: 0x06002A12 RID: 10770
		bool CheckReceivedSequenceNumber(bool bReliable, uint sequenceNumber);

		// Token: 0x06002A13 RID: 10771
		void SetLatencyTestPaused(bool paused);

		// Token: 0x06002A14 RID: 10772
		bool GetLatencyTestPaused();
	}
}
