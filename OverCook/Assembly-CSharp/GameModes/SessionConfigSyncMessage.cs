using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

namespace GameModes
{
	// Token: 0x020008DD RID: 2269
	public class SessionConfigSyncMessage : Serialisable
	{
		// Token: 0x06002C16 RID: 11286 RVA: 0x000CD7A9 File Offset: 0x000CBBA9
		public void Initialise(SessionConfig config)
		{
			this.m_config = config;
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000CD7B4 File Offset: 0x000CBBB4
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((uint)this.m_config.m_kind, this.k_gameModeKindBits);
			for (int i = 0; i < 3; i++)
			{
				writer.Write(this.m_config.m_settings[i]);
			}
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x000CD800 File Offset: 0x000CBC00
		public bool Deserialise(BitStreamReader reader)
		{
			this.m_config.m_kind = (Kind)reader.ReadUInt32(this.k_gameModeKindBits);
			for (int i = 0; i < 3; i++)
			{
				this.m_config.m_settings[i] = reader.ReadBit();
			}
			return true;
		}

		// Token: 0x04002367 RID: 9063
		public int k_gameModeKindBits = 3;

		// Token: 0x04002368 RID: 9064
		public SessionConfig m_config = new SessionConfig();
	}
}
