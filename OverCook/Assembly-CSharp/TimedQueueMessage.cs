using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200015D RID: 349
public class TimedQueueMessage : Serialisable
{
	// Token: 0x0600062D RID: 1581 RVA: 0x0002C275 File Offset: 0x0002A675
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_msgType, 1);
		if (this.m_msgType == TimedQueueMessage.MsgType.QueueEvent)
		{
			writer.Write((uint)this.m_index, 4);
			writer.Write(this.m_time);
		}
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0002C2A8 File Offset: 0x0002A6A8
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_msgType = (TimedQueueMessage.MsgType)reader.ReadUInt32(1);
		if (this.m_msgType == TimedQueueMessage.MsgType.QueueEvent)
		{
			this.m_index = (int)reader.ReadUInt32(4);
			this.m_time = reader.ReadFloat32();
		}
		return true;
	}

	// Token: 0x04000521 RID: 1313
	public const int kBitsPerMsgType = 1;

	// Token: 0x04000522 RID: 1314
	public const int kBitsPerIndex = 4;

	// Token: 0x04000523 RID: 1315
	public TimedQueueMessage.MsgType m_msgType;

	// Token: 0x04000524 RID: 1316
	public int m_index;

	// Token: 0x04000525 RID: 1317
	public float m_time;

	// Token: 0x0200015E RID: 350
	public enum MsgType
	{
		// Token: 0x04000527 RID: 1319
		QueueEvent,
		// Token: 0x04000528 RID: 1320
		Cancel
	}
}
