using System;
using System.Collections.Generic;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace Team17.Online.Multiplayer.Connection
{
	// Token: 0x0200087E RID: 2174
	public class LatencyMeasure
	{
		// Token: 0x06002A16 RID: 10774 RVA: 0x000C4C09 File Offset: 0x000C3009
		public float GetAverageOneWayTripTime()
		{
			return this.m_fAverageOneWayLatency;
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000C4C11 File Offset: 0x000C3011
		public int GetMessageSize()
		{
			return 3;
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000C4C14 File Offset: 0x000C3014
		public void Initialise(PeerBase localPeer, NetworkConnection connection, IOnlineMultiplayerSessionUserId remoteUserId, bool bReliable)
		{
			Mailbox.Server.RegisterForMessageType(MessageType.LatencyMeasure, new OrderedMessageReceivedCallback(this.OnLatencyReceived));
			Mailbox.Client.RegisterForMessageType(MessageType.LatencyMeasure, new OrderedMessageReceivedCallback(this.OnLatencyReceived));
			this.m_Writer = new BitStreamWriter(this.m_WriteBuffer);
			this.m_LocalPeer = localPeer;
			this.m_Connection = connection;
			this.m_RemoteUserId = remoteUserId;
			this.m_Tracker = this.m_LocalPeer.GetTracker();
			this.m_bReliable = bReliable;
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000C4C90 File Offset: 0x000C3090
		public void Shutdown()
		{
			Mailbox.Server.UnregisterForMessageType(MessageType.LatencyMeasure, new OrderedMessageReceivedCallback(this.OnLatencyReceived));
			Mailbox.Client.UnregisterForMessageType(MessageType.LatencyMeasure, new OrderedMessageReceivedCallback(this.OnLatencyReceived));
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000C4CC4 File Offset: 0x000C30C4
		public void OnLatencyReceived(IOnlineMultiplayerSessionUserId from, Serialisable message)
		{
			if (!this.m_Connection.GetLatencyTestPaused() && from.UniqueId == this.m_RemoteUserId.UniqueId)
			{
				LatencyMessage latencyMessage = (LatencyMessage)message;
				if (latencyMessage.m_bReliable == this.m_bReliable)
				{
					if (latencyMessage.m_Stage == LatencyMessage.Stage.Ping)
					{
						this.SendLatencyMessage(LatencyMessage.Stage.Pong, latencyMessage.m_fTime);
					}
					else if (latencyMessage.m_Stage == LatencyMessage.Stage.Pong)
					{
						this.m_fTimeTimeReceived = Time.realtimeSinceStartup;
						float num = (this.m_fTimeTimeReceived - latencyMessage.m_fTime) * 0.5f;
						if (this.m_OneWayLatencyHistory.Count >= 3)
						{
							this.m_fRunningLatencyTotal -= this.m_OneWayLatencyHistory.Dequeue();
						}
						this.m_OneWayLatencyHistory.Enqueue(num);
						this.m_fRunningLatencyTotal += num;
						this.m_fAverageOneWayLatency = this.m_fRunningLatencyTotal / (float)this.m_OneWayLatencyHistory.Count;
					}
				}
			}
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000C4DB4 File Offset: 0x000C31B4
		public bool ShouldAppendLatency(MessageBatcher batch)
		{
			if (!this.m_Connection.GetLatencyTestPaused())
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (this.m_fLastSync + 0.1f < realtimeSinceStartup)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x000C4DEC File Offset: 0x000C31EC
		public bool ShouldForceLatency()
		{
			if (!this.m_Connection.GetLatencyTestPaused())
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (this.m_fLastSync + 0.2f < realtimeSinceStartup)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000C4E24 File Offset: 0x000C3224
		public void SendLatencyMessage(LatencyMessage.Stage stage, float fTime)
		{
			this.m_TimeMessage.m_Stage = stage;
			this.m_TimeMessage.m_bReliable = this.m_bReliable;
			this.m_TimeMessage.m_fTime = fTime;
			this.m_WriteBuffer.Clear();
			this.m_Writer.Reset(this.m_WriteBuffer);
			this.m_Message.Type = MessageType.LatencyMeasure;
			this.m_Message.Payload = this.m_TimeMessage;
			this.m_Message.Serialise(this.m_Writer);
			this.m_Connection.SendMessage(this.m_WriteBuffer._items, this.m_WriteBuffer.Count, this.m_bReliable);
			if (this.m_Tracker != null)
			{
				this.m_Tracker.TrackSentGlobalEvent(MessageType.LatencyMeasure);
			}
			if (stage == LatencyMessage.Stage.Ping)
			{
				this.m_fLastSync = fTime;
			}
		}

		// Token: 0x04002126 RID: 8486
		public const int LatencyHistorySize = 3;

		// Token: 0x04002127 RID: 8487
		public const float SyncFrequencyPerSecond = 10f;

		// Token: 0x04002128 RID: 8488
		public const float SyncDelay = 0.1f;

		// Token: 0x04002129 RID: 8489
		private BitStreamWriter m_Writer;

		// Token: 0x0400212A RID: 8490
		private FastList<byte> m_WriteBuffer = new FastList<byte>(16);

		// Token: 0x0400212B RID: 8491
		private Message m_Message = new Message();

		// Token: 0x0400212C RID: 8492
		private LatencyMessage m_TimeMessage = new LatencyMessage();

		// Token: 0x0400212D RID: 8493
		private float m_fLastSync;

		// Token: 0x0400212E RID: 8494
		private float m_fTimeTimeReceived;

		// Token: 0x0400212F RID: 8495
		private float m_fAverageOneWayLatency;

		// Token: 0x04002130 RID: 8496
		private float m_fRunningLatencyTotal;

		// Token: 0x04002131 RID: 8497
		private Queue<float> m_OneWayLatencyHistory = new Queue<float>(3);

		// Token: 0x04002132 RID: 8498
		private NetworkMessageTracker m_Tracker;

		// Token: 0x04002133 RID: 8499
		private bool m_bReliable;

		// Token: 0x04002134 RID: 8500
		private PeerBase m_LocalPeer;

		// Token: 0x04002135 RID: 8501
		private NetworkConnection m_Connection;

		// Token: 0x04002136 RID: 8502
		private IOnlineMultiplayerSessionUserId m_RemoteUserId;
	}
}
