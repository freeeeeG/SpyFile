using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020006E2 RID: 1762
public class LobbyClientMessage : Serialisable
{
	// Token: 0x06002140 RID: 8512 RVA: 0x0009EE78 File Offset: 0x0009D278
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_type, 2);
		switch (this.m_type)
		{
		case LobbyClientMessage.LobbyMessageType.ThemeSelected:
			writer.Write((uint)this.m_theme, SceneDirectoryData.c_bitsPerTheme);
			writer.Write((uint)this.m_chefIndex, 2);
			break;
		case LobbyClientMessage.LobbyMessageType.TeamChangeRequest:
			writer.Write((uint)this.m_chefIndex, 2);
			break;
		}
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x0009EEF0 File Offset: 0x0009D2F0
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_type = (LobbyClientMessage.LobbyMessageType)reader.ReadUInt32(2);
		switch (this.m_type)
		{
		case LobbyClientMessage.LobbyMessageType.ThemeSelected:
			this.m_theme = (SceneDirectoryData.LevelTheme)reader.ReadUInt32(SceneDirectoryData.c_bitsPerTheme);
			this.m_chefIndex = (int)reader.ReadUInt32(2);
			break;
		case LobbyClientMessage.LobbyMessageType.TeamChangeRequest:
			this.m_chefIndex = (int)reader.ReadUInt32(2);
			break;
		}
		return true;
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x0009EF68 File Offset: 0x0009D368
	public override string ToString()
	{
		string text = base.GetType() + "(" + this.m_type;
		LobbyClientMessage.LobbyMessageType type = this.m_type;
		if (type != LobbyClientMessage.LobbyMessageType.ThemeSelected)
		{
			if (type != LobbyClientMessage.LobbyMessageType.TeamChangeRequest)
			{
			}
		}
		else
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				", ThemeSelection(",
				this.m_theme,
				", ",
				this.m_chefIndex,
				")"
			});
		}
		return text + ")";
	}

	// Token: 0x0400196E RID: 6510
	public const int kBitsPerLobbyMsgType = 2;

	// Token: 0x0400196F RID: 6511
	public LobbyClientMessage.LobbyMessageType m_type;

	// Token: 0x04001970 RID: 6512
	public SceneDirectoryData.LevelTheme m_theme;

	// Token: 0x04001971 RID: 6513
	public const int kBitsPerChefIndex = 2;

	// Token: 0x04001972 RID: 6514
	public int m_chefIndex;

	// Token: 0x020006E3 RID: 1763
	public enum LobbyMessageType
	{
		// Token: 0x04001974 RID: 6516
		ThemeSelected,
		// Token: 0x04001975 RID: 6517
		StateRequest,
		// Token: 0x04001976 RID: 6518
		TeamChangeRequest
	}
}
