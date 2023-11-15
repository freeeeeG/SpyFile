using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020004AC RID: 1196
public class HeatedStationMessage : Serialisable
{
	// Token: 0x0600163B RID: 5691 RVA: 0x00076341 File Offset: 0x00074741
	public void Serialise(BitStreamWriter _writer)
	{
		_writer.Write((uint)this.m_msgType, 2);
		if (this.m_msgType == HeatedStationMessage.MsgType.Heat)
		{
			_writer.Write(this.m_heat);
		}
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x00076367 File Offset: 0x00074767
	public bool Deserialise(BitStreamReader _reader)
	{
		this.m_msgType = (HeatedStationMessage.MsgType)_reader.ReadUInt32(2);
		if (this.m_msgType == HeatedStationMessage.MsgType.Heat)
		{
			this.m_heat = _reader.ReadFloat32();
		}
		return true;
	}

	// Token: 0x040010D5 RID: 4309
	public HeatedStationMessage.MsgType m_msgType;

	// Token: 0x040010D6 RID: 4310
	public float m_heat;

	// Token: 0x020004AD RID: 1197
	public enum MsgType
	{
		// Token: 0x040010D8 RID: 4312
		Heat,
		// Token: 0x040010D9 RID: 4313
		ItemAdded
	}
}
