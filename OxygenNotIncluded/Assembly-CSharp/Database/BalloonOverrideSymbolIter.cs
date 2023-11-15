using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000CEC RID: 3308
	public class BalloonOverrideSymbolIter
	{
		// Token: 0x0600694D RID: 26957 RVA: 0x0027DC7C File Offset: 0x0027BE7C
		public BalloonOverrideSymbolIter(Option<BalloonArtistFacadeResource> facade)
		{
			global::Debug.Assert(facade.IsNone() || facade.Unwrap().balloonOverrideSymbolIDs.Length != 0);
			this.facade = facade;
			if (facade.IsSome())
			{
				this.index = UnityEngine.Random.Range(0, facade.Unwrap().balloonOverrideSymbolIDs.Length);
			}
			this.Next();
		}

		// Token: 0x0600694E RID: 26958 RVA: 0x0027DCE1 File Offset: 0x0027BEE1
		public BalloonOverrideSymbol Current()
		{
			return this.current;
		}

		// Token: 0x0600694F RID: 26959 RVA: 0x0027DCEC File Offset: 0x0027BEEC
		public BalloonOverrideSymbol Next()
		{
			if (this.facade.IsSome())
			{
				BalloonArtistFacadeResource balloonArtistFacadeResource = this.facade.Unwrap();
				this.current = new BalloonOverrideSymbol(balloonArtistFacadeResource.animFilename, balloonArtistFacadeResource.balloonOverrideSymbolIDs[this.index]);
				this.index = (this.index + 1) % balloonArtistFacadeResource.balloonOverrideSymbolIDs.Length;
				return this.current;
			}
			return default(BalloonOverrideSymbol);
		}

		// Token: 0x040048F9 RID: 18681
		public readonly Option<BalloonArtistFacadeResource> facade;

		// Token: 0x040048FA RID: 18682
		private BalloonOverrideSymbol current;

		// Token: 0x040048FB RID: 18683
		private int index;
	}
}
