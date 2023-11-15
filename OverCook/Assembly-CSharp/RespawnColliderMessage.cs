using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200055D RID: 1373
public class RespawnColliderMessage : Serialisable
{
	// Token: 0x060019E1 RID: 6625 RVA: 0x00081F8B File Offset: 0x0008038B
	public void Initialise(GameObject _object)
	{
		this.m_targetObject = _object;
		this.m_killPosition = this.m_targetObject.transform.position;
	}

	// Token: 0x060019E2 RID: 6626 RVA: 0x00081FAC File Offset: 0x000803AC
	public void Serialise(BitStreamWriter writer)
	{
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_targetObject);
		entry.m_Header.Serialise(writer);
		writer.Write(ref this.m_killPosition);
	}

	// Token: 0x060019E3 RID: 6627 RVA: 0x00081FE0 File Offset: 0x000803E0
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_targetObjectHeader.Deserialise(reader);
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_targetObjectHeader.m_uEntityID);
		if (entry != null)
		{
			this.m_targetObject = entry.m_GameObject;
			reader.ReadVector3(ref this.m_killPosition);
			return true;
		}
		return false;
	}

	// Token: 0x04001492 RID: 5266
	public GameObject m_targetObject;

	// Token: 0x04001493 RID: 5267
	public Vector3 m_killPosition = Vector3.zero;

	// Token: 0x04001494 RID: 5268
	private EntityMessageHeader m_targetObjectHeader = new EntityMessageHeader();
}
