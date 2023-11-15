using System;

namespace Team17.Online
{
	// Token: 0x0200072E RID: 1838
	public abstract class PCOnlinePlatformManager : Manager, IOnlinePlatformManager
	{
		// Token: 0x060022FC RID: 8956 RVA: 0x000A7B43 File Offset: 0x000A5F43
		public string Name()
		{
			return "EDITOR";
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x000A7B4A File Offset: 0x000A5F4A
		public bool PluginsReady()
		{
			return false;
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x000A7B4D File Offset: 0x000A5F4D
		public IOnlineMultiplayerNotificationCoordinator OnlineMultiplayerNotificationCoordinator()
		{
			return null;
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x000A7B50 File Offset: 0x000A5F50
		public IOnlineFriendsCoordinator OnlineFriendsCoordinator()
		{
			return null;
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x000A7B53 File Offset: 0x000A5F53
		public IOnlineAvatarImageCoordinator OnlineAvatarImageCoordinator()
		{
			return null;
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x000A7B56 File Offset: 0x000A5F56
		public IOnlineMultiplayerConnectionModeCoordinator OnlineMultiplayerConnectionModeCoordinator()
		{
			return null;
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x000A7B59 File Offset: 0x000A5F59
		public IOnlineMultiplayerSessionPropertyCoordinator OnlineMultiplayerSessionPropertyCoordinator()
		{
			return null;
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x000A7B5C File Offset: 0x000A5F5C
		public IOnlineMultiplayerSessionCoordinator OnlineMultiplayerSessionCoordinator()
		{
			return null;
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x000A7B5F File Offset: 0x000A5F5F
		public IOnlineMultiplayerPrivilegeChecksCoordinator OnlineMultiplayerPrivilegeChecksCoordinator()
		{
			return null;
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x000A7B62 File Offset: 0x000A5F62
		public IOnlineMultiplayerGameInviteCoordinator OnlineMultiplayerGameInviteCoordinator()
		{
			return null;
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x000A7B65 File Offset: 0x000A5F65
		public IOnlineMultiplayerSessionEnumerateCoordinator OnlineMultiplayerSessionEnumerateCoordinator()
		{
			return null;
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x000A7B68 File Offset: 0x000A5F68
		public IOnlineMultiplayerTransportStats OnlineMultiplayerTransportStats()
		{
			return null;
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000A7B6B File Offset: 0x000A5F6B
		protected void Awake()
		{
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000A7B6D File Offset: 0x000A5F6D
		protected void Start()
		{
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x000A7B6F File Offset: 0x000A5F6F
		protected void OnDestroy()
		{
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000A7B71 File Offset: 0x000A5F71
		protected void Update()
		{
		}
	}
}
