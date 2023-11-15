using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000B26 RID: 2854
public static class InventoryOrganization
{
	// Token: 0x060057DB RID: 22491 RVA: 0x00201FF8 File Offset: 0x002001F8
	public static string GetPermitSubcategory(PermitResource permit)
	{
		foreach (KeyValuePair<string, HashSet<string>> self in InventoryOrganization.subcategoryIdToPermitIdsMap)
		{
			string text;
			HashSet<string> hashSet;
			self.Deconstruct(out text, out hashSet);
			string result = text;
			if (hashSet.Contains(permit.Id))
			{
				return result;
			}
		}
		return "UNCATEGORIZED";
	}

	// Token: 0x060057DC RID: 22492 RVA: 0x00202068 File Offset: 0x00200268
	public static string GetCategoryName(string categoryId)
	{
		return Strings.Get("STRINGS.UI.KLEI_INVENTORY_SCREEN.TOP_LEVEL_CATEGORIES." + categoryId.ToUpper());
	}

	// Token: 0x060057DD RID: 22493 RVA: 0x00202084 File Offset: 0x00200284
	public static string GetSubcategoryName(string subcategoryId)
	{
		return Strings.Get("STRINGS.UI.KLEI_INVENTORY_SCREEN.SUBCATEGORIES." + subcategoryId.ToUpper());
	}

	// Token: 0x060057DE RID: 22494 RVA: 0x002020A0 File Offset: 0x002002A0
	public static void Initialize()
	{
		if (InventoryOrganization.initialized)
		{
			return;
		}
		InventoryOrganization.initialized = true;
		InventoryOrganization.GenerateTopLevelCategories();
		InventoryOrganization.GenerateSubcategories();
		foreach (KeyValuePair<string, List<string>> self in InventoryOrganization.categoryIdToSubcategoryIdsMap)
		{
			string text;
			List<string> list;
			self.Deconstruct(out text, out list);
			string key = text;
			List<string> list2 = list;
			bool value = true;
			foreach (string key2 in list2)
			{
				HashSet<string> hashSet;
				if (InventoryOrganization.subcategoryIdToPermitIdsMap.TryGetValue(key2, out hashSet) && hashSet.Count != 0)
				{
					value = false;
					break;
				}
			}
			InventoryOrganization.categoryIdToIsEmptyMap[key] = value;
		}
	}

	// Token: 0x060057DF RID: 22495 RVA: 0x00202174 File Offset: 0x00200374
	private static void AddTopLevelCategory(string categoryID, Sprite icon, string[] subcategoryIDs)
	{
		InventoryOrganization.categoryIdToSubcategoryIdsMap.Add(categoryID, new List<string>(subcategoryIDs));
		InventoryOrganization.categoryIdToIconMap.Add(categoryID, icon);
	}

	// Token: 0x060057E0 RID: 22496 RVA: 0x00202194 File Offset: 0x00200394
	private static void AddSubcategory(string subcategoryID, Sprite icon, int sortkey, string[] permitIDs)
	{
		if (InventoryOrganization.subcategoryIdToPermitIdsMap.ContainsKey(subcategoryID))
		{
			return;
		}
		InventoryOrganization.subcategoryIdToPresentationDataMap.Add(subcategoryID, new InventoryOrganization.SubcategoryPresentationData(subcategoryID, icon, sortkey));
		InventoryOrganization.subcategoryIdToPermitIdsMap.Add(subcategoryID, new HashSet<string>());
		for (int i = 0; i < permitIDs.Length; i++)
		{
			InventoryOrganization.subcategoryIdToPermitIdsMap[subcategoryID].Add(permitIDs[i]);
		}
	}

	// Token: 0x060057E1 RID: 22497 RVA: 0x002021F4 File Offset: 0x002003F4
	private static void GenerateTopLevelCategories()
	{
		InventoryOrganization.AddTopLevelCategory("CLOTHING_TOPS", Assets.GetSprite("icon_inventory_tops"), new string[]
		{
			"CLOTHING_TOPS_BASIC",
			"CLOTHING_TOPS_TSHIRT",
			"CLOTHING_TOPS_JACKET",
			"CLOTHING_TOPS_UNDERSHIRT"
		});
		InventoryOrganization.AddTopLevelCategory("CLOTHING_BOTTOMS", Assets.GetSprite("icon_inventory_bottoms"), new string[]
		{
			"CLOTHING_BOTTOMS_BASIC",
			"CLOTHING_BOTTOMS_FANCY",
			"CLOTHING_BOTTOMS_SHORTS",
			"CLOTHING_BOTTOMS_SKIRTS",
			"CLOTHING_BOTTOMS_UNDERWEAR"
		});
		InventoryOrganization.AddTopLevelCategory("CLOTHING_GLOVES", Assets.GetSprite("icon_inventory_gloves"), new string[]
		{
			"CLOTHING_GLOVES_BASIC",
			"CLOTHING_GLOVES_PRINTS",
			"CLOTHING_GLOVES_SHORT"
		});
		InventoryOrganization.AddTopLevelCategory("CLOTHING_SHOES", Assets.GetSprite("icon_inventory_shoes"), new string[]
		{
			"CLOTHING_SHOES_BASIC",
			"CLOTHING_SHOE_SOCKS"
		});
		InventoryOrganization.AddTopLevelCategory("ATMOSUITS", Assets.GetSprite("icon_inventory_atmosuit_helmet"), new string[]
		{
			"ATMOSUIT_BODIES_BASIC",
			"ATMOSUIT_BODIES_FANCY",
			"ATMOSUIT_HELMETS_BASIC",
			"ATMOSUIT_HELMETS_FANCY",
			"ATMOSUIT_BELTS_BASIC",
			"ATMOSUIT_BELTS_FANCY",
			"ATMOSUIT_GLOVES_BASIC",
			"ATMOSUIT_GLOVES_FANCY",
			"ATMOSUIT_SHOES_BASIC",
			"ATMOSUIT_SHOES_FANCY"
		});
		InventoryOrganization.AddTopLevelCategory("BUILDINGS", Assets.GetSprite("icon_inventory_buildings"), new string[]
		{
			"BUILDINGS_BED_COT",
			"BUILDINGS_BED_LUXURY",
			"BUILDINGS_FLOWER_VASE",
			"BUILDING_CEILING_LIGHT",
			"BUILDINGS_STORAGE",
			"BUILDINGS_INDUSTRIAL",
			"BUILDINGS_FOOD",
			"BUILDINGS_RECREATION"
		});
		InventoryOrganization.AddTopLevelCategory("WALLPAPERS", Def.GetFacadeUISprite("ExteriorWall_tropical"), new string[]
		{
			"BUILDING_WALLPAPER_BASIC",
			"BUILDING_WALLPAPER_FANCY",
			"BUILDING_WALLPAPER_PRINTS"
		});
		InventoryOrganization.AddTopLevelCategory("ARTWORK", Assets.GetSprite("icon_inventory_artworks"), new string[]
		{
			"BUILDING_CANVAS_STANDARD",
			"BUILDING_CANVAS_PORTRAIT",
			"BUILDING_CANVAS_LANDSCAPE",
			"BUILDING_SCULPTURE"
		});
		InventoryOrganization.AddTopLevelCategory("JOY_RESPONSES", Assets.GetSprite("icon_inventory_joyresponses"), new string[]
		{
			"JOY_BALLOON"
		});
	}

	// Token: 0x060057E2 RID: 22498 RVA: 0x00202458 File Offset: 0x00200658
	private static void GenerateSubcategories()
	{
		InventoryOrganization.AddSubcategory("BUILDING_CEILING_LIGHT", Def.GetUISprite("CeilingLight", "ui", false).first, 100, new string[]
		{
			"CeilingLight_mining",
			"CeilingLight_flower",
			"CeilingLight_polka_lamp_shade",
			"CeilingLight_burt_shower",
			"CeilingLight_ada_flask_round",
			"CeilingLight_rubiks"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_BED_COT", Def.GetUISprite("Bed", "ui", false).first, 200, new string[]
		{
			"Bed_star_curtain",
			"Bed_canopy",
			"Bed_rowan_tropical",
			"Bed_ada_science_lab"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_BED_LUXURY", Def.GetUISprite("LuxuryBed", "ui", false).first, 300, new string[]
		{
			"LuxuryBed_boat",
			"LuxuryBed_bouncy",
			"LuxuryBed_grandprix",
			"LuxuryBed_rocket",
			"LuxuryBed_puft",
			"LuxuryBed_hand",
			"LuxuryBed_rubiks"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_FLOWER_VASE", Def.GetUISprite("FlowerVase", "ui", false).first, 400, new string[]
		{
			"FlowerVase_retro",
			"FlowerVase_retro_red",
			"FlowerVase_retro_white",
			"FlowerVase_retro_green",
			"FlowerVase_retro_blue",
			"FlowerVaseWall_retro_green",
			"FlowerVaseWall_retro_yellow",
			"FlowerVaseWall_retro_red",
			"FlowerVaseWall_retro_blue",
			"FlowerVaseWall_retro_white",
			"FlowerVaseHanging_retro_red",
			"FlowerVaseHanging_retro_green",
			"FlowerVaseHanging_retro_blue",
			"FlowerVaseHanging_retro_yellow",
			"FlowerVaseHanging_retro_white",
			"FlowerVaseHanging_beaker",
			"FlowerVaseHanging_rubiks",
			"PlanterBox_mealwood",
			"PlanterBox_bristleblossom",
			"PlanterBox_wheezewort",
			"PlanterBox_sleetwheat",
			"PlanterBox_salmon_pink"
		});
		InventoryOrganization.AddSubcategory("BUILDING_WALLPAPER_BASIC", Assets.GetSprite("icon_inventory_solid_wallpapers"), 500, new string[]
		{
			"ExteriorWall_basic_white",
			"ExteriorWall_basic_blue_cobalt",
			"ExteriorWall_basic_green_kelly",
			"ExteriorWall_basic_grey_charcoal",
			"ExteriorWall_basic_orange_satsuma",
			"ExteriorWall_basic_pink_flamingo",
			"ExteriorWall_basic_red_deep",
			"ExteriorWall_basic_yellow_lemon",
			"ExteriorWall_pastel_pink",
			"ExteriorWall_pastel_yellow",
			"ExteriorWall_pastel_green",
			"ExteriorWall_pastel_blue",
			"ExteriorWall_pastel_purple"
		});
		InventoryOrganization.AddSubcategory("BUILDING_WALLPAPER_FANCY", Assets.GetSprite("icon_inventory_geometric_wallpapers"), 600, new string[]
		{
			"ExteriorWall_diagonal_red_deep_white",
			"ExteriorWall_diagonal_orange_satsuma_white",
			"ExteriorWall_diagonal_yellow_lemon_white",
			"ExteriorWall_diagonal_green_kelly_white",
			"ExteriorWall_diagonal_blue_cobalt_white",
			"ExteriorWall_diagonal_pink_flamingo_white",
			"ExteriorWall_diagonal_grey_charcoal_white",
			"ExteriorWall_circle_red_deep_white",
			"ExteriorWall_circle_orange_satsuma_white",
			"ExteriorWall_circle_yellow_lemon_white",
			"ExteriorWall_circle_green_kelly_white",
			"ExteriorWall_circle_blue_cobalt_white",
			"ExteriorWall_circle_pink_flamingo_white",
			"ExteriorWall_circle_grey_charcoal_white",
			"ExteriorWall_stripes_blue",
			"ExteriorWall_stripes_diagonal_blue",
			"ExteriorWall_stripes_circle_blue",
			"ExteriorWall_squares_red_deep_white",
			"ExteriorWall_squares_orange_satsuma_white",
			"ExteriorWall_squares_yellow_lemon_white",
			"ExteriorWall_squares_green_kelly_white",
			"ExteriorWall_squares_blue_cobalt_white",
			"ExteriorWall_squares_pink_flamingo_white",
			"ExteriorWall_squares_grey_charcoal_white",
			"ExteriorWall_plus_red_deep_white",
			"ExteriorWall_plus_orange_satsuma_white",
			"ExteriorWall_plus_yellow_lemon_white",
			"ExteriorWall_plus_green_kelly_white",
			"ExteriorWall_plus_blue_cobalt_white",
			"ExteriorWall_plus_pink_flamingo_white",
			"ExteriorWall_plus_grey_charcoal_white"
		});
		InventoryOrganization.AddSubcategory("BUILDING_WALLPAPER_PRINTS", Assets.GetSprite("icon_inventory_patterned_wallpapers"), 700, new string[]
		{
			"ExteriorWall_balm_lily",
			"ExteriorWall_clouds",
			"ExteriorWall_coffee",
			"ExteriorWall_mosaic",
			"ExteriorWall_mushbar",
			"ExteriorWall_plaid",
			"ExteriorWall_rain",
			"ExteriorWall_rainbow",
			"ExteriorWall_snow",
			"ExteriorWall_sun",
			"ExteriorWall_polka",
			"ExteriorWall_blueberries",
			"ExteriorWall_grapes",
			"ExteriorWall_lemon",
			"ExteriorWall_lime",
			"ExteriorWall_satsuma",
			"ExteriorWall_strawberry",
			"ExteriorWall_watermelon",
			"ExteriorWall_toiletpaper",
			"ExteriorWall_plunger",
			"ExteriorWall_tropical",
			"ExteriorWall_kitchen_retro1"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_RECREATION", Def.GetUISprite("WaterCooler", "ui", false).first, 700, new string[]
		{
			"WaterCooler_round_body",
			"ItemPedestal_hand",
			"MassageTable_shiatsu"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_STORAGE", Def.GetUISprite("RockCrusher", "ui", false).first, 800, new string[]
		{
			"StorageLocker_green_mush",
			"StorageLocker_red_rose",
			"StorageLocker_blue_babytears",
			"StorageLocker_purple_brainfat",
			"StorageLocker_yellow_tartar",
			"GasReservoir_lightgold",
			"GasReservoir_peagreen",
			"GasReservoir_lightcobalt",
			"GasReservoir_polka_darkpurpleresin",
			"GasReservoir_polka_darknavynookgreen"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_INDUSTRIAL", Def.GetUISprite("RockCrusher", "ui", false).first, 800, new string[]
		{
			"RockCrusher_hands",
			"RockCrusher_teeth",
			"RockCrusher_roundstamp",
			"RockCrusher_spikebeds"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_FOOD", Def.GetUISprite("EggCracker", "ui", false).first, 800, new string[]
		{
			"EggCracker_beaker",
			"EggCracker_flower",
			"EggCracker_hands"
		});
		InventoryOrganization.AddSubcategory("JOY_BALLOON", Db.Get().Permits.BalloonArtistFacades[0].GetPermitPresentationInfo().sprite, 100, new string[]
		{
			"BalloonRedFireEngineLongSparkles",
			"BalloonYellowLongSparkles",
			"BalloonBlueLongSparkles",
			"BalloonGreenLongSparkles",
			"BalloonPinkLongSparkles",
			"BalloonPurpleLongSparkles",
			"BalloonBabyPacuEgg",
			"BalloonBabyGlossyDreckoEgg",
			"BalloonBabyHatchEgg",
			"BalloonBabyPokeshellEgg",
			"BalloonBabyPuftEgg",
			"BalloonBabyShovoleEgg",
			"BalloonBabyPipEgg",
			"BalloonCandyBlueberry",
			"BalloonCandyGrape",
			"BalloonCandyLemon",
			"BalloonCandyLime",
			"BalloonCandyOrange",
			"BalloonCandyStrawberry",
			"BalloonCandyWatermelon",
			"BalloonHandGold"
		});
		InventoryOrganization.AddSubcategory("JOY_STICKER", Db.Get().Permits.StickerBombs[0].GetPermitPresentationInfo().sprite, 200, new string[]
		{
			"a",
			"b",
			"c",
			"d",
			"e",
			"f",
			"g",
			"h",
			"rocket",
			"paperplane",
			"plant",
			"plantpot",
			"mushroom",
			"mermaid",
			"spacepet",
			"spacepet2",
			"spacepet3",
			"spacepet4",
			"spacepet5",
			"unicorn"
		});
		InventoryOrganization.AddSubcategory("PRIMO_GARB", null, 200, new string[]
		{
			"clubshirt",
			"cummerbund",
			"decor_02",
			"decor_03",
			"decor_04",
			"decor_05",
			"gaudysweater",
			"limone",
			"mondrian",
			"overalls",
			"triangles",
			"workout"
		});
		InventoryOrganization.AddSubcategory("BUILDING_CANVAS_STANDARD", Def.GetUISprite("Canvas", "ui", false).first, 100, new string[]
		{
			"Canvas_Bad",
			"Canvas_Average",
			"Canvas_Good",
			"Canvas_Good2",
			"Canvas_Good3",
			"Canvas_Good4",
			"Canvas_Good5",
			"Canvas_Good6",
			"Canvas_Good7",
			"Canvas_Good8",
			"Canvas_Good9",
			"Canvas_Good10",
			"Canvas_Good11",
			"Canvas_Good13",
			"Canvas_Good12",
			"Canvas_Good14"
		});
		InventoryOrganization.AddSubcategory("BUILDING_CANVAS_PORTRAIT", Def.GetUISprite("CanvasTall", "ui", false).first, 200, new string[]
		{
			"CanvasTall_Bad",
			"CanvasTall_Average",
			"CanvasTall_Good",
			"CanvasTall_Good2",
			"CanvasTall_Good3",
			"CanvasTall_Good4",
			"CanvasTall_Good5",
			"CanvasTall_Good6",
			"CanvasTall_Good7",
			"CanvasTall_Good8",
			"CanvasTall_Good9",
			"CanvasTall_Good11",
			"CanvasTall_Good10"
		});
		InventoryOrganization.AddSubcategory("BUILDING_CANVAS_LANDSCAPE", Def.GetUISprite("CanvasWide", "ui", false).first, 300, new string[]
		{
			"CanvasWide_Bad",
			"CanvasWide_Average",
			"CanvasWide_Good",
			"CanvasWide_Good2",
			"CanvasWide_Good3",
			"CanvasWide_Good4",
			"CanvasWide_Good5",
			"CanvasWide_Good6",
			"CanvasWide_Good7",
			"CanvasWide_Good8",
			"CanvasWide_Good9",
			"CanvasWide_Good10",
			"CanvasWide_Good11"
		});
		InventoryOrganization.AddSubcategory("BUILDING_SCULPTURE", Def.GetUISprite("Sculpture", "ui", false).first, 400, new string[]
		{
			"Sculpture_Bad",
			"Sculpture_Average",
			"Sculpture_Good1",
			"Sculpture_Good2",
			"Sculpture_Good3",
			"Sculpture_Good5",
			"Sculpture_Good6",
			"SmallSculpture_Bad",
			"SmallSculpture_Average",
			"SmallSculpture_Good",
			"SmallSculpture_Good2",
			"SmallSculpture_Good3",
			"SmallSculpture_Good5",
			"SmallSculpture_Good6",
			"IceSculpture_Bad",
			"IceSculpture_Average",
			"MarbleSculpture_Bad",
			"MarbleSculpture_Average",
			"MarbleSculpture_Good1",
			"MarbleSculpture_Good2",
			"MarbleSculpture_Good3",
			"MetalSculpture_Bad",
			"MetalSculpture_Average",
			"MetalSculpture_Good1",
			"MetalSculpture_Good2",
			"MetalSculpture_Good3",
			"MetalSculpture_Good5",
			"Sculpture_Good4",
			"SmallSculpture_Good4",
			"MetalSculpture_Good4",
			"MarbleSculpture_Good4",
			"MarbleSculpture_Good5",
			"IceSculpture_Average2",
			"IceSculpture_Average3"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_TOPS_BASIC", Assets.GetSprite("icon_inventory_basic_shirts"), 100, new string[]
		{
			"TopBasicBlack",
			"TopBasicWhite",
			"TopBasicRed",
			"TopBasicOrange",
			"TopBasicYellow",
			"TopBasicGreen",
			"TopBasicAqua",
			"TopBasicPurple",
			"TopBasicPinkOrchid"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_TOPS_TSHIRT", Assets.GetSprite("icon_inventory_tees"), 300, new string[]
		{
			"TopRaglanDeepRed",
			"TopRaglanCobalt",
			"TopRaglanFlamingo",
			"TopRaglanKellyGreen",
			"TopRaglanCharcoal",
			"TopRaglanLemon",
			"TopRaglanSatsuma",
			"TopTShirtWhite",
			"TopTShirtMagenta"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_TOPS_UNDERSHIRT", Assets.GetSprite("icon_inventory_undershirts"), 400, new string[]
		{
			"TopUndershirtExecutive",
			"TopUndershirtUnderling",
			"TopUndershirtGroupthink",
			"TopUndershirtStakeholder",
			"TopUndershirtAdmin",
			"TopUndershirtBuzzword",
			"TopUndershirtSynergy"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_TOPS_JACKET", Assets.GetSprite("icon_inventory_jackets"), 500, new string[]
		{
			"TopJellypuffJacketBlueberry",
			"TopJellypuffJacketGrape",
			"TopJellypuffJacketLemon",
			"TopJellypuffJacketLime",
			"TopJellypuffJacketSatsuma",
			"TopJellypuffJacketStrawberry",
			"TopJellypuffJacketWatermelon",
			"TopAthlete",
			"TopCircuitGreen",
			"TopResearcher",
			"TopDenimBlue",
			"TopRebelGi"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_BOTTOMS_BASIC", Assets.GetSprite("icon_inventory_basic_pants"), 100, new string[]
		{
			"BottomBasicBlack",
			"BottomBasicWhite",
			"BottomBasicRed",
			"BottomBasicOrange",
			"BottomBasicYellow",
			"BottomBasicGreen",
			"BottomBasicAqua",
			"BottomBasicPurple",
			"BottomBasicPinkOrchid",
			"PantsBasicRedOrange",
			"PantsBasicLightBrown"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_BOTTOMS_FANCY", Assets.GetSprite("icon_inventory_fancy_pants"), 200, new string[]
		{
			"PantsAthlete",
			"PantsCircuitGreen",
			"PantsJeans",
			"PantsRebelGi",
			"PantsResearch"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_BOTTOMS_SHORTS", Assets.GetSprite("icon_inventory_shorts"), 300, new string[]
		{
			"ShortsBasicDeepRed",
			"ShortsBasicSatsuma",
			"ShortsBasicYellowcake",
			"ShortsBasicKellyGreen",
			"ShortsBasicBlueCobalt",
			"ShortsBasicPinkFlamingo",
			"ShortsBasicCharcoal"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_BOTTOMS_SKIRTS", Assets.GetSprite("icon_inventory_skirts"), 300, new string[]
		{
			"SkirtBasicBlueMiddle",
			"SkirtBasicPurple",
			"SkirtBasicGreen",
			"SkirtBasicOrange",
			"SkirtBasicPinkOrchid",
			"SkirtBasicRed",
			"SkirtBasicYellow",
			"SkirtBasicPolkadot",
			"SkirtBasicWatermelon",
			"SkirtDenimBlue",
			"SkirtLeopardPrintBluePink",
			"SkirtSparkleBlue"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_BOTTOMS_UNDERWEAR", Assets.GetSprite("icon_inventory_underwear"), 300, new string[]
		{
			"BottomBriefsExecutive",
			"BottomBriefsUnderling",
			"BottomBriefsGroupthink",
			"BottomBriefsStakeholder",
			"BottomBriefsAdmin",
			"BottomBriefsBuzzword",
			"BottomBriefsSynergy"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_GLOVES_BASIC", Assets.GetSprite("icon_inventory_basic_gloves"), 100, new string[]
		{
			"GlovesBasicBlack",
			"GlovesBasicWhite",
			"GlovesBasicRed",
			"GlovesBasicOrange",
			"GlovesBasicYellow",
			"GlovesBasicGreen",
			"GlovesBasicAqua",
			"GlovesBasicPurple",
			"GlovesBasicPinkOrchid",
			"GlovesBasicBlueGrey",
			"GlovesBasicBrownKhaki"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_GLOVES_SHORT", Assets.GetSprite("icon_inventory_short_gloves"), 200, new string[]
		{
			"GlovesCufflessBlueberry",
			"GlovesCufflessGrape",
			"GlovesCufflessLemon",
			"GlovesCufflessLime",
			"GlovesCufflessSatsuma",
			"GlovesCufflessStrawberry",
			"GlovesCufflessWatermelon",
			"GlovesCufflessBlack"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_GLOVES_PRINTS", Assets.GetSprite("icon_inventory_specialty_gloves"), 300, new string[]
		{
			"GlovesAthlete",
			"GlovesCircuitGreen",
			"GlovesAthleticRedDeep",
			"GlovesAthleticOrangeSatsuma",
			"GlovesAthleticYellowLemon",
			"GlovesAthleticGreenKelly",
			"GlovesAthleticBlueCobalt",
			"GlovesAthleticPinkFlamingo",
			"GlovesAthleticGreyCharcoal",
			"GlovesDenimBlue"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_SHOES_BASIC", Assets.GetSprite("icon_inventory_basic_shoes"), 100, new string[]
		{
			"ShoesBasicBlack",
			"ShoesBasicWhite",
			"ShoesBasicRed",
			"ShoesBasicOrange",
			"ShoesBasicYellow",
			"ShoesBasicGreen",
			"ShoesBasicAqua",
			"ShoesBasicPurple",
			"ShoesBasicPinkOrchid",
			"ShoesBasicBlueGrey",
			"ShoesBasicTan",
			"ShoesBasicGray",
			"ShoesDenimBlue"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_SHOE_SOCKS", Assets.GetSprite("icon_inventory_socks"), 500, new string[]
		{
			"SocksAthleticDeepRed",
			"SocksAthleticOrangeSatsuma",
			"SocksAthleticYellowLemon",
			"SocksAthleticGreenKelly",
			"SocksAthleticBlueCobalt",
			"SocksAthleticPinkFlamingo",
			"SocksAthleticGreyCharcoal",
			"SocksLegwarmersBlueberry",
			"SocksLegwarmersGrape",
			"SocksLegwarmersLemon",
			"SocksLegwarmersLime",
			"SocksLegwarmersSatsuma",
			"SocksLegwarmersStrawberry",
			"SocksLegwarmersWatermelon"
		});
		InventoryOrganization.AddSubcategory("ATMOSUIT_BODIES_BASIC", Assets.GetSprite("icon_inventory_atmosuit_body"), 100, new string[]
		{
			"AtmoSuitBasicYellow",
			"AtmoSuitSparkleRed",
			"AtmoSuitSparkleGreen",
			"AtmoSuitSparkleBlue",
			"AtmoSuitSparkleLavender",
			"AtmoSuitPuft",
			"AtmoSuitConfetti",
			"AtmoSuitCrispEggplant",
			"AtmoSuitBasicNeonPink",
			"AtmoSuitMultiRedBlack"
		});
		InventoryOrganization.AddSubcategory("ATMOSUIT_HELMETS_BASIC", Assets.GetSprite("icon_inventory_atmosuit_helmet"), 300, new string[]
		{
			"AtmoHelmetLimone",
			"AtmoHelmetSparkleRed",
			"AtmoHelmetSparkleGreen",
			"AtmoHelmetSparkleBlue",
			"AtmoHelmetSparklePurple",
			"AtmoHelmetPuft",
			"AtmoHelmetConfetti",
			"AtmoHelmetEggplant",
			"AtmoHelmetCummerbundRed",
			"AtmoHelmetWorkoutLavender"
		});
		InventoryOrganization.AddSubcategory("ATMOSUIT_GLOVES_BASIC", Assets.GetSprite("icon_inventory_atmosuit_gloves"), 500, new string[]
		{
			"AtmoGlovesLime",
			"AtmoGlovesSparkleRed",
			"AtmoGlovesSparkleGreen",
			"AtmoGlovesSparkleBlue",
			"AtmoGlovesSparkleLavender",
			"AtmoGlovesPuft",
			"AtmoGlovesGold",
			"AtmoGlovesEggplant",
			"AtmoGlovesWhite",
			"AtmoGlovesStripesLavender"
		});
		InventoryOrganization.AddSubcategory("ATMOSUIT_BELTS_BASIC", Assets.GetSprite("icon_inventory_atmosuit_belt"), 700, new string[]
		{
			"AtmoBeltBasicLime",
			"AtmoBeltSparkleRed",
			"AtmoBeltSparkleGreen",
			"AtmoBeltSparkleBlue",
			"AtmoBeltSparkleLavender",
			"AtmoBeltPuft",
			"AtmoBeltBasicGold",
			"AtmoBeltEggplant",
			"AtmoBeltBasicGrey",
			"AtmoBeltBasicNeonPink"
		});
		InventoryOrganization.AddSubcategory("ATMOSUIT_SHOES_BASIC", Assets.GetSprite("icon_inventory_atmosuit_boots"), 900, new string[]
		{
			"AtmoShoesBasicYellow",
			"AtmoShoesSparkleBlack",
			"AtmoShoesPuft",
			"AtmoShoesStealth",
			"AtmoShoesEggplant",
			"AtmoShoesBasicLavender"
		});
	}

	// Token: 0x04003B69 RID: 15209
	public static Dictionary<string, List<string>> categoryIdToSubcategoryIdsMap = new Dictionary<string, List<string>>();

	// Token: 0x04003B6A RID: 15210
	public static Dictionary<string, Sprite> categoryIdToIconMap = new Dictionary<string, Sprite>();

	// Token: 0x04003B6B RID: 15211
	public static Dictionary<string, bool> categoryIdToIsEmptyMap = new Dictionary<string, bool>();

	// Token: 0x04003B6C RID: 15212
	public static bool initialized = false;

	// Token: 0x04003B6D RID: 15213
	public static Dictionary<string, HashSet<string>> subcategoryIdToPermitIdsMap = new Dictionary<string, HashSet<string>>
	{
		{
			"UNCATEGORIZED",
			new HashSet<string>()
		},
		{
			"YAML",
			new HashSet<string>()
		}
	};

	// Token: 0x04003B6E RID: 15214
	public static Dictionary<string, InventoryOrganization.SubcategoryPresentationData> subcategoryIdToPresentationDataMap = new Dictionary<string, InventoryOrganization.SubcategoryPresentationData>
	{
		{
			"UNCATEGORIZED",
			new InventoryOrganization.SubcategoryPresentationData("UNCATEGORIZED", Assets.GetSprite("error_message"), 0)
		},
		{
			"YAML",
			new InventoryOrganization.SubcategoryPresentationData("YAML", Assets.GetSprite("error_message"), 0)
		}
	};

	// Token: 0x02001A25 RID: 6693
	public class SubcategoryPresentationData
	{
		// Token: 0x0600963A RID: 38458 RVA: 0x0033C1EF File Offset: 0x0033A3EF
		public SubcategoryPresentationData(string subcategoryID, Sprite icon, int sortKey)
		{
			this.subcategoryID = subcategoryID;
			this.sortKey = sortKey;
			this.icon = icon;
		}

		// Token: 0x04007872 RID: 30834
		public string subcategoryID;

		// Token: 0x04007873 RID: 30835
		public int sortKey;

		// Token: 0x04007874 RID: 30836
		public Sprite icon;
	}

	// Token: 0x02001A26 RID: 6694
	public static class InventoryPermitCategories
	{
		// Token: 0x04007875 RID: 30837
		public const string CLOTHING_TOPS = "CLOTHING_TOPS";

		// Token: 0x04007876 RID: 30838
		public const string CLOTHING_BOTTOMS = "CLOTHING_BOTTOMS";

		// Token: 0x04007877 RID: 30839
		public const string CLOTHING_GLOVES = "CLOTHING_GLOVES";

		// Token: 0x04007878 RID: 30840
		public const string CLOTHING_SHOES = "CLOTHING_SHOES";

		// Token: 0x04007879 RID: 30841
		public const string ATMOSUITS = "ATMOSUITS";

		// Token: 0x0400787A RID: 30842
		public const string BUILDINGS = "BUILDINGS";

		// Token: 0x0400787B RID: 30843
		public const string WALLPAPERS = "WALLPAPERS";

		// Token: 0x0400787C RID: 30844
		public const string ARTWORK = "ARTWORK";

		// Token: 0x0400787D RID: 30845
		public const string JOY_RESPONSES = "JOY_RESPONSES";

		// Token: 0x0400787E RID: 30846
		public const string ATMO_SUIT_HELMET = "ATMO_SUIT_HELMET";

		// Token: 0x0400787F RID: 30847
		public const string ATMO_SUIT_BODY = "ATMO_SUIT_BODY";

		// Token: 0x04007880 RID: 30848
		public const string ATMO_SUIT_GLOVES = "ATMO_SUIT_GLOVES";

		// Token: 0x04007881 RID: 30849
		public const string ATMO_SUIT_BELT = "ATMO_SUIT_BELT";

		// Token: 0x04007882 RID: 30850
		public const string ATMO_SUIT_SHOES = "ATMO_SUIT_SHOES";
	}

	// Token: 0x02001A27 RID: 6695
	public static class PermitSubcategories
	{
		// Token: 0x04007883 RID: 30851
		public const string YAML = "YAML";

		// Token: 0x04007884 RID: 30852
		public const string UNCATEGORIZED = "UNCATEGORIZED";

		// Token: 0x04007885 RID: 30853
		public const string JOY_BALLOON = "JOY_BALLOON";

		// Token: 0x04007886 RID: 30854
		public const string JOY_STICKER = "JOY_STICKER";

		// Token: 0x04007887 RID: 30855
		public const string PRIMO_GARB = "PRIMO_GARB";

		// Token: 0x04007888 RID: 30856
		public const string CLOTHING_TOPS_BASIC = "CLOTHING_TOPS_BASIC";

		// Token: 0x04007889 RID: 30857
		public const string CLOTHING_TOPS_TSHIRT = "CLOTHING_TOPS_TSHIRT";

		// Token: 0x0400788A RID: 30858
		public const string CLOTHING_TOPS_JACKET = "CLOTHING_TOPS_JACKET";

		// Token: 0x0400788B RID: 30859
		public const string CLOTHING_TOPS_UNDERSHIRT = "CLOTHING_TOPS_UNDERSHIRT";

		// Token: 0x0400788C RID: 30860
		public const string CLOTHING_BOTTOMS_BASIC = "CLOTHING_BOTTOMS_BASIC";

		// Token: 0x0400788D RID: 30861
		public const string CLOTHING_BOTTOMS_FANCY = "CLOTHING_BOTTOMS_FANCY";

		// Token: 0x0400788E RID: 30862
		public const string CLOTHING_BOTTOMS_SHORTS = "CLOTHING_BOTTOMS_SHORTS";

		// Token: 0x0400788F RID: 30863
		public const string CLOTHING_BOTTOMS_SKIRTS = "CLOTHING_BOTTOMS_SKIRTS";

		// Token: 0x04007890 RID: 30864
		public const string CLOTHING_BOTTOMS_UNDERWEAR = "CLOTHING_BOTTOMS_UNDERWEAR";

		// Token: 0x04007891 RID: 30865
		public const string CLOTHING_GLOVES_BASIC = "CLOTHING_GLOVES_BASIC";

		// Token: 0x04007892 RID: 30866
		public const string CLOTHING_GLOVES_PRINTS = "CLOTHING_GLOVES_PRINTS";

		// Token: 0x04007893 RID: 30867
		public const string CLOTHING_GLOVES_SHORT = "CLOTHING_GLOVES_SHORT";

		// Token: 0x04007894 RID: 30868
		public const string CLOTHING_SHOES_BASIC = "CLOTHING_SHOES_BASIC";

		// Token: 0x04007895 RID: 30869
		public const string CLOTHING_SHOE_SOCKS = "CLOTHING_SHOE_SOCKS";

		// Token: 0x04007896 RID: 30870
		public const string ATMOSUIT_HELMETS_BASIC = "ATMOSUIT_HELMETS_BASIC";

		// Token: 0x04007897 RID: 30871
		public const string ATMOSUIT_HELMETS_FANCY = "ATMOSUIT_HELMETS_FANCY";

		// Token: 0x04007898 RID: 30872
		public const string ATMOSUIT_BODIES_BASIC = "ATMOSUIT_BODIES_BASIC";

		// Token: 0x04007899 RID: 30873
		public const string ATMOSUIT_BODIES_FANCY = "ATMOSUIT_BODIES_FANCY";

		// Token: 0x0400789A RID: 30874
		public const string ATMOSUIT_GLOVES_BASIC = "ATMOSUIT_GLOVES_BASIC";

		// Token: 0x0400789B RID: 30875
		public const string ATMOSUIT_GLOVES_FANCY = "ATMOSUIT_GLOVES_FANCY";

		// Token: 0x0400789C RID: 30876
		public const string ATMOSUIT_BELTS_BASIC = "ATMOSUIT_BELTS_BASIC";

		// Token: 0x0400789D RID: 30877
		public const string ATMOSUIT_BELTS_FANCY = "ATMOSUIT_BELTS_FANCY";

		// Token: 0x0400789E RID: 30878
		public const string ATMOSUIT_SHOES_BASIC = "ATMOSUIT_SHOES_BASIC";

		// Token: 0x0400789F RID: 30879
		public const string ATMOSUIT_SHOES_FANCY = "ATMOSUIT_SHOES_FANCY";

		// Token: 0x040078A0 RID: 30880
		public const string BUILDING_WALLPAPER_BASIC = "BUILDING_WALLPAPER_BASIC";

		// Token: 0x040078A1 RID: 30881
		public const string BUILDING_WALLPAPER_FANCY = "BUILDING_WALLPAPER_FANCY";

		// Token: 0x040078A2 RID: 30882
		public const string BUILDING_WALLPAPER_PRINTS = "BUILDING_WALLPAPER_PRINTS";

		// Token: 0x040078A3 RID: 30883
		public const string BUILDINGS_STORAGE = "BUILDINGS_STORAGE";

		// Token: 0x040078A4 RID: 30884
		public const string BUILDINGS_INDUSTRIAL = "BUILDINGS_INDUSTRIAL";

		// Token: 0x040078A5 RID: 30885
		public const string BUILDINGS_FOOD = "BUILDINGS_FOOD";

		// Token: 0x040078A6 RID: 30886
		public const string BUILDINGS_RECREATION = "BUILDINGS_RECREATION";

		// Token: 0x040078A7 RID: 30887
		public const string BUILDING_CANVAS_STANDARD = "BUILDING_CANVAS_STANDARD";

		// Token: 0x040078A8 RID: 30888
		public const string BUILDING_CANVAS_PORTRAIT = "BUILDING_CANVAS_PORTRAIT";

		// Token: 0x040078A9 RID: 30889
		public const string BUILDING_CANVAS_LANDSCAPE = "BUILDING_CANVAS_LANDSCAPE";

		// Token: 0x040078AA RID: 30890
		public const string BUILDING_SCULPTURE = "BUILDING_SCULPTURE";

		// Token: 0x040078AB RID: 30891
		public const string MONUMENT_PARTS = "MONUMENT_PARTS";

		// Token: 0x040078AC RID: 30892
		public const string BUILDINGS_FLOWER_VASE = "BUILDINGS_FLOWER_VASE";

		// Token: 0x040078AD RID: 30893
		public const string BUILDINGS_BED_COT = "BUILDINGS_BED_COT";

		// Token: 0x040078AE RID: 30894
		public const string BUILDINGS_BED_LUXURY = "BUILDINGS_BED_LUXURY";

		// Token: 0x040078AF RID: 30895
		public const string BUILDING_CEILING_LIGHT = "BUILDING_CEILING_LIGHT";
	}
}
