using System;

namespace Database
{
	// Token: 0x02000CEB RID: 3307
	public readonly struct BalloonOverrideSymbol
	{
		// Token: 0x0600694A RID: 26954 RVA: 0x0027DBEC File Offset: 0x0027BDEC
		public BalloonOverrideSymbol(string animFileID, string animFileSymbolID)
		{
			if (string.IsNullOrEmpty(animFileID) || string.IsNullOrEmpty(animFileSymbolID))
			{
				this = default(BalloonOverrideSymbol);
				return;
			}
			this.animFileID = animFileID;
			this.animFileSymbolID = animFileSymbolID;
			this.animFile = Assets.GetAnim(animFileID);
			this.symbol = this.animFile.Value.GetData().build.GetSymbol(animFileSymbolID);
		}

		// Token: 0x0600694B RID: 26955 RVA: 0x0027DC60 File Offset: 0x0027BE60
		public void ApplyTo(BalloonArtist.Instance artist)
		{
			artist.SetBalloonSymbolOverride(this);
		}

		// Token: 0x0600694C RID: 26956 RVA: 0x0027DC6E File Offset: 0x0027BE6E
		public void ApplyTo(BalloonFX.Instance balloon)
		{
			balloon.SetBalloonSymbolOverride(this);
		}

		// Token: 0x040048F5 RID: 18677
		public readonly Option<KAnim.Build.Symbol> symbol;

		// Token: 0x040048F6 RID: 18678
		public readonly Option<KAnimFile> animFile;

		// Token: 0x040048F7 RID: 18679
		public readonly string animFileID;

		// Token: 0x040048F8 RID: 18680
		public readonly string animFileSymbolID;
	}
}
