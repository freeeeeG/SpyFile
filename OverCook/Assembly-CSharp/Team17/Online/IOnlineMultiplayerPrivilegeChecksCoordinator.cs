using System;

namespace Team17.Online
{
	// Token: 0x02000936 RID: 2358
	public interface IOnlineMultiplayerPrivilegeChecksCoordinator
	{
		// Token: 0x06002E76 RID: 11894
		bool IsIdle();

		// Token: 0x06002E77 RID: 11895
		bool Start(GamepadUser localUser, OnlineMultiplayerPrivilegeCheckCallback callback);

		// Token: 0x06002E78 RID: 11896
		void Cancel();
	}
}
