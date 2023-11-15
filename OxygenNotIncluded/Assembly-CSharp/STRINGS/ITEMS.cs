using System;

namespace STRINGS
{
	// Token: 0x02000DB2 RID: 3506
	public class ITEMS
	{
		// Token: 0x02001E04 RID: 7684
		public class PILLS
		{
			// Token: 0x02002E22 RID: 11810
			public class PLACEBO
			{
				// Token: 0x0400BDB3 RID: 48563
				public static LocString NAME = "Placebo";

				// Token: 0x0400BDB4 RID: 48564
				public static LocString DESC = "A general, all-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".\n\nThe less one knows about it, the better it works.";

				// Token: 0x0400BDB5 RID: 48565
				public static LocString RECIPEDESC = "All-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".";
			}

			// Token: 0x02002E23 RID: 11811
			public class BASICBOOSTER
			{
				// Token: 0x0400BDB6 RID: 48566
				public static LocString NAME = "Vitamin Chews";

				// Token: 0x0400BDB7 RID: 48567
				public static LocString DESC = "Minorly reduces the chance of becoming sick.";

				// Token: 0x0400BDB8 RID: 48568
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A supplement that minorly reduces the chance of contracting a ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nMust be taken daily."
				});
			}

			// Token: 0x02002E24 RID: 11812
			public class INTERMEDIATEBOOSTER
			{
				// Token: 0x0400BDB9 RID: 48569
				public static LocString NAME = "Immuno Booster";

				// Token: 0x0400BDBA RID: 48570
				public static LocString DESC = "Significantly reduces the chance of becoming sick.";

				// Token: 0x0400BDBB RID: 48571
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A supplement that significantly reduces the chance of contracting a ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nMust be taken daily."
				});
			}

			// Token: 0x02002E25 RID: 11813
			public class ANTIHISTAMINE
			{
				// Token: 0x0400BDBC RID: 48572
				public static LocString NAME = "Allergy Medication";

				// Token: 0x0400BDBD RID: 48573
				public static LocString DESC = "Suppresses and prevents allergic reactions.";

				// Token: 0x0400BDBE RID: 48574
				public static LocString RECIPEDESC = "A strong antihistamine Duplicants can take to halt an allergic reaction. " + ITEMS.PILLS.ANTIHISTAMINE.NAME + " will also prevent further reactions from occurring for a short time after ingestion.";
			}

			// Token: 0x02002E26 RID: 11814
			public class BASICCURE
			{
				// Token: 0x0400BDBF RID: 48575
				public static LocString NAME = "Curative Tablet";

				// Token: 0x0400BDC0 RID: 48576
				public static LocString DESC = "A simple, easy-to-take remedy for minor germ-based diseases.";

				// Token: 0x0400BDC1 RID: 48577
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Duplicants can take this to cure themselves of minor ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nCurative Tablets are very effective against ",
					UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
					"."
				});
			}

			// Token: 0x02002E27 RID: 11815
			public class INTERMEDIATECURE
			{
				// Token: 0x0400BDC2 RID: 48578
				public static LocString NAME = "Medical Pack";

				// Token: 0x0400BDC3 RID: 48579
				public static LocString DESC = "A doctor-administered cure for moderate ailments.";

				// Token: 0x0400BDC4 RID: 48580
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A doctor-administered cure for moderate ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					". ",
					ITEMS.PILLS.INTERMEDIATECURE.NAME,
					"s are very effective against ",
					UI.FormatAsLink("Slimelung", "SLIMESICKNESS"),
					".\n\nMust be administered by a Duplicant with the ",
					DUPLICANTS.ROLES.MEDIC.NAME,
					" Skill."
				});
			}

			// Token: 0x02002E28 RID: 11816
			public class ADVANCEDCURE
			{
				// Token: 0x0400BDC5 RID: 48581
				public static LocString NAME = "Serum Vial";

				// Token: 0x0400BDC6 RID: 48582
				public static LocString DESC = "A doctor-administered cure for severe ailments.";

				// Token: 0x0400BDC7 RID: 48583
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"An extremely powerful medication created to treat severe ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					". ",
					ITEMS.PILLS.ADVANCEDCURE.NAME,
					" is very effective against ",
					UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
					".\n\nMust be administered by a Duplicant with the ",
					DUPLICANTS.ROLES.SENIOR_MEDIC.NAME,
					" Skill."
				});
			}

			// Token: 0x02002E29 RID: 11817
			public class BASICRADPILL
			{
				// Token: 0x0400BDC8 RID: 48584
				public static LocString NAME = "Basic Rad Pill";

				// Token: 0x0400BDC9 RID: 48585
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400BDCA RID: 48586
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}

			// Token: 0x02002E2A RID: 11818
			public class INTERMEDIATERADPILL
			{
				// Token: 0x0400BDCB RID: 48587
				public static LocString NAME = "Intermediate Rad Pill";

				// Token: 0x0400BDCC RID: 48588
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400BDCD RID: 48589
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}
		}

		// Token: 0x02001E05 RID: 7685
		public class FOOD
		{
			// Token: 0x040089F7 RID: 35319
			public static LocString COMPOST = "Compost";

			// Token: 0x02002E2B RID: 11819
			public class FOODSPLAT
			{
				// Token: 0x0400BDCE RID: 48590
				public static LocString NAME = "Food Splatter";

				// Token: 0x0400BDCF RID: 48591
				public static LocString DESC = "Food smeared on the wall from a recent Food Fight";
			}

			// Token: 0x02002E2C RID: 11820
			public class BURGER
			{
				// Token: 0x0400BDD0 RID: 48592
				public static LocString NAME = UI.FormatAsLink("Frost Burger", "BURGER");

				// Token: 0x0400BDD1 RID: 48593
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					".\n\nIt's the only burger best served cold."
				});

				// Token: 0x0400BDD2 RID: 48594
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					"."
				});
			}

			// Token: 0x02002E2D RID: 11821
			public class FIELDRATION
			{
				// Token: 0x0400BDD3 RID: 48595
				public static LocString NAME = UI.FormatAsLink("Nutrient Bar", "FIELDRATION");

				// Token: 0x0400BDD4 RID: 48596
				public static LocString DESC = "A nourishing nutrient paste, sandwiched between thin wafer layers.";
			}

			// Token: 0x02002E2E RID: 11822
			public class MUSHBAR
			{
				// Token: 0x0400BDD5 RID: 48597
				public static LocString NAME = UI.FormatAsLink("Mush Bar", "MUSHBAR");

				// Token: 0x0400BDD6 RID: 48598
				public static LocString DESC = "An edible, putrefied mudslop.\n\nMush Bars are preferable to starvation, but only just barely.";

				// Token: 0x0400BDD7 RID: 48599
				public static LocString RECIPEDESC = "An edible, putrefied mudslop.\n\n" + ITEMS.FOOD.MUSHBAR.NAME + "s are preferable to starvation, but only just barely.";
			}

			// Token: 0x02002E2F RID: 11823
			public class MUSHROOMWRAP
			{
				// Token: 0x0400BDD8 RID: 48600
				public static LocString NAME = UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP");

				// Token: 0x0400BDD9 RID: 48601
				public static LocString DESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nIt has an earthy flavor punctuated by a refreshing crunch."
				});

				// Token: 0x0400BDDA RID: 48602
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});
			}

			// Token: 0x02002E30 RID: 11824
			public class MICROWAVEDLETTUCE
			{
				// Token: 0x0400BDDB RID: 48603
				public static LocString NAME = UI.FormatAsLink("Microwaved Lettuce", "MICROWAVEDLETTUCE");

				// Token: 0x0400BDDC RID: 48604
				public static LocString DESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";

				// Token: 0x0400BDDD RID: 48605
				public static LocString RECIPEDESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x02002E31 RID: 11825
			public class GAMMAMUSH
			{
				// Token: 0x0400BDDE RID: 48606
				public static LocString NAME = UI.FormatAsLink("Gamma Mush", "GAMMAMUSH");

				// Token: 0x0400BDDF RID: 48607
				public static LocString DESC = "A disturbingly delicious mixture of irradiated dirt and water.";

				// Token: 0x0400BDE0 RID: 48608
				public static LocString RECIPEDESC = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR") + " reheated in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x02002E32 RID: 11826
			public class FRUITCAKE
			{
				// Token: 0x0400BDE1 RID: 48609
				public static LocString NAME = UI.FormatAsLink("Berry Sludge", "FRUITCAKE");

				// Token: 0x0400BDE2 RID: 48610
				public static LocString DESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.\n\nIts aggressive, overbearing sweetness can leave the tongue feeling temporarily numb.";

				// Token: 0x0400BDE3 RID: 48611
				public static LocString RECIPEDESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.";
			}

			// Token: 0x02002E33 RID: 11827
			public class POPCORN
			{
				// Token: 0x0400BDE4 RID: 48612
				public static LocString NAME = UI.FormatAsLink("Popcorn", "POPCORN");

				// Token: 0x0400BDE5 RID: 48613
				public static LocString DESC = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " popped in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".\n\nCompletely devoid of any fancy flavorings.";

				// Token: 0x0400BDE6 RID: 48614
				public static LocString RECIPEDESC = "Gamma-radiated " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + ".";
			}

			// Token: 0x02002E34 RID: 11828
			public class SUSHI
			{
				// Token: 0x0400BDE7 RID: 48615
				public static LocString NAME = UI.FormatAsLink("Sushi", "SUSHI");

				// Token: 0x0400BDE8 RID: 48616
				public static LocString DESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nWhile the salt of the lettuce may initially overpower the flavor, a keen palate can discern the subtle sweetness of the fillet beneath."
				});

				// Token: 0x0400BDE9 RID: 48617
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});
			}

			// Token: 0x02002E35 RID: 11829
			public class HATCHEGG
			{
				// Token: 0x0400BDEA RID: 48618
				public static LocString NAME = CREATURES.SPECIES.HATCH.EGG_NAME;

				// Token: 0x0400BDEB RID: 48619
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Hatch", "HATCH"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Hatchling", "HATCH"),
					"."
				});

				// Token: 0x0400BDEC RID: 48620
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Hatch", "HATCH") + ".";
			}

			// Token: 0x02002E36 RID: 11830
			public class DRECKOEGG
			{
				// Token: 0x0400BDED RID: 48621
				public static LocString NAME = CREATURES.SPECIES.DRECKO.EGG_NAME;

				// Token: 0x0400BDEE RID: 48622
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Drecko", "DRECKO"),
					".\n\nIf incubated, it will hatch into a new ",
					UI.FormatAsLink("Drecklet", "DRECKO"),
					"."
				});

				// Token: 0x0400BDEF RID: 48623
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Drecko", "DRECKO") + ".";
			}

			// Token: 0x02002E37 RID: 11831
			public class LIGHTBUGEGG
			{
				// Token: 0x0400BDF0 RID: 48624
				public static LocString NAME = CREATURES.SPECIES.LIGHTBUG.EGG_NAME;

				// Token: 0x0400BDF1 RID: 48625
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Shine Bug", "LIGHTBUG"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Shine Nymph", "LIGHTBUG"),
					"."
				});

				// Token: 0x0400BDF2 RID: 48626
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Shine Bug", "LIGHTBUG") + ".";
			}

			// Token: 0x02002E38 RID: 11832
			public class LETTUCE
			{
				// Token: 0x0400BDF3 RID: 48627
				public static LocString NAME = UI.FormatAsLink("Lettuce", "LETTUCE");

				// Token: 0x0400BDF4 RID: 48628
				public static LocString DESC = "Crunchy, slightly salty leaves from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + " plant.";

				// Token: 0x0400BDF5 RID: 48629
				public static LocString RECIPEDESC = "Edible roughage from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + ".";
			}

			// Token: 0x02002E39 RID: 11833
			public class PASTA
			{
				// Token: 0x0400BDF6 RID: 48630
				public static LocString NAME = UI.FormatAsLink("Pasta", "PASTA");

				// Token: 0x0400BDF7 RID: 48631
				public static LocString DESC = "pasta made from egg and wheat";

				// Token: 0x0400BDF8 RID: 48632
				public static LocString RECIPEDESC = "pasta made from egg and wheat";
			}

			// Token: 0x02002E3A RID: 11834
			public class PANCAKES
			{
				// Token: 0x0400BDF9 RID: 48633
				public static LocString NAME = UI.FormatAsLink("Soufflé Pancakes", "PANCAKES");

				// Token: 0x0400BDFA RID: 48634
				public static LocString DESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					".\n\nThey're so thick!"
				});

				// Token: 0x0400BDFB RID: 48635
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					"."
				});
			}

			// Token: 0x02002E3B RID: 11835
			public class OILFLOATEREGG
			{
				// Token: 0x0400BDFC RID: 48636
				public static LocString NAME = CREATURES.SPECIES.OILFLOATER.EGG_NAME;

				// Token: 0x0400BDFD RID: 48637
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Slickster", "OILFLOATER"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Slickster Larva", "OILFLOATER"),
					"."
				});

				// Token: 0x0400BDFE RID: 48638
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Slickster", "OILFLOATER") + ".";
			}

			// Token: 0x02002E3C RID: 11836
			public class PUFTEGG
			{
				// Token: 0x0400BDFF RID: 48639
				public static LocString NAME = CREATURES.SPECIES.PUFT.EGG_NAME;

				// Token: 0x0400BE00 RID: 48640
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Puft", "PUFT"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Puftlet", "PUFT"),
					"."
				});

				// Token: 0x0400BE01 RID: 48641
				public static LocString RECIPEDESC = "An egg laid by a " + CREATURES.SPECIES.PUFT.NAME + ".";
			}

			// Token: 0x02002E3D RID: 11837
			public class FISHMEAT
			{
				// Token: 0x0400BE02 RID: 48642
				public static LocString NAME = UI.FormatAsLink("Pacu Fillet", "FISHMEAT");

				// Token: 0x0400BE03 RID: 48643
				public static LocString DESC = "An uncooked fillet from a very dead " + CREATURES.SPECIES.PACU.NAME + ". Yum!";
			}

			// Token: 0x02002E3E RID: 11838
			public class MEAT
			{
				// Token: 0x0400BE04 RID: 48644
				public static LocString NAME = UI.FormatAsLink("Meat", "MEAT");

				// Token: 0x0400BE05 RID: 48645
				public static LocString DESC = "Uncooked meat from a very dead critter. Yum!";
			}

			// Token: 0x02002E3F RID: 11839
			public class PLANTMEAT
			{
				// Token: 0x0400BE06 RID: 48646
				public static LocString NAME = UI.FormatAsLink("Plant Meat", "PLANTMEAT");

				// Token: 0x0400BE07 RID: 48647
				public static LocString DESC = "Planty plant meat from a plant. How nice!";
			}

			// Token: 0x02002E40 RID: 11840
			public class SHELLFISHMEAT
			{
				// Token: 0x0400BE08 RID: 48648
				public static LocString NAME = UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT");

				// Token: 0x0400BE09 RID: 48649
				public static LocString DESC = "An uncooked chunk of very dead " + CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.NAME + ". Yum!";
			}

			// Token: 0x02002E41 RID: 11841
			public class MUSHROOM
			{
				// Token: 0x0400BE0A RID: 48650
				public static LocString NAME = UI.FormatAsLink("Mushroom", "MUSHROOM");

				// Token: 0x0400BE0B RID: 48651
				public static LocString DESC = "An edible, flavorless fungus that grew in the dark.";
			}

			// Token: 0x02002E42 RID: 11842
			public class COOKEDFISH
			{
				// Token: 0x0400BE0C RID: 48652
				public static LocString NAME = UI.FormatAsLink("Cooked Seafood", "COOKEDFISH");

				// Token: 0x0400BE0D RID: 48653
				public static LocString DESC = "A cooked piece of freshly caught aquatic critter.\n\nUnsurprisingly, it tastes a bit fishy.";

				// Token: 0x0400BE0E RID: 48654
				public static LocString RECIPEDESC = "A cooked piece of freshly caught aquatic critter.";
			}

			// Token: 0x02002E43 RID: 11843
			public class COOKEDMEAT
			{
				// Token: 0x0400BE0F RID: 48655
				public static LocString NAME = UI.FormatAsLink("Barbeque", "COOKEDMEAT");

				// Token: 0x0400BE10 RID: 48656
				public static LocString DESC = "The cooked meat of a defeated critter.\n\nIt has a delightful smoky aftertaste.";

				// Token: 0x0400BE11 RID: 48657
				public static LocString RECIPEDESC = "The cooked meat of a defeated critter.";
			}

			// Token: 0x02002E44 RID: 11844
			public class PICKLEDMEAL
			{
				// Token: 0x0400BE12 RID: 48658
				public static LocString NAME = UI.FormatAsLink("Pickled Meal", "PICKLEDMEAL");

				// Token: 0x0400BE13 RID: 48659
				public static LocString DESC = "Meal Lice preserved in vinegar.\n\nIt's a rarely acquired taste.";

				// Token: 0x0400BE14 RID: 48660
				public static LocString RECIPEDESC = ITEMS.FOOD.BASICPLANTFOOD.NAME + " regrettably preserved in vinegar.";
			}

			// Token: 0x02002E45 RID: 11845
			public class FRIEDMUSHBAR
			{
				// Token: 0x0400BE15 RID: 48661
				public static LocString NAME = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR");

				// Token: 0x0400BE16 RID: 48662
				public static LocString DESC = "Deep fried, solidified mudslop.\n\nThe inside is almost completely uncooked, despite the crunch on the outside.";

				// Token: 0x0400BE17 RID: 48663
				public static LocString RECIPEDESC = "Deep fried, solidified mudslop.";
			}

			// Token: 0x02002E46 RID: 11846
			public class RAWEGG
			{
				// Token: 0x0400BE18 RID: 48664
				public static LocString NAME = UI.FormatAsLink("Raw Egg", "RAWEGG");

				// Token: 0x0400BE19 RID: 48665
				public static LocString DESC = "A raw Egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.\n\nIt will never hatch.";

				// Token: 0x0400BE1A RID: 48666
				public static LocString RECIPEDESC = "A raw egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.";
			}

			// Token: 0x02002E47 RID: 11847
			public class COOKEDEGG
			{
				// Token: 0x0400BE1B RID: 48667
				public static LocString NAME = UI.FormatAsLink("Omelette", "COOKEDEGG");

				// Token: 0x0400BE1C RID: 48668
				public static LocString DESC = "Fluffed and folded Egg innards.\n\nIt turns out you do, in fact, have to break a few eggs to make it.";

				// Token: 0x0400BE1D RID: 48669
				public static LocString RECIPEDESC = "Fluffed and folded egg innards.";
			}

			// Token: 0x02002E48 RID: 11848
			public class FRIEDMUSHROOM
			{
				// Token: 0x0400BE1E RID: 48670
				public static LocString NAME = UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM");

				// Token: 0x0400BE1F RID: 48671
				public static LocString DESC = "A fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".\n\nIt has a thick, savory flavor with subtle earthy undertones.";

				// Token: 0x0400BE20 RID: 48672
				public static LocString RECIPEDESC = "A fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".";
			}

			// Token: 0x02002E49 RID: 11849
			public class PRICKLEFRUIT
			{
				// Token: 0x0400BE21 RID: 48673
				public static LocString NAME = UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT");

				// Token: 0x0400BE22 RID: 48674
				public static LocString DESC = "A sweet, mostly pleasant-tasting fruit covered in prickly barbs.";
			}

			// Token: 0x02002E4A RID: 11850
			public class GRILLEDPRICKLEFRUIT
			{
				// Token: 0x0400BE23 RID: 48675
				public static LocString NAME = UI.FormatAsLink("Gristle Berry", "GRILLEDPRICKLEFRUIT");

				// Token: 0x0400BE24 RID: 48676
				public static LocString DESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".\n\nHeat unlocked an exquisite taste in the fruit, though the burnt spines leave something to be desired.";

				// Token: 0x0400BE25 RID: 48677
				public static LocString RECIPEDESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".";
			}

			// Token: 0x02002E4B RID: 11851
			public class SWAMPFRUIT
			{
				// Token: 0x0400BE26 RID: 48678
				public static LocString NAME = UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT");

				// Token: 0x0400BE27 RID: 48679
				public static LocString DESC = "A fruit with an outer film that contains chewy gelatinous cubes.";
			}

			// Token: 0x02002E4C RID: 11852
			public class SWAMPDELIGHTS
			{
				// Token: 0x0400BE28 RID: 48680
				public static LocString NAME = UI.FormatAsLink("Swampy Delights", "SWAMPDELIGHTS");

				// Token: 0x0400BE29 RID: 48681
				public static LocString DESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".\n\nEach cube has a wonderfully chewy texture and is lightly coated in a delicate powder.";

				// Token: 0x0400BE2A RID: 48682
				public static LocString RECIPEDESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".";
			}

			// Token: 0x02002E4D RID: 11853
			public class WORMBASICFRUIT
			{
				// Token: 0x0400BE2B RID: 48683
				public static LocString NAME = UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT");

				// Token: 0x0400BE2C RID: 48684
				public static LocString DESC = "A " + UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT") + " that failed to develop properly.\n\nIt is nonetheless edible, and vaguely tasty.";
			}

			// Token: 0x02002E4E RID: 11854
			public class WORMBASICFOOD
			{
				// Token: 0x0400BE2D RID: 48685
				public static LocString NAME = UI.FormatAsLink("Roast Grubfruit Nut", "WORMBASICFOOD");

				// Token: 0x0400BE2E RID: 48686
				public static LocString DESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".\n\nIt has a smoky aroma and tastes of coziness.";

				// Token: 0x0400BE2F RID: 48687
				public static LocString RECIPEDESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".";
			}

			// Token: 0x02002E4F RID: 11855
			public class WORMSUPERFRUIT
			{
				// Token: 0x0400BE30 RID: 48688
				public static LocString NAME = UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT");

				// Token: 0x0400BE31 RID: 48689
				public static LocString DESC = "A plump, healthy fruit with a honey-like taste.";
			}

			// Token: 0x02002E50 RID: 11856
			public class WORMSUPERFOOD
			{
				// Token: 0x0400BE32 RID: 48690
				public static LocString NAME = UI.FormatAsLink("Grubfruit Preserve", "WORMSUPERFOOD");

				// Token: 0x0400BE33 RID: 48691
				public static LocString DESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					".\n\nThe thick, goopy jam retains the shape of the jar when poured out, but the sweet taste can't be matched."
				});

				// Token: 0x0400BE34 RID: 48692
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					"."
				});
			}

			// Token: 0x02002E51 RID: 11857
			public class BERRYPIE
			{
				// Token: 0x0400BE35 RID: 48693
				public static LocString NAME = UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE");

				// Token: 0x0400BE36 RID: 48694
				public static LocString DESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					".\n\nThe mixture of berries creates a fragrant, colorful filling that packs a sweet punch."
				});

				// Token: 0x0400BE37 RID: 48695
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					"."
				});
			}

			// Token: 0x02002E52 RID: 11858
			public class COLDWHEATBREAD
			{
				// Token: 0x0400BE38 RID: 48696
				public static LocString NAME = UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD");

				// Token: 0x0400BE39 RID: 48697
				public static LocString DESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.\n\nEach bite leaves a mild cooling sensation in one's mouth, even when the bun itself is warm.";

				// Token: 0x0400BE3A RID: 48698
				public static LocString RECIPEDESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.";
			}

			// Token: 0x02002E53 RID: 11859
			public class BEAN
			{
				// Token: 0x0400BE3B RID: 48699
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEAN");

				// Token: 0x0400BE3C RID: 48700
				public static LocString DESC = "The crisp bean of a " + UI.FormatAsLink("Nosh Sprout", "BEAN_PLANT") + ".\n\nEach bite tastes refreshingly natural and wholesome.";
			}

			// Token: 0x02002E54 RID: 11860
			public class SPICENUT
			{
				// Token: 0x0400BE3D RID: 48701
				public static LocString NAME = UI.FormatAsLink("Pincha Peppernut", "SPICENUT");

				// Token: 0x0400BE3E RID: 48702
				public static LocString DESC = "The flavorful nut of a " + UI.FormatAsLink("Pincha Pepperplant", "SPICE_VINE") + ".\n\nThe bitter outer rind hides a rich, peppery core that is useful in cooking.";
			}

			// Token: 0x02002E55 RID: 11861
			public class SPICEBREAD
			{
				// Token: 0x0400BE3F RID: 48703
				public static LocString NAME = UI.FormatAsLink("Pepper Bread", "SPICEBREAD");

				// Token: 0x0400BE40 RID: 48704
				public static LocString DESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.\n\nThere's a simple joy to be had in pulling it apart in one's fingers.";

				// Token: 0x0400BE41 RID: 48705
				public static LocString RECIPEDESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.";
			}

			// Token: 0x02002E56 RID: 11862
			public class SURFANDTURF
			{
				// Token: 0x0400BE42 RID: 48706
				public static LocString NAME = UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF");

				// Token: 0x0400BE43 RID: 48707
				public static LocString DESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea.\n\nIt's hearty and satisfying."
				});

				// Token: 0x0400BE44 RID: 48708
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea."
				});
			}

			// Token: 0x02002E57 RID: 11863
			public class TOFU
			{
				// Token: 0x0400BE45 RID: 48709
				public static LocString NAME = UI.FormatAsLink("Tofu", "TOFU");

				// Token: 0x0400BE46 RID: 48710
				public static LocString DESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEAN") + ".\n\nIt has an unusual but pleasant consistency.";

				// Token: 0x0400BE47 RID: 48711
				public static LocString RECIPEDESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEAN") + ".";
			}

			// Token: 0x02002E58 RID: 11864
			public class SPICYTOFU
			{
				// Token: 0x0400BE48 RID: 48712
				public static LocString NAME = UI.FormatAsLink("Spicy Tofu", "SPICYTOFU");

				// Token: 0x0400BE49 RID: 48713
				public static LocString DESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.\n\nIt packs a delightful punch.";

				// Token: 0x0400BE4A RID: 48714
				public static LocString RECIPEDESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.";
			}

			// Token: 0x02002E59 RID: 11865
			public class CURRY
			{
				// Token: 0x0400BE4B RID: 48715
				public static LocString NAME = UI.FormatAsLink("Curried Beans", "CURRY");

				// Token: 0x0400BE4C RID: 48716
				public static LocString DESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					".\n\nIt's so spicy!"
				});

				// Token: 0x0400BE4D RID: 48717
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					"."
				});
			}

			// Token: 0x02002E5A RID: 11866
			public class SALSA
			{
				// Token: 0x0400BE4E RID: 48718
				public static LocString NAME = UI.FormatAsLink("Stuffed Berry", "SALSA");

				// Token: 0x0400BE4F RID: 48719
				public static LocString DESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x0400BE50 RID: 48720
				public static LocString RECIPEDESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";
			}

			// Token: 0x02002E5B RID: 11867
			public class BASICPLANTFOOD
			{
				// Token: 0x0400BE51 RID: 48721
				public static LocString NAME = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD");

				// Token: 0x0400BE52 RID: 48722
				public static LocString DESC = "A flavorless grain that almost never wiggles on its own.";
			}

			// Token: 0x02002E5C RID: 11868
			public class BASICPLANTBAR
			{
				// Token: 0x0400BE53 RID: 48723
				public static LocString NAME = UI.FormatAsLink("Liceloaf", "BASICPLANTBAR");

				// Token: 0x0400BE54 RID: 48724
				public static LocString DESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";

				// Token: 0x0400BE55 RID: 48725
				public static LocString RECIPEDESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";
			}

			// Token: 0x02002E5D RID: 11869
			public class BASICFORAGEPLANT
			{
				// Token: 0x0400BE56 RID: 48726
				public static LocString NAME = UI.FormatAsLink("Muckroot", "BASICFORAGEPLANT");

				// Token: 0x0400BE57 RID: 48727
				public static LocString DESC = "A seedless fruit with an upsettingly bland aftertaste.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a " + ITEMS.FOOD.BASICFORAGEPLANT.NAME + ".";
			}

			// Token: 0x02002E5E RID: 11870
			public class FORESTFORAGEPLANT
			{
				// Token: 0x0400BE58 RID: 48728
				public static LocString NAME = UI.FormatAsLink("Hexalent Fruit", "FORESTFORAGEPLANT");

				// Token: 0x0400BE59 RID: 48729
				public static LocString DESC = "A seedless fruit with an unusual rubbery texture.\n\nIt cannot be replanted.\n\nHexalent fruit is much more calorie dense than Muckroot fruit.";
			}

			// Token: 0x02002E5F RID: 11871
			public class SWAMPFORAGEPLANT
			{
				// Token: 0x0400BE5A RID: 48730
				public static LocString NAME = UI.FormatAsLink("Swamp Chard Heart", "SWAMPFORAGEPLANT");

				// Token: 0x0400BE5B RID: 48731
				public static LocString DESC = "A seedless plant with a squishy, juicy center and an awful smell.\n\nIt cannot be replanted.";
			}

			// Token: 0x02002E60 RID: 11872
			public class ROTPILE
			{
				// Token: 0x0400BE5C RID: 48732
				public static LocString NAME = UI.FormatAsLink("Rot Pile", "COMPOST");

				// Token: 0x0400BE5D RID: 48733
				public static LocString DESC = string.Concat(new string[]
				{
					"An inedible glop of former foodstuff.\n\n",
					ITEMS.FOOD.ROTPILE.NAME,
					"s break down into ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" over time."
				});
			}

			// Token: 0x02002E61 RID: 11873
			public class COLDWHEATSEED
			{
				// Token: 0x0400BE5E RID: 48734
				public static LocString NAME = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED");

				// Token: 0x0400BE5F RID: 48735
				public static LocString DESC = "An edible grain that leaves a cool taste on the tongue.";
			}

			// Token: 0x02002E62 RID: 11874
			public class BEANPLANTSEED
			{
				// Token: 0x0400BE60 RID: 48736
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED");

				// Token: 0x0400BE61 RID: 48737
				public static LocString DESC = "An inedible bean that can be processed into delicious foods.";
			}

			// Token: 0x02002E63 RID: 11875
			public class QUICHE
			{
				// Token: 0x0400BE62 RID: 48738
				public static LocString NAME = UI.FormatAsLink("Mushroom Quiche", "QUICHE");

				// Token: 0x0400BE63 RID: 48739
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust.\n\nSomehow, it's both soggy <i>and</i> crispy."
				});

				// Token: 0x0400BE64 RID: 48740
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust."
				});
			}
		}

		// Token: 0x02001E06 RID: 7686
		public class INGREDIENTS
		{
			// Token: 0x02002E64 RID: 11876
			public class SWAMPLILYFLOWER
			{
				// Token: 0x0400BE65 RID: 48741
				public static LocString NAME = UI.FormatAsLink("Balm Lily Flower", "SWAMPLILYFLOWER");

				// Token: 0x0400BE66 RID: 48742
				public static LocString DESC = "A medicinal flower that soothes most minor maladies.\n\nIt is exceptionally fragrant.";
			}

			// Token: 0x02002E65 RID: 11877
			public class GINGER
			{
				// Token: 0x0400BE67 RID: 48743
				public static LocString NAME = UI.FormatAsLink("Tonic Root", "GINGER");

				// Token: 0x0400BE68 RID: 48744
				public static LocString DESC = "A chewy, fibrous rhizome with a fiery aftertaste.";
			}
		}

		// Token: 0x02001E07 RID: 7687
		public class INDUSTRIAL_PRODUCTS
		{
			// Token: 0x02002E66 RID: 11878
			public class FUEL_BRICK
			{
				// Token: 0x0400BE69 RID: 48745
				public static LocString NAME = "Fuel Brick";

				// Token: 0x0400BE6A RID: 48746
				public static LocString DESC = "A densely compressed brick of combustible material.\n\nIt can be burned to produce a one-time burst of " + UI.FormatAsLink("Power", "POWER") + ".";
			}

			// Token: 0x02002E67 RID: 11879
			public class BASIC_FABRIC
			{
				// Token: 0x0400BE6B RID: 48747
				public static LocString NAME = "Reed Fiber";

				// Token: 0x0400BE6C RID: 48748
				public static LocString DESC = "A ball of raw cellulose used in the production of " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " and textiles.";
			}

			// Token: 0x02002E68 RID: 11880
			public class TRAP_PARTS
			{
				// Token: 0x0400BE6D RID: 48749
				public static LocString NAME = "Trap Components";

				// Token: 0x0400BE6E RID: 48750
				public static LocString DESC = string.Concat(new string[]
				{
					"These components can be assembled into a ",
					BUILDINGS.PREFABS.CREATURETRAP.NAME,
					" and used to catch ",
					UI.FormatAsLink("Critters", "CREATURES"),
					"."
				});
			}

			// Token: 0x02002E69 RID: 11881
			public class POWER_STATION_TOOLS
			{
				// Token: 0x0400BE6F RID: 48751
				public static LocString NAME = "Microchip";

				// Token: 0x0400BE70 RID: 48752
				public static LocString DESC = string.Concat(new string[]
				{
					"A specialized ",
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					" created by a professional engineer.\n\nTunes up ",
					UI.PRE_KEYWORD,
					"Generators",
					UI.PST_KEYWORD,
					" to increase their ",
					UI.FormatAsLink("Power", "POWER"),
					" output."
				});

				// Token: 0x0400BE71 RID: 48753
				public static LocString TINKER_REQUIREMENT_NAME = "Skill: " + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;

				// Token: 0x0400BE72 RID: 48754
				public static LocString TINKER_REQUIREMENT_TOOLTIP = string.Concat(new string[]
				{
					"Can only be used by a Duplicant with ",
					DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME,
					" to apply a ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400BE73 RID: 48755
				public static LocString TINKER_EFFECT_NAME = "Engie's Tune-Up: {0} {1}";

				// Token: 0x0400BE74 RID: 48756
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					" a generator, increasing its {0} by <b>{1}</b>."
				});
			}

			// Token: 0x02002E6A RID: 11882
			public class FARM_STATION_TOOLS
			{
				// Token: 0x0400BE75 RID: 48757
				public static LocString NAME = "Micronutrient Fertilizer";

				// Token: 0x0400BE76 RID: 48758
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized ",
					UI.FormatAsLink("Fertilizer", "FERTILIZER"),
					" mixed by a Duplicant with the ",
					DUPLICANTS.ROLES.FARMER.NAME,
					" Skill.\n\nIncreases the ",
					UI.PRE_KEYWORD,
					"Growth Rate",
					UI.PST_KEYWORD,
					" of one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					"."
				});
			}

			// Token: 0x02002E6B RID: 11883
			public class MACHINE_PARTS
			{
				// Token: 0x0400BE77 RID: 48759
				public static LocString NAME = "Custom Parts";

				// Token: 0x0400BE78 RID: 48760
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized Parts crafted by a professional engineer.\n\n",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" machine buildings to increase their efficiency."
				});

				// Token: 0x0400BE79 RID: 48761
				public static LocString TINKER_REQUIREMENT_NAME = "Job: " + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;

				// Token: 0x0400BE7A RID: 48762
				public static LocString TINKER_REQUIREMENT_TOOLTIP = string.Concat(new string[]
				{
					"Can only be used by a Duplicant with ",
					DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME,
					" to apply a ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400BE7B RID: 48763
				public static LocString TINKER_EFFECT_NAME = "Engineer's Jerry Rig: {0} {1}";

				// Token: 0x0400BE7C RID: 48764
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" upgrades to a machine building, increasing its {0} by <b>{1}</b>."
				});
			}

			// Token: 0x02002E6C RID: 11884
			public class RESEARCH_DATABANK
			{
				// Token: 0x0400BE7D RID: 48765
				public static LocString NAME = "Data Bank";

				// Token: 0x0400BE7E RID: 48766
				public static LocString DESC = "Raw data that can be processed into " + UI.FormatAsLink("Interstellar Research", "RESEARCH") + " points.";
			}

			// Token: 0x02002E6D RID: 11885
			public class ORBITAL_RESEARCH_DATABANK
			{
				// Token: 0x0400BE7F RID: 48767
				public static LocString NAME = "Data Bank";

				// Token: 0x0400BE80 RID: 48768
				public static LocString DESC = "Raw Data that can be processed into " + UI.FormatAsLink("Data Analysis Research", "RESEARCH") + " points.";

				// Token: 0x0400BE81 RID: 48769
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Databanks of raw data generated from exploring, either by exploring new areas with Duplicants, or by using an ",
					UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER"),
					".\n\nUsed by the ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to conduct research."
				});
			}

			// Token: 0x02002E6E RID: 11886
			public class EGG_SHELL
			{
				// Token: 0x0400BE82 RID: 48770
				public static LocString NAME = "Egg Shell";

				// Token: 0x0400BE83 RID: 48771
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";
			}

			// Token: 0x02002E6F RID: 11887
			public class CRAB_SHELL
			{
				// Token: 0x0400BE84 RID: 48772
				public static LocString NAME = "Pokeshell Molt";

				// Token: 0x0400BE85 RID: 48773
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x020031E3 RID: 12771
				public class VARIANT_WOOD
				{
					// Token: 0x0400C74A RID: 51018
					public static LocString NAME = "Oakshell Molt";

					// Token: 0x0400C74B RID: 51019
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lumber", "WOOD") + ".";
				}
			}

			// Token: 0x02002E70 RID: 11888
			public class BABY_CRAB_SHELL
			{
				// Token: 0x0400BE86 RID: 48774
				public static LocString NAME = "Small Pokeshell Molt";

				// Token: 0x0400BE87 RID: 48775
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x020031E4 RID: 12772
				public class VARIANT_WOOD
				{
					// Token: 0x0400C74C RID: 51020
					public static LocString NAME = "Small Oakshell Molt";

					// Token: 0x0400C74D RID: 51021
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lumber", "WOOD") + ".";
				}
			}

			// Token: 0x02002E71 RID: 11889
			public class WOOD
			{
				// Token: 0x0400BE88 RID: 48776
				public static LocString NAME = "Lumber";

				// Token: 0x0400BE89 RID: 48777
				public static LocString DESC = string.Concat(new string[]
				{
					"Wood harvested from an ",
					UI.FormatAsLink("Arbor Tree", "FOREST_TREE"),
					" or an ",
					UI.FormatAsLink("Oakshell", "CRABWOOD"),
					"."
				});
			}

			// Token: 0x02002E72 RID: 11890
			public class GENE_SHUFFLER_RECHARGE
			{
				// Token: 0x0400BE8A RID: 48778
				public static LocString NAME = "Vacillator Recharge";

				// Token: 0x0400BE8B RID: 48779
				public static LocString DESC = "Replenishes one charge to a depleted " + BUILDINGS.PREFABS.GENESHUFFLER.NAME + ".";
			}

			// Token: 0x02002E73 RID: 11891
			public class TABLE_SALT
			{
				// Token: 0x0400BE8C RID: 48780
				public static LocString NAME = "Table Salt";

				// Token: 0x0400BE8D RID: 48781
				public static LocString DESC = string.Concat(new string[]
				{
					"A seasoning that Duplicants can add to their ",
					UI.FormatAsLink("Food", "FOOD"),
					" to boost ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nDuplicants will automatically use Table Salt while sitting at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME,
					" during mealtime.\n\n<i>Only the finest grains are chosen.</i>"
				});
			}

			// Token: 0x02002E74 RID: 11892
			public class REFINED_SUGAR
			{
				// Token: 0x0400BE8E RID: 48782
				public static LocString NAME = "Refined Sugar";

				// Token: 0x0400BE8F RID: 48783
				public static LocString DESC = string.Concat(new string[]
				{
					"A seasoning that Duplicants can add to their ",
					UI.FormatAsLink("Food", "FOOD"),
					" to boost ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nDuplicants will automatically use Refined Sugar while sitting at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME,
					" during mealtime.\n\n<i>Only the finest grains are chosen.</i>"
				});
			}
		}

		// Token: 0x02001E08 RID: 7688
		public class CARGO_CAPSULE
		{
			// Token: 0x040089F8 RID: 35320
			public static LocString NAME = "Care Package";

			// Token: 0x040089F9 RID: 35321
			public static LocString DESC = "A delivery system for recently printed resources.\n\nIt will dematerialize shortly.";
		}

		// Token: 0x02001E09 RID: 7689
		public class RAILGUNPAYLOAD
		{
			// Token: 0x040089FA RID: 35322
			public static LocString NAME = UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD");

			// Token: 0x040089FB RID: 35323
			public static LocString DESC = string.Concat(new string[]
			{
				"Contains resources packed for interstellar shipping.\n\nCan be launched by a ",
				BUILDINGS.PREFABS.RAILGUN.NAME,
				" or unpacked with a ",
				BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME,
				"."
			});
		}

		// Token: 0x02001E0A RID: 7690
		public class MISSILE_BASIC
		{
			// Token: 0x040089FC RID: 35324
			public static LocString NAME = UI.FormatAsLink("Blastshot", "MISSILELAUNCHER");

			// Token: 0x040089FD RID: 35325
			public static LocString DESC = "An explosive projectile designed to defend against meteor showers.\n\nMust be launched by a " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}

		// Token: 0x02001E0B RID: 7691
		public class DEBRISPAYLOAD
		{
			// Token: 0x040089FE RID: 35326
			public static LocString NAME = "Rocket Debris";

			// Token: 0x040089FF RID: 35327
			public static LocString DESC = "Whatever is left over from a Rocket Self-Destruct can be recovered once it has crash-landed.";
		}

		// Token: 0x02001E0C RID: 7692
		public class RADIATION
		{
			// Token: 0x02002E75 RID: 11893
			public class HIGHENERGYPARITCLE
			{
				// Token: 0x0400BE90 RID: 48784
				public static LocString NAME = "Radbolts";

				// Token: 0x0400BE91 RID: 48785
				public static LocString DESC = string.Concat(new string[]
				{
					"A concentrated field of ",
					UI.FormatAsKeyWord("Radbolts"),
					" that can be largely redirected using a ",
					UI.FormatAsLink("Radbolt Reflector", "HIGHENERGYPARTICLEREDIRECTOR"),
					"."
				});
			}
		}

		// Token: 0x02001E0D RID: 7693
		public class DREAMJOURNAL
		{
			// Token: 0x04008A00 RID: 35328
			public static LocString NAME = "Dream Journal";

			// Token: 0x04008A01 RID: 35329
			public static LocString DESC = string.Concat(new string[]
			{
				"A hand-scrawled account of ",
				UI.FormatAsLink("Pajama", "SLEEP_CLINIC_PAJAMAS"),
				"-induced dreams.\n\nCan be analyzed using a ",
				UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK"),
				"."
			});
		}

		// Token: 0x02001E0E RID: 7694
		public class SPICES
		{
			// Token: 0x02002E76 RID: 11894
			public class MACHINERY_SPICE
			{
				// Token: 0x0400BE92 RID: 48786
				public static LocString NAME = UI.FormatAsLink("Machinist Spice", "MACHINERY_SPICE");

				// Token: 0x0400BE93 RID: 48787
				public static LocString DESC = "Improves operating skills when ingested.";
			}

			// Token: 0x02002E77 RID: 11895
			public class PILOTING_SPICE
			{
				// Token: 0x0400BE94 RID: 48788
				public static LocString NAME = UI.FormatAsLink("Rocketeer Spice", "PILOTING_SPICE");

				// Token: 0x0400BE95 RID: 48789
				public static LocString DESC = "Provides a boost to piloting abilities.";
			}

			// Token: 0x02002E78 RID: 11896
			public class PRESERVING_SPICE
			{
				// Token: 0x0400BE96 RID: 48790
				public static LocString NAME = UI.FormatAsLink("Freshener Spice", "PRESERVING_SPICE");

				// Token: 0x0400BE97 RID: 48791
				public static LocString DESC = "Slows the decomposition of perishable foods.";
			}

			// Token: 0x02002E79 RID: 11897
			public class STRENGTH_SPICE
			{
				// Token: 0x0400BE98 RID: 48792
				public static LocString NAME = UI.FormatAsLink("Brawny Spice", "STRENGTH_SPICE");

				// Token: 0x0400BE99 RID: 48793
				public static LocString DESC = "Strengthens even the weakest of muscles.";
			}
		}
	}
}
