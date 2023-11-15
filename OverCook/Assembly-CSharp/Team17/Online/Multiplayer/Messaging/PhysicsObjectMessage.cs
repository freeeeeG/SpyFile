using System;
using BitStream;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008D5 RID: 2261
	public class PhysicsObjectMessage : Serialisable
	{
		// Token: 0x06002BDC RID: 11228 RVA: 0x000CC9B4 File Offset: 0x000CADB4
		public virtual void Serialise(BitStreamWriter writer)
		{
			writer.Write(ref this.Velocity);
			writer.Write(this.ContactCount, 3);
			int num = 0;
			while ((long)num < (long)((ulong)this.ContactCount))
			{
				writer.Write(this.Contacts[num], 10);
				writer.Write(ref this.RelativePositions[num]);
				writer.Write(ref this.ContactVelocitys[num]);
				writer.Write(this.ContactTimes[num]);
				num++;
			}
			this.WorldObject.Serialise(writer);
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000CCA44 File Offset: 0x000CAE44
		public virtual bool Deserialise(BitStreamReader reader)
		{
			reader.ReadVector3(ref this.Velocity);
			this.ContactCount = reader.ReadUInt32(3);
			int num = 0;
			while ((long)num < (long)((ulong)this.ContactCount))
			{
				this.Contacts[num] = reader.ReadUInt32(10);
				reader.ReadVector3(ref this.RelativePositions[num]);
				reader.ReadVector3(ref this.ContactVelocitys[num]);
				this.ContactTimes[num] = reader.ReadFloat32();
				num++;
			}
			return this.WorldObject.Deserialise(reader);
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000CCAD4 File Offset: 0x000CAED4
		public void Copy(PhysicsObjectMessage _other)
		{
			this.Velocity = _other.Velocity;
			this.ContactCount = _other.ContactCount;
			int num = 0;
			while ((long)num < (long)((ulong)this.ContactCount))
			{
				this.Contacts[num] = _other.Contacts[num];
				this.RelativePositions[num] = _other.RelativePositions[num];
				this.ContactVelocitys[num] = _other.ContactVelocitys[num];
				this.ContactTimes[num] = _other.ContactTimes[num];
				num++;
			}
			this.WorldObject.Copy(_other.WorldObject);
		}

		// Token: 0x04002337 RID: 9015
		public const int kTrackedChefCount = 4;

		// Token: 0x04002338 RID: 9016
		public Vector3 Velocity = default(Vector3);

		// Token: 0x04002339 RID: 9017
		public uint ContactCount;

		// Token: 0x0400233A RID: 9018
		public uint[] Contacts = new uint[4];

		// Token: 0x0400233B RID: 9019
		public Vector3[] RelativePositions = new Vector3[4];

		// Token: 0x0400233C RID: 9020
		public Vector3[] ContactVelocitys = new Vector3[4];

		// Token: 0x0400233D RID: 9021
		public float[] ContactTimes = new float[4];

		// Token: 0x0400233E RID: 9022
		public WorldObjectMessage WorldObject = new WorldObjectMessage();
	}
}
