using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000A88 RID: 2696
public class EmoteWheelMessage : Serialisable
{
	// Token: 0x0600354B RID: 13643 RVA: 0x000F865F File Offset: 0x000F6A5F
	public void InitialiseStartEmote(int _emoteIdx, PlayerInputLookup.Player _player, bool _forUI)
	{
		this.m_emoteIdx = _emoteIdx;
		this.m_player = _player;
		this.m_forUI = _forUI;
	}

	// Token: 0x0600354C RID: 13644 RVA: 0x000F8676 File Offset: 0x000F6A76
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_emoteIdx, 3);
		writer.Write((uint)this.m_player, 5);
		writer.Write(this.m_forUI);
	}

	// Token: 0x0600354D RID: 13645 RVA: 0x000F869E File Offset: 0x000F6A9E
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_emoteIdx = (int)reader.ReadUInt32(3);
		this.m_player = (PlayerInputLookup.Player)reader.ReadUInt32(5);
		this.m_forUI = reader.ReadBit();
		return true;
	}

	// Token: 0x04002ABB RID: 10939
	public int m_emoteIdx;

	// Token: 0x04002ABC RID: 10940
	private const int kBitsPerEmoteIdx = 3;

	// Token: 0x04002ABD RID: 10941
	public PlayerInputLookup.Player m_player;

	// Token: 0x04002ABE RID: 10942
	private const int kBitsPerPlayerId = 5;

	// Token: 0x04002ABF RID: 10943
	public bool m_forUI;
}
