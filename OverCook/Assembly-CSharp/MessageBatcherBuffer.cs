using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Connection;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200089F RID: 2207
public class MessageBatcherBuffer
{
	// Token: 0x06002B0A RID: 11018 RVA: 0x000C9DD4 File Offset: 0x000C81D4
	public void Initialise(TechMessageType type)
	{
		this.m_Type = type;
		for (int i = 0; i < this.m_Batchers.Capacity; i++)
		{
			this.AddBatcher();
		}
		this.Reset();
	}

	// Token: 0x06002B0B RID: 11019 RVA: 0x000C9E14 File Offset: 0x000C8214
	public void Reset()
	{
		if (this.m_Batchers.Count == 0)
		{
			this.m_CurrentBatcher = this.AddBatcher();
		}
		else
		{
			this.m_CurrentBatcher = this.m_Batchers._items[0];
			for (int i = 0; i < this.m_Batchers.Count; i++)
			{
				this.m_Batchers._items[i].Reset(this.m_Type);
			}
		}
		this.m_CurrentIndex = 0;
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x000C9E90 File Offset: 0x000C8290
	public bool IsSizeSupported(int size)
	{
		return this.m_CurrentBatcher.IsSizeSupported(size);
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x000C9EA0 File Offset: 0x000C82A0
	public bool Dispatch(MessageTransmitter transmitter, NetworkMessageTracker tracker, ref ushort sequence)
	{
		bool flag = true;
		bool flag2 = this.m_Type == TechMessageType.ReliableMessageBatch || this.m_Type == TechMessageType.ReliableGameMessage;
		for (int i = 0; i < this.m_Batchers.Count; i++)
		{
			MessageBatcher messageBatcher = this.m_Batchers._items[i];
			if (messageBatcher.MessagesBatched <= 0)
			{
				break;
			}
			messageBatcher.SetSequenceNumber((uint)sequence);
			sequence += 1;
			flag &= transmitter.Transmit(messageBatcher.m_PendingSendBuffer, messageBatcher.m_BytesUsed, flag2);
			if (tracker != null)
			{
				if (flag2)
				{
					tracker.TrackSentMessageBatch(NetworkMessageTracker.MessageBatchType.Reliable, messageBatcher.MessagesBatched);
				}
				else
				{
					tracker.TrackSentMessageBatch(NetworkMessageTracker.MessageBatchType.Unreliable, messageBatcher.MessagesBatched);
				}
			}
		}
		this.Reset();
		return flag;
	}

	// Token: 0x06002B0E RID: 11022 RVA: 0x000C9F5E File Offset: 0x000C835E
	public MessageBatcher GetCurrentBatcher()
	{
		return this.m_CurrentBatcher;
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x000C9F68 File Offset: 0x000C8368
	public bool AddData(byte[] data, int size)
	{
		if (this.m_CurrentBatcher.AddData(data, size))
		{
			return true;
		}
		this.m_CurrentIndex++;
		if (this.m_CurrentIndex < this.m_Batchers.Count)
		{
			this.m_CurrentBatcher = this.m_Batchers._items[this.m_CurrentIndex];
			if (this.m_CurrentBatcher == null)
			{
				this.m_Batchers._items[this.m_CurrentIndex] = new MessageBatcher();
				this.m_CurrentBatcher = this.m_Batchers._items[this.m_CurrentIndex];
				this.m_CurrentBatcher.Initialise(this.m_Type);
			}
		}
		else
		{
			this.m_CurrentBatcher = this.AddBatcher();
		}
		return this.m_CurrentBatcher.AddData(data, size);
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x000CA030 File Offset: 0x000C8430
	private MessageBatcher AddBatcher()
	{
		if (this.m_Batchers.Count == this.m_Batchers.Capacity)
		{
		}
		MessageBatcher messageBatcher = new MessageBatcher();
		messageBatcher.Initialise(this.m_Type);
		this.m_Batchers.Add(messageBatcher);
		return messageBatcher;
	}

	// Token: 0x04002202 RID: 8706
	private TechMessageType m_Type;

	// Token: 0x04002203 RID: 8707
	private int m_CurrentIndex = -1;

	// Token: 0x04002204 RID: 8708
	private MessageBatcher m_CurrentBatcher;

	// Token: 0x04002205 RID: 8709
	private FastList<MessageBatcher> m_Batchers = new FastList<MessageBatcher>(4);
}
