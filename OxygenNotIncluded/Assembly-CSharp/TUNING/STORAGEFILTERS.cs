using System;
using System.Collections.Generic;
using System.Linq;

namespace TUNING
{
	// Token: 0x02000D9B RID: 3483
	public class STORAGEFILTERS
	{
		// Token: 0x04005068 RID: 20584
		public static List<Tag> FOOD = new List<Tag>
		{
			GameTags.Edible,
			GameTags.CookingIngredient,
			GameTags.Medicine
		};

		// Token: 0x04005069 RID: 20585
		public static List<Tag> BAGABLE_CREATURES = new List<Tag>
		{
			GameTags.BagableCreature
		};

		// Token: 0x0400506A RID: 20586
		public static List<Tag> SWIMMING_CREATURES = new List<Tag>
		{
			GameTags.SwimmingCreature
		};

		// Token: 0x0400506B RID: 20587
		public static List<Tag> NOT_EDIBLE_SOLIDS = new List<Tag>
		{
			GameTags.Alloy,
			GameTags.RefinedMetal,
			GameTags.Metal,
			GameTags.BuildableRaw,
			GameTags.BuildableProcessed,
			GameTags.Farmable,
			GameTags.Organics,
			GameTags.Compostable,
			GameTags.Seed,
			GameTags.Agriculture,
			GameTags.Filter,
			GameTags.ConsumableOre,
			GameTags.Liquifiable,
			GameTags.IndustrialProduct,
			GameTags.IndustrialIngredient,
			GameTags.MedicalSupplies,
			GameTags.Clothes,
			GameTags.ManufacturedMaterial,
			GameTags.Egg,
			GameTags.RareMaterials,
			GameTags.Other,
			GameTags.StoryTraitResource
		};

		// Token: 0x0400506C RID: 20588
		public static List<Tag> LIQUIDS = new List<Tag>
		{
			GameTags.Liquid
		};

		// Token: 0x0400506D RID: 20589
		public static List<Tag> GASES = new List<Tag>
		{
			GameTags.Breathable,
			GameTags.Unbreathable
		};

		// Token: 0x0400506E RID: 20590
		public static List<Tag> PAYLOADS = new List<Tag>
		{
			"RailGunPayload"
		};

		// Token: 0x0400506F RID: 20591
		public static Tag[] SOLID_TRANSFER_ARM_CONVEYABLE = new List<Tag>
		{
			GameTags.Seed,
			GameTags.CropSeed
		}.Concat(STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Concat(STORAGEFILTERS.FOOD).Concat(STORAGEFILTERS.PAYLOADS)).ToArray<Tag>();
	}
}
