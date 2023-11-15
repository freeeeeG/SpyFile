using System;

namespace STRINGS
{
	// Token: 0x02000DA6 RID: 3494
	public class EQUIPMENT
	{
		// Token: 0x02001D84 RID: 7556
		public class PREFABS
		{
			// Token: 0x020024F8 RID: 9464
			public class OXYGEN_MASK
			{
				// Token: 0x0400A216 RID: 41494
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK");

				// Token: 0x0400A217 RID: 41495
				public static LocString DESC = "Ensures my Duplicants can breathe easy... for a little while, anyways.";

				// Token: 0x0400A218 RID: 41496
				public static LocString EFFECT = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.\n\nMust be refilled with oxygen at an " + UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER") + " when depleted.";

				// Token: 0x0400A219 RID: 41497
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.";

				// Token: 0x0400A21A RID: 41498
				public static LocString GENERICNAME = "Suit";

				// Token: 0x0400A21B RID: 41499
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Oxygen Mask", "OXYGEN_MASK");

				// Token: 0x0400A21C RID: 41500
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK"),
					".\n\nMasks can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x020024F9 RID: 9465
			public class ATMO_SUIT
			{
				// Token: 0x0400A21D RID: 41501
				public static LocString NAME = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT");

				// Token: 0x0400A21E RID: 41502
				public static LocString DESC = "Ensures my Duplicants can breathe easy, anytime, anywhere.";

				// Token: 0x0400A21F RID: 41503
				public static LocString EFFECT = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.\n\nMust be refilled with oxygen at an " + UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER") + " when depleted.";

				// Token: 0x0400A220 RID: 41504
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.";

				// Token: 0x0400A221 RID: 41505
				public static LocString GENERICNAME = "Suit";

				// Token: 0x0400A222 RID: 41506
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Atmo Suit", "ATMO_SUIT");

				// Token: 0x0400A223 RID: 41507
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Atmo Suit", "ATMO_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});

				// Token: 0x0400A224 RID: 41508
				public static LocString REPAIR_WORN_RECIPE_NAME = "Repair " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400A225 RID: 41509
				public static LocString REPAIR_WORN_DESC = "Restore a " + UI.FormatAsLink("Worn Atmo Suit", "ATMO_SUIT") + " to working order.";
			}

			// Token: 0x020024FA RID: 9466
			public class ATMO_SUIT_SET
			{
				// Token: 0x02002FE9 RID: 12265
				public class PUFT
				{
					// Token: 0x0400C2C3 RID: 49859
					public static LocString NAME = "Puft Atmo Suit";

					// Token: 0x0400C2C4 RID: 49860
					public static LocString DESC = "Critter-forward protective gear for the intrepid explorer!\nReleased for Klei Fest 2023.";
				}
			}

			// Token: 0x020024FB RID: 9467
			public class ATMO_SUIT_HELMET
			{
				// Token: 0x0400A226 RID: 41510
				public static LocString NAME = "Default Atmo Helmet";

				// Token: 0x0400A227 RID: 41511
				public static LocString DESC = "Default helmet for atmo suits.";

				// Token: 0x02002FEA RID: 12266
				public class FACADES
				{
					// Token: 0x020032ED RID: 13037
					public class SPARKLE_RED
					{
						// Token: 0x0400CA3A RID: 51770
						public static LocString NAME = "Red Glitter Atmo Helmet";

						// Token: 0x0400CA3B RID: 51771
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x020032EE RID: 13038
					public class SPARKLE_GREEN
					{
						// Token: 0x0400CA3C RID: 51772
						public static LocString NAME = "Green Glitter Atmo Helmet";

						// Token: 0x0400CA3D RID: 51773
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x020032EF RID: 13039
					public class SPARKLE_BLUE
					{
						// Token: 0x0400CA3E RID: 51774
						public static LocString NAME = "Blue Glitter Atmo Helmet";

						// Token: 0x0400CA3F RID: 51775
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x020032F0 RID: 13040
					public class SPARKLE_PURPLE
					{
						// Token: 0x0400CA40 RID: 51776
						public static LocString NAME = "Violet Glitter Atmo Helmet";

						// Token: 0x0400CA41 RID: 51777
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x020032F1 RID: 13041
					public class LIMONE
					{
						// Token: 0x0400CA42 RID: 51778
						public static LocString NAME = "Citrus Atmo Helmet";

						// Token: 0x0400CA43 RID: 51779
						public static LocString DESC = "Fresh, fruity and full of breathable air.";
					}

					// Token: 0x020032F2 RID: 13042
					public class PUFT
					{
						// Token: 0x0400CA44 RID: 51780
						public static LocString NAME = "Puft Atmo Helmet";

						// Token: 0x0400CA45 RID: 51781
						public static LocString DESC = "Convincing enough to fool most Pufts and even a few Duplicants.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x020032F3 RID: 13043
					public class CLUBSHIRT_PURPLE
					{
						// Token: 0x0400CA46 RID: 51782
						public static LocString NAME = "Eggplant Atmo Helmet";

						// Token: 0x0400CA47 RID: 51783
						public static LocString DESC = "It is neither an egg, nor a plant. But it <i>is</i> a functional helmet.";
					}

					// Token: 0x020032F4 RID: 13044
					public class TRIANGLES_TURQ
					{
						// Token: 0x0400CA48 RID: 51784
						public static LocString NAME = "Confetti Atmo Helmet";

						// Token: 0x0400CA49 RID: 51785
						public static LocString DESC = "Doubles as a party hat.";
					}

					// Token: 0x020032F5 RID: 13045
					public class CUMMERBUND_RED
					{
						// Token: 0x0400CA4A RID: 51786
						public static LocString NAME = "Blastoff Atmo Helmet";

						// Token: 0x0400CA4B RID: 51787
						public static LocString DESC = "Red means go!";
					}

					// Token: 0x020032F6 RID: 13046
					public class WORKOUT_LAVENDER
					{
						// Token: 0x0400CA4C RID: 51788
						public static LocString NAME = "Pink Punch Atmo Helmet";

						// Token: 0x0400CA4D RID: 51789
						public static LocString DESC = "Unapologetically ostentatious.";
					}
				}
			}

			// Token: 0x020024FC RID: 9468
			public class ATMO_SUIT_BODY
			{
				// Token: 0x0400A228 RID: 41512
				public static LocString NAME = "Default Atmo Uniform";

				// Token: 0x0400A229 RID: 41513
				public static LocString DESC = "Default top and bottom of an atmo suit.";

				// Token: 0x02002FEB RID: 12267
				public class FACADES
				{
					// Token: 0x020032F7 RID: 13047
					public class SPARKLE_RED
					{
						// Token: 0x0400CA4E RID: 51790
						public static LocString NAME = "Red Glitter Atmo Suit";

						// Token: 0x0400CA4F RID: 51791
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x020032F8 RID: 13048
					public class SPARKLE_GREEN
					{
						// Token: 0x0400CA50 RID: 51792
						public static LocString NAME = "Green Glitter Atmo Suit";

						// Token: 0x0400CA51 RID: 51793
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x020032F9 RID: 13049
					public class SPARKLE_BLUE
					{
						// Token: 0x0400CA52 RID: 51794
						public static LocString NAME = "Blue Glitter Atmo Suit";

						// Token: 0x0400CA53 RID: 51795
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x020032FA RID: 13050
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400CA54 RID: 51796
						public static LocString NAME = "Violet Glitter Atmo Suit";

						// Token: 0x0400CA55 RID: 51797
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x020032FB RID: 13051
					public class LIMONE
					{
						// Token: 0x0400CA56 RID: 51798
						public static LocString NAME = "Citrus Atmo Suit";

						// Token: 0x0400CA57 RID: 51799
						public static LocString DESC = "Perfect for summery, atmospheric excursions.";
					}

					// Token: 0x020032FC RID: 13052
					public class PUFT
					{
						// Token: 0x0400CA58 RID: 51800
						public static LocString NAME = "Puft Atmo Suit";

						// Token: 0x0400CA59 RID: 51801
						public static LocString DESC = "Warning: prolonged wear may result in feelings of Puft-up pride.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x020032FD RID: 13053
					public class BASIC_PURPLE
					{
						// Token: 0x0400CA5A RID: 51802
						public static LocString NAME = "Crisp Eggplant Atmo Suit";

						// Token: 0x0400CA5B RID: 51803
						public static LocString DESC = "It really emphasizes wide shoulders.";
					}

					// Token: 0x020032FE RID: 13054
					public class PRINT_TRIANGLES_TURQ
					{
						// Token: 0x0400CA5C RID: 51804
						public static LocString NAME = "Confetti Atmo Suit";

						// Token: 0x0400CA5D RID: 51805
						public static LocString DESC = "It puts the \"fun\" in \"perfunctory nods to personnel individuality\"!";
					}

					// Token: 0x020032FF RID: 13055
					public class BASIC_NEON_PINK
					{
						// Token: 0x0400CA5E RID: 51806
						public static LocString NAME = "Crisp Neon Pink Atmo Suit";

						// Token: 0x0400CA5F RID: 51807
						public static LocString DESC = "The neck is a little snug.";
					}

					// Token: 0x02003300 RID: 13056
					public class MULTI_RED_BLACK
					{
						// Token: 0x0400CA60 RID: 51808
						public static LocString NAME = "Red-bellied Atmo Suit";

						// Token: 0x0400CA61 RID: 51809
						public static LocString DESC = "It really highlights the midsection.";
					}
				}
			}

			// Token: 0x020024FD RID: 9469
			public class ATMO_SUIT_GLOVES
			{
				// Token: 0x0400A22A RID: 41514
				public static LocString NAME = "Default Atmo Gloves";

				// Token: 0x0400A22B RID: 41515
				public static LocString DESC = "Default atmo suit gloves.";

				// Token: 0x02002FEC RID: 12268
				public class FACADES
				{
					// Token: 0x02003301 RID: 13057
					public class SPARKLE_RED
					{
						// Token: 0x0400CA62 RID: 51810
						public static LocString NAME = "Red Glitter Atmo Gloves";

						// Token: 0x0400CA63 RID: 51811
						public static LocString DESC = "Sparkly red gloves for hostile environments.";
					}

					// Token: 0x02003302 RID: 13058
					public class SPARKLE_GREEN
					{
						// Token: 0x0400CA64 RID: 51812
						public static LocString NAME = "Green Glitter Atmo Gloves";

						// Token: 0x0400CA65 RID: 51813
						public static LocString DESC = "Sparkly green gloves for hostile environments.";
					}

					// Token: 0x02003303 RID: 13059
					public class SPARKLE_BLUE
					{
						// Token: 0x0400CA66 RID: 51814
						public static LocString NAME = "Blue Glitter Atmo Gloves";

						// Token: 0x0400CA67 RID: 51815
						public static LocString DESC = "Sparkly blue gloves for hostile environments.";
					}

					// Token: 0x02003304 RID: 13060
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400CA68 RID: 51816
						public static LocString NAME = "Violet Glitter Atmo Gloves";

						// Token: 0x0400CA69 RID: 51817
						public static LocString DESC = "Sparkly violet gloves for hostile environments.";
					}

					// Token: 0x02003305 RID: 13061
					public class LIMONE
					{
						// Token: 0x0400CA6A RID: 51818
						public static LocString NAME = "Citrus Atmo Gloves";

						// Token: 0x0400CA6B RID: 51819
						public static LocString DESC = "Lime-inspired gloves brighten up hostile environments.";
					}

					// Token: 0x02003306 RID: 13062
					public class PUFT
					{
						// Token: 0x0400CA6C RID: 51820
						public static LocString NAME = "Puft Atmo Gloves";

						// Token: 0x0400CA6D RID: 51821
						public static LocString DESC = "A little Puft-love for delicate extremities.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02003307 RID: 13063
					public class GOLD
					{
						// Token: 0x0400CA6E RID: 51822
						public static LocString NAME = "Gold Atmo Gloves";

						// Token: 0x0400CA6F RID: 51823
						public static LocString DESC = "A golden touch! Without all the Midas-type baggage.";
					}

					// Token: 0x02003308 RID: 13064
					public class PURPLE
					{
						// Token: 0x0400CA70 RID: 51824
						public static LocString NAME = "Eggplant Atmo Gloves";

						// Token: 0x0400CA71 RID: 51825
						public static LocString DESC = "Fab purple gloves for hostile environments.";
					}

					// Token: 0x02003309 RID: 13065
					public class WHITE
					{
						// Token: 0x0400CA72 RID: 51826
						public static LocString NAME = "White Atmo Gloves";

						// Token: 0x0400CA73 RID: 51827
						public static LocString DESC = "For the Duplicant who never gets their hands dirty.";
					}

					// Token: 0x0200330A RID: 13066
					public class STRIPES_LAVENDER
					{
						// Token: 0x0400CA74 RID: 51828
						public static LocString NAME = "Wildberry Atmo Gloves";

						// Token: 0x0400CA75 RID: 51829
						public static LocString DESC = "Functional finger-protectors with fruity flair.";
					}
				}
			}

			// Token: 0x020024FE RID: 9470
			public class ATMO_SUIT_BELT
			{
				// Token: 0x0400A22C RID: 41516
				public static LocString NAME = "Default Atmo Belt";

				// Token: 0x0400A22D RID: 41517
				public static LocString DESC = "Default belt for atmo suits.";

				// Token: 0x02002FED RID: 12269
				public class FACADES
				{
					// Token: 0x0200330B RID: 13067
					public class SPARKLE_RED
					{
						// Token: 0x0400CA76 RID: 51830
						public static LocString NAME = "Red Glitter Atmo Belt";

						// Token: 0x0400CA77 RID: 51831
						public static LocString DESC = "It's red! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x0200330C RID: 13068
					public class SPARKLE_GREEN
					{
						// Token: 0x0400CA78 RID: 51832
						public static LocString NAME = "Green Glitter Atmo Belt";

						// Token: 0x0400CA79 RID: 51833
						public static LocString DESC = "It's green! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x0200330D RID: 13069
					public class SPARKLE_BLUE
					{
						// Token: 0x0400CA7A RID: 51834
						public static LocString NAME = "Blue Glitter Atmo Belt";

						// Token: 0x0400CA7B RID: 51835
						public static LocString DESC = "It's blue! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x0200330E RID: 13070
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400CA7C RID: 51836
						public static LocString NAME = "Violet Glitter Atmo Belt";

						// Token: 0x0400CA7D RID: 51837
						public static LocString DESC = "It's violet! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x0200330F RID: 13071
					public class LIMONE
					{
						// Token: 0x0400CA7E RID: 51838
						public static LocString NAME = "Citrus Atmo Belt";

						// Token: 0x0400CA7F RID: 51839
						public static LocString DESC = "This lime-hued belt really pulls an atmo suit together.";
					}

					// Token: 0x02003310 RID: 13072
					public class PUFT
					{
						// Token: 0x0400CA80 RID: 51840
						public static LocString NAME = "Puft Atmo Belt";

						// Token: 0x0400CA81 RID: 51841
						public static LocString DESC = "If critters wore belts...\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02003311 RID: 13073
					public class TWOTONE_PURPLE
					{
						// Token: 0x0400CA82 RID: 51842
						public static LocString NAME = "Eggplant Atmo Belt";

						// Token: 0x0400CA83 RID: 51843
						public static LocString DESC = "In the more pretentious space-fashion circles, it's known as \"aubergine.\"";
					}

					// Token: 0x02003312 RID: 13074
					public class BASIC_GOLD
					{
						// Token: 0x0400CA84 RID: 51844
						public static LocString NAME = "Gold Atmo Belt";

						// Token: 0x0400CA85 RID: 51845
						public static LocString DESC = "Better to be overdressed than underdressed.";
					}

					// Token: 0x02003313 RID: 13075
					public class BASIC_GREY
					{
						// Token: 0x0400CA86 RID: 51846
						public static LocString NAME = "Slate Atmo Belt";

						// Token: 0x0400CA87 RID: 51847
						public static LocString DESC = "Slick and understated space style.";
					}

					// Token: 0x02003314 RID: 13076
					public class BASIC_NEON_PINK
					{
						// Token: 0x0400CA88 RID: 51848
						public static LocString NAME = "Neon Pink Atmo Belt";

						// Token: 0x0400CA89 RID: 51849
						public static LocString DESC = "Visible from several planetoids away.";
					}
				}
			}

			// Token: 0x020024FF RID: 9471
			public class ATMO_SUIT_SHOES
			{
				// Token: 0x0400A22E RID: 41518
				public static LocString NAME = "Default Atmo Boots";

				// Token: 0x0400A22F RID: 41519
				public static LocString DESC = "Default footwear for atmo suits.";

				// Token: 0x02002FEE RID: 12270
				public class FACADES
				{
					// Token: 0x02003315 RID: 13077
					public class LIMONE
					{
						// Token: 0x0400CA8A RID: 51850
						public static LocString NAME = "Citrus Atmo Boots";

						// Token: 0x0400CA8B RID: 51851
						public static LocString DESC = "Cheery boots for stomping around in hostile environments.";
					}

					// Token: 0x02003316 RID: 13078
					public class PUFT
					{
						// Token: 0x0400CA8C RID: 51852
						public static LocString NAME = "Puft Atmo Boots";

						// Token: 0x0400CA8D RID: 51853
						public static LocString DESC = "These boots were made for puft-ing.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02003317 RID: 13079
					public class SPARKLE_BLACK
					{
						// Token: 0x0400CA8E RID: 51854
						public static LocString NAME = "Black Glitter Atmo Boots";

						// Token: 0x0400CA8F RID: 51855
						public static LocString DESC = "A timeless color, with a little pizzazz.";
					}

					// Token: 0x02003318 RID: 13080
					public class BASIC_BLACK
					{
						// Token: 0x0400CA90 RID: 51856
						public static LocString NAME = "Stealth Atmo Boots";

						// Token: 0x0400CA91 RID: 51857
						public static LocString DESC = "They attract no attention at all.";
					}

					// Token: 0x02003319 RID: 13081
					public class BASIC_PURPLE
					{
						// Token: 0x0400CA92 RID: 51858
						public static LocString NAME = "Eggplant Atmo Boots";

						// Token: 0x0400CA93 RID: 51859
						public static LocString DESC = "Purple boots for stomping around in hostile environments.";
					}

					// Token: 0x0200331A RID: 13082
					public class BASIC_LAVENDER
					{
						// Token: 0x0400CA94 RID: 51860
						public static LocString NAME = "Lavender Atmo Boots";

						// Token: 0x0400CA95 RID: 51861
						public static LocString DESC = "Soothing space booties for tired feet.";
					}
				}
			}

			// Token: 0x02002500 RID: 9472
			public class AQUA_SUIT
			{
				// Token: 0x0400A230 RID: 41520
				public static LocString NAME = UI.FormatAsLink("Aqua Suit", "AQUA_SUIT");

				// Token: 0x0400A231 RID: 41521
				public static LocString DESC = "Because breathing underwater is better than... not.";

				// Token: 0x0400A232 RID: 41522
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" at an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400A233 RID: 41523
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.";

				// Token: 0x0400A234 RID: 41524
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "AQUA_SUIT");

				// Token: 0x0400A235 RID: 41525
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Aqua Suit", "AQUA_SUIT"),
					".\n\nSuits can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02002501 RID: 9473
			public class TEMPERATURE_SUIT
			{
				// Token: 0x0400A236 RID: 41526
				public static LocString NAME = UI.FormatAsLink("Thermo Suit", "TEMPERATURE_SUIT");

				// Token: 0x0400A237 RID: 41527
				public static LocString DESC = "Keeps my Duplicants cool in case things heat up.";

				// Token: 0x0400A238 RID: 41528
				public static LocString EFFECT = "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.\n\nMust be powered at a Thermo Suit Dock when depleted.";

				// Token: 0x0400A239 RID: 41529
				public static LocString RECIPE_DESC = "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.";

				// Token: 0x0400A23A RID: 41530
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "TEMPERATURE_SUIT");

				// Token: 0x0400A23B RID: 41531
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Thermo Suit", "TEMPERATURE_SUIT"),
					".\n\nSuits can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02002502 RID: 9474
			public class JET_SUIT
			{
				// Token: 0x0400A23C RID: 41532
				public static LocString NAME = UI.FormatAsLink("Jet Suit", "JET_SUIT");

				// Token: 0x0400A23D RID: 41533
				public static LocString DESC = "Allows my Duplicants to take to the skies, for a time.";

				// Token: 0x0400A23E RID: 41534
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments.\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" at a ",
					UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400A23F RID: 41535
				public static LocString RECIPE_DESC = "Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " in toxic and low breathability environments.\n\nAllows Duplicant flight.";

				// Token: 0x0400A240 RID: 41536
				public static LocString GENERICNAME = "Jet Suit";

				// Token: 0x0400A241 RID: 41537
				public static LocString TANK_EFFECT_NAME = "Fuel Tank";

				// Token: 0x0400A242 RID: 41538
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Jet Suit", "JET_SUIT");

				// Token: 0x0400A243 RID: 41539
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Jet Suit", "JET_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});
			}

			// Token: 0x02002503 RID: 9475
			public class LEAD_SUIT
			{
				// Token: 0x0400A244 RID: 41540
				public static LocString NAME = UI.FormatAsLink("Lead Suit", "LEAD_SUIT");

				// Token: 0x0400A245 RID: 41541
				public static LocString DESC = "Because exposure to radiation doesn't grant Duplicants superpowers.";

				// Token: 0x0400A246 RID: 41542
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and protection in areas with ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					".\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" at a ",
					UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400A247 RID: 41543
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments.\n\nProtects Duplicants from ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					"."
				});

				// Token: 0x0400A248 RID: 41544
				public static LocString GENERICNAME = "Lead Suit";

				// Token: 0x0400A249 RID: 41545
				public static LocString BATTERY_EFFECT_NAME = "Suit Battery";

				// Token: 0x0400A24A RID: 41546
				public static LocString SUIT_OUT_OF_BATTERIES = "Suit Batteries Empty";

				// Token: 0x0400A24B RID: 41547
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "LEAD_SUIT");

				// Token: 0x0400A24C RID: 41548
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Lead Suit", "LEAD_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});
			}

			// Token: 0x02002504 RID: 9476
			public class COOL_VEST
			{
				// Token: 0x0400A24D RID: 41549
				public static LocString NAME = UI.FormatAsLink("Cool Vest", "COOL_VEST");

				// Token: 0x0400A24E RID: 41550
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400A24F RID: 41551
				public static LocString DESC = "Don't sweat it!";

				// Token: 0x0400A250 RID: 41552
				public static LocString EFFECT = "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation.";

				// Token: 0x0400A251 RID: 41553
				public static LocString RECIPE_DESC = "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation";
			}

			// Token: 0x02002505 RID: 9477
			public class WARM_VEST
			{
				// Token: 0x0400A252 RID: 41554
				public static LocString NAME = UI.FormatAsLink("Warm Sweater", "WARM_VEST");

				// Token: 0x0400A253 RID: 41555
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400A254 RID: 41556
				public static LocString DESC = "Happiness is a warm Duplicant.";

				// Token: 0x0400A255 RID: 41557
				public static LocString EFFECT = "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation.";

				// Token: 0x0400A256 RID: 41558
				public static LocString RECIPE_DESC = "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation";
			}

			// Token: 0x02002506 RID: 9478
			public class FUNKY_VEST
			{
				// Token: 0x0400A257 RID: 41559
				public static LocString NAME = UI.FormatAsLink("Snazzy Suit", "FUNKY_VEST");

				// Token: 0x0400A258 RID: 41560
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400A259 RID: 41561
				public static LocString DESC = "This transforms my Duplicant into a walking beacon of charm and style.";

				// Token: 0x0400A25A RID: 41562
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases Decor in a small area effect around the wearer. Can be upgraded to ",
					UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING"),
					" at the ",
					UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION"),
					"."
				});

				// Token: 0x0400A25B RID: 41563
				public static LocString RECIPE_DESC = "Increases Decor in a small area effect around the wearer. Can be upgraded to " + UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING") + " at the " + UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION");
			}

			// Token: 0x02002507 RID: 9479
			public class CUSTOMCLOTHING
			{
				// Token: 0x0400A25C RID: 41564
				public static LocString NAME = UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING");

				// Token: 0x0400A25D RID: 41565
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400A25E RID: 41566
				public static LocString DESC = "This transforms my Duplicant into a colony-inspiring fashion icon.";

				// Token: 0x0400A25F RID: 41567
				public static LocString EFFECT = "Increases Decor in a small area effect around the wearer.";

				// Token: 0x0400A260 RID: 41568
				public static LocString RECIPE_DESC = "Increases Decor in a small area effect around the wearer";

				// Token: 0x02002FEF RID: 12271
				public class FACADES
				{
					// Token: 0x0400C2C5 RID: 49861
					public static LocString CLUBSHIRT = UI.FormatAsLink("Purple Polyester Suit", "CUSTOMCLOTHING");

					// Token: 0x0400C2C6 RID: 49862
					public static LocString CUMMERBUND = UI.FormatAsLink("Classic Cummerbund", "CUSTOMCLOTHING");

					// Token: 0x0400C2C7 RID: 49863
					public static LocString DECOR_02 = UI.FormatAsLink("Snazzier Red Suit", "CUSTOMCLOTHING");

					// Token: 0x0400C2C8 RID: 49864
					public static LocString DECOR_03 = UI.FormatAsLink("Snazzier Blue Suit", "CUSTOMCLOTHING");

					// Token: 0x0400C2C9 RID: 49865
					public static LocString DECOR_04 = UI.FormatAsLink("Snazzier Green Suit", "CUSTOMCLOTHING");

					// Token: 0x0400C2CA RID: 49866
					public static LocString DECOR_05 = UI.FormatAsLink("Snazzier Violet Suit", "CUSTOMCLOTHING");

					// Token: 0x0400C2CB RID: 49867
					public static LocString GAUDYSWEATER = UI.FormatAsLink("Pompom Knit Suit", "CUSTOMCLOTHING");

					// Token: 0x0400C2CC RID: 49868
					public static LocString LIMONE = UI.FormatAsLink("Citrus Spandex Suit", "CUSTOMCLOTHING");

					// Token: 0x0400C2CD RID: 49869
					public static LocString MONDRIAN = UI.FormatAsLink("Cubist Knit Suit", "CUSTOMCLOTHING");

					// Token: 0x0400C2CE RID: 49870
					public static LocString OVERALLS = UI.FormatAsLink("Spiffy Overalls", "CUSTOMCLOTHING");

					// Token: 0x0400C2CF RID: 49871
					public static LocString TRIANGLES = UI.FormatAsLink("Confetti Suit", "CUSTOMCLOTHING");

					// Token: 0x0400C2D0 RID: 49872
					public static LocString WORKOUT = UI.FormatAsLink("Pink Unitard", "CUSTOMCLOTHING");
				}
			}

			// Token: 0x02002508 RID: 9480
			public class CLOTHING_GLOVES
			{
				// Token: 0x0400A261 RID: 41569
				public static LocString NAME = "Default Gloves";

				// Token: 0x0400A262 RID: 41570
				public static LocString DESC = "The default gloves.";

				// Token: 0x02002FF0 RID: 12272
				public class FACADES
				{
					// Token: 0x0200331B RID: 13083
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400CA96 RID: 51862
						public static LocString NAME = "Basic Aqua Gloves";

						// Token: 0x0400CA97 RID: 51863
						public static LocString DESC = "A good, solid pair of aqua-blue gloves that go with everything.";
					}

					// Token: 0x0200331C RID: 13084
					public class BASIC_YELLOW
					{
						// Token: 0x0400CA98 RID: 51864
						public static LocString NAME = "Basic Yellow Gloves";

						// Token: 0x0400CA99 RID: 51865
						public static LocString DESC = "A good, solid pair of yellow gloves that go with everything.";
					}

					// Token: 0x0200331D RID: 13085
					public class BASIC_BLACK
					{
						// Token: 0x0400CA9A RID: 51866
						public static LocString NAME = "Basic Black Gloves";

						// Token: 0x0400CA9B RID: 51867
						public static LocString DESC = "A good, solid pair of black gloves that go with everything.";
					}

					// Token: 0x0200331E RID: 13086
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400CA9C RID: 51868
						public static LocString NAME = "Basic Bubblegum Gloves";

						// Token: 0x0400CA9D RID: 51869
						public static LocString DESC = "A good, solid pair of bubblegum-pink gloves that go with everything.";
					}

					// Token: 0x0200331F RID: 13087
					public class BASIC_GREEN
					{
						// Token: 0x0400CA9E RID: 51870
						public static LocString NAME = "Basic Green Gloves";

						// Token: 0x0400CA9F RID: 51871
						public static LocString DESC = "A good, solid pair of green gloves that go with everything.";
					}

					// Token: 0x02003320 RID: 13088
					public class BASIC_ORANGE
					{
						// Token: 0x0400CAA0 RID: 51872
						public static LocString NAME = "Basic Orange Gloves";

						// Token: 0x0400CAA1 RID: 51873
						public static LocString DESC = "A good, solid pair of orange gloves that go with everything.";
					}

					// Token: 0x02003321 RID: 13089
					public class BASIC_PURPLE
					{
						// Token: 0x0400CAA2 RID: 51874
						public static LocString NAME = "Basic Purple Gloves";

						// Token: 0x0400CAA3 RID: 51875
						public static LocString DESC = "A good, solid pair of purple gloves that go with everything.";
					}

					// Token: 0x02003322 RID: 13090
					public class BASIC_RED
					{
						// Token: 0x0400CAA4 RID: 51876
						public static LocString NAME = "Basic Red Gloves";

						// Token: 0x0400CAA5 RID: 51877
						public static LocString DESC = "A good, solid pair of red gloves that go with everything.";
					}

					// Token: 0x02003323 RID: 13091
					public class BASIC_WHITE
					{
						// Token: 0x0400CAA6 RID: 51878
						public static LocString NAME = "Basic White Gloves";

						// Token: 0x0400CAA7 RID: 51879
						public static LocString DESC = "A good, solid pair of white gloves that go with everything.";
					}

					// Token: 0x02003324 RID: 13092
					public class GLOVES_ATHLETIC_DEEPRED
					{
						// Token: 0x0400CAA8 RID: 51880
						public static LocString NAME = "Team Captain Sports Gloves";

						// Token: 0x0400CAA9 RID: 51881
						public static LocString DESC = "Red-striped gloves for winning at any activity.";
					}

					// Token: 0x02003325 RID: 13093
					public class GLOVES_ATHLETIC_SATSUMA
					{
						// Token: 0x0400CAAA RID: 51882
						public static LocString NAME = "Superfan Sports Gloves";

						// Token: 0x0400CAAB RID: 51883
						public static LocString DESC = "Orange-striped gloves for enthusiastic athletes.";
					}

					// Token: 0x02003326 RID: 13094
					public class GLOVES_ATHLETIC_LEMON
					{
						// Token: 0x0400CAAC RID: 51884
						public static LocString NAME = "Hype Sports Gloves";

						// Token: 0x0400CAAD RID: 51885
						public static LocString DESC = "Yellow-striped gloves for athletes who seek to raise the bar.";
					}

					// Token: 0x02003327 RID: 13095
					public class GLOVES_ATHLETIC_KELLYGREEN
					{
						// Token: 0x0400CAAE RID: 51886
						public static LocString NAME = "Go Team Sports Gloves";

						// Token: 0x0400CAAF RID: 51887
						public static LocString DESC = "Green-striped gloves for the perenially good sport.";
					}

					// Token: 0x02003328 RID: 13096
					public class GLOVES_ATHLETIC_COBALT
					{
						// Token: 0x0400CAB0 RID: 51888
						public static LocString NAME = "True Blue Sports Gloves";

						// Token: 0x0400CAB1 RID: 51889
						public static LocString DESC = "Blue-striped gloves perfect for shaking hands after the game.";
					}

					// Token: 0x02003329 RID: 13097
					public class GLOVES_ATHLETIC_FLAMINGO
					{
						// Token: 0x0400CAB2 RID: 51890
						public static LocString NAME = "Pep Rally Sports Gloves";

						// Token: 0x0400CAB3 RID: 51891
						public static LocString DESC = "Pink-striped glove designed to withstand countless high-fives.";
					}

					// Token: 0x0200332A RID: 13098
					public class GLOVES_ATHLETIC_CHARCOAL
					{
						// Token: 0x0400CAB4 RID: 51892
						public static LocString NAME = "Underdog Sports Gloves";

						// Token: 0x0400CAB5 RID: 51893
						public static LocString DESC = "The muted stripe minimizes distractions so its wearer can focus on trying very, very hard.";
					}

					// Token: 0x0200332B RID: 13099
					public class CUFFLESS_BLUEBERRY
					{
						// Token: 0x0400CAB6 RID: 51894
						public static LocString NAME = "Blueberry Glovelets";

						// Token: 0x0400CAB7 RID: 51895
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x0200332C RID: 13100
					public class CUFFLESS_GRAPE
					{
						// Token: 0x0400CAB8 RID: 51896
						public static LocString NAME = "Grape Glovelets";

						// Token: 0x0400CAB9 RID: 51897
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x0200332D RID: 13101
					public class CUFFLESS_LEMON
					{
						// Token: 0x0400CABA RID: 51898
						public static LocString NAME = "Lemon Glovelets";

						// Token: 0x0400CABB RID: 51899
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x0200332E RID: 13102
					public class CUFFLESS_LIME
					{
						// Token: 0x0400CABC RID: 51900
						public static LocString NAME = "Lime Glovelets";

						// Token: 0x0400CABD RID: 51901
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x0200332F RID: 13103
					public class CUFFLESS_SATSUMA
					{
						// Token: 0x0400CABE RID: 51902
						public static LocString NAME = "Satsuma Glovelets";

						// Token: 0x0400CABF RID: 51903
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003330 RID: 13104
					public class CUFFLESS_STRAWBERRY
					{
						// Token: 0x0400CAC0 RID: 51904
						public static LocString NAME = "Strawberry Glovelets";

						// Token: 0x0400CAC1 RID: 51905
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003331 RID: 13105
					public class CUFFLESS_WATERMELON
					{
						// Token: 0x0400CAC2 RID: 51906
						public static LocString NAME = "Watermelon Glovelets";

						// Token: 0x0400CAC3 RID: 51907
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x02003332 RID: 13106
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400CAC4 RID: 51908
						public static LocString NAME = "LED Gloves";

						// Token: 0x0400CAC5 RID: 51909
						public static LocString DESC = "Great for gesticulating at parties.";
					}

					// Token: 0x02003333 RID: 13107
					public class ATHLETE
					{
						// Token: 0x0400CAC6 RID: 51910
						public static LocString NAME = "Racing Gloves";

						// Token: 0x0400CAC7 RID: 51911
						public static LocString DESC = "Crafted for high-speed handshakes.";
					}

					// Token: 0x02003334 RID: 13108
					public class BASIC_BROWN_KHAKI
					{
						// Token: 0x0400CAC8 RID: 51912
						public static LocString NAME = "Basic Khaki Gloves";

						// Token: 0x0400CAC9 RID: 51913
						public static LocString DESC = "They don't show dirt.";
					}

					// Token: 0x02003335 RID: 13109
					public class BASIC_BLUEGREY
					{
						// Token: 0x0400CACA RID: 51914
						public static LocString NAME = "Basic Gunmetal Gloves";

						// Token: 0x0400CACB RID: 51915
						public static LocString DESC = "A tough name for soft gloves.";
					}

					// Token: 0x02003336 RID: 13110
					public class CUFFLESS_BLACK
					{
						// Token: 0x0400CACC RID: 51916
						public static LocString NAME = "Stealth Glovelets";

						// Token: 0x0400CACD RID: 51917
						public static LocString DESC = "It's easy to forget they're even on.";
					}

					// Token: 0x02003337 RID: 13111
					public class DENIM_BLUE
					{
						// Token: 0x0400CACE RID: 51918
						public static LocString NAME = "Denim Gloves";

						// Token: 0x0400CACF RID: 51919
						public static LocString DESC = "They're not great for dexterity.";
					}
				}
			}

			// Token: 0x02002509 RID: 9481
			public class CLOTHING_TOPS
			{
				// Token: 0x0400A263 RID: 41571
				public static LocString NAME = "Default Top";

				// Token: 0x0400A264 RID: 41572
				public static LocString DESC = "The default shirt.";

				// Token: 0x02002FF1 RID: 12273
				public class FACADES
				{
					// Token: 0x02003338 RID: 13112
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400CAD0 RID: 51920
						public static LocString NAME = "Basic Aqua Shirt";

						// Token: 0x0400CAD1 RID: 51921
						public static LocString DESC = "A nice aqua-blue shirt that goes with everything.";
					}

					// Token: 0x02003339 RID: 13113
					public class BASIC_BLACK
					{
						// Token: 0x0400CAD2 RID: 51922
						public static LocString NAME = "Basic Black Shirt";

						// Token: 0x0400CAD3 RID: 51923
						public static LocString DESC = "A nice black shirt that goes with everything.";
					}

					// Token: 0x0200333A RID: 13114
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400CAD4 RID: 51924
						public static LocString NAME = "Basic Bubblegum Shirt";

						// Token: 0x0400CAD5 RID: 51925
						public static LocString DESC = "A nice bubblegum-pink shirt that goes with everything.";
					}

					// Token: 0x0200333B RID: 13115
					public class BASIC_GREEN
					{
						// Token: 0x0400CAD6 RID: 51926
						public static LocString NAME = "Basic Green Shirt";

						// Token: 0x0400CAD7 RID: 51927
						public static LocString DESC = "A nice green shirt that goes with everything.";
					}

					// Token: 0x0200333C RID: 13116
					public class BASIC_ORANGE
					{
						// Token: 0x0400CAD8 RID: 51928
						public static LocString NAME = "Basic Orange Shirt";

						// Token: 0x0400CAD9 RID: 51929
						public static LocString DESC = "A nice orange shirt that goes with everything.";
					}

					// Token: 0x0200333D RID: 13117
					public class BASIC_PURPLE
					{
						// Token: 0x0400CADA RID: 51930
						public static LocString NAME = "Basic Purple Shirt";

						// Token: 0x0400CADB RID: 51931
						public static LocString DESC = "A nice purple shirt that goes with everything.";
					}

					// Token: 0x0200333E RID: 13118
					public class BASIC_RED_BURNT
					{
						// Token: 0x0400CADC RID: 51932
						public static LocString NAME = "Basic Red Shirt";

						// Token: 0x0400CADD RID: 51933
						public static LocString DESC = "A nice red shirt that goes with everything.";
					}

					// Token: 0x0200333F RID: 13119
					public class BASIC_WHITE
					{
						// Token: 0x0400CADE RID: 51934
						public static LocString NAME = "Basic White Shirt";

						// Token: 0x0400CADF RID: 51935
						public static LocString DESC = "A nice white shirt that goes with everything.";
					}

					// Token: 0x02003340 RID: 13120
					public class BASIC_YELLOW
					{
						// Token: 0x0400CAE0 RID: 51936
						public static LocString NAME = "Basic Yellow Shirt";

						// Token: 0x0400CAE1 RID: 51937
						public static LocString DESC = "A nice yellow shirt that goes with everything.";
					}

					// Token: 0x02003341 RID: 13121
					public class RAGLANTOP_DEEPRED
					{
						// Token: 0x0400CAE2 RID: 51938
						public static LocString NAME = "Team Captain T-shirt";

						// Token: 0x0400CAE3 RID: 51939
						public static LocString DESC = "A slightly sweat-stained tee for natural leaders.";
					}

					// Token: 0x02003342 RID: 13122
					public class RAGLANTOP_COBALT
					{
						// Token: 0x0400CAE4 RID: 51940
						public static LocString NAME = "True Blue T-shirt";

						// Token: 0x0400CAE5 RID: 51941
						public static LocString DESC = "A slightly sweat-stained tee for the real team players.";
					}

					// Token: 0x02003343 RID: 13123
					public class RAGLANTOP_FLAMINGO
					{
						// Token: 0x0400CAE6 RID: 51942
						public static LocString NAME = "Pep Rally T-shirt";

						// Token: 0x0400CAE7 RID: 51943
						public static LocString DESC = "A slightly sweat-stained tee to boost team spirits.";
					}

					// Token: 0x02003344 RID: 13124
					public class RAGLANTOP_KELLYGREEN
					{
						// Token: 0x0400CAE8 RID: 51944
						public static LocString NAME = "Go Team T-shirt";

						// Token: 0x0400CAE9 RID: 51945
						public static LocString DESC = "A slightly sweat-stained tee for cheering from the sidelines.";
					}

					// Token: 0x02003345 RID: 13125
					public class RAGLANTOP_CHARCOAL
					{
						// Token: 0x0400CAEA RID: 51946
						public static LocString NAME = "Underdog T-shirt";

						// Token: 0x0400CAEB RID: 51947
						public static LocString DESC = "For those who don't win a lot.";
					}

					// Token: 0x02003346 RID: 13126
					public class RAGLANTOP_LEMON
					{
						// Token: 0x0400CAEC RID: 51948
						public static LocString NAME = "Hype T-shirt";

						// Token: 0x0400CAED RID: 51949
						public static LocString DESC = "A slightly sweat-stained tee to wear when talking a big game.";
					}

					// Token: 0x02003347 RID: 13127
					public class RAGLANTOP_SATSUMA
					{
						// Token: 0x0400CAEE RID: 51950
						public static LocString NAME = "Superfan T-shirt";

						// Token: 0x0400CAEF RID: 51951
						public static LocString DESC = "A slightly sweat-stained tee for the long-time supporter.";
					}

					// Token: 0x02003348 RID: 13128
					public class JELLYPUFFJACKET_BLUEBERRY
					{
						// Token: 0x0400CAF0 RID: 51952
						public static LocString NAME = "Blueberry Jelly Jacket";

						// Token: 0x0400CAF1 RID: 51953
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02003349 RID: 13129
					public class JELLYPUFFJACKET_GRAPE
					{
						// Token: 0x0400CAF2 RID: 51954
						public static LocString NAME = "Grape Jelly Jacket";

						// Token: 0x0400CAF3 RID: 51955
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x0200334A RID: 13130
					public class JELLYPUFFJACKET_LEMON
					{
						// Token: 0x0400CAF4 RID: 51956
						public static LocString NAME = "Lemon Jelly Jacket";

						// Token: 0x0400CAF5 RID: 51957
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x0200334B RID: 13131
					public class JELLYPUFFJACKET_LIME
					{
						// Token: 0x0400CAF6 RID: 51958
						public static LocString NAME = "Lime Jelly Jacket";

						// Token: 0x0400CAF7 RID: 51959
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x0200334C RID: 13132
					public class JELLYPUFFJACKET_SATSUMA
					{
						// Token: 0x0400CAF8 RID: 51960
						public static LocString NAME = "Satsuma Jelly Jacket";

						// Token: 0x0400CAF9 RID: 51961
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x0200334D RID: 13133
					public class JELLYPUFFJACKET_STRAWBERRY
					{
						// Token: 0x0400CAFA RID: 51962
						public static LocString NAME = "Strawberry Jelly Jacket";

						// Token: 0x0400CAFB RID: 51963
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x0200334E RID: 13134
					public class JELLYPUFFJACKET_WATERMELON
					{
						// Token: 0x0400CAFC RID: 51964
						public static LocString NAME = "Watermelon Jelly Jacket";

						// Token: 0x0400CAFD RID: 51965
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x0200334F RID: 13135
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400CAFE RID: 51966
						public static LocString NAME = "LED Jacket";

						// Token: 0x0400CAFF RID: 51967
						public static LocString DESC = "For dancing in the dark.";
					}

					// Token: 0x02003350 RID: 13136
					public class TSHIRT_WHITE
					{
						// Token: 0x0400CB00 RID: 51968
						public static LocString NAME = "Classic White Tee";

						// Token: 0x0400CB01 RID: 51969
						public static LocString DESC = "It's practically begging for a big Bog Jelly stain down the front.";
					}

					// Token: 0x02003351 RID: 13137
					public class TSHIRT_MAGENTA
					{
						// Token: 0x0400CB02 RID: 51970
						public static LocString NAME = "Classic Magenta Tee";

						// Token: 0x0400CB03 RID: 51971
						public static LocString DESC = "It will never chafe against delicate inner-elbow skin.";
					}

					// Token: 0x02003352 RID: 13138
					public class ATHLETE
					{
						// Token: 0x0400CB04 RID: 51972
						public static LocString NAME = "Racing Jacket";

						// Token: 0x0400CB05 RID: 51973
						public static LocString DESC = "The epitome of fast fashion.";
					}

					// Token: 0x02003353 RID: 13139
					public class DENIM_BLUE
					{
						// Token: 0x0400CB06 RID: 51974
						public static LocString NAME = "Denim Jacket";

						// Token: 0x0400CB07 RID: 51975
						public static LocString DESC = "The top half of a Canadian tuxedo.";
					}

					// Token: 0x02003354 RID: 13140
					public class GONCH_STRAWBERRY
					{
						// Token: 0x0400CB08 RID: 51976
						public static LocString NAME = "Executive Undershirt";

						// Token: 0x0400CB09 RID: 51977
						public static LocString DESC = "The breathable base layer every power suit needs.";
					}

					// Token: 0x02003355 RID: 13141
					public class GONCH_SATSUMA
					{
						// Token: 0x0400CB0A RID: 51978
						public static LocString NAME = "Underling Undershirt";

						// Token: 0x0400CB0B RID: 51979
						public static LocString DESC = "Extra-absorbent fabric in the underarms to mop up nervous sweat.";
					}

					// Token: 0x02003356 RID: 13142
					public class GONCH_LEMON
					{
						// Token: 0x0400CB0C RID: 51980
						public static LocString NAME = "Groupthink Undershirt";

						// Token: 0x0400CB0D RID: 51981
						public static LocString DESC = "Because the most popular choice is always the right choice.";
					}

					// Token: 0x02003357 RID: 13143
					public class GONCH_LIME
					{
						// Token: 0x0400CB0E RID: 51982
						public static LocString NAME = "Stakeholder Undershirt";

						// Token: 0x0400CB0F RID: 51983
						public static LocString DESC = "Soft against the skin, for those who have skin in the game.";
					}

					// Token: 0x02003358 RID: 13144
					public class GONCH_BLUEBERRY
					{
						// Token: 0x0400CB10 RID: 51984
						public static LocString NAME = "Admin Undershirt";

						// Token: 0x0400CB11 RID: 51985
						public static LocString DESC = "Criminally underappreciated.";
					}

					// Token: 0x02003359 RID: 13145
					public class GONCH_GRAPE
					{
						// Token: 0x0400CB12 RID: 51986
						public static LocString NAME = "Buzzword Undershirt";

						// Token: 0x0400CB13 RID: 51987
						public static LocString DESC = "A value-added vest for touching base and thinking outside the box using best practices ASAP.";
					}

					// Token: 0x0200335A RID: 13146
					public class GONCH_WATERMELON
					{
						// Token: 0x0400CB14 RID: 51988
						public static LocString NAME = "Synergy Undershirt";

						// Token: 0x0400CB15 RID: 51989
						public static LocString DESC = "Asking for it by name often triggers dramatic eye-rolls from bystanders.";
					}

					// Token: 0x0200335B RID: 13147
					public class NERD_BROWN
					{
						// Token: 0x0400CB16 RID: 51990
						public static LocString NAME = "Research Shirt";

						// Token: 0x0400CB17 RID: 51991
						public static LocString DESC = "Comes with a thoughtfully chewed-up ballpoint pen.";
					}

					// Token: 0x0200335C RID: 13148
					public class GI_WHITE
					{
						// Token: 0x0400CB18 RID: 51992
						public static LocString NAME = "Rebel Gi Jacket";

						// Token: 0x0400CB19 RID: 51993
						public static LocString DESC = "The contrasting trim hides stains from messy post-sparring snacks.";
					}
				}
			}

			// Token: 0x0200250A RID: 9482
			public class CLOTHING_BOTTOMS
			{
				// Token: 0x0400A265 RID: 41573
				public static LocString NAME = "Default Bottom";

				// Token: 0x0400A266 RID: 41574
				public static LocString DESC = "The default bottoms.";

				// Token: 0x02002FF2 RID: 12274
				public class FACADES
				{
					// Token: 0x0200335D RID: 13149
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400CB1A RID: 51994
						public static LocString NAME = "Basic Aqua Pants";

						// Token: 0x0400CB1B RID: 51995
						public static LocString DESC = "A clean pair of aqua-blue pants that go with everything.";
					}

					// Token: 0x0200335E RID: 13150
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400CB1C RID: 51996
						public static LocString NAME = "Basic Bubblegum Pants";

						// Token: 0x0400CB1D RID: 51997
						public static LocString DESC = "A clean pair of bubblegum-pink pants that go with everything.";
					}

					// Token: 0x0200335F RID: 13151
					public class BASIC_GREEN
					{
						// Token: 0x0400CB1E RID: 51998
						public static LocString NAME = "Basic Green Pants";

						// Token: 0x0400CB1F RID: 51999
						public static LocString DESC = "A clean pair of green pants that go with everything.";
					}

					// Token: 0x02003360 RID: 13152
					public class BASIC_ORANGE
					{
						// Token: 0x0400CB20 RID: 52000
						public static LocString NAME = "Basic Orange Pants";

						// Token: 0x0400CB21 RID: 52001
						public static LocString DESC = "A clean pair of orange pants that go with everything.";
					}

					// Token: 0x02003361 RID: 13153
					public class BASIC_PURPLE
					{
						// Token: 0x0400CB22 RID: 52002
						public static LocString NAME = "Basic Purple Pants";

						// Token: 0x0400CB23 RID: 52003
						public static LocString DESC = "A clean pair of purple pants that go with everything.";
					}

					// Token: 0x02003362 RID: 13154
					public class BASIC_RED
					{
						// Token: 0x0400CB24 RID: 52004
						public static LocString NAME = "Basic Red Pants";

						// Token: 0x0400CB25 RID: 52005
						public static LocString DESC = "A clean pair of red pants that go with everything.";
					}

					// Token: 0x02003363 RID: 13155
					public class BASIC_WHITE
					{
						// Token: 0x0400CB26 RID: 52006
						public static LocString NAME = "Basic White Pants";

						// Token: 0x0400CB27 RID: 52007
						public static LocString DESC = "A clean pair of white pants that go with everything.";
					}

					// Token: 0x02003364 RID: 13156
					public class BASIC_YELLOW
					{
						// Token: 0x0400CB28 RID: 52008
						public static LocString NAME = "Basic Yellow Pants";

						// Token: 0x0400CB29 RID: 52009
						public static LocString DESC = "A clean pair of yellow pants that go with everything.";
					}

					// Token: 0x02003365 RID: 13157
					public class BASIC_BLACK
					{
						// Token: 0x0400CB2A RID: 52010
						public static LocString NAME = "Basic Black Pants";

						// Token: 0x0400CB2B RID: 52011
						public static LocString DESC = "A clean pair of black pants that go with everything.";
					}

					// Token: 0x02003366 RID: 13158
					public class SHORTS_BASIC_DEEPRED
					{
						// Token: 0x0400CB2C RID: 52012
						public static LocString NAME = "Team Captain Shorts";

						// Token: 0x0400CB2D RID: 52013
						public static LocString DESC = "A fresh pair of shorts for natural leaders.";
					}

					// Token: 0x02003367 RID: 13159
					public class SHORTS_BASIC_SATSUMA
					{
						// Token: 0x0400CB2E RID: 52014
						public static LocString NAME = "Superfan Shorts";

						// Token: 0x0400CB2F RID: 52015
						public static LocString DESC = "A fresh pair of shorts for long-time supporters of...shorts.";
					}

					// Token: 0x02003368 RID: 13160
					public class SHORTS_BASIC_YELLOWCAKE
					{
						// Token: 0x0400CB30 RID: 52016
						public static LocString NAME = "Yellowcake Shorts";

						// Token: 0x0400CB31 RID: 52017
						public static LocString DESC = "A fresh pair of uranium-powder-colored shorts that are definitely not radioactive. Probably.";
					}

					// Token: 0x02003369 RID: 13161
					public class SHORTS_BASIC_KELLYGREEN
					{
						// Token: 0x0400CB32 RID: 52018
						public static LocString NAME = "Go Team Shorts";

						// Token: 0x0400CB33 RID: 52019
						public static LocString DESC = "A fresh pair of shorts for cheering from the sidelines.";
					}

					// Token: 0x0200336A RID: 13162
					public class SHORTS_BASIC_BLUE_COBALT
					{
						// Token: 0x0400CB34 RID: 52020
						public static LocString NAME = "True Blue Shorts";

						// Token: 0x0400CB35 RID: 52021
						public static LocString DESC = "A fresh pair of shorts for the real team players.";
					}

					// Token: 0x0200336B RID: 13163
					public class SHORTS_BASIC_PINK_FLAMINGO
					{
						// Token: 0x0400CB36 RID: 52022
						public static LocString NAME = "Pep Rally Shorts";

						// Token: 0x0400CB37 RID: 52023
						public static LocString DESC = "The peppiest pair of shorts this side of the asteroid.";
					}

					// Token: 0x0200336C RID: 13164
					public class SHORTS_BASIC_CHARCOAL
					{
						// Token: 0x0400CB38 RID: 52024
						public static LocString NAME = "Underdog Shorts";

						// Token: 0x0400CB39 RID: 52025
						public static LocString DESC = "A fresh pair of shorts. They're cleaner than they look.";
					}

					// Token: 0x0200336D RID: 13165
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400CB3A RID: 52026
						public static LocString NAME = "LED Pants";

						// Token: 0x0400CB3B RID: 52027
						public static LocString DESC = "These legs are lit.";
					}

					// Token: 0x0200336E RID: 13166
					public class ATHLETE
					{
						// Token: 0x0400CB3C RID: 52028
						public static LocString NAME = "Racing Pants";

						// Token: 0x0400CB3D RID: 52029
						public static LocString DESC = "Fast, furious fashion.";
					}

					// Token: 0x0200336F RID: 13167
					public class BASIC_LIGHTBROWN
					{
						// Token: 0x0400CB3E RID: 52030
						public static LocString NAME = "Basic Khaki Pants";

						// Token: 0x0400CB3F RID: 52031
						public static LocString DESC = "Transition effortlessly from subterranean day to subterranean night.";
					}

					// Token: 0x02003370 RID: 13168
					public class BASIC_REDORANGE
					{
						// Token: 0x0400CB40 RID: 52032
						public static LocString NAME = "Basic Crimson Pants";

						// Token: 0x0400CB41 RID: 52033
						public static LocString DESC = "Like red pants, but slightly fancier-sounding.";
					}

					// Token: 0x02003371 RID: 13169
					public class GONCH_STRAWBERRY
					{
						// Token: 0x0400CB42 RID: 52034
						public static LocString NAME = "Executive Briefs";

						// Token: 0x0400CB43 RID: 52035
						public static LocString DESC = "Bossy (under)pants.";
					}

					// Token: 0x02003372 RID: 13170
					public class GONCH_SATSUMA
					{
						// Token: 0x0400CB44 RID: 52036
						public static LocString NAME = "Underling Briefs";

						// Token: 0x0400CB45 RID: 52037
						public static LocString DESC = "The seams are already unraveling.";
					}

					// Token: 0x02003373 RID: 13171
					public class GONCH_LEMON
					{
						// Token: 0x0400CB46 RID: 52038
						public static LocString NAME = "Groupthink Briefs";

						// Token: 0x0400CB47 RID: 52039
						public static LocString DESC = "All the cool people are wearing them.";
					}

					// Token: 0x02003374 RID: 13172
					public class GONCH_LIME
					{
						// Token: 0x0400CB48 RID: 52040
						public static LocString NAME = "Stakeholder Briefs";

						// Token: 0x0400CB49 RID: 52041
						public static LocString DESC = "They're really invested in keeping the wearer comfortable.";
					}

					// Token: 0x02003375 RID: 13173
					public class GONCH_BLUEBERRY
					{
						// Token: 0x0400CB4A RID: 52042
						public static LocString NAME = "Admin Briefs";

						// Token: 0x0400CB4B RID: 52043
						public static LocString DESC = "The workhorse of the underwear world.";
					}

					// Token: 0x02003376 RID: 13174
					public class GONCH_GRAPE
					{
						// Token: 0x0400CB4C RID: 52044
						public static LocString NAME = "Buzzword Briefs";

						// Token: 0x0400CB4D RID: 52045
						public static LocString DESC = "Underwear that works hard, plays hard, and gives 110% to maximize the \"bottom\" line.";
					}

					// Token: 0x02003377 RID: 13175
					public class GONCH_WATERMELON
					{
						// Token: 0x0400CB4E RID: 52046
						public static LocString NAME = "Synergy Briefs";

						// Token: 0x0400CB4F RID: 52047
						public static LocString DESC = "Teamwork makes the dream work.";
					}

					// Token: 0x02003378 RID: 13176
					public class DENIM_BLUE
					{
						// Token: 0x0400CB50 RID: 52048
						public static LocString NAME = "Jeans";

						// Token: 0x0400CB51 RID: 52049
						public static LocString DESC = "The bottom half of a Canadian tuxedo.";
					}

					// Token: 0x02003379 RID: 13177
					public class GI_WHITE
					{
						// Token: 0x0400CB52 RID: 52050
						public static LocString NAME = "White Capris";

						// Token: 0x0400CB53 RID: 52051
						public static LocString DESC = "The cropped length is ideal for wading through flooded hallways.";
					}

					// Token: 0x0200337A RID: 13178
					public class NERD_BROWN
					{
						// Token: 0x0400CB54 RID: 52052
						public static LocString NAME = "Research Pants";

						// Token: 0x0400CB55 RID: 52053
						public static LocString DESC = "The pockets are full of illegible notes that didn't quite survive the wash.";
					}

					// Token: 0x0200337B RID: 13179
					public class SKIRT_BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400CB56 RID: 52054
						public static LocString NAME = "Aqua Rayon Skirt";

						// Token: 0x0400CB57 RID: 52055
						public static LocString DESC = "The tag says \"Dry Clean Only.\" There are no dry cleaners in space.";
					}

					// Token: 0x0200337C RID: 13180
					public class SKIRT_BASIC_PURPLE
					{
						// Token: 0x0400CB58 RID: 52056
						public static LocString NAME = "Purple Rayon Skirt";

						// Token: 0x0400CB59 RID: 52057
						public static LocString DESC = "It's not the most breathable fabric, but it <i>is</i> a lovely shade of purple.";
					}

					// Token: 0x0200337D RID: 13181
					public class SKIRT_BASIC_GREEN
					{
						// Token: 0x0400CB5A RID: 52058
						public static LocString NAME = "Olive Rayon Skirt";

						// Token: 0x0400CB5B RID: 52059
						public static LocString DESC = "Designed not to get snagged on ladders.";
					}

					// Token: 0x0200337E RID: 13182
					public class SKIRT_BASIC_ORANGE
					{
						// Token: 0x0400CB5C RID: 52060
						public static LocString NAME = "Apricot Rayon Skirt";

						// Token: 0x0400CB5D RID: 52061
						public static LocString DESC = "Ready for spontaneous workplace twirling.";
					}

					// Token: 0x0200337F RID: 13183
					public class SKIRT_BASIC_PINK_ORCHID
					{
						// Token: 0x0400CB5E RID: 52062
						public static LocString NAME = "Bubblegum Rayon Skirt";

						// Token: 0x0400CB5F RID: 52063
						public static LocString DESC = "The bubblegum scent lasts 100 washes!";
					}

					// Token: 0x02003380 RID: 13184
					public class SKIRT_BASIC_RED
					{
						// Token: 0x0400CB60 RID: 52064
						public static LocString NAME = "Garnet Rayon Skirt";

						// Token: 0x0400CB61 RID: 52065
						public static LocString DESC = "It's business time.";
					}

					// Token: 0x02003381 RID: 13185
					public class SKIRT_BASIC_YELLOW
					{
						// Token: 0x0400CB62 RID: 52066
						public static LocString NAME = "Yellow Rayon Skirt";

						// Token: 0x0400CB63 RID: 52067
						public static LocString DESC = "A formerly white skirt that has not aged well.";
					}

					// Token: 0x02003382 RID: 13186
					public class SKIRT_BASIC_POLKADOT
					{
						// Token: 0x0400CB64 RID: 52068
						public static LocString NAME = "Polka Dot Skirt";

						// Token: 0x0400CB65 RID: 52069
						public static LocString DESC = "Polka dots are a way to infinity.";
					}

					// Token: 0x02003383 RID: 13187
					public class SKIRT_BASIC_WATERMELON
					{
						// Token: 0x0400CB66 RID: 52070
						public static LocString NAME = "Picnic Skirt";

						// Token: 0x0400CB67 RID: 52071
						public static LocString DESC = "The seeds are spittable, but will bear no fruit.";
					}

					// Token: 0x02003384 RID: 13188
					public class SKIRT_DENIM_BLUE
					{
						// Token: 0x0400CB68 RID: 52072
						public static LocString NAME = "Denim Tux Skirt";

						// Token: 0x0400CB69 RID: 52073
						public static LocString DESC = "Designed for the casual red carpet.";
					}

					// Token: 0x02003385 RID: 13189
					public class SKIRT_LEOPARD_PRINT_BLUE_PINK
					{
						// Token: 0x0400CB6A RID: 52074
						public static LocString NAME = "Disco Leopard Skirt";

						// Token: 0x0400CB6B RID: 52075
						public static LocString DESC = "A faux-fur party staple.";
					}

					// Token: 0x02003386 RID: 13190
					public class SKIRT_SPARKLE_BLUE
					{
						// Token: 0x0400CB6C RID: 52076
						public static LocString NAME = "Blue Tinsel Skirt";

						// Token: 0x0400CB6D RID: 52077
						public static LocString DESC = "The tinsel is scratchy, but look how shiny!";
					}
				}
			}

			// Token: 0x0200250B RID: 9483
			public class CLOTHING_SHOES
			{
				// Token: 0x0400A267 RID: 41575
				public static LocString NAME = "Default Footwear";

				// Token: 0x0400A268 RID: 41576
				public static LocString DESC = "The default style of footwear.";

				// Token: 0x02002FF3 RID: 12275
				public class FACADES
				{
					// Token: 0x02003387 RID: 13191
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400CB6E RID: 52078
						public static LocString NAME = "Basic Aqua Shoes";

						// Token: 0x0400CB6F RID: 52079
						public static LocString DESC = "A fresh pair of aqua-blue shoes that go with everything.";
					}

					// Token: 0x02003388 RID: 13192
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400CB70 RID: 52080
						public static LocString NAME = "Basic Bubblegum Shoes";

						// Token: 0x0400CB71 RID: 52081
						public static LocString DESC = "A fresh pair of bubblegum-pink shoes that go with everything.";
					}

					// Token: 0x02003389 RID: 13193
					public class BASIC_GREEN
					{
						// Token: 0x0400CB72 RID: 52082
						public static LocString NAME = "Basic Green Shoes";

						// Token: 0x0400CB73 RID: 52083
						public static LocString DESC = "A fresh pair of green shoes that go with everything.";
					}

					// Token: 0x0200338A RID: 13194
					public class BASIC_ORANGE
					{
						// Token: 0x0400CB74 RID: 52084
						public static LocString NAME = "Basic Orange Shoes";

						// Token: 0x0400CB75 RID: 52085
						public static LocString DESC = "A fresh pair of orange shoes that go with everything.";
					}

					// Token: 0x0200338B RID: 13195
					public class BASIC_PURPLE
					{
						// Token: 0x0400CB76 RID: 52086
						public static LocString NAME = "Basic Purple Shoes";

						// Token: 0x0400CB77 RID: 52087
						public static LocString DESC = "A fresh pair of purple shoes that go with everything.";
					}

					// Token: 0x0200338C RID: 13196
					public class BASIC_RED
					{
						// Token: 0x0400CB78 RID: 52088
						public static LocString NAME = "Basic Red Shoes";

						// Token: 0x0400CB79 RID: 52089
						public static LocString DESC = "A fresh pair of red shoes that go with everything.";
					}

					// Token: 0x0200338D RID: 13197
					public class BASIC_WHITE
					{
						// Token: 0x0400CB7A RID: 52090
						public static LocString NAME = "Basic White Shoes";

						// Token: 0x0400CB7B RID: 52091
						public static LocString DESC = "A fresh pair of white shoes that go with everything.";
					}

					// Token: 0x0200338E RID: 13198
					public class BASIC_YELLOW
					{
						// Token: 0x0400CB7C RID: 52092
						public static LocString NAME = "Basic Yellow Shoes";

						// Token: 0x0400CB7D RID: 52093
						public static LocString DESC = "A fresh pair of yellow shoes that go with everything.";
					}

					// Token: 0x0200338F RID: 13199
					public class BASIC_BLACK
					{
						// Token: 0x0400CB7E RID: 52094
						public static LocString NAME = "Basic Black Shoes";

						// Token: 0x0400CB7F RID: 52095
						public static LocString DESC = "A fresh pair of black shoes that go with everything.";
					}

					// Token: 0x02003390 RID: 13200
					public class BASIC_BLUEGREY
					{
						// Token: 0x0400CB80 RID: 52096
						public static LocString NAME = "Basic Gunmetal Shoes";

						// Token: 0x0400CB81 RID: 52097
						public static LocString DESC = "A fresh pair of pastel shoes that go with everything.";
					}

					// Token: 0x02003391 RID: 13201
					public class BASIC_TAN
					{
						// Token: 0x0400CB82 RID: 52098
						public static LocString NAME = "Basic Tan Shoes";

						// Token: 0x0400CB83 RID: 52099
						public static LocString DESC = "They're remarkably unremarkable.";
					}

					// Token: 0x02003392 RID: 13202
					public class SOCKS_ATHLETIC_DEEPRED
					{
						// Token: 0x0400CB84 RID: 52100
						public static LocString NAME = "Team Captain Gym Socks";

						// Token: 0x0400CB85 RID: 52101
						public static LocString DESC = "Breathable socks with sporty red stripes.";
					}

					// Token: 0x02003393 RID: 13203
					public class SOCKS_ATHLETIC_SATSUMA
					{
						// Token: 0x0400CB86 RID: 52102
						public static LocString NAME = "Superfan Gym Socks";

						// Token: 0x0400CB87 RID: 52103
						public static LocString DESC = "Breathable socks with sporty orange stripes.";
					}

					// Token: 0x02003394 RID: 13204
					public class SOCKS_ATHLETIC_LEMON
					{
						// Token: 0x0400CB88 RID: 52104
						public static LocString NAME = "Hype Gym Socks";

						// Token: 0x0400CB89 RID: 52105
						public static LocString DESC = "Breathable socks with sporty yellow stripes.";
					}

					// Token: 0x02003395 RID: 13205
					public class SOCKS_ATHLETIC_KELLYGREEN
					{
						// Token: 0x0400CB8A RID: 52106
						public static LocString NAME = "Go Team Gym Socks";

						// Token: 0x0400CB8B RID: 52107
						public static LocString DESC = "Breathable socks with sporty green stripes.";
					}

					// Token: 0x02003396 RID: 13206
					public class SOCKS_ATHLETIC_COBALT
					{
						// Token: 0x0400CB8C RID: 52108
						public static LocString NAME = "True Blue Gym Socks";

						// Token: 0x0400CB8D RID: 52109
						public static LocString DESC = "Breathable socks with sporty blue stripes.";
					}

					// Token: 0x02003397 RID: 13207
					public class SOCKS_ATHLETIC_FLAMINGO
					{
						// Token: 0x0400CB8E RID: 52110
						public static LocString NAME = "Pep Rally Gym Socks";

						// Token: 0x0400CB8F RID: 52111
						public static LocString DESC = "Breathable socks with sporty pink stripes.";
					}

					// Token: 0x02003398 RID: 13208
					public class SOCKS_ATHLETIC_CHARCOAL
					{
						// Token: 0x0400CB90 RID: 52112
						public static LocString NAME = "Underdog Gym Socks";

						// Token: 0x0400CB91 RID: 52113
						public static LocString DESC = "Breathable socks that do nothing whatsoever to eliminate foot odor.";
					}

					// Token: 0x02003399 RID: 13209
					public class BASIC_GREY
					{
						// Token: 0x0400CB92 RID: 52114
						public static LocString NAME = "Basic Gray Shoes";

						// Token: 0x0400CB93 RID: 52115
						public static LocString DESC = "A fresh pair of gray shoes that go with everything.";
					}

					// Token: 0x0200339A RID: 13210
					public class DENIM_BLUE
					{
						// Token: 0x0400CB94 RID: 52116
						public static LocString NAME = "Denim Shoes";

						// Token: 0x0400CB95 RID: 52117
						public static LocString DESC = "Not technically essential for a Canadian tuxedo, but why not?";
					}

					// Token: 0x0200339B RID: 13211
					public class LEGWARMERS_STRAWBERRY
					{
						// Token: 0x0400CB96 RID: 52118
						public static LocString NAME = "Slouchy Strawberry Socks";

						// Token: 0x0400CB97 RID: 52119
						public static LocString DESC = "Freckly knitted socks that don't stay up.";
					}

					// Token: 0x0200339C RID: 13212
					public class LEGWARMERS_SATSUMA
					{
						// Token: 0x0400CB98 RID: 52120
						public static LocString NAME = "Slouchy Satsuma Socks";

						// Token: 0x0400CB99 RID: 52121
						public static LocString DESC = "Sweet knitted socks for spontaneous dance segments.";
					}

					// Token: 0x0200339D RID: 13213
					public class LEGWARMERS_LEMON
					{
						// Token: 0x0400CB9A RID: 52122
						public static LocString NAME = "Slouchy Lemon Socks";

						// Token: 0x0400CB9B RID: 52123
						public static LocString DESC = "Zesty knitted socks that don't stay up.";
					}

					// Token: 0x0200339E RID: 13214
					public class LEGWARMERS_LIME
					{
						// Token: 0x0400CB9C RID: 52124
						public static LocString NAME = "Slouchy Lime Socks";

						// Token: 0x0400CB9D RID: 52125
						public static LocString DESC = "Juicy knitted socks that don't stay up.";
					}

					// Token: 0x0200339F RID: 13215
					public class LEGWARMERS_BLUEBERRY
					{
						// Token: 0x0400CB9E RID: 52126
						public static LocString NAME = "Slouchy Blueberry Socks";

						// Token: 0x0400CB9F RID: 52127
						public static LocString DESC = "Knitted socks with a fun bobble-stitch texture.";
					}

					// Token: 0x020033A0 RID: 13216
					public class LEGWARMERS_GRAPE
					{
						// Token: 0x0400CBA0 RID: 52128
						public static LocString NAME = "Slouchy Grape Socks";

						// Token: 0x0400CBA1 RID: 52129
						public static LocString DESC = "These fabulous knitted that don't stay up. are really raisin the bar.";
					}

					// Token: 0x020033A1 RID: 13217
					public class LEGWARMERS_WATERMELON
					{
						// Token: 0x0400CBA2 RID: 52130
						public static LocString NAME = "Slouchy Watermelon Socks";

						// Token: 0x0400CBA3 RID: 52131
						public static LocString DESC = "Summery knitted socks that don't stay up.";
					}
				}
			}

			// Token: 0x0200250C RID: 9484
			public class CLOTHING_HATS
			{
				// Token: 0x0400A269 RID: 41577
				public static LocString NAME = "Default Headgear";

				// Token: 0x0400A26A RID: 41578
				public static LocString DESC = "<DESC>";

				// Token: 0x02002FF4 RID: 12276
				public class FACADES
				{
				}
			}

			// Token: 0x0200250D RID: 9485
			public class CLOTHING_ACCESORIES
			{
				// Token: 0x0400A26B RID: 41579
				public static LocString NAME = "Default Accessory";

				// Token: 0x0400A26C RID: 41580
				public static LocString DESC = "<DESC>";

				// Token: 0x02002FF5 RID: 12277
				public class FACADES
				{
				}
			}

			// Token: 0x0200250E RID: 9486
			public class OXYGEN_TANK
			{
				// Token: 0x0400A26D RID: 41581
				public static LocString NAME = UI.FormatAsLink("Oxygen Tank", "OXYGEN_TANK");

				// Token: 0x0400A26E RID: 41582
				public static LocString GENERICNAME = "Equipment";

				// Token: 0x0400A26F RID: 41583
				public static LocString DESC = "It's like a to-go bag for your lungs.";

				// Token: 0x0400A270 RID: 41584
				public static LocString EFFECT = "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>.";

				// Token: 0x0400A271 RID: 41585
				public static LocString RECIPE_DESC = "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>";
			}

			// Token: 0x0200250F RID: 9487
			public class OXYGEN_TANK_UNDERWATER
			{
				// Token: 0x0400A272 RID: 41586
				public static LocString NAME = "Oxygen Rebreather";

				// Token: 0x0400A273 RID: 41587
				public static LocString GENERICNAME = "Equipment";

				// Token: 0x0400A274 RID: 41588
				public static LocString DESC = "";

				// Token: 0x0400A275 RID: 41589
				public static LocString EFFECT = "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid.";

				// Token: 0x0400A276 RID: 41590
				public static LocString RECIPE_DESC = "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid";
			}

			// Token: 0x02002510 RID: 9488
			public class EQUIPPABLEBALLOON
			{
				// Token: 0x0400A277 RID: 41591
				public static LocString NAME = UI.FormatAsLink("Balloon Friend", "EQUIPPABLEBALLOON");

				// Token: 0x0400A278 RID: 41592
				public static LocString DESC = "A floating friend to reassure my Duplicants they are so very, very clever.";

				// Token: 0x0400A279 RID: 41593
				public static LocString EFFECT = "Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist " + UI.FormatAsLink("Overjoyed", "MORALE") + " response.";

				// Token: 0x0400A27A RID: 41594
				public static LocString RECIPE_DESC = "Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist " + UI.FormatAsLink("Overjoyed", "MORALE") + " response";

				// Token: 0x0400A27B RID: 41595
				public static LocString GENERICNAME = "Balloon Friend";

				// Token: 0x02002FF6 RID: 12278
				public class FACADES
				{
					// Token: 0x020033A2 RID: 13218
					public class DEFAULT_BALLOON
					{
						// Token: 0x0400CBA4 RID: 52132
						public static LocString NAME = UI.FormatAsLink("Balloon Friend", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBA5 RID: 52133
						public static LocString DESC = "A floating friend to reassure my Duplicants that they are so very, very clever.";
					}

					// Token: 0x020033A3 RID: 13219
					public class BALLOON_FIREENGINE_LONG_SPARKLES
					{
						// Token: 0x0400CBA6 RID: 52134
						public static LocString NAME = UI.FormatAsLink("Magma Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBA7 RID: 52135
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x020033A4 RID: 13220
					public class BALLOON_YELLOW_LONG_SPARKLES
					{
						// Token: 0x0400CBA8 RID: 52136
						public static LocString NAME = UI.FormatAsLink("Lavatory Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBA9 RID: 52137
						public static LocString DESC = "Sparkly balloons in an all-too-familiar hue.";
					}

					// Token: 0x020033A5 RID: 13221
					public class BALLOON_BLUE_LONG_SPARKLES
					{
						// Token: 0x0400CBAA RID: 52138
						public static LocString NAME = UI.FormatAsLink("Wheezewort Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBAB RID: 52139
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x020033A6 RID: 13222
					public class BALLOON_GREEN_LONG_SPARKLES
					{
						// Token: 0x0400CBAC RID: 52140
						public static LocString NAME = UI.FormatAsLink("Mush Bar Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBAD RID: 52141
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x020033A7 RID: 13223
					public class BALLOON_PINK_LONG_SPARKLES
					{
						// Token: 0x0400CBAE RID: 52142
						public static LocString NAME = UI.FormatAsLink("Petal Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBAF RID: 52143
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x020033A8 RID: 13224
					public class BALLOON_PURPLE_LONG_SPARKLES
					{
						// Token: 0x0400CBB0 RID: 52144
						public static LocString NAME = UI.FormatAsLink("Dusky Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBB1 RID: 52145
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x020033A9 RID: 13225
					public class BALLOON_BABY_PACU_EGG
					{
						// Token: 0x0400CBB2 RID: 52146
						public static LocString NAME = UI.FormatAsLink("Floatie Fish", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBB3 RID: 52147
						public static LocString DESC = "They do not taste as good as the real thing.";
					}

					// Token: 0x020033AA RID: 13226
					public class BALLOON_BABY_GLOSSY_DRECKO_EGG
					{
						// Token: 0x0400CBB4 RID: 52148
						public static LocString NAME = UI.FormatAsLink("Glossy Glee", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBB5 RID: 52149
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020033AB RID: 13227
					public class BALLOON_BABY_HATCH_EGG
					{
						// Token: 0x0400CBB6 RID: 52150
						public static LocString NAME = UI.FormatAsLink("Helium Hatches", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBB7 RID: 52151
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020033AC RID: 13228
					public class BALLOON_BABY_POKESHELL_EGG
					{
						// Token: 0x0400CBB8 RID: 52152
						public static LocString NAME = UI.FormatAsLink("Peppy Pokeshells", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBB9 RID: 52153
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020033AD RID: 13229
					public class BALLOON_BABY_PUFT_EGG
					{
						// Token: 0x0400CBBA RID: 52154
						public static LocString NAME = UI.FormatAsLink("Puffed-Up Pufts", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBBB RID: 52155
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020033AE RID: 13230
					public class BALLOON_BABY_SHOVOLE_EGG
					{
						// Token: 0x0400CBBC RID: 52156
						public static LocString NAME = UI.FormatAsLink("Voley Voley Voles", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBBD RID: 52157
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020033AF RID: 13231
					public class BALLOON_BABY_PIP_EGG
					{
						// Token: 0x0400CBBE RID: 52158
						public static LocString NAME = UI.FormatAsLink("Pip Pip Hooray", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBBF RID: 52159
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020033B0 RID: 13232
					public class CANDY_BLUEBERRY
					{
						// Token: 0x0400CBC0 RID: 52160
						public static LocString NAME = UI.FormatAsLink("Candied Blueberry", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBC1 RID: 52161
						public static LocString DESC = "A juicy bunch of blueberry-scented balloons.";
					}

					// Token: 0x020033B1 RID: 13233
					public class CANDY_GRAPE
					{
						// Token: 0x0400CBC2 RID: 52162
						public static LocString NAME = UI.FormatAsLink("Candied Grape", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBC3 RID: 52163
						public static LocString DESC = "A juicy bunch of grape-scented balloons.";
					}

					// Token: 0x020033B2 RID: 13234
					public class CANDY_LEMON
					{
						// Token: 0x0400CBC4 RID: 52164
						public static LocString NAME = UI.FormatAsLink("Candied Lemon", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBC5 RID: 52165
						public static LocString DESC = "A juicy lemon-scented bunch of balloons.";
					}

					// Token: 0x020033B3 RID: 13235
					public class CANDY_LIME
					{
						// Token: 0x0400CBC6 RID: 52166
						public static LocString NAME = UI.FormatAsLink("Candied Lime", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBC7 RID: 52167
						public static LocString DESC = "A juicy lime-scented bunch of balloons.";
					}

					// Token: 0x020033B4 RID: 13236
					public class CANDY_ORANGE
					{
						// Token: 0x0400CBC8 RID: 52168
						public static LocString NAME = UI.FormatAsLink("Candied Satsuma", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBC9 RID: 52169
						public static LocString DESC = "A juicy satsuma-scented bunch of balloons.";
					}

					// Token: 0x020033B5 RID: 13237
					public class CANDY_STRAWBERRY
					{
						// Token: 0x0400CBCA RID: 52170
						public static LocString NAME = UI.FormatAsLink("Candied Strawberry", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBCB RID: 52171
						public static LocString DESC = "A juicy strawberry-scented bunch of balloons.";
					}

					// Token: 0x020033B6 RID: 13238
					public class CANDY_WATERMELON
					{
						// Token: 0x0400CBCC RID: 52172
						public static LocString NAME = UI.FormatAsLink("Candied Watermelon", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBCD RID: 52173
						public static LocString DESC = "A juicy watermelon-scented bunch of balloons.";
					}

					// Token: 0x020033B7 RID: 13239
					public class HAND_GOLD
					{
						// Token: 0x0400CBCE RID: 52174
						public static LocString NAME = UI.FormatAsLink("Gold Fingers", "EQUIPPABLEBALLOON");

						// Token: 0x0400CBCF RID: 52175
						public static LocString DESC = "Inflatable gestures of encouragement.";
					}
				}
			}

			// Token: 0x02002511 RID: 9489
			public class SLEEPCLINICPAJAMAS
			{
				// Token: 0x0400A27C RID: 41596
				public static LocString NAME = UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS");

				// Token: 0x0400A27D RID: 41597
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400A27E RID: 41598
				public static LocString DESC = "A soft, fleecy ticket to dreamland.";

				// Token: 0x0400A27F RID: 41599
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Helps Duplicants fall asleep by reducing ",
					UI.FormatAsLink("Stamina", "STAMINA"),
					".\n\nEnables the wearer to dream and produce ",
					UI.FormatAsLink("Dream Journals", "DREAMJOURNAL"),
					"."
				});

				// Token: 0x0400A280 RID: 41600
				public static LocString DESTROY_TOAST = "Ripped Pajamas";
			}
		}
	}
}
