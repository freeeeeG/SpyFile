using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D0C RID: 3340
	public static class PermitCategories
	{
		// Token: 0x060069CB RID: 27083 RVA: 0x00290A92 File Offset: 0x0028EC92
		public static string GetDisplayName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].displayName;
		}

		// Token: 0x060069CC RID: 27084 RVA: 0x00290AA4 File Offset: 0x0028ECA4
		public static string GetUppercaseDisplayName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].displayName.ToUpper();
		}

		// Token: 0x060069CD RID: 27085 RVA: 0x00290ABB File Offset: 0x0028ECBB
		public static string GetIconName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].iconName;
		}

		// Token: 0x060069CE RID: 27086 RVA: 0x00290AD0 File Offset: 0x0028ECD0
		public static PermitCategory GetCategoryForId(string id)
		{
			try
			{
				return (PermitCategory)Enum.Parse(typeof(PermitCategory), id);
			}
			catch (ArgumentException)
			{
				Debug.LogError(id + " is not a valid PermitCategory.");
			}
			return PermitCategory.Equipment;
		}

		// Token: 0x060069CF RID: 27087 RVA: 0x00290B1C File Offset: 0x0028ED1C
		public static Option<ClothingOutfitUtility.OutfitType> GetOutfitTypeFor(PermitCategory permitCategory)
		{
			return PermitCategories.CategoryInfos[permitCategory].outfitType;
		}

		// Token: 0x04004C81 RID: 19585
		private static Dictionary<PermitCategory, PermitCategories.CategoryInfo> CategoryInfos = new Dictionary<PermitCategory, PermitCategories.CategoryInfo>
		{
			{
				PermitCategory.Equipment,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.EQUIPMENT, "icon_inventory_equipment", Option.None)
			},
			{
				PermitCategory.DupeTops,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_TOPS, "icon_inventory_tops", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeBottoms,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_BOTTOMS, "icon_inventory_bottoms", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeGloves,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_GLOVES, "icon_inventory_gloves", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeShoes,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_SHOES, "icon_inventory_shoes", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeHats,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_HATS, "icon_inventory_hats", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeAccessories,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_ACCESSORIES, "icon_inventory_accessories", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.AtmoSuitHelmet,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_HELMET, "icon_inventory_atmosuit_helmet", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitBody,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BODY, "icon_inventory_atmosuit_body", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitGloves,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_GLOVES, "icon_inventory_atmosuit_gloves", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitBelt,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BELT, "icon_inventory_atmosuit_belt", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitShoes,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_SHOES, "icon_inventory_atmosuit_boots", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.Building,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.BUILDINGS, "icon_inventory_buildings", Option.None)
			},
			{
				PermitCategory.Critter,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.CRITTERS, "icon_inventory_critters", Option.None)
			},
			{
				PermitCategory.Sweepy,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.SWEEPYS, "icon_inventory_sweepys", Option.None)
			},
			{
				PermitCategory.Duplicant,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPLICANTS, "icon_inventory_duplicants", Option.None)
			},
			{
				PermitCategory.Artwork,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ARTWORKS, "icon_inventory_artworks", Option.None)
			},
			{
				PermitCategory.JoyResponse,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSE, "icon_inventory_joyresponses", ClothingOutfitUtility.OutfitType.JoyResponse)
			}
		};

		// Token: 0x02001C33 RID: 7219
		private class CategoryInfo
		{
			// Token: 0x06009CAF RID: 40111 RVA: 0x0034EB9A File Offset: 0x0034CD9A
			public CategoryInfo(string displayName, string iconName, Option<ClothingOutfitUtility.OutfitType> outfitType)
			{
				this.displayName = displayName;
				this.iconName = iconName;
				this.outfitType = outfitType;
			}

			// Token: 0x04008007 RID: 32775
			public string displayName;

			// Token: 0x04008008 RID: 32776
			public string iconName;

			// Token: 0x04008009 RID: 32777
			public Option<ClothingOutfitUtility.OutfitType> outfitType;
		}
	}
}
