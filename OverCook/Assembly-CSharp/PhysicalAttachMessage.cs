using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020008D4 RID: 2260
public class PhysicalAttachMessage : Serialisable
{
	// Token: 0x06002BD9 RID: 11225 RVA: 0x000CC8B0 File Offset: 0x000CACB0
	public void Serialise(BitStreamWriter writer)
	{
		uint bits = 0U;
		if (this.m_parentable != null)
		{
			GameObject gameObject = (this.m_parentable as MonoBehaviour).gameObject;
			if (gameObject != null)
			{
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(gameObject);
				if (entry != null)
				{
					bits = entry.m_Header.m_uEntityID;
				}
			}
		}
		writer.Write(bits, 10);
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x000CC90C File Offset: 0x000CAD0C
	public bool Deserialise(BitStreamReader reader)
	{
		uint num = reader.ReadUInt32(10);
		if (num != 0U)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(num);
			if (entry != null)
			{
				this.m_parentable = entry.m_GameObject.RequireInterface<IParentable>();
			}
		}
		else
		{
			this.m_parentable = null;
		}
		return true;
	}

	// Token: 0x04002336 RID: 9014
	public IParentable m_parentable;
}
