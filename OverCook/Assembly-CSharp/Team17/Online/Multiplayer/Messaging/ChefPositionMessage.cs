using System;
using BitStream;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008A9 RID: 2217
	public class ChefPositionMessage : Serialisable
	{
		// Token: 0x06002B2B RID: 11051 RVA: 0x000CA498 File Offset: 0x000C8898
		public void Serialise(BitStreamWriter writer)
		{
			this.WorldObject.Serialise(writer);
			writer.Write(this.Velocity.x);
			writer.Write(this.Velocity.y);
			writer.Write(this.Velocity.z);
			writer.Write(this.NetworkTime);
			writer.Write(this.ClientTimeStamp);
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x000CA4FC File Offset: 0x000C88FC
		public bool Deserialise(BitStreamReader reader)
		{
			this.WorldObject.Deserialise(reader);
			this.Velocity.Set(reader.ReadFloat32(), reader.ReadFloat32(), reader.ReadFloat32());
			this.NetworkTime = reader.ReadFloat32();
			this.ClientTimeStamp = reader.ReadFloat32();
			return true;
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x000CA54C File Offset: 0x000C894C
		public void Copy(ChefPositionMessage _other)
		{
			this.WorldObject.Copy(_other.WorldObject);
			this.Velocity = _other.Velocity;
			this.NetworkTime = _other.NetworkTime;
			this.ClientTimeStamp = _other.ClientTimeStamp;
		}

		// Token: 0x04002231 RID: 8753
		public WorldObjectMessage WorldObject = new WorldObjectMessage();

		// Token: 0x04002232 RID: 8754
		public Vector3 Velocity = Vector3.zero;

		// Token: 0x04002233 RID: 8755
		public float NetworkTime;

		// Token: 0x04002234 RID: 8756
		public float ClientTimeStamp;
	}
}
