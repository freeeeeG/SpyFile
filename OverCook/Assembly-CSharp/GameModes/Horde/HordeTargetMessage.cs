using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

namespace GameModes.Horde
{
	// Token: 0x020007DD RID: 2013
	public struct HordeTargetMessage : Serialisable
	{
		// Token: 0x060026AA RID: 9898 RVA: 0x000B7FF0 File Offset: 0x000B63F0
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((uint)this.m_kind, HordeTargetMessage.k_kindBitCount);
			HordeTargetMessage.Kind kind = this.m_kind;
			if (kind == HordeTargetMessage.Kind.Health)
			{
				writer.Write(this.m_health);
			}
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000B8034 File Offset: 0x000B6434
		public bool Deserialise(BitStreamReader reader)
		{
			bool result = true;
			this.m_kind = (HordeTargetMessage.Kind)reader.ReadUInt32(HordeTargetMessage.k_kindBitCount);
			HordeTargetMessage.Kind kind = this.m_kind;
			if (kind == HordeTargetMessage.Kind.Health)
			{
				this.m_health = reader.ReadFloat32();
			}
			return result;
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000B8079 File Offset: 0x000B6479
		public static void Health(ref HordeTargetMessage message, float health)
		{
			message.m_kind = HordeTargetMessage.Kind.Health;
			message.m_health = health;
		}

		// Token: 0x04001EA9 RID: 7849
		private static readonly int k_kindBitCount = GameUtils.GetRequiredBitCount(2);

		// Token: 0x04001EAA RID: 7850
		public HordeTargetMessage.Kind m_kind;

		// Token: 0x04001EAB RID: 7851
		public float m_health;

		// Token: 0x020007DE RID: 2014
		public enum Kind
		{
			// Token: 0x04001EAD RID: 7853
			Invalid,
			// Token: 0x04001EAE RID: 7854
			Health,
			// Token: 0x04001EAF RID: 7855
			Count
		}
	}
}
