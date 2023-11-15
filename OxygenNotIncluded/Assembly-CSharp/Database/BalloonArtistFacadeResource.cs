using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000CEA RID: 3306
	public class BalloonArtistFacadeResource : PermitResource
	{
		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06006940 RID: 26944 RVA: 0x0027DA4B File Offset: 0x0027BC4B
		// (set) Token: 0x06006941 RID: 26945 RVA: 0x0027DA53 File Offset: 0x0027BC53
		public string animFilename { get; private set; }

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06006942 RID: 26946 RVA: 0x0027DA5C File Offset: 0x0027BC5C
		// (set) Token: 0x06006943 RID: 26947 RVA: 0x0027DA64 File Offset: 0x0027BC64
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x06006944 RID: 26948 RVA: 0x0027DA70 File Offset: 0x0027BC70
		public BalloonArtistFacadeResource(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType) : base(id, name, desc, PermitCategory.JoyResponse, rarity)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.balloonFacadeType = balloonFacadeType;
			Db.Get().Accessories.AddAccessories(id, this.AnimFile);
			this.balloonOverrideSymbolIDs = this.GetBalloonOverrideSymbolIDs();
			Debug.Assert(this.balloonOverrideSymbolIDs.Length != 0);
		}

		// Token: 0x06006945 RID: 26949 RVA: 0x0027DAE0 File Offset: 0x0027BCE0
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.BALLOON_ARTIST_FACADE_FOR);
			return result;
		}

		// Token: 0x06006946 RID: 26950 RVA: 0x0027DB24 File Offset: 0x0027BD24
		public BalloonOverrideSymbol GetNextOverride()
		{
			int num = this.nextSymbolIndex;
			this.nextSymbolIndex = (this.nextSymbolIndex + 1) % this.balloonOverrideSymbolIDs.Length;
			return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[num]);
		}

		// Token: 0x06006947 RID: 26951 RVA: 0x0027DB62 File Offset: 0x0027BD62
		public BalloonOverrideSymbolIter GetSymbolIter()
		{
			return new BalloonOverrideSymbolIter(this);
		}

		// Token: 0x06006948 RID: 26952 RVA: 0x0027DB6F File Offset: 0x0027BD6F
		public BalloonOverrideSymbol GetOverrideAt(int index)
		{
			return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[index]);
		}

		// Token: 0x06006949 RID: 26953 RVA: 0x0027DB84 File Offset: 0x0027BD84
		private string[] GetBalloonOverrideSymbolIDs()
		{
			KAnim.Build build = this.AnimFile.GetData().build;
			BalloonArtistFacadeType balloonArtistFacadeType = this.balloonFacadeType;
			string[] result;
			if (balloonArtistFacadeType != BalloonArtistFacadeType.Single)
			{
				if (balloonArtistFacadeType != BalloonArtistFacadeType.ThreeSet)
				{
					throw new NotImplementedException();
				}
				result = new string[]
				{
					"body1",
					"body2",
					"body3"
				};
			}
			else
			{
				result = new string[]
				{
					"body"
				};
			}
			return result;
		}

		// Token: 0x040048F2 RID: 18674
		private BalloonArtistFacadeType balloonFacadeType;

		// Token: 0x040048F3 RID: 18675
		public readonly string[] balloonOverrideSymbolIDs;

		// Token: 0x040048F4 RID: 18676
		public int nextSymbolIndex;
	}
}
