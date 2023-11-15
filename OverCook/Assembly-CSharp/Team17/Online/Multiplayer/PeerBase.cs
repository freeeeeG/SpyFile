using System;
using System.Diagnostics;
using BitStream;
using Team17.Online.Multiplayer.Connection;
using Team17.Online.Multiplayer.Messaging;

namespace Team17.Online.Multiplayer
{
	// Token: 0x020008F0 RID: 2288
	public abstract class PeerBase : NetworkPeer
	{
		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06002C80 RID: 11392 RVA: 0x000BEBD8 File Offset: 0x000BCFD8
		// (remove) Token: 0x06002C81 RID: 11393 RVA: 0x000BEC10 File Offset: 0x000BD010
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event GenericVoid<IOnlineMultiplayerSessionUserId, MessageType, Serialisable, uint, bool> OnMessageReceived;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06002C82 RID: 11394 RVA: 0x000BEC48 File Offset: 0x000BD048
		// (remove) Token: 0x06002C83 RID: 11395 RVA: 0x000BEC80 File Offset: 0x000BD080
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event GenericVoid OnLeftSession;

		// Token: 0x06002C84 RID: 11396 RVA: 0x000BECB8 File Offset: 0x000BD0B8
		public void HandleReceivedBytesFromConnection(NetworkConnection connection, byte[] data, int size)
		{
			if (this.OnMessageReceived != null)
			{
				BitStreamReader bitStreamReader = new BitStreamReader(data);
				this.m_TechHeader.Deserialize(bitStreamReader);
				if (this.m_TechHeader.IsGameMessage)
				{
					if (this.m_TechHeader.MessageTypeId == 5)
					{
						this.HandleDisconnectMessage(connection);
					}
					else
					{
						bool bReliable = this.m_TechHeader.MessageTypeId == 0 || this.m_TechHeader.MessageTypeId == 2 || this.m_TechHeader.MessageTypeId == 4;
						ushort num = bitStreamReader.ReadUInt16(16);
						bool flag = connection.CheckReceivedSequenceNumber(bReliable, (uint)num);
						if (flag)
						{
							if (this.m_TechHeader.MessageTypeId == 0 || this.m_TechHeader.MessageTypeId == 1)
							{
								this.HandleReceivedBatchMessage(bitStreamReader, connection, bReliable, size, (uint)num);
							}
							else if (this.m_TechHeader.MessageTypeId == 4)
							{
								this.HandleReceivedMultiPart(bitStreamReader, data, connection, bReliable, size, (uint)num);
							}
							else
							{
								this.HandleReceivedGameMessage(bitStreamReader, connection, bReliable, size, (uint)num);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x000BEDBC File Offset: 0x000BD1BC
		private void HandleReceivedMultiPart(BitStreamReader reader, byte[] data, NetworkConnection connection, bool bReliable, int gameMessageSize, uint sequence)
		{
			if (this.m_RemainingMultiPartMessages == -1)
			{
				this.m_RemainingMultiPartMessages = (int)reader.ReadByte(8);
				this.m_MultiPartBufferWritePosition = 0;
			}
			int num = reader.CurrentIndex + 1;
			int num2 = data.Length - num;
			Array.Copy(data, num, this.m_MultiPartMessageData, this.m_MultiPartBufferWritePosition, num2);
			this.m_MultiPartBufferWritePosition += num2;
			this.m_RemainingMultiPartMessages--;
			if (this.m_RemainingMultiPartMessages == 0)
			{
				BitStreamReader reader2 = new BitStreamReader(this.m_MultiPartMessageData);
				this.HandleReceivedGameMessage(reader2, connection, bReliable, this.m_MultiPartBufferWritePosition, sequence);
				this.m_MultiPartBufferWritePosition = -1;
				this.m_RemainingMultiPartMessages = -1;
			}
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x000BEE60 File Offset: 0x000BD260
		private void HandleReceivedGameMessage(BitStreamReader reader, NetworkConnection connection, bool bReliable, int gameMessageSize, uint sequence)
		{
			if (this.m_CurrentMessage.Deserialise(reader))
			{
				if (this.m_Tracker != null)
				{
					this.m_Tracker.TrackReceivedGlobalEvent(this.m_CurrentMessage.Type);
				}
				try
				{
					this.OnMessageReceived(connection.GetRemoteSessionUserId(), this.m_CurrentMessage.Type, this.m_CurrentMessage.Payload, sequence, bReliable);
				}
				catch (Exception e)
				{
					ExceptionManager exceptionManager = GameUtils.RequestManager<ExceptionManager>();
					if (null != exceptionManager)
					{
						exceptionManager.LogACaughtException(e, "Exception caught when processing received message. " + NetworkUtils.GetNetworkMessageDescription(this.m_CurrentMessage));
					}
				}
				this.iRunningCount++;
			}
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x000BEF28 File Offset: 0x000BD328
		private void HandleReceivedBatchMessage(BitStreamReader reader, NetworkConnection connection, bool bReliable, int size, uint sequence)
		{
			this.iRunningCount = 0;
			while (reader.CurrentIndex + 1 < size)
			{
				reader.AdvanceToNextByteBoundary();
				int currentIndex = reader.CurrentIndex;
				int num = (int)reader.ReadUInt32(8);
				int num2 = currentIndex + num;
				this.HandleReceivedGameMessage(reader, connection, bReliable, num, sequence);
				if (reader.CurrentIndex != num2)
				{
					reader.SkipToByteIndex(num2 + 1);
				}
			}
			if (this.m_Tracker != null && connection.GetRemoteSessionUserId() != null)
			{
				this.m_Tracker.TrackReceivedMessageBatch((!bReliable) ? NetworkMessageTracker.MessageBatchType.Unreliable : NetworkMessageTracker.MessageBatchType.Reliable, this.iRunningCount);
			}
		}

		// Token: 0x06002C88 RID: 11400
		public abstract void Dispatch();

		// Token: 0x06002C89 RID: 11401
		public abstract ConnectionStats GetConnectionStats(bool bReliable);

		// Token: 0x06002C8A RID: 11402 RVA: 0x000BEFBE File Offset: 0x000BD3BE
		public void HandleManuallyDeserialisedMessage(IOnlineMultiplayerSessionUserId sessionUserId, MessageType type, Serialisable message)
		{
			if (this.OnMessageReceived != null)
			{
				this.OnMessageReceived(sessionUserId, type, message, uint.MaxValue, true);
			}
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x000BEFDB File Offset: 0x000BD3DB
		public void SetTracker(NetworkMessageTracker tracker)
		{
			this.m_Tracker = tracker;
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x000BEFE4 File Offset: 0x000BD3E4
		public NetworkMessageTracker GetTracker()
		{
			return this.m_Tracker;
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x000BEFEC File Offset: 0x000BD3EC
		public void LeaveSession(IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator)
		{
			m_iOnlineMultiplayerSessionCoordinator.Leave();
			if (this.OnLeftSession != null)
			{
				this.OnLeftSession();
			}
		}

		// Token: 0x06002C8E RID: 11406
		public abstract void HandleConnectionLost(IOnlineMultiplayerSessionUserId sessionUserId, NetworkConnection connection);

		// Token: 0x06002C8F RID: 11407
		public abstract void HandleLocalLoopbackConnectionLost(NetworkConnection connection);

		// Token: 0x06002C90 RID: 11408
		public abstract void HandleDisconnectMessage(NetworkConnection connection);

		// Token: 0x06002C91 RID: 11409
		public abstract void SetLatencyTestPaused(bool paused);

		// Token: 0x040023D1 RID: 9169
		private int iRunningCount;

		// Token: 0x040023D2 RID: 9170
		protected Message m_CurrentMessage = new Message();

		// Token: 0x040023D3 RID: 9171
		private OnlineMultiplayerSessionTransportMessageHeader m_TechHeader = new OnlineMultiplayerSessionTransportMessageHeader();

		// Token: 0x040023D4 RID: 9172
		protected NetworkMessageTracker m_Tracker;

		// Token: 0x040023D5 RID: 9173
		private byte[] m_MultiPartMessageData = new byte[262144];

		// Token: 0x040023D6 RID: 9174
		private int m_MultiPartBufferWritePosition = -1;

		// Token: 0x040023D7 RID: 9175
		private int m_RemainingMultiPartMessages = -1;
	}
}
