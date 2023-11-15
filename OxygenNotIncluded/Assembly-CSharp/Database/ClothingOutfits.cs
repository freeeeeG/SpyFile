using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000CF6 RID: 3318
	public class ClothingOutfits : ResourceSet<ClothingOutfitResource>
	{
		// Token: 0x0600697E RID: 27006 RVA: 0x00288E90 File Offset: 0x00287090
		public ClothingOutfits(ResourceSet parent, ClothingItems items_resource) : base("ClothingOutfits", parent)
		{
			base.Initialize();
			this.Add("BasicBlack", new string[]
			{
				"TopBasicBlack",
				"BottomBasicBlack",
				"GlovesBasicBlack",
				"ShoesBasicBlack"
			}, UI.OUTFITS.BASIC_BLACK.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicWhite", new string[]
			{
				"TopBasicWhite",
				"BottomBasicWhite",
				"GlovesBasicWhite",
				"ShoesBasicWhite"
			}, UI.OUTFITS.BASIC_WHITE.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicRed", new string[]
			{
				"TopBasicRed",
				"BottomBasicRed",
				"GlovesBasicRed",
				"ShoesBasicRed"
			}, UI.OUTFITS.BASIC_RED.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicOrange", new string[]
			{
				"TopBasicOrange",
				"BottomBasicOrange",
				"GlovesBasicOrange",
				"ShoesBasicOrange"
			}, UI.OUTFITS.BASIC_ORANGE.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicYellow", new string[]
			{
				"TopBasicYellow",
				"BottomBasicYellow",
				"GlovesBasicYellow",
				"ShoesBasicYellow"
			}, UI.OUTFITS.BASIC_YELLOW.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicGreen", new string[]
			{
				"TopBasicGreen",
				"BottomBasicGreen",
				"GlovesBasicGreen",
				"ShoesBasicGreen"
			}, UI.OUTFITS.BASIC_GREEN.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicAqua", new string[]
			{
				"TopBasicAqua",
				"BottomBasicAqua",
				"GlovesBasicAqua",
				"ShoesBasicAqua"
			}, UI.OUTFITS.BASIC_AQUA.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicPurple", new string[]
			{
				"TopBasicPurple",
				"BottomBasicPurple",
				"GlovesBasicPurple",
				"ShoesBasicPurple"
			}, UI.OUTFITS.BASIC_PURPLE.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicPinkOrchid", new string[]
			{
				"TopBasicPinkOrchid",
				"BottomBasicPinkOrchid",
				"GlovesBasicPinkOrchid",
				"ShoesBasicPinkOrchid"
			}, UI.OUTFITS.BASIC_PINK_ORCHID.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicDeepRed", new string[]
			{
				"TopRaglanDeepRed",
				"ShortsBasicDeepRed",
				"GlovesAthleticRedDeep",
				"SocksAthleticDeepRed"
			}, UI.OUTFITS.BASIC_DEEPRED.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicOrangeSatsuma", new string[]
			{
				"TopRaglanSatsuma",
				"ShortsBasicSatsuma",
				"GlovesAthleticOrangeSatsuma",
				"SocksAthleticOrangeSatsuma"
			}, UI.OUTFITS.BASIC_SATSUMA.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicLemon", new string[]
			{
				"TopRaglanLemon",
				"ShortsBasicYellowcake",
				"GlovesAthleticYellowLemon",
				"SocksAthleticYellowLemon"
			}, UI.OUTFITS.BASIC_LEMON.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicBlueCobalt", new string[]
			{
				"TopRaglanCobalt",
				"ShortsBasicBlueCobalt",
				"GlovesAthleticBlueCobalt",
				"SocksAthleticBlueCobalt"
			}, UI.OUTFITS.BASIC_BLUE_COBALT.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicGreenKelly", new string[]
			{
				"TopRaglanKellyGreen",
				"ShortsBasicKellyGreen",
				"GlovesAthleticGreenKelly",
				"SocksAthleticGreenKelly"
			}, UI.OUTFITS.BASIC_GREEN_KELLY.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicPinkFlamingo", new string[]
			{
				"TopRaglanFlamingo",
				"ShortsBasicPinkFlamingo",
				"GlovesAthleticPinkFlamingo",
				"SocksAthleticPinkFlamingo"
			}, UI.OUTFITS.BASIC_PINK_FLAMINGO.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("BasicGreyCharcoal", new string[]
			{
				"TopRaglanCharcoal",
				"ShortsBasicCharcoal",
				"GlovesAthleticGreyCharcoal",
				"SocksAthleticGreyCharcoal"
			}, UI.OUTFITS.BASIC_GREY_CHARCOAL.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("JellypuffBlueberry", new string[]
			{
				"TopJellypuffJacketBlueberry",
				"GlovesCufflessBlueberry"
			}, UI.OUTFITS.JELLYPUFF_BLUEBERRY.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("JellypuffGrape", new string[]
			{
				"TopJellypuffJacketGrape",
				"GlovesCufflessGrape"
			}, UI.OUTFITS.JELLYPUFF_GRAPE.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("JellypuffLemon", new string[]
			{
				"TopJellypuffJacketLemon",
				"GlovesCufflessLemon"
			}, UI.OUTFITS.JELLYPUFF_LEMON.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("JellypuffLime", new string[]
			{
				"TopJellypuffJacketLime",
				"GlovesCufflessLime"
			}, UI.OUTFITS.JELLYPUFF_LIME.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("JellypuffSatsuma", new string[]
			{
				"TopJellypuffJacketSatsuma",
				"GlovesCufflessSatsuma"
			}, UI.OUTFITS.JELLYPUFF_SATSUMA.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("JellypuffStrawberry", new string[]
			{
				"TopJellypuffJacketStrawberry",
				"GlovesCufflessStrawberry"
			}, UI.OUTFITS.JELLYPUFF_STRAWBERRY.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("JellypuffWatermelon", new string[]
			{
				"TopJellypuffJacketWatermelon",
				"GlovesCufflessWatermelon"
			}, UI.OUTFITS.JELLYPUFF_WATERMELON.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("Athlete", new string[]
			{
				"TopAthlete",
				"PantsAthlete",
				"GlovesAthlete",
				"ShoesBasicBlack"
			}, UI.OUTFITS.ATHLETE.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("Circuit", new string[]
			{
				"TopCircuitGreen",
				"PantsCircuitGreen",
				"GlovesCircuitGreen"
			}, UI.OUTFITS.CIRCUIT.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("AtmoLimone", new string[]
			{
				"AtmoHelmetLimone",
				"AtmoSuitBasicYellow",
				"AtmoGlovesLime",
				"AtmoBeltBasicLime",
				"AtmoShoesBasicYellow"
			}, UI.OUTFITS.ATMOSUIT_LIMONE.NAME, ClothingOutfitUtility.OutfitType.AtmoSuit);
			this.Add("AtmoPuft", new string[]
			{
				"AtmoHelmetPuft",
				"AtmoSuitPuft",
				"AtmoGlovesPuft",
				"AtmoBeltPuft",
				"AtmoShoesPuft"
			}, UI.OUTFITS.ATMOSUIT_PUFT.NAME, ClothingOutfitUtility.OutfitType.AtmoSuit);
			this.Add("AtmoSparkleRed", new string[]
			{
				"AtmoHelmetSparkleRed",
				"AtmoSuitSparkleRed",
				"AtmoGlovesSparkleRed",
				"AtmoBeltSparkleRed",
				"AtmoShoesSparkleBlack"
			}, UI.OUTFITS.ATMOSUIT_SPARKLE_RED.NAME, ClothingOutfitUtility.OutfitType.AtmoSuit);
			this.Add("AtmoSparkleBlue", new string[]
			{
				"AtmoHelmetSparkleBlue",
				"AtmoSuitSparkleBlue",
				"AtmoGlovesSparkleBlue",
				"AtmoBeltSparkleBlue",
				"AtmoShoesSparkleBlack"
			}, UI.OUTFITS.ATMOSUIT_SPARKLE_BLUE.NAME, ClothingOutfitUtility.OutfitType.AtmoSuit);
			this.Add("AtmoSparkleGreen", new string[]
			{
				"AtmoHelmetSparkleGreen",
				"AtmoSuitSparkleGreen",
				"AtmoGlovesSparkleGreen",
				"AtmoBeltSparkleGreen",
				"AtmoShoesSparkleBlack"
			}, UI.OUTFITS.ATMOSUIT_SPARKLE_GREEN.NAME, ClothingOutfitUtility.OutfitType.AtmoSuit);
			this.Add("AtmoSparkleLavender", new string[]
			{
				"AtmoHelmetSparklePurple",
				"AtmoSuitSparkleLavender",
				"AtmoGlovesSparkleLavender",
				"AtmoBeltSparkleLavender",
				"AtmoShoesSparkleBlack"
			}, UI.OUTFITS.ATMOSUIT_SPARKLE_LAVENDER.NAME, ClothingOutfitUtility.OutfitType.AtmoSuit);
			this.Add("AtmoConfetti", new string[]
			{
				"AtmoHelmetConfetti",
				"AtmoSuitConfetti",
				"AtmoGlovesGold",
				"AtmoBeltBasicGold",
				"AtmoShoesStealth"
			}, UI.OUTFITS.ATMOSUIT_CONFETTI.NAME, ClothingOutfitUtility.OutfitType.AtmoSuit);
			this.Add("AtmoEggplant", new string[]
			{
				"AtmoHelmetEggplant",
				"AtmoSuitCrispEggplant",
				"AtmoGlovesEggplant",
				"AtmoBeltEggplant",
				"AtmoShoesEggplant"
			}, UI.OUTFITS.ATMOSUIT_BASIC_PURPLE.NAME, ClothingOutfitUtility.OutfitType.AtmoSuit);
			this.Add("CanadianTuxedo", new string[]
			{
				"TopDenimBlue",
				"PantsJeans",
				"GlovesDenimBlue",
				"ShoesDenimBlue"
			}, UI.OUTFITS.CANUXTUX.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("Researcher", new string[]
			{
				"TopResearcher",
				"PantsResearch",
				"GlovesBasicBrownKhaki",
				"ShoesBasicGray"
			}, UI.OUTFITS.NERD.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("UndiesExec", new string[]
			{
				"TopUndershirtExecutive",
				"BottomBriefsExecutive"
			}, UI.OUTFITS.GONCHIES_STRAWBERRY.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("UndiesUnderling", new string[]
			{
				"TopUndershirtUnderling",
				"BottomBriefsUnderling"
			}, UI.OUTFITS.GONCHIES_SATSUMA.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("UndiesGroupthink", new string[]
			{
				"TopUndershirtGroupthink",
				"BottomBriefsGroupthink"
			}, UI.OUTFITS.GONCHIES_LEMON.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("UndiesStakeholder", new string[]
			{
				"TopUndershirtStakeholder",
				"BottomBriefsStakeholder"
			}, UI.OUTFITS.GONCHIES_LIME.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("UndiesAdmin", new string[]
			{
				"TopUndershirtAdmin",
				"BottomBriefsAdmin"
			}, UI.OUTFITS.GONCHIES_BLUEBERRY.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("UndiesBuzzword", new string[]
			{
				"TopUndershirtBuzzword",
				"BottomBriefsBuzzword"
			}, UI.OUTFITS.GONCHIES_GRAPE.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("UndiesSynergy", new string[]
			{
				"TopUndershirtSynergy",
				"BottomBriefsSynergy"
			}, UI.OUTFITS.GONCHIES_WATERMELON.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("RebelGiOutfit", new string[]
			{
				"TopRebelGi",
				"PantsRebelGi",
				"GlovesCufflessBlack"
			}, UI.OUTFITS.REBELGI.NAME, ClothingOutfitUtility.OutfitType.Clothing);
			this.Add("AtmoPinkPurple", new string[]
			{
				"AtmoBeltBasicNeonPink",
				"AtmoGlovesStripesLavender",
				"AtmoHelmetWorkoutLavender",
				"AtmoSuitBasicNeonPink",
				"AtmoShoesBasicLavender"
			}, UI.OUTFITS.ATMOSUIT_PINK_PURPLE.NAME, ClothingOutfitUtility.OutfitType.AtmoSuit);
			this.Add("AtmoRedGrey", new string[]
			{
				"AtmoBeltBasicGrey",
				"AtmoGlovesWhite",
				"AtmoHelmetCummerbundRed",
				"AtmoSuitMultiRedBlack"
			}, UI.OUTFITS.ATMOSUIT_RED_GREY.NAME, ClothingOutfitUtility.OutfitType.AtmoSuit);
			ClothingOutfitUtility.LoadClothingOutfitData(this);
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x002897B8 File Offset: 0x002879B8
		public void Add(string id, string[] items_in_outfit, LocString name, ClothingOutfitUtility.OutfitType outfitType)
		{
			ClothingOutfitResource item = new ClothingOutfitResource(id, items_in_outfit, name, outfitType);
			this.resources.Add(item);
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x002897DC File Offset: 0x002879DC
		public void SetDuplicantPersonalityOutfit(string personalityId, Option<string> outfit_id, ClothingOutfitUtility.OutfitType outfit_type)
		{
			Db.Get().Personalities.Get(personalityId).Internal_SetSelectedTemplateOutfitId(outfit_type, outfit_id);
			CustomClothingOutfits.Instance.Internal_SetDuplicantPersonalityOutfit(personalityId, outfit_id, outfit_type);
		}

		// Token: 0x02001C26 RID: 7206
		public class ClothingOutfitInfo
		{
			// Token: 0x17000A5D RID: 2653
			// (get) Token: 0x06009C28 RID: 39976 RVA: 0x0034C338 File Offset: 0x0034A538
			// (set) Token: 0x06009C29 RID: 39977 RVA: 0x0034C340 File Offset: 0x0034A540
			public string id { get; set; }

			// Token: 0x17000A5E RID: 2654
			// (get) Token: 0x06009C2A RID: 39978 RVA: 0x0034C349 File Offset: 0x0034A549
			// (set) Token: 0x06009C2B RID: 39979 RVA: 0x0034C351 File Offset: 0x0034A551
			public string name { get; set; }

			// Token: 0x17000A5F RID: 2655
			// (get) Token: 0x06009C2C RID: 39980 RVA: 0x0034C35A File Offset: 0x0034A55A
			// (set) Token: 0x06009C2D RID: 39981 RVA: 0x0034C362 File Offset: 0x0034A562
			public List<ClothingOutfits.ClothingOutfitInfo.ClothingItem> items { get; set; }

			// Token: 0x0200223E RID: 8766
			public class ClothingItem
			{
				// Token: 0x17000A8E RID: 2702
				// (get) Token: 0x0600AD36 RID: 44342 RVA: 0x00379288 File Offset: 0x00377488
				// (set) Token: 0x0600AD37 RID: 44343 RVA: 0x00379290 File Offset: 0x00377490
				public string id { get; set; }

				// Token: 0x17000A8F RID: 2703
				// (get) Token: 0x0600AD38 RID: 44344 RVA: 0x00379299 File Offset: 0x00377499
				// (set) Token: 0x0600AD39 RID: 44345 RVA: 0x003792A1 File Offset: 0x003774A1
				public string name { get; set; }

				// Token: 0x17000A90 RID: 2704
				// (get) Token: 0x0600AD3A RID: 44346 RVA: 0x003792AA File Offset: 0x003774AA
				// (set) Token: 0x0600AD3B RID: 44347 RVA: 0x003792B2 File Offset: 0x003774B2
				public string description { get; set; }

				// Token: 0x17000A91 RID: 2705
				// (get) Token: 0x0600AD3C RID: 44348 RVA: 0x003792BB File Offset: 0x003774BB
				// (set) Token: 0x0600AD3D RID: 44349 RVA: 0x003792C3 File Offset: 0x003774C3
				public string category { get; set; }

				// Token: 0x17000A92 RID: 2706
				// (get) Token: 0x0600AD3E RID: 44350 RVA: 0x003792CC File Offset: 0x003774CC
				// (set) Token: 0x0600AD3F RID: 44351 RVA: 0x003792D4 File Offset: 0x003774D4
				public string animFilename { get; set; }
			}
		}
	}
}
