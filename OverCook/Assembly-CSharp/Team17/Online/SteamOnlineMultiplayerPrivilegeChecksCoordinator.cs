using System;
using Steamworks;

namespace Team17.Online
{
	// Token: 0x0200097E RID: 2430
	public class SteamOnlineMultiplayerPrivilegeChecksCoordinator : IOnlineMultiplayerPrivilegeChecksCoordinator
	{
		// Token: 0x06002F5F RID: 12127 RVA: 0x000DD46E File Offset: 0x000DB86E
		public void Initialize()
		{
			if (!this.m_isInitialized)
			{
				this.m_isInitialized = true;
			}
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000DD482 File Offset: 0x000DB882
		public bool IsIdle()
		{
			return this.m_isInitialized && SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eIdle == this.m_status;
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000DD49A File Offset: 0x000DB89A
		public bool Start(GamepadUser localUser, OnlineMultiplayerPrivilegeCheckCallback callback)
		{
			if (this.m_isInitialized && this.m_status == SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eIdle && null != localUser && callback != null)
			{
				this.m_callback = callback;
				this.m_status = SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eStart;
				return true;
			}
			return false;
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000DD4D8 File Offset: 0x000DB8D8
		public void Cancel()
		{
			if (this.m_isInitialized)
			{
				SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status status = this.m_status;
				if (status != SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eIdle)
				{
					this.ResetInternalData();
				}
			}
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000DD514 File Offset: 0x000DB914
		public void Update(float gameTimeAtStartOfFrame)
		{
			if (this.m_isInitialized)
			{
				this.m_gameTimeAtStartOfFrame = gameTimeAtStartOfFrame;
				switch (this.m_status)
				{
				case SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eStart:
					this.LoggedOnTest();
					break;
				case SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eComplete:
					if (this.m_callback != null)
					{
						try
						{
							OnlineMultiplayerLocalUserId localOnlineUser = null;
							if (this.m_result == OnlineMultiplayerPrivilegeCheckResult.eSuccess)
							{
								CSteamID steamID = SteamUser.GetSteamID();
								localOnlineUser = new OnlineMultiplayerLocalUserId
								{
									m_userName = SteamFriends.GetPersonaName(),
									m_platformId = new OnlineUserPlatformId
									{
										m_steamId = steamID
									},
									m_steamId = steamID,
									m_steamUserRestrictions = SteamFriends.GetUserRestrictions()
								};
							}
							OnlineMultiplayerReturnCode<OnlineMultiplayerPrivilegeCheckResult> result = new OnlineMultiplayerReturnCode<OnlineMultiplayerPrivilegeCheckResult>
							{
								m_returnCode = this.m_result
							};
							this.m_callback(result, localOnlineUser);
						}
						catch (Exception)
						{
						}
					}
					this.ResetInternalData();
					break;
				}
			}
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000DD60C File Offset: 0x000DBA0C
		private void LoggedOnTest()
		{
			if (this.m_status == SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eStart)
			{
				try
				{
					if (SteamUser.BLoggedOn())
					{
						this.m_result = OnlineMultiplayerPrivilegeCheckResult.eSuccess;
						this.m_status = SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eComplete;
					}
					else
					{
						this.m_result = OnlineMultiplayerPrivilegeCheckResult.eNotSignedInToPlatform;
						this.m_status = SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eComplete;
					}
				}
				catch (Exception)
				{
					this.m_result = OnlineMultiplayerPrivilegeCheckResult.eNotSignedInToPlatform;
					this.m_status = SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eComplete;
				}
			}
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000DD67C File Offset: 0x000DBA7C
		private void ResetInternalData()
		{
			this.m_status = SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status.eIdle;
			this.m_callback = null;
			this.m_gameTimeCurrentRequestTimeout = 0f;
			this.m_result = OnlineMultiplayerPrivilegeCheckResult.eGenericFailure;
		}

		// Token: 0x040025DC RID: 9692
		private readonly float m_waitForNetworkLinkMaxTimeInSeconds = 10f;

		// Token: 0x040025DD RID: 9693
		private bool m_isInitialized;

		// Token: 0x040025DE RID: 9694
		private SteamOnlineMultiplayerPrivilegeChecksCoordinator.Status m_status;

		// Token: 0x040025DF RID: 9695
		private OnlineMultiplayerPrivilegeCheckCallback m_callback;

		// Token: 0x040025E0 RID: 9696
		private float m_gameTimeAtStartOfFrame;

		// Token: 0x040025E1 RID: 9697
		private float m_gameTimeCurrentRequestTimeout;

		// Token: 0x040025E2 RID: 9698
		private OnlineMultiplayerPrivilegeCheckResult m_result = OnlineMultiplayerPrivilegeCheckResult.eGenericFailure;

		// Token: 0x0200097F RID: 2431
		private enum Status : byte
		{
			// Token: 0x040025E4 RID: 9700
			eIdle,
			// Token: 0x040025E5 RID: 9701
			eStart,
			// Token: 0x040025E6 RID: 9702
			eComplete
		}
	}
}
