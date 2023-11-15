using System;
using BitStream;
using GameModes;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008DE RID: 2270
public class SetupCoopSessionNetworkMessage : Serialisable
{
	// Token: 0x06002C1A RID: 11290 RVA: 0x000CD868 File Offset: 0x000CBC68
	public void Serialise(BitStreamWriter writer)
	{
		bool flag = this.m_DLCID != -1;
		writer.Write(flag);
		if (flag)
		{
			writer.Write((uint)this.m_DLCID, 4);
		}
		this.m_Progress.Serialise(writer);
		this.m_sessionConfig.Serialise(writer);
	}

	// Token: 0x06002C1B RID: 11291 RVA: 0x000CD8B4 File Offset: 0x000CBCB4
	public bool Deserialise(BitStreamReader reader)
	{
		bool flag = reader.ReadBit();
		if (flag)
		{
			this.m_DLCID = (int)reader.ReadUInt32(4);
		}
		else
		{
			this.m_DLCID = -1;
		}
		bool flag2 = true;
		flag2 &= this.m_Progress.Deserialise(reader);
		return flag2 & this.m_sessionConfig.Deserialise(reader);
	}

	// Token: 0x04002369 RID: 9065
	public int m_DLCID;

	// Token: 0x0400236A RID: 9066
	public GameProgressDataNetworkMessage m_Progress = new GameProgressDataNetworkMessage();

	// Token: 0x0400236B RID: 9067
	public SessionConfigSyncMessage m_sessionConfig = new SessionConfigSyncMessage();
}
