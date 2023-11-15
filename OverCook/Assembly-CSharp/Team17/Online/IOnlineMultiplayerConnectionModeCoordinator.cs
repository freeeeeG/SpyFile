using System;

namespace Team17.Online
{
	// Token: 0x0200093C RID: 2364
	public interface IOnlineMultiplayerConnectionModeCoordinator
	{
		// Token: 0x06002E81 RID: 11905
		string DebugStatus();

		// Token: 0x06002E82 RID: 11906
		bool IsIdle();

		// Token: 0x06002E83 RID: 11907
		void RegisterErrorCallback(OnlineMultiplayerConnectionModeErrorCallback callback);

		// Token: 0x06002E84 RID: 11908
		void UnRegisterErrorCallback(OnlineMultiplayerConnectionModeErrorCallback callback);

		// Token: 0x06002E85 RID: 11909
		OnlineMultiplayerConnectionMode Mode();

		// Token: 0x06002E86 RID: 11910
		bool Connect(GamepadUser localUser, OnlineMultiplayerConnectionMode mode, OnlineMultiplayerConnectionModeConnectCallback connectCallback);

		// Token: 0x06002E87 RID: 11911
		void Disconnect();
	}
}
