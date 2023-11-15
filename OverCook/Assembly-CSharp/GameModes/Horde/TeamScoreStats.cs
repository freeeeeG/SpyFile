using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

namespace GameModes.Horde
{
	// Token: 0x020007CA RID: 1994
	public struct TeamScoreStats : Serialisable
	{
		// Token: 0x0600262D RID: 9773 RVA: 0x000B53BE File Offset: 0x000B37BE
		public int GetTotalMoney()
		{
			return this.TotalMoneyEarned - this.TotalMoneySpent;
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000B53CD File Offset: 0x000B37CD
		public bool Copy(TeamScoreStats _other)
		{
			this.TotalHealth = _other.TotalHealth;
			this.TotalMoneyEarned = _other.TotalMoneyEarned;
			this.TotalMoneySpent = _other.TotalMoneySpent;
			this.TotalEnemiesDefeated = _other.TotalEnemiesDefeated;
			return true;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000B5404 File Offset: 0x000B3804
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((uint)this.TotalHealth, TeamScoreStats.k_healthBitCount);
			writer.Write((uint)this.TotalMoneyEarned, TeamScoreStats.k_moneyBitCount);
			writer.Write((uint)this.TotalMoneySpent, TeamScoreStats.k_moneyBitCount);
			writer.Write((uint)this.TotalEnemiesDefeated, TeamScoreStats.k_defeatedEnemiesBitCount);
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000B5458 File Offset: 0x000B3858
		public bool Deserialise(BitStreamReader reader)
		{
			this.TotalHealth = (int)reader.ReadUInt32(TeamScoreStats.k_healthBitCount);
			this.TotalMoneyEarned = (int)reader.ReadUInt32(TeamScoreStats.k_moneyBitCount);
			this.TotalMoneySpent = (int)reader.ReadUInt32(TeamScoreStats.k_moneyBitCount);
			this.TotalEnemiesDefeated = (int)reader.ReadUInt32(TeamScoreStats.k_defeatedEnemiesBitCount);
			return true;
		}

		// Token: 0x04001E3C RID: 7740
		public const int k_healthMax = 255;

		// Token: 0x04001E3D RID: 7741
		public const int k_moneyMax = 9999;

		// Token: 0x04001E3E RID: 7742
		public const int k_defeatedEnemiesMax = 255;

		// Token: 0x04001E3F RID: 7743
		private static readonly int k_healthBitCount = GameUtils.GetRequiredBitCount(255);

		// Token: 0x04001E40 RID: 7744
		private static readonly int k_moneyBitCount = GameUtils.GetRequiredBitCount(9999);

		// Token: 0x04001E41 RID: 7745
		private static readonly int k_defeatedEnemiesBitCount = GameUtils.GetRequiredBitCount(255);

		// Token: 0x04001E42 RID: 7746
		public int TotalHealth;

		// Token: 0x04001E43 RID: 7747
		public int TotalMoneyEarned;

		// Token: 0x04001E44 RID: 7748
		public int TotalMoneySpent;

		// Token: 0x04001E45 RID: 7749
		public int TotalEnemiesDefeated;
	}
}
