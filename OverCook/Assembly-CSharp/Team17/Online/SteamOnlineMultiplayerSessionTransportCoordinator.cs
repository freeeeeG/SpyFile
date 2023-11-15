using System;
using System.Collections.Generic;
using Steamworks;

namespace Team17.Online
{
	// Token: 0x0200098B RID: 2443
	public class SteamOnlineMultiplayerSessionTransportCoordinator
	{
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06002FAD RID: 12205 RVA: 0x000DFFB8 File Offset: 0x000DE3B8
		public static uint DisconnectionTimeInSeconds
		{
			get
			{
				return 5U;
			}
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000DFFBB File Offset: 0x000DE3BB
		public void Initialize()
		{
			if (!this.m_isInitialized)
			{
				SteamOnlineMultiplayerSessionTransportCoordinator.s_steamTransportConnectionFail = Callback<P2PSessionConnectFail_t>.Create(new Callback<P2PSessionConnectFail_t>.DispatchDelegate(this.OnSteamConnectionFail));
				SteamOnlineMultiplayerSessionTransportCoordinator.s_steamTransportConnectionRequest = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.OnSteamConnectionRequest));
				this.m_isInitialized = true;
			}
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000DFFFC File Offset: 0x000DE3FC
		public bool Open(CSteamID lobbyId, SteamOnlineMultiplayerSessionTransportCoordinator.DisconnectionCallback disconnectionCallback, SteamOnlineMultiplayerSessionTransportCoordinator.DataCallback dataCallback, SteamOnlineMultiplayerSessionTransportCoordinator.DataCallback voipDataCallback)
		{
			if (this.m_isInitialized && this.m_status == SteamOnlineMultiplayerSessionTransportCoordinator.Status.eIdle)
			{
				if (lobbyId.IsValid() && lobbyId.IsLobby() && disconnectionCallback != null && dataCallback != null && voipDataCallback != null)
				{
					try
					{
						if (SteamPlayerManager.Initialized)
						{
							this.m_disconnectionCallback = disconnectionCallback;
							this.m_dataCallback = dataCallback;
							this.m_voipDataCallback = voipDataCallback;
							this.m_lobbyId = lobbyId;
							this.m_status = SteamOnlineMultiplayerSessionTransportCoordinator.Status.eOpen;
							return true;
						}
					}
					catch (Exception)
					{
					}
				}
				this.Close();
			}
			return false;
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x000E00A4 File Offset: 0x000DE4A4
		public void Close()
		{
			if (this.m_status == SteamOnlineMultiplayerSessionTransportCoordinator.Status.eOpen)
			{
				try
				{
					if (SteamPlayerManager.Initialized)
					{
						for (int i = 0; i < this.m_connectionIds.Count; i++)
						{
							SteamNetworking.CloseP2PSessionWithUser(this.m_connectionIds[i]);
						}
					}
				}
				catch (Exception)
				{
				}
			}
			this.m_disconnectionCallback = null;
			this.m_dataCallback = null;
			this.m_voipDataCallback = null;
			this.m_lobbyId.Clear();
			this.m_connectionIds.Clear();
			this.m_status = SteamOnlineMultiplayerSessionTransportCoordinator.Status.eIdle;
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000E0144 File Offset: 0x000DE544
		public void CloseConnection(CSteamID steamId)
		{
			if (this.m_status == SteamOnlineMultiplayerSessionTransportCoordinator.Status.eOpen && steamId.IsValid() && !steamId.IsLobby())
			{
				if (this.m_connectionIds.Exists((CSteamID x) => x == steamId))
				{
					this.m_connectionIds.Remove(steamId);
				}
				try
				{
					SteamNetworking.CloseP2PSessionWithUser(steamId);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x000E01E0 File Offset: 0x000DE5E0
		public bool SendData(CSteamID toSteamId, byte[] data, int dataSize, bool sendReliably)
		{
			if (this.m_status == SteamOnlineMultiplayerSessionTransportCoordinator.Status.eOpen && toSteamId.IsValid() && !toSteamId.IsLobby())
			{
				try
				{
					EP2PSend eP2PSendType = (!sendReliably) ? EP2PSend.k_EP2PSendUnreliable : EP2PSend.k_EP2PSendReliable;
					return SteamNetworking.SendP2PPacket(toSteamId, data, (uint)dataSize, eP2PSendType, 1);
				}
				catch (Exception)
				{
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x000E024C File Offset: 0x000DE64C
		public bool SendVoipData(CSteamID toSteamId, byte[] data, int dataSize)
		{
			if (this.m_status == SteamOnlineMultiplayerSessionTransportCoordinator.Status.eOpen && toSteamId.IsValid() && !toSteamId.IsLobby())
			{
				try
				{
					return SteamNetworking.SendP2PPacket(toSteamId, data, (uint)dataSize, EP2PSend.k_EP2PSendUnreliableNoDelay, 2);
				}
				catch (Exception)
				{
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x000E02A8 File Offset: 0x000DE6A8
		public void Update(byte[] receiveBuffer, uint maxIterations)
		{
			uint num = 0U;
			bool flag = false;
			do
			{
				flag = true;
				try
				{
					uint num2;
					if (receiveBuffer != null && num++ < maxIterations && SteamNetworking.IsP2PPacketAvailable(out num2, 1))
					{
						this.receiveFromSteamId.Clear();
						if (num2 > 0U && (ulong)num2 <= (ulong)((long)receiveBuffer.Length))
						{
							uint num3;
							if (SteamNetworking.ReadP2PPacket(receiveBuffer, (uint)receiveBuffer.Length, out num3, out this.receiveFromSteamId, 1))
							{
								if (num3 > 0U)
								{
									if (this.m_status == SteamOnlineMultiplayerSessionTransportCoordinator.Status.eOpen && this.receiveFromSteamId.IsValid())
									{
										this.OnDataEvent(this.receiveFromSteamId, receiveBuffer, (int)num3);
									}
									flag = false;
								}
							}
							else if (SteamNetworking.ReadP2PPacket(receiveBuffer, (uint)receiveBuffer.Length, out num3, out this.receiveFromSteamId, 2) && num3 > 0U)
							{
								if (this.m_status == SteamOnlineMultiplayerSessionTransportCoordinator.Status.eOpen && this.receiveFromSteamId.IsValid())
								{
									this.OnVoipDataEvent(this.receiveFromSteamId, receiveBuffer, (int)num3);
								}
								flag = false;
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
			while (!flag);
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x000E03B0 File Offset: 0x000DE7B0
		private void OnDisconnectEvent(CSteamID fromSteamId)
		{
			if (this.m_disconnectionCallback != null)
			{
				try
				{
					this.m_disconnectionCallback(fromSteamId);
				}
				catch (Exception)
				{
				}
				if (this.m_connectionIds.Exists((CSteamID x) => x == fromSteamId))
				{
					this.m_connectionIds.Remove(fromSteamId);
				}
			}
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x000E0430 File Offset: 0x000DE830
		private void OnDataEvent(CSteamID fromSteamId, byte[] data, int dataSizeInBytes)
		{
			if (this.m_dataCallback != null)
			{
				try
				{
					this.m_dataCallback(fromSteamId, data, dataSizeInBytes);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x000E0474 File Offset: 0x000DE874
		private void OnVoipDataEvent(CSteamID fromSteamId, byte[] data, int dataSizeInBytes)
		{
			if (this.m_voipDataCallback != null)
			{
				try
				{
					this.m_voipDataCallback(fromSteamId, data, dataSizeInBytes);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x000E04B8 File Offset: 0x000DE8B8
		private void OnSteamConnectionRequest(P2PSessionRequest_t param)
		{
			try
			{
				if (this.m_status == SteamOnlineMultiplayerSessionTransportCoordinator.Status.eOpen)
				{
					int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(this.m_lobbyId);
					for (int i = 0; i < numLobbyMembers; i++)
					{
						if (param.m_steamIDRemote == SteamMatchmaking.GetLobbyMemberByIndex(this.m_lobbyId, i))
						{
							SteamNetworking.AcceptP2PSessionWithUser(param.m_steamIDRemote);
							if (!this.m_connectionIds.Exists((CSteamID x) => x == param.m_steamIDRemote))
							{
								this.m_connectionIds.Add(param.m_steamIDRemote);
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

		// Token: 0x06002FB9 RID: 12217 RVA: 0x000E0580 File Offset: 0x000DE980
		private void OnSteamConnectionFail(P2PSessionConnectFail_t param)
		{
			try
			{
				if (this.m_status == SteamOnlineMultiplayerSessionTransportCoordinator.Status.eOpen)
				{
					this.OnDisconnectEvent(param.m_steamIDRemote);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04002641 RID: 9793
		private static Callback<P2PSessionRequest_t> s_steamTransportConnectionRequest;

		// Token: 0x04002642 RID: 9794
		private static Callback<P2PSessionConnectFail_t> s_steamTransportConnectionFail;

		// Token: 0x04002643 RID: 9795
		private bool m_isInitialized;

		// Token: 0x04002644 RID: 9796
		private SteamOnlineMultiplayerSessionTransportCoordinator.Status m_status;

		// Token: 0x04002645 RID: 9797
		private SteamOnlineMultiplayerSessionTransportCoordinator.DisconnectionCallback m_disconnectionCallback;

		// Token: 0x04002646 RID: 9798
		private SteamOnlineMultiplayerSessionTransportCoordinator.DataCallback m_dataCallback;

		// Token: 0x04002647 RID: 9799
		private SteamOnlineMultiplayerSessionTransportCoordinator.DataCallback m_voipDataCallback;

		// Token: 0x04002648 RID: 9800
		private CSteamID m_lobbyId = default(CSteamID);

		// Token: 0x04002649 RID: 9801
		private List<CSteamID> m_connectionIds = new List<CSteamID>();

		// Token: 0x0400264A RID: 9802
		private CSteamID receiveFromSteamId = default(CSteamID);

		// Token: 0x0200098C RID: 2444
		// (Invoke) Token: 0x06002FBC RID: 12220
		public delegate void DisconnectionCallback(CSteamID fromSteamId);

		// Token: 0x0200098D RID: 2445
		// (Invoke) Token: 0x06002FC0 RID: 12224
		public delegate void DataCallback(CSteamID fromSteamId, byte[] data, int dataSizeInBytes);

		// Token: 0x0200098E RID: 2446
		private enum Status : byte
		{
			// Token: 0x0400264C RID: 9804
			eIdle,
			// Token: 0x0400264D RID: 9805
			eOpen
		}

		// Token: 0x0200098F RID: 2447
		private enum Channels : byte
		{
			// Token: 0x0400264F RID: 9807
			eData = 1,
			// Token: 0x04002650 RID: 9808
			eVoip
		}
	}
}
