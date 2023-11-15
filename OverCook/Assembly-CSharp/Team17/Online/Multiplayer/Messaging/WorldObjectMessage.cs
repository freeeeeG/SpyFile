using System;
using BitStream;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008E9 RID: 2281
	public class WorldObjectMessage : Serialisable
	{
		// Token: 0x06002C4A RID: 11338 RVA: 0x000CE314 File Offset: 0x000CC714
		public virtual void Serialise(BitStreamWriter writer)
		{
			writer.Write(this.HasParent);
			if (this.HasParent)
			{
				writer.Write(this.ParentEntityID, 10);
			}
			writer.Write(this.HasPositions);
			if (this.HasPositions)
			{
				writer.Write(ref this.LocalPosition);
				writer.Write(ref this.LocalRotation);
			}
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x000CE378 File Offset: 0x000CC778
		public virtual bool Deserialise(BitStreamReader reader)
		{
			this.HasParent = reader.ReadBit();
			if (this.HasParent)
			{
				this.ParentEntityID = reader.ReadUInt32(10);
			}
			else
			{
				this.ParentEntityID = 0U;
			}
			this.HasPositions = reader.ReadBit();
			if (this.HasPositions)
			{
				reader.ReadVector3(ref this.LocalPosition);
				reader.ReadQuaternion(ref this.LocalRotation);
			}
			return true;
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x000CE3E6 File Offset: 0x000CC7E6
		public void Copy(WorldObjectMessage _other)
		{
			this.LocalPosition = _other.LocalPosition;
			this.LocalRotation = _other.LocalRotation;
			this.HasParent = _other.HasParent;
			this.HasPositions = _other.HasPositions;
			this.ParentEntityID = _other.ParentEntityID;
		}

		// Token: 0x0400239D RID: 9117
		public Vector3 LocalPosition = default(Vector3);

		// Token: 0x0400239E RID: 9118
		public Quaternion LocalRotation = default(Quaternion);

		// Token: 0x0400239F RID: 9119
		public bool HasParent;

		// Token: 0x040023A0 RID: 9120
		public bool HasPositions;

		// Token: 0x040023A1 RID: 9121
		public uint ParentEntityID;
	}
}
