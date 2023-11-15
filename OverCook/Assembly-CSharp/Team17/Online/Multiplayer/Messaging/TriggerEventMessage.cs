using System;
using System.Text;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008E5 RID: 2277
	public class TriggerEventMessage : Serialisable
	{
		// Token: 0x06002C34 RID: 11316 RVA: 0x000CDD84 File Offset: 0x000CC184
		public void Initialise(EntityMessageHeader _source, EntityMessageHeader _target, string _event, float _time)
		{
			this.m_SourceHeader = _source;
			this.m_TargetHeader = _target;
			this.m_Event = _event;
			this.m_Time = _time;
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x000CDDA3 File Offset: 0x000CC1A3
		public void Serialise(BitStreamWriter writer)
		{
			this.m_SourceHeader.Serialise(writer);
			this.m_TargetHeader.Serialise(writer);
			writer.Write(this.m_Time);
			writer.Write(this.m_Event, Encoding.ASCII);
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x000CDDDC File Offset: 0x000CC1DC
		public bool Deserialise(BitStreamReader reader)
		{
			if (this.m_SourceHeader.Deserialise(reader) && this.m_TargetHeader.Deserialise(reader))
			{
				this.m_Time = reader.ReadFloat32();
				this.m_Event = reader.ReadString(Encoding.ASCII);
				return true;
			}
			return false;
		}

		// Token: 0x04002386 RID: 9094
		public EntityMessageHeader m_SourceHeader = new EntityMessageHeader();

		// Token: 0x04002387 RID: 9095
		public EntityMessageHeader m_TargetHeader = new EntityMessageHeader();

		// Token: 0x04002388 RID: 9096
		public string m_Event;

		// Token: 0x04002389 RID: 9097
		public float m_Time;
	}
}
