using System;
using System.Collections.Generic;
using BitStream;
using Team17.Online.Multiplayer.Connection;
using Team17.Online.Multiplayer.Messaging;

namespace Team17.Online.Multiplayer
{
	// Token: 0x0200083B RID: 2107
	public class Client : PeerBase
	{
		// Token: 0x0600289D RID: 10397 RVA: 0x000BF038 File Offset: 0x000BD438
		public void Initialise(bool bOnline)
		{
			if (bOnline)
			{
				IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
				this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
				if (this.m_iOnlineMultiplayerSessionCoordinator != null)
				{
					this.m_iOnlineMultiplayerSessionCoordinator.RegisterDataReceivedCallback(new OnlineMultiplayerSessionDataReceivedCallback(this.OnlineMultiplayerSessionDataReceivedCallback));
				}
			}
			this.m_ClientUserSystem.Initialise();
			this.m_Writer = new BitStreamWriter(this.m_WriteBuffer);
			this.m_Time.Initialise();
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000BF0A8 File Offset: 0x000BD4A8
		public void Reset()
		{
			if (this.m_ServerConnection != null)
			{
				this.m_ServerConnection.Disconnect();
				this.m_ServerConnection = null;
			}
			if (this.m_iOnlineMultiplayerSessionCoordinator != null)
			{
				this.m_iOnlineMultiplayerSessionCoordinator.UnRegisterDataReceivedCallback(new OnlineMultiplayerSessionDataReceivedCallback(this.OnlineMultiplayerSessionDataReceivedCallback));
			}
			this.m_Time.Shutdown();
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000BF0FF File Offset: 0x000BD4FF
		public ClientUserSystem GetUserSystem()
		{
			return this.m_ClientUserSystem;
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x000BF108 File Offset: 0x000BD508
		public override ConnectionStats GetConnectionStats(bool bReliable)
		{
			if (this.m_ServerConnection != null)
			{
				return this.m_ServerConnection.GetConnectionStats(bReliable);
			}
			return default(ConnectionStats);
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000BF138 File Offset: 0x000BD538
		public void SendMessageToServer(MessageType type, Serialisable message, bool bReliable = true)
		{
			if (this.m_ServerConnection != null)
			{
				this.m_WriteBuffer.Clear();
				this.m_Writer.Reset(this.m_WriteBuffer);
				this.m_CurrentMessage.Type = type;
				this.m_CurrentMessage.Payload = message;
				this.m_CurrentMessage.Serialise(this.m_Writer);
				if (this.m_Tracker != null)
				{
					this.m_Tracker.TrackSentGlobalEvent(type);
				}
				if (!this.m_ServerConnection.SendMessage(this.m_WriteBuffer._items, this.m_WriteBuffer.Count, bReliable))
				{
					this.m_ServerConnection.Disconnect();
				}
			}
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x000BF1DE File Offset: 0x000BD5DE
		public void Update()
		{
			ClientTime.Update();
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000BF1E5 File Offset: 0x000BD5E5
		public override void Dispatch()
		{
			if (this.m_ServerConnection != null)
			{
				this.m_ServerConnection.Dispatch();
			}
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000BF1FD File Offset: 0x000BD5FD
		public void HandleOutgoingServerConnectionAccepted(NetworkConnection serverConnection)
		{
			this.m_ServerConnection = serverConnection;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000BF206 File Offset: 0x000BD606
		public void OnlineMultiplayerSessionDataReceivedCallback(IOnlineMultiplayerSessionUserId fromUserId, byte[] receivedData, int receivedDataSize)
		{
			this.m_ServerConnection.HandleReceivedBytes(receivedData, receivedDataSize);
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000BF215 File Offset: 0x000BD615
		public override void HandleConnectionLost(IOnlineMultiplayerSessionUserId sessionUserId, NetworkConnection connection)
		{
			this.m_ServerConnection = null;
			base.LeaveSession(this.m_iOnlineMultiplayerSessionCoordinator);
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000BF22A File Offset: 0x000BD62A
		public override void HandleLocalLoopbackConnectionLost(NetworkConnection connection)
		{
			this.m_ServerConnection = null;
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000BF233 File Offset: 0x000BD633
		public override void HandleDisconnectMessage(NetworkConnection connection)
		{
			DisconnectionHandler.HandleKickMessage();
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000BF23A File Offset: 0x000BD63A
		public override void SetLatencyTestPaused(bool paused)
		{
			if (this.m_ServerConnection != null)
			{
				this.m_ServerConnection.SetLatencyTestPaused(paused);
			}
		}

		// Token: 0x04002011 RID: 8209
		private FastList<byte> m_WriteBuffer = new FastList<byte>(1024);

		// Token: 0x04002012 RID: 8210
		private ClientUserSystem m_ClientUserSystem = new ClientUserSystem();

		// Token: 0x04002013 RID: 8211
		private BitStreamWriter m_Writer;

		// Token: 0x04002014 RID: 8212
		private NetworkConnection m_ServerConnection;

		// Token: 0x04002015 RID: 8213
		private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

		// Token: 0x04002016 RID: 8214
		private ClientTime m_Time = new ClientTime();
	}
}
