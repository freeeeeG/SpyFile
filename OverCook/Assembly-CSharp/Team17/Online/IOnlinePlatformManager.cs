using System;

namespace Team17.Online
{
	// Token: 0x02000717 RID: 1815
	public interface IOnlinePlatformManager
	{
		// Token: 0x06002278 RID: 8824
		string Name();

		// Token: 0x06002279 RID: 8825
		bool PluginsReady();

		// Token: 0x0600227A RID: 8826
		IOnlineMultiplayerNotificationCoordinator OnlineMultiplayerNotificationCoordinator();

		// Token: 0x0600227B RID: 8827
		IOnlineAvatarImageCoordinator OnlineAvatarImageCoordinator();

		// Token: 0x0600227C RID: 8828
		IOnlineFriendsCoordinator OnlineFriendsCoordinator();

		// Token: 0x0600227D RID: 8829
		IOnlineMultiplayerConnectionModeCoordinator OnlineMultiplayerConnectionModeCoordinator();

		// Token: 0x0600227E RID: 8830
		IOnlineMultiplayerSessionPropertyCoordinator OnlineMultiplayerSessionPropertyCoordinator();

		// Token: 0x0600227F RID: 8831
		IOnlineMultiplayerSessionCoordinator OnlineMultiplayerSessionCoordinator();

		// Token: 0x06002280 RID: 8832
		IOnlineMultiplayerSessionEnumerateCoordinator OnlineMultiplayerSessionEnumerateCoordinator();

		// Token: 0x06002281 RID: 8833
		IOnlineMultiplayerPrivilegeChecksCoordinator OnlineMultiplayerPrivilegeChecksCoordinator();

		// Token: 0x06002282 RID: 8834
		IOnlineMultiplayerGameInviteCoordinator OnlineMultiplayerGameInviteCoordinator();

		// Token: 0x06002283 RID: 8835
		IOnlineMultiplayerTransportStats OnlineMultiplayerTransportStats();
	}
}
