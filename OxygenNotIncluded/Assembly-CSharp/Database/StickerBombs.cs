using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D25 RID: 3365
	public class StickerBombs : ResourceSet<DbStickerBomb>
	{
		// Token: 0x06006A1A RID: 27162 RVA: 0x00294F18 File Offset: 0x00293118
		public StickerBombs(ResourceSet parent) : base("StickerBombs", parent)
		{
			foreach (StickerBombs.Info info in StickerBombs.Infos_All)
			{
				this.Add(info.id, info.stickerName, info.rarity, info.animfilename, info.sticker);
			}
		}

		// Token: 0x06006A1B RID: 27163 RVA: 0x00294F74 File Offset: 0x00293174
		private DbStickerBomb Add(string id, string stickerType, PermitRarity rarity, string animfilename, string symbolName)
		{
			DbStickerBomb dbStickerBomb = new DbStickerBomb(id, stickerType, rarity, animfilename, symbolName);
			this.resources.Add(dbStickerBomb);
			return dbStickerBomb;
		}

		// Token: 0x06006A1C RID: 27164 RVA: 0x00294F9B File Offset: 0x0029319B
		public DbStickerBomb GetRandomSticker()
		{
			return this.resources.GetRandom<DbStickerBomb>();
		}

		// Token: 0x04004D3C RID: 19772
		public static StickerBombs.Info[] Infos_Default = new StickerBombs.Info[]
		{
			new StickerBombs.Info("a", STICKERNAMES.STICKER_A, PermitRarity.Universal, "sticker_a_kanim", "a"),
			new StickerBombs.Info("b", STICKERNAMES.STICKER_B, PermitRarity.Universal, "sticker_b_kanim", "b"),
			new StickerBombs.Info("c", STICKERNAMES.STICKER_C, PermitRarity.Universal, "sticker_c_kanim", "c"),
			new StickerBombs.Info("d", STICKERNAMES.STICKER_D, PermitRarity.Universal, "sticker_d_kanim", "d"),
			new StickerBombs.Info("e", STICKERNAMES.STICKER_E, PermitRarity.Universal, "sticker_e_kanim", "e"),
			new StickerBombs.Info("f", STICKERNAMES.STICKER_F, PermitRarity.Universal, "sticker_f_kanim", "f"),
			new StickerBombs.Info("g", STICKERNAMES.STICKER_G, PermitRarity.Universal, "sticker_g_kanim", "g"),
			new StickerBombs.Info("h", STICKERNAMES.STICKER_H, PermitRarity.Universal, "sticker_h_kanim", "h"),
			new StickerBombs.Info("rocket", STICKERNAMES.STICKER_ROCKET, PermitRarity.Universal, "sticker_rocket_kanim", "rocket"),
			new StickerBombs.Info("paperplane", STICKERNAMES.STICKER_PAPERPLANE, PermitRarity.Universal, "sticker_paperplane_kanim", "paperplane"),
			new StickerBombs.Info("plant", STICKERNAMES.STICKER_PLANT, PermitRarity.Universal, "sticker_plant_kanim", "plant"),
			new StickerBombs.Info("plantpot", STICKERNAMES.STICKER_PLANTPOT, PermitRarity.Universal, "sticker_plantpot_kanim", "plantpot"),
			new StickerBombs.Info("mushroom", STICKERNAMES.STICKER_MUSHROOM, PermitRarity.Universal, "sticker_mushroom_kanim", "mushroom"),
			new StickerBombs.Info("mermaid", STICKERNAMES.STICKER_MERMAID, PermitRarity.Universal, "sticker_mermaid_kanim", "mermaid"),
			new StickerBombs.Info("spacepet", STICKERNAMES.STICKER_SPACEPET, PermitRarity.Universal, "sticker_spacepet_kanim", "spacepet"),
			new StickerBombs.Info("spacepet2", STICKERNAMES.STICKER_SPACEPET2, PermitRarity.Universal, "sticker_spacepet2_kanim", "spacepet2"),
			new StickerBombs.Info("spacepet3", STICKERNAMES.STICKER_SPACEPET3, PermitRarity.Universal, "sticker_spacepet3_kanim", "spacepet3"),
			new StickerBombs.Info("spacepet4", STICKERNAMES.STICKER_SPACEPET4, PermitRarity.Universal, "sticker_spacepet4_kanim", "spacepet4"),
			new StickerBombs.Info("spacepet5", STICKERNAMES.STICKER_SPACEPET5, PermitRarity.Universal, "sticker_spacepet5_kanim", "spacepet5"),
			new StickerBombs.Info("unicorn", STICKERNAMES.STICKER_UNICORN, PermitRarity.Universal, "sticker_unicorn_kanim", "unicorn")
		};

		// Token: 0x04004D3D RID: 19773
		public static StickerBombs.Info[] Infos_Skins = new StickerBombs.Info[0];

		// Token: 0x04004D3E RID: 19774
		public static StickerBombs.Info[] Infos_All = StickerBombs.Infos_Default.Concat(StickerBombs.Infos_Skins);

		// Token: 0x02001C3D RID: 7229
		public struct Info
		{
			// Token: 0x06009CCA RID: 40138 RVA: 0x0034EDE3 File Offset: 0x0034CFE3
			public Info(string id, string stickerName, PermitRarity rarity, string animfilename, string sticker)
			{
				this.id = id;
				this.stickerName = stickerName;
				this.rarity = rarity;
				this.animfilename = animfilename;
				this.sticker = sticker;
			}

			// Token: 0x0400803F RID: 32831
			public string id;

			// Token: 0x04008040 RID: 32832
			public string stickerName;

			// Token: 0x04008041 RID: 32833
			public PermitRarity rarity;

			// Token: 0x04008042 RID: 32834
			public string animfilename;

			// Token: 0x04008043 RID: 32835
			public string sticker;
		}
	}
}
