using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000D11 RID: 3345
	public class PermitResources : ResourceSet<PermitResource>
	{
		// Token: 0x060069DC RID: 27100 RVA: 0x00290F58 File Offset: 0x0028F158
		public PermitResources(ResourceSet parent) : base("PermitResources", parent)
		{
			this.Root = new ResourceSet<Resource>("Root", null);
			this.Permits = new Dictionary<string, IEnumerable<PermitResource>>();
			this.BuildingFacades = new BuildingFacades(this.Root);
			this.Permits.Add(this.BuildingFacades.Id, this.BuildingFacades.resources);
			this.EquippableFacades = new EquippableFacades(this.Root);
			this.Permits.Add(this.EquippableFacades.Id, this.EquippableFacades.resources);
			this.ArtableStages = new ArtableStages(this.Root);
			this.Permits.Add(this.ArtableStages.Id, this.ArtableStages.resources);
			this.StickerBombs = new StickerBombs(this.Root);
			this.Permits.Add(this.StickerBombs.Id, this.StickerBombs.resources);
			this.ClothingItems = new ClothingItems(this.Root);
			this.ClothingOutfits = new ClothingOutfits(this.Root, this.ClothingItems);
			this.Permits.Add(this.ClothingItems.Id, this.ClothingItems.resources);
			this.BalloonArtistFacades = new BalloonArtistFacades(this.Root);
			this.Permits.Add(this.BalloonArtistFacades.Id, this.BalloonArtistFacades.resources);
			this.MonumentParts = new MonumentParts(this.Root);
			foreach (IEnumerable<PermitResource> collection in this.Permits.Values)
			{
				this.resources.AddRange(collection);
			}
		}

		// Token: 0x060069DD RID: 27101 RVA: 0x00291134 File Offset: 0x0028F334
		public void PostProcess()
		{
			this.BuildingFacades.PostProcess();
		}

		// Token: 0x04004C8F RID: 19599
		public ResourceSet Root;

		// Token: 0x04004C90 RID: 19600
		public BuildingFacades BuildingFacades;

		// Token: 0x04004C91 RID: 19601
		public EquippableFacades EquippableFacades;

		// Token: 0x04004C92 RID: 19602
		public ArtableStages ArtableStages;

		// Token: 0x04004C93 RID: 19603
		public StickerBombs StickerBombs;

		// Token: 0x04004C94 RID: 19604
		public ClothingItems ClothingItems;

		// Token: 0x04004C95 RID: 19605
		public ClothingOutfits ClothingOutfits;

		// Token: 0x04004C96 RID: 19606
		public MonumentParts MonumentParts;

		// Token: 0x04004C97 RID: 19607
		public BalloonArtistFacades BalloonArtistFacades;

		// Token: 0x04004C98 RID: 19608
		public Dictionary<string, IEnumerable<PermitResource>> Permits;
	}
}
