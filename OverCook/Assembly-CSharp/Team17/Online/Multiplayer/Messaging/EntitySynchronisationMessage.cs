using System;
using System.Collections.Generic;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008B5 RID: 2229
	public class EntitySynchronisationMessage : Serialisable
	{
		// Token: 0x06002B65 RID: 11109 RVA: 0x000CAFAD File Offset: 0x000C93AD
		public void Initialise(EntityMessageHeader header, FastList<Serialisable> payloads)
		{
			this.m_Header = header;
			this.m_Payloads = payloads;
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x000CAFC0 File Offset: 0x000C93C0
		public void Serialise(BitStreamWriter writer)
		{
			this.m_Header.Serialise(writer);
			for (int i = 0; i < this.m_Payloads.Count; i++)
			{
				if (this.m_Payloads._items[i] != null)
				{
					writer.Write(true);
					this.m_Payloads._items[i].Serialise(writer);
				}
				else
				{
					writer.Write(false);
				}
			}
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x000CB030 File Offset: 0x000C9430
		public bool Deserialise(BitStreamReader reader)
		{
			this.m_Payloads.Clear();
			if (this.m_Header.Deserialise(reader))
			{
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_Header.m_uEntityID);
				if (entry != null)
				{
					for (int i = 0; i < entry.m_ClientSynchronisedComponents.Count; i++)
					{
						bool flag = reader.ReadBit();
						if (flag)
						{
							ClientSynchroniser clientSynchroniser = entry.m_ClientSynchronisedComponents._items[i];
							Serialisable item;
							SerialisationRegistry<EntityType>.Deserialise(out item, clientSynchroniser.GetEntityType(), reader);
							this.m_Payloads.Add(item);
						}
						else
						{
							this.m_Payloads.Add(null);
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002299 RID: 8857
		public EntityMessageHeader m_Header = new EntityMessageHeader();

		// Token: 0x0400229A RID: 8858
		public FastList<Serialisable> m_Payloads = new FastList<Serialisable>(16);
	}
}
