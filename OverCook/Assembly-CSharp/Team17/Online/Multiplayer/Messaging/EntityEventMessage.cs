using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008B1 RID: 2225
	public class EntityEventMessage : Serialisable
	{
		// Token: 0x06002B5B RID: 11099 RVA: 0x000CAEA7 File Offset: 0x000C92A7
		public void Initialise(EntityMessageHeader header, uint uComponentId, Serialisable payload)
		{
			this.m_Header = header;
			this.m_ComponentId = uComponentId;
			this.m_Payload = payload;
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000CAEBE File Offset: 0x000C92BE
		public void Serialise(BitStreamWriter writer)
		{
			this.m_Header.Serialise(writer);
			writer.Write((byte)this.m_ComponentId, 4);
			this.m_Payload.Serialise(writer);
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000CAEE8 File Offset: 0x000C92E8
		public bool Deserialise(BitStreamReader reader)
		{
			if (!this.m_Header.Deserialise(reader))
			{
				return false;
			}
			this.m_ComponentId = (uint)reader.ReadByte(4);
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_Header.m_uEntityID);
			if (entry != null)
			{
				ClientSynchroniser clientSynchroniser = entry.m_ClientSynchronisedComponents._items[(int)this.m_ComponentId];
				return SerialisationRegistry<EntityType>.Deserialise(out this.m_Payload, clientSynchroniser.GetEntityType(), reader);
			}
			return false;
		}

		// Token: 0x04002252 RID: 8786
		public EntityMessageHeader m_Header = new EntityMessageHeader();

		// Token: 0x04002253 RID: 8787
		public uint m_ComponentId;

		// Token: 0x04002254 RID: 8788
		public Serialisable m_Payload;
	}
}
