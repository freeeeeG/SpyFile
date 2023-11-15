using System;
using TUNING;

namespace Database
{
	// Token: 0x02000CE5 RID: 3301
	public class ArtifactDropRates : ResourceSet<ArtifactDropRate>
	{
		// Token: 0x06006937 RID: 26935 RVA: 0x0027C3B3 File Offset: 0x0027A5B3
		public ArtifactDropRates(ResourceSet parent) : base("ArtifactDropRates", parent)
		{
			this.CreateDropRates();
		}

		// Token: 0x06006938 RID: 26936 RVA: 0x0027C3C8 File Offset: 0x0027A5C8
		private void CreateDropRates()
		{
			this.None = new ArtifactDropRate();
			this.None.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 1f);
			base.Add(this.None);
			this.Bad = new ArtifactDropRate();
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER0, 5f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER1, 3f);
			this.Bad.AddItem(DECOR.SPACEARTIFACT.TIER2, 2f);
			base.Add(this.Bad);
			this.Mediocre = new ArtifactDropRate();
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER1, 5f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER2, 3f);
			this.Mediocre.AddItem(DECOR.SPACEARTIFACT.TIER3, 2f);
			base.Add(this.Mediocre);
			this.Good = new ArtifactDropRate();
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER2, 5f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER3, 3f);
			this.Good.AddItem(DECOR.SPACEARTIFACT.TIER4, 2f);
			base.Add(this.Good);
			this.Great = new ArtifactDropRate();
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER3, 5f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER4, 3f);
			this.Great.AddItem(DECOR.SPACEARTIFACT.TIER5, 2f);
			base.Add(this.Great);
			this.Amazing = new ArtifactDropRate();
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER3, 3f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER4, 5f);
			this.Amazing.AddItem(DECOR.SPACEARTIFACT.TIER5, 2f);
			base.Add(this.Amazing);
			this.Perfect = new ArtifactDropRate();
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER_NONE, 10f);
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER4, 6f);
			this.Perfect.AddItem(DECOR.SPACEARTIFACT.TIER5, 4f);
			base.Add(this.Perfect);
		}

		// Token: 0x040048A3 RID: 18595
		public ArtifactDropRate None;

		// Token: 0x040048A4 RID: 18596
		public ArtifactDropRate Bad;

		// Token: 0x040048A5 RID: 18597
		public ArtifactDropRate Mediocre;

		// Token: 0x040048A6 RID: 18598
		public ArtifactDropRate Good;

		// Token: 0x040048A7 RID: 18599
		public ArtifactDropRate Great;

		// Token: 0x040048A8 RID: 18600
		public ArtifactDropRate Amazing;

		// Token: 0x040048A9 RID: 18601
		public ArtifactDropRate Perfect;
	}
}
