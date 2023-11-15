using System;

namespace Database
{
	// Token: 0x02000D24 RID: 3364
	public class DbStickerBomb : PermitResource
	{
		// Token: 0x06006A18 RID: 27160 RVA: 0x00294E8F File Offset: 0x0029308F
		public DbStickerBomb(string id, string stickerName, PermitRarity rarity, string animfilename, string sticker) : base(id, stickerName, "TODO:DbStickers", PermitCategory.Artwork, rarity)
		{
			this.id = id;
			this.sticker = sticker;
			this.animFile = Assets.GetAnim(animfilename);
		}

		// Token: 0x06006A19 RID: 27161 RVA: 0x00294EC4 File Offset: 0x002930C4
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			return new PermitPresentationInfo
			{
				sprite = Def.GetUISpriteFromMultiObjectAnim(this.animFile, string.Format("{0}_{1}", "idle_sticker", this.sticker), false, string.Format("{0}_{1}", "sticker", this.sticker))
			};
		}

		// Token: 0x04004D37 RID: 19767
		public string id;

		// Token: 0x04004D38 RID: 19768
		public string sticker;

		// Token: 0x04004D39 RID: 19769
		public KAnimFile animFile;

		// Token: 0x04004D3A RID: 19770
		private const string stickerAnimPrefix = "idle_sticker";

		// Token: 0x04004D3B RID: 19771
		private const string stickerSymbolPrefix = "sticker";
	}
}
