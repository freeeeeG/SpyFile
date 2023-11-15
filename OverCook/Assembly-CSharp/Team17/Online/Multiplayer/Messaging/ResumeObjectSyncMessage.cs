using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008D9 RID: 2265
	public class ResumeObjectSyncMessage<T> : Serialisable where T : Serialisable, new()
	{
		// Token: 0x06002BE7 RID: 11239 RVA: 0x000CCC69 File Offset: 0x000CB069
		public void Initialise(uint _entityID, T _data)
		{
			this.EntityID = _entityID;
			this.Data = _data;
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x000CCC7C File Offset: 0x000CB07C
		public void Serialise(BitStreamWriter writer)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.EntityID);
			entry.m_Header.Serialise(writer);
			writer.Write(this.Data != null);
			if (this.Data != null)
			{
				this.Data.Serialise(writer);
			}
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000CCCDC File Offset: 0x000CB0DC
		public bool Deserialise(BitStreamReader reader)
		{
			this.m_entityHeader.Deserialise(reader);
			this.EntityID = this.m_entityHeader.m_uEntityID;
			bool flag = reader.ReadBit();
			if (flag)
			{
				this.Data.Deserialise(reader);
			}
			return true;
		}

		// Token: 0x04002346 RID: 9030
		public uint EntityID;

		// Token: 0x04002347 RID: 9031
		public T Data = Activator.CreateInstance<T>();

		// Token: 0x04002348 RID: 9032
		private EntityMessageHeader m_entityHeader = new EntityMessageHeader();
	}
}
