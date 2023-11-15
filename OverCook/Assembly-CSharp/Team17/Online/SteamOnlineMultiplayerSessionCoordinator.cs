using System;
using System.Collections;
using System.Collections.Generic;
using BitStream;
using Steamworks;
using Team17.Online.Shared;

namespace Team17.Online
{
	// Token: 0x02000981 RID: 2433
	public class SteamOnlineMultiplayerSessionCoordinator : IOnlineMultiplayerSessionCoordinator
	{
		// Token: 0x06002F69 RID: 12137 RVA: 0x000DD810 File Offset: 0x000DBC10
		public void Initialize(OnlineMultiplayerSessionPropertyCoordinator sessionPropertyCoordinator, OnlineMultiplayerTransportStats transportStats)
		{
			if (!this.m_isInitialized && sessionPropertyCoordinator != null)
			{
				try
				{
					if (SteamPlayerManager.Initialized)
					{
						this.m_transportCoordinator.Initialize();
						SteamOnlineMultiplayerSessionCoordinator.s_steamLobbyCreatedCallback = Callback<LobbyCreated_t>.Create(new Callback<LobbyCreated_t>.DispatchDelegate(this.OnSteamLobbyCreated));
						SteamOnlineMultiplayerSessionCoordinator.s_steamLobbyJoinedCallback = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(this.OnSteamLobbyJoined));
						SteamOnlineMultiplayerSessionCoordinator.s_steamLobbyMembersChangedCallback = Callback<LobbyChatUpdate_t>.Create(new Callback<LobbyChatUpdate_t>.DispatchDelegate(this.OnSteamLobbyMembersChanged));
						this.m_transportStats = transportStats;
						this.m_sessionPropertyCoordinator = sessionPropertyCoordinator;
						this.m_transportBitStreamWriter = new BitStreamWriter(this.m_transportSendList);
						this.m_transportBitStreamReader = new BitStreamReader(this.m_transportReceiveBuffer);
						this.m_isInitialized = true;
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x000DD8D8 File Offset: 0x000DBCD8
		public void RegisterDisconnectionCallback(OnlineMultiplayerSessionDisconnectionCallback callback)
		{
			if (callback != null)
			{
				this.m_disconnectionCallbacks = (OnlineMultiplayerSessionDisconnectionCallback)Delegate.Combine(this.m_disconnectionCallbacks, callback);
			}
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000DD8F7 File Offset: 0x000DBCF7
		public void UnRegisterDisconnectionCallback(OnlineMultiplayerSessionDisconnectionCallback callback)
		{
			if (callback != null)
			{
				this.m_disconnectionCallbacks = (OnlineMultiplayerSessionDisconnectionCallback)Delegate.Remove(this.m_disconnectionCallbacks, callback);
			}
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000DD916 File Offset: 0x000DBD16
		public void RegisterRemoteUserDisconnectionCallback(OnlineMultiplayerSessionRemoteUserDisconnectionCallback callback)
		{
			if (callback != null)
			{
				this.m_remoteUserDisconnectionCallbacks = (OnlineMultiplayerSessionRemoteUserDisconnectionCallback)Delegate.Combine(this.m_remoteUserDisconnectionCallbacks, callback);
			}
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000DD935 File Offset: 0x000DBD35
		public void UnRegisterRemoteUserDisconnectionCallback(OnlineMultiplayerSessionRemoteUserDisconnectionCallback callback)
		{
			if (callback != null)
			{
				this.m_remoteUserDisconnectionCallbacks = (OnlineMultiplayerSessionRemoteUserDisconnectionCallback)Delegate.Remove(this.m_remoteUserDisconnectionCallbacks, callback);
			}
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000DD954 File Offset: 0x000DBD54
		public void RegisterDataReceivedCallback(OnlineMultiplayerSessionDataReceivedCallback callback)
		{
			if (callback != null)
			{
				this.m_dataReceivedCallback = (OnlineMultiplayerSessionDataReceivedCallback)Delegate.Combine(this.m_dataReceivedCallback, callback);
			}
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000DD973 File Offset: 0x000DBD73
		public void UnRegisterDataReceivedCallback(OnlineMultiplayerSessionDataReceivedCallback callback)
		{
			if (callback != null)
			{
				this.m_dataReceivedCallback = (OnlineMultiplayerSessionDataReceivedCallback)Delegate.Remove(this.m_dataReceivedCallback, callback);
			}
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000DD994 File Offset: 0x000DBD94
		public void Update(float gameTimeAtStartOfFrame)
		{
			if (this.m_isInitialized)
			{
				this.m_gameTimeAtStartOfFrame = gameTimeAtStartOfFrame;
				switch (this.m_sessionStatus)
				{
				case SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eCreating:
					this.UpdateCreatingLobby();
					break;
				case SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning:
					this.UpdateRunning();
					break;
				case SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eJoining:
					this.UpdateJoiningLobby();
					break;
				case SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eConnectingToHost:
					this.UpdateConnectingToHost();
					break;
				}
				if (this.m_leaveRequested)
				{
					if (this.m_lobbyId.IsValid())
					{
						try
						{
							SteamMatchmaking.LeaveLobby(this.m_lobbyId);
						}
						catch (Exception)
						{
						}
					}
					this.ResetToIdle();
				}
				this.m_transportCoordinator.Update(this.m_transportReceiveBuffer, OnlineMultiplayerConfig.MaxSocketIterationsPerUpdate);
			}
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000DDA6C File Offset: 0x000DBE6C
		public bool IsIdle()
		{
			return this.m_isInitialized && SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eIdle == this.m_sessionStatus;
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000DDA85 File Offset: 0x000DBE85
		public bool IsHost()
		{
			return this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning && this.m_hostSteamId == this.m_localPlayerUserId.m_steamId;
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000DDAB0 File Offset: 0x000DBEB0
		public bool Create(OnlineMultiplayerLocalUserId localUserId, List<OnlineMultiplayerSessionPropertyValue> sessionProperties, OnlineMultiplayerSessionVisibility visibility, string sessionName, OnlineMultiplayerSessionPlayTogetherHosting playtogetherHosting, OnlineMultiplayerSessionCreateCallback createCallback, OnlineMultiplayerSessionJoinDecisionCallback joinDecisionCallback, OnlineMultiplayerSessionUserJoinedCallback newUserJoinedCallback)
		{
			if (this.IsIdle())
			{
				if (this.m_sessionPropertyCoordinator.IsInitialized() && localUserId != null && sessionProperties != null && createCallback != null && joinDecisionCallback != null && newUserJoinedCallback != null)
				{
					bool flag = this.ValidateSessionProperties(sessionProperties);
					if (flag)
					{
						try
						{
							if (SteamUser.BLoggedOn())
							{
								this.m_localPlayerUserId = localUserId;
								this.m_creatingPropertyValues = sessionProperties;
								this.m_sessionVisibility = visibility;
								this.m_createSessionCallback = createCallback;
								this.m_joinDecisionCallback = joinDecisionCallback;
								this.m_newUserJoinedCallback = newUserJoinedCallback;
								this.m_entryDelayMaxGameTime = this.m_gameTimeAtStartOfFrame + this.m_entryDelayInSeconds;
								this.m_sessionStatus = SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eCreating;
								this.m_creatingSubStatus = SteamOnlineMultiplayerSessionCoordinator.CreatingSubStatus.eInitialDelay;
								return true;
							}
						}
						catch (Exception ex)
						{
						}
					}
				}
				this.ResetToIdle();
			}
			return false;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000DDB8C File Offset: 0x000DBF8C
		public bool Join(List<OnlineMultiplayerSessionJoinLocalUserData> localUserData, OnlineMultiplayerSessionInvite sessionInvite, OnlineMultiplayerSessionJoinCallback joinCallback)
		{
			if (this.IsIdle())
			{
				if (this.ValidateJoinLocalUserData(localUserData) && sessionInvite != null && joinCallback != null)
				{
					try
					{
						if (sessionInvite.m_steamLobbyId.IsValid() && sessionInvite.m_steamLobbyId.IsLobby() && SteamUser.BLoggedOn())
						{
							this.m_joinSessionCallback = joinCallback;
							for (int i = 0; i < localUserData.Count; i++)
							{
								OnlineMultiplayerSessionJoinLocalUserData onlineMultiplayerSessionJoinLocalUserData = localUserData[i];
								this.m_localUserJoinData.Add(onlineMultiplayerSessionJoinLocalUserData);
								if (i == 0)
								{
									this.m_localPlayerUserId = onlineMultiplayerSessionJoinLocalUserData.Id;
								}
								else
								{
									this.m_secondaryLocalUserIds.Add(onlineMultiplayerSessionJoinLocalUserData.Id);
								}
							}
							this.m_lobbyIdToJoin = sessionInvite.m_steamLobbyId;
							this.m_entryDelayMaxGameTime = this.m_gameTimeAtStartOfFrame + this.m_entryDelayInSeconds;
							this.m_sessionStatus = SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eJoining;
							this.m_joiningSubStatus = SteamOnlineMultiplayerSessionCoordinator.JoiningSubStatus.eInitialDelay;
							return true;
						}
					}
					catch (Exception ex)
					{
					}
				}
				this.ResetToIdle();
			}
			return false;
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000DDC98 File Offset: 0x000DC098
		public bool Join(List<OnlineMultiplayerSessionJoinLocalUserData> localUserData, OnlineMultiplayerSessionEnumeratedRoom enumeratedSession, OnlineMultiplayerSessionJoinCallback joinCallback)
		{
			if (this.IsIdle())
			{
				if (this.ValidateJoinLocalUserData(localUserData) && enumeratedSession != null && joinCallback != null)
				{
					try
					{
						if (enumeratedSession.m_steamLobbyId.IsValid() && enumeratedSession.m_steamLobbyId.IsLobby() && SteamUser.BLoggedOn())
						{
							this.m_joinSessionCallback = joinCallback;
							for (int i = 0; i < localUserData.Count; i++)
							{
								OnlineMultiplayerSessionJoinLocalUserData onlineMultiplayerSessionJoinLocalUserData = localUserData[i];
								this.m_localUserJoinData.Add(onlineMultiplayerSessionJoinLocalUserData);
								if (i == 0)
								{
									this.m_localPlayerUserId = onlineMultiplayerSessionJoinLocalUserData.Id;
								}
								else
								{
									this.m_secondaryLocalUserIds.Add(onlineMultiplayerSessionJoinLocalUserData.Id);
								}
							}
							this.m_lobbyIdToJoin = enumeratedSession.m_steamLobbyId;
							this.m_entryDelayMaxGameTime = this.m_gameTimeAtStartOfFrame + this.m_entryDelayInSeconds;
							this.m_sessionStatus = SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eJoining;
							this.m_joiningSubStatus = SteamOnlineMultiplayerSessionCoordinator.JoiningSubStatus.eInitialDelay;
							return true;
						}
					}
					catch (Exception ex)
					{
					}
				}
				this.ResetToIdle();
			}
			return false;
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000DDDA4 File Offset: 0x000DC1A4
		public OnlineMultiplayerNonPrimaryLocalUserChangeResult AddNonPrimaryLocalUser(OnlineMultiplayerLocalUserId localUserId, OnlineMultiplayerSessionAddNonPrimaryLocalUserCallback joinCallback)
		{
			OnlineMultiplayerNonPrimaryLocalUserChangeResult result = OnlineMultiplayerNonPrimaryLocalUserChangeResult.eNotPossible;
			try
			{
				if (localUserId != null && joinCallback != null && this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning && !this.m_leaveRequested && this.IsHost() && !this.m_secondaryLocalUserIds.Exists((OnlineMultiplayerLocalUserId x) => x == localUserId) && (long)(this.m_userIdList.Count + this.m_secondaryLocalUserIds.Count) < (long)((ulong)OnlineMultiplayerConfig.MaxPlayers))
				{
					this.m_secondaryLocalUserIds.Add(localUserId);
					result = OnlineMultiplayerNonPrimaryLocalUserChangeResult.eComplete;
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000DDE64 File Offset: 0x000DC264
		public OnlineMultiplayerNonPrimaryLocalUserChangeResult RemoveNonPrimaryLocalUser(OnlineMultiplayerLocalUserId localUserId)
		{
			OnlineMultiplayerNonPrimaryLocalUserChangeResult result = OnlineMultiplayerNonPrimaryLocalUserChangeResult.eNotPossible;
			try
			{
				if (localUserId != null && this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning && !this.m_leaveRequested && this.IsHost() && this.m_secondaryLocalUserIds.Exists((OnlineMultiplayerLocalUserId x) => x == localUserId))
				{
					this.m_secondaryLocalUserIds.Remove(localUserId);
					result = OnlineMultiplayerNonPrimaryLocalUserChangeResult.eComplete;
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000DDEFC File Offset: 0x000DC2FC
		public bool Modify(List<OnlineMultiplayerSessionPropertyValue> sessionProperties, OnlineMultiplayerSessionVisibility visibility)
		{
			if (this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning && !this.m_leaveRequested && this.IsHost())
			{
				try
				{
					if (sessionProperties != null && sessionProperties.Count > 0)
					{
						for (int i = 0; i < sessionProperties.Count; i++)
						{
							string name = sessionProperties[i].m_property.Name;
							string pchValue = sessionProperties[i].m_value.ToString();
							SteamMatchmaking.SetLobbyData(this.m_lobbyId, name, pchValue);
						}
					}
					if (visibility != this.m_sessionVisibility || this.m_forceModify)
					{
						this.m_sessionVisibility = visibility;
						SteamMatchmaking.SetLobbyJoinable(this.m_lobbyId, this.m_sessionVisibility != OnlineMultiplayerSessionVisibility.eClosed);
						ELobbyType eLobbyType = ELobbyType.k_ELobbyTypePrivate;
						OnlineMultiplayerSessionVisibility sessionVisibility = this.m_sessionVisibility;
						if (sessionVisibility == OnlineMultiplayerSessionVisibility.ePublic || sessionVisibility == OnlineMultiplayerSessionVisibility.eMatchmaking)
						{
							eLobbyType = ELobbyType.k_ELobbyTypePublic;
						}
						SteamMatchmaking.SetLobbyType(this.m_lobbyId, eLobbyType);
					}
					return true;
				}
				catch (Exception ex)
				{
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000DE01C File Offset: 0x000DC41C
		public bool AutoMatchmake(OnlineMultiplayerSessionJoinLocalUserData localUserData, List<OnlineMultiplayerSessionPropertyValue> hostingSessionProperties, string hostingSessionName, OnlineMultiplayerSessionJoinDecisionCallback hostingJoinDecisionCallback, OnlineMultiplayerSessionUserJoinedCallback hostingNewUserJoinedCallback, List<OnlineMultiplayerSessionPropertySearchValue> autoMatchingFilterParameters, OnlineMultiplayerSessionJoinCallback joinCallback)
		{
			return false;
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x000DE020 File Offset: 0x000DC420
		public bool SendData(IOnlineMultiplayerSessionUserId recipientUserId, byte[] data, int dataSize, bool sendReliably)
		{
			if (this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning && !this.m_leaveRequested && recipientUserId != null && data != null && dataSize > 0 && dataSize <= data.Length && (long)dataSize <= (long)((ulong)OnlineMultiplayerConfig.MaxTransportMessageSize))
			{
				if (!recipientUserId.IsLocal)
				{
					OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = recipientUserId as OnlineMultiplayerSessionUserId;
					if (onlineMultiplayerSessionUserId != null && onlineMultiplayerSessionUserId.HasDirectTransportConnection() && this.m_transportCoordinator.SendData(onlineMultiplayerSessionUserId.m_steamId, data, dataSize, sendReliably))
					{
						onlineMultiplayerSessionUserId.m_steamLocalKeepaliveLastSendTime = this.m_gameTimeAtStartOfFrame;
						if (this.m_transportStats != null)
						{
							this.m_transportStats.Add(OnlineMultiplayerTransportStats.StatType.eDataSent, (uint)dataSize);
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000DE0D8 File Offset: 0x000DC4D8
		public void Leave()
		{
			if (this.m_isInitialized && this.m_isInitialized)
			{
				SteamOnlineMultiplayerSessionCoordinator.SessionStatus sessionStatus = this.m_sessionStatus;
				if (sessionStatus != SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eIdle)
				{
					if (!this.m_leaveRequested)
					{
						this.m_leaveRequested = true;
						for (int i = 0; i < this.m_userIdList.Count; i++)
						{
							OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = this.m_userIdList[i];
							if (!onlineMultiplayerSessionUserId.IsLocal)
							{
								this.m_transportCoordinator.CloseConnection(onlineMultiplayerSessionUserId.m_steamId);
								onlineMultiplayerSessionUserId.m_steamLocalTransportConnectionStatus = SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eConnectionDead;
							}
						}
						if (this.m_hostSteamIdDuringConnection.IsValid())
						{
							this.m_transportCoordinator.CloseConnection(this.m_hostSteamIdDuringConnection);
							this.m_hostSteamIdDuringConnection.Clear();
							this.m_connectingToHostTransportStatus = SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eConnectionDead;
							this.m_connectingToHostMaxGameTime = 0f;
						}
						this.m_transportCoordinator.Close();
					}
				}
			}
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000DE1C0 File Offset: 0x000DC5C0
		public void ShowSendInviteDialog(string msg)
		{
			if (this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning && !this.m_leaveRequested && this.m_lobbyId.IsValid())
			{
				try
				{
					SteamFriends.ActivateGameOverlayInviteDialog(this.m_lobbyId);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x000DE21C File Offset: 0x000DC61C
		public IOnlineMultiplayerSessionUserId[] Members()
		{
			if (this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning && !this.m_leaveRequested && this.m_userIdList.Count > 0)
			{
				return this.m_userIdList.ToArray();
			}
			return null;
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x000DE254 File Offset: 0x000DC654
		public bool IsMemberAlready(OnlineMultiplayerSessionInvite pendingSessionInvite)
		{
			if (this.m_isInitialized && pendingSessionInvite != null)
			{
				SteamOnlineMultiplayerSessionCoordinator.SessionStatus sessionStatus = this.m_sessionStatus;
				if (sessionStatus != SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eIdle)
				{
					if (!this.m_leaveRequested && this.m_lobbyId.IsValid() && this.m_lobbyId == pendingSessionInvite.m_steamLobbyId)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x000DE2C4 File Offset: 0x000DC6C4
		private void ResetToIdle()
		{
			this.m_localUserJoinData.Clear();
			this.m_remoteUserJoinData.Clear();
			this.m_secondaryLocalUserIds.Clear();
			this.m_sessionStatus = SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eIdle;
			this.m_creatingSubStatus = SteamOnlineMultiplayerSessionCoordinator.CreatingSubStatus.eIdle;
			this.m_joiningSubStatus = SteamOnlineMultiplayerSessionCoordinator.JoiningSubStatus.eIdle;
			this.m_joiningLobbyResponse = EChatRoomEnterResponse.k_EChatRoomEnterResponseError;
			this.m_creatingPropertyValues = null;
			this.m_entryDelayMaxGameTime = 0f;
			this.m_localPlayerUserId = null;
			this.m_leaveRequested = false;
			this.m_forceModify = false;
			this.m_userIdList.Clear();
			this.m_uniqueUserIdCounter = OnlineMultiplayerSessionUserId.c_InvalidUniqueId;
			this.m_sessionVisibility = OnlineMultiplayerSessionVisibility.eClosed;
			this.m_createSessionCallback = null;
			this.m_joinDecisionCallback = null;
			this.m_newUserJoinedCallback = null;
			this.m_joinSessionCallback = null;
			this.m_connectingToHostMaxGameTime = 0f;
			this.m_connectingToHostTransportStatus = SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eNotApplicable;
			this.m_hostSteamId.Clear();
			this.m_hostSteamIdDuringConnection.Clear();
			this.m_lobbyId.Clear();
			this.m_lobbyIdToJoin.Clear();
			this.m_transportCoordinator.Close();
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x000DE3B8 File Offset: 0x000DC7B8
		private byte GenerateUniqueUserId()
		{
			byte id = OnlineMultiplayerSessionUserId.c_InvalidUniqueId;
			bool flag;
			do
			{
				flag = false;
				id = (this.m_uniqueUserIdCounter += 1);
				if (OnlineMultiplayerSessionUserId.c_InvalidUniqueId != id)
				{
					flag = !this.m_userIdList.Exists((OnlineMultiplayerSessionUserId x) => x.m_uniqueId == id);
				}
			}
			while (!flag);
			return id;
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x000DE42C File Offset: 0x000DC82C
		private bool ValidateSessionProperties(List<OnlineMultiplayerSessionPropertyValue> sessionProperties)
		{
			IEnumerator enumerator = Enum.GetValues(typeof(OnlineMultiplayerSessionPropertyId)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					OnlineMultiplayerSessionPropertyId entry = (OnlineMultiplayerSessionPropertyId)enumerator.Current;
					if (!sessionProperties.Exists((OnlineMultiplayerSessionPropertyValue x) => x.m_property.Id == (uint)entry))
					{
						return false;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return true;
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000DE4C0 File Offset: 0x000DC8C0
		private bool ValidateJoinLocalUserData(List<OnlineMultiplayerSessionJoinLocalUserData> localUserData)
		{
			bool flag = true;
			if (localUserData != null && localUserData.Count > 0 && (long)localUserData.Count < (long)((ulong)OnlineMultiplayerConfig.MaxPlayers))
			{
				int num = 0;
				while (num < localUserData.Count && flag)
				{
					flag &= (null != localUserData[num].Id);
					flag &= (null != localUserData[num].GameData);
					flag &= (0U != localUserData[num].GameDataSize);
					flag &= ((ulong)localUserData[num].GameDataSize <= (ulong)((long)localUserData[num].GameData.Length));
					num++;
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000DE57C File Offset: 0x000DC97C
		private void UpdateCreatingLobby()
		{
			switch (this.m_creatingSubStatus)
			{
			case SteamOnlineMultiplayerSessionCoordinator.CreatingSubStatus.eInitialDelay:
				if (this.m_gameTimeAtStartOfFrame >= this.m_entryDelayMaxGameTime)
				{
					if (!this.m_leaveRequested)
					{
						if (SteamUser.BLoggedOn())
						{
							try
							{
								if (SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePrivate, (int)OnlineMultiplayerConfig.MaxPlayers).m_SteamAPICall != 0UL)
								{
									this.m_entryDelayMaxGameTime = 0f;
									this.m_creatingSubStatus = SteamOnlineMultiplayerSessionCoordinator.CreatingSubStatus.eWaitingForSteamToCreateTheLobby;
								}
								else
								{
									this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGeneric);
								}
							}
							catch (Exception)
							{
								this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGeneric);
							}
						}
						else
						{
							this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGoneOffline);
						}
					}
					else
					{
						this.ResetToIdle();
					}
				}
				break;
			case SteamOnlineMultiplayerSessionCoordinator.CreatingSubStatus.eComplete:
				if (this.m_lobbyId.IsValid())
				{
					this.m_creatingSubStatus = SteamOnlineMultiplayerSessionCoordinator.CreatingSubStatus.eIdle;
					if (!this.m_leaveRequested)
					{
						if (this.ValidateSessionProperties(this.m_creatingPropertyValues))
						{
							this.m_hostSteamId = this.m_localPlayerUserId.m_steamId;
							this.m_sessionStatus = SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning;
							OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = new OnlineMultiplayerSessionUserId();
							onlineMultiplayerSessionUserId.m_isHost = true;
							onlineMultiplayerSessionUserId.m_isLocal = true;
							onlineMultiplayerSessionUserId.m_uniqueId = this.GenerateUniqueUserId();
							onlineMultiplayerSessionUserId.m_displayName = this.m_localPlayerUserId.m_userName;
							onlineMultiplayerSessionUserId.m_steamId = this.m_localPlayerUserId.m_steamId;
							onlineMultiplayerSessionUserId.m_steamUserRestrictions = this.m_localPlayerUserId.m_steamUserRestrictions;
							this.m_userIdList.Add(onlineMultiplayerSessionUserId);
							this.m_forceModify = true;
							if (this.Modify(this.m_creatingPropertyValues, this.m_sessionVisibility))
							{
								this.m_forceModify = false;
								this.m_creatingPropertyValues = null;
								this.m_transportCoordinator.Open(this.m_lobbyId, new SteamOnlineMultiplayerSessionTransportCoordinator.DisconnectionCallback(this.OnTransportDisconnectionCallback), new SteamOnlineMultiplayerSessionTransportCoordinator.DataCallback(this.OnTransportDataCallback), new SteamOnlineMultiplayerSessionTransportCoordinator.DataCallback(this.OnTransportVoipDataCallback));
								try
								{
									OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult> result = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult>
									{
										m_returnCode = OnlineMultiplayerSessionCreateResult.eSuccess
									};
									this.m_createSessionCallback(result);
								}
								catch (Exception ex)
								{
								}
							}
							else
							{
								this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGeneric);
							}
						}
						else
						{
							this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGeneric);
						}
					}
					else
					{
						this.Leave();
					}
				}
				else
				{
					this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGeneric);
				}
				break;
			}
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000DE7C8 File Offset: 0x000DCBC8
		private void UpdateJoiningLobby()
		{
			switch (this.m_joiningSubStatus)
			{
			case SteamOnlineMultiplayerSessionCoordinator.JoiningSubStatus.eInitialDelay:
				if (this.m_gameTimeAtStartOfFrame >= this.m_entryDelayMaxGameTime)
				{
					if (!this.m_leaveRequested)
					{
						if (SteamUser.BLoggedOn())
						{
							try
							{
								if (SteamMatchmaking.JoinLobby(this.m_lobbyIdToJoin).m_SteamAPICall != 0UL)
								{
									this.m_entryDelayMaxGameTime = 0f;
									this.m_joiningSubStatus = SteamOnlineMultiplayerSessionCoordinator.JoiningSubStatus.eWaitingForSteamToJoinTheLobby;
								}
								else
								{
									this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGeneric);
								}
							}
							catch (Exception)
							{
								this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGeneric);
							}
						}
						else
						{
							this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGoneOffline);
						}
					}
					else
					{
						this.ResetToIdle();
					}
				}
				break;
			case SteamOnlineMultiplayerSessionCoordinator.JoiningSubStatus.eCompleteComplete:
				if (!this.m_leaveRequested)
				{
					this.m_joiningSubStatus = SteamOnlineMultiplayerSessionCoordinator.JoiningSubStatus.eIdle;
					if (this.m_lobbyId.IsValid())
					{
						OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = new OnlineMultiplayerSessionUserId();
						onlineMultiplayerSessionUserId.m_isHost = false;
						onlineMultiplayerSessionUserId.m_isLocal = true;
						onlineMultiplayerSessionUserId.m_uniqueId = OnlineMultiplayerSessionUserId.c_InvalidUniqueId;
						onlineMultiplayerSessionUserId.m_displayName = this.m_localPlayerUserId.m_userName;
						onlineMultiplayerSessionUserId.m_steamId = this.m_localPlayerUserId.m_steamId;
						onlineMultiplayerSessionUserId.m_steamUserRestrictions = this.m_localPlayerUserId.m_steamUserRestrictions;
						this.m_userIdList.Add(onlineMultiplayerSessionUserId);
						try
						{
							CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(this.m_lobbyId);
							if (lobbyOwner != onlineMultiplayerSessionUserId.m_steamId)
							{
								this.m_hostSteamIdDuringConnection = lobbyOwner;
								this.m_hostSteamId = lobbyOwner;
							}
						}
						catch (Exception)
						{
						}
						if (this.m_hostSteamIdDuringConnection.IsValid() && this.m_transportCoordinator.Open(this.m_lobbyId, new SteamOnlineMultiplayerSessionTransportCoordinator.DisconnectionCallback(this.OnTransportDisconnectionCallback), new SteamOnlineMultiplayerSessionTransportCoordinator.DataCallback(this.OnTransportDataCallback), new SteamOnlineMultiplayerSessionTransportCoordinator.DataCallback(this.OnTransportVoipDataCallback)))
						{
							try
							{
								this.SetupOutgoingMessage(SteamOnlineMultiplayerSessionCoordinator.TransportMessageTypes.eJoinRequest);
								this.m_transportBitStreamWriter.Write(OnlineMultiplayerConfig.CodeVersion, 32);
								onlineMultiplayerSessionUserId.Serialize(this.m_transportBitStreamWriter);
								if (OnlineMultiplayerSessionJoinUserDataHelper.Serialize(this.m_localUserJoinData, this.m_transportBitStreamWriter))
								{
									this.m_localUserJoinData.Clear();
									if (this.m_transportCoordinator.SendData(this.m_hostSteamIdDuringConnection, this.m_transportSendList._items, this.m_transportSendList.Count, true))
									{
										this.m_sessionStatus = SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eConnectingToHost;
										this.m_connectingToHostMaxGameTime = this.m_gameTimeAtStartOfFrame + this.m_connectToHostMaxTimeInSeconds;
										this.m_connectingToHostTransportStatus = SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eWaitingForJoinApprovalFromHost;
										break;
									}
								}
							}
							catch (Exception)
							{
							}
						}
						this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eHostDisconnected);
					}
					else
					{
						this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGeneric);
					}
				}
				break;
			}
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000DEA70 File Offset: 0x000DCE70
		private void UpdateConnectingToHost()
		{
			if (!this.m_leaveRequested && this.m_gameTimeAtStartOfFrame > this.m_connectingToHostMaxGameTime)
			{
				this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eHostDisconnected);
			}
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x000DEA98 File Offset: 0x000DCE98
		private void UpdateRunning()
		{
			if (!this.m_leaveRequested)
			{
				if (SteamUser.BLoggedOn())
				{
					for (int i = 0; i < this.m_userIdList.Count; i++)
					{
						OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = this.m_userIdList[i];
						if (!onlineMultiplayerSessionUserId.IsLocal && onlineMultiplayerSessionUserId.HasDirectTransportConnection())
						{
							float num = this.m_gameTimeAtStartOfFrame - onlineMultiplayerSessionUserId.m_steamLocalKeepaliveLastSendTime;
							if (num > this.m_keepaliveMessageFrequencyInSeconds)
							{
								this.SendKeepaliveMessage(onlineMultiplayerSessionUserId);
							}
							float num2 = this.m_gameTimeAtStartOfFrame - onlineMultiplayerSessionUserId.m_steamLocalKeepaliveLastReceiveTime;
							if (num2 > SteamOnlineMultiplayerSessionTransportCoordinator.DisconnectionTimeInSeconds * 1f)
							{
								this.OnUserDisconnected(onlineMultiplayerSessionUserId);
								return;
							}
						}
					}
				}
				else
				{
					this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eGoneOffline);
				}
			}
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x000DEB50 File Offset: 0x000DCF50
		private void OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult disconnectionReason)
		{
			if (this.m_isInitialized)
			{
				switch (this.m_sessionStatus)
				{
				case SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eCreating:
					if (!this.m_leaveRequested)
					{
						try
						{
							OnlineMultiplayerSessionCreateResult returnCode = OnlineMultiplayerSessionCreateResult.eGenericFailure;
							switch (disconnectionReason)
							{
							case OnlineMultiplayerSessionDisconnectionResult.eLostNetwork:
								returnCode = OnlineMultiplayerSessionCreateResult.eLostNetwork;
								break;
							case OnlineMultiplayerSessionDisconnectionResult.eGoneOffline:
								returnCode = OnlineMultiplayerSessionCreateResult.eGoneOffline;
								break;
							case OnlineMultiplayerSessionDisconnectionResult.eLoggedOut:
								returnCode = OnlineMultiplayerSessionCreateResult.eLoggedOut;
								break;
							}
							OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult> result = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult>
							{
								m_returnCode = returnCode
							};
							this.m_createSessionCallback(result);
						}
						catch (Exception ex)
						{
						}
						this.Leave();
					}
					break;
				case SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning:
					if (!this.m_leaveRequested)
					{
						try
						{
							OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> result2 = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>
							{
								m_returnCode = disconnectionReason
							};
							this.m_disconnectionCallbacks(result2);
						}
						catch (Exception ex2)
						{
						}
						this.Leave();
					}
					break;
				case SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eJoining:
				case SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eConnectingToHost:
					if (!this.m_leaveRequested)
					{
						try
						{
							OnlineMultiplayerSessionJoinResult returnCode2 = OnlineMultiplayerSessionJoinResult.eGenericFailure;
							switch (disconnectionReason)
							{
							case OnlineMultiplayerSessionDisconnectionResult.eGeneric:
							{
								EChatRoomEnterResponse joiningLobbyResponse = this.m_joiningLobbyResponse;
								if (joiningLobbyResponse != EChatRoomEnterResponse.k_EChatRoomEnterResponseFull)
								{
									if (joiningLobbyResponse == EChatRoomEnterResponse.k_EChatRoomEnterResponseDoesntExist)
									{
										returnCode2 = OnlineMultiplayerSessionJoinResult.eNoLongerExists;
									}
								}
								else
								{
									returnCode2 = OnlineMultiplayerSessionJoinResult.eFull;
								}
								break;
							}
							case OnlineMultiplayerSessionDisconnectionResult.eLostNetwork:
								returnCode2 = OnlineMultiplayerSessionJoinResult.eLostNetwork;
								break;
							case OnlineMultiplayerSessionDisconnectionResult.eGoneOffline:
								returnCode2 = OnlineMultiplayerSessionJoinResult.eGoneOffline;
								break;
							case OnlineMultiplayerSessionDisconnectionResult.eHostDisconnected:
								returnCode2 = OnlineMultiplayerSessionJoinResult.eNoHostConnection;
								break;
							}
							OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult> result3 = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
							{
								m_returnCode = returnCode2
							};
							this.m_joinSessionCallback(result3, null, 0);
						}
						catch (Exception ex3)
						{
						}
						this.Leave();
					}
					break;
				}
			}
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x000DED34 File Offset: 0x000DD134
		private void OnUserDisconnected(OnlineMultiplayerSessionUserId userId)
		{
			if (!this.m_leaveRequested)
			{
				SteamOnlineMultiplayerSessionCoordinator.SessionStatus sessionStatus = this.m_sessionStatus;
				if (sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning)
				{
					if (userId != null)
					{
						this.m_transportCoordinator.CloseConnection(userId.m_steamId);
						userId.m_steamLocalTransportConnectionStatus = SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eConnectionDead;
						if (this.IsHost())
						{
							this.m_userIdList.Remove(userId);
							try
							{
								this.m_remoteUserDisconnectionCallbacks(userId);
							}
							catch (Exception ex)
							{
							}
							this.SetupOutgoingMessage(SteamOnlineMultiplayerSessionCoordinator.TransportMessageTypes.eUserDisconnected);
							this.m_transportBitStreamWriter.Write(userId.m_uniqueId, 8);
							for (int i = 0; i < this.m_userIdList.Count; i++)
							{
								if (!this.m_userIdList[i].m_isLocal)
								{
									this.m_transportCoordinator.SendData(this.m_userIdList[i].m_steamId, this.m_transportSendList._items, this.m_transportSendList.Count, true);
								}
							}
						}
						else if (userId.IsHost)
						{
							OnlineMultiplayerSessionDisconnectionResult disconnectionReason = (!SteamUser.BLoggedOn()) ? OnlineMultiplayerSessionDisconnectionResult.eGoneOffline : OnlineMultiplayerSessionDisconnectionResult.eHostDisconnected;
							this.OnLocalDisconnection(disconnectionReason);
						}
					}
				}
			}
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000DEE70 File Offset: 0x000DD270
		private void SendKeepaliveMessage(OnlineMultiplayerSessionUserId targetUserId)
		{
			if (targetUserId != null && !targetUserId.IsLocal)
			{
				this.SetupOutgoingMessage(SteamOnlineMultiplayerSessionCoordinator.TransportMessageTypes.eKeepalive);
				this.m_transportBitStreamWriter.Write(0, 8);
				targetUserId.m_steamLocalKeepaliveLastSendTime = this.m_gameTimeAtStartOfFrame;
				if (this.m_transportCoordinator.SendData(targetUserId.m_steamId, this.m_transportSendList._items, this.m_transportSendList.Count, false) && this.m_transportStats != null)
				{
					this.m_transportStats.Add(OnlineMultiplayerTransportStats.StatType.eDataSent, (uint)this.m_transportSendList.Count);
				}
			}
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x000DEF00 File Offset: 0x000DD300
		private void OnSteamLobbyCreated(LobbyCreated_t param)
		{
			if (this.m_isInitialized)
			{
				CSteamID csteamID = new CSteamID(param.m_ulSteamIDLobby);
				if (this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eCreating && this.m_creatingSubStatus == SteamOnlineMultiplayerSessionCoordinator.CreatingSubStatus.eWaitingForSteamToCreateTheLobby && !this.m_leaveRequested)
				{
					this.m_creatingSubStatus = SteamOnlineMultiplayerSessionCoordinator.CreatingSubStatus.eComplete;
					if (param.m_eResult == EResult.k_EResultOK && csteamID.IsValid() && csteamID.IsLobby())
					{
						this.m_lobbyId = csteamID;
						return;
					}
				}
				if (csteamID.IsValid())
				{
					try
					{
						SteamMatchmaking.LeaveLobby(csteamID);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x000DEFAC File Offset: 0x000DD3AC
		private void OnSteamLobbyJoined(LobbyEnter_t param)
		{
			if (this.m_isInitialized)
			{
				bool flag = false;
				CSteamID csteamID = new CSteamID(param.m_ulSteamIDLobby);
				if (this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eJoining && this.m_joiningSubStatus == SteamOnlineMultiplayerSessionCoordinator.JoiningSubStatus.eWaitingForSteamToJoinTheLobby && !this.m_leaveRequested)
				{
					if (csteamID.IsValid() && csteamID.IsLobby())
					{
						if (csteamID == this.m_lobbyIdToJoin)
						{
							this.m_joiningSubStatus = SteamOnlineMultiplayerSessionCoordinator.JoiningSubStatus.eCompleteComplete;
							this.m_joiningLobbyResponse = (EChatRoomEnterResponse)param.m_EChatRoomEnterResponse;
							if (param.m_EChatRoomEnterResponse == 1U)
							{
								this.m_lobbyId = csteamID;
								return;
							}
						}
						else
						{
							flag = (param.m_EChatRoomEnterResponse == 1U);
						}
					}
				}
				else if (this.m_lobbyId.IsValid() && this.m_lobbyId == csteamID)
				{
					return;
				}
				if (csteamID.IsValid() && flag)
				{
					try
					{
						SteamMatchmaking.LeaveLobby(csteamID);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x000DF0B4 File Offset: 0x000DD4B4
		private void OnSteamLobbyMembersChanged(LobbyChatUpdate_t param)
		{
			if (this.m_isInitialized && !this.m_leaveRequested)
			{
				EChatMemberStateChange echatMemberStateChange = EChatMemberStateChange.k_EChatMemberStateChangeLeft | EChatMemberStateChange.k_EChatMemberStateChangeDisconnected | EChatMemberStateChange.k_EChatMemberStateChangeKicked | EChatMemberStateChange.k_EChatMemberStateChangeBanned;
				if (this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning)
				{
					if (param.m_ulSteamIDLobby == this.m_lobbyId.m_SteamID && (echatMemberStateChange & (EChatMemberStateChange)param.m_rgfChatMemberStateChange) > (EChatMemberStateChange)0)
					{
						OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = this.m_userIdList.Find((OnlineMultiplayerSessionUserId x) => x.m_steamId.m_SteamID == param.m_ulSteamIDUserChanged);
						if (onlineMultiplayerSessionUserId != null)
						{
							this.OnUserDisconnected(onlineMultiplayerSessionUserId);
						}
					}
				}
				else if (this.m_sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eConnectingToHost && param.m_ulSteamIDLobby == this.m_lobbyId.m_SteamID && (echatMemberStateChange & (EChatMemberStateChange)param.m_rgfChatMemberStateChange) > (EChatMemberStateChange)0 && param.m_ulSteamIDUserChanged == this.m_hostSteamIdDuringConnection.m_SteamID)
				{
					this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eHostDisconnected);
				}
			}
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x000DF1A8 File Offset: 0x000DD5A8
		private void SetupOutgoingMessage(SteamOnlineMultiplayerSessionCoordinator.TransportMessageTypes msgType)
		{
			this.m_transportSendList.Clear();
			this.m_transportBitStreamWriter.Reset(this.m_transportSendList);
			this.m_transportMessageHeader.IsGameMessage = false;
			this.m_transportMessageHeader.MessageTypeId = (byte)msgType;
			this.m_transportMessageHeader.Serialize(this.m_transportBitStreamWriter);
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x000DF1FC File Offset: 0x000DD5FC
		private void ReceiveJoinRequestMessage(CSteamID fromSteamId)
		{
			if (this.IsHost())
			{
				OnlineMultiplayerSessionJoinResult onlineMultiplayerSessionJoinResult = OnlineMultiplayerSessionJoinResult.eGenericFailure;
				uint num = this.m_transportBitStreamReader.ReadUInt32(32);
				if (OnlineMultiplayerConfig.CodeVersion == num)
				{
					if ((long)(this.m_userIdList.Count + this.m_secondaryLocalUserIds.Count) < (long)((ulong)OnlineMultiplayerConfig.MaxPlayers))
					{
						byte b = this.GenerateUniqueUserId();
						OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = new OnlineMultiplayerSessionUserId();
						onlineMultiplayerSessionUserId.Deserialize(this.m_transportBitStreamReader);
						onlineMultiplayerSessionUserId.m_steamLocalTransportConnectionStatus = SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eConnectionActive;
						onlineMultiplayerSessionUserId.m_uniqueId = b;
						onlineMultiplayerSessionUserId.m_steamLocalKeepaliveLastSendTime = this.m_gameTimeAtStartOfFrame;
						onlineMultiplayerSessionUserId.m_steamLocalKeepaliveLastReceiveTime = this.m_gameTimeAtStartOfFrame;
						if (OnlineMultiplayerSessionJoinUserDataHelper.Deserialize(this.m_transportBitStreamReader, this.m_remoteUserJoinData))
						{
							byte[] array = null;
							int num2 = 0;
							try
							{
								onlineMultiplayerSessionJoinResult = this.m_joinDecisionCallback(onlineMultiplayerSessionUserId, this.m_remoteUserJoinData, out array, out num2);
							}
							catch (Exception ex)
							{
							}
							this.m_remoteUserJoinData.Clear();
							if (onlineMultiplayerSessionJoinResult == OnlineMultiplayerSessionJoinResult.eSuccess)
							{
								this.SetupOutgoingMessage(SteamOnlineMultiplayerSessionCoordinator.TransportMessageTypes.eJoinRequestReply);
								this.m_transportBitStreamWriter.Write(0, 8);
								this.m_transportBitStreamWriter.Write(b, 8);
								this.m_transportBitStreamWriter.Write((uint)this.m_userIdList.Count, 8);
								for (int i = 0; i < this.m_userIdList.Count; i++)
								{
									this.m_userIdList[i].Serialize(this.m_transportBitStreamWriter);
								}
								if (array != null && num2 > 0)
								{
									this.m_transportBitStreamWriter.Write((uint)num2, 32);
									for (int j = 0; j < num2; j++)
									{
										this.m_transportBitStreamWriter.Write(array[j], 8);
									}
								}
								else
								{
									uint bits = 0U;
									this.m_transportBitStreamWriter.Write(bits, 32);
								}
								this.m_transportCoordinator.SendData(fromSteamId, this.m_transportSendList._items, this.m_transportSendList.Count, true);
								this.SetupOutgoingMessage(SteamOnlineMultiplayerSessionCoordinator.TransportMessageTypes.eUserJoined);
								onlineMultiplayerSessionUserId.Serialize(this.m_transportBitStreamWriter);
								for (int k = 0; k < this.m_userIdList.Count; k++)
								{
									if (!this.m_userIdList[k].m_isLocal)
									{
										this.m_transportCoordinator.SendData(this.m_userIdList[k].m_steamId, this.m_transportSendList._items, this.m_transportSendList.Count, true);
									}
								}
								this.m_userIdList.Add(onlineMultiplayerSessionUserId);
								try
								{
									this.m_newUserJoinedCallback(onlineMultiplayerSessionUserId);
								}
								catch (Exception ex2)
								{
								}
								return;
							}
						}
						else
						{
							onlineMultiplayerSessionJoinResult = OnlineMultiplayerSessionJoinResult.eNoHostConnection;
						}
					}
					else
					{
						onlineMultiplayerSessionJoinResult = OnlineMultiplayerSessionJoinResult.eFull;
					}
				}
				else
				{
					onlineMultiplayerSessionJoinResult = OnlineMultiplayerSessionJoinResult.eCodeVersionMismatch;
				}
				this.SetupOutgoingMessage(SteamOnlineMultiplayerSessionCoordinator.TransportMessageTypes.eJoinRequestReply);
				this.m_transportBitStreamWriter.Write((byte)onlineMultiplayerSessionJoinResult, 8);
				this.m_transportCoordinator.SendData(fromSteamId, this.m_transportSendList._items, this.m_transportSendList.Count, true);
			}
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x000DF4E4 File Offset: 0x000DD8E4
		private void ReceiveJoinRequestReplyMessage(CSteamID fromSteamId)
		{
			if (this.m_connectingToHostTransportStatus == SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eWaitingForJoinApprovalFromHost && this.m_hostSteamIdDuringConnection == fromSteamId)
			{
				OnlineMultiplayerSessionJoinResult onlineMultiplayerSessionJoinResult = (OnlineMultiplayerSessionJoinResult)this.m_transportBitStreamReader.ReadByte(8);
				string commandLineArgument = this.GetCommandLineArgument("-p2p");
				if (!string.IsNullOrEmpty(commandLineArgument) && string.Compare(commandLineArgument, "fail") == 0)
				{
					onlineMultiplayerSessionJoinResult = OnlineMultiplayerSessionJoinResult.eGenericFailure;
				}
				if (onlineMultiplayerSessionJoinResult == OnlineMultiplayerSessionJoinResult.eSuccess)
				{
					this.m_userIdList[0].m_uniqueId = this.m_transportBitStreamReader.ReadByte(8);
					uint num = this.m_transportBitStreamReader.ReadUInt32(8);
					for (uint num2 = 0U; num2 < num; num2 += 1U)
					{
						OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = new OnlineMultiplayerSessionUserId();
						onlineMultiplayerSessionUserId.Deserialize(this.m_transportBitStreamReader);
						if (onlineMultiplayerSessionUserId.IsHost)
						{
							onlineMultiplayerSessionUserId.m_steamLocalTransportConnectionStatus = SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eConnectionActive;
							onlineMultiplayerSessionUserId.m_steamLocalKeepaliveLastSendTime = this.m_gameTimeAtStartOfFrame;
							onlineMultiplayerSessionUserId.m_steamLocalKeepaliveLastReceiveTime = this.m_gameTimeAtStartOfFrame;
						}
						this.m_userIdList.Add(onlineMultiplayerSessionUserId);
					}
					byte[] array = null;
					int num3 = (int)this.m_transportBitStreamReader.ReadUInt32(32);
					if (num3 > 0)
					{
						array = new byte[num3];
						for (int i = 0; i < num3; i++)
						{
							array[i] = this.m_transportBitStreamReader.ReadByte(8);
						}
					}
					this.m_connectingToHostMaxGameTime = 0f;
					this.m_hostSteamIdDuringConnection.Clear();
					this.m_connectingToHostTransportStatus = SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eNotApplicable;
					this.m_sessionStatus = SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning;
					try
					{
						OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult> result = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
						{
							m_returnCode = onlineMultiplayerSessionJoinResult
						};
						this.m_joinSessionCallback(result, array, num3);
					}
					catch (Exception ex)
					{
					}
				}
				else
				{
					try
					{
						OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult> result2 = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
						{
							m_returnCode = onlineMultiplayerSessionJoinResult
						};
						this.m_joinSessionCallback(result2, null, 0);
					}
					catch (Exception ex2)
					{
					}
					this.Leave();
				}
			}
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x000DF6CC File Offset: 0x000DDACC
		private void ReceiveUserJoinedMessage(CSteamID fromSteamId)
		{
			OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = this.m_userIdList.Find((OnlineMultiplayerSessionUserId x) => x.m_steamId == fromSteamId);
			if (onlineMultiplayerSessionUserId != null && onlineMultiplayerSessionUserId.IsHost)
			{
				OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId2 = new OnlineMultiplayerSessionUserId();
				onlineMultiplayerSessionUserId2.Deserialize(this.m_transportBitStreamReader);
				onlineMultiplayerSessionUserId2.m_steamLocalTransportConnectionStatus = SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eWaitingToStartClientConnection;
				this.m_userIdList.Add(onlineMultiplayerSessionUserId2);
			}
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000DF734 File Offset: 0x000DDB34
		private void ReceiveUserDisconnectedMessage(CSteamID fromSteamId)
		{
			OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = this.m_userIdList.Find((OnlineMultiplayerSessionUserId x) => x.m_steamId == fromSteamId);
			if (onlineMultiplayerSessionUserId != null && onlineMultiplayerSessionUserId.IsHost)
			{
				byte uniqueId = this.m_transportBitStreamReader.ReadByte(8);
				OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId2 = this.m_userIdList.Find((OnlineMultiplayerSessionUserId x) => x.m_uniqueId == uniqueId);
				if (onlineMultiplayerSessionUserId2 != null)
				{
					this.m_transportCoordinator.CloseConnection(onlineMultiplayerSessionUserId2.m_steamId);
					this.m_userIdList.Remove(onlineMultiplayerSessionUserId2);
				}
			}
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000DF7CC File Offset: 0x000DDBCC
		private void OnTransportDisconnectionCallback(CSteamID steamId)
		{
			if (!this.m_leaveRequested)
			{
				SteamOnlineMultiplayerSessionCoordinator.SessionStatus sessionStatus = this.m_sessionStatus;
				if (sessionStatus != SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eConnectingToHost)
				{
					if (sessionStatus == SteamOnlineMultiplayerSessionCoordinator.SessionStatus.eRunning)
					{
						OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = this.m_userIdList.Find((OnlineMultiplayerSessionUserId x) => x.m_steamId == steamId);
						if (onlineMultiplayerSessionUserId != null)
						{
							this.OnUserDisconnected(onlineMultiplayerSessionUserId);
						}
					}
				}
				else
				{
					this.OnLocalDisconnection(OnlineMultiplayerSessionDisconnectionResult.eHostDisconnected);
				}
			}
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000DF848 File Offset: 0x000DDC48
		private void OnTransportDataCallback(CSteamID fromSteamId, byte[] data, int dataSizeInBytes)
		{
			if (!this.m_leaveRequested)
			{
				try
				{
					if (this.m_transportStats != null)
					{
						this.m_transportStats.Add(OnlineMultiplayerTransportStats.StatType.eDataReceived, (uint)dataSizeInBytes);
					}
					this.m_transportBitStreamReader.Reset(data, dataSizeInBytes);
					this.m_transportMessageHeader.Deserialize(this.m_transportBitStreamReader);
					OnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = this.m_userIdList.Find((OnlineMultiplayerSessionUserId x) => x.m_steamId == fromSteamId);
					if (onlineMultiplayerSessionUserId != null)
					{
						onlineMultiplayerSessionUserId.m_steamLocalKeepaliveLastReceiveTime = this.m_gameTimeAtStartOfFrame;
					}
					if (this.m_transportMessageHeader.IsGameMessage)
					{
						if (onlineMultiplayerSessionUserId != null)
						{
							this.m_dataReceivedCallback(onlineMultiplayerSessionUserId, data, dataSizeInBytes);
						}
					}
					else
					{
						switch (this.m_transportMessageHeader.MessageTypeId)
						{
						case 1:
							this.ReceiveJoinRequestMessage(fromSteamId);
							break;
						case 2:
							this.ReceiveJoinRequestReplyMessage(fromSteamId);
							break;
						case 3:
							this.ReceiveUserJoinedMessage(fromSteamId);
							break;
						case 4:
							this.ReceiveUserDisconnectedMessage(fromSteamId);
							break;
						}
					}
				}
				catch (Exception ex)
				{
				}
			}
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000DF998 File Offset: 0x000DDD98
		private void OnTransportVoipDataCallback(CSteamID fromSteamId, byte[] data, int dataSiszeInBytes)
		{
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x000DF99C File Offset: 0x000DDD9C
		private string GetCommandLineArgument(string Key)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].Contains(Key))
				{
					string[] array = commandLineArgs[i].Split(new char[]
					{
						':'
					});
					if (array.Length == 2)
					{
						return array[1];
					}
				}
			}
			return null;
		}

		// Token: 0x040025E7 RID: 9703
		private readonly float m_entryDelayInSeconds = 1f;

		// Token: 0x040025E8 RID: 9704
		private readonly float m_connectToClientMaxTimeInSeconds = 30f;

		// Token: 0x040025E9 RID: 9705
		private readonly float m_connectToHostMaxTimeInSeconds = 20f;

		// Token: 0x040025EA RID: 9706
		private readonly float m_keepaliveMessageFrequencyInSeconds = 1f;

		// Token: 0x040025EB RID: 9707
		private static Callback<LobbyCreated_t> s_steamLobbyCreatedCallback;

		// Token: 0x040025EC RID: 9708
		private static Callback<LobbyEnter_t> s_steamLobbyJoinedCallback;

		// Token: 0x040025ED RID: 9709
		private static Callback<LobbyChatUpdate_t> s_steamLobbyMembersChangedCallback;

		// Token: 0x040025EE RID: 9710
		private float m_gameTimeAtStartOfFrame;

		// Token: 0x040025EF RID: 9711
		private bool m_isInitialized;

		// Token: 0x040025F0 RID: 9712
		private bool m_leaveRequested;

		// Token: 0x040025F1 RID: 9713
		private bool m_forceModify;

		// Token: 0x040025F2 RID: 9714
		private CSteamID m_hostSteamId = default(CSteamID);

		// Token: 0x040025F3 RID: 9715
		private CSteamID m_hostSteamIdDuringConnection = default(CSteamID);

		// Token: 0x040025F4 RID: 9716
		private byte m_uniqueUserIdCounter = OnlineMultiplayerSessionUserId.c_InvalidUniqueId;

		// Token: 0x040025F5 RID: 9717
		private CSteamID m_lobbyId = default(CSteamID);

		// Token: 0x040025F6 RID: 9718
		private CSteamID m_lobbyIdToJoin = default(CSteamID);

		// Token: 0x040025F7 RID: 9719
		private SteamOnlineMultiplayerSessionCoordinator.SessionStatus m_sessionStatus;

		// Token: 0x040025F8 RID: 9720
		private SteamOnlineMultiplayerSessionCoordinator.CreatingSubStatus m_creatingSubStatus;

		// Token: 0x040025F9 RID: 9721
		private SteamOnlineMultiplayerSessionCoordinator.JoiningSubStatus m_joiningSubStatus;

		// Token: 0x040025FA RID: 9722
		private EChatRoomEnterResponse m_joiningLobbyResponse = EChatRoomEnterResponse.k_EChatRoomEnterResponseError;

		// Token: 0x040025FB RID: 9723
		private List<OnlineMultiplayerSessionPropertyValue> m_creatingPropertyValues;

		// Token: 0x040025FC RID: 9724
		private float m_entryDelayMaxGameTime;

		// Token: 0x040025FD RID: 9725
		private OnlineMultiplayerLocalUserId m_localPlayerUserId;

		// Token: 0x040025FE RID: 9726
		private OnlineMultiplayerSessionVisibility m_sessionVisibility = OnlineMultiplayerSessionVisibility.eClosed;

		// Token: 0x040025FF RID: 9727
		public List<OnlineMultiplayerSessionUserId> m_userIdList = new List<OnlineMultiplayerSessionUserId>();

		// Token: 0x04002600 RID: 9728
		private OnlineMultiplayerSessionPropertyCoordinator m_sessionPropertyCoordinator;

		// Token: 0x04002601 RID: 9729
		private OnlineMultiplayerTransportStats m_transportStats;

		// Token: 0x04002602 RID: 9730
		private OnlineMultiplayerSessionCreateCallback m_createSessionCallback;

		// Token: 0x04002603 RID: 9731
		private OnlineMultiplayerSessionDisconnectionCallback m_disconnectionCallbacks = delegate(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> result)
		{
		};

		// Token: 0x04002604 RID: 9732
		private OnlineMultiplayerSessionRemoteUserDisconnectionCallback m_remoteUserDisconnectionCallbacks = delegate(IOnlineMultiplayerSessionUserId leavingUserId)
		{
		};

		// Token: 0x04002605 RID: 9733
		private OnlineMultiplayerSessionDataReceivedCallback m_dataReceivedCallback = delegate(IOnlineMultiplayerSessionUserId fromUserId, byte[] receivedData, int receivedDataSize)
		{
		};

		// Token: 0x04002606 RID: 9734
		private OnlineMultiplayerSessionJoinDecisionCallback m_joinDecisionCallback;

		// Token: 0x04002607 RID: 9735
		private OnlineMultiplayerSessionUserJoinedCallback m_newUserJoinedCallback;

		// Token: 0x04002608 RID: 9736
		private OnlineMultiplayerSessionJoinCallback m_joinSessionCallback;

		// Token: 0x04002609 RID: 9737
		private FastList<byte> m_transportSendList = new FastList<byte>((int)OnlineMultiplayerConfig.MaxTransportMessageSize);

		// Token: 0x0400260A RID: 9738
		private byte[] m_transportReceiveBuffer = new byte[OnlineMultiplayerConfig.MaxTransportMessageSize];

		// Token: 0x0400260B RID: 9739
		private OnlineMultiplayerSessionTransportMessageHeader m_transportMessageHeader = new OnlineMultiplayerSessionTransportMessageHeader();

		// Token: 0x0400260C RID: 9740
		private BitStreamWriter m_transportBitStreamWriter;

		// Token: 0x0400260D RID: 9741
		private BitStreamReader m_transportBitStreamReader;

		// Token: 0x0400260E RID: 9742
		private SteamOnlineMultiplayerSessionTransportCoordinator m_transportCoordinator = new SteamOnlineMultiplayerSessionTransportCoordinator();

		// Token: 0x0400260F RID: 9743
		private float m_connectingToHostMaxGameTime;

		// Token: 0x04002610 RID: 9744
		private SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus m_connectingToHostTransportStatus;

		// Token: 0x04002611 RID: 9745
		private List<OnlineMultiplayerLocalUserId> m_secondaryLocalUserIds = new List<OnlineMultiplayerLocalUserId>();

		// Token: 0x04002612 RID: 9746
		private List<OnlineMultiplayerSessionJoinLocalUserData> m_localUserJoinData = new List<OnlineMultiplayerSessionJoinLocalUserData>((int)(OnlineMultiplayerConfig.MaxPlayers - 1U));

		// Token: 0x04002613 RID: 9747
		private List<OnlineMultiplayerSessionJoinRemoteUserData> m_remoteUserJoinData = new List<OnlineMultiplayerSessionJoinRemoteUserData>((int)(OnlineMultiplayerConfig.MaxPlayers - 1U));

		// Token: 0x02000982 RID: 2434
		private enum SessionStatus : byte
		{
			// Token: 0x04002618 RID: 9752
			eIdle,
			// Token: 0x04002619 RID: 9753
			eCreating,
			// Token: 0x0400261A RID: 9754
			eRunning,
			// Token: 0x0400261B RID: 9755
			eJoining,
			// Token: 0x0400261C RID: 9756
			eConnectingToHost
		}

		// Token: 0x02000983 RID: 2435
		private enum CreatingSubStatus : byte
		{
			// Token: 0x0400261E RID: 9758
			eIdle,
			// Token: 0x0400261F RID: 9759
			eInitialDelay,
			// Token: 0x04002620 RID: 9760
			eWaitingForSteamToCreateTheLobby,
			// Token: 0x04002621 RID: 9761
			eComplete
		}

		// Token: 0x02000984 RID: 2436
		private enum JoiningSubStatus : byte
		{
			// Token: 0x04002623 RID: 9763
			eIdle,
			// Token: 0x04002624 RID: 9764
			eInitialDelay,
			// Token: 0x04002625 RID: 9765
			eWaitingForSteamToJoinTheLobby,
			// Token: 0x04002626 RID: 9766
			eCompleteComplete
		}

		// Token: 0x02000985 RID: 2437
		private enum TransportMessageTypes : byte
		{
			// Token: 0x04002628 RID: 9768
			eNothing,
			// Token: 0x04002629 RID: 9769
			eJoinRequest,
			// Token: 0x0400262A RID: 9770
			eJoinRequestReply,
			// Token: 0x0400262B RID: 9771
			eUserJoined,
			// Token: 0x0400262C RID: 9772
			eUserDisconnected,
			// Token: 0x0400262D RID: 9773
			eKeepalive
		}
	}
}
