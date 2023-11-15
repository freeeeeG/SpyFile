using System;

namespace STRINGS
{
	// Token: 0x02000DAF RID: 3503
	public class CREATURES
	{
		// Token: 0x04005125 RID: 20773
		public static LocString BAGGED_NAME_FMT = "Bagged {0}";

		// Token: 0x04005126 RID: 20774
		public static LocString BAGGED_DESC_FMT = "This {0} has been captured and is now safe to relocate.";

		// Token: 0x02001DE6 RID: 7654
		public class FAMILY
		{
			// Token: 0x04008993 RID: 35219
			public static LocString HATCH = UI.FormatAsLink("Hatch", "HATCHSPECIES");

			// Token: 0x04008994 RID: 35220
			public static LocString LIGHTBUG = UI.FormatAsLink("Shine Bug", "LIGHTBUGSPECIES");

			// Token: 0x04008995 RID: 35221
			public static LocString OILFLOATER = UI.FormatAsLink("Slickster", "OILFLOATERSPECIES");

			// Token: 0x04008996 RID: 35222
			public static LocString DRECKO = UI.FormatAsLink("Drecko", "DRECKOSPECIES");

			// Token: 0x04008997 RID: 35223
			public static LocString GLOM = UI.FormatAsLink("Morb", "GLOMSPECIES");

			// Token: 0x04008998 RID: 35224
			public static LocString PUFT = UI.FormatAsLink("Puft", "PUFTSPECIES");

			// Token: 0x04008999 RID: 35225
			public static LocString PACU = UI.FormatAsLink("Pacu", "PACUSPECIES");

			// Token: 0x0400899A RID: 35226
			public static LocString MOO = UI.FormatAsLink("Moo", "MOOSPECIES");

			// Token: 0x0400899B RID: 35227
			public static LocString MOLE = UI.FormatAsLink("Shove Vole", "MOLESPECIES");

			// Token: 0x0400899C RID: 35228
			public static LocString SQUIRREL = UI.FormatAsLink("Pip", "SQUIRRELSPECIES");

			// Token: 0x0400899D RID: 35229
			public static LocString CRAB = UI.FormatAsLink("Pokeshell", "CRABSPECIES");

			// Token: 0x0400899E RID: 35230
			public static LocString STATERPILLAR = UI.FormatAsLink("Plug Slug", "STATERPILLARSPECIES");

			// Token: 0x0400899F RID: 35231
			public static LocString DIVERGENTSPECIES = UI.FormatAsLink("Divergent", "DIVERGENTSPECIES");

			// Token: 0x040089A0 RID: 35232
			public static LocString SWEEPBOT = UI.FormatAsLink("Sweepies", "SWEEPBOT");

			// Token: 0x040089A1 RID: 35233
			public static LocString SCOUTROVER = UI.FormatAsLink("Rover", "SCOUTROVER");
		}

		// Token: 0x02001DE7 RID: 7655
		public class FAMILY_PLURAL
		{
			// Token: 0x040089A2 RID: 35234
			public static LocString HATCHSPECIES = UI.FormatAsLink("Hatches", "HATCHSPECIES");

			// Token: 0x040089A3 RID: 35235
			public static LocString LIGHTBUGSPECIES = UI.FormatAsLink("Shine Bugs", "LIGHTBUGSPECIES");

			// Token: 0x040089A4 RID: 35236
			public static LocString OILFLOATERSPECIES = UI.FormatAsLink("Slicksters", "OILFLOATERSPECIES");

			// Token: 0x040089A5 RID: 35237
			public static LocString DRECKOSPECIES = UI.FormatAsLink("Dreckos", "DRECKOSPECIES");

			// Token: 0x040089A6 RID: 35238
			public static LocString GLOMSPECIES = UI.FormatAsLink("Morbs", "GLOMSPECIES");

			// Token: 0x040089A7 RID: 35239
			public static LocString PUFTSPECIES = UI.FormatAsLink("Pufts", "PUFTSPECIES");

			// Token: 0x040089A8 RID: 35240
			public static LocString PACUSPECIES = UI.FormatAsLink("Pacus", "PACUSPECIES");

			// Token: 0x040089A9 RID: 35241
			public static LocString MOOSPECIES = UI.FormatAsLink("Moos", "MOOSPECIES");

			// Token: 0x040089AA RID: 35242
			public static LocString MOLESPECIES = UI.FormatAsLink("Shove Voles", "MOLESPECIES");

			// Token: 0x040089AB RID: 35243
			public static LocString CRABSPECIES = UI.FormatAsLink("Pokeshells", "CRABSPECIES");

			// Token: 0x040089AC RID: 35244
			public static LocString SQUIRRELSPECIES = UI.FormatAsLink("Pips", "SQUIRRELSPECIES");

			// Token: 0x040089AD RID: 35245
			public static LocString STATERPILLARSPECIES = UI.FormatAsLink("Plug Slugs", "STATERPILLARSPECIES");

			// Token: 0x040089AE RID: 35246
			public static LocString BEETASPECIES = UI.FormatAsLink("Beetas", "BEETASPECIES");

			// Token: 0x040089AF RID: 35247
			public static LocString DIVERGENTSPECIES = UI.FormatAsLink("Divergents", "DIVERGENTSPECIES");

			// Token: 0x040089B0 RID: 35248
			public static LocString SWEEPBOT = UI.FormatAsLink("Sweepies", "SWEEPBOT");

			// Token: 0x040089B1 RID: 35249
			public static LocString SCOUTROVER = UI.FormatAsLink("Rovers", "SCOUTROVER");
		}

		// Token: 0x02001DE8 RID: 7656
		public class PLANT_MUTATIONS
		{
			// Token: 0x040089B2 RID: 35250
			public static LocString PLANT_NAME_FMT = "{PlantName} ({MutationList})";

			// Token: 0x040089B3 RID: 35251
			public static LocString UNIDENTIFIED = "Unidentified Subspecies";

			// Token: 0x040089B4 RID: 35252
			public static LocString UNIDENTIFIED_DESC = "This seed must be identified at the " + BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME + " before it can be planted.";

			// Token: 0x040089B5 RID: 35253
			public static LocString BONUS_CROP_FMT = "Bonus Crop: +{Amount} {Crop}";

			// Token: 0x02002927 RID: 10535
			public class NONE
			{
				// Token: 0x0400B19C RID: 45468
				public static LocString NAME = "Original";
			}

			// Token: 0x02002928 RID: 10536
			public class MODERATELYLOOSE
			{
				// Token: 0x0400B19D RID: 45469
				public static LocString NAME = "Easygoing";

				// Token: 0x0400B19E RID: 45470
				public static LocString DESCRIPTION = "Plants with this mutation are easier to take care of, but don't yield as much produce.";
			}

			// Token: 0x02002929 RID: 10537
			public class MODERATELYTIGHT
			{
				// Token: 0x0400B19F RID: 45471
				public static LocString NAME = "Specialized";

				// Token: 0x0400B1A0 RID: 45472
				public static LocString DESCRIPTION = "Plants with this mutation are pickier about their conditions but yield more produce.";
			}

			// Token: 0x0200292A RID: 10538
			public class EXTREMELYTIGHT
			{
				// Token: 0x0400B1A1 RID: 45473
				public static LocString NAME = "Superspecialized";

				// Token: 0x0400B1A2 RID: 45474
				public static LocString DESCRIPTION = "Plants with this mutation are very difficult to keep alive, but produce a bounty.";
			}

			// Token: 0x0200292B RID: 10539
			public class BONUSLICE
			{
				// Token: 0x0400B1A3 RID: 45475
				public static LocString NAME = "Licey";

				// Token: 0x0400B1A4 RID: 45476
				public static LocString DESCRIPTION = "Something about this mutation causes Meal Lice to pupate on this plant.";
			}

			// Token: 0x0200292C RID: 10540
			public class SUNNYSPEED
			{
				// Token: 0x0400B1A5 RID: 45477
				public static LocString NAME = "Leafy";

				// Token: 0x0400B1A6 RID: 45478
				public static LocString DESCRIPTION = "This mutation provides the plant with sun-collecting leaves, allowing faster growth.";
			}

			// Token: 0x0200292D RID: 10541
			public class SLOWBURN
			{
				// Token: 0x0400B1A7 RID: 45479
				public static LocString NAME = "Wildish";

				// Token: 0x0400B1A8 RID: 45480
				public static LocString DESCRIPTION = "These plants grow almost as slow as their wild cousins, but also consume almost no fertilizer.";
			}

			// Token: 0x0200292E RID: 10542
			public class BLOOMS
			{
				// Token: 0x0400B1A9 RID: 45481
				public static LocString NAME = "Blooming";

				// Token: 0x0400B1AA RID: 45482
				public static LocString DESCRIPTION = "Vestigial flowers increase the beauty of this plant. Don't inhale the pollen, though!";
			}

			// Token: 0x0200292F RID: 10543
			public class LOADEDWITHFRUIT
			{
				// Token: 0x0400B1AB RID: 45483
				public static LocString NAME = "Bountiful";

				// Token: 0x0400B1AC RID: 45484
				public static LocString DESCRIPTION = "This mutation produces lots of extra produce, though it also takes a long time to pick it all!";
			}

			// Token: 0x02002930 RID: 10544
			public class ROTTENHEAPS
			{
				// Token: 0x0400B1AD RID: 45485
				public static LocString NAME = "Exuberant";

				// Token: 0x0400B1AE RID: 45486
				public static LocString DESCRIPTION = "Plants with this mutation grow extremely quickly, though the produce they make is sometimes questionable.";
			}

			// Token: 0x02002931 RID: 10545
			public class HEAVYFRUIT
			{
				// Token: 0x0400B1AF RID: 45487
				public static LocString NAME = "Juicy Fruits";

				// Token: 0x0400B1B0 RID: 45488
				public static LocString DESCRIPTION = "Extra water in these plump mutant veggies causes them to fall right off the plant! There's no extra nutritional value, though...";
			}
		}

		// Token: 0x02001DE9 RID: 7657
		public class SPECIES
		{
			// Token: 0x02002932 RID: 10546
			public class CRAB
			{
				// Token: 0x0400B1B1 RID: 45489
				public static LocString NAME = UI.FormatAsLink("Pokeshell", "Crab");

				// Token: 0x0400B1B2 RID: 45490
				public static LocString DESC = string.Concat(new string[]
				{
					"Pokeshells are nonhostile critters unless their eggs are nearby.\n\nThey eat ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" and ",
					UI.FormatAsLink("Rot Piles", "COMPOST"),
					".\n\nThe shells they leave behind after molting can be crushed into ",
					UI.FormatAsLink("Lime", "LIME"),
					"."
				});

				// Token: 0x0400B1B3 RID: 45491
				public static LocString EGG_NAME = UI.FormatAsLink("Pinch Roe", "Crab");

				// Token: 0x02003179 RID: 12665
				public class BABY
				{
					// Token: 0x0400C635 RID: 50741
					public static LocString NAME = UI.FormatAsLink("Pokeshell Spawn", "CRAB");

					// Token: 0x0400C636 RID: 50742
					public static LocString DESC = "A snippy little Pokeshell Spawn.\n\nIn time it will mature into a fully grown " + UI.FormatAsLink("Pokeshell", "CRAB") + ".";
				}

				// Token: 0x0200317A RID: 12666
				public class VARIANT_WOOD
				{
					// Token: 0x0400C637 RID: 50743
					public static LocString NAME = UI.FormatAsLink("Oakshell", "CRABWOOD");

					// Token: 0x0400C638 RID: 50744
					public static LocString DESC = string.Concat(new string[]
					{
						"Oakshells are nonhostile critters unless their eggs are nearby.\n\nThey eat ",
						UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
						", ",
						UI.FormatAsLink("Slime", "SLIMEMOLD"),
						" and ",
						UI.FormatAsLink("Rot Piles", "COMPOST"),
						".\n\nThe shells they leave behind after molting can be crushed into ",
						UI.FormatAsLink("Lumber", "WOOD"),
						".\n\nOakshells thrive in ",
						UI.FormatAsLink("Ethanol", "ETHANOL"),
						"."
					});

					// Token: 0x0400C639 RID: 50745
					public static LocString EGG_NAME = UI.FormatAsLink("Oak Pinch Roe", "CRABWOOD");

					// Token: 0x02003416 RID: 13334
					public class BABY
					{
						// Token: 0x0400CCBB RID: 52411
						public static LocString NAME = UI.FormatAsLink("Oakshell Spawn", "CRABWOOD");

						// Token: 0x0400CCBC RID: 52412
						public static LocString DESC = "A knotty little Oakshell Spawn.\n\nIn time it will mature into a fully grown " + UI.FormatAsLink("Oakshell", "CRABWOOD") + ".";
					}
				}

				// Token: 0x0200317B RID: 12667
				public class VARIANT_FRESH_WATER
				{
					// Token: 0x0400C63A RID: 50746
					public static LocString NAME = UI.FormatAsLink("Sanishell", "CRABFRESHWATER");

					// Token: 0x0400C63B RID: 50747
					public static LocString DESC = string.Concat(new string[]
					{
						"Sanishells are nonhostile critters unless their eggs are nearby.\n\nThey thrive in ",
						UI.FormatAsLink("Water", "WATER"),
						" and eliminate ",
						UI.FormatAsLink("Germs", "DISEASE"),
						" from any liquid it inhabits.\n\nThey eat ",
						UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
						", ",
						UI.FormatAsLink("Slime", "SLIMEMOLD"),
						" and ",
						UI.FormatAsLink("Rot Piles", "COMPOST"),
						"."
					});

					// Token: 0x0400C63C RID: 50748
					public static LocString EGG_NAME = UI.FormatAsLink("Sani Pinch Roe", "CRABFRESHWATER");

					// Token: 0x02003417 RID: 13335
					public class BABY
					{
						// Token: 0x0400CCBD RID: 52413
						public static LocString NAME = UI.FormatAsLink("Sanishell Spawn", "CRABFRESHWATER");

						// Token: 0x0400CCBE RID: 52414
						public static LocString DESC = "A picky little Sanishell Spawn.\n\nIn time it will mature into a fully grown " + UI.FormatAsLink("Sanishell", "CRABFRESHWATER") + ".";
					}
				}
			}

			// Token: 0x02002933 RID: 10547
			public class BEE
			{
				// Token: 0x0400B1B4 RID: 45492
				public static LocString NAME = UI.FormatAsLink("Beeta", "BEE");

				// Token: 0x0400B1B5 RID: 45493
				public static LocString DESC = string.Concat(new string[]
				{
					"Beetas are hostile critters that thrive in ",
					UI.FormatAsLink("Radioactive", "RADIATION"),
					" environments.\n\nThey commonly gather ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" for their ",
					UI.FormatAsLink("Beeta Hives", "BEEHIVE"),
					" to produce ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					"."
				});

				// Token: 0x0200317C RID: 12668
				public class BABY
				{
					// Token: 0x0400C63D RID: 50749
					public static LocString NAME = UI.FormatAsLink("Beetiny", "BEE");

					// Token: 0x0400C63E RID: 50750
					public static LocString DESC = "A harmless little Beetiny.\n\nIn time, it will mature into a vicious adult " + UI.FormatAsLink("Beeta", "BEE") + ".";
				}
			}

			// Token: 0x02002934 RID: 10548
			public class CHLORINEGEYSER
			{
				// Token: 0x0400B1B6 RID: 45494
				public static LocString NAME = UI.FormatAsLink("Chlorine Geyser", "GeyserGeneric_CHLORINE_GAS");

				// Token: 0x0400B1B7 RID: 45495
				public static LocString DESC = "A highly pressurized geyser that periodically erupts with " + UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS") + ".";
			}

			// Token: 0x02002935 RID: 10549
			public class PACU
			{
				// Token: 0x0400B1B8 RID: 45496
				public static LocString NAME = UI.FormatAsLink("Pacu", "PACU");

				// Token: 0x0400B1B9 RID: 45497
				public static LocString DESC = string.Concat(new string[]
				{
					"Pacus are aquatic creatures that can live in any liquid, such as ",
					UI.FormatAsLink("Water", "WATER"),
					" or ",
					UI.FormatAsLink("Contaminated Water", "DIRTYWATER"),
					".\n\nEvery organism in the known universe finds the Pacu extremely delicious."
				});

				// Token: 0x0400B1BA RID: 45498
				public static LocString EGG_NAME = UI.FormatAsLink("Fry Egg", "PACU");

				// Token: 0x0200317D RID: 12669
				public class BABY
				{
					// Token: 0x0400C63F RID: 50751
					public static LocString NAME = UI.FormatAsLink("Pacu Fry", "PACU");

					// Token: 0x0400C640 RID: 50752
					public static LocString DESC = "A wriggly little Pacu Fry.\n\nIn time, it will mature into an adult " + UI.FormatAsLink("Pacu", "PACU") + ".";
				}

				// Token: 0x0200317E RID: 12670
				public class VARIANT_TROPICAL
				{
					// Token: 0x0400C641 RID: 50753
					public static LocString NAME = UI.FormatAsLink("Tropical Pacu", "PACUTROPICAL");

					// Token: 0x0400C642 RID: 50754
					public static LocString DESC = "Every organism in the known universe finds the Pacu extremely delicious.";

					// Token: 0x0400C643 RID: 50755
					public static LocString EGG_NAME = UI.FormatAsLink("Tropical Fry Egg", "PACUTROPICAL");

					// Token: 0x02003418 RID: 13336
					public class BABY
					{
						// Token: 0x0400CCBF RID: 52415
						public static LocString NAME = UI.FormatAsLink("Tropical Fry", "PACUTROPICAL");

						// Token: 0x0400CCC0 RID: 52416
						public static LocString DESC = "A wriggly little Tropical Fry.\n\nIn time it will mature into an adult Pacu morph, the " + UI.FormatAsLink("Tropical Pacu", "PACUTROPICAL") + ".";
					}
				}

				// Token: 0x0200317F RID: 12671
				public class VARIANT_CLEANER
				{
					// Token: 0x0400C644 RID: 50756
					public static LocString NAME = UI.FormatAsLink("Gulp Fish", "PACUCLEANER");

					// Token: 0x0400C645 RID: 50757
					public static LocString DESC = "Every organism in the known universe finds the Pacu extremely delicious.";

					// Token: 0x0400C646 RID: 50758
					public static LocString EGG_NAME = UI.FormatAsLink("Gulp Fry Egg", "PACUCLEANER");

					// Token: 0x02003419 RID: 13337
					public class BABY
					{
						// Token: 0x0400CCC1 RID: 52417
						public static LocString NAME = UI.FormatAsLink("Gulp Fry", "PACUCLEANER");

						// Token: 0x0400CCC2 RID: 52418
						public static LocString DESC = "A wriggly little Gulp Fry.\n\nIn time, it will mature into an adult " + UI.FormatAsLink("Gulp Fish", "PACUCLEANER") + ".";
					}
				}
			}

			// Token: 0x02002936 RID: 10550
			public class GLOM
			{
				// Token: 0x0400B1BB RID: 45499
				public static LocString NAME = UI.FormatAsLink("Morb", "GLOM");

				// Token: 0x0400B1BC RID: 45500
				public static LocString DESC = "Morbs are attracted to unhygienic conditions and frequently excrete bursts of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + ".";

				// Token: 0x0400B1BD RID: 45501
				public static LocString EGG_NAME = UI.FormatAsLink("Morb Pod", "MORB");
			}

			// Token: 0x02002937 RID: 10551
			public class HATCH
			{
				// Token: 0x0400B1BE RID: 45502
				public static LocString NAME = UI.FormatAsLink("Hatch", "HATCH");

				// Token: 0x0400B1BF RID: 45503
				public static LocString DESC = "Hatches excrete solid " + UI.FormatAsLink("Coal", "CARBON") + " as waste and may be uncovered by digging up Buried Objects.";

				// Token: 0x0400B1C0 RID: 45504
				public static LocString EGG_NAME = UI.FormatAsLink("Hatchling Egg", "HATCH");

				// Token: 0x02003180 RID: 12672
				public class BABY
				{
					// Token: 0x0400C647 RID: 50759
					public static LocString NAME = UI.FormatAsLink("Hatchling", "HATCH");

					// Token: 0x0400C648 RID: 50760
					public static LocString DESC = "An innocent little Hatchling.\n\nIn time, it will mature into an adult " + UI.FormatAsLink("Hatch", "HATCH") + ".";
				}

				// Token: 0x02003181 RID: 12673
				public class VARIANT_HARD
				{
					// Token: 0x0400C649 RID: 50761
					public static LocString NAME = UI.FormatAsLink("Stone Hatch", "HATCHHARD");

					// Token: 0x0400C64A RID: 50762
					public static LocString DESC = "Stone Hatches excrete solid " + UI.FormatAsLink("Coal", "CARBON") + " as waste and enjoy burrowing into the ground.";

					// Token: 0x0400C64B RID: 50763
					public static LocString EGG_NAME = UI.FormatAsLink("Stone Hatchling Egg", "HATCHHARD");

					// Token: 0x0200341A RID: 13338
					public class BABY
					{
						// Token: 0x0400CCC3 RID: 52419
						public static LocString NAME = UI.FormatAsLink("Stone Hatchling", "HATCHHARD");

						// Token: 0x0400CCC4 RID: 52420
						public static LocString DESC = "A doofy little Stone Hatchling.\n\nIt matures into an adult Hatch morph, the " + UI.FormatAsLink("Stone Hatch", "HATCHHARD") + ", which loves nibbling on various rocks and metals.";
					}
				}

				// Token: 0x02003182 RID: 12674
				public class VARIANT_VEGGIE
				{
					// Token: 0x0400C64C RID: 50764
					public static LocString NAME = UI.FormatAsLink("Sage Hatch", "HATCHVEGGIE");

					// Token: 0x0400C64D RID: 50765
					public static LocString DESC = "Sage Hatches excrete solid " + UI.FormatAsLink("Coal", "CARBON") + " as waste and enjoy burrowing into the ground.";

					// Token: 0x0400C64E RID: 50766
					public static LocString EGG_NAME = UI.FormatAsLink("Sage Hatchling Egg", "HATCHVEGGIE");

					// Token: 0x0200341B RID: 13339
					public class BABY
					{
						// Token: 0x0400CCC5 RID: 52421
						public static LocString NAME = UI.FormatAsLink("Sage Hatchling", "HATCHVEGGIE");

						// Token: 0x0400CCC6 RID: 52422
						public static LocString DESC = "A doofy little Sage Hatchling.\n\nIt matures into an adult Hatch morph, the " + UI.FormatAsLink("Sage Hatch", "HATCHVEGGIE") + ", which loves nibbling on organic materials.";
					}
				}

				// Token: 0x02003183 RID: 12675
				public class VARIANT_METAL
				{
					// Token: 0x0400C64F RID: 50767
					public static LocString NAME = UI.FormatAsLink("Smooth Hatch", "HATCHMETAL");

					// Token: 0x0400C650 RID: 50768
					public static LocString DESC = string.Concat(new string[]
					{
						"Smooth Hatches enjoy burrowing into the ground and excrete ",
						UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
						" when fed ",
						UI.FormatAsLink("Metal Ore", "RAWMETAL"),
						"."
					});

					// Token: 0x0400C651 RID: 50769
					public static LocString EGG_NAME = UI.FormatAsLink("Smooth Hatchling Egg", "HATCHMETAL");

					// Token: 0x0200341C RID: 13340
					public class BABY
					{
						// Token: 0x0400CCC7 RID: 52423
						public static LocString NAME = UI.FormatAsLink("Smooth Hatchling", "HATCHMETAL");

						// Token: 0x0400CCC8 RID: 52424
						public static LocString DESC = "A doofy little Smooth Hatchling.\n\nIt matures into an adult Hatch morph, the " + UI.FormatAsLink("Smooth Hatch", "HATCHMETAL") + ", which loves nibbling on different types of metals.";
					}
				}
			}

			// Token: 0x02002938 RID: 10552
			public class STATERPILLAR
			{
				// Token: 0x0400B1C1 RID: 45505
				public static LocString NAME = UI.FormatAsLink("Plug Slug", "STATERPILLAR");

				// Token: 0x0400B1C2 RID: 45506
				public static LocString DESC = "Plug Slugs are dynamic creatures that generate electrical " + UI.FormatAsLink("Power", "POWER") + " during the night.\n\nTheir power can be harnessed by leaving an exposed wire near areas where they like to sleep.";

				// Token: 0x0400B1C3 RID: 45507
				public static LocString EGG_NAME = UI.FormatAsLink("Slug Egg", "STATERPILLAR");

				// Token: 0x02003184 RID: 12676
				public class BABY
				{
					// Token: 0x0400C652 RID: 50770
					public static LocString NAME = UI.FormatAsLink("Plug Sluglet", "STATERPILLAR");

					// Token: 0x0400C653 RID: 50771
					public static LocString DESC = "A chubby little Plug Sluglet.\n\nIn time it will mature into a fully grown " + UI.FormatAsLink("Plug Slug", "STATERPILLAR") + ".";
				}

				// Token: 0x02003185 RID: 12677
				public class VARIANT_GAS
				{
					// Token: 0x0400C654 RID: 50772
					public static LocString NAME = UI.FormatAsLink("Smog Slug", "STATERPILLAR");

					// Token: 0x0400C655 RID: 50773
					public static LocString DESC = string.Concat(new string[]
					{
						"Smog Slugs are porous creatures that draw in unbreathable ",
						UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
						" during the day.\n\nAt night, they sleep near exposed ",
						UI.FormatAsLink("Gas Pipes,", "GASCONDUIT"),
						" where they deposit their cache."
					});

					// Token: 0x0400C656 RID: 50774
					public static LocString EGG_NAME = UI.FormatAsLink("Smog Slug Egg", "STATERPILLAR");

					// Token: 0x0200341D RID: 13341
					public class BABY
					{
						// Token: 0x0400CCC9 RID: 52425
						public static LocString NAME = UI.FormatAsLink("Smog Sluglet", "STATERPILLAR");

						// Token: 0x0400CCCA RID: 52426
						public static LocString DESC = "A tubby little Smog Sluglet.\n\nIn time it will mature into a fully grown " + UI.FormatAsLink("Smog Slug", "STATERPILLAR") + ".";
					}
				}

				// Token: 0x02003186 RID: 12678
				public class VARIANT_LIQUID
				{
					// Token: 0x0400C657 RID: 50775
					public static LocString NAME = UI.FormatAsLink("Sponge Slug", "STATERPILLAR");

					// Token: 0x0400C658 RID: 50776
					public static LocString DESC = string.Concat(new string[]
					{
						"Sponge Slugs are thirsty creatures that soak up ",
						UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
						" during the day.\n\nThey deposit their stored ",
						UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
						" into the exposed ",
						UI.FormatAsLink("Liquid Pipes", "LIQUIDCONDUIT"),
						" they sleep next to at night."
					});

					// Token: 0x0400C659 RID: 50777
					public static LocString EGG_NAME = UI.FormatAsLink("Sponge Slug Egg", "STATERPILLAR");

					// Token: 0x0200341E RID: 13342
					public class BABY
					{
						// Token: 0x0400CCCB RID: 52427
						public static LocString NAME = UI.FormatAsLink("Sponge Sluglet", "STATERPILLAR");

						// Token: 0x0400CCCC RID: 52428
						public static LocString DESC = "A chonky little Sponge Sluglet.\n\nIn time it will mature into a fully grown " + UI.FormatAsLink("Sponge Slug", "STATERPILLAR") + ".";
					}
				}
			}

			// Token: 0x02002939 RID: 10553
			public class DIVERGENT
			{
				// Token: 0x02003187 RID: 12679
				public class VARIANT_BEETLE
				{
					// Token: 0x0400C65A RID: 50778
					public static LocString NAME = UI.FormatAsLink("Sweetle", "DIVERGENTBEETLE");

					// Token: 0x0400C65B RID: 50779
					public static LocString DESC = string.Concat(new string[]
					{
						"Sweetles are nonhostile critters that excrete large amounts of ",
						UI.FormatAsLink("Sucrose", "SUCROSE"),
						".\n\nThey are closely related to the ",
						UI.FormatAsLink("Grubgrub", "DIVERGENTWORM"),
						" and exhibit similar, albeit less effective farming behaviors."
					});

					// Token: 0x0400C65C RID: 50780
					public static LocString EGG_NAME = UI.FormatAsLink("Sweetle Egg", "DIVERGENTBEETLE");

					// Token: 0x0200341F RID: 13343
					public class BABY
					{
						// Token: 0x0400CCCD RID: 52429
						public static LocString NAME = UI.FormatAsLink("Sweetle Larva", "DIVERGENTBEETLE");

						// Token: 0x0400CCCE RID: 52430
						public static LocString DESC = "A crawly little Sweetle Larva.\n\nIn time, it will mature into an adult " + UI.FormatAsLink("Sweetle", "DIVERGENTBEETLE") + ".";
					}
				}

				// Token: 0x02003188 RID: 12680
				public class VARIANT_WORM
				{
					// Token: 0x0400C65D RID: 50781
					public static LocString NAME = UI.FormatAsLink("Grubgrub", "DIVERGENTWORM");

					// Token: 0x0400C65E RID: 50782
					public static LocString DESC = string.Concat(new string[]
					{
						"Grubgrubs form symbiotic relationships with plants, especially ",
						UI.FormatAsLink("Grubfruit Plants", "WORMPLANT"),
						", and instinctually tend to them.\n\nGrubgrubs are closely related to ",
						UI.FormatAsLink("Sweetles", "DIVERGENTBEETLE"),
						"."
					});

					// Token: 0x0400C65F RID: 50783
					public static LocString EGG_NAME = UI.FormatAsLink("Grubgrub Egg", "DIVERGENTWORM");

					// Token: 0x02003420 RID: 13344
					public class BABY
					{
						// Token: 0x0400CCCF RID: 52431
						public static LocString NAME = UI.FormatAsLink("Grubgrub Wormling", "DIVERGENTWORM");

						// Token: 0x0400CCD0 RID: 52432
						public static LocString DESC = "A squirmy little Grubgrub Wormling.\n\nIn time, it will mature into an adult " + UI.FormatAsLink("Grubgrub", "WORM") + " and drastically grow in size.";
					}
				}
			}

			// Token: 0x0200293A RID: 10554
			public class DRECKO
			{
				// Token: 0x0400B1C4 RID: 45508
				public static LocString NAME = UI.FormatAsLink("Drecko", "DRECKO");

				// Token: 0x0400B1C5 RID: 45509
				public static LocString DESC = string.Concat(new string[]
				{
					"Dreckos are nonhostile critters that graze on ",
					UI.FormatAsLink("Pincha Pepperplants", "SPICE_VINE"),
					", ",
					UI.FormatAsLink("Balm Lily", "SWAMPLILY"),
					" or ",
					UI.FormatAsLink("Mealwood Plants", "BASICSINGLEHARVESTPLANT"),
					".\n\nTheir backsides are covered in thick woolly fibers that only grow in ",
					UI.FormatAsLink("Hydrogen", "HYDROGEN"),
					" climates."
				});

				// Token: 0x0400B1C6 RID: 45510
				public static LocString EGG_NAME = UI.FormatAsLink("Drecklet Egg", "DRECKO");

				// Token: 0x02003189 RID: 12681
				public class BABY
				{
					// Token: 0x0400C660 RID: 50784
					public static LocString NAME = UI.FormatAsLink("Drecklet", "DRECKO");

					// Token: 0x0400C661 RID: 50785
					public static LocString DESC = "A little, bug-eyed Drecklet.\n\nIn time it will mature into a fully grown " + UI.FormatAsLink("Drecko", "DRECKO") + ".";
				}

				// Token: 0x0200318A RID: 12682
				public class VARIANT_PLASTIC
				{
					// Token: 0x0400C662 RID: 50786
					public static LocString NAME = UI.FormatAsLink("Glossy Drecko", "DRECKOPLASTIC");

					// Token: 0x0400C663 RID: 50787
					public static LocString DESC = string.Concat(new string[]
					{
						"Glossy Dreckos are nonhostile critters that graze on live ",
						UI.FormatAsLink("Mealwood Plants", "BASICSINGLEHARVESTPLANT"),
						" and ",
						UI.FormatAsLink("Bristle Blossoms", "PRICKLEFLOWER"),
						".\n\nTheir backsides are covered in bioplastic scales that only grow in ",
						UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
						" climates."
					});

					// Token: 0x0400C664 RID: 50788
					public static LocString EGG_NAME = UI.FormatAsLink("Glossy Drecklet Egg", "DRECKOPLASTIC");

					// Token: 0x02003421 RID: 13345
					public class BABY
					{
						// Token: 0x0400CCD1 RID: 52433
						public static LocString NAME = UI.FormatAsLink("Glossy Drecklet", "DRECKOPLASTIC");

						// Token: 0x0400CCD2 RID: 52434
						public static LocString DESC = "A bug-eyed little Glossy Drecklet.\n\nIn time it will mature into an adult Drecko morph, the " + UI.FormatAsLink("Glossy Drecko", "DRECKOPLASTIC") + ".";
					}
				}
			}

			// Token: 0x0200293B RID: 10555
			public class SQUIRREL
			{
				// Token: 0x0400B1C7 RID: 45511
				public static LocString NAME = UI.FormatAsLink("Pip", "SQUIRREL");

				// Token: 0x0400B1C8 RID: 45512
				public static LocString DESC = string.Concat(new string[]
				{
					"Pips are pesky, nonhostile critters that subsist on ",
					UI.FormatAsLink("Thimble Reeds", "BASICFABRICPLANT"),
					" and ",
					UI.FormatAsLink("Arbor Tree", "FOREST_TREE"),
					" branches.\n\nThey are known to bury ",
					UI.FormatAsLink("Seeds", "PLANTS"),
					" in the ground whenever they can find a suitable area with enough space."
				});

				// Token: 0x0400B1C9 RID: 45513
				public static LocString EGG_NAME = UI.FormatAsLink("Pip Egg", "SQUIRREL");

				// Token: 0x0200318B RID: 12683
				public class BABY
				{
					// Token: 0x0400C665 RID: 50789
					public static LocString NAME = UI.FormatAsLink("Pipsqueak", "SQUIRREL");

					// Token: 0x0400C666 RID: 50790
					public static LocString DESC = "A little purring Pipsqueak.\n\nIn time it will mature into a fully grown " + UI.FormatAsLink("Pip", "SQUIRREL") + ".";
				}

				// Token: 0x0200318C RID: 12684
				public class VARIANT_HUG
				{
					// Token: 0x0400C667 RID: 50791
					public static LocString NAME = UI.FormatAsLink("Cuddle Pip", "SQUIRREL");

					// Token: 0x0400C668 RID: 50792
					public static LocString DESC = "Cuddle Pips are fluffy, affectionate critters who exhibit a strong snuggling instinct towards all types of eggs.\n\nThis is temporarily amplified when they are hugged by a passing Duplicant.";

					// Token: 0x0400C669 RID: 50793
					public static LocString EGG_NAME = UI.FormatAsLink("Cuddle Pip Egg", "SQUIRREL");

					// Token: 0x02003422 RID: 13346
					public class BABY
					{
						// Token: 0x0400CCD3 RID: 52435
						public static LocString NAME = UI.FormatAsLink("Cuddle Pipsqueak", "SQUIRREL");

						// Token: 0x0400CCD4 RID: 52436
						public static LocString DESC = "A fuzzy little Cuddle Pipsqueak.\n\nIn time it will mature into a fully grown " + UI.FormatAsLink("Cuddle Pip", "SQUIRREL") + ".";
					}
				}
			}

			// Token: 0x0200293C RID: 10556
			public class OILFLOATER
			{
				// Token: 0x0400B1CA RID: 45514
				public static LocString NAME = UI.FormatAsLink("Slickster", "OILFLOATER");

				// Token: 0x0400B1CB RID: 45515
				public static LocString DESC = string.Concat(new string[]
				{
					"Slicksters are slimy critters that consume ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and exude ",
					UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
					"."
				});

				// Token: 0x0400B1CC RID: 45516
				public static LocString EGG_NAME = UI.FormatAsLink("Larva Egg", "OILFLOATER");

				// Token: 0x0200318D RID: 12685
				public class BABY
				{
					// Token: 0x0400C66A RID: 50794
					public static LocString NAME = UI.FormatAsLink("Slickster Larva", "OILFLOATER");

					// Token: 0x0400C66B RID: 50795
					public static LocString DESC = "A goopy little Slickster Larva.\n\nOne day it will grow into an adult " + UI.FormatAsLink("Slickster", "OILFLOATER") + ".";
				}

				// Token: 0x0200318E RID: 12686
				public class VARIANT_HIGHTEMP
				{
					// Token: 0x0400C66C RID: 50796
					public static LocString NAME = UI.FormatAsLink("Molten Slickster", "OILFLOATERHIGHTEMP");

					// Token: 0x0400C66D RID: 50797
					public static LocString DESC = string.Concat(new string[]
					{
						"Molten Slicksters are slimy critters that consume ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and exude ",
						UI.FormatAsLink("Petroleum", "PETROLEUM"),
						"."
					});

					// Token: 0x0400C66E RID: 50798
					public static LocString EGG_NAME = UI.FormatAsLink("Molten Larva Egg", "OILFLOATERHIGHTEMP");

					// Token: 0x02003423 RID: 13347
					public class BABY
					{
						// Token: 0x0400CCD5 RID: 52437
						public static LocString NAME = UI.FormatAsLink("Molten Larva", "OILFLOATERHIGHTEMP");

						// Token: 0x0400CCD6 RID: 52438
						public static LocString DESC = "A goopy little Molten Larva.\n\nOne day it will grow into an adult Slickster morph, the " + UI.FormatAsLink("Molten Slickster", "OILFLOATERHIGHTEMP") + ".";
					}
				}

				// Token: 0x0200318F RID: 12687
				public class VARIANT_DECOR
				{
					// Token: 0x0400C66F RID: 50799
					public static LocString NAME = UI.FormatAsLink("Longhair Slickster", "OILFLOATERDECOR");

					// Token: 0x0400C670 RID: 50800
					public static LocString DESC = "Longhair Slicksters are friendly critters that consume " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and thrive in close contact with Duplicant companions.\n\nLonghairs have extremely beautiful and luxurious coats.";

					// Token: 0x0400C671 RID: 50801
					public static LocString EGG_NAME = UI.FormatAsLink("Longhair Larva Egg", "OILFLOATERDECOR");

					// Token: 0x02003424 RID: 13348
					public class BABY
					{
						// Token: 0x0400CCD7 RID: 52439
						public static LocString NAME = UI.FormatAsLink("Longhair Larva", "OILFLOATERDECOR");

						// Token: 0x0400CCD8 RID: 52440
						public static LocString DESC = "A snuggly little Longhair Larva.\n\nOne day it will grow into an adult Slickster morph, the " + UI.FormatAsLink("Longhair Slickster", "OILFLOATERDECOR") + ".";
					}
				}
			}

			// Token: 0x0200293D RID: 10557
			public class PUFT
			{
				// Token: 0x0400B1CD RID: 45517
				public static LocString NAME = UI.FormatAsLink("Puft", "PUFT");

				// Token: 0x0400B1CE RID: 45518
				public static LocString DESC = "Pufts are non-aggressive critters that excrete lumps of " + UI.FormatAsLink("Slime", "SLIMEMOLD") + " with each breath.";

				// Token: 0x0400B1CF RID: 45519
				public static LocString EGG_NAME = UI.FormatAsLink("Puftlet Egg", "PUFT");

				// Token: 0x02003190 RID: 12688
				public class BABY
				{
					// Token: 0x0400C672 RID: 50802
					public static LocString NAME = UI.FormatAsLink("Puftlet", "PUFT");

					// Token: 0x0400C673 RID: 50803
					public static LocString DESC = "A gassy little Puftlet.\n\nIn time it will grow into an adult " + UI.FormatAsLink("Puft", "PUFT") + ".";
				}

				// Token: 0x02003191 RID: 12689
				public class VARIANT_ALPHA
				{
					// Token: 0x0400C674 RID: 50804
					public static LocString NAME = UI.FormatAsLink("Puft Prince", "PUFTALPHA");

					// Token: 0x0400C675 RID: 50805
					public static LocString DESC = "The Puft Prince is a lazy critter that excretes little " + UI.FormatAsLink("Solid", "SOLID") + " lumps of whatever it has been breathing.";

					// Token: 0x0400C676 RID: 50806
					public static LocString EGG_NAME = UI.FormatAsLink("Puftlet Prince Egg", "PUFTALPHA");

					// Token: 0x02003425 RID: 13349
					public class BABY
					{
						// Token: 0x0400CCD9 RID: 52441
						public static LocString NAME = UI.FormatAsLink("Puftlet Prince", "PUFTALPHA");

						// Token: 0x0400CCDA RID: 52442
						public static LocString DESC = "A gassy little Puftlet Prince.\n\nOne day it will grow into an adult Puft morph, the " + UI.FormatAsLink("Puft Prince", "PUFTALPHA") + ".\n\nIt seems a bit snobby...";
					}
				}

				// Token: 0x02003192 RID: 12690
				public class VARIANT_OXYLITE
				{
					// Token: 0x0400C677 RID: 50807
					public static LocString NAME = UI.FormatAsLink("Dense Puft", "PUFTOXYLITE");

					// Token: 0x0400C678 RID: 50808
					public static LocString DESC = "Dense Pufts are non-aggressive critters that excrete condensed " + UI.FormatAsLink("Oxylite", "OXYROCK") + " with each breath.";

					// Token: 0x0400C679 RID: 50809
					public static LocString EGG_NAME = UI.FormatAsLink("Dense Puftlet Egg", "PUFTOXYLITE");

					// Token: 0x02003426 RID: 13350
					public class BABY
					{
						// Token: 0x0400CCDB RID: 52443
						public static LocString NAME = UI.FormatAsLink("Dense Puftlet", "PUFTOXYLITE");

						// Token: 0x0400CCDC RID: 52444
						public static LocString DESC = "A stocky little Dense Puftlet.\n\nOne day it will grow into an adult Puft morph, the " + UI.FormatAsLink("Dense Puft", "PUFTOXYLITE") + ".";
					}
				}

				// Token: 0x02003193 RID: 12691
				public class VARIANT_BLEACHSTONE
				{
					// Token: 0x0400C67A RID: 50810
					public static LocString NAME = UI.FormatAsLink("Squeaky Puft", "PUFTBLEACHSTONE");

					// Token: 0x0400C67B RID: 50811
					public static LocString DESC = "Squeaky Pufts are non-aggressive critters that excrete lumps of " + UI.FormatAsLink("Bleachstone", "BLEACHSTONE") + " with each breath.";

					// Token: 0x0400C67C RID: 50812
					public static LocString EGG_NAME = UI.FormatAsLink("Squeaky Puftlet Egg", "PUFTBLEACHSTONE");

					// Token: 0x02003427 RID: 13351
					public class BABY
					{
						// Token: 0x0400CCDD RID: 52445
						public static LocString NAME = UI.FormatAsLink("Squeaky Puftlet", "PUFTBLEACHSTONE");

						// Token: 0x0400CCDE RID: 52446
						public static LocString DESC = "A frazzled little Squeaky Puftlet.\n\nOne day it will grow into an adult Puft morph, the " + UI.FormatAsLink("Squeaky Puft", "PUFTBLEACHSTONE") + ".";
					}
				}
			}

			// Token: 0x0200293E RID: 10558
			public class MOO
			{
				// Token: 0x0400B1D0 RID: 45520
				public static LocString NAME = UI.FormatAsLink("Gassy Moo", "MOO");

				// Token: 0x0400B1D1 RID: 45521
				public static LocString DESC = string.Concat(new string[]
				{
					"Moos are extraterrestrial critters that feed on ",
					UI.FormatAsLink("Gas Grass", "GASGRASS"),
					" and excrete ",
					UI.FormatAsLink("Natural Gas", "METHANE"),
					".\n\nWhen domesticated and fed, they can be milked for ",
					ELEMENTS.MILK.NAME,
					"."
				});
			}

			// Token: 0x0200293F RID: 10559
			public class MOLE
			{
				// Token: 0x0400B1D2 RID: 45522
				public static LocString NAME = UI.FormatAsLink("Shove Vole", "MOLE");

				// Token: 0x0400B1D3 RID: 45523
				public static LocString DESC = string.Concat(new string[]
				{
					"Shove Voles are burrowing critters that eat the ",
					UI.FormatAsLink("Regolith", "REGOLITH"),
					" collected on terrestrial surfaces.\n\nThey cannot burrow through ",
					UI.FormatAsLink("Refined Metals", "REFINEDMETAL"),
					"."
				});

				// Token: 0x0400B1D4 RID: 45524
				public static LocString EGG_NAME = UI.FormatAsLink("Shove Vole Egg", "MOLE");

				// Token: 0x02003194 RID: 12692
				public class BABY
				{
					// Token: 0x0400C67D RID: 50813
					public static LocString NAME = UI.FormatAsLink("Vole Pup", "MOLE");

					// Token: 0x0400C67E RID: 50814
					public static LocString DESC = "A snuggly little pup.\n\nOne day it will grow into an adult " + UI.FormatAsLink("Shove Vole", "MOLE") + ".";
				}

				// Token: 0x02003195 RID: 12693
				public class VARIANT_DELICACY
				{
					// Token: 0x0400C67F RID: 50815
					public static LocString NAME = UI.FormatAsLink("Delecta Vole", "MOLEDELICACY");

					// Token: 0x0400C680 RID: 50816
					public static LocString DESC = string.Concat(new string[]
					{
						"Delecta Voles are burrowing critters whose bodies sprout shearable ",
						UI.FormatAsLink("Tonic Root", "GINGER"),
						" when ",
						UI.FormatAsLink("Regolith", "REGOLITH"),
						" is ingested at preferred temperatures.\n\nThey cannot burrow through ",
						UI.FormatAsLink("Refined Metals", "REFINEDMETAL"),
						"."
					});

					// Token: 0x0400C681 RID: 50817
					public static LocString EGG_NAME = UI.FormatAsLink("Delecta Vole Egg", "MOLEDELICACY");

					// Token: 0x02003428 RID: 13352
					public class BABY
					{
						// Token: 0x0400CCDF RID: 52447
						public static LocString NAME = UI.FormatAsLink("Delecta Vole Pup", "MOLEDELICACY");

						// Token: 0x0400CCE0 RID: 52448
						public static LocString DESC = "A tender little Delecta Vole pup.\n\nOne day it will grow into an adult Shove Vole morph, the " + UI.FormatAsLink("Delecta Vole", "MOLEDELICACY") + ".";
					}
				}
			}

			// Token: 0x02002940 RID: 10560
			public class GREEDYGREEN
			{
				// Token: 0x0400B1D5 RID: 45525
				public static LocString NAME = UI.FormatAsLink("Avari Vine", "GREEDYGREEN");

				// Token: 0x0400B1D6 RID: 45526
				public static LocString DESC = "A rapidly growing, subterranean " + UI.FormatAsLink("Plant", "PLANTS") + ".";
			}

			// Token: 0x02002941 RID: 10561
			public class SHOCKWORM
			{
				// Token: 0x0400B1D7 RID: 45527
				public static LocString NAME = UI.FormatAsLink("Shockworm", "SHOCKWORM");

				// Token: 0x0400B1D8 RID: 45528
				public static LocString DESC = "Shockworms are exceptionally aggressive and discharge electrical shocks to stun their prey.";
			}

			// Token: 0x02002942 RID: 10562
			public class LIGHTBUG
			{
				// Token: 0x0400B1D9 RID: 45529
				public static LocString NAME = UI.FormatAsLink("Shine Bug", "LIGHTBUG");

				// Token: 0x0400B1DA RID: 45530
				public static LocString DESC = "Shine Bugs emit a soft " + UI.FormatAsLink("Light", "LIGHT") + " in hopes of attracting more of their kind for company.";

				// Token: 0x0400B1DB RID: 45531
				public static LocString EGG_NAME = UI.FormatAsLink("Shine Nymph Egg", "LIGHTBUG");

				// Token: 0x02003196 RID: 12694
				public class BABY
				{
					// Token: 0x0400C682 RID: 50818
					public static LocString NAME = UI.FormatAsLink("Shine Nymph", "LIGHTBUG");

					// Token: 0x0400C683 RID: 50819
					public static LocString DESC = "A chubby baby " + UI.FormatAsLink("Shine Bug", "LIGHTBUG") + ".";
				}

				// Token: 0x02003197 RID: 12695
				public class VARIANT_ORANGE
				{
					// Token: 0x0400C684 RID: 50820
					public static LocString NAME = UI.FormatAsLink("Sun Bug", "LIGHTBUGORANGE");

					// Token: 0x0400C685 RID: 50821
					public static LocString DESC = "Shine Bugs emit a soft " + UI.FormatAsLink("Light", "LIGHT") + " in hopes of attracting more of their kind for company.\n\nThe light of the Sun morph has been turned orange through selective breeding.";

					// Token: 0x0400C686 RID: 50822
					public static LocString EGG_NAME = UI.FormatAsLink("Sun Nymph Egg", "LIGHTBUGORANGE");

					// Token: 0x02003429 RID: 13353
					public class BABY
					{
						// Token: 0x0400CCE1 RID: 52449
						public static LocString NAME = UI.FormatAsLink("Sun Nymph", "LIGHTBUGORANGE");

						// Token: 0x0400CCE2 RID: 52450
						public static LocString DESC = "A chubby baby " + UI.FormatAsLink("Shine Bug", "LIGHTBUGORANGE") + ".\n\nThis one is a Sun morph.";
					}
				}

				// Token: 0x02003198 RID: 12696
				public class VARIANT_PURPLE
				{
					// Token: 0x0400C687 RID: 50823
					public static LocString NAME = UI.FormatAsLink("Royal Bug", "LIGHTBUGPURPLE");

					// Token: 0x0400C688 RID: 50824
					public static LocString DESC = "Shine Bugs emit a soft " + UI.FormatAsLink("Light", "LIGHT") + " in hopes of attracting more of their kind for company.\n\nThe light of the Royal morph has been turned purple through selective breeding.";

					// Token: 0x0400C689 RID: 50825
					public static LocString EGG_NAME = UI.FormatAsLink("Royal Nymph Egg", "LIGHTBUGPURPLE");

					// Token: 0x0200342A RID: 13354
					public class BABY
					{
						// Token: 0x0400CCE3 RID: 52451
						public static LocString NAME = UI.FormatAsLink("Royal Nymph", "LIGHTBUGPURPLE");

						// Token: 0x0400CCE4 RID: 52452
						public static LocString DESC = "A chubby baby " + UI.FormatAsLink("Shine Bug", "LIGHTBUGPURPLE") + ".\n\nThis one is a Royal morph.";
					}
				}

				// Token: 0x02003199 RID: 12697
				public class VARIANT_PINK
				{
					// Token: 0x0400C68A RID: 50826
					public static LocString NAME = UI.FormatAsLink("Coral Bug", "LIGHTBUGPINK");

					// Token: 0x0400C68B RID: 50827
					public static LocString DESC = "Shine Bugs emit a soft " + UI.FormatAsLink("Light", "LIGHT") + " in hopes of attracting more of their kind for company.\n\nThe light of the Coral morph has been turned pink through selective breeding.";

					// Token: 0x0400C68C RID: 50828
					public static LocString EGG_NAME = UI.FormatAsLink("Coral Nymph Egg", "LIGHTBUGPINK");

					// Token: 0x0200342B RID: 13355
					public class BABY
					{
						// Token: 0x0400CCE5 RID: 52453
						public static LocString NAME = UI.FormatAsLink("Coral Nymph", "LIGHTBUGPINK");

						// Token: 0x0400CCE6 RID: 52454
						public static LocString DESC = "A chubby baby " + UI.FormatAsLink("Shine Bug", "LIGHTBUGPINK") + ".\n\nThis one is a Coral morph.";
					}
				}

				// Token: 0x0200319A RID: 12698
				public class VARIANT_BLUE
				{
					// Token: 0x0400C68D RID: 50829
					public static LocString NAME = UI.FormatAsLink("Azure Bug", "LIGHTBUGBLUE");

					// Token: 0x0400C68E RID: 50830
					public static LocString DESC = "Shine Bugs emit a soft " + UI.FormatAsLink("Light", "LIGHT") + " in hopes of attracting more of their kind for company.\n\nThe light of the Azure morph has been turned blue through selective breeding.";

					// Token: 0x0400C68F RID: 50831
					public static LocString EGG_NAME = UI.FormatAsLink("Azure Nymph Egg", "LIGHTBUGBLUE");

					// Token: 0x0200342C RID: 13356
					public class BABY
					{
						// Token: 0x0400CCE7 RID: 52455
						public static LocString NAME = UI.FormatAsLink("Azure Nymph", "LIGHTBUGBLUE");

						// Token: 0x0400CCE8 RID: 52456
						public static LocString DESC = "A chubby baby " + UI.FormatAsLink("Shine Bug", "LIGHTBUGBLUE") + ".\n\nThis one is an Azure morph.";
					}
				}

				// Token: 0x0200319B RID: 12699
				public class VARIANT_BLACK
				{
					// Token: 0x0400C690 RID: 50832
					public static LocString NAME = UI.FormatAsLink("Abyss Bug", "LIGHTBUGBLACK");

					// Token: 0x0400C691 RID: 50833
					public static LocString DESC = "This Shine Bug emits no " + UI.FormatAsLink("Light", "LIGHT") + ", but it makes up for it by having an excellent personality.";

					// Token: 0x0400C692 RID: 50834
					public static LocString EGG_NAME = UI.FormatAsLink("Abyss Nymph Egg", "LIGHTBUGBLACK");

					// Token: 0x0200342D RID: 13357
					public class BABY
					{
						// Token: 0x0400CCE9 RID: 52457
						public static LocString NAME = UI.FormatAsLink("Abyss Nymph", "LIGHTBUGBLACK");

						// Token: 0x0400CCEA RID: 52458
						public static LocString DESC = "A chubby baby " + UI.FormatAsLink("Shine Bug", "LIGHTBUGBLACK") + ".\n\nThis one is an Abyss morph.";
					}
				}

				// Token: 0x0200319C RID: 12700
				public class VARIANT_CRYSTAL
				{
					// Token: 0x0400C693 RID: 50835
					public static LocString NAME = UI.FormatAsLink("Radiant Bug", "LIGHTBUGCRYSTAL");

					// Token: 0x0400C694 RID: 50836
					public static LocString DESC = "Shine Bugs emit a soft " + UI.FormatAsLink("Light", "LIGHT") + " in hopes of attracting more of their kind for company.\n\nThe light of the Radiant morph has been amplified through selective breeding.";

					// Token: 0x0400C695 RID: 50837
					public static LocString EGG_NAME = UI.FormatAsLink("Radiant Nymph Egg", "LIGHTBUGCRYSTAL");

					// Token: 0x0200342E RID: 13358
					public class BABY
					{
						// Token: 0x0400CCEB RID: 52459
						public static LocString NAME = UI.FormatAsLink("Radiant Nymph", "LIGHTBUGCRYSTAL");

						// Token: 0x0400CCEC RID: 52460
						public static LocString DESC = "A chubby baby " + UI.FormatAsLink("Shine Bug", "LIGHTBUGCRYSTAL") + ".\n\nThis one is a Radiant morph.";
					}
				}

				// Token: 0x0200319D RID: 12701
				public class VARIANT_RADIOACTIVE
				{
					// Token: 0x0400C696 RID: 50838
					public static LocString NAME = UI.FormatAsLink("Ionizing Bug", "LIGHTBUGRADIOACTIVE");

					// Token: 0x0400C697 RID: 50839
					public static LocString DESC = "Shine Bugs emit a dangerously radioactive " + UI.FormatAsLink("Light", "LIGHT") + " in hopes of attracting more of their kind for company.";

					// Token: 0x0400C698 RID: 50840
					public static LocString EGG_NAME = UI.FormatAsLink("Ionizing Nymph Egg", "LIGHTBUGCRYSTAL");

					// Token: 0x0200342F RID: 13359
					public class BABY
					{
						// Token: 0x0400CCED RID: 52461
						public static LocString NAME = UI.FormatAsLink("Ionizing Nymph", "LIGHTBUGRADIOACTIVE");

						// Token: 0x0400CCEE RID: 52462
						public static LocString DESC = "A chubby baby " + UI.FormatAsLink("Shine Bug", "LIGHTBUGRADIOACTIVE") + ".\n\nThis one is an Ionizing morph.";
					}
				}
			}

			// Token: 0x02002943 RID: 10563
			public class GEYSER
			{
				// Token: 0x0400B1DC RID: 45532
				public static LocString NAME = UI.FormatAsLink("Steam Geyser", "GEYSER");

				// Token: 0x0400B1DD RID: 45533
				public static LocString DESC = string.Concat(new string[]
				{
					"A highly pressurized geyser that periodically erupts, spraying ",
					UI.FormatAsLink("Steam", "STEAM"),
					" and boiling hot ",
					UI.FormatAsLink("Water", "WATER"),
					"."
				});

				// Token: 0x0200319E RID: 12702
				public class STEAM
				{
					// Token: 0x0400C699 RID: 50841
					public static LocString NAME = UI.FormatAsLink("Cool Steam Vent", "GeyserGeneric_STEAM");

					// Token: 0x0400C69A RID: 50842
					public static LocString DESC = "A highly pressurized vent that periodically erupts with " + UI.FormatAsLink("Steam", "STEAM") + ".";
				}

				// Token: 0x0200319F RID: 12703
				public class HOT_STEAM
				{
					// Token: 0x0400C69B RID: 50843
					public static LocString NAME = UI.FormatAsLink("Steam Vent", "GeyserGeneric_HOT_STEAM");

					// Token: 0x0400C69C RID: 50844
					public static LocString DESC = "A highly pressurized vent that periodically erupts with scalding " + UI.FormatAsLink("Steam", "STEAM") + ".";
				}

				// Token: 0x020031A0 RID: 12704
				public class SALT_WATER
				{
					// Token: 0x0400C69D RID: 50845
					public static LocString NAME = UI.FormatAsLink("Salt Water Geyser", "GeyserGeneric_SALT_WATER");

					// Token: 0x0400C69E RID: 50846
					public static LocString DESC = "A highly pressurized geyser that periodically erupts with " + UI.FormatAsLink("Salt Water", "SALTWATER") + ".";
				}

				// Token: 0x020031A1 RID: 12705
				public class SLUSH_SALT_WATER
				{
					// Token: 0x0400C69F RID: 50847
					public static LocString NAME = UI.FormatAsLink("Cool Salt Slush Geyser", "GeyserGeneric_SLUSH_SALT_WATER");

					// Token: 0x0400C6A0 RID: 50848
					public static LocString DESC = "A highly pressurized geyser that periodically erupts with freezing " + ELEMENTS.BRINE.NAME + ".";
				}

				// Token: 0x020031A2 RID: 12706
				public class HOT_WATER
				{
					// Token: 0x0400C6A1 RID: 50849
					public static LocString NAME = UI.FormatAsLink("Water Geyser", "GeyserGeneric_HOT_WATER");

					// Token: 0x0400C6A2 RID: 50850
					public static LocString DESC = "A highly pressurized geyser that periodically erupts with hot " + UI.FormatAsLink("Water", "WATER") + ".";
				}

				// Token: 0x020031A3 RID: 12707
				public class SLUSH_WATER
				{
					// Token: 0x0400C6A3 RID: 50851
					public static LocString NAME = UI.FormatAsLink("Cool Slush Geyser", "GeyserGeneric_SLUSHWATER");

					// Token: 0x0400C6A4 RID: 50852
					public static LocString DESC = "A highly pressurized geyser that periodically erupts with freezing " + ELEMENTS.DIRTYWATER.NAME + ".";
				}

				// Token: 0x020031A4 RID: 12708
				public class FILTHY_WATER
				{
					// Token: 0x0400C6A5 RID: 50853
					public static LocString NAME = UI.FormatAsLink("Polluted Water Vent", "GeyserGeneric_FILTHYWATER");

					// Token: 0x0400C6A6 RID: 50854
					public static LocString DESC = "A highly pressurized vent that periodically erupts with boiling " + UI.FormatAsLink("Contaminated Water", "DIRTYWATER") + ".";
				}

				// Token: 0x020031A5 RID: 12709
				public class SMALL_VOLCANO
				{
					// Token: 0x0400C6A7 RID: 50855
					public static LocString NAME = UI.FormatAsLink("Minor Volcano", "GeyserGeneric_SMALL_VOLCANO");

					// Token: 0x0400C6A8 RID: 50856
					public static LocString DESC = "A miniature volcano that periodically erupts with molten " + UI.FormatAsLink("Magma", "MAGMA") + ".";
				}

				// Token: 0x020031A6 RID: 12710
				public class BIG_VOLCANO
				{
					// Token: 0x0400C6A9 RID: 50857
					public static LocString NAME = UI.FormatAsLink("Volcano", "GeyserGeneric_BIG_VOLCANO");

					// Token: 0x0400C6AA RID: 50858
					public static LocString DESC = "A massive volcano that periodically erupts with molten " + UI.FormatAsLink("Magma", "MAGMA") + ".";
				}

				// Token: 0x020031A7 RID: 12711
				public class LIQUID_CO2
				{
					// Token: 0x0400C6AB RID: 50859
					public static LocString NAME = UI.FormatAsLink("Carbon Dioxide Geyser", "GeyserGeneric_LIQUID_CO2");

					// Token: 0x0400C6AC RID: 50860
					public static LocString DESC = "A highly pressurized geyser that periodically erupts with boiling " + UI.FormatAsLink("Liquid Carbon Dioxide", "LIQUIDCARBONDIOXIDE") + ".";
				}

				// Token: 0x020031A8 RID: 12712
				public class HOT_CO2
				{
					// Token: 0x0400C6AD RID: 50861
					public static LocString NAME = UI.FormatAsLink("Carbon Dioxide Vent", "GeyserGeneric_HOT_CO2");

					// Token: 0x0400C6AE RID: 50862
					public static LocString DESC = "A highly pressurized vent that periodically erupts with hot " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE") + ".";
				}

				// Token: 0x020031A9 RID: 12713
				public class HOT_HYDROGEN
				{
					// Token: 0x0400C6AF RID: 50863
					public static LocString NAME = UI.FormatAsLink("Hydrogen Vent", "GeyserGeneric_HOT_HYDROGEN");

					// Token: 0x0400C6B0 RID: 50864
					public static LocString DESC = "A highly pressurized vent that periodically erupts with hot gaseous " + UI.FormatAsLink("Hydrogen", "HYDROGEN") + ".";
				}

				// Token: 0x020031AA RID: 12714
				public class HOT_PO2
				{
					// Token: 0x0400C6B1 RID: 50865
					public static LocString NAME = UI.FormatAsLink("Hot Polluted Oxygen Vent", "GeyserGeneric_HOT_PO2");

					// Token: 0x0400C6B2 RID: 50866
					public static LocString DESC = "A highly pressurized vent that periodically erupts with hot " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + ".";
				}

				// Token: 0x020031AB RID: 12715
				public class SLIMY_PO2
				{
					// Token: 0x0400C6B3 RID: 50867
					public static LocString NAME = UI.FormatAsLink("Infectious Polluted Oxygen Vent", "GeyserGeneric_SLIMY_PO2");

					// Token: 0x0400C6B4 RID: 50868
					public static LocString DESC = "A highly pressurized vent that periodically erupts with warm " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + ".";
				}

				// Token: 0x020031AC RID: 12716
				public class CHLORINE_GAS
				{
					// Token: 0x0400C6B5 RID: 50869
					public static LocString NAME = UI.FormatAsLink("Chlorine Gas Vent", "GeyserGeneric_CHLORINE_GAS");

					// Token: 0x0400C6B6 RID: 50870
					public static LocString DESC = "A highly pressurized vent that periodically erupts with warm " + UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS") + ".";
				}

				// Token: 0x020031AD RID: 12717
				public class METHANE
				{
					// Token: 0x0400C6B7 RID: 50871
					public static LocString NAME = UI.FormatAsLink("Natural Gas Geyser", "GeyserGeneric_METHANE");

					// Token: 0x0400C6B8 RID: 50872
					public static LocString DESC = "A highly pressurized geyser that periodically erupts with hot " + UI.FormatAsLink("Natural Gas", "METHANE") + ".";
				}

				// Token: 0x020031AE RID: 12718
				public class MOLTEN_COPPER
				{
					// Token: 0x0400C6B9 RID: 50873
					public static LocString NAME = UI.FormatAsLink("Copper Volcano", "GeyserGeneric_MOLTEN_COPPER");

					// Token: 0x0400C6BA RID: 50874
					public static LocString DESC = "A large volcano that periodically erupts with " + UI.FormatAsLink("Molten Copper", "MOLTENCOPPER") + ".";
				}

				// Token: 0x020031AF RID: 12719
				public class MOLTEN_IRON
				{
					// Token: 0x0400C6BB RID: 50875
					public static LocString NAME = UI.FormatAsLink("Iron Volcano", "GeyserGeneric_MOLTEN_IRON");

					// Token: 0x0400C6BC RID: 50876
					public static LocString DESC = "A large volcano that periodically erupts with " + UI.FormatAsLink("Molten Iron", "MOLTENIRON") + ".";
				}

				// Token: 0x020031B0 RID: 12720
				public class MOLTEN_ALUMINUM
				{
					// Token: 0x0400C6BD RID: 50877
					public static LocString NAME = UI.FormatAsLink("Aluminum Volcano", "GeyserGeneric_MOLTEN_ALUMINUM");

					// Token: 0x0400C6BE RID: 50878
					public static LocString DESC = "A large volcano that periodically erupts with " + UI.FormatAsLink("Molten Aluminum", "MOLTENALUMINUM") + ".";
				}

				// Token: 0x020031B1 RID: 12721
				public class MOLTEN_TUNGSTEN
				{
					// Token: 0x0400C6BF RID: 50879
					public static LocString NAME = UI.FormatAsLink("Tungsten Volcano", "GeyserGeneric_MOLTEN_TUNGSTEN");

					// Token: 0x0400C6C0 RID: 50880
					public static LocString DESC = "A large volcano that periodically erupts with " + UI.FormatAsLink("Molten Tungsten", "MOLTENTUNGSTEN") + ".";
				}

				// Token: 0x020031B2 RID: 12722
				public class MOLTEN_GOLD
				{
					// Token: 0x0400C6C1 RID: 50881
					public static LocString NAME = UI.FormatAsLink("Gold Volcano", "GeyserGeneric_MOLTEN_GOLD");

					// Token: 0x0400C6C2 RID: 50882
					public static LocString DESC = "A large volcano that periodically erupts with " + UI.FormatAsLink("Molten Gold", "MOLTENGOLD") + ".";
				}

				// Token: 0x020031B3 RID: 12723
				public class MOLTEN_COBALT
				{
					// Token: 0x0400C6C3 RID: 50883
					public static LocString NAME = UI.FormatAsLink("Cobalt Volcano", "GeyserGeneric_MOLTEN_COBALT");

					// Token: 0x0400C6C4 RID: 50884
					public static LocString DESC = "A large volcano that periodically erupts with " + UI.FormatAsLink("Molten Cobalt", "MOLTENCOBALT") + ".";
				}

				// Token: 0x020031B4 RID: 12724
				public class MOLTEN_NIOBIUM
				{
					// Token: 0x0400C6C5 RID: 50885
					public static LocString NAME = UI.FormatAsLink("Niobium Volcano", "NiobiumGeyser");

					// Token: 0x0400C6C6 RID: 50886
					public static LocString DESC = "A large volcano that periodically erupts with " + UI.FormatAsLink("Niobium", "NIOBIUM") + ".";
				}

				// Token: 0x020031B5 RID: 12725
				public class OIL_DRIP
				{
					// Token: 0x0400C6C7 RID: 50887
					public static LocString NAME = UI.FormatAsLink("Leaky Oil Fissure", "GeyserGeneric_OIL_DRIP");

					// Token: 0x0400C6C8 RID: 50888
					public static LocString DESC = "A fissure that periodically erupts with boiling " + UI.FormatAsLink("Crude Oil", "CRUDEOIL") + ".";
				}

				// Token: 0x020031B6 RID: 12726
				public class LIQUID_SULFUR
				{
					// Token: 0x0400C6C9 RID: 50889
					public static LocString NAME = UI.FormatAsLink("Liquid Sulfur Geyser", "GeyserGeneric_LIQUID_SULFUR");

					// Token: 0x0400C6CA RID: 50890
					public static LocString DESC = "A highly pressurized geyser that periodically erupts with boiling " + UI.FormatAsLink("Liquid Sulfur", "LIQUIDSULFUR") + ".";
				}
			}

			// Token: 0x02002944 RID: 10564
			public class METHANEGEYSER
			{
				// Token: 0x0400B1DE RID: 45534
				public static LocString NAME = UI.FormatAsLink("Natural Gas Geyser", "GeyserGeneric_METHANEGEYSER");

				// Token: 0x0400B1DF RID: 45535
				public static LocString DESC = "A highly pressurized geyser that periodically erupts with " + UI.FormatAsLink("Natural Gas", "METHANE") + ".";
			}

			// Token: 0x02002945 RID: 10565
			public class OIL_WELL
			{
				// Token: 0x0400B1E0 RID: 45536
				public static LocString NAME = UI.FormatAsLink("Oil Reservoir", "OIL_WELL");

				// Token: 0x0400B1E1 RID: 45537
				public static LocString DESC = "Oil Reservoirs are rock formations with " + UI.FormatAsLink("Crude Oil", "CRUDEOIL") + " deposits beneath their surface.\n\nOil can be extracted from a reservoir with sufficient pressure.";
			}

			// Token: 0x02002946 RID: 10566
			public class MUSHROOMPLANT
			{
				// Token: 0x0400B1E2 RID: 45538
				public static LocString NAME = UI.FormatAsLink("Dusk Cap", "MUSHROOMPLANT");

				// Token: 0x0400B1E3 RID: 45539
				public static LocString DESC = string.Concat(new string[]
				{
					"Dusk Caps produce ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					", fungal growths that can be harvested for ",
					UI.FormatAsLink("Food", "FOOD"),
					"."
				});

				// Token: 0x0400B1E4 RID: 45540
				public static LocString DOMESTICATEDDESC = "This plant produces edible " + UI.FormatAsLink("Mushrooms", "MUSHROOM") + ".";
			}

			// Token: 0x02002947 RID: 10567
			public class STEAMSPOUT
			{
				// Token: 0x0400B1E5 RID: 45541
				public static LocString NAME = UI.FormatAsLink("Steam Spout", "GEYSERS");

				// Token: 0x0400B1E6 RID: 45542
				public static LocString DESC = "A rocky vent that spouts " + UI.FormatAsLink("Steam", "STEAM") + ".";
			}

			// Token: 0x02002948 RID: 10568
			public class PROPANESPOUT
			{
				// Token: 0x0400B1E7 RID: 45543
				public static LocString NAME = UI.FormatAsLink("Propane Spout", "GEYSERS");

				// Token: 0x0400B1E8 RID: 45544
				public static LocString DESC = "A rocky vent that spouts " + ELEMENTS.PROPANE.NAME + ".";
			}

			// Token: 0x02002949 RID: 10569
			public class OILSPOUT
			{
				// Token: 0x0400B1E9 RID: 45545
				public static LocString NAME = UI.FormatAsLink("Oil Spout", "OILSPOUT");

				// Token: 0x0400B1EA RID: 45546
				public static LocString DESC = "A rocky vent that spouts " + UI.FormatAsLink("Crude Oil", "CRUDEOIL") + ".";
			}

			// Token: 0x0200294A RID: 10570
			public class HEATBULB
			{
				// Token: 0x0400B1EB RID: 45547
				public static LocString NAME = UI.FormatAsLink("Fervine", "HEATBULB");

				// Token: 0x0400B1EC RID: 45548
				public static LocString DESC = "A temperature reactive, subterranean " + UI.FormatAsLink("Plant", "PLANTS") + ".";
			}

			// Token: 0x0200294B RID: 10571
			public class HEATBULBSEED
			{
				// Token: 0x0400B1ED RID: 45549
				public static LocString NAME = UI.FormatAsLink("Fervine Bulb", "HEATBULBSEED");

				// Token: 0x0400B1EE RID: 45550
				public static LocString DESC = "A temperature reactive, subterranean " + UI.FormatAsLink("Plant", "PLANTS") + ".";
			}

			// Token: 0x0200294C RID: 10572
			public class PACUEGG
			{
				// Token: 0x0400B1EF RID: 45551
				public static LocString NAME = UI.FormatAsLink("Pacu Egg", "PACUEGG");

				// Token: 0x0400B1F0 RID: 45552
				public static LocString DESC = "A tiny Pacu is nestled inside.\n\nIt is not yet ready for the world.";
			}

			// Token: 0x0200294D RID: 10573
			public class MYSTERYEGG
			{
				// Token: 0x0400B1F1 RID: 45553
				public static LocString NAME = UI.FormatAsLink("Mysterious Egg", "MYSTERYEGG");

				// Token: 0x0400B1F2 RID: 45554
				public static LocString DESC = "What's growing inside? Something nice? Something mean?";
			}

			// Token: 0x0200294E RID: 10574
			public class SWAMPLILY
			{
				// Token: 0x0400B1F3 RID: 45555
				public static LocString NAME = UI.FormatAsLink("Balm Lily", "SWAMPLILY");

				// Token: 0x0400B1F4 RID: 45556
				public static LocString DESC = "Balm Lilies produce " + ITEMS.INGREDIENTS.SWAMPLILYFLOWER.NAME + ", a lovely bloom with medicinal properties.";

				// Token: 0x0400B1F5 RID: 45557
				public static LocString DOMESTICATEDDESC = "This plant produces medicinal " + ITEMS.INGREDIENTS.SWAMPLILYFLOWER.NAME + ".";
			}

			// Token: 0x0200294F RID: 10575
			public class JUNGLEGASPLANT
			{
				// Token: 0x0400B1F6 RID: 45558
				public static LocString NAME = UI.FormatAsLink("Palmera Tree", "JUNGLEGASPLANT");

				// Token: 0x0400B1F7 RID: 45559
				public static LocString DESC = "A large, chlorine-dwelling " + UI.FormatAsLink("Plant", "PLANTS") + " that can be grown in farm buildings.\n\nPalmeras grow inedible buds that emit unbreathable hydrogen gas.";

				// Token: 0x0400B1F8 RID: 45560
				public static LocString DOMESTICATEDDESC = "A large, chlorine-dwelling " + UI.FormatAsLink("Plant", "PLANTS") + " that grows inedible buds which emit unbreathable hydrogen gas.";
			}

			// Token: 0x02002950 RID: 10576
			public class PRICKLEFLOWER
			{
				// Token: 0x0400B1F9 RID: 45561
				public static LocString NAME = UI.FormatAsLink("Bristle Blossom", "PRICKLEFLOWER");

				// Token: 0x0400B1FA RID: 45562
				public static LocString DESC = "Bristle Blossoms produce " + ITEMS.FOOD.PRICKLEFRUIT.NAME + ", a prickly edible bud.";

				// Token: 0x0400B1FB RID: 45563
				public static LocString DOMESTICATEDDESC = "This plant produces edible " + UI.FormatAsLink("Bristle Berries", UI.StripLinkFormatting(ITEMS.FOOD.PRICKLEFRUIT.NAME)) + ".";
			}

			// Token: 0x02002951 RID: 10577
			public class COLDWHEAT
			{
				// Token: 0x0400B1FC RID: 45564
				public static LocString NAME = UI.FormatAsLink("Sleet Wheat", "COLDWHEAT");

				// Token: 0x0400B1FD RID: 45565
				public static LocString DESC = string.Concat(new string[]
				{
					"Sleet Wheat produces ",
					ITEMS.FOOD.COLDWHEATSEED.NAME,
					", a chilly grain that can be processed into ",
					UI.FormatAsLink("Food", "FOOD"),
					"."
				});

				// Token: 0x0400B1FE RID: 45566
				public static LocString DOMESTICATEDDESC = "This plant produces edible " + ITEMS.FOOD.COLDWHEATSEED.NAME + ".";
			}

			// Token: 0x02002952 RID: 10578
			public class GASGRASS
			{
				// Token: 0x0400B1FF RID: 45567
				public static LocString NAME = UI.FormatAsLink("Gas Grass", "GASGRASS");

				// Token: 0x0400B200 RID: 45568
				public static LocString DESC = "Gas Grass is an alien lifeform tentatively categorized as a \"plant,\" which makes up the entirety of the " + UI.FormatAsLink("Gassy Moo's", "MOO") + " diet.";

				// Token: 0x0400B201 RID: 45569
				public static LocString DOMESTICATEDDESC = "An alien grass variety that is eaten by " + UI.FormatAsLink("Gassy Moos", "MOO") + ".";
			}

			// Token: 0x02002953 RID: 10579
			public class PRICKLEGRASS
			{
				// Token: 0x0400B202 RID: 45570
				public static LocString NAME = UI.FormatAsLink("Bluff Briar", "PRICKLEGRASS");

				// Token: 0x0400B203 RID: 45571
				public static LocString DESC = "Bluff Briars exude pheromones causing critters to view them as especially beautiful.";

				// Token: 0x0400B204 RID: 45572
				public static LocString DOMESTICATEDDESC = "This plant improves " + UI.FormatAsLink("Decor", "DECOR") + ".";

				// Token: 0x0400B205 RID: 45573
				public static LocString GROWTH_BONUS = "Growth Bonus";

				// Token: 0x0400B206 RID: 45574
				public static LocString WILT_PENALTY = "Wilt Penalty";
			}

			// Token: 0x02002954 RID: 10580
			public class CYLINDRICA
			{
				// Token: 0x0400B207 RID: 45575
				public static LocString NAME = UI.FormatAsLink("Bliss Burst", "CYLINDRICA");

				// Token: 0x0400B208 RID: 45576
				public static LocString DESC = "Bliss Bursts release an explosion of " + UI.FormatAsLink("Decor", "DECOR") + " into otherwise dull environments.";

				// Token: 0x0400B209 RID: 45577
				public static LocString DOMESTICATEDDESC = "This plant improves ambient " + UI.FormatAsLink("Decor", "DECOR") + ".";

				// Token: 0x0400B20A RID: 45578
				public static LocString GROWTH_BONUS = "Growth Bonus";

				// Token: 0x0400B20B RID: 45579
				public static LocString WILT_PENALTY = "Wilt Penalty";
			}

			// Token: 0x02002955 RID: 10581
			public class TOEPLANT
			{
				// Token: 0x0400B20C RID: 45580
				public static LocString NAME = UI.FormatAsLink("Tranquil Toes", "TOEPLANT");

				// Token: 0x0400B20D RID: 45581
				public static LocString DESC = "Tranquil Toes improve " + UI.FormatAsLink("Decor", "DECOR") + " by giving their surroundings the visual equivalent of a foot rub.";

				// Token: 0x0400B20E RID: 45582
				public static LocString DOMESTICATEDDESC = "This plant improves ambient " + UI.FormatAsLink("Decor", "DECOR") + ".";

				// Token: 0x0400B20F RID: 45583
				public static LocString GROWTH_BONUS = "Growth Bonus";

				// Token: 0x0400B210 RID: 45584
				public static LocString WILT_PENALTY = "Wilt Penalty";
			}

			// Token: 0x02002956 RID: 10582
			public class WINECUPS
			{
				// Token: 0x0400B211 RID: 45585
				public static LocString NAME = UI.FormatAsLink("Mellow Mallow", "WINECUPS");

				// Token: 0x0400B212 RID: 45586
				public static LocString DESC = string.Concat(new string[]
				{
					"Mellow Mallows heighten ",
					UI.FormatAsLink("Decor", "DECOR"),
					" and alleviate ",
					UI.FormatAsLink("Stress", "STRESS"),
					" with their calming color and cradle shape."
				});

				// Token: 0x0400B213 RID: 45587
				public static LocString DOMESTICATEDDESC = "This plant improves ambient " + UI.FormatAsLink("Decor", "DECOR") + ".";

				// Token: 0x0400B214 RID: 45588
				public static LocString GROWTH_BONUS = "Growth Bonus";

				// Token: 0x0400B215 RID: 45589
				public static LocString WILT_PENALTY = "Wilt Penalty";
			}

			// Token: 0x02002957 RID: 10583
			public class EVILFLOWER
			{
				// Token: 0x0400B216 RID: 45590
				public static LocString NAME = UI.FormatAsLink("Sporechid", "EVILFLOWER");

				// Token: 0x0400B217 RID: 45591
				public static LocString DESC = "Sporechids have an eerily alluring appearance to mask the fact that they host particularly nasty strain of brain fungus.";

				// Token: 0x0400B218 RID: 45592
				public static LocString DOMESTICATEDDESC = string.Concat(new string[]
				{
					"This plant improves ambient ",
					UI.FormatAsLink("Decor", "DECOR"),
					" but produces high quantities of ",
					UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
					"."
				});

				// Token: 0x0400B219 RID: 45593
				public static LocString GROWTH_BONUS = "Growth Bonus";

				// Token: 0x0400B21A RID: 45594
				public static LocString WILT_PENALTY = "Wilt Penalty";
			}

			// Token: 0x02002958 RID: 10584
			public class LEAFYPLANT
			{
				// Token: 0x0400B21B RID: 45595
				public static LocString NAME = UI.FormatAsLink("Mirth Leaf", "POTTED_LEAFY");

				// Token: 0x0400B21C RID: 45596
				public static LocString DESC = string.Concat(new string[]
				{
					"Mirth Leaves sport a calm green hue known for alleviating ",
					UI.FormatAsLink("Stress", "STRESS"),
					" and improving ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x0400B21D RID: 45597
				public static LocString DOMESTICATEDDESC = "This plant improves ambient " + UI.FormatAsLink("Decor", "DECOR") + ".";

				// Token: 0x0400B21E RID: 45598
				public static LocString GROWTH_BONUS = "Growth Bonus";

				// Token: 0x0400B21F RID: 45599
				public static LocString WILT_PENALTY = "Wilt Penalty";
			}

			// Token: 0x02002959 RID: 10585
			public class CACTUSPLANT
			{
				// Token: 0x0400B220 RID: 45600
				public static LocString NAME = UI.FormatAsLink("Jumping Joya", "POTTED_CACTUS");

				// Token: 0x0400B221 RID: 45601
				public static LocString DESC = string.Concat(new string[]
				{
					"Joyas are ",
					UI.FormatAsLink("Decorative", "DECOR"),
					" ",
					UI.FormatAsLink("Plants", "PLANTS"),
					" that are colloquially said to make gardeners \"jump for joy\"."
				});

				// Token: 0x0400B222 RID: 45602
				public static LocString DOMESTICATEDDESC = "This plant improves ambient " + UI.FormatAsLink("Decor", "DECOR") + ".";

				// Token: 0x0400B223 RID: 45603
				public static LocString GROWTH_BONUS = "Growth Bonus";

				// Token: 0x0400B224 RID: 45604
				public static LocString WILT_PENALTY = "Wilt Penalty";
			}

			// Token: 0x0200295A RID: 10586
			public class BULBPLANT
			{
				// Token: 0x0400B225 RID: 45605
				public static LocString NAME = UI.FormatAsLink("Buddy Bud", "POTTED_BULB");

				// Token: 0x0400B226 RID: 45606
				public static LocString DESC = "Buddy Buds are leafy plants that have a positive effect on " + UI.FormatAsLink("Morale", "MORALE") + ", much like a friend.";

				// Token: 0x0400B227 RID: 45607
				public static LocString DOMESTICATEDDESC = "This plant improves ambient " + UI.FormatAsLink("Decor", "DECOR") + ".";

				// Token: 0x0400B228 RID: 45608
				public static LocString GROWTH_BONUS = "Growth Bonus";

				// Token: 0x0400B229 RID: 45609
				public static LocString WILT_PENALTY = "Wilt Penalty";
			}

			// Token: 0x0200295B RID: 10587
			public class BASICSINGLEHARVESTPLANT
			{
				// Token: 0x0400B22A RID: 45610
				public static LocString NAME = UI.FormatAsLink("Mealwood", "BASICSINGLEHARVESTPLANT");

				// Token: 0x0400B22B RID: 45611
				public static LocString DESC = string.Concat(new string[]
				{
					"Mealwoods produce ",
					ITEMS.FOOD.BASICPLANTFOOD.NAME,
					", an oddly wriggly grain that can be harvested for ",
					UI.FormatAsLink("Food", "FOOD"),
					"."
				});

				// Token: 0x0400B22C RID: 45612
				public static LocString DOMESTICATEDDESC = "This plant produces edible " + ITEMS.FOOD.BASICPLANTFOOD.NAME + ".";
			}

			// Token: 0x0200295C RID: 10588
			public class SWAMPHARVESTPLANT
			{
				// Token: 0x0400B22D RID: 45613
				public static LocString NAME = UI.FormatAsLink("Bog Bucket", "SWAMPHARVESTPLANT");

				// Token: 0x0400B22E RID: 45614
				public static LocString DESC = string.Concat(new string[]
				{
					"Bog Buckets produce juicy, sweet ",
					UI.FormatAsLink("Bog Jellies", "SWAMPFRUIT"),
					" for ",
					UI.FormatAsLink("Food", "FOOD"),
					"."
				});

				// Token: 0x0400B22F RID: 45615
				public static LocString DOMESTICATEDDESC = "This plant produces edible " + UI.FormatAsLink("Bog Jellies", "SWAMPFRUIT") + ".";
			}

			// Token: 0x0200295D RID: 10589
			public class WORMPLANT
			{
				// Token: 0x0400B230 RID: 45616
				public static LocString NAME = UI.FormatAsLink("Spindly Grubfruit Plant", "WORMPLANT");

				// Token: 0x0400B231 RID: 45617
				public static LocString DESC = string.Concat(new string[]
				{
					"Spindly Grubfruit Plants produce ",
					UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT"),
					" for ",
					UI.FormatAsLink("Food", "FOOD"),
					".\n\nIf it is tended by a ",
					CREATURES.FAMILY.DIVERGENTSPECIES,
					" critter, it will produce high quality fruits instead."
				});

				// Token: 0x0400B232 RID: 45618
				public static LocString DOMESTICATEDDESC = "This plant produces edible " + ITEMS.FOOD.WORMBASICFRUIT.NAME + ".";
			}

			// Token: 0x0200295E RID: 10590
			public class SUPERWORMPLANT
			{
				// Token: 0x0400B233 RID: 45619
				public static LocString NAME = UI.FormatAsLink("Grubfruit Plant", "WORMPLANT");

				// Token: 0x0400B234 RID: 45620
				public static LocString DESC = string.Concat(new string[]
				{
					"A Grubfruit Plant that has flourished after being tended by a ",
					CREATURES.FAMILY.DIVERGENTSPECIES,
					" critter.\n\nIt will produce high quality ",
					UI.FormatAsLink("Grubfruits", "WORMSUPERFRUIT"),
					"."
				});

				// Token: 0x0400B235 RID: 45621
				public static LocString DOMESTICATEDDESC = "This plant produces edible " + ITEMS.FOOD.WORMSUPERFRUIT.NAME + ".";
			}

			// Token: 0x0200295F RID: 10591
			public class BASICFABRICMATERIALPLANT
			{
				// Token: 0x0400B236 RID: 45622
				public static LocString NAME = UI.FormatAsLink("Thimble Reed", "BASICFABRICPLANT");

				// Token: 0x0400B237 RID: 45623
				public static LocString DESC = string.Concat(new string[]
				{
					"Thimble Reeds produce indescribably soft ",
					ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME,
					" for ",
					UI.FormatAsLink("Clothing", "EQUIPMENT"),
					" production."
				});

				// Token: 0x0400B238 RID: 45624
				public static LocString DOMESTICATEDDESC = "This plant produces " + ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME + ".";
			}

			// Token: 0x02002960 RID: 10592
			public class BASICFORAGEPLANTPLANTED
			{
				// Token: 0x0400B239 RID: 45625
				public static LocString NAME = UI.FormatAsLink("Buried Muckroot", "BASICFORAGEPLANTPLANTED");

				// Token: 0x0400B23A RID: 45626
				public static LocString DESC = "Muckroots are incapable of propagating but can be harvested for a single " + UI.FormatAsLink("Food", "FOOD") + " serving.";
			}

			// Token: 0x02002961 RID: 10593
			public class FORESTFORAGEPLANTPLANTED
			{
				// Token: 0x0400B23B RID: 45627
				public static LocString NAME = UI.FormatAsLink("Hexalent", "FORESTFORAGEPLANTPLANTED");

				// Token: 0x0400B23C RID: 45628
				public static LocString DESC = "Hexalents are incapable of propagating but can be harvested for a single, calorie dense " + UI.FormatAsLink("Food", "FOOD") + " serving.";
			}

			// Token: 0x02002962 RID: 10594
			public class SWAMPFORAGEPLANTPLANTED
			{
				// Token: 0x0400B23D RID: 45629
				public static LocString NAME = UI.FormatAsLink("Swamp Chard", "SWAMPFORAGEPLANTPLANTED");

				// Token: 0x0400B23E RID: 45630
				public static LocString DESC = "Swamp Chards are incapable of propagating but can be harvested for a single low quality and calorie dense " + UI.FormatAsLink("Food", "FOOD") + " serving.";
			}

			// Token: 0x02002963 RID: 10595
			public class COLDBREATHER
			{
				// Token: 0x0400B23F RID: 45631
				public static LocString NAME = UI.FormatAsLink("Wheezewort", "COLDBREATHER");

				// Token: 0x0400B240 RID: 45632
				public static LocString DESC = string.Concat(new string[]
				{
					"Wheezeworts can be planted in ",
					UI.FormatAsLink("Planter Boxes", "PLANTERBOX"),
					", ",
					UI.FormatAsLink("Farm Tiles", "FARMTILE"),
					" or ",
					UI.FormatAsLink("Hydroponic Farms", "HYDROPONICFARM"),
					", and absorb ",
					UI.FormatAsLink("Heat", "Heat"),
					" by respiring through their porous outer membranes."
				});

				// Token: 0x0400B241 RID: 45633
				public static LocString DOMESTICATEDDESC = "This plant absorbs " + UI.FormatAsLink("Heat", "Heat") + ".";
			}

			// Token: 0x02002964 RID: 10596
			public class COLDBREATHERCLUSTER
			{
				// Token: 0x0400B242 RID: 45634
				public static LocString NAME = UI.FormatAsLink("Wheezewort", "COLDBREATHERCLUSTER");

				// Token: 0x0400B243 RID: 45635
				public static LocString DESC = string.Concat(new string[]
				{
					"Wheezeworts can be planted in ",
					UI.FormatAsLink("Planter Boxes", "PLANTERBOX"),
					", ",
					UI.FormatAsLink("Farm Tiles", "FARMTILE"),
					" or ",
					UI.FormatAsLink("Hydroponic Farms", "HYDROPONICFARM"),
					", and absorb ",
					UI.FormatAsLink("Heat", "Heat"),
					" by respiring through their porous outer membranes."
				});

				// Token: 0x0400B244 RID: 45636
				public static LocString DOMESTICATEDDESC = "This plant absorbs " + UI.FormatAsLink("Heat", "Heat") + ".";
			}

			// Token: 0x02002965 RID: 10597
			public class SPICE_VINE
			{
				// Token: 0x0400B245 RID: 45637
				public static LocString NAME = UI.FormatAsLink("Pincha Pepperplant", "SPICE_VINE");

				// Token: 0x0400B246 RID: 45638
				public static LocString DESC = string.Concat(new string[]
				{
					"Pincha Pepperplants produce flavorful ",
					ITEMS.FOOD.SPICENUT.NAME,
					" for spicing ",
					UI.FormatAsLink("Food", "FOOD"),
					"."
				});

				// Token: 0x0400B247 RID: 45639
				public static LocString DOMESTICATEDDESC = "This plant produces " + ITEMS.FOOD.SPICENUT.NAME + " spices.";
			}

			// Token: 0x02002966 RID: 10598
			public class SALTPLANT
			{
				// Token: 0x0400B248 RID: 45640
				public static LocString NAME = UI.FormatAsLink("Dasha Saltvine", "SALTPLANT");

				// Token: 0x0400B249 RID: 45641
				public static LocString DESC = string.Concat(new string[]
				{
					"Dasha Saltvines consume small amounts of ",
					UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
					" and form sodium deposits as they grow, producing harvestable ",
					UI.FormatAsLink("Salt", "SALT"),
					"."
				});

				// Token: 0x0400B24A RID: 45642
				public static LocString DOMESTICATEDDESC = "This plant produces unrefined " + UI.FormatAsLink("Salt", "SALT") + ".";
			}

			// Token: 0x02002967 RID: 10599
			public class FILTERPLANT
			{
				// Token: 0x0400B24B RID: 45643
				public static LocString NAME = UI.FormatAsLink("Hydrocactus", "FILTERPLANT");

				// Token: 0x0400B24C RID: 45644
				public static LocString DESC = string.Concat(new string[]
				{
					"Hydrocacti act as natural ",
					UI.FormatAsLink("Water", "WATER"),
					" filters when given access to ",
					UI.FormatAsLink("Sand", "SAND"),
					"."
				});

				// Token: 0x0400B24D RID: 45645
				public static LocString DOMESTICATEDDESC = string.Concat(new string[]
				{
					"This plant uses ",
					UI.FormatAsLink("Sand", "SAND"),
					" to convert ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" into ",
					UI.FormatAsLink("Water", "WATER"),
					"."
				});
			}

			// Token: 0x02002968 RID: 10600
			public class OXYFERN
			{
				// Token: 0x0400B24E RID: 45646
				public static LocString NAME = UI.FormatAsLink("Oxyfern", "OXYFERN");

				// Token: 0x0400B24F RID: 45647
				public static LocString DESC = string.Concat(new string[]
				{
					"Oxyferns absorb ",
					UI.FormatAsLink("Carbon Dioxide Gas", "CARBONDIOXIDE"),
					" and exude breathable ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					"."
				});

				// Token: 0x0400B250 RID: 45648
				public static LocString DOMESTICATEDDESC = string.Concat(new string[]
				{
					"This plant converts ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					"."
				});
			}

			// Token: 0x02002969 RID: 10601
			public class BEAN_PLANT
			{
				// Token: 0x0400B251 RID: 45649
				public static LocString NAME = UI.FormatAsLink("Nosh Sprout", "BEAN_PLANT");

				// Token: 0x0400B252 RID: 45650
				public static LocString DESC = "Nosh Sprouts thrive in colder climates and produce edible " + UI.FormatAsLink("Nosh Beans", "BEAN") + ".";

				// Token: 0x0400B253 RID: 45651
				public static LocString DOMESTICATEDDESC = "This plant produces " + UI.FormatAsLink("Nosh Beans", "BEAN") + ".";
			}

			// Token: 0x0200296A RID: 10602
			public class WOOD_TREE
			{
				// Token: 0x0400B254 RID: 45652
				public static LocString NAME = UI.FormatAsLink("Arbor Tree", "FOREST_TREE");

				// Token: 0x0400B255 RID: 45653
				public static LocString DESC = "Arbor Trees grow " + UI.FormatAsLink("Arbor Tree Branches", "FOREST_TREE") + " and can be harvested for lumber.";

				// Token: 0x0400B256 RID: 45654
				public static LocString DOMESTICATEDDESC = "This plant produces " + UI.FormatAsLink("Arbor Tree Branches", "FOREST_TREE") + " that can be harvested for lumber.";
			}

			// Token: 0x0200296B RID: 10603
			public class WOOD_TREE_BRANCH
			{
				// Token: 0x0400B257 RID: 45655
				public static LocString NAME = UI.FormatAsLink("Arbor Tree Branch", "FOREST_TREE");

				// Token: 0x0400B258 RID: 45656
				public static LocString DESC = "Arbor Trees Branches can be harvested for lumber.";
			}

			// Token: 0x0200296C RID: 10604
			public class SEALETTUCE
			{
				// Token: 0x0400B259 RID: 45657
				public static LocString NAME = UI.FormatAsLink("Waterweed", "SEALETTUCE");

				// Token: 0x0400B25A RID: 45658
				public static LocString DESC = "Waterweeds thrive in salty water and can be harvested for fresh, edible " + UI.FormatAsLink("Lettuce", "LETTUCE") + ".";

				// Token: 0x0400B25B RID: 45659
				public static LocString DOMESTICATEDDESC = "This plant produces " + UI.FormatAsLink("Lettuce", "LETTUCE") + ".";
			}

			// Token: 0x0200296D RID: 10605
			public class CRITTERTRAPPLANT
			{
				// Token: 0x0400B25C RID: 45660
				public static LocString NAME = UI.FormatAsLink("Saturn Critter Trap", "CRITTERTRAPPLANT");

				// Token: 0x0400B25D RID: 45661
				public static LocString DESC = "Critter Traps are carnivorous plants that trap unsuspecting critters and consume them, releasing " + UI.FormatAsLink("Hydrogen Gas", "HYDROGEN") + " as waste.";

				// Token: 0x0400B25E RID: 45662
				public static LocString DOMESTICATEDDESC = "This plant eats critters and produces " + UI.FormatAsLink("Hydrogen Gas", "HYDROGEN") + ".";
			}

			// Token: 0x0200296E RID: 10606
			public class SAPTREE
			{
				// Token: 0x0400B25F RID: 45663
				public static LocString NAME = UI.FormatAsLink("Experiment 52B", "SAPTREE");

				// Token: 0x0400B260 RID: 45664
				public static LocString DESC = "A " + UI.FormatAsLink("Liquid Resin", "RESIN") + "-producing cybernetic tree that shows signs of sentience.\n\nIt is rooted firmly in place, and is waiting for some brave soul to bring it food.";
			}

			// Token: 0x0200296F RID: 10607
			public class SEEDS
			{
				// Token: 0x020031B7 RID: 12727
				public class LEAFYPLANT
				{
					// Token: 0x0400C6CB RID: 50891
					public static LocString NAME = UI.FormatAsLink("Mirth Leaf Seed", "LEAFYPLANT");

					// Token: 0x0400C6CC RID: 50892
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						UI.FormatAsLink("Mirth Leaf", "LEAFYPLANT"),
						".\n\nDigging up Buried Objects may uncover a Mirth Leaf Seed."
					});
				}

				// Token: 0x020031B8 RID: 12728
				public class CACTUSPLANT
				{
					// Token: 0x0400C6CD RID: 50893
					public static LocString NAME = UI.FormatAsLink("Joya Seed", "CACTUSPLANT");

					// Token: 0x0400C6CE RID: 50894
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						UI.FormatAsLink("Jumping Joya", "CACTUSPLANT"),
						".\n\nDigging up Buried Objects may uncover a Joya Seed."
					});
				}

				// Token: 0x020031B9 RID: 12729
				public class BULBPLANT
				{
					// Token: 0x0400C6CF RID: 50895
					public static LocString NAME = UI.FormatAsLink("Buddy Bud Seed", "BULBPLANT");

					// Token: 0x0400C6D0 RID: 50896
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						UI.FormatAsLink("Buddy Bud", "BULBPLANT"),
						".\n\nDigging up Buried Objects may uncover a Buddy Bud Seed."
					});
				}

				// Token: 0x020031BA RID: 12730
				public class JUNGLEGASPLANT
				{
				}

				// Token: 0x020031BB RID: 12731
				public class PRICKLEFLOWER
				{
					// Token: 0x0400C6D1 RID: 50897
					public static LocString NAME = UI.FormatAsLink("Blossom Seed", "PRICKLEFLOWER");

					// Token: 0x0400C6D2 RID: 50898
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						UI.FormatAsLink("Bristle Blossom", "PRICKLEFLOWER"),
						".\n\nDigging up Buried Objects may uncover a Blossom Seed."
					});
				}

				// Token: 0x020031BC RID: 12732
				public class MUSHROOMPLANT
				{
					// Token: 0x0400C6D3 RID: 50899
					public static LocString NAME = UI.FormatAsLink("Fungal Spore", "MUSHROOMPLANT");

					// Token: 0x0400C6D4 RID: 50900
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.MUSHROOMPLANT.NAME,
						".\n\nDigging up Buried Objects may uncover a Fungal Spore."
					});
				}

				// Token: 0x020031BD RID: 12733
				public class COLDWHEAT
				{
					// Token: 0x0400C6D5 RID: 50901
					public static LocString NAME = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEAT");

					// Token: 0x0400C6D6 RID: 50902
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.COLDWHEAT.NAME,
						" plant.\n\nGrain can be sown to cultivate more Sleet Wheat, or processed into ",
						UI.FormatAsLink("Food", "FOOD"),
						"."
					});
				}

				// Token: 0x020031BE RID: 12734
				public class GASGRASS
				{
					// Token: 0x0400C6D7 RID: 50903
					public static LocString NAME = UI.FormatAsLink("Gas Grass Seed", "GASGRASS");

					// Token: 0x0400C6D8 RID: 50904
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.GASGRASS.NAME,
						" plant."
					});
				}

				// Token: 0x020031BF RID: 12735
				public class PRICKLEGRASS
				{
					// Token: 0x0400C6D9 RID: 50905
					public static LocString NAME = UI.FormatAsLink("Briar Seed", "PRICKLEGRASS");

					// Token: 0x0400C6DA RID: 50906
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.PRICKLEGRASS.NAME,
						".\n\nDigging up Buried Objects may uncover a Briar Seed."
					});
				}

				// Token: 0x020031C0 RID: 12736
				public class CYLINDRICA
				{
					// Token: 0x0400C6DB RID: 50907
					public static LocString NAME = UI.FormatAsLink("Bliss Burst Seed", "CYLINDRICA");

					// Token: 0x0400C6DC RID: 50908
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.CYLINDRICA.NAME,
						".\n\nDigging up Buried Objects may uncover a Bliss Burst Seed."
					});
				}

				// Token: 0x020031C1 RID: 12737
				public class TOEPLANT
				{
					// Token: 0x0400C6DD RID: 50909
					public static LocString NAME = UI.FormatAsLink("Tranquil Toe Seed", "TOEPLANT");

					// Token: 0x0400C6DE RID: 50910
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.TOEPLANT.NAME,
						".\n\nDigging up Buried Objects may uncover a Tranquil Toe Seed."
					});
				}

				// Token: 0x020031C2 RID: 12738
				public class WINECUPS
				{
					// Token: 0x0400C6DF RID: 50911
					public static LocString NAME = UI.FormatAsLink("Mallow Seed", "WINECUPS");

					// Token: 0x0400C6E0 RID: 50912
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.WINECUPS.NAME,
						".\n\nDigging up Buried Objects may uncover a Mallow Seed."
					});
				}

				// Token: 0x020031C3 RID: 12739
				public class EVILFLOWER
				{
					// Token: 0x0400C6E1 RID: 50913
					public static LocString NAME = UI.FormatAsLink("Sporechid Seed", "EVILFLOWER");

					// Token: 0x0400C6E2 RID: 50914
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.EVILFLOWER.NAME,
						".\n\nDigging up Buried Objects may uncover a ",
						CREATURES.SPECIES.SEEDS.EVILFLOWER.NAME,
						"."
					});
				}

				// Token: 0x020031C4 RID: 12740
				public class SWAMPLILY
				{
					// Token: 0x0400C6E3 RID: 50915
					public static LocString NAME = UI.FormatAsLink("Balm Lily Seed", "SWAMPLILY");

					// Token: 0x0400C6E4 RID: 50916
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.SWAMPLILY.NAME,
						".\n\nDigging up Buried Objects may uncover a Balm Lily Seed."
					});
				}

				// Token: 0x020031C5 RID: 12741
				public class BASICSINGLEHARVESTPLANT
				{
					// Token: 0x0400C6E5 RID: 50917
					public static LocString NAME = UI.FormatAsLink("Mealwood Seed", "BASICSINGLEHARVESTPLANT");

					// Token: 0x0400C6E6 RID: 50918
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.NAME,
						".\n\nDigging up Buried Objects may uncover a Mealwood Seed."
					});
				}

				// Token: 0x020031C6 RID: 12742
				public class SWAMPHARVESTPLANT
				{
					// Token: 0x0400C6E7 RID: 50919
					public static LocString NAME = UI.FormatAsLink("Bog Bucket Seed", "SWAMPHARVESTPLANT");

					// Token: 0x0400C6E8 RID: 50920
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.SWAMPHARVESTPLANT.NAME,
						".\n\nDigging up Buried Objects may uncover a Bog Bucket Seed."
					});
				}

				// Token: 0x020031C7 RID: 12743
				public class WORMPLANT
				{
					// Token: 0x0400C6E9 RID: 50921
					public static LocString NAME = UI.FormatAsLink("Grubfruit Seed", "WORMPLANT");

					// Token: 0x0400C6EA RID: 50922
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.WORMPLANT.NAME,
						".\n\nDigging up Buried Objects may uncover a Grubfruit Seed."
					});
				}

				// Token: 0x020031C8 RID: 12744
				public class COLDBREATHER
				{
					// Token: 0x0400C6EB RID: 50923
					public static LocString NAME = UI.FormatAsLink("Wort Seed", "COLDBREATHER");

					// Token: 0x0400C6EC RID: 50924
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.COLDBREATHER.NAME,
						".\n\nDigging up Buried Objects may uncover a Wort Seed."
					});
				}

				// Token: 0x020031C9 RID: 12745
				public class BASICFABRICMATERIALPLANT
				{
					// Token: 0x0400C6ED RID: 50925
					public static LocString NAME = UI.FormatAsLink("Thimble Reed Seed", "BASICFABRICPLANT");

					// Token: 0x0400C6EE RID: 50926
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME,
						".\n\nDigging up Buried Objects may uncover a Thimble Reed Seed."
					});
				}

				// Token: 0x020031CA RID: 12746
				public class SALTPLANT
				{
					// Token: 0x0400C6EF RID: 50927
					public static LocString NAME = UI.FormatAsLink("Dasha Saltvine Seed", "SALTPLANT");

					// Token: 0x0400C6F0 RID: 50928
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.SALTPLANT.NAME,
						".\n\nDigging up Buried Objects may uncover a Dasha Saltvine Seed."
					});
				}

				// Token: 0x020031CB RID: 12747
				public class FILTERPLANT
				{
					// Token: 0x0400C6F1 RID: 50929
					public static LocString NAME = UI.FormatAsLink("Hydrocactus Seed", "FILTERPLANT");

					// Token: 0x0400C6F2 RID: 50930
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.FILTERPLANT.NAME,
						".\n\nDigging up Buried Objects may uncover a Hydrocactus Seed."
					});
				}

				// Token: 0x020031CC RID: 12748
				public class SPICE_VINE
				{
					// Token: 0x0400C6F3 RID: 50931
					public static LocString NAME = UI.FormatAsLink("Pincha Pepper Seed", "SPICE_VINE");

					// Token: 0x0400C6F4 RID: 50932
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						CREATURES.SPECIES.SPICE_VINE.NAME,
						".\n\nDigging up Buried Objects may uncover a Pincha Pepper Seed."
					});
				}

				// Token: 0x020031CD RID: 12749
				public class BEAN_PLANT
				{
					// Token: 0x0400C6F5 RID: 50933
					public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEAN_PLANT");

					// Token: 0x0400C6F6 RID: 50934
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						UI.FormatAsLink("Nosh Sprout", "BEAN_PLANT"),
						".\n\nDigging up Buried Objects may uncover a Nosh Bean."
					});
				}

				// Token: 0x020031CE RID: 12750
				public class WOOD_TREE
				{
					// Token: 0x0400C6F7 RID: 50935
					public static LocString NAME = UI.FormatAsLink("Arbor Acorn", "FOREST_TREE");

					// Token: 0x0400C6F8 RID: 50936
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of an ",
						UI.FormatAsLink("Arbor Tree", "FOREST_TREE"),
						".\n\nDigging up Buried Objects may uncover an Arbor Acorn."
					});
				}

				// Token: 0x020031CF RID: 12751
				public class OILEATER
				{
					// Token: 0x0400C6F9 RID: 50937
					public static LocString NAME = UI.FormatAsLink("Ink Bloom Seed", "OILEATER");

					// Token: 0x0400C6FA RID: 50938
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						UI.FormatAsLink("Plant", "Ink Bloom"),
						".\n\nDigging up Buried Objects may uncover an Ink Bloom Seed."
					});
				}

				// Token: 0x020031D0 RID: 12752
				public class OXYFERN
				{
					// Token: 0x0400C6FB RID: 50939
					public static LocString NAME = UI.FormatAsLink("Oxyfern Seed", "OXYFERN");

					// Token: 0x0400C6FC RID: 50940
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of an ",
						UI.FormatAsLink("Oxyfern", "OXYFERN"),
						" plant."
					});
				}

				// Token: 0x020031D1 RID: 12753
				public class SEALETTUCE
				{
					// Token: 0x0400C6FD RID: 50941
					public static LocString NAME = UI.FormatAsLink("Waterweed Seed", "SEALETTUCE");

					// Token: 0x0400C6FE RID: 50942
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						UI.FormatAsLink("Waterweed", "SEALETTUCE"),
						".\n\nDigging up Buried Objects may uncover a Waterweed Seed."
					});
				}

				// Token: 0x020031D2 RID: 12754
				public class CRITTERTRAPPLANT
				{
					// Token: 0x0400C6FF RID: 50943
					public static LocString NAME = UI.FormatAsLink("Saturn Critter Trap Seed", "CRITTERTRAPPLANT");

					// Token: 0x0400C700 RID: 50944
					public static LocString DESC = string.Concat(new string[]
					{
						"The ",
						UI.FormatAsLink("Seed", "PLANTS"),
						" of a ",
						UI.FormatAsLink("Saturn Critter Trap", "CRITTERTRAPPLANT"),
						".\n\nDigging up Buried Objects may uncover a Saturn Critter Trap Seed."
					});
				}
			}
		}

		// Token: 0x02001DEA RID: 7658
		public class STATUSITEMS
		{
			// Token: 0x040089B6 RID: 35254
			public static LocString NAME_NON_GROWING_PLANT = "Wilted";

			// Token: 0x02002970 RID: 10608
			public class DROWSY
			{
				// Token: 0x0400B261 RID: 45665
				public static LocString NAME = "Drowsy";

				// Token: 0x0400B262 RID: 45666
				public static LocString TOOLTIP = "This critter is looking for a place to nap";
			}

			// Token: 0x02002971 RID: 10609
			public class SLEEPING
			{
				// Token: 0x0400B263 RID: 45667
				public static LocString NAME = "Sleeping";

				// Token: 0x0400B264 RID: 45668
				public static LocString TOOLTIP = "This critter is replenishing its " + UI.PRE_KEYWORD + "Stamina" + UI.PST_KEYWORD;
			}

			// Token: 0x02002972 RID: 10610
			public class CALL_ADULT
			{
				// Token: 0x0400B265 RID: 45669
				public static LocString NAME = "Calling Adult";

				// Token: 0x0400B266 RID: 45670
				public static LocString TOOLTIP = "This baby's craving attention from one of its own kind";
			}

			// Token: 0x02002973 RID: 10611
			public class HOT
			{
				// Token: 0x0400B267 RID: 45671
				public static LocString NAME = "Toasty surroundings";

				// Token: 0x0400B268 RID: 45672
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter cannot let off enough ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" to keep cool in this environment\n\nIt prefers ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" between <b>{0}</b> and <b>{1}</b>"
				});
			}

			// Token: 0x02002974 RID: 10612
			public class COLD
			{
				// Token: 0x0400B269 RID: 45673
				public static LocString NAME = "Chilly surroundings";

				// Token: 0x0400B26A RID: 45674
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter cannot retain enough ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" to stay warm in this environment\n\nIt prefers ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" between <b>{0}</b> and <b>{1}</b>"
				});
			}

			// Token: 0x02002975 RID: 10613
			public class CROP_TOO_DARK
			{
				// Token: 0x0400B26B RID: 45675
				public static LocString NAME = "    • " + CREATURES.STATS.ILLUMINATION.NAME;

				// Token: 0x0400B26C RID: 45676
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Growth will resume when ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" requirements are met"
				});
			}

			// Token: 0x02002976 RID: 10614
			public class CROP_TOO_BRIGHT
			{
				// Token: 0x0400B26D RID: 45677
				public static LocString NAME = "    • " + CREATURES.STATS.ILLUMINATION.NAME;

				// Token: 0x0400B26E RID: 45678
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Growth will resume when ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" requirements are met"
				});
			}

			// Token: 0x02002977 RID: 10615
			public class CROP_BLIGHTED
			{
				// Token: 0x0400B26F RID: 45679
				public static LocString NAME = "    • Blighted";

				// Token: 0x0400B270 RID: 45680
				public static LocString TOOLTIP = "This plant has been struck by blight and will need to be replaced";
			}

			// Token: 0x02002978 RID: 10616
			public class HOT_CROP
			{
				// Token: 0x0400B271 RID: 45681
				public static LocString NAME = "    • " + DUPLICANTS.STATS.TEMPERATURE.NAME;

				// Token: 0x0400B272 RID: 45682
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Growth will resume when ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is between <b>{low_temperature}</b> and <b>{high_temperature}</b>"
				});
			}

			// Token: 0x02002979 RID: 10617
			public class COLD_CROP
			{
				// Token: 0x0400B273 RID: 45683
				public static LocString NAME = "    • " + DUPLICANTS.STATS.TEMPERATURE.NAME;

				// Token: 0x0400B274 RID: 45684
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Growth will resume when ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is between <b>{low_temperature}</b> and <b>{high_temperature}</b>"
				});
			}

			// Token: 0x0200297A RID: 10618
			public class PERFECTTEMPERATURE
			{
				// Token: 0x0400B275 RID: 45685
				public static LocString NAME = "Ideal Temperature";

				// Token: 0x0400B276 RID: 45686
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter finds the current ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" comfortable\n\nIdeal Range: <b>{0}</b> - <b>{1}</b>"
				});
			}

			// Token: 0x0200297B RID: 10619
			public class EATING
			{
				// Token: 0x0400B277 RID: 45687
				public static LocString NAME = "Eating";

				// Token: 0x0400B278 RID: 45688
				public static LocString TOOLTIP = "This critter found something tasty";
			}

			// Token: 0x0200297C RID: 10620
			public class DRINKINGMILK
			{
				// Token: 0x0400B279 RID: 45689
				public static LocString NAME = "Drinking";

				// Token: 0x0400B27A RID: 45690
				public static LocString TOOLTIP = "This critter found a tasty beverage";
			}

			// Token: 0x0200297D RID: 10621
			public class DIGESTING
			{
				// Token: 0x0400B27B RID: 45691
				public static LocString NAME = "Digesting";

				// Token: 0x0400B27C RID: 45692
				public static LocString TOOLTIP = "This critter is working off a big meal";
			}

			// Token: 0x0200297E RID: 10622
			public class COOLING
			{
				// Token: 0x0400B27D RID: 45693
				public static LocString NAME = "Chilly Breath";

				// Token: 0x0400B27E RID: 45694
				public static LocString TOOLTIP = "This critter's respiration is having a cooling effect on the area";
			}

			// Token: 0x0200297F RID: 10623
			public class LOOKINGFORFOOD
			{
				// Token: 0x0400B27F RID: 45695
				public static LocString NAME = "Foraging";

				// Token: 0x0400B280 RID: 45696
				public static LocString TOOLTIP = "This critter is hungry and looking for " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02002980 RID: 10624
			public class LOOKINGFORLIQUID
			{
				// Token: 0x0400B281 RID: 45697
				public static LocString NAME = "Parched";

				// Token: 0x0400B282 RID: 45698
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter is looking for ",
					UI.PRE_KEYWORD,
					"Liquids",
					UI.PST_KEYWORD,
					" to mop up"
				});
			}

			// Token: 0x02002981 RID: 10625
			public class LOOKINGFORGAS
			{
				// Token: 0x0400B283 RID: 45699
				public static LocString NAME = "Seeking Gas";

				// Token: 0x0400B284 RID: 45700
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter is on the lookout for unbreathable ",
					UI.PRE_KEYWORD,
					"Gases",
					UI.PST_KEYWORD,
					" to collect"
				});
			}

			// Token: 0x02002982 RID: 10626
			public class LOOKINGFORMILK
			{
				// Token: 0x0400B285 RID: 45701
				public static LocString NAME = "Seeking Brackene";

				// Token: 0x0400B286 RID: 45702
				public static LocString TOOLTIP = "This critter is thirsty for " + UI.PRE_KEYWORD + "Brackene" + UI.PST_KEYWORD;
			}

			// Token: 0x02002983 RID: 10627
			public class IDLE
			{
				// Token: 0x0400B287 RID: 45703
				public static LocString NAME = "Idle";

				// Token: 0x0400B288 RID: 45704
				public static LocString TOOLTIP = "Just enjoying life, y'know?";
			}

			// Token: 0x02002984 RID: 10628
			public class HIVE_DIGESTING
			{
				// Token: 0x0400B289 RID: 45705
				public static LocString NAME = "Digesting";

				// Token: 0x0400B28A RID: 45706
				public static LocString TOOLTIP = "Digesting yummy food!";
			}

			// Token: 0x02002985 RID: 10629
			public class EXCITED_TO_GET_RANCHED
			{
				// Token: 0x0400B28B RID: 45707
				public static LocString NAME = "Excited";

				// Token: 0x0400B28C RID: 45708
				public static LocString TOOLTIP = "This critter heard a Duplicant call for it and is very excited!";
			}

			// Token: 0x02002986 RID: 10630
			public class GETTING_RANCHED
			{
				// Token: 0x0400B28D RID: 45709
				public static LocString NAME = "Being Groomed";

				// Token: 0x0400B28E RID: 45710
				public static LocString TOOLTIP = "This critter's going to look so good when they're done";
			}

			// Token: 0x02002987 RID: 10631
			public class GETTING_MILKED
			{
				// Token: 0x0400B28F RID: 45711
				public static LocString NAME = "Being Milked";

				// Token: 0x0400B290 RID: 45712
				public static LocString TOOLTIP = "This critter's going to be so relieved when they're done";
			}

			// Token: 0x02002988 RID: 10632
			public class EXCITED_TO_BE_RANCHED
			{
				// Token: 0x0400B291 RID: 45713
				public static LocString NAME = "Freshly Groomed";

				// Token: 0x0400B292 RID: 45714
				public static LocString TOOLTIP = "This critter just received some attention and feels great";
			}

			// Token: 0x02002989 RID: 10633
			public class GETTING_WRANGLED
			{
				// Token: 0x0400B293 RID: 45715
				public static LocString NAME = "Being Wrangled";

				// Token: 0x0400B294 RID: 45716
				public static LocString TOOLTIP = "Someone's trying to capture this critter!";
			}

			// Token: 0x0200298A RID: 10634
			public class BAGGED
			{
				// Token: 0x0400B295 RID: 45717
				public static LocString NAME = "Trussed";

				// Token: 0x0400B296 RID: 45718
				public static LocString TOOLTIP = "Tied up and ready for relocation";
			}

			// Token: 0x0200298B RID: 10635
			public class IN_INCUBATOR
			{
				// Token: 0x0400B297 RID: 45719
				public static LocString NAME = "Incubation Complete";

				// Token: 0x0400B298 RID: 45720
				public static LocString TOOLTIP = "This critter has hatched and is waiting to be released from its incubator";
			}

			// Token: 0x0200298C RID: 10636
			public class HYPOTHERMIA
			{
				// Token: 0x0400B299 RID: 45721
				public static LocString NAME = "Freezing";

				// Token: 0x0400B29A RID: 45722
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Internal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is dangerously low"
				});
			}

			// Token: 0x0200298D RID: 10637
			public class SCALDING
			{
				// Token: 0x0400B29B RID: 45723
				public static LocString NAME = "Scalding";

				// Token: 0x0400B29C RID: 45724
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Current external ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is perilously high [<b>{ExternalTemperature}</b> / <b>{TargetTemperature}</b>]"
				});

				// Token: 0x0400B29D RID: 45725
				public static LocString NOTIFICATION_NAME = "Scalding";

				// Token: 0x0400B29E RID: 45726
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Scalding ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" are hurting these Duplicants:"
				});
			}

			// Token: 0x0200298E RID: 10638
			public class HYPERTHERMIA
			{
				// Token: 0x0400B29F RID: 45727
				public static LocString NAME = "Overheating";

				// Token: 0x0400B2A0 RID: 45728
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Internal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is dangerously high [<b>{InternalTemperature}</b> / <b>{TargetTemperature}</b>]"
				});
			}

			// Token: 0x0200298F RID: 10639
			public class TIRED
			{
				// Token: 0x0400B2A1 RID: 45729
				public static LocString NAME = "Fatigued";

				// Token: 0x0400B2A2 RID: 45730
				public static LocString TOOLTIP = "This critter needs some sleepytime";
			}

			// Token: 0x02002990 RID: 10640
			public class BREATH
			{
				// Token: 0x0400B2A3 RID: 45731
				public static LocString NAME = "Suffocating";

				// Token: 0x0400B2A4 RID: 45732
				public static LocString TOOLTIP = "This critter is about to suffocate";
			}

			// Token: 0x02002991 RID: 10641
			public class DEAD
			{
				// Token: 0x0400B2A5 RID: 45733
				public static LocString NAME = "Dead";

				// Token: 0x0400B2A6 RID: 45734
				public static LocString TOOLTIP = "This critter won't be getting back up...";
			}

			// Token: 0x02002992 RID: 10642
			public class PLANTDEATH
			{
				// Token: 0x0400B2A7 RID: 45735
				public static LocString NAME = "Dead";

				// Token: 0x0400B2A8 RID: 45736
				public static LocString TOOLTIP = "This plant will produce no more harvests";

				// Token: 0x0400B2A9 RID: 45737
				public static LocString NOTIFICATION = "Plants have died";

				// Token: 0x0400B2AA RID: 45738
				public static LocString NOTIFICATION_TOOLTIP = "These plants have died and will produce no more harvests:\n";
			}

			// Token: 0x02002993 RID: 10643
			public class STRUGGLING
			{
				// Token: 0x0400B2AB RID: 45739
				public static LocString NAME = "Struggling";

				// Token: 0x0400B2AC RID: 45740
				public static LocString TOOLTIP = "This critter is trying to get away";
			}

			// Token: 0x02002994 RID: 10644
			public class BURROWING
			{
				// Token: 0x0400B2AD RID: 45741
				public static LocString NAME = "Burrowing";

				// Token: 0x0400B2AE RID: 45742
				public static LocString TOOLTIP = "This critter is trying to hide";
			}

			// Token: 0x02002995 RID: 10645
			public class BURROWED
			{
				// Token: 0x0400B2AF RID: 45743
				public static LocString NAME = "Burrowed";

				// Token: 0x0400B2B0 RID: 45744
				public static LocString TOOLTIP = "Shh! It thinks it's hiding";
			}

			// Token: 0x02002996 RID: 10646
			public class EMERGING
			{
				// Token: 0x0400B2B1 RID: 45745
				public static LocString NAME = "Emerging";

				// Token: 0x0400B2B2 RID: 45746
				public static LocString TOOLTIP = "This critter is leaving its burrow";
			}

			// Token: 0x02002997 RID: 10647
			public class FORAGINGMATERIAL
			{
				// Token: 0x0400B2B3 RID: 45747
				public static LocString NAME = "Foraging for Materials";

				// Token: 0x0400B2B4 RID: 45748
				public static LocString TOOLTIP = "This critter is stocking up on supplies for later use";
			}

			// Token: 0x02002998 RID: 10648
			public class PLANTINGSEED
			{
				// Token: 0x0400B2B5 RID: 45749
				public static LocString NAME = "Planting Seed";

				// Token: 0x0400B2B6 RID: 45750
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter is burying a ",
					UI.PRE_KEYWORD,
					"Seed",
					UI.PST_KEYWORD,
					" for later"
				});
			}

			// Token: 0x02002999 RID: 10649
			public class RUMMAGINGSEED
			{
				// Token: 0x0400B2B7 RID: 45751
				public static LocString NAME = "Rummaging for seeds";

				// Token: 0x0400B2B8 RID: 45752
				public static LocString TOOLTIP = "This critter is searching for tasty " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;
			}

			// Token: 0x0200299A RID: 10650
			public class HUGEGG
			{
				// Token: 0x0400B2B9 RID: 45753
				public static LocString NAME = "Hugging Eggs";

				// Token: 0x0400B2BA RID: 45754
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter is snuggling up to an ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x0200299B RID: 10651
			public class HUGMINIONWAITING
			{
				// Token: 0x0400B2BB RID: 45755
				public static LocString NAME = "Hoping for hugs";

				// Token: 0x0400B2BC RID: 45756
				public static LocString TOOLTIP = "This critter is hoping for a Duplicant to pass by and give it a hug\n\nA hug from a Duplicant will prompt it to cuddle more eggs";
			}

			// Token: 0x0200299C RID: 10652
			public class HUGMINION
			{
				// Token: 0x0400B2BD RID: 45757
				public static LocString NAME = "Hugging";

				// Token: 0x0400B2BE RID: 45758
				public static LocString TOOLTIP = "This critter is happily hugging a Duplicant";
			}

			// Token: 0x0200299D RID: 10653
			public class EXPELLING_SOLID
			{
				// Token: 0x0400B2BF RID: 45759
				public static LocString NAME = "Expelling Waste";

				// Token: 0x0400B2C0 RID: 45760
				public static LocString TOOLTIP = "This critter is doing their \"business\"";
			}

			// Token: 0x0200299E RID: 10654
			public class EXPELLING_GAS
			{
				// Token: 0x0400B2C1 RID: 45761
				public static LocString NAME = "Passing Gas";

				// Token: 0x0400B2C2 RID: 45762
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter is emitting ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					"\n\nYuck!"
				});
			}

			// Token: 0x0200299F RID: 10655
			public class EXPELLING_LIQUID
			{
				// Token: 0x0400B2C3 RID: 45763
				public static LocString NAME = "Expelling Waste";

				// Token: 0x0400B2C4 RID: 45764
				public static LocString TOOLTIP = "This critter is doing their \"business\"";
			}

			// Token: 0x020029A0 RID: 10656
			public class DEBUGGOTO
			{
				// Token: 0x0400B2C5 RID: 45765
				public static LocString NAME = "Moving to debug location";

				// Token: 0x0400B2C6 RID: 45766
				public static LocString TOOLTIP = "All that obedience training paid off";
			}

			// Token: 0x020029A1 RID: 10657
			public class ATTACK_APPROACH
			{
				// Token: 0x0400B2C7 RID: 45767
				public static LocString NAME = "Stalking Target";

				// Token: 0x0400B2C8 RID: 45768
				public static LocString TOOLTIP = "This critter is hostile and readying to pounce!";
			}

			// Token: 0x020029A2 RID: 10658
			public class ATTACK
			{
				// Token: 0x0400B2C9 RID: 45769
				public static LocString NAME = "Combat!";

				// Token: 0x0400B2CA RID: 45770
				public static LocString TOOLTIP = "This critter is on the attack!";
			}

			// Token: 0x020029A3 RID: 10659
			public class ATTACKINGENTITY
			{
				// Token: 0x0400B2CB RID: 45771
				public static LocString NAME = "Attacking";

				// Token: 0x0400B2CC RID: 45772
				public static LocString TOOLTIP = "This critter is violently defending their young";
			}

			// Token: 0x020029A4 RID: 10660
			public class PROTECTINGENTITY
			{
				// Token: 0x0400B2CD RID: 45773
				public static LocString NAME = "Protecting";

				// Token: 0x0400B2CE RID: 45774
				public static LocString TOOLTIP = "This creature is guarding something special to them and will likely attack if approached";
			}

			// Token: 0x020029A5 RID: 10661
			public class LAYINGANEGG
			{
				// Token: 0x0400B2CF RID: 45775
				public static LocString NAME = "Laying egg";

				// Token: 0x0400B2D0 RID: 45776
				public static LocString TOOLTIP = "Witness the miracle of life!";
			}

			// Token: 0x020029A6 RID: 10662
			public class TENDINGANEGG
			{
				// Token: 0x0400B2D1 RID: 45777
				public static LocString NAME = "Tending egg";

				// Token: 0x0400B2D2 RID: 45778
				public static LocString TOOLTIP = "Nurturing the miracle of life!";
			}

			// Token: 0x020029A7 RID: 10663
			public class GROWINGUP
			{
				// Token: 0x0400B2D3 RID: 45779
				public static LocString NAME = "Maturing";

				// Token: 0x0400B2D4 RID: 45780
				public static LocString TOOLTIP = "This baby critter is about to reach adulthood";
			}

			// Token: 0x020029A8 RID: 10664
			public class SUFFOCATING
			{
				// Token: 0x0400B2D5 RID: 45781
				public static LocString NAME = "Suffocating";

				// Token: 0x0400B2D6 RID: 45782
				public static LocString TOOLTIP = "This critter cannot breathe";
			}

			// Token: 0x020029A9 RID: 10665
			public class HATCHING
			{
				// Token: 0x0400B2D7 RID: 45783
				public static LocString NAME = "Hatching";

				// Token: 0x0400B2D8 RID: 45784
				public static LocString TOOLTIP = "Here it comes!";
			}

			// Token: 0x020029AA RID: 10666
			public class INCUBATING
			{
				// Token: 0x0400B2D9 RID: 45785
				public static LocString NAME = "Incubating";

				// Token: 0x0400B2DA RID: 45786
				public static LocString TOOLTIP = "Cozily preparing to meet the world";
			}

			// Token: 0x020029AB RID: 10667
			public class CONSIDERINGLURE
			{
				// Token: 0x0400B2DB RID: 45787
				public static LocString NAME = "Piqued";

				// Token: 0x0400B2DC RID: 45788
				public static LocString TOOLTIP = "This critter is tempted to bite a nearby " + UI.PRE_KEYWORD + "Lure" + UI.PST_KEYWORD;
			}

			// Token: 0x020029AC RID: 10668
			public class FALLING
			{
				// Token: 0x0400B2DD RID: 45789
				public static LocString NAME = "Falling";

				// Token: 0x0400B2DE RID: 45790
				public static LocString TOOLTIP = "AHHHH!";
			}

			// Token: 0x020029AD RID: 10669
			public class FLOPPING
			{
				// Token: 0x0400B2DF RID: 45791
				public static LocString NAME = "Flopping";

				// Token: 0x0400B2E0 RID: 45792
				public static LocString TOOLTIP = "Fish out of water!";
			}

			// Token: 0x020029AE RID: 10670
			public class DRYINGOUT
			{
				// Token: 0x0400B2E1 RID: 45793
				public static LocString NAME = "    • Beached";

				// Token: 0x0400B2E2 RID: 45794
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This plant must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to grow"
				});
			}

			// Token: 0x020029AF RID: 10671
			public class GROWING
			{
				// Token: 0x0400B2E3 RID: 45795
				public static LocString NAME = "Growing [{PercentGrow}%]";

				// Token: 0x0400B2E4 RID: 45796
				public static LocString TOOLTIP = "Next harvest: <b>{TimeUntilNextHarvest}</b>";
			}

			// Token: 0x020029B0 RID: 10672
			public class CROP_SLEEPING
			{
				// Token: 0x0400B2E5 RID: 45797
				public static LocString NAME = "Sleeping [{REASON}]";

				// Token: 0x0400B2E6 RID: 45798
				public static LocString TOOLTIP = "Requires: {REQUIREMENTS}";

				// Token: 0x0400B2E7 RID: 45799
				public static LocString REQUIREMENT_LUMINANCE = "<b>{0}</b> Lux";

				// Token: 0x0400B2E8 RID: 45800
				public static LocString REASON_TOO_DARK = "Too Dark";

				// Token: 0x0400B2E9 RID: 45801
				public static LocString REASON_TOO_BRIGHT = "Too Bright";
			}

			// Token: 0x020029B1 RID: 10673
			public class MOLTING
			{
				// Token: 0x0400B2EA RID: 45802
				public static LocString NAME = "Molting";

				// Token: 0x0400B2EB RID: 45803
				public static LocString TOOLTIP = "This critter is shedding its skin. Yuck";
			}

			// Token: 0x020029B2 RID: 10674
			public class CLEANING
			{
				// Token: 0x0400B2EC RID: 45804
				public static LocString NAME = "Cleaning";

				// Token: 0x0400B2ED RID: 45805
				public static LocString TOOLTIP = "This critter is de-germ-ifying its liquid surroundings";
			}

			// Token: 0x020029B3 RID: 10675
			public class MILKPRODUCER
			{
				// Token: 0x0400B2EE RID: 45806
				public static LocString NAME = "Producing Brackene {amount}";

				// Token: 0x0400B2EF RID: 45807
				public static LocString TOOLTIP = "This critter's internal tank is refilling itself";
			}

			// Token: 0x020029B4 RID: 10676
			public class NEEDSFERTILIZER
			{
				// Token: 0x0400B2F0 RID: 45808
				public static LocString NAME = "    • " + CREATURES.STATS.FERTILIZATION.NAME;

				// Token: 0x0400B2F1 RID: 45809
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Growth will resume when ",
					UI.PRE_KEYWORD,
					"Fertilization",
					UI.PST_KEYWORD,
					" requirements are met"
				});

				// Token: 0x0400B2F2 RID: 45810
				public static LocString LINE_ITEM = "\n            • {Resource}: {Amount}";
			}

			// Token: 0x020029B5 RID: 10677
			public class NEEDSIRRIGATION
			{
				// Token: 0x0400B2F3 RID: 45811
				public static LocString NAME = "    • " + CREATURES.STATS.IRRIGATION.NAME;

				// Token: 0x0400B2F4 RID: 45812
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Growth will resume when ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" requirements are met"
				});

				// Token: 0x0400B2F5 RID: 45813
				public static LocString LINE_ITEM = "\n            • {Resource}: {Amount}";
			}

			// Token: 0x020029B6 RID: 10678
			public class WRONGFERTILIZER
			{
				// Token: 0x0400B2F6 RID: 45814
				public static LocString NAME = "    • " + CREATURES.STATS.FERTILIZATION.NAME;

				// Token: 0x0400B2F7 RID: 45815
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This farm is storing materials that are not suitable for this plant\n\nEmpty this building's ",
					UI.PRE_KEYWORD,
					"Storage",
					UI.PST_KEYWORD,
					" to remove the unusable materials"
				});

				// Token: 0x0400B2F8 RID: 45816
				public static LocString LINE_ITEM = "            • {0}: {1}\n";
			}

			// Token: 0x020029B7 RID: 10679
			public class WRONGIRRIGATION
			{
				// Token: 0x0400B2F9 RID: 45817
				public static LocString NAME = "    • " + CREATURES.STATS.FERTILIZATION.NAME;

				// Token: 0x0400B2FA RID: 45818
				public static LocString TOOLTIP = "This farm is storing materials that are not suitable for this plant\n\nEmpty this building's storage to remove the unusable materials";

				// Token: 0x0400B2FB RID: 45819
				public static LocString LINE_ITEM = "            • {0}: {1}\n";
			}

			// Token: 0x020029B8 RID: 10680
			public class WRONGFERTILIZERMAJOR
			{
				// Token: 0x0400B2FC RID: 45820
				public static LocString NAME = "    • " + CREATURES.STATS.FERTILIZATION.NAME;

				// Token: 0x0400B2FD RID: 45821
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This farm is storing materials that are not suitable for this plant\n\n",
					UI.PRE_KEYWORD,
					"Empty Storage",
					UI.PST_KEYWORD,
					" on this building to remove the unusable materials"
				});

				// Token: 0x0400B2FE RID: 45822
				public static LocString LINE_ITEM = "        " + CREATURES.STATUSITEMS.WRONGFERTILIZER.LINE_ITEM;
			}

			// Token: 0x020029B9 RID: 10681
			public class WRONGIRRIGATIONMAJOR
			{
				// Token: 0x0400B2FF RID: 45823
				public static LocString NAME = "    • Irrigation (Stored)";

				// Token: 0x0400B300 RID: 45824
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This farm is storing ",
					UI.PRE_KEYWORD,
					"Liquids",
					UI.PST_KEYWORD,
					" that are not suitable for this plant\n\n",
					UI.PRE_KEYWORD,
					"Empty Storage",
					UI.PST_KEYWORD,
					" on this building to remove the unusable liquids"
				});

				// Token: 0x0400B301 RID: 45825
				public static LocString LINE_ITEM = "        " + CREATURES.STATUSITEMS.WRONGIRRIGATION.LINE_ITEM;
			}

			// Token: 0x020029BA RID: 10682
			public class CANTACCEPTFERTILIZER
			{
				// Token: 0x0400B302 RID: 45826
				public static LocString NAME = "    • " + CREATURES.STATS.FERTILIZATION.NAME;

				// Token: 0x0400B303 RID: 45827
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This farm plot does not accept ",
					UI.PRE_KEYWORD,
					"Fertilizer",
					UI.PST_KEYWORD,
					"\n\nMove the selected plant to a fertilization capable plot for optimal growth"
				});
			}

			// Token: 0x020029BB RID: 10683
			public class CANTACCEPTIRRIGATION
			{
				// Token: 0x0400B304 RID: 45828
				public static LocString NAME = "    • " + CREATURES.STATS.IRRIGATION.NAME;

				// Token: 0x0400B305 RID: 45829
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This farm plot does not accept ",
					UI.PRE_KEYWORD,
					"Irrigation",
					UI.PST_KEYWORD,
					"\n\nMove the selected plant to an irrigation capable plot for optimal growth"
				});
			}

			// Token: 0x020029BC RID: 10684
			public class READYFORHARVEST
			{
				// Token: 0x0400B306 RID: 45830
				public static LocString NAME = "Harvest Ready";

				// Token: 0x0400B307 RID: 45831
				public static LocString TOOLTIP = "This plant can be harvested for materials";
			}

			// Token: 0x020029BD RID: 10685
			public class LOW_YIELD
			{
				// Token: 0x0400B308 RID: 45832
				public static LocString NAME = "Standard Yield";

				// Token: 0x0400B309 RID: 45833
				public static LocString TOOLTIP = "This plant produced an average yield";
			}

			// Token: 0x020029BE RID: 10686
			public class NORMAL_YIELD
			{
				// Token: 0x0400B30A RID: 45834
				public static LocString NAME = "Good Yield";

				// Token: 0x0400B30B RID: 45835
				public static LocString TOOLTIP = "Comfortable conditions allowed this plant to produce a better yield\n{Effects}";

				// Token: 0x0400B30C RID: 45836
				public static LocString LINE_ITEM = "    • {0}\n";
			}

			// Token: 0x020029BF RID: 10687
			public class HIGH_YIELD
			{
				// Token: 0x0400B30D RID: 45837
				public static LocString NAME = "Excellent Yield";

				// Token: 0x0400B30E RID: 45838
				public static LocString TOOLTIP = "Consistently ideal conditions allowed this plant to bear a large yield\n{Effects}";

				// Token: 0x0400B30F RID: 45839
				public static LocString LINE_ITEM = "    • {0}\n";
			}

			// Token: 0x020029C0 RID: 10688
			public class ENTOMBED
			{
				// Token: 0x0400B310 RID: 45840
				public static LocString NAME = "Entombed";

				// Token: 0x0400B311 RID: 45841
				public static LocString TOOLTIP = "This {0} is trapped and needs help digging out";

				// Token: 0x0400B312 RID: 45842
				public static LocString LINE_ITEM = "    • Entombed";
			}

			// Token: 0x020029C1 RID: 10689
			public class DROWNING
			{
				// Token: 0x0400B313 RID: 45843
				public static LocString NAME = "Drowning";

				// Token: 0x0400B314 RID: 45844
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter can't breathe in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					"!"
				});
			}

			// Token: 0x020029C2 RID: 10690
			public class DISABLED
			{
				// Token: 0x0400B315 RID: 45845
				public static LocString NAME = "Disabled";

				// Token: 0x0400B316 RID: 45846
				public static LocString TOOLTIP = "Something is preventing this critter from functioning!";
			}

			// Token: 0x020029C3 RID: 10691
			public class SATURATED
			{
				// Token: 0x0400B317 RID: 45847
				public static LocString NAME = "Too Wet!";

				// Token: 0x0400B318 RID: 45848
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter likes ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					", but not that much!"
				});
			}

			// Token: 0x020029C4 RID: 10692
			public class WILTING
			{
				// Token: 0x0400B319 RID: 45849
				public static LocString NAME = "Growth Halted{Reasons}";

				// Token: 0x0400B31A RID: 45850
				public static LocString TOOLTIP = "Growth will resume when conditions improve";
			}

			// Token: 0x020029C5 RID: 10693
			public class WILTINGDOMESTIC
			{
				// Token: 0x0400B31B RID: 45851
				public static LocString NAME = "Growth Halted{Reasons}";

				// Token: 0x0400B31C RID: 45852
				public static LocString TOOLTIP = "Growth will resume when conditions improve";
			}

			// Token: 0x020029C6 RID: 10694
			public class WILTING_NON_GROWING_PLANT
			{
				// Token: 0x0400B31D RID: 45853
				public static LocString NAME = "Growth Halted{Reasons}";

				// Token: 0x0400B31E RID: 45854
				public static LocString TOOLTIP = "Growth will resume when conditions improve";
			}

			// Token: 0x020029C7 RID: 10695
			public class BARREN
			{
				// Token: 0x0400B31F RID: 45855
				public static LocString NAME = "Barren";

				// Token: 0x0400B320 RID: 45856
				public static LocString TOOLTIP = "This plant will produce no more " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;
			}

			// Token: 0x020029C8 RID: 10696
			public class ATMOSPHERICPRESSURETOOLOW
			{
				// Token: 0x0400B321 RID: 45857
				public static LocString NAME = "    • Pressure";

				// Token: 0x0400B322 RID: 45858
				public static LocString TOOLTIP = "Growth will resume when air pressure is between <b>{low_mass}</b> and <b>{high_mass}</b>";
			}

			// Token: 0x020029C9 RID: 10697
			public class WRONGATMOSPHERE
			{
				// Token: 0x0400B323 RID: 45859
				public static LocString NAME = "    • Atmosphere";

				// Token: 0x0400B324 RID: 45860
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Growth will resume when submersed in one of the following ",
					UI.PRE_KEYWORD,
					"Gases",
					UI.PST_KEYWORD,
					": {elements}"
				});
			}

			// Token: 0x020029CA RID: 10698
			public class ATMOSPHERICPRESSURETOOHIGH
			{
				// Token: 0x0400B325 RID: 45861
				public static LocString NAME = "    • Pressure";

				// Token: 0x0400B326 RID: 45862
				public static LocString TOOLTIP = "Growth will resume when air pressure is between <b>{low_mass}</b> and <b>{high_mass}</b>";
			}

			// Token: 0x020029CB RID: 10699
			public class PERFECTATMOSPHERICPRESSURE
			{
				// Token: 0x0400B327 RID: 45863
				public static LocString NAME = "Ideal Air Pressure";

				// Token: 0x0400B328 RID: 45864
				public static LocString TOOLTIP = "This critter is comfortable in the current atmospheric pressure\n\nIdeal Range: <b>{0}</b> - <b>{1}</b>";
			}

			// Token: 0x020029CC RID: 10700
			public class HEALTHSTATUS
			{
				// Token: 0x0400B329 RID: 45865
				public static LocString NAME = "Injuries: {healthState}";

				// Token: 0x0400B32A RID: 45866
				public static LocString TOOLTIP = "Current physical status: {healthState}";
			}

			// Token: 0x020029CD RID: 10701
			public class FLEEING
			{
				// Token: 0x0400B32B RID: 45867
				public static LocString NAME = "Fleeing";

				// Token: 0x0400B32C RID: 45868
				public static LocString TOOLTIP = "This critter is trying to escape\nGet'em!";
			}

			// Token: 0x020029CE RID: 10702
			public class REFRIGERATEDFROZEN
			{
				// Token: 0x0400B32D RID: 45869
				public static LocString NAME = "Deep Freeze";

				// Token: 0x0400B32E RID: 45870
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" below <b>{PreserveTemperature}</b> are greatly prolonging the shelf-life of this food\n\n",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" above <b>{RotTemperature}</b> spoil food more quickly"
				});
			}

			// Token: 0x020029CF RID: 10703
			public class REFRIGERATED
			{
				// Token: 0x0400B32F RID: 45871
				public static LocString NAME = "Refrigerated";

				// Token: 0x0400B330 RID: 45872
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Ideal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" storage is slowing this food's ",
					UI.PRE_KEYWORD,
					"Decay Rate",
					UI.PST_KEYWORD,
					"\n\n",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" above <b>{RotTemperature}</b> spoil food more quickly\n\nStore food below {PreserveTemperature} to further reduce spoilage."
				});
			}

			// Token: 0x020029D0 RID: 10704
			public class UNREFRIGERATED
			{
				// Token: 0x0400B331 RID: 45873
				public static LocString NAME = "Unrefrigerated";

				// Token: 0x0400B332 RID: 45874
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This food is warm\n\n",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" above <b>{RotTemperature}</b> spoil food more quickly"
				});
			}

			// Token: 0x020029D1 RID: 10705
			public class CONTAMINATEDATMOSPHERE
			{
				// Token: 0x0400B333 RID: 45875
				public static LocString NAME = "Pollution Exposure";

				// Token: 0x0400B334 RID: 45876
				public static LocString TOOLTIP = "Exposure to contaminants is accelerating this food's " + UI.PRE_KEYWORD + "Decay Rate" + UI.PST_KEYWORD;
			}

			// Token: 0x020029D2 RID: 10706
			public class STERILIZINGATMOSPHERE
			{
				// Token: 0x0400B335 RID: 45877
				public static LocString NAME = "Sterile Atmosphere";

				// Token: 0x0400B336 RID: 45878
				public static LocString TOOLTIP = "Microbe destroying conditions have decreased this food's " + UI.PRE_KEYWORD + "Decay Rate" + UI.PST_KEYWORD;
			}

			// Token: 0x020029D3 RID: 10707
			public class EXCHANGINGELEMENTCONSUME
			{
				// Token: 0x0400B337 RID: 45879
				public static LocString NAME = "Consuming {ConsumeElement} at {ConsumeRate}";

				// Token: 0x0400B338 RID: 45880
				public static LocString TOOLTIP = "{ConsumeElement} is being used at a rate of " + UI.FormatAsNegativeRate("{ConsumeRate}");
			}

			// Token: 0x020029D4 RID: 10708
			public class EXCHANGINGELEMENTOUTPUT
			{
				// Token: 0x0400B339 RID: 45881
				public static LocString NAME = "Outputting {OutputElement} at {OutputRate}";

				// Token: 0x0400B33A RID: 45882
				public static LocString TOOLTIP = "{OutputElement} is being expelled at a rate of " + UI.FormatAsPositiveRate("{OutputRate}");
			}

			// Token: 0x020029D5 RID: 10709
			public class FRESH
			{
				// Token: 0x0400B33B RID: 45883
				public static LocString NAME = "Fresh {RotPercentage}";

				// Token: 0x0400B33C RID: 45884
				public static LocString TOOLTIP = "Get'em while they're hot!\n\n{RotTooltip}";
			}

			// Token: 0x020029D6 RID: 10710
			public class STALE
			{
				// Token: 0x0400B33D RID: 45885
				public static LocString NAME = "Stale {RotPercentage}";

				// Token: 0x0400B33E RID: 45886
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" is still edible but will soon expire\n{RotTooltip}"
				});
			}

			// Token: 0x020029D7 RID: 10711
			public class SPOILED
			{
				// Token: 0x0400B33F RID: 45887
				public static LocString NAME = "Rotten";

				// Token: 0x0400B340 RID: 45888
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" has putrefied and should not be consumed"
				});
			}

			// Token: 0x020029D8 RID: 10712
			public class STUNTED_SCALE_GROWTH
			{
				// Token: 0x0400B341 RID: 45889
				public static LocString NAME = "Stunted Scales";

				// Token: 0x0400B342 RID: 45890
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter's ",
					UI.PRE_KEYWORD,
					"Scale Growth",
					UI.PST_KEYWORD,
					" is being stunted by an unfavorable environment"
				});
			}

			// Token: 0x020029D9 RID: 10713
			public class RECEPTACLEINOPERATIONAL
			{
				// Token: 0x0400B343 RID: 45891
				public static LocString NAME = "    • Farm plot inoperable";

				// Token: 0x0400B344 RID: 45892
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This farm plot cannot grow ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					" in its current state"
				});
			}

			// Token: 0x020029DA RID: 10714
			public class TRAPPED
			{
				// Token: 0x0400B345 RID: 45893
				public static LocString NAME = "Trapped";

				// Token: 0x0400B346 RID: 45894
				public static LocString TOOLTIP = "This critter has been contained and cannot escape";
			}

			// Token: 0x020029DB RID: 10715
			public class EXHALING
			{
				// Token: 0x0400B347 RID: 45895
				public static LocString NAME = "Exhaling";

				// Token: 0x0400B348 RID: 45896
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter is expelling ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" from its lungsacs"
				});
			}

			// Token: 0x020029DC RID: 10716
			public class INHALING
			{
				// Token: 0x0400B349 RID: 45897
				public static LocString NAME = "Inhaling";

				// Token: 0x0400B34A RID: 45898
				public static LocString TOOLTIP = "This critter is taking a deep breath";
			}

			// Token: 0x020029DD RID: 10717
			public class EXTERNALTEMPERATURE
			{
				// Token: 0x0400B34B RID: 45899
				public static LocString NAME = "External Temperature";

				// Token: 0x0400B34C RID: 45900
				public static LocString TOOLTIP = "External Temperature\n\nThis critter's environment is {0}";
			}

			// Token: 0x020029DE RID: 10718
			public class RECEPTACLEOPERATIONAL
			{
				// Token: 0x0400B34D RID: 45901
				public static LocString NAME = "Farm plot operational";

				// Token: 0x0400B34E RID: 45902
				public static LocString TOOLTIP = "This plant's farm plot is operational";
			}

			// Token: 0x020029DF RID: 10719
			public class DOMESTICATION
			{
				// Token: 0x0400B34F RID: 45903
				public static LocString NAME = "Domestication Level: {LevelName}";

				// Token: 0x0400B350 RID: 45904
				public static LocString TOOLTIP = "{LevelDesc}";
			}

			// Token: 0x020029E0 RID: 10720
			public class HUNGRY
			{
				// Token: 0x0400B351 RID: 45905
				public static LocString NAME = "Hungry";

				// Token: 0x0400B352 RID: 45906
				public static LocString TOOLTIP = "This critter's tummy is rumbling";
			}

			// Token: 0x020029E1 RID: 10721
			public class HIVEHUNGRY
			{
				// Token: 0x0400B353 RID: 45907
				public static LocString NAME = "Food Supply Low";

				// Token: 0x0400B354 RID: 45908
				public static LocString TOOLTIP = "The food reserves in this hive are running low";
			}

			// Token: 0x020029E2 RID: 10722
			public class STARVING
			{
				// Token: 0x0400B355 RID: 45909
				public static LocString NAME = "Starving\nTime until death: {TimeUntilDeath}\n";

				// Token: 0x0400B356 RID: 45910
				public static LocString TOOLTIP = "This critter is starving and will die if it is not fed soon";

				// Token: 0x0400B357 RID: 45911
				public static LocString NOTIFICATION_NAME = "Critter Starvation";

				// Token: 0x0400B358 RID: 45912
				public static LocString NOTIFICATION_TOOLTIP = "These critters are starving and will die if not fed soon:";
			}

			// Token: 0x020029E3 RID: 10723
			public class OLD
			{
				// Token: 0x0400B359 RID: 45913
				public static LocString NAME = "Elderly";

				// Token: 0x0400B35A RID: 45914
				public static LocString TOOLTIP = "This sweet ol'critter is over the hill and will pass on in <b>{TimeUntilDeath}</b>";
			}

			// Token: 0x020029E4 RID: 10724
			public class DIVERGENT_WILL_TEND
			{
				// Token: 0x0400B35B RID: 45915
				public static LocString NAME = "Moving to Plant";

				// Token: 0x0400B35C RID: 45916
				public static LocString TOOLTIP = "This critter is off to tend a plant that's caught its attention";
			}

			// Token: 0x020029E5 RID: 10725
			public class DIVERGENT_TENDING
			{
				// Token: 0x0400B35D RID: 45917
				public static LocString NAME = "Plant Tending";

				// Token: 0x0400B35E RID: 45918
				public static LocString TOOLTIP = "This critter is snuggling a plant to help it grow";
			}

			// Token: 0x020029E6 RID: 10726
			public class NOSLEEPSPOT
			{
				// Token: 0x0400B35F RID: 45919
				public static LocString NAME = "Nowhere To Sleep";

				// Token: 0x0400B360 RID: 45920
				public static LocString TOOLTIP = "This critter wants to sleep but can't find a good spot to snuggle up!";
			}

			// Token: 0x020029E7 RID: 10727
			public class PILOTNEEDED
			{
			}

			// Token: 0x020029E8 RID: 10728
			public class ORIGINALPLANTMUTATION
			{
				// Token: 0x0400B361 RID: 45921
				public static LocString NAME = "Original Plant";

				// Token: 0x0400B362 RID: 45922
				public static LocString TOOLTIP = "This is the original, unmutated variant of this species.";
			}

			// Token: 0x020029E9 RID: 10729
			public class UNKNOWNMUTATION
			{
				// Token: 0x0400B363 RID: 45923
				public static LocString NAME = "Unknown Mutation";

				// Token: 0x0400B364 RID: 45924
				public static LocString TOOLTIP = "This seed carries some unexpected genetic markers. Analyze it at the " + UI.FormatAsLink(BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME, "GENETICANALYSISSTATION") + " to learn its secrets.";
			}

			// Token: 0x020029EA RID: 10730
			public class SPECIFICPLANTMUTATION
			{
				// Token: 0x0400B365 RID: 45925
				public static LocString NAME = "Mutant Plant: {MutationName}";

				// Token: 0x0400B366 RID: 45926
				public static LocString TOOLTIP = "This plant is mutated with a genetic variant I call {MutationName}.";
			}

			// Token: 0x020029EB RID: 10731
			public class CROP_TOO_NONRADIATED
			{
				// Token: 0x0400B367 RID: 45927
				public static LocString NAME = "    • Low Radiation Levels";

				// Token: 0x0400B368 RID: 45928
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Growth will resume when ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" requirements are met"
				});
			}

			// Token: 0x020029EC RID: 10732
			public class CROP_TOO_RADIATED
			{
				// Token: 0x0400B369 RID: 45929
				public static LocString NAME = "    • High Radiation Levels";

				// Token: 0x0400B36A RID: 45930
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Growth will resume when ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" requirements are met"
				});
			}

			// Token: 0x020029ED RID: 10733
			public class ELEMENT_GROWTH_GROWING
			{
				// Token: 0x0400B36B RID: 45931
				public static LocString NAME = "Picky Eater: Just Right";

				// Token: 0x0400B36C RID: 45932
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter's ",
					UI.PRE_KEYWORD,
					"Quill Growth",
					UI.PST_KEYWORD,
					" rate is optimal\n\nPreferred food temperature range: {templo}-{temphi}"
				});

				// Token: 0x0400B36D RID: 45933
				public static LocString PREFERRED_TEMP = "Last eaten: {element} at {temperature}";
			}

			// Token: 0x020029EE RID: 10734
			public class ELEMENT_GROWTH_STUNTED
			{
				// Token: 0x0400B36E RID: 45934
				public static LocString NAME = "Picky Eater: {reason}";

				// Token: 0x0400B36F RID: 45935
				public static LocString TOO_HOT = "Too Hot";

				// Token: 0x0400B370 RID: 45936
				public static LocString TOO_COLD = "Too Cold";

				// Token: 0x0400B371 RID: 45937
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter's ",
					UI.PRE_KEYWORD,
					"Quill Growth",
					UI.PST_KEYWORD,
					" rate has slowed because they ate food outside their preferred temperature range\n\nPreferred food temperature range: {templo}-{temphi}"
				});
			}

			// Token: 0x020029EF RID: 10735
			public class ELEMENT_GROWTH_HALTED
			{
				// Token: 0x0400B372 RID: 45938
				public static LocString NAME = "Picky Eater: Hungry";

				// Token: 0x0400B373 RID: 45939
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter's ",
					UI.PRE_KEYWORD,
					"Quill Growth",
					UI.PST_KEYWORD,
					" is halted because they are hungry\n\nPreferred food temperature range: {templo}-{temphi}"
				});
			}

			// Token: 0x020029F0 RID: 10736
			public class ELEMENT_GROWTH_COMPLETE
			{
				// Token: 0x0400B374 RID: 45940
				public static LocString NAME = "Picky Eater: All Done";

				// Token: 0x0400B375 RID: 45941
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter's ",
					UI.PRE_KEYWORD,
					"Tonic Root",
					UI.PST_KEYWORD,
					" quills are fully grown\n\nPreferred food temperature range: {templo}-{temphi}"
				});
			}

			// Token: 0x020029F1 RID: 10737
			public class GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN
			{
				// Token: 0x0400B376 RID: 45942
				public static LocString NAME = "Processing Sample: {percent}";

				// Token: 0x0400B377 RID: 45943
				public static LocString TOOLTIP = "This building is busy analyzing genetic data from a recently scanned specimen\n\nRemaining: {timeleft}";
			}

			// Token: 0x020029F2 RID: 10738
			public class BECKONING
			{
				// Token: 0x0400B378 RID: 45944
				public static LocString NAME = "Mooing";

				// Token: 0x0400B379 RID: 45945
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter is inviting faraway friends to graze on this asteroid's abundant food supply\n\nA new ",
					UI.PRE_KEYWORD,
					"Gassy Moo",
					UI.PST_KEYWORD,
					" will soon arrive"
				});
			}

			// Token: 0x020029F3 RID: 10739
			public class BECKONINGBLOCKED
			{
				// Token: 0x0400B37A RID: 45946
				public static LocString NAME = "Moo-ted";

				// Token: 0x0400B37B RID: 45947
				public static LocString TOOLTIP = "This critter needs a clear view of space in order to invite a friend to this asteroid";
			}

			// Token: 0x020029F4 RID: 10740
			public class MILKFULL
			{
				// Token: 0x0400B37C RID: 45948
				public static LocString NAME = "Udderly Full";

				// Token: 0x0400B37D RID: 45949
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter is full of ",
					UI.PRE_KEYWORD,
					"Brackene",
					UI.PST_KEYWORD,
					" and ready to be milked"
				});
			}
		}

		// Token: 0x02001DEB RID: 7659
		public class STATS
		{
			// Token: 0x020029F5 RID: 10741
			public class HEALTH
			{
				// Token: 0x0400B37E RID: 45950
				public static LocString NAME = "Health";
			}

			// Token: 0x020029F6 RID: 10742
			public class AGE
			{
				// Token: 0x0400B37F RID: 45951
				public static LocString NAME = "Age";

				// Token: 0x0400B380 RID: 45952
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter will die when its ",
					UI.PRE_KEYWORD,
					"Age",
					UI.PST_KEYWORD,
					" reaches its species' maximum lifespan"
				});
			}

			// Token: 0x020029F7 RID: 10743
			public class MATURITY
			{
				// Token: 0x0400B381 RID: 45953
				public static LocString NAME = "Growth Progress";

				// Token: 0x0400B382 RID: 45954
				public static LocString TOOLTIP = "Growth Progress\n\n";

				// Token: 0x0400B383 RID: 45955
				public static LocString TOOLTIP_GROWING = "Predicted Maturation: <b>{0}</b>";

				// Token: 0x0400B384 RID: 45956
				public static LocString TOOLTIP_GROWING_CROP = "Predicted Maturation Time: <b>{0}</b>\nNext harvest occurs in approximately <b>{1}</b>";

				// Token: 0x0400B385 RID: 45957
				public static LocString TOOLTIP_GROWN = "Growth paused while plant awaits harvest";

				// Token: 0x0400B386 RID: 45958
				public static LocString TOOLTIP_STALLED = "Poor conditions have halted this plant's growth";

				// Token: 0x0400B387 RID: 45959
				public static LocString AMOUNT_DESC_FMT = "{0}: {1}\nNext harvest in <b>{2}</b>";

				// Token: 0x0400B388 RID: 45960
				public static LocString GROWING = "Domestic Growth Rate";

				// Token: 0x0400B389 RID: 45961
				public static LocString GROWINGWILD = "Wild Growth Rate";
			}

			// Token: 0x020029F8 RID: 10744
			public class FERTILIZATION
			{
				// Token: 0x0400B38A RID: 45962
				public static LocString NAME = "Fertilization";

				// Token: 0x0400B38B RID: 45963
				public static LocString CONSUME_MODIFIER = "Consuming";

				// Token: 0x0400B38C RID: 45964
				public static LocString ABSORBING_MODIFIER = "Absorbing";
			}

			// Token: 0x020029F9 RID: 10745
			public class DOMESTICATION
			{
				// Token: 0x0400B38D RID: 45965
				public static LocString NAME = "Domestication";

				// Token: 0x0400B38E RID: 45966
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Fully ",
					UI.PRE_KEYWORD,
					"Tame",
					UI.PST_KEYWORD,
					" critters produce more materials than wild ones, and may even provide psychological benefits to my colony\n\nThis critter is <b>{0}</b> domesticated"
				});
			}

			// Token: 0x020029FA RID: 10746
			public class HAPPINESS
			{
				// Token: 0x0400B38F RID: 45967
				public static LocString NAME = "Happiness";

				// Token: 0x0400B390 RID: 45968
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Happiness",
					UI.PST_KEYWORD,
					" increases a critter's productivity and indirectly improves their ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" laying rates\n\nIt also provides the satisfaction in knowing they're living a good little critter life"
				});
			}

			// Token: 0x020029FB RID: 10747
			public class WILDNESS
			{
				// Token: 0x0400B391 RID: 45969
				public static LocString NAME = "Wildness";

				// Token: 0x0400B392 RID: 45970
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"At 0% ",
					UI.PRE_KEYWORD,
					"Wildness",
					UI.PST_KEYWORD,
					" a critter becomes ",
					UI.PRE_KEYWORD,
					"Tame",
					UI.PST_KEYWORD,
					", increasing its ",
					UI.PRE_KEYWORD,
					"Metabolism",
					UI.PST_KEYWORD,
					" and requiring regular care from Duplicants\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill to care for critters"
				});
			}

			// Token: 0x020029FC RID: 10748
			public class FERTILITY
			{
				// Token: 0x0400B393 RID: 45971
				public static LocString NAME = "Reproduction";

				// Token: 0x0400B394 RID: 45972
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"At 100% ",
					UI.PRE_KEYWORD,
					"Reproduction",
					UI.PST_KEYWORD,
					", critters will reach the end of their reproduction cycle and lay a new ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					"\n\nAfter an ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" is laid, ",
					UI.PRE_KEYWORD,
					"Reproduction",
					UI.PST_KEYWORD,
					" is rolled back to 0%"
				});
			}

			// Token: 0x020029FD RID: 10749
			public class BECKONING
			{
				// Token: 0x0400B395 RID: 45973
				public static LocString NAME = "Accu-moo-lation";

				// Token: 0x0400B396 RID: 45974
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"At 100% ",
					UI.PRE_KEYWORD,
					"Accu-moo-lation",
					UI.PST_KEYWORD,
					", a Gassy Moo calls a friend to join them on this asteroid\n\nAfter the new Gassy Moo has landed, ",
					UI.PRE_KEYWORD,
					"Accu-moo-lation",
					UI.PST_KEYWORD,
					" is rolled back to 0%"
				});
			}

			// Token: 0x020029FE RID: 10750
			public class INCUBATION
			{
				// Token: 0x0400B397 RID: 45975
				public static LocString NAME = "Incubation";

				// Token: 0x0400B398 RID: 45976
				public static LocString TOOLTIP = "Eggs hatch into brand new " + UI.FormatAsLink("Critters", "CREATURES") + " at the end of their incubation period";
			}

			// Token: 0x020029FF RID: 10751
			public class VIABILITY
			{
				// Token: 0x0400B399 RID: 45977
				public static LocString NAME = "Viability";

				// Token: 0x0400B39A RID: 45978
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Eggs will lose ",
					UI.PRE_KEYWORD,
					"Viability",
					UI.PST_KEYWORD,
					" over time when exposed to poor environmental conditions\n\nAt 0% ",
					UI.PRE_KEYWORD,
					"Viability",
					UI.PST_KEYWORD,
					" a critter egg will crack and produce a ",
					ITEMS.FOOD.RAWEGG.NAME,
					" and ",
					ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.NAME
				});
			}

			// Token: 0x02002A00 RID: 10752
			public class IRRIGATION
			{
				// Token: 0x0400B39B RID: 45979
				public static LocString NAME = "Irrigation";

				// Token: 0x0400B39C RID: 45980
				public static LocString CONSUME_MODIFIER = "Consuming";

				// Token: 0x0400B39D RID: 45981
				public static LocString ABSORBING_MODIFIER = "Absorbing";
			}

			// Token: 0x02002A01 RID: 10753
			public class ILLUMINATION
			{
				// Token: 0x0400B39E RID: 45982
				public static LocString NAME = "Illumination";
			}

			// Token: 0x02002A02 RID: 10754
			public class THERMALCONDUCTIVITYBARRIER
			{
				// Token: 0x0400B39F RID: 45983
				public static LocString NAME = "Thermal Conductivity Barrier";

				// Token: 0x0400B3A0 RID: 45984
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Thick ",
					UI.PRE_KEYWORD,
					"Conductivity Barriers",
					UI.PST_KEYWORD,
					" increase the time it takes an object to heat up or cool down"
				});
			}

			// Token: 0x02002A03 RID: 10755
			public class ROT
			{
				// Token: 0x0400B3A1 RID: 45985
				public static LocString NAME = "Freshness";

				// Token: 0x0400B3A2 RID: 45986
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Food items become stale at fifty percent ",
					UI.PRE_KEYWORD,
					"Freshness",
					UI.PST_KEYWORD,
					", and rot at zero percent"
				});
			}

			// Token: 0x02002A04 RID: 10756
			public class SCALEGROWTH
			{
				// Token: 0x0400B3A3 RID: 45987
				public static LocString NAME = "Scale Growth";

				// Token: 0x0400B3A4 RID: 45988
				public static LocString TOOLTIP = "The amount of time required for this critter to regrow its scales";
			}

			// Token: 0x02002A05 RID: 10757
			public class MILKPRODUCTION
			{
				// Token: 0x0400B3A5 RID: 45989
				public static LocString NAME = "Brackene Supply";

				// Token: 0x0400B3A6 RID: 45990
				public static LocString TOOLTIP = "The amount of time required for this critter to replenish its natural reserves of " + UI.PRE_KEYWORD + "Brackene" + UI.PST_KEYWORD;
			}

			// Token: 0x02002A06 RID: 10758
			public class ELEMENTGROWTH
			{
				// Token: 0x0400B3A7 RID: 45991
				public static LocString NAME = "Quill Growth";

				// Token: 0x0400B3A8 RID: 45992
				public static LocString TOOLTIP = "The amount of time required for this critter to regrow its " + UI.PRE_KEYWORD + "Tonic Root" + UI.PST_KEYWORD;
			}

			// Token: 0x02002A07 RID: 10759
			public class AIRPRESSURE
			{
				// Token: 0x0400B3A9 RID: 45993
				public static LocString NAME = "Air Pressure";

				// Token: 0x0400B3AA RID: 45994
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The average ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" density of the air surrounding this plant"
				});
			}
		}

		// Token: 0x02001DEC RID: 7660
		public class ATTRIBUTES
		{
			// Token: 0x02002A08 RID: 10760
			public class INCUBATIONDELTA
			{
				// Token: 0x0400B3AB RID: 45995
				public static LocString NAME = "Incubation Rate";

				// Token: 0x0400B3AC RID: 45996
				public static LocString DESC = "";
			}

			// Token: 0x02002A09 RID: 10761
			public class POWERCHARGEDELTA
			{
				// Token: 0x0400B3AD RID: 45997
				public static LocString NAME = "Power Charge Loss Rate";

				// Token: 0x0400B3AE RID: 45998
				public static LocString DESC = "";
			}

			// Token: 0x02002A0A RID: 10762
			public class VIABILITYDELTA
			{
				// Token: 0x0400B3AF RID: 45999
				public static LocString NAME = "Viability Loss Rate";

				// Token: 0x0400B3B0 RID: 46000
				public static LocString DESC = "";
			}

			// Token: 0x02002A0B RID: 10763
			public class SCALEGROWTHDELTA
			{
				// Token: 0x0400B3B1 RID: 46001
				public static LocString NAME = "Scale Growth";

				// Token: 0x0400B3B2 RID: 46002
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Determines how long this ",
					UI.PRE_KEYWORD,
					"Critter's",
					UI.PST_KEYWORD,
					" scales will take to grow back."
				});
			}

			// Token: 0x02002A0C RID: 10764
			public class MILKPRODUCTIONDELTA
			{
				// Token: 0x0400B3B3 RID: 46003
				public static LocString NAME = "Brackene Supply";

				// Token: 0x0400B3B4 RID: 46004
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Determines how long this ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" takes to replenish its natural supply of ",
					UI.PRE_KEYWORD,
					"Brackene",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02002A0D RID: 10765
			public class WILDNESSDELTA
			{
				// Token: 0x0400B3B5 RID: 46005
				public static LocString NAME = "Wildness";

				// Token: 0x0400B3B6 RID: 46006
				public static LocString DESC = string.Concat(new string[]
				{
					"Wild creatures can survive on fewer ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD,
					" than domesticated ones."
				});
			}

			// Token: 0x02002A0E RID: 10766
			public class FERTILITYDELTA
			{
				// Token: 0x0400B3B7 RID: 46007
				public static LocString NAME = "Reproduction Rate";

				// Token: 0x0400B3B8 RID: 46008
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the amount of time needed for a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to lay new ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02002A0F RID: 10767
			public class MATURITYDELTA
			{
				// Token: 0x0400B3B9 RID: 46009
				public static LocString NAME = "Growth Speed";

				// Token: 0x0400B3BA RID: 46010
				public static LocString DESC = "Determines the amount of time needed to reach maturation.";
			}

			// Token: 0x02002A10 RID: 10768
			public class MATURITYMAX
			{
				// Token: 0x0400B3BB RID: 46011
				public static LocString NAME = "Life Cycle";

				// Token: 0x0400B3BC RID: 46012
				public static LocString DESC = "The amount of time it takes this plant to grow.";
			}

			// Token: 0x02002A11 RID: 10769
			public class ROTDELTA
			{
				// Token: 0x0400B3BD RID: 46013
				public static LocString NAME = "Freshness";

				// Token: 0x0400B3BE RID: 46014
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Food items become stale at fifty percent ",
					UI.PRE_KEYWORD,
					"Freshness",
					UI.PST_KEYWORD,
					", and rot at zero percent"
				});
			}

			// Token: 0x02002A12 RID: 10770
			public class BECKONINGDELTA
			{
				// Token: 0x0400B3BF RID: 46015
				public static LocString NAME = "Accu-moo-lation";

				// Token: 0x0400B3C0 RID: 46016
				public static LocString DESC = "Accu-moo-lation increases when this critter eats.";
			}
		}

		// Token: 0x02001DED RID: 7661
		public class MODIFIERS
		{
			// Token: 0x02002A13 RID: 10771
			public class DOMESTICATION_INCREASING
			{
				// Token: 0x0400B3C1 RID: 46017
				public static LocString NAME = "Happiness Increasing";

				// Token: 0x0400B3C2 RID: 46018
				public static LocString TOOLTIP = "This critter is very happy its needs are being met";
			}

			// Token: 0x02002A14 RID: 10772
			public class DOMESTICATION_DECREASING
			{
				// Token: 0x0400B3C3 RID: 46019
				public static LocString NAME = "Happiness Decreasing";

				// Token: 0x0400B3C4 RID: 46020
				public static LocString TOOLTIP = "Unfavorable conditions are making this critter unhappy";
			}

			// Token: 0x02002A15 RID: 10773
			public class BASE_FERTILITY
			{
				// Token: 0x0400B3C5 RID: 46021
				public static LocString NAME = "Base Reproduction";

				// Token: 0x0400B3C6 RID: 46022
				public static LocString TOOLTIP = "This is the base speed with which critters produce new " + UI.PRE_KEYWORD + "Eggs" + UI.PST_KEYWORD;
			}

			// Token: 0x02002A16 RID: 10774
			public class BASE_INCUBATION_RATE
			{
				// Token: 0x0400B3C7 RID: 46023
				public static LocString NAME = "Base Incubation Rate";
			}

			// Token: 0x02002A17 RID: 10775
			public class BASE_PRODUCTION_RATE
			{
				// Token: 0x0400B3C8 RID: 46024
				public static LocString NAME = "Base production rate";
			}

			// Token: 0x02002A18 RID: 10776
			public class SCALE_GROWTH_RATE
			{
				// Token: 0x0400B3C9 RID: 46025
				public static LocString NAME = "Scale Regrowth Rate";
			}

			// Token: 0x02002A19 RID: 10777
			public class ELEMENT_GROWTH_RATE
			{
				// Token: 0x0400B3CA RID: 46026
				public static LocString NAME = "Quill Regrowth Rate";
			}

			// Token: 0x02002A1A RID: 10778
			public class INCUBATOR_SONG
			{
				// Token: 0x0400B3CB RID: 46027
				public static LocString NAME = "Lullabied";

				// Token: 0x0400B3CC RID: 46028
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This egg was recently sung to by a kind Duplicant\n\nIncreased ",
					UI.PRE_KEYWORD,
					"Incubation Rate",
					UI.PST_KEYWORD,
					"\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill to sing to eggs"
				});
			}

			// Token: 0x02002A1B RID: 10779
			public class EGGHUG
			{
				// Token: 0x0400B3CD RID: 46029
				public static LocString NAME = "Cuddled";

				// Token: 0x0400B3CE RID: 46030
				public static LocString TOOLTIP = "This egg was recently hugged by an affectionate critter\n\nIncreased " + UI.PRE_KEYWORD + "Incubation Rate" + UI.PST_KEYWORD;
			}

			// Token: 0x02002A1C RID: 10780
			public class HUGGINGFRENZY
			{
				// Token: 0x0400B3CF RID: 46031
				public static LocString NAME = "Hugging Spree";

				// Token: 0x0400B3D0 RID: 46032
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter was recently hugged by a Duplicant and is feeling extra affectionate\n\nWhile in this state, it hugs ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" more frequently"
				});
			}

			// Token: 0x02002A1D RID: 10781
			public class INCUBATING
			{
				// Token: 0x0400B3D1 RID: 46033
				public static LocString NAME = "Incubating";

				// Token: 0x0400B3D2 RID: 46034
				public static LocString TOOLTIP = "This egg is happily incubating";
			}

			// Token: 0x02002A1E RID: 10782
			public class INCUBATING_SUPPRESSED
			{
				// Token: 0x0400B3D3 RID: 46035
				public static LocString NAME = "Growth Suppressed";

				// Token: 0x0400B3D4 RID: 46036
				public static LocString TOOLTIP = "Environmental conditions are preventing this egg from developing\n\nIt will not hatch if current conditions continue";
			}

			// Token: 0x02002A1F RID: 10783
			public class GOTMILK
			{
				// Token: 0x0400B3D5 RID: 46037
				public static LocString NAME = "Hydrated";

				// Token: 0x0400B3D6 RID: 46038
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter recently drank ",
					UI.PRE_KEYWORD,
					"Brackene",
					UI.PST_KEYWORD,
					"\n\nIt doesn't mind overcrowding quite as much right now"
				});
			}

			// Token: 0x02002A20 RID: 10784
			public class RANCHED
			{
				// Token: 0x0400B3D7 RID: 46039
				public static LocString NAME = "Groomed";

				// Token: 0x0400B3D8 RID: 46040
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter has recently been attended to by a kind Duplicant\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill to care for critters"
				});
			}

			// Token: 0x02002A21 RID: 10785
			public class HAPPY
			{
				// Token: 0x0400B3D9 RID: 46041
				public static LocString NAME = "Happy";

				// Token: 0x0400B3DA RID: 46042
				public static LocString TOOLTIP = "This critter's in high spirits because its needs are being adequately met\n\nIt will produce more materials as a result";
			}

			// Token: 0x02002A22 RID: 10786
			public class UNHAPPY
			{
				// Token: 0x0400B3DB RID: 46043
				public static LocString NAME = "Glum";

				// Token: 0x0400B3DC RID: 46044
				public static LocString TOOLTIP = "This critter's feeling down because its needs aren't being met\n\nIt will produce less materials as a result";
			}

			// Token: 0x02002A23 RID: 10787
			public class ATE_FROM_FEEDER
			{
				// Token: 0x0400B3DD RID: 46045
				public static LocString NAME = "Ate From Feeder";

				// Token: 0x0400B3DE RID: 46046
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter is getting more ",
					UI.PRE_KEYWORD,
					"Tame",
					UI.PST_KEYWORD,
					" because it ate from a feeder"
				});
			}

			// Token: 0x02002A24 RID: 10788
			public class WILD
			{
				// Token: 0x0400B3DF RID: 46047
				public static LocString NAME = "Wild";

				// Token: 0x0400B3E0 RID: 46048
				public static LocString TOOLTIP = "This critter is wild";
			}

			// Token: 0x02002A25 RID: 10789
			public class AGE
			{
				// Token: 0x0400B3E1 RID: 46049
				public static LocString NAME = "Aging";

				// Token: 0x0400B3E2 RID: 46050
				public static LocString TOOLTIP = "Time takes its toll on all things";
			}

			// Token: 0x02002A26 RID: 10790
			public class BABY
			{
				// Token: 0x0400B3E3 RID: 46051
				public static LocString NAME = "Tiny Baby!";

				// Token: 0x0400B3E4 RID: 46052
				public static LocString TOOLTIP = "This critter will grow into an adult as it ages and becomes wise to the ways of the world";
			}

			// Token: 0x02002A27 RID: 10791
			public class TAME
			{
				// Token: 0x0400B3E5 RID: 46053
				public static LocString NAME = "Tame";

				// Token: 0x0400B3E6 RID: 46054
				public static LocString TOOLTIP = "This critter is " + UI.PRE_KEYWORD + "Tame" + UI.PST_KEYWORD;
			}

			// Token: 0x02002A28 RID: 10792
			public class OUT_OF_CALORIES
			{
				// Token: 0x0400B3E7 RID: 46055
				public static LocString NAME = "Starving";

				// Token: 0x0400B3E8 RID: 46056
				public static LocString TOOLTIP = "Get this critter something to eat!";
			}

			// Token: 0x02002A29 RID: 10793
			public class FUTURE_OVERCROWDED
			{
				// Token: 0x0400B3E9 RID: 46057
				public static LocString NAME = "Cramped";

				// Token: 0x0400B3EA RID: 46058
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" will become overcrowded once all nearby ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" hatch\n\nThe selected critter has slowed its ",
					UI.PRE_KEYWORD,
					"Reproduction",
					UI.PST_KEYWORD,
					" to prevent further overpopulation"
				});
			}

			// Token: 0x02002A2A RID: 10794
			public class OVERCROWDED
			{
				// Token: 0x0400B3EB RID: 46059
				public static LocString NAME = "Overcrowded";

				// Token: 0x0400B3EC RID: 46060
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter isn't comfortable with so many other critters in a ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" of this size"
				});

				// Token: 0x0400B3ED RID: 46061
				public static LocString FISHTOOLTIP = "This critter is uncomfortable with either the size of this pool, or the number of other critters sharing it";
			}

			// Token: 0x02002A2B RID: 10795
			public class CONFINED
			{
				// Token: 0x0400B3EE RID: 46062
				public static LocString NAME = "Confined";

				// Token: 0x0400B3EF RID: 46063
				public static LocString TOOLTIP = "This critter is trapped inside a door, tile, or confined space\n\nSounds uncomfortable!";
			}

			// Token: 0x02002A2C RID: 10796
			public class DIVERGENTPLANTTENDED
			{
				// Token: 0x0400B3F0 RID: 46064
				public static LocString NAME = "Sweetle Tending";

				// Token: 0x0400B3F1 RID: 46065
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.NAME,
					" rubbed against this ",
					UI.PRE_KEYWORD,
					"Plant",
					UI.PST_KEYWORD,
					" for a tiny growth boost"
				});
			}

			// Token: 0x02002A2D RID: 10797
			public class DIVERGENTPLANTTENDEDWORM
			{
				// Token: 0x0400B3F2 RID: 46066
				public static LocString NAME = "Grubgrub Rub";

				// Token: 0x0400B3F3 RID: 46067
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.NAME,
					" rubbed against this ",
					UI.PRE_KEYWORD,
					"Plant",
					UI.PST_KEYWORD,
					", dramatically boosting growth"
				});
			}

			// Token: 0x02002A2E RID: 10798
			public class MOOWELLFED
			{
				// Token: 0x0400B3F4 RID: 46068
				public static LocString NAME = "Welcoming Moo'd";

				// Token: 0x0400B3F5 RID: 46069
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This critter's recent meal is boosting their ",
					UI.PRE_KEYWORD,
					"Brackene",
					UI.PST_KEYWORD,
					" supply and making them wish all their friends were here\n\nThey're thinking about calling a new Gassy Moo to this asteroid"
				});
			}
		}

		// Token: 0x02001DEE RID: 7662
		public class FERTILITY_MODIFIERS
		{
			// Token: 0x02002A2F RID: 10799
			public class DIET
			{
				// Token: 0x0400B3F6 RID: 46070
				public static LocString NAME = "Diet";

				// Token: 0x0400B3F7 RID: 46071
				public static LocString DESC = "Eats: {0}";
			}

			// Token: 0x02002A30 RID: 10800
			public class NEARBY_CREATURE
			{
				// Token: 0x0400B3F8 RID: 46072
				public static LocString NAME = "Nearby Critters";

				// Token: 0x0400B3F9 RID: 46073
				public static LocString DESC = "Penned with: {0}";
			}

			// Token: 0x02002A31 RID: 10801
			public class NEARBY_CREATURE_NEG
			{
				// Token: 0x0400B3FA RID: 46074
				public static LocString NAME = "No Nearby Critters";

				// Token: 0x0400B3FB RID: 46075
				public static LocString DESC = "Not penned with: {0}";
			}

			// Token: 0x02002A32 RID: 10802
			public class TEMPERATURE
			{
				// Token: 0x0400B3FC RID: 46076
				public static LocString NAME = "Temperature";

				// Token: 0x0400B3FD RID: 46077
				public static LocString DESC = "Body temperature: Between {0} and {1}";
			}

			// Token: 0x02002A33 RID: 10803
			public class CROPTENDING
			{
				// Token: 0x0400B3FE RID: 46078
				public static LocString NAME = "Crop Tending";

				// Token: 0x0400B3FF RID: 46079
				public static LocString DESC = "Tends to: {0}";
			}

			// Token: 0x02002A34 RID: 10804
			public class LIVING_IN_ELEMENT
			{
				// Token: 0x0400B400 RID: 46080
				public static LocString NAME = "Habitat";

				// Token: 0x0400B401 RID: 46081
				public static LocString DESC = "Dwells in {0}";

				// Token: 0x0400B402 RID: 46082
				public static LocString UNBREATHABLE = "Dwells in unbreathable" + UI.FormatAsLink("Gas", "UNBREATHABLE");

				// Token: 0x0400B403 RID: 46083
				public static LocString LIQUID = "Dwells in " + UI.FormatAsLink("Liquid", "LIQUID");
			}
		}
	}
}
