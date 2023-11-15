using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000A54 RID: 2644
public class DialogueStateMessage : Serialisable
{
	// Token: 0x0600343A RID: 13370 RVA: 0x000F55FD File Offset: 0x000F39FD
	public void Initialise(DialogueController.Dialogue _dialogue, int _newState)
	{
		this.m_dialogueID = _dialogue.UniqueID;
		this.m_state = _newState;
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x000F5612 File Offset: 0x000F3A12
	public void Serialise(BitStreamWriter _writer)
	{
		_writer.Write((uint)this.m_dialogueID, 24);
		_writer.Write((uint)this.m_state, 8);
	}

	// Token: 0x0600343C RID: 13372 RVA: 0x000F562F File Offset: 0x000F3A2F
	public bool Deserialise(BitStreamReader _reader)
	{
		this.m_dialogueID = (int)_reader.ReadUInt32(24);
		this.m_state = (int)_reader.ReadUInt32(8);
		return true;
	}

	// Token: 0x040029EA RID: 10730
	private const int c_bitsPerDialogueID = 24;

	// Token: 0x040029EB RID: 10731
	private const int c_bitsPerState = 8;

	// Token: 0x040029EC RID: 10732
	public int m_dialogueID;

	// Token: 0x040029ED RID: 10733
	public int m_state;
}
