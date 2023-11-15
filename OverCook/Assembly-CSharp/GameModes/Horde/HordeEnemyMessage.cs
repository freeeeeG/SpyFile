using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

namespace GameModes.Horde
{
	// Token: 0x020007C5 RID: 1989
	public struct HordeEnemyMessage : Serialisable
	{
		// Token: 0x06002612 RID: 9746 RVA: 0x000B4D11 File Offset: 0x000B3111
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((uint)this.m_kind, HordeEnemyMessage.k_kindBitCount);
			writer.Write((uint)this.m_toState, HordeEnemyMessage.k_stateBitCount);
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x000B4D38 File Offset: 0x000B3138
		public bool Deserialise(BitStreamReader reader)
		{
			bool flag = true;
			this.m_kind = (HordeEnemyMessage.Kind)reader.ReadUInt32(HordeEnemyMessage.k_kindBitCount);
			this.m_toState = (HordeEnemyBehaviorState)reader.ReadUInt32(HordeEnemyMessage.k_stateBitCount);
			return flag & this.m_toState != HordeEnemyBehaviorState.Start;
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x000B4D79 File Offset: 0x000B3179
		public static void Transition(ref HordeEnemyMessage message, HordeEnemyBehaviorState state)
		{
			message.m_kind = HordeEnemyMessage.Kind.Transition;
			message.m_toState = state;
		}

		// Token: 0x04001E1D RID: 7709
		public static readonly int k_kindBitCount = GameUtils.GetRequiredBitCount(2);

		// Token: 0x04001E1E RID: 7710
		public static readonly int k_stateBitCount = GameUtils.GetRequiredBitCount(8);

		// Token: 0x04001E1F RID: 7711
		public HordeEnemyMessage.Kind m_kind;

		// Token: 0x04001E20 RID: 7712
		public HordeEnemyBehaviorState m_toState;

		// Token: 0x020007C6 RID: 1990
		public enum Kind
		{
			// Token: 0x04001E22 RID: 7714
			Invalid,
			// Token: 0x04001E23 RID: 7715
			Transition,
			// Token: 0x04001E24 RID: 7716
			Count
		}
	}
}
