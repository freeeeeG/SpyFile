using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007CB RID: 1995
	public struct HordeFlowMessage : Serialisable
	{
		// Token: 0x06002632 RID: 9778 RVA: 0x000B54DC File Offset: 0x000B38DC
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((uint)this.m_kind, HordeFlowMessage.k_kindBitCount);
			switch (this.m_kind)
			{
			case HordeFlowMessage.Kind.BeginWave:
			case HordeFlowMessage.Kind.EndWave:
				writer.Write((uint)this.m_index, HordeFlowMessage.k_waveIndexBitCount);
				this.m_score.Serialise(writer);
				break;
			case HordeFlowMessage.Kind.Spawn:
			{
				writer.Write((uint)this.m_index, HordeFlowMessage.k_indexBitCount);
				uint id = EntitySerialisationRegistry.GetId(this.m_enemy);
				writer.Write(id, 10);
				break;
			}
			case HordeFlowMessage.Kind.EntryAdded:
				writer.Write((uint)this.m_index, HordeFlowMessage.k_indexBitCount);
				this.m_entry.Serialise(writer);
				break;
			case HordeFlowMessage.Kind.SuccessfulDelivery:
			case HordeFlowMessage.Kind.IncorrectDelivery:
				writer.Write((uint)this.m_index, HordeFlowMessage.k_indexBitCount);
				this.m_score.Serialise(writer);
				break;
			case HordeFlowMessage.Kind.ScoreOnly:
				this.m_score.Serialise(writer);
				break;
			}
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000B55CC File Offset: 0x000B39CC
		public bool Deserialise(BitStreamReader reader)
		{
			bool flag = true;
			this.m_kind = (HordeFlowMessage.Kind)reader.ReadUInt32(HordeFlowMessage.k_kindBitCount);
			flag &= (this.m_kind != HordeFlowMessage.Kind.Invalid);
			switch (this.m_kind)
			{
			case HordeFlowMessage.Kind.BeginWave:
			case HordeFlowMessage.Kind.EndWave:
				this.m_index = (int)reader.ReadUInt32(HordeFlowMessage.k_waveIndexBitCount);
				flag &= (this.m_index != -1);
				flag &= this.m_score.Deserialise(reader);
				break;
			case HordeFlowMessage.Kind.Spawn:
				this.m_index = (int)reader.ReadUInt32(HordeFlowMessage.k_indexBitCount);
				flag &= (this.m_index != -1);
				flag &= NetworkUtils.DeserialiseGameObject(out this.m_enemy, reader);
				break;
			case HordeFlowMessage.Kind.EntryAdded:
				this.m_index = (int)reader.ReadUInt32(HordeFlowMessage.k_indexBitCount);
				flag &= (this.m_index != -1);
				if (this.m_entry == null)
				{
					this.m_entry = new RecipeList.Entry();
				}
				flag &= this.m_entry.Deserialise(reader);
				break;
			case HordeFlowMessage.Kind.SuccessfulDelivery:
			case HordeFlowMessage.Kind.IncorrectDelivery:
				this.m_index = (int)reader.ReadUInt32(HordeFlowMessage.k_indexBitCount);
				flag &= (this.m_index != -1);
				flag &= this.m_score.Deserialise(reader);
				break;
			case HordeFlowMessage.Kind.ScoreOnly:
				flag &= this.m_score.Deserialise(reader);
				break;
			}
			return flag;
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000B5726 File Offset: 0x000B3B26
		public static void BeginWave(ref HordeFlowMessage message, int waveIndex, TeamScoreStats score)
		{
			HordeFlowMessage.ScoreOnly(ref message, score);
			message.m_kind = HordeFlowMessage.Kind.BeginWave;
			message.m_index = waveIndex;
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000B573D File Offset: 0x000B3B3D
		public static void EndWave(ref HordeFlowMessage message, int waveIndex, TeamScoreStats score)
		{
			HordeFlowMessage.ScoreOnly(ref message, score);
			message.m_kind = HordeFlowMessage.Kind.EndWave;
			message.m_index = waveIndex;
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000B5754 File Offset: 0x000B3B54
		public static void Spawn(ref HordeFlowMessage message, int index, GameObject enemy)
		{
			message.m_kind = HordeFlowMessage.Kind.Spawn;
			message.m_index = index;
			message.m_enemy = enemy;
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000B576B File Offset: 0x000B3B6B
		public static void EntryAdded(ref HordeFlowMessage message, int index, RecipeList.Entry entry)
		{
			message.m_kind = HordeFlowMessage.Kind.EntryAdded;
			message.m_index = index;
			message.m_entry = entry;
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000B5782 File Offset: 0x000B3B82
		public static void SuccessfulDelivery(ref HordeFlowMessage message, int index, TeamScoreStats score)
		{
			HordeFlowMessage.ScoreOnly(ref message, score);
			message.m_kind = HordeFlowMessage.Kind.SuccessfulDelivery;
			message.m_index = index;
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000B5799 File Offset: 0x000B3B99
		public static void IncorrectDelivery(ref HordeFlowMessage message, int index, TeamScoreStats score)
		{
			HordeFlowMessage.ScoreOnly(ref message, score);
			message.m_kind = HordeFlowMessage.Kind.IncorrectDelivery;
			message.m_index = index;
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000B57B0 File Offset: 0x000B3BB0
		public static void ScoreOnly(ref HordeFlowMessage message, TeamScoreStats score)
		{
			message.m_kind = HordeFlowMessage.Kind.ScoreOnly;
			message.m_score.Copy(score);
		}

		// Token: 0x04001E46 RID: 7750
		private const int k_indexMax = 32;

		// Token: 0x04001E47 RID: 7751
		private static readonly int k_indexBitCount = GameUtils.GetRequiredBitCount(32);

		// Token: 0x04001E48 RID: 7752
		private static readonly int k_waveIndexBitCount = GameUtils.GetRequiredBitCount(32);

		// Token: 0x04001E49 RID: 7753
		private static readonly int k_kindBitCount = GameUtils.GetRequiredBitCount(8);

		// Token: 0x04001E4A RID: 7754
		public HordeFlowMessage.Kind m_kind;

		// Token: 0x04001E4B RID: 7755
		public TeamScoreStats m_score;

		// Token: 0x04001E4C RID: 7756
		public int m_index;

		// Token: 0x04001E4D RID: 7757
		public GameObject m_enemy;

		// Token: 0x04001E4E RID: 7758
		public RecipeList.Entry m_entry;

		// Token: 0x020007CC RID: 1996
		public enum Kind
		{
			// Token: 0x04001E50 RID: 7760
			Invalid,
			// Token: 0x04001E51 RID: 7761
			BeginWave,
			// Token: 0x04001E52 RID: 7762
			EndWave,
			// Token: 0x04001E53 RID: 7763
			Spawn,
			// Token: 0x04001E54 RID: 7764
			EntryAdded,
			// Token: 0x04001E55 RID: 7765
			SuccessfulDelivery,
			// Token: 0x04001E56 RID: 7766
			IncorrectDelivery,
			// Token: 0x04001E57 RID: 7767
			ScoreOnly,
			// Token: 0x04001E58 RID: 7768
			Count
		}
	}
}
