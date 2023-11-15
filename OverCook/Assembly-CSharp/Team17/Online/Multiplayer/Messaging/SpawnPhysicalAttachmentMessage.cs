using System;
using BitStream;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008E0 RID: 2272
	public class SpawnPhysicalAttachmentMessage : Serialisable
	{
		// Token: 0x06002C21 RID: 11297 RVA: 0x000CDA01 File Offset: 0x000CBE01
		public void Initialise(EntityMessageHeader _spawner, int _spawnableID, EntityMessageHeader _desiredHeader, Vector3 _position, Quaternion _rotation, EntityMessageHeader _container)
		{
			this.m_SpawnEntityData.Initialise(_spawner, _spawnableID, _desiredHeader, _position, _rotation);
			this.m_ContainerHeader = _container;
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x000CDA1D File Offset: 0x000CBE1D
		public void Serialise(BitStreamWriter writer)
		{
			this.m_SpawnEntityData.Serialise(writer);
			this.m_ContainerHeader.Serialise(writer);
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000CDA38 File Offset: 0x000CBE38
		public bool Deserialise(BitStreamReader reader)
		{
			bool flag = this.m_SpawnEntityData.Deserialise(reader);
			return flag | this.m_ContainerHeader.Deserialise(reader);
		}

		// Token: 0x04002372 RID: 9074
		public SpawnEntityMessage m_SpawnEntityData = new SpawnEntityMessage();

		// Token: 0x04002373 RID: 9075
		public EntityMessageHeader m_ContainerHeader = new EntityMessageHeader();
	}
}
