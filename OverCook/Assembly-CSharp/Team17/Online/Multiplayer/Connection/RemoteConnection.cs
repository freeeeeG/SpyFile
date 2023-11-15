using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace Team17.Online.Multiplayer.Connection
{
	// Token: 0x02000880 RID: 2176
	public class RemoteConnection : BaseConnection
	{
		// Token: 0x06002A29 RID: 10793 RVA: 0x000C50C3 File Offset: 0x000C34C3
		public override IOnlineMultiplayerSessionUserId GetRemoteSessionUserId()
		{
			return this.m_RemoteUserId;
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x000C50CC File Offset: 0x000C34CC
		public void Initialise(PeerBase localPeer, IOnlineMultiplayerSessionCoordinator sessionCoordinator, IOnlineMultiplayerSessionUserId remoteUserId)
		{
			base.Initialise();
			this.m_OutgoingUnreliable.Initialise(TechMessageType.UnreliableMessageBatch);
			this.m_OutgoingReliable.Initialise(TechMessageType.ReliableMessageBatch);
			this.m_LocalPeer = localPeer;
			this.m_RemoteUserId = remoteUserId;
			this.m_OnlineMultiplayerSessionCoordinator = sessionCoordinator;
			this.m_ReliableLatencyMeasure.Initialise(localPeer, this, remoteUserId, true);
			this.m_UnreliableLatencyMeasure.Initialise(localPeer, this, remoteUserId, false);
			this.m_Transmitter.Initialise(new Generic<bool, byte[], int, bool>(this.TransmitMessage));
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x000C5141 File Offset: 0x000C3541
		private bool TransmitMessage(byte[] data, int size, bool bReliable)
		{
			return this.m_OnlineMultiplayerSessionCoordinator.SendData(this.m_RemoteUserId, data, size, bReliable);
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x000C5157 File Offset: 0x000C3557
		public override bool SendMessage(byte[] data, int size, bool bReliable)
		{
			return this.m_OnlineMultiplayerSessionCoordinator != null && base.SendMessage(data, size, bReliable);
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x000C5170 File Offset: 0x000C3570
		public override void Dispatch()
		{
			MessageBatcherBuffer messageBatcher = this.GetMessageBatcher(true);
			this.DispatchLatencyPings(messageBatcher.GetCurrentBatcher(), this.m_ReliableLatencyMeasure);
			MessageBatcherBuffer messageBatcher2 = this.GetMessageBatcher(false);
			this.DispatchLatencyPings(messageBatcher2.GetCurrentBatcher(), this.m_UnreliableLatencyMeasure);
			if (!this.SendBufferedData(true))
			{
			}
			if (!this.SendBufferedData(false))
			{
			}
			this.m_Transmitter.Update();
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x000C51DC File Offset: 0x000C35DC
		private bool DispatchLatencyPings(MessageBatcher batch, LatencyMeasure measure)
		{
			if (batch.MessagesBatched > 0 && batch.m_BytesUsed > 0)
			{
				if (batch.GetAvailableSpace() > measure.GetMessageSize() && measure.ShouldAppendLatency(batch))
				{
					measure.SendLatencyMessage(LatencyMessage.Stage.Ping, Time.realtimeSinceStartup);
				}
			}
			else if (measure.ShouldForceLatency())
			{
				measure.SendLatencyMessage(LatencyMessage.Stage.Ping, Time.realtimeSinceStartup);
			}
			return false;
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x000C5248 File Offset: 0x000C3648
		public override ConnectionStats GetConnectionStats(bool bReliable)
		{
			this.m_Stats.m_fLatency = this.GetLatencyMeasure(bReliable).GetAverageOneWayTripTime();
			if (bReliable)
			{
				this.m_Stats.m_fMaxTimeBetweenReceives = this.m_fMaxReliableReceiveWait;
				this.m_Stats.m_fIncomingSequenceNumber = (float)this.m_IncomingReliableSequenceNumber;
				this.m_Stats.m_fOutgoingSequenceNumber = (float)this.m_OutgoingReliableSequenceNumber;
			}
			else
			{
				this.m_Stats.m_fMaxTimeBetweenReceives = this.m_fMaxUnreliableReceiveWait;
				this.m_Stats.m_fIncomingSequenceNumber = (float)this.m_IncomingUnreliableSequenceNumber;
				this.m_Stats.m_fOutgoingSequenceNumber = (float)this.m_OutgoingUnreliableSequenceNumber;
			}
			return this.m_Stats;
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x000C52E7 File Offset: 0x000C36E7
		private LatencyMeasure GetLatencyMeasure(bool bReliable)
		{
			if (bReliable)
			{
				return this.m_ReliableLatencyMeasure;
			}
			return this.m_UnreliableLatencyMeasure;
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x000C52FC File Offset: 0x000C36FC
		private bool SendBufferedData(bool bReliable)
		{
			MessageBatcherBuffer messageBatcher = this.GetMessageBatcher(bReliable);
			bool flag = true;
			NetworkMessageTracker tracker = this.m_LocalPeer.GetTracker();
			if (bReliable)
			{
				flag &= messageBatcher.Dispatch(this.m_Transmitter, tracker, ref this.m_OutgoingReliableSequenceNumber);
			}
			else
			{
				flag &= messageBatcher.Dispatch(this.m_Transmitter, tracker, ref this.m_OutgoingUnreliableSequenceNumber);
			}
			return flag;
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x000C5357 File Offset: 0x000C3757
		public override void HandleReceivedBytes(byte[] data, int size)
		{
			this.m_LocalPeer.HandleReceivedBytesFromConnection(this, data, size);
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x000C5368 File Offset: 0x000C3768
		public override void Disconnect()
		{
			this.m_LargeMessageBuffer[0] = this.m_TechHeaderWriter.SerialiseHeader(true, TechMessageType.Disconnect);
			FastList<byte> fastList = this.m_TechHeaderWriter.SerialiseSequence((uint)this.m_OutgoingUnreliableSequenceNumber);
			for (int i = 0; i < fastList.Count; i++)
			{
				this.m_LargeMessageBuffer[1 + i] = fastList._items[i];
			}
			this.m_OutgoingReliableSequenceNumber += 1;
			this.m_Transmitter.Transmit(this.m_LargeMessageBuffer, 3, true);
			this.m_LocalPeer.HandleConnectionLost(this.m_RemoteUserId, this);
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x000C53FA File Offset: 0x000C37FA
		protected override MessageBatcherBuffer GetMessageBatcher(bool bReliable)
		{
			if (bReliable)
			{
				return this.m_OutgoingReliable;
			}
			return this.m_OutgoingUnreliable;
		}

		// Token: 0x0400213B RID: 8507
		private IOnlineMultiplayerSessionCoordinator m_OnlineMultiplayerSessionCoordinator;

		// Token: 0x0400213C RID: 8508
		private IOnlineMultiplayerSessionUserId m_RemoteUserId;

		// Token: 0x0400213D RID: 8509
		protected MessageBatcherBuffer m_OutgoingUnreliable = new MessageBatcherBuffer();

		// Token: 0x0400213E RID: 8510
		protected MessageBatcherBuffer m_OutgoingReliable = new MessageBatcherBuffer();

		// Token: 0x0400213F RID: 8511
		private LatencyMeasure m_ReliableLatencyMeasure = new LatencyMeasure();

		// Token: 0x04002140 RID: 8512
		private LatencyMeasure m_UnreliableLatencyMeasure = new LatencyMeasure();
	}
}
