using System;
using BitStream;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008A6 RID: 2214
	public class ChefEventMessage : Serialisable
	{
		// Token: 0x06002B26 RID: 11046 RVA: 0x000CA367 File Offset: 0x000C8767
		public void Initialise(ChefEventMessage.ChefEventType _type, uint _chefEntityID, uint _entityID)
		{
			this.EventType = _type;
			this.EntityID = _entityID;
			this.ChefEntityID = _chefEntityID;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000CA380 File Offset: 0x000C8780
		public bool Deserialise(BitStreamReader reader)
		{
			this.EventType = (ChefEventMessage.ChefEventType)reader.ReadByte(ChefEventMessage.kChefEventBitCount);
			this.ChefEntityID = reader.ReadUInt32(10);
			this.EntityID = reader.ReadUInt32(10);
			if (this.EventType == ChefEventMessage.ChefEventType.KnockBack)
			{
				this.Knockback_Type = (ChefEventMessage.KnockbackType)reader.ReadUInt32(ChefEventMessage.kChefEventKnockbackTypeBitCount);
				reader.ReadVector2(ref this.KnockbackForce);
				reader.ReadVector3(ref this.RelativeContactPoint);
			}
			return true;
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000CA3F0 File Offset: 0x000C87F0
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((byte)this.EventType, ChefEventMessage.kChefEventBitCount);
			writer.Write(this.ChefEntityID, 10);
			writer.Write(this.EntityID, 10);
			if (this.EventType == ChefEventMessage.ChefEventType.KnockBack)
			{
				writer.Write((uint)this.Knockback_Type, ChefEventMessage.kChefEventKnockbackTypeBitCount);
				writer.Write(ref this.KnockbackForce);
				writer.Write(ref this.RelativeContactPoint);
			}
		}

		// Token: 0x0400221C RID: 8732
		private static readonly int kChefEventBitCount = GameUtils.GetRequiredBitCount(7);

		// Token: 0x0400221D RID: 8733
		private static readonly int kChefEventKnockbackTypeBitCount = GameUtils.GetRequiredBitCount(2);

		// Token: 0x0400221E RID: 8734
		public ChefEventMessage.ChefEventType EventType = ChefEventMessage.ChefEventType.COUNT;

		// Token: 0x0400221F RID: 8735
		public uint EntityID;

		// Token: 0x04002220 RID: 8736
		public uint ChefEntityID;

		// Token: 0x04002221 RID: 8737
		public Vector2 KnockbackForce = default(Vector2);

		// Token: 0x04002222 RID: 8738
		public Vector3 RelativeContactPoint = default(Vector3);

		// Token: 0x04002223 RID: 8739
		public ChefEventMessage.KnockbackType Knockback_Type = ChefEventMessage.KnockbackType.COUNT;

		// Token: 0x020008A7 RID: 2215
		public enum ChefEventType : byte
		{
			// Token: 0x04002225 RID: 8741
			PickUp,
			// Token: 0x04002226 RID: 8742
			Place,
			// Token: 0x04002227 RID: 8743
			Take,
			// Token: 0x04002228 RID: 8744
			Interact,
			// Token: 0x04002229 RID: 8745
			TriggerInteract,
			// Token: 0x0400222A RID: 8746
			Throw,
			// Token: 0x0400222B RID: 8747
			KnockBack,
			// Token: 0x0400222C RID: 8748
			COUNT
		}

		// Token: 0x020008A8 RID: 2216
		public enum KnockbackType
		{
			// Token: 0x0400222E RID: 8750
			Throw,
			// Token: 0x0400222F RID: 8751
			Fire,
			// Token: 0x04002230 RID: 8752
			COUNT
		}
	}
}
