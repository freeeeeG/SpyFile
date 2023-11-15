using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008C6 RID: 2246
public class LevelLoadByIndexMessage : Serialisable
{
	// Token: 0x06002BA3 RID: 11171 RVA: 0x000CBF9B File Offset: 0x000CA39B
	public void Initialise(GameState start, GameState stop, uint uLevelIndex, uint uPlayers, bool bUseLoadingScreen)
	{
		this.m_StartLoadGameState = start;
		this.m_HideLoadingScreenGameState = stop;
		this.LevelIndex = uLevelIndex;
		this.Players = uPlayers;
		this.UseLoadingScreen = bUseLoadingScreen;
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x000CBFC4 File Offset: 0x000CA3C4
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_StartLoadGameState, 6);
		writer.Write((uint)this.m_HideLoadingScreenGameState, 6);
		writer.Write(this.LevelIndex, 8);
		writer.Write(this.Players, 8);
		writer.Write(this.UseLoadingScreen);
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x000CC014 File Offset: 0x000CA414
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_StartLoadGameState = (GameState)reader.ReadUInt32(6);
		this.m_HideLoadingScreenGameState = (GameState)reader.ReadUInt32(6);
		this.LevelIndex = reader.ReadUInt32(8);
		this.Players = reader.ReadUInt32(8);
		this.UseLoadingScreen = reader.ReadBit();
		return true;
	}

	// Token: 0x040022E3 RID: 8931
	public const uint kInvalidLevel = 255U;

	// Token: 0x040022E4 RID: 8932
	public GameState m_StartLoadGameState;

	// Token: 0x040022E5 RID: 8933
	public GameState m_HideLoadingScreenGameState;

	// Token: 0x040022E6 RID: 8934
	public uint LevelIndex = 255U;

	// Token: 0x040022E7 RID: 8935
	public uint Players = 255U;

	// Token: 0x040022E8 RID: 8936
	public bool UseLoadingScreen = true;
}
