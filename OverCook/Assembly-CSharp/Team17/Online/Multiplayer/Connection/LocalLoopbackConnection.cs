using System;
using Team17.Online.Multiplayer.Messaging;

namespace Team17.Online.Multiplayer.Connection
{
	// Token: 0x0200087F RID: 2175
	public class LocalLoopbackConnection : BaseConnection
	{
		// Token: 0x06002A1F RID: 10783 RVA: 0x000C4F14 File Offset: 0x000C3314
		public void Initialise(NetworkPeer localPeer, NetworkConnection remoteConnection)
		{
			base.Initialise();
			for (int i = 0; i < this.m_OutgoingUnreliable.Length; i++)
			{
				this.m_OutgoingUnreliable[i] = new MessageBatcherBuffer();
				this.m_OutgoingUnreliable[i].Initialise(TechMessageType.UnreliableMessageBatch);
			}
			for (int j = 0; j < this.m_OutgoingReliable.Length; j++)
			{
				this.m_OutgoingReliable[j] = new MessageBatcherBuffer();
				this.m_OutgoingReliable[j].Initialise(TechMessageType.ReliableMessageBatch);
			}
			this.m_LocalPeer = localPeer;
			this.m_RemoteConnection = remoteConnection;
			this.m_Transmitter.Initialise(new Generic<bool, byte[], int, bool>(this.TransmitMessage));
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x000C4FB4 File Offset: 0x000C33B4
		private bool TransmitMessage(byte[] data, int size, bool bReliable)
		{
			this.m_RemoteConnection.HandleReceivedBytes(data, size);
			return true;
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x000C4FC4 File Offset: 0x000C33C4
		public override IOnlineMultiplayerSessionUserId GetRemoteSessionUserId()
		{
			return null;
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x000C4FC7 File Offset: 0x000C33C7
		public override ConnectionStats GetConnectionStats(bool bReliable)
		{
			return this.m_Stats;
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x000C4FD0 File Offset: 0x000C33D0
		public override void Dispatch()
		{
			MessageBatcherBuffer messageBatcher = this.GetMessageBatcher(true);
			MessageBatcherBuffer messageBatcher2 = this.GetMessageBatcher(false);
			this.SwitchBatchBuffers();
			messageBatcher.Dispatch(this.m_Transmitter, null, ref this.m_OutgoingReliableSequenceNumber);
			messageBatcher2.Dispatch(this.m_Transmitter, null, ref this.m_OutgoingUnreliableSequenceNumber);
			this.m_Transmitter.Update();
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x000C5026 File Offset: 0x000C3426
		public override void Disconnect()
		{
			this.m_LocalPeer.HandleLocalLoopbackConnectionLost(this);
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x000C5034 File Offset: 0x000C3434
		public override bool SendMessage(byte[] data, int size, bool bReliable)
		{
			return this.m_RemoteConnection != null && base.SendMessage(data, size, bReliable);
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000C504C File Offset: 0x000C344C
		protected override MessageBatcherBuffer GetMessageBatcher(bool bReliable)
		{
			if (bReliable)
			{
				return this.m_OutgoingReliable[this.m_iOutgoingMessageBatchers];
			}
			return this.m_OutgoingUnreliable[this.m_iOutgoingMessageBatchers];
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x000C506F File Offset: 0x000C346F
		protected void SwitchBatchBuffers()
		{
			if (this.m_iOutgoingMessageBatchers == 0)
			{
				this.m_iOutgoingMessageBatchers = 1;
			}
			else
			{
				this.m_iOutgoingMessageBatchers = 0;
			}
		}

		// Token: 0x04002137 RID: 8503
		protected NetworkConnection m_RemoteConnection;

		// Token: 0x04002138 RID: 8504
		private int m_iOutgoingMessageBatchers;

		// Token: 0x04002139 RID: 8505
		private MessageBatcherBuffer[] m_OutgoingUnreliable = new MessageBatcherBuffer[2];

		// Token: 0x0400213A RID: 8506
		private MessageBatcherBuffer[] m_OutgoingReliable = new MessageBatcherBuffer[2];
	}
}
