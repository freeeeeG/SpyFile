using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000CE8 RID: 3304
	public class BalloonArtistFacades : ResourceSet<BalloonArtistFacadeResource>
	{
		// Token: 0x0600693D RID: 26941 RVA: 0x0027D5F8 File Offset: 0x0027B7F8
		public BalloonArtistFacades(ResourceSet parent) : base("BalloonArtistFacades", parent)
		{
			foreach (BalloonArtistFacades.Info info in BalloonArtistFacades.Infos_All)
			{
				this.Add(info.id, info.name, info.desc, info.rarity, info.animFile, info.balloonFacadeType);
			}
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x0027D658 File Offset: 0x0027B858
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType)
		{
			BalloonArtistFacadeResource item = new BalloonArtistFacadeResource(id, name, desc, rarity, animFile, balloonFacadeType);
			this.resources.Add(item);
		}

		// Token: 0x040048EB RID: 18667
		public static BalloonArtistFacades.Info[] Infos_Skins = new BalloonArtistFacades.Info[]
		{
			new BalloonArtistFacades.Info("BalloonRedFireEngineLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_FIREENGINE_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_FIREENGINE_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_red_fireengine_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonYellowLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_YELLOW_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_YELLOW_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_yellow_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonBlueLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BLUE_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BLUE_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_blue_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonGreenLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_GREEN_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_GREEN_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_green_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonPinkLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PINK_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PINK_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_pink_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonPurpleLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PURPLE_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PURPLE_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_purple_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonBabyPacuEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PACU_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PACU_EGG.DESC, PermitRarity.Splendid, "balloon_babypacu_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonBabyGlossyDreckoEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_GLOSSY_DRECKO_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_GLOSSY_DRECKO_EGG.DESC, PermitRarity.Splendid, "balloon_babyglossydrecko_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonBabyHatchEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_HATCH_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_HATCH_EGG.DESC, PermitRarity.Splendid, "balloon_babyhatch_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonBabyPokeshellEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_POKESHELL_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_POKESHELL_EGG.DESC, PermitRarity.Splendid, "balloon_babypokeshell_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonBabyPuftEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PUFT_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PUFT_EGG.DESC, PermitRarity.Splendid, "balloon_babypuft_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonBabyShovoleEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_SHOVOLE_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_SHOVOLE_EGG.DESC, PermitRarity.Splendid, "balloon_babyshovole_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonBabyPipEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PIP_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PIP_EGG.DESC, PermitRarity.Splendid, "balloon_babypip_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonCandyBlueberry", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_BLUEBERRY.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_BLUEBERRY.DESC, PermitRarity.Decent, "balloon_candy_blueberry_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonCandyGrape", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_GRAPE.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_GRAPE.DESC, PermitRarity.Decent, "balloon_candy_grape_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonCandyLemon", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LEMON.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LEMON.DESC, PermitRarity.Decent, "balloon_candy_lemon_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonCandyLime", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LIME.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LIME.DESC, PermitRarity.Decent, "balloon_candy_lime_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonCandyOrange", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_ORANGE.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_ORANGE.DESC, PermitRarity.Decent, "balloon_candy_orange_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonCandyStrawberry", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_STRAWBERRY.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_STRAWBERRY.DESC, PermitRarity.Decent, "balloon_candy_strawberry_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonCandyWatermelon", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_WATERMELON.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_WATERMELON.DESC, PermitRarity.Decent, "balloon_candy_watermelon_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacades.Info("BalloonHandGold", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.HAND_GOLD.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.HAND_GOLD.DESC, PermitRarity.Decent, "balloon_hand_gold_kanim", BalloonArtistFacadeType.ThreeSet)
		};

		// Token: 0x040048EC RID: 18668
		public static BalloonArtistFacades.Info[] Infos_All = BalloonArtistFacades.Infos_Skins;

		// Token: 0x02001C1B RID: 7195
		public struct Info
		{
			// Token: 0x06009BA6 RID: 39846 RVA: 0x00349E4B File Offset: 0x0034804B
			public Info(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType)
			{
				this.id = id;
				this.name = name;
				this.desc = desc;
				this.rarity = rarity;
				this.animFile = animFile;
				this.balloonFacadeType = balloonFacadeType;
			}

			// Token: 0x04007EDE RID: 32478
			public string id;

			// Token: 0x04007EDF RID: 32479
			public string name;

			// Token: 0x04007EE0 RID: 32480
			public string desc;

			// Token: 0x04007EE1 RID: 32481
			public PermitRarity rarity;

			// Token: 0x04007EE2 RID: 32482
			public string animFile;

			// Token: 0x04007EE3 RID: 32483
			public BalloonArtistFacadeType balloonFacadeType;
		}
	}
}
