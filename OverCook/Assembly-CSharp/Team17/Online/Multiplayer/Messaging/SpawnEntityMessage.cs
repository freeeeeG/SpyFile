using System;
using BitStream;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008DF RID: 2271
	public class SpawnEntityMessage : Serialisable
	{
		// Token: 0x06002C1D RID: 11293 RVA: 0x000CD926 File Offset: 0x000CBD26
		public void Initialise(EntityMessageHeader _spawner, int _spawnableID, EntityMessageHeader _desiredHeader, Vector3 _position, Quaternion _rotation)
		{
			this.m_SpawnerHeader = _spawner;
			this.m_SpawnableID = _spawnableID;
			this.m_DesiredHeader = _desiredHeader;
			this.m_Position = _position;
			this.m_Rotation = _rotation;
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000CD94D File Offset: 0x000CBD4D
		public void Serialise(BitStreamWriter writer)
		{
			this.m_SpawnerHeader.Serialise(writer);
			writer.Write((uint)this.m_SpawnableID, 4);
			this.m_DesiredHeader.Serialise(writer);
			writer.Write(ref this.m_Position);
			writer.Write(ref this.m_Rotation);
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000CD98C File Offset: 0x000CBD8C
		public bool Deserialise(BitStreamReader reader)
		{
			if (this.m_SpawnerHeader.Deserialise(reader))
			{
				this.m_SpawnableID = (int)reader.ReadUInt32(4);
				if (this.m_DesiredHeader.Deserialise(reader))
				{
					reader.ReadVector3(ref this.m_Position);
					reader.ReadQuaternion(ref this.m_Rotation);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400236C RID: 9068
		public const int kBitsPerSpawnableID = 4;

		// Token: 0x0400236D RID: 9069
		public EntityMessageHeader m_SpawnerHeader = new EntityMessageHeader();

		// Token: 0x0400236E RID: 9070
		public int m_SpawnableID;

		// Token: 0x0400236F RID: 9071
		public EntityMessageHeader m_DesiredHeader = new EntityMessageHeader();

		// Token: 0x04002370 RID: 9072
		public Vector3 m_Position;

		// Token: 0x04002371 RID: 9073
		public Quaternion m_Rotation;
	}
}
