using System;
using System.Collections.Generic;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200089E RID: 2206
public class MessageBatcher
{
	// Token: 0x06002B03 RID: 11011 RVA: 0x000C9C19 File Offset: 0x000C8019
	public void Initialise(TechMessageType type)
	{
		this.m_SizeWriter = new BitStreamWriter(this.m_SizeBuffer);
		this.m_TechHeaderWriter.Initialise();
		this.Reset(type);
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x000C9C40 File Offset: 0x000C8040
	public void Reset(TechMessageType type)
	{
		this.m_SizeBuffer.Clear();
		this.m_SizeWriter.Reset(this.m_SizeBuffer);
		Array.Clear(this.m_PendingSendBuffer, 0, this.m_PendingSendBuffer.Length);
		this.m_PendingSendBuffer[0] = this.m_TechHeaderWriter.SerialiseHeader(true, type);
		for (int i = 1; i < 3; i++)
		{
			this.m_PendingSendBuffer[i] = 0;
		}
		this.m_BytesUsed = 3;
		this.MessagesBatched = 0;
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x000C9CBC File Offset: 0x000C80BC
	public void SetSequenceNumber(uint sequenceNumber)
	{
		FastList<byte> fastList = this.m_TechHeaderWriter.SerialiseSequence(sequenceNumber);
		for (int i = 0; i < fastList.Count; i++)
		{
			this.m_PendingSendBuffer[1 + i] = fastList._items[i];
		}
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x000C9CFF File Offset: 0x000C80FF
	public bool IsSizeSupported(int size)
	{
		return size <= 255;
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x000C9D0C File Offset: 0x000C810C
	public bool AddData(byte[] data, int size)
	{
		if (size + 1 > this.GetAvailableSpace())
		{
			return false;
		}
		if (size + 1 > 255)
		{
			return false;
		}
		this.m_SizeBuffer.Clear();
		this.m_SizeWriter.Write((uint)(size + 1), 8);
		this.m_PendingSendBuffer[this.m_BytesUsed] = this.m_SizeBuffer._items[0];
		this.m_BytesUsed++;
		Array.Copy(data, 0, this.m_PendingSendBuffer, this.m_BytesUsed, size);
		this.m_BytesUsed += size;
		this.MessagesBatched++;
		return true;
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x000C9DAB File Offset: 0x000C81AB
	public int GetAvailableSpace()
	{
		return 1024 - this.m_BytesUsed;
	}

	// Token: 0x040021FB RID: 8699
	public const int MAX_GAMEMESSAGE_SIZE = 255;

	// Token: 0x040021FC RID: 8700
	public int m_BytesUsed;

	// Token: 0x040021FD RID: 8701
	public byte[] m_PendingSendBuffer = new byte[1024];

	// Token: 0x040021FE RID: 8702
	public int MessagesBatched;

	// Token: 0x040021FF RID: 8703
	private TechHeaderWriter m_TechHeaderWriter = new TechHeaderWriter();

	// Token: 0x04002200 RID: 8704
	private FastList<byte> m_SizeBuffer = new FastList<byte>(1);

	// Token: 0x04002201 RID: 8705
	private BitStreamWriter m_SizeWriter;
}
