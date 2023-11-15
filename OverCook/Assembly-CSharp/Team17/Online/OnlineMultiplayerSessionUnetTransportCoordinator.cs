using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UnityEngine.Networking;

namespace Team17.Online
{
	// Token: 0x02000963 RID: 2403
	public class OnlineMultiplayerSessionUnetTransportCoordinator
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06002EFF RID: 12031 RVA: 0x000DBECB File Offset: 0x000DA2CB
		public static uint DisconnectionTimeInSeconds
		{
			get
			{
				return 7U;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06002F00 RID: 12032 RVA: 0x000DBECE File Offset: 0x000DA2CE
		public bool IsIdle
		{
			get
			{
				return OnlineMultiplayerSessionUnetTransportCoordinator.Status.eIdle == this.m_status;
			}
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000DBEDC File Offset: 0x000DA2DC
		public bool Open(ushort gamePort, uint numConnections, bool usePlatformProtocols, OnlineMultiplayerSessionUnetTransportCoordinator.ConnectionCallback connectionCallback, OnlineMultiplayerSessionUnetTransportCoordinator.DisconnectionCallback disconnectionCallback, OnlineMultiplayerSessionUnetTransportCoordinator.DataCallback dataCallback, OnlineMultiplayerSessionUnetTransportCoordinator.DataCallback voipDataCallback)
		{
			if (this.m_status != OnlineMultiplayerSessionUnetTransportCoordinator.Status.eIdle)
			{
				NetworkTransport.RemoveHost(this.m_hostId);
				NetworkTransport.Shutdown();
				this.m_connectionIds.Clear();
				this.m_shutdownStopwatch.Stop();
				this.m_connectionCallback = null;
				this.m_disconnectionCallback = null;
				this.m_dataCallback = null;
				this.m_hostId = 0;
				this.m_reliableChannelId = 0;
				this.m_unreliableChannelId = 0;
				this.m_voipChannelId = 0;
				this.m_status = OnlineMultiplayerSessionUnetTransportCoordinator.Status.eIdle;
			}
			if (connectionCallback != null && disconnectionCallback != null && dataCallback != null && voipDataCallback != null)
			{
				try
				{
					this.m_connectionCallback = connectionCallback;
					this.m_disconnectionCallback = disconnectionCallback;
					this.m_dataCallback = dataCallback;
					this.m_voipDataCallback = voipDataCallback;
					NetworkTransport.Init();
					ushort num = (ushort)OnlineMultiplayerConfig.MaxTransportMessageSize;
					num += this.m_unetPacketOverhead;
					ConnectionConfig connectionConfig = new ConnectionConfig();
					connectionConfig.UsePlatformSpecificProtocols = usePlatformProtocols;
					connectionConfig.DisconnectTimeout = OnlineMultiplayerSessionUnetTransportCoordinator.DisconnectionTimeInSeconds * 1000U;
					connectionConfig.PacketSize = num;
					connectionConfig.SendDelay = 1U;
					this.m_reliableChannelId = connectionConfig.AddChannel(QosType.ReliableSequenced);
					this.m_unreliableChannelId = connectionConfig.AddChannel(QosType.Unreliable);
					this.m_voipChannelId = connectionConfig.AddChannel(QosType.Unreliable);
					HostTopology topology = new HostTopology(connectionConfig, (int)numConnections);
					this.m_hostId = NetworkTransport.AddHost(topology, (int)gamePort);
					this.m_status = OnlineMultiplayerSessionUnetTransportCoordinator.Status.eOpen;
					return true;
				}
				catch (Exception ex)
				{
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x000DC038 File Offset: 0x000DA438
		public void Close()
		{
			if (this.m_status == OnlineMultiplayerSessionUnetTransportCoordinator.Status.eOpen)
			{
				for (int i = 0; i < this.m_connectionIds.Count; i++)
				{
					byte b;
					NetworkTransport.Disconnect(this.m_hostId, this.m_connectionIds[i], out b);
				}
				this.m_status = OnlineMultiplayerSessionUnetTransportCoordinator.Status.eClosing;
				this.m_shutdownStopwatch.Reset();
				this.m_shutdownStopwatch.Start();
			}
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000DC0A4 File Offset: 0x000DA4A4
		public int OpenConnection(EndPoint endpoint)
		{
			if (this.m_status == OnlineMultiplayerSessionUnetTransportCoordinator.Status.eOpen && endpoint != null)
			{
				byte b;
				int result = NetworkTransport.ConnectEndPoint(this.m_hostId, endpoint, 0, out b);
				if (b == 0)
				{
					return result;
				}
			}
			return OnlineMultiplayerSessionUnetTransportCoordinator.s_InvalidConnectionId;
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000DC0E0 File Offset: 0x000DA4E0
		public void CloseConnection(int connectionId)
		{
			if (this.m_status == OnlineMultiplayerSessionUnetTransportCoordinator.Status.eOpen && OnlineMultiplayerSessionUnetTransportCoordinator.s_InvalidConnectionId != connectionId && this.m_connectionIds.Exists((int x) => x == connectionId))
			{
				byte b;
				NetworkTransport.Disconnect(this.m_hostId, connectionId, out b);
				this.m_connectionIds.Remove(connectionId);
			}
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x000DC158 File Offset: 0x000DA558
		public bool SendData(int unetConnectionId, byte[] data, int dataSize, bool sendReliably)
		{
			if (this.m_status == OnlineMultiplayerSessionUnetTransportCoordinator.Status.eOpen && OnlineMultiplayerSessionUnetTransportCoordinator.s_InvalidConnectionId != unetConnectionId)
			{
				byte channelId = (!sendReliably) ? this.m_unreliableChannelId : this.m_reliableChannelId;
				byte b;
				return NetworkTransport.Send(this.m_hostId, unetConnectionId, (int)channelId, data, dataSize, out b);
			}
			return false;
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x000DC1A8 File Offset: 0x000DA5A8
		public bool SendVoipData(int unetConnectionId, byte[] data, int dataSize)
		{
			byte b;
			return this.m_status == OnlineMultiplayerSessionUnetTransportCoordinator.Status.eOpen && OnlineMultiplayerSessionUnetTransportCoordinator.s_InvalidConnectionId != unetConnectionId && NetworkTransport.Send(this.m_hostId, unetConnectionId, (int)this.m_voipChannelId, data, dataSize, out b);
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000DC1E4 File Offset: 0x000DA5E4
		public void Update(byte[] receiveBuffer, uint maxIterations)
		{
			uint num = 0U;
			bool flag;
			do
			{
				flag = true;
				if (receiveBuffer != null && this.m_status == OnlineMultiplayerSessionUnetTransportCoordinator.Status.eOpen && num++ < maxIterations)
				{
					int num2;
					int connectionId;
					int num3;
					int dataSizeInBytes;
					byte b;
					NetworkEventType networkEventType = NetworkTransport.Receive(out num2, out connectionId, out num3, receiveBuffer, receiveBuffer.Length, out dataSizeInBytes, out b);
					if (b == 0)
					{
						if (num3 == (int)this.m_voipChannelId)
						{
							this.OnVoipDataEvent(connectionId, receiveBuffer, dataSizeInBytes);
						}
						else
						{
							switch (networkEventType)
							{
							case NetworkEventType.DataEvent:
								if (num2 == this.m_hostId)
								{
									this.OnDataEvent(connectionId, receiveBuffer, dataSizeInBytes);
								}
								flag = false;
								break;
							case NetworkEventType.ConnectEvent:
								if (num2 == this.m_hostId)
								{
									this.OnConnectEvent(connectionId);
								}
								flag = false;
								break;
							case NetworkEventType.DisconnectEvent:
								if (num2 == this.m_hostId)
								{
									this.OnDisconnectEvent(connectionId);
								}
								flag = false;
								break;
							}
						}
					}
				}
			}
			while (!flag);
			if (this.m_status == OnlineMultiplayerSessionUnetTransportCoordinator.Status.eClosing && this.m_shutdownStopwatch.ElapsedMilliseconds >= this.m_shutdownDelayTimeInMilliseconds)
			{
				NetworkTransport.RemoveHost(this.m_hostId);
				NetworkTransport.Shutdown();
				this.m_connectionIds.Clear();
				this.m_shutdownStopwatch.Stop();
				this.m_connectionCallback = null;
				this.m_disconnectionCallback = null;
				this.m_dataCallback = null;
				this.m_hostId = 0;
				this.m_reliableChannelId = 0;
				this.m_unreliableChannelId = 0;
				this.m_voipChannelId = 0;
				this.m_status = OnlineMultiplayerSessionUnetTransportCoordinator.Status.eIdle;
			}
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000DC34C File Offset: 0x000DA74C
		private void OnConnectEvent(int connectionId)
		{
			if (this.m_connectionCallback != null)
			{
				if (!this.m_connectionIds.Exists((int x) => x == connectionId))
				{
					this.m_connectionIds.Add(connectionId);
				}
				try
				{
					this.m_connectionCallback(connectionId);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000DC3CC File Offset: 0x000DA7CC
		private void OnDisconnectEvent(int connectionId)
		{
			if (this.m_disconnectionCallback != null)
			{
				try
				{
					this.m_disconnectionCallback(connectionId);
				}
				catch (Exception)
				{
				}
				if (this.m_connectionIds.Exists((int x) => x == connectionId))
				{
					this.m_connectionIds.Remove(connectionId);
				}
			}
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000DC44C File Offset: 0x000DA84C
		private void OnDataEvent(int connectionId, byte[] data, int dataSizeInBytes)
		{
			if (this.m_dataCallback != null)
			{
				try
				{
					this.m_dataCallback(connectionId, data, dataSizeInBytes);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x000DC490 File Offset: 0x000DA890
		private void OnVoipDataEvent(int connectionId, byte[] data, int dataSizeInBytes)
		{
			if (this.m_voipDataCallback != null)
			{
				try
				{
					this.m_voipDataCallback(connectionId, data, dataSizeInBytes);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x04002592 RID: 9618
		public static readonly int s_InvalidConnectionId;

		// Token: 0x04002593 RID: 9619
		private readonly long m_shutdownDelayTimeInMilliseconds = 1000L;

		// Token: 0x04002594 RID: 9620
		private readonly ushort m_unetPacketOverhead = 100;

		// Token: 0x04002595 RID: 9621
		private OnlineMultiplayerSessionUnetTransportCoordinator.Status m_status;

		// Token: 0x04002596 RID: 9622
		private int m_hostId;

		// Token: 0x04002597 RID: 9623
		private List<int> m_connectionIds = new List<int>();

		// Token: 0x04002598 RID: 9624
		private OnlineMultiplayerSessionUnetTransportCoordinator.ConnectionCallback m_connectionCallback;

		// Token: 0x04002599 RID: 9625
		private OnlineMultiplayerSessionUnetTransportCoordinator.DisconnectionCallback m_disconnectionCallback;

		// Token: 0x0400259A RID: 9626
		private OnlineMultiplayerSessionUnetTransportCoordinator.DataCallback m_dataCallback;

		// Token: 0x0400259B RID: 9627
		private OnlineMultiplayerSessionUnetTransportCoordinator.DataCallback m_voipDataCallback;

		// Token: 0x0400259C RID: 9628
		private Stopwatch m_shutdownStopwatch = new Stopwatch();

		// Token: 0x0400259D RID: 9629
		private byte m_reliableChannelId;

		// Token: 0x0400259E RID: 9630
		private byte m_unreliableChannelId;

		// Token: 0x0400259F RID: 9631
		private byte m_voipChannelId;

		// Token: 0x02000964 RID: 2404
		// (Invoke) Token: 0x06002F0E RID: 12046
		public delegate void ConnectionCallback(int connectionId);

		// Token: 0x02000965 RID: 2405
		// (Invoke) Token: 0x06002F12 RID: 12050
		public delegate void DisconnectionCallback(int connectionId);

		// Token: 0x02000966 RID: 2406
		// (Invoke) Token: 0x06002F16 RID: 12054
		public delegate void DataCallback(int connectionId, byte[] data, int dataSiszeInBytes);

		// Token: 0x02000967 RID: 2407
		private enum Status
		{
			// Token: 0x040025A1 RID: 9633
			eIdle,
			// Token: 0x040025A2 RID: 9634
			eOpen,
			// Token: 0x040025A3 RID: 9635
			eClosing
		}
	}
}
