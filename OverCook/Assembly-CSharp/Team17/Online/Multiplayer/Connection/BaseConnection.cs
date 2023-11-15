using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace Team17.Online.Multiplayer.Connection
{
	// Token: 0x0200087B RID: 2171
	public abstract class BaseConnection : NetworkConnection
	{
		// Token: 0x060029FD RID: 10749 RVA: 0x000C473C File Offset: 0x000C2B3C
		protected void Initialise()
		{
			this.m_OutgoingReliableSequenceNumber = 0;
			this.m_OutgoingUnreliableSequenceNumber = 0;
			this.m_IncomingReliableSequenceNumber = -1;
			this.m_IncomingUnreliableSequenceNumber = -1;
			this.m_TechHeaderWriter.Initialise();
			this.m_Transmitter = this.CreateTransmitter();
			this.m_fLastUnreliablePacketReceivedTime = Time.realtimeSinceStartup + this.m_fNetworkStatsSettleTime;
			this.m_fMaxUnreliableReceiveWait = 0f;
			this.m_fLastReliablePacketReceivedTime = Time.realtimeSinceStartup + this.m_fNetworkStatsSettleTime;
			this.m_fMaxReliableReceiveWait = 0f;
		}

		// Token: 0x060029FE RID: 10750
		public abstract IOnlineMultiplayerSessionUserId GetRemoteSessionUserId();

		// Token: 0x060029FF RID: 10751
		protected abstract MessageBatcherBuffer GetMessageBatcher(bool bReliable);

		// Token: 0x06002A00 RID: 10752 RVA: 0x000C47B8 File Offset: 0x000C2BB8
		public virtual bool SendMessage(byte[] data, int size, bool bReliable)
		{
			MessageBatcherBuffer messageBatcher = this.GetMessageBatcher(bReliable);
			if (messageBatcher.IsSizeSupported(size))
			{
				return messageBatcher.AddData(data, size);
			}
			if (size < 1023)
			{
				this.TransmitLargeMessage(data, size, bReliable);
				return true;
			}
			return this.TransmitMultiPartMessage(data, size);
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000C4801 File Offset: 0x000C2C01
		public virtual void HandleReceivedBytes(byte[] data, int size)
		{
			this.m_LocalPeer.HandleReceivedBytesFromConnection(this, data, size);
		}

		// Token: 0x06002A02 RID: 10754
		public abstract void Dispatch();

		// Token: 0x06002A03 RID: 10755
		public abstract void Disconnect();

		// Token: 0x06002A04 RID: 10756
		public abstract ConnectionStats GetConnectionStats(bool bReliable);

		// Token: 0x06002A05 RID: 10757 RVA: 0x000C4811 File Offset: 0x000C2C11
		public virtual void SetLatencyTestPaused(bool paused)
		{
			this.m_LatencyPaused = paused;
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000C481A File Offset: 0x000C2C1A
		public virtual bool GetLatencyTestPaused()
		{
			return this.m_LatencyPaused;
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x000C4822 File Offset: 0x000C2C22
		protected virtual MessageTransmitter CreateTransmitter()
		{
			return new MessageTransmitter();
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000C482C File Offset: 0x000C2C2C
		protected bool TransmitLargeMessage(byte[] data, int size, bool bReliable)
		{
			if (bReliable)
			{
				this.m_LargeMessageBuffer[0] = this.m_TechHeaderWriter.SerialiseHeader(true, TechMessageType.ReliableGameMessage);
				FastList<byte> fastList = this.m_TechHeaderWriter.SerialiseSequence((uint)this.m_OutgoingReliableSequenceNumber);
				for (int i = 0; i < fastList.Count; i++)
				{
					this.m_LargeMessageBuffer[1 + i] = fastList._items[i];
				}
				this.m_OutgoingReliableSequenceNumber += 1;
			}
			else
			{
				this.m_LargeMessageBuffer[0] = this.m_TechHeaderWriter.SerialiseHeader(true, TechMessageType.UnreliableGameMessage);
				FastList<byte> fastList2 = this.m_TechHeaderWriter.SerialiseSequence((uint)this.m_OutgoingUnreliableSequenceNumber);
				for (int j = 0; j < fastList2.Count; j++)
				{
					this.m_LargeMessageBuffer[1 + j] = fastList2._items[j];
				}
				this.m_OutgoingUnreliableSequenceNumber += 1;
			}
			Array.Copy(data, 0, this.m_LargeMessageBuffer, 3, size);
			return this.m_Transmitter.Transmit(this.m_LargeMessageBuffer, size + 3, bReliable);
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000C4928 File Offset: 0x000C2D28
		protected bool TransmitMultiPartMessage(byte[] data, int size)
		{
			bool flag = true;
			int num = 1020;
			byte b = 2;
			b += (byte)((size - num) / 1021);
			int num2 = 3;
			int num3 = 0;
			this.m_LargeMessageBuffer[0] = this.m_TechHeaderWriter.SerialiseHeader(true, TechMessageType.ReliableMultiPart);
			FastList<byte> fastList = this.m_TechHeaderWriter.SerialiseSequence((uint)this.m_OutgoingReliableSequenceNumber);
			for (int i = 0; i < fastList.Count; i++)
			{
				this.m_LargeMessageBuffer[1 + i] = fastList._items[i];
			}
			this.m_LargeMessageBuffer[3] = b;
			this.m_OutgoingReliableSequenceNumber += 1;
			int num4 = 1024 - num2;
			int num5 = size;
			int num6;
			if (num5 > num4)
			{
				num5 = num4;
				num6 = size - num4;
			}
			else
			{
				num6 = 0;
			}
			Array.Copy(data, num3, this.m_LargeMessageBuffer, num2, num5);
			num3 += num5;
			int num7 = 0;
			flag |= this.m_Transmitter.Transmit(this.m_LargeMessageBuffer, num2 + num5, true);
			num7++;
			num2 = 2;
			while (flag && num6 > 0)
			{
				this.m_LargeMessageBuffer[0] = this.m_TechHeaderWriter.SerialiseHeader(true, TechMessageType.ReliableMultiPart);
				fastList = this.m_TechHeaderWriter.SerialiseSequence((uint)this.m_OutgoingReliableSequenceNumber);
				for (int j = 0; j < fastList.Count; j++)
				{
					this.m_LargeMessageBuffer[1 + j] = fastList._items[j];
				}
				this.m_OutgoingReliableSequenceNumber += 1;
				num4 = 1024 - num2;
				num5 = num6;
				if (num5 > num4)
				{
					num5 = num4;
					num6 -= num4;
				}
				else
				{
					num6 = 0;
				}
				Array.Copy(data, num3, this.m_LargeMessageBuffer, num2, num5);
				num3 += num5;
				flag |= this.m_Transmitter.Transmit(this.m_LargeMessageBuffer, num2 + num5, true);
				num7++;
			}
			return flag;
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x000C4B08 File Offset: 0x000C2F08
		public virtual bool CheckReceivedSequenceNumber(bool bReliable, uint sequenceNumber)
		{
			bool result = true;
			if (bReliable)
			{
				int num = this.m_IncomingReliableSequenceNumber + 1;
				if (num > 65535)
				{
					num = 0;
				}
				if (this.m_IncomingReliableSequenceNumber == -1 || (long)num == (long)((ulong)sequenceNumber))
				{
					this.m_IncomingReliableSequenceNumber = (int)sequenceNumber;
				}
				else
				{
					result = false;
					this.Disconnect();
				}
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				this.m_fMaxReliableReceiveWait = Mathf.Max(realtimeSinceStartup - this.m_fLastReliablePacketReceivedTime, this.m_fMaxReliableReceiveWait);
				this.m_fLastReliablePacketReceivedTime = realtimeSinceStartup;
			}
			else
			{
				this.TrackReceivedUnreliableMessage(this.m_IncomingUnreliableSequenceNumber, (int)sequenceNumber);
				this.m_IncomingUnreliableSequenceNumber = (int)sequenceNumber;
			}
			return result;
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000C4B9C File Offset: 0x000C2F9C
		private void TrackReceivedUnreliableMessage(int iLastSequence, int iCurrentSequence)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			this.m_fMaxUnreliableReceiveWait = Mathf.Max(realtimeSinceStartup - this.m_fLastUnreliablePacketReceivedTime, this.m_fMaxUnreliableReceiveWait);
			this.m_fLastUnreliablePacketReceivedTime = realtimeSinceStartup;
		}

		// Token: 0x04002110 RID: 8464
		public const ushort MaxSequenceValue = 65535;

		// Token: 0x04002111 RID: 8465
		public const ushort MaxSequenceJump = 32767;

		// Token: 0x04002112 RID: 8466
		public const int UninitialisedSequence = -1;

		// Token: 0x04002113 RID: 8467
		protected float m_fLastUnreliablePacketReceivedTime;

		// Token: 0x04002114 RID: 8468
		protected float m_fMaxUnreliableReceiveWait;

		// Token: 0x04002115 RID: 8469
		protected float m_fLastReliablePacketReceivedTime;

		// Token: 0x04002116 RID: 8470
		protected float m_fMaxReliableReceiveWait;

		// Token: 0x04002117 RID: 8471
		private float m_fNetworkStatsSettleTime = 5f;

		// Token: 0x04002118 RID: 8472
		protected ConnectionStats m_Stats;

		// Token: 0x04002119 RID: 8473
		protected TechHeaderWriter m_TechHeaderWriter = new TechHeaderWriter();

		// Token: 0x0400211A RID: 8474
		protected byte[] m_LargeMessageBuffer = new byte[1024];

		// Token: 0x0400211B RID: 8475
		protected ushort m_OutgoingReliableSequenceNumber;

		// Token: 0x0400211C RID: 8476
		protected ushort m_OutgoingUnreliableSequenceNumber;

		// Token: 0x0400211D RID: 8477
		protected int m_IncomingReliableSequenceNumber = -1;

		// Token: 0x0400211E RID: 8478
		protected int m_IncomingUnreliableSequenceNumber = -1;

		// Token: 0x0400211F RID: 8479
		protected NetworkPeer m_LocalPeer;

		// Token: 0x04002120 RID: 8480
		protected MessageTransmitter m_Transmitter;

		// Token: 0x04002121 RID: 8481
		protected bool m_LatencyPaused;
	}
}
