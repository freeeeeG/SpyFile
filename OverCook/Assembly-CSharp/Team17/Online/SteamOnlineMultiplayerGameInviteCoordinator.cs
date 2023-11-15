using System;
using Steamworks;

namespace Team17.Online
{
	// Token: 0x0200097A RID: 2426
	public class SteamOnlineMultiplayerGameInviteCoordinator : IOnlineMultiplayerGameInviteCoordinator
	{
		// Token: 0x06002F4F RID: 12111 RVA: 0x000DD25B File Offset: 0x000DB65B
		public void Initialize()
		{
			if (!this.m_isInitialized)
			{
				SteamOnlineMultiplayerGameInviteCoordinator.s_LobbyJoinRequestedCallback = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(this.OnLobbyJoinRequested));
				this.m_isInitialized = true;
				this.CheckForBootInvite();
			}
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x000DD28C File Offset: 0x000DB68C
		public OnlineMultiplayerSessionInvite InviteAccepted()
		{
			OnlineMultiplayerSessionInvite acceptedInvite = this.m_acceptedInvite;
			this.m_acceptedInvite = null;
			return acceptedInvite;
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x000DD2A8 File Offset: 0x000DB6A8
		public bool HasPendingAcceptedInvite()
		{
			return this.m_acceptedInvite != null;
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x000DD2B6 File Offset: 0x000DB6B6
		public OnlineMultiplayerSessionPlayTogetherHosting PlayTogetherHosting()
		{
			return null;
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x000DD2B9 File Offset: 0x000DB6B9
		public void Update()
		{
			if (this.m_isInitialized)
			{
			}
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x000DD2C8 File Offset: 0x000DB6C8
		private void OnLobbyJoinRequested(GameLobbyJoinRequested_t pCallback)
		{
			this.m_acceptedInvite = new OnlineMultiplayerSessionInvite
			{
				m_steamLobbyId = pCallback.m_steamIDLobby
			};
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x000DD2F0 File Offset: 0x000DB6F0
		private void CheckForBootInvite()
		{
			try
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				if (commandLineArgs != null && commandLineArgs.Length >= 2)
				{
					for (int i = 0; i < commandLineArgs.Length; i++)
					{
						if (commandLineArgs[i] == "+connect_lobby")
						{
							if (i + 1 < commandLineArgs.Length && !string.IsNullOrEmpty(commandLineArgs[i + 1]))
							{
								ulong ulSteamID = 0UL;
								if (ulong.TryParse(commandLineArgs[i + 1], out ulSteamID))
								{
									CSteamID steamLobbyId = new CSteamID(ulSteamID);
									if (steamLobbyId.IsValid() && steamLobbyId.IsLobby())
									{
										this.m_acceptedInvite = new OnlineMultiplayerSessionInvite
										{
											m_steamLobbyId = steamLobbyId
										};
									}
								}
							}
							break;
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x040025D2 RID: 9682
		private static Callback<GameLobbyJoinRequested_t> s_LobbyJoinRequestedCallback;

		// Token: 0x040025D3 RID: 9683
		private bool m_isInitialized;

		// Token: 0x040025D4 RID: 9684
		private OnlineMultiplayerSessionInvite m_acceptedInvite;
	}
}
