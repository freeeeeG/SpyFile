using System;
using System.Collections.Generic;
using BitStream;
using Team17.Online.Multiplayer.Connection;
using Team17.Online.Multiplayer.Messaging;

namespace Team17.Online.Multiplayer
{
	// Token: 0x02000902 RID: 2306
	public class Server : PeerBase
	{
		// Token: 0x06002CFE RID: 11518 RVA: 0x000D455C File Offset: 0x000D295C
		public void Initialise(bool bOnline)
		{
			if (bOnline)
			{
				this.m_OnlinePlatform = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
				this.m_OnlineMultiplayerSessionCoordinator = this.m_OnlinePlatform.OnlineMultiplayerSessionCoordinator();
				if (this.m_OnlineMultiplayerSessionCoordinator != null)
				{
					this.m_OnlineMultiplayerSessionCoordinator.RegisterDataReceivedCallback(new OnlineMultiplayerSessionDataReceivedCallback(this.OnlineMultiplayerSessionDataReceivedCallback));
				}
			}
			ServerUserSystem.OnUserRemoved = (GenericVoid<User>)Delegate.Combine(ServerUserSystem.OnUserRemoved, new GenericVoid<User>(this.OnUserRemoved));
			this.m_Writer = new BitStreamWriter(this.m_WriteBuffer);
			while (this.m_remoteGameUserDataStorageCache.Count != 4)
			{
				this.m_remoteGameUserDataStorageCache.Add(new JoinDataProvider.GameUserData());
			}
			this.m_bInitialised = true;
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x000D4610 File Offset: 0x000D2A10
		public void Reset()
		{
			for (int i = this.m_AllConnections.Count - 1; i >= 0; i--)
			{
				this.m_AllConnections._items[i].Disconnect();
			}
			this.m_RemoteClientConnections.Clear();
			this.m_AllConnections.Clear();
			this.m_LocalClientConnection = null;
			if (this.m_OnlineMultiplayerSessionCoordinator != null)
			{
				this.m_OnlineMultiplayerSessionCoordinator.UnRegisterDataReceivedCallback(new OnlineMultiplayerSessionDataReceivedCallback(this.OnlineMultiplayerSessionDataReceivedCallback));
			}
			ServerUserSystem.OnUserRemoved = (GenericVoid<User>)Delegate.Remove(ServerUserSystem.OnUserRemoved, new GenericVoid<User>(this.OnUserRemoved));
			this.m_bInitialised = false;
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x000D46B3 File Offset: 0x000D2AB3
		public override ConnectionStats GetConnectionStats(bool bReliable)
		{
			return this.m_LocalClientConnection.GetConnectionStats(bReliable);
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x000D46C4 File Offset: 0x000D2AC4
		public FastList<ConnectionStats> GetAllConnectionStats(bool bReliable)
		{
			int num = 0;
			for (int i = 0; i < this.m_AllConnections.Count; i++)
			{
				if (this.m_AllConnections._items[i] != this.m_LocalClientConnection)
				{
					this.m_ConnectionStatsList._items[num] = this.m_AllConnections._items[i].GetConnectionStats(bReliable);
					num++;
				}
			}
			return this.m_ConnectionStatsList;
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x000D473A File Offset: 0x000D2B3A
		public void Update()
		{
			if (this.m_bInitialised)
			{
				this.m_ServerUserSystem.Update();
			}
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x000D4754 File Offset: 0x000D2B54
		public override void Dispatch()
		{
			if (this.m_bInitialised)
			{
				for (int i = 0; i < this.m_AllConnections.Count; i++)
				{
					this.m_AllConnections._items[i].Dispatch();
				}
			}
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x000D479A File Offset: 0x000D2B9A
		public ServerUserSystem GetUserSystem()
		{
			return this.m_ServerUserSystem;
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x000D47A4 File Offset: 0x000D2BA4
		public void BroadcastMessageToAll(MessageType type, Serialisable message, bool bReliable = true)
		{
			for (int i = this.m_AllConnections.Count - 1; i >= 0; i--)
			{
				this.SendMessageToClient(this.m_AllConnections._items[i], type, message, bReliable);
			}
			if (this.m_Tracker != null)
			{
				this.m_Tracker.TrackSentGlobalEvent(type);
			}
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000D47FC File Offset: 0x000D2BFC
		public void SendMessageToClient(IOnlineMultiplayerSessionUserId sessionUser, MessageType type, Serialisable message, bool bReliable = true)
		{
			NetworkConnection connection;
			if (sessionUser == null)
			{
				this.SendMessageToClient(this.m_LocalClientConnection, type, message, bReliable);
			}
			else if (this.m_RemoteClientConnections.TryGetValue(sessionUser, out connection))
			{
				this.SendMessageToClient(connection, type, message, bReliable);
			}
			if (this.m_Tracker != null)
			{
				this.m_Tracker.TrackSentGlobalEvent(type);
			}
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x000D4859 File Offset: 0x000D2C59
		public void EnsureLocalLoopbackClientConnection(LocalLoopbackConnection connection)
		{
			if (this.m_LocalClientConnection != null)
			{
				this.m_AllConnections.Remove(this.m_LocalClientConnection);
			}
			this.m_LocalClientConnection = connection;
			this.m_AllConnections.Add(connection);
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000D488C File Offset: 0x000D2C8C
		public override void HandleConnectionLost(IOnlineMultiplayerSessionUserId sessionUserId, NetworkConnection connection)
		{
			this.RemoveConnection(sessionUserId, connection);
			User user = UserSystemUtils.FindUser(ServerUserSystem.m_Users, connection.GetRemoteSessionUserId(), User.MachineID.Count, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count);
			if (user != null)
			{
				FastList<User> users = ServerUserSystem.m_Users;
				User.MachineID machine = user.Machine;
				User[] array = UserSystemUtils.FindUsers(users, null, machine, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count);
				if (array != null && array.Length > 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						ServerUserSystem.RemoveUser(array[i], i == array.Length - 1);
					}
				}
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000D490E File Offset: 0x000D2D0E
		public override void HandleLocalLoopbackConnectionLost(NetworkConnection connection)
		{
			if (this.m_LocalClientConnection != null && this.m_LocalClientConnection == connection)
			{
				this.m_LocalClientConnection = null;
			}
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000D4930 File Offset: 0x000D2D30
		public override void HandleDisconnectMessage(NetworkConnection connection)
		{
			this.RemoveConnection(connection.GetRemoteSessionUserId(), connection);
			User user = UserSystemUtils.FindUser(ServerUserSystem.m_Users, connection.GetRemoteSessionUserId(), User.MachineID.Count, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count);
			if (user != null)
			{
				FastList<User> users = ServerUserSystem.m_Users;
				User.MachineID machine = user.Machine;
				User[] array = UserSystemUtils.FindUsers(users, null, machine, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count);
				if (array != null && array.Length > 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						ServerUserSystem.RemoveUser(array[i], i == array.Length - 1);
					}
				}
			}
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000D49B8 File Offset: 0x000D2DB8
		public override void SetLatencyTestPaused(bool paused)
		{
			for (int i = 0; i < this.m_AllConnections.Count; i++)
			{
				this.m_AllConnections._items[i].SetLatencyTestPaused(paused);
			}
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000D49F4 File Offset: 0x000D2DF4
		public OnlineMultiplayerSessionJoinResult OnlineMultiplayerSessionJoinDecisionCallback(IOnlineMultiplayerSessionUserId primaryRemoteSessionUserId, List<OnlineMultiplayerSessionJoinRemoteUserData> remoteUserData, out byte[] replyData, out int replyDataSize)
		{
			OnlineMultiplayerSessionVisibility onlineMultiplayerSessionVisibility = OnlineMultiplayerSessionVisibility.eClosed;
			replyData = null;
			replyDataSize = 0;
			if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Server)
			{
				onlineMultiplayerSessionVisibility = ((ServerOptions)ConnectionModeSwitcher.GetAgentData()).visibility;
			}
			else if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Matchmake)
			{
				onlineMultiplayerSessionVisibility = OnlineMultiplayerSessionVisibility.eMatchmaking;
			}
			if (onlineMultiplayerSessionVisibility == OnlineMultiplayerSessionVisibility.eClosed)
			{
				return OnlineMultiplayerSessionJoinResult.eClosed;
			}
			if (ServerUserSystem.m_Users.Count == 4)
			{
				return OnlineMultiplayerSessionJoinResult.eFull;
			}
			if (ServerUserSystem.m_Users.Count + remoteUserData.Count > 4)
			{
				return OnlineMultiplayerSessionJoinResult.eNotEnoughRoomForAllLocalUsers;
			}
			if (remoteUserData.Count > this.m_remoteGameUserDataStorageCache.Count)
			{
				return OnlineMultiplayerSessionJoinResult.eNotEnoughRoomForAllLocalUsers;
			}
			BitStreamReader bitStreamReader = new BitStreamReader(remoteUserData[0].GameData);
			for (int i = 0; i < remoteUserData.Count; i++)
			{
				this.m_remoteGameUserDataStorageCache[i].Clear();
				bitStreamReader.Reset(remoteUserData[i].GameData);
				if (!this.m_remoteGameUserDataStorageCache[i].Deserialize(bitStreamReader))
				{
					for (int j = 0; j < this.m_remoteGameUserDataStorageCache.Count; j++)
					{
						this.m_remoteGameUserDataStorageCache[j].Clear();
					}
					return OnlineMultiplayerSessionJoinResult.eGenericFailure;
				}
			}
			if (this.m_remoteGameUserDataStorageCache[0].JoinMethod == NetConnectionState.AcceptInvite && onlineMultiplayerSessionVisibility != OnlineMultiplayerSessionVisibility.ePublic && onlineMultiplayerSessionVisibility != OnlineMultiplayerSessionVisibility.ePrivate)
			{
				return OnlineMultiplayerSessionJoinResult.eClosed;
			}
			if (this.m_remoteGameUserDataStorageCache[0].JoinMethod == NetConnectionState.Matchmake && onlineMultiplayerSessionVisibility != OnlineMultiplayerSessionVisibility.ePublic && onlineMultiplayerSessionVisibility != OnlineMultiplayerSessionVisibility.eMatchmaking)
			{
				return OnlineMultiplayerSessionJoinResult.eClosed;
			}
			User.MachineID availableMachineId = ServerUserSystem.GetAvailableMachineId();
			for (int k = 0; k < remoteUserData.Count; k++)
			{
				User user = this.m_ServerUserSystem.AddNewRemoteUser(availableMachineId, (k != 0) ? null : primaryRemoteSessionUserId, this.m_remoteGameUserDataStorageCache[k]);
				this.m_remoteGameUserDataStorageCache[k].Clear();
			}
			RemoteConnection remoteConnection = new RemoteConnection();
			remoteConnection.Initialise(this, this.m_OnlineMultiplayerSessionCoordinator, primaryRemoteSessionUserId);
			this.AddConnection(primaryRemoteSessionUserId, remoteConnection);
			this.m_ReplyData.Clear();
			BitStreamWriter bitStreamWriter = new BitStreamWriter(this.m_ReplyData);
			bitStreamWriter.Write((uint)availableMachineId, 3);
			this.m_TimeSync.Initialise(ClientTime.Time());
			this.m_TimeSync.Serialise(bitStreamWriter);
			this.m_UsersChanged.Initialise(ServerUserSystem.m_Users);
			this.m_UsersChanged.Serialise(bitStreamWriter);
			this.m_GameSetup.Initialise(ClientGameSetup.Mode);
			this.m_GameSetup.Serialise(bitStreamWriter);
			replyData = this.m_ReplyData._items;
			replyDataSize = this.m_ReplyData.Count;
			return OnlineMultiplayerSessionJoinResult.eSuccess;
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000D4C7E File Offset: 0x000D307E
		public void OnlineMultiplayerSessionUserJoinedCallback(IOnlineMultiplayerSessionUserId primaryRemoteSessionUserId)
		{
			ServerMessenger.UsersChanged();
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000D4C86 File Offset: 0x000D3086
		private void OnlineMultiplayerSessionDataReceivedCallback(IOnlineMultiplayerSessionUserId fromUserId, byte[] receivedData, int receivedDataSize)
		{
			if (this.m_RemoteClientConnections.ContainsKey(fromUserId))
			{
				this.m_RemoteClientConnections[fromUserId].HandleReceivedBytes(receivedData, receivedDataSize);
			}
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000D4CAC File Offset: 0x000D30AC
		private void SendMessageToClient(NetworkConnection connection, MessageType type, Serialisable message, bool bReliable)
		{
			this.m_WriteBuffer.Clear();
			this.m_Writer.Reset(this.m_WriteBuffer);
			this.m_CurrentMessage.Type = type;
			this.m_CurrentMessage.Payload = message;
			this.m_CurrentMessage.Serialise(this.m_Writer);
			if (!connection.SendMessage(this.m_WriteBuffer._items, this.m_WriteBuffer.Count, bReliable))
			{
				connection.Disconnect();
			}
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000D4D28 File Offset: 0x000D3128
		private void AddConnection(IOnlineMultiplayerSessionUserId sessionUserId, NetworkConnection connection)
		{
			this.m_RemoteClientConnections.Add(sessionUserId, connection);
			this.m_AllConnections.Add(connection);
			this.m_ConnectionStatsList.Add(default(ConnectionStats));
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x000D4D64 File Offset: 0x000D3164
		private void RemoveConnection(IOnlineMultiplayerSessionUserId sessionUserId, NetworkConnection connection)
		{
			if (this.m_RemoteClientConnections.ContainsKey(sessionUserId))
			{
				this.m_RemoteClientConnections.Remove(sessionUserId);
				this.m_ConnectionStatsList.RemoveAt(this.m_ConnectionStatsList.Count - 1);
			}
			this.m_AllConnections.Remove(connection);
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000D4DB4 File Offset: 0x000D31B4
		private void OnUserRemoved(User user)
		{
			IOnlineMultiplayerSessionUserId sessionId = user.SessionId;
			if (sessionId != null && this.m_RemoteClientConnections.ContainsKey(sessionId))
			{
				NetworkConnection networkConnection = this.m_RemoteClientConnections[sessionId];
				networkConnection.Disconnect();
				this.RemoveConnection(sessionId, networkConnection);
			}
		}

		// Token: 0x04002423 RID: 9251
		private IOnlinePlatformManager m_OnlinePlatform;

		// Token: 0x04002424 RID: 9252
		private IOnlineMultiplayerSessionCoordinator m_OnlineMultiplayerSessionCoordinator;

		// Token: 0x04002425 RID: 9253
		private FastList<byte> m_WriteBuffer = new FastList<byte>(1024);

		// Token: 0x04002426 RID: 9254
		private BitStreamWriter m_Writer;

		// Token: 0x04002427 RID: 9255
		private Dictionary<IOnlineMultiplayerSessionUserId, NetworkConnection> m_RemoteClientConnections = new Dictionary<IOnlineMultiplayerSessionUserId, NetworkConnection>();

		// Token: 0x04002428 RID: 9256
		private LocalLoopbackConnection m_LocalClientConnection;

		// Token: 0x04002429 RID: 9257
		private FastList<NetworkConnection> m_AllConnections = new FastList<NetworkConnection>();

		// Token: 0x0400242A RID: 9258
		private ServerUserSystem m_ServerUserSystem = new ServerUserSystem();

		// Token: 0x0400242B RID: 9259
		private FastList<ConnectionStats> m_ConnectionStatsList = new FastList<ConnectionStats>(4);

		// Token: 0x0400242C RID: 9260
		private List<JoinDataProvider.GameUserData> m_remoteGameUserDataStorageCache = new List<JoinDataProvider.GameUserData>(4);

		// Token: 0x0400242D RID: 9261
		private bool m_bInitialised;

		// Token: 0x0400242E RID: 9262
		private FastList<byte> m_ReplyData = new FastList<byte>((int)OnlineMultiplayerConfig.MaxTransportMessageSize);

		// Token: 0x0400242F RID: 9263
		private UsersChangedMessage m_UsersChanged = new UsersChangedMessage();

		// Token: 0x04002430 RID: 9264
		private TimeSyncMessage m_TimeSync = new TimeSyncMessage();

		// Token: 0x04002431 RID: 9265
		private GameSetupMessage m_GameSetup = new GameSetupMessage();
	}
}
