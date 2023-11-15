using System;
using System.Collections.Generic;
using BitStream;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200089D RID: 2205
public class TechHeaderWriter
{
	// Token: 0x06002AFF RID: 11007 RVA: 0x000C9B45 File Offset: 0x000C7F45
	public void Initialise()
	{
		this.m_TechHeaderWriter = new BitStreamWriter(this.m_TechHeaderBuffer);
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x000C9B58 File Offset: 0x000C7F58
	public byte SerialiseHeader(bool bIsGameMessage, TechMessageType type)
	{
		this.m_TechHeaderBuffer.Clear();
		this.m_TechHeaderWriter.Reset(this.m_TechHeaderBuffer);
		this.m_TechHeader.IsGameMessage = bIsGameMessage;
		this.m_TechHeader.MessageTypeId = (byte)type;
		this.m_TechHeader.Serialize(this.m_TechHeaderWriter);
		return this.m_TechHeaderBuffer._items[0];
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x000C9BB8 File Offset: 0x000C7FB8
	public FastList<byte> SerialiseSequence(uint sequenceNumber)
	{
		this.m_TechHeaderBuffer.Clear();
		this.m_TechHeaderWriter.Reset(this.m_TechHeaderBuffer);
		this.m_TechHeaderWriter.Write(sequenceNumber, 16);
		return this.m_TechHeaderBuffer;
	}

	// Token: 0x040021F4 RID: 8692
	public const int HeaderIndex = 0;

	// Token: 0x040021F5 RID: 8693
	public const int SequenceIndex = 1;

	// Token: 0x040021F6 RID: 8694
	public const int SequenceSize = 2;

	// Token: 0x040021F7 RID: 8695
	public const int Size = 3;

	// Token: 0x040021F8 RID: 8696
	private OnlineMultiplayerSessionTransportMessageHeader m_TechHeader = new OnlineMultiplayerSessionTransportMessageHeader();

	// Token: 0x040021F9 RID: 8697
	private FastList<byte> m_TechHeaderBuffer = new FastList<byte>(2);

	// Token: 0x040021FA RID: 8698
	private BitStreamWriter m_TechHeaderWriter;
}
