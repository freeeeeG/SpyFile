using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200077A RID: 1914
[Serializable]
public class TeamMonitor
{
	// Token: 0x04001C86 RID: 7302
	[SerializeField]
	public RecipeFlowGUI m_recipeBarUIController;

	// Token: 0x0200077B RID: 1915
	public class TeamScoreStats : Serialisable
	{
		// Token: 0x060024FB RID: 9467 RVA: 0x000AECA8 File Offset: 0x000AD0A8
		public int GetTotalScore()
		{
			return this.TotalBaseScore + this.TotalTipsScore - this.TotalTimeExpireDeductions;
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000AECC0 File Offset: 0x000AD0C0
		public bool Copy(TeamMonitor.TeamScoreStats _other)
		{
			this.TotalBaseScore = _other.TotalBaseScore;
			this.TotalTipsScore = _other.TotalTipsScore;
			this.TotalMultiplier = _other.TotalMultiplier;
			this.TotalCombo = _other.TotalCombo;
			this.TotalTimeExpireDeductions = _other.TotalTimeExpireDeductions;
			this.ComboMaintained = _other.ComboMaintained;
			this.TotalSuccessfulDeliveries = _other.TotalSuccessfulDeliveries;
			return true;
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000AED24 File Offset: 0x000AD124
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((uint)this.TotalBaseScore, 16);
			writer.Write((uint)this.TotalTipsScore, 16);
			writer.Write((uint)this.TotalMultiplier, 3);
			writer.Write((uint)this.TotalCombo, 8);
			writer.Write((uint)this.TotalTimeExpireDeductions, 16);
			writer.Write(this.ComboMaintained);
			writer.Write((uint)this.TotalSuccessfulDeliveries, 8);
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000AED90 File Offset: 0x000AD190
		public bool Deserialise(BitStreamReader reader)
		{
			this.TotalBaseScore = (int)reader.ReadUInt32(16);
			this.TotalTipsScore = (int)reader.ReadUInt32(16);
			this.TotalMultiplier = (int)reader.ReadUInt32(3);
			this.TotalCombo = (int)reader.ReadUInt32(8);
			this.TotalTimeExpireDeductions = (int)reader.ReadUInt32(16);
			this.ComboMaintained = reader.ReadBit();
			this.TotalSuccessfulDeliveries = (int)reader.ReadUInt32(8);
			return true;
		}

		// Token: 0x04001C87 RID: 7303
		private const int kBitsPerScore = 16;

		// Token: 0x04001C88 RID: 7304
		private const int kBitsPerMultiplier = 3;

		// Token: 0x04001C89 RID: 7305
		private const int kBitsPerCombo = 8;

		// Token: 0x04001C8A RID: 7306
		private const int kBitsPerDelivery = 8;

		// Token: 0x04001C8B RID: 7307
		public int TotalBaseScore;

		// Token: 0x04001C8C RID: 7308
		public int TotalTipsScore;

		// Token: 0x04001C8D RID: 7309
		public int TotalMultiplier;

		// Token: 0x04001C8E RID: 7310
		public int TotalCombo;

		// Token: 0x04001C8F RID: 7311
		public int TotalTimeExpireDeductions;

		// Token: 0x04001C90 RID: 7312
		public bool ComboMaintained = true;

		// Token: 0x04001C91 RID: 7313
		public int TotalSuccessfulDeliveries;
	}
}
