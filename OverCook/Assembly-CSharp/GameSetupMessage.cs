using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008B8 RID: 2232
public class GameSetupMessage : Serialisable
{
	// Token: 0x06002B70 RID: 11120 RVA: 0x000CB5EA File Offset: 0x000C99EA
	public void Initialise(GameMode mode)
	{
		this.m_Mode = mode;
	}

	// Token: 0x06002B71 RID: 11121 RVA: 0x000CB5F3 File Offset: 0x000C99F3
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((byte)this.m_Mode, 3);
	}

	// Token: 0x06002B72 RID: 11122 RVA: 0x000CB603 File Offset: 0x000C9A03
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_Mode = (GameMode)reader.ReadByte(3);
		return true;
	}

	// Token: 0x040022A5 RID: 8869
	public GameMode m_Mode = GameMode.COUNT;
}
