using System;
using BitStream;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008A1 RID: 2209
	public class AvatarPositionMessage : Serialisable
	{
		// Token: 0x06002B16 RID: 11030 RVA: 0x000CA124 File Offset: 0x000C8524
		public void Serialise(BitStreamWriter writer)
		{
			this.WorldObject.Serialise(writer);
			writer.Write(this.Velocity.x);
			writer.Write(this.Velocity.y);
			writer.Write(this.Velocity.z);
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x000CA170 File Offset: 0x000C8570
		public bool Deserialise(BitStreamReader reader)
		{
			this.WorldObject.Deserialise(reader);
			this.Velocity.Set(reader.ReadFloat32(), reader.ReadFloat32(), reader.ReadFloat32());
			return true;
		}

		// Token: 0x0400220B RID: 8715
		public WorldObjectMessage WorldObject = new WorldObjectMessage();

		// Token: 0x0400220C RID: 8716
		public Vector3 Velocity = Vector3.zero;
	}
}
