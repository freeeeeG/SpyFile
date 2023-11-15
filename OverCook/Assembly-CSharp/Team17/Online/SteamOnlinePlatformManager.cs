using System;
using UnityEngine;

namespace Team17.Online
{
	// Token: 0x02000750 RID: 1872
	public abstract class SteamOnlinePlatformManager : Manager, IOnlinePlatformManager
	{
		// Token: 0x060023E5 RID: 9189 RVA: 0x000A6E99 File Offset: 0x000A5299
		public string Name()
		{
			return "STEAM";
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x000A6EA0 File Offset: 0x000A52A0
		public bool PluginsReady()
		{
			return this.m_steamworksPluginsReady;
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x000A6EA8 File Offset: 0x000A52A8
		public IOnlineFriendsCoordinator OnlineFriendsCoordinator()
		{
			return null;
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x000A6EAB File Offset: 0x000A52AB
		public IOnlineMultiplayerNotificationCoordinator OnlineMultiplayerNotificationCoordinator()
		{
			return null;
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000A6EAE File Offset: 0x000A52AE
		public IOnlineAvatarImageCoordinator OnlineAvatarImageCoordinator()
		{
			return this.m_avatarImageCoordinator;
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x000A6EB6 File Offset: 0x000A52B6
		public IOnlineMultiplayerConnectionModeCoordinator OnlineMultiplayerConnectionModeCoordinator()
		{
			return null;
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x000A6EB9 File Offset: 0x000A52B9
		public IOnlineMultiplayerSessionPropertyCoordinator OnlineMultiplayerSessionPropertyCoordinator()
		{
			return this.m_multiplayerSessionPropertyCoordinator;
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x000A6EC1 File Offset: 0x000A52C1
		public IOnlineMultiplayerSessionCoordinator OnlineMultiplayerSessionCoordinator()
		{
			return this.m_multiplayerSessionCoordinator;
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x000A6EC9 File Offset: 0x000A52C9
		public IOnlineMultiplayerPrivilegeChecksCoordinator OnlineMultiplayerPrivilegeChecksCoordinator()
		{
			return this.m_multiplayerPrivilegeChecksCoordinator;
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x000A6ED1 File Offset: 0x000A52D1
		public IOnlineMultiplayerGameInviteCoordinator OnlineMultiplayerGameInviteCoordinator()
		{
			return this.m_multiplayerGameInviteCoordinator;
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x000A6ED9 File Offset: 0x000A52D9
		public IOnlineMultiplayerSessionEnumerateCoordinator OnlineMultiplayerSessionEnumerateCoordinator()
		{
			return this.m_multiplayerSessionEnumerateCoordinator;
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000A6EE1 File Offset: 0x000A52E1
		public IOnlineMultiplayerTransportStats OnlineMultiplayerTransportStats()
		{
			return this.m_transportStats;
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x000A6EE9 File Offset: 0x000A52E9
		protected void Awake()
		{
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000A6EEC File Offset: 0x000A52EC
		protected void Start()
		{
			if (SteamPlayerManager.Initialized)
			{
				if (Debug.isDebugBuild)
				{
					this.m_transportStats = new OnlineMultiplayerTransportStats();
				}
				this.m_multiplayerSessionPropertyCoordinator.Initialize();
				this.m_multiplayerGameInviteCoordinator.Initialize();
				this.m_multiplayerPrivilegeChecksCoordinator.Initialize();
				this.m_multiplayerSessionEnumerateCoordinator.Initialize(this.m_multiplayerSessionPropertyCoordinator);
				this.m_multiplayerSessionCoordinator.Initialize(this.m_multiplayerSessionPropertyCoordinator, this.m_transportStats);
				this.m_avatarImageCoordinator.Initialize();
				this.m_steamworksPluginsReady = true;
			}
			this.m_gameTimeAtStartOfFrame = Time.time;
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000A6F80 File Offset: 0x000A5380
		protected void Update()
		{
			this.m_gameTimeAtStartOfFrame = Time.time;
			if (this.m_steamworksPluginsReady)
			{
				this.m_multiplayerGameInviteCoordinator.Update();
				this.m_multiplayerPrivilegeChecksCoordinator.Update(this.m_gameTimeAtStartOfFrame);
				this.m_multiplayerSessionEnumerateCoordinator.Update(this.m_gameTimeAtStartOfFrame);
				this.m_multiplayerSessionCoordinator.Update(this.m_gameTimeAtStartOfFrame);
				this.m_avatarImageCoordinator.Update(this.m_gameTimeAtStartOfFrame);
				if (this.m_transportStats != null)
				{
					this.m_transportStats.Update(this.m_gameTimeAtStartOfFrame);
				}
			}
		}

		// Token: 0x04001B7A RID: 7034
		private bool m_steamworksPluginsReady;

		// Token: 0x04001B7B RID: 7035
		private float m_gameTimeAtStartOfFrame;

		// Token: 0x04001B7C RID: 7036
		private OnlineMultiplayerTransportStats m_transportStats;

		// Token: 0x04001B7D RID: 7037
		private SteamOnlineMultiplayerGameInviteCoordinator m_multiplayerGameInviteCoordinator = new SteamOnlineMultiplayerGameInviteCoordinator();

		// Token: 0x04001B7E RID: 7038
		private SteamOnlineMultiplayerPrivilegeChecksCoordinator m_multiplayerPrivilegeChecksCoordinator = new SteamOnlineMultiplayerPrivilegeChecksCoordinator();

		// Token: 0x04001B7F RID: 7039
		private OnlineMultiplayerSessionPropertyCoordinator m_multiplayerSessionPropertyCoordinator = new OnlineMultiplayerSessionPropertyCoordinator();

		// Token: 0x04001B80 RID: 7040
		private SteamOnlineMultiplayerSessionCoordinator m_multiplayerSessionCoordinator = new SteamOnlineMultiplayerSessionCoordinator();

		// Token: 0x04001B81 RID: 7041
		private SteamOnlineMultiplayerSessionEnumerateCoordinator m_multiplayerSessionEnumerateCoordinator = new SteamOnlineMultiplayerSessionEnumerateCoordinator();

		// Token: 0x04001B82 RID: 7042
		private SteamOnlineAvatarImageCoordinator m_avatarImageCoordinator = new SteamOnlineAvatarImageCoordinator();
	}
}
