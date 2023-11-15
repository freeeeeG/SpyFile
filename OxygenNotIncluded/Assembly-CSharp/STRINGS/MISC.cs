using System;

namespace STRINGS
{
	// Token: 0x02000DB5 RID: 3509
	public class MISC
	{
		// Token: 0x02001EC4 RID: 7876
		public class TAGS
		{
			// Token: 0x04008B74 RID: 35700
			public static LocString OTHER = "Miscellaneous";

			// Token: 0x04008B75 RID: 35701
			public static LocString FILTER = UI.FormatAsLink("Filtration Medium", "FILTER");

			// Token: 0x04008B76 RID: 35702
			public static LocString FILTER_DESC = string.Concat(new string[]
			{
				"Filtration Mediums are materials which are supplied to some filtration buildings that are used in separating purified ",
				UI.FormatAsLink("gases", "ELEMENTS_GASSES"),
				" or ",
				UI.FormatAsLink("liquids", "ELEMENTS_LIQUID"),
				" from their polluted forms.\n\nExamples include filtering ",
				UI.FormatAsLink("Water", "WATER"),
				" from ",
				UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
				" using a ",
				UI.FormatAsLink("Water Sieve", "WATERPURIFIER"),
				", or a ",
				UI.FormatAsLink("Deodorizer", "AIRFILTER"),
				" purifying ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				" from ",
				UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
				".\n\nFiltration Mediums are a consumable that will be transformed by the filtering process to generate a by-product, like when ",
				UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
				" is the result after ",
				UI.FormatAsLink("Sand", "SAND"),
				" has been used to filter polluted water. The filtering building will cease to function once the filtering material has been consumed. Once the Filtering Material has been resupplied to the filtering building it will start working again."
			});

			// Token: 0x04008B77 RID: 35703
			public static LocString ICEORE = UI.FormatAsLink("Ice", "ICEORE");

			// Token: 0x04008B78 RID: 35704
			public static LocString ICEORE_DESC = string.Concat(new string[]
			{
				"Ice is a class of materials made up mostly (if not completely) of ",
				UI.FormatAsLink("Water", "WATER"),
				" in a frozen or partially frozen form.\n\nAs a material in a frigid solid or semi-solid state, these elements are very useful as a low-cost way to cool the environment around them.\n\nWhen heated, ice will melt into its original liquified form (ie.",
				UI.FormatAsLink("Brine Ice", "BRINEICE"),
				" will liquify into ",
				UI.FormatAsLink("Brine", "BRINE"),
				"). Each ice element has a different freezing and melting point based upon its composition and state."
			});

			// Token: 0x04008B79 RID: 35705
			public static LocString PHOSPHORUS = "Phosphorus";

			// Token: 0x04008B7A RID: 35706
			public static LocString BUILDABLERAW = "Raw Mineral";

			// Token: 0x04008B7B RID: 35707
			public static LocString BUILDABLERAW_DESC = "";

			// Token: 0x04008B7C RID: 35708
			public static LocString BUILDABLEPROCESSED = "Refined Mineral";

			// Token: 0x04008B7D RID: 35709
			public static LocString BUILDABLEANY = "General Buildable";

			// Token: 0x04008B7E RID: 35710
			public static LocString BUILDABLEANY_DESC = "";

			// Token: 0x04008B7F RID: 35711
			public static LocString REFINEDMETAL = UI.FormatAsLink("Refined Metal", "REFINEDMETAL");

			// Token: 0x04008B80 RID: 35712
			public static LocString REFINEDMETAL_DESC = string.Concat(new string[]
			{
				"Refined metals are purified forms of metal often used in higher-tier electronics due to their tendency to be able to withstand higher temperatures when they are made into wires. Other benefits include the increased decor value for some metals which can greatly improve the well-being of a colony.\n\nMetal ore can be refined in either the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				" or the ",
				UI.FormatAsLink("Metal Refinery", "METALREFINERY"),
				"."
			});

			// Token: 0x04008B81 RID: 35713
			public static LocString METAL = "Metal Ore";

			// Token: 0x04008B82 RID: 35714
			public static LocString METAL_DESC = "";

			// Token: 0x04008B83 RID: 35715
			public static LocString PRECIOUSMETAL = "Precious Metal";

			// Token: 0x04008B84 RID: 35716
			public static LocString RAWPRECIOUSMETAL = "Precious Metal Ore";

			// Token: 0x04008B85 RID: 35717
			public static LocString PRECIOUSROCK = "Precious Rock";

			// Token: 0x04008B86 RID: 35718
			public static LocString PRECIOUSROCK_DESC = "";

			// Token: 0x04008B87 RID: 35719
			public static LocString ALLOY = "Alloy";

			// Token: 0x04008B88 RID: 35720
			public static LocString BUILDINGFIBER = "Fiber";

			// Token: 0x04008B89 RID: 35721
			public static LocString BUILDINGFIBER_DESC = "";

			// Token: 0x04008B8A RID: 35722
			public static LocString BUILDINGWOOD = "Wood";

			// Token: 0x04008B8B RID: 35723
			public static LocString BUILDINGWOOD_DESC = "";

			// Token: 0x04008B8C RID: 35724
			public static LocString CRUSHABLE = "Crushable";

			// Token: 0x04008B8D RID: 35725
			public static LocString CROPSEEDS = "Crop Seeds";

			// Token: 0x04008B8E RID: 35726
			public static LocString BAGABLECREATURE = "Critter";

			// Token: 0x04008B8F RID: 35727
			public static LocString SWIMMINGCREATURE = "Aquatic Critter";

			// Token: 0x04008B90 RID: 35728
			public static LocString LIFE = "Life";

			// Token: 0x04008B91 RID: 35729
			public static LocString LIQUIFIABLE = "Liquefiable";

			// Token: 0x04008B92 RID: 35730
			public static LocString LIQUID = "Liquid";

			// Token: 0x04008B93 RID: 35731
			public static LocString SPECIAL = "Special";

			// Token: 0x04008B94 RID: 35732
			public static LocString FARMABLE = "Cultivable Soil";

			// Token: 0x04008B95 RID: 35733
			public static LocString FARMABLE_DESC = "";

			// Token: 0x04008B96 RID: 35734
			public static LocString AGRICULTURE = "Agriculture";

			// Token: 0x04008B97 RID: 35735
			public static LocString COAL = "Coal";

			// Token: 0x04008B98 RID: 35736
			public static LocString BLEACHSTONE = "Bleach Stone";

			// Token: 0x04008B99 RID: 35737
			public static LocString ORGANICS = "Organic";

			// Token: 0x04008B9A RID: 35738
			public static LocString CONSUMABLEORE = "Consumable Ore";

			// Token: 0x04008B9B RID: 35739
			public static LocString ORE = "Ore";

			// Token: 0x04008B9C RID: 35740
			public static LocString BREATHABLE = "Breathable Gas";

			// Token: 0x04008B9D RID: 35741
			public static LocString UNBREATHABLE = "Unbreathable Gas";

			// Token: 0x04008B9E RID: 35742
			public static LocString GAS = "Gas";

			// Token: 0x04008B9F RID: 35743
			public static LocString BURNS = "Flammable";

			// Token: 0x04008BA0 RID: 35744
			public static LocString UNSTABLE = "Unstable";

			// Token: 0x04008BA1 RID: 35745
			public static LocString TOXIC = "Toxic";

			// Token: 0x04008BA2 RID: 35746
			public static LocString MIXTURE = "Mixture";

			// Token: 0x04008BA3 RID: 35747
			public static LocString SOLID = "Solid";

			// Token: 0x04008BA4 RID: 35748
			public static LocString FLYINGCRITTEREDIBLE = "Bait";

			// Token: 0x04008BA5 RID: 35749
			public static LocString INDUSTRIALPRODUCT = "Industrial Product";

			// Token: 0x04008BA6 RID: 35750
			public static LocString INDUSTRIALINGREDIENT = "Industrial Ingredient";

			// Token: 0x04008BA7 RID: 35751
			public static LocString MEDICALSUPPLIES = "Medical Supplies";

			// Token: 0x04008BA8 RID: 35752
			public static LocString CLOTHES = "Clothing";

			// Token: 0x04008BA9 RID: 35753
			public static LocString EMITSLIGHT = "Light Emitter";

			// Token: 0x04008BAA RID: 35754
			public static LocString BED = "Bed";

			// Token: 0x04008BAB RID: 35755
			public static LocString MESSSTATION = "Dining Table";

			// Token: 0x04008BAC RID: 35756
			public static LocString TOY = "Toy";

			// Token: 0x04008BAD RID: 35757
			public static LocString SUIT = "Suit";

			// Token: 0x04008BAE RID: 35758
			public static LocString MULTITOOL = "Multitool";

			// Token: 0x04008BAF RID: 35759
			public static LocString CLINIC = "Clinic";

			// Token: 0x04008BB0 RID: 35760
			public static LocString RELAXATION_POINT = "Leisure Area";

			// Token: 0x04008BB1 RID: 35761
			public static LocString SOLIDMATERIAL = "Solid Material";

			// Token: 0x04008BB2 RID: 35762
			public static LocString EXTRUDABLE = "Extrudable";

			// Token: 0x04008BB3 RID: 35763
			public static LocString PLUMBABLE = "Plumbable";

			// Token: 0x04008BB4 RID: 35764
			public static LocString PLUMBABLE_DESC = "";

			// Token: 0x04008BB5 RID: 35765
			public static LocString COMPOSTABLE = UI.FormatAsLink("Compostable", "COMPOSTABLE");

			// Token: 0x04008BB6 RID: 35766
			public static LocString COMPOSTABLE_DESC = string.Concat(new string[]
			{
				"Compostables are biological materials which can be put into a ",
				UI.FormatAsLink("Compost", "COMPOST"),
				" to generate clean ",
				UI.FormatAsLink("Dirt", "DIRT"),
				".\n\nComposting also generates a small amount of ",
				UI.FormatAsLink("Heat", "HEAT"),
				".\n\nOnce it starts to rot, consumable food should be composted to prevent ",
				UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
				"."
			});

			// Token: 0x04008BB7 RID: 35767
			public static LocString COMPOSTBASICPLANTFOOD = "Compost Muckroot";

			// Token: 0x04008BB8 RID: 35768
			public static LocString EDIBLE = "Edible";

			// Token: 0x04008BB9 RID: 35769
			public static LocString OXIDIZER = "Oxidizer";

			// Token: 0x04008BBA RID: 35770
			public static LocString COOKINGINGREDIENT = "Cooking Ingredient";

			// Token: 0x04008BBB RID: 35771
			public static LocString MEDICINE = "Medicine";

			// Token: 0x04008BBC RID: 35772
			public static LocString SEED = "Seed";

			// Token: 0x04008BBD RID: 35773
			public static LocString ANYWATER = "Water Based";

			// Token: 0x04008BBE RID: 35774
			public static LocString MARKEDFORCOMPOST = "Marked For Compost";

			// Token: 0x04008BBF RID: 35775
			public static LocString MARKEDFORCOMPOSTINSTORAGE = "In Compost Storage";

			// Token: 0x04008BC0 RID: 35776
			public static LocString COMPOSTMEAT = "Compost Meat";

			// Token: 0x04008BC1 RID: 35777
			public static LocString PICKLED = "Pickled";

			// Token: 0x04008BC2 RID: 35778
			public static LocString PLASTIC = "Plastics";

			// Token: 0x04008BC3 RID: 35779
			public static LocString PLASTIC_DESC = "";

			// Token: 0x04008BC4 RID: 35780
			public static LocString TOILET = "Toilet";

			// Token: 0x04008BC5 RID: 35781
			public static LocString MASSAGE_TABLE = "Massage Table";

			// Token: 0x04008BC6 RID: 35782
			public static LocString POWERSTATION = "Power Station";

			// Token: 0x04008BC7 RID: 35783
			public static LocString FARMSTATION = "Farm Station";

			// Token: 0x04008BC8 RID: 35784
			public static LocString MACHINE_SHOP = "Machine Shop";

			// Token: 0x04008BC9 RID: 35785
			public static LocString ANTISEPTIC = "Antiseptic";

			// Token: 0x04008BCA RID: 35786
			public static LocString OIL = "Hydrocarbon";

			// Token: 0x04008BCB RID: 35787
			public static LocString DECORATION = "Decoration";

			// Token: 0x04008BCC RID: 35788
			public static LocString EGG = "Critter Egg";

			// Token: 0x04008BCD RID: 35789
			public static LocString EGGSHELL = "Egg Shell";

			// Token: 0x04008BCE RID: 35790
			public static LocString MANUFACTUREDMATERIAL = "Manufactured Material";

			// Token: 0x04008BCF RID: 35791
			public static LocString STEEL = "Steel";

			// Token: 0x04008BD0 RID: 35792
			public static LocString RAW = "Raw Animal Product";

			// Token: 0x04008BD1 RID: 35793
			public static LocString ANY = "Any";

			// Token: 0x04008BD2 RID: 35794
			public static LocString TRANSPARENT = "Transparent";

			// Token: 0x04008BD3 RID: 35795
			public static LocString TRANSPARENT_DESC = "";

			// Token: 0x04008BD4 RID: 35796
			public static LocString RAREMATERIALS = "Rare Resource";

			// Token: 0x04008BD5 RID: 35797
			public static LocString FARMINGMATERIAL = "Fertilizer";

			// Token: 0x04008BD6 RID: 35798
			public static LocString INSULATOR = "Insulator";

			// Token: 0x04008BD7 RID: 35799
			public static LocString INSULATOR_DESC = "";

			// Token: 0x04008BD8 RID: 35800
			public static LocString RAILGUNPAYLOADEMPTYABLE = "Payload";

			// Token: 0x04008BD9 RID: 35801
			public static LocString NONCRUSHABLE = "Uncrushable";

			// Token: 0x04008BDA RID: 35802
			public static LocString STORYTRAITRESOURCE = "Story Trait";

			// Token: 0x04008BDB RID: 35803
			public static LocString COMMAND_MODULE = "Command Module";

			// Token: 0x04008BDC RID: 35804
			public static LocString HABITAT_MODULE = "Habitat Module";

			// Token: 0x04008BDD RID: 35805
			public static LocString COMBUSTIBLEGAS = "Combustible Gas";

			// Token: 0x04008BDE RID: 35806
			public static LocString COMBUSTIBLELIQUID = UI.FormatAsLink("Combustible Liquid", "COMBUSTIBLELIQUID");

			// Token: 0x04008BDF RID: 35807
			public static LocString COMBUSTIBLELIQUID_DESC = string.Concat(new string[]
			{
				"Combustible Liquids are liquids that can be burned as fuel to be used in energy production such as in a ",
				UI.FormatAsLink("Petroleum Generator", "PETROLEUMGENERATOR"),
				" or a ",
				UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINE"),
				".\n\nThough these liquids have other uses, such as fertilizer for growing a ",
				UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED"),
				", their primary usefulness lies in their ability to be burned for ",
				UI.FormatAsLink("power", "POWER"),
				"."
			});

			// Token: 0x04008BE0 RID: 35808
			public static LocString COMBUSTIBLESOLID = "Combustible Solid";

			// Token: 0x04008BE1 RID: 35809
			public static LocString UNIDENTIFIEDSEED = "Seed (Unidentified Mutation)";

			// Token: 0x04008BE2 RID: 35810
			public static LocString CHARMEDARTIFACT = "Artifact of Interest";

			// Token: 0x04008BE3 RID: 35811
			public static LocString GENE_SHUFFLER = "Neural Vacillator";

			// Token: 0x04008BE4 RID: 35812
			public static LocString WARP_PORTAL = "Teleportal";

			// Token: 0x04008BE5 RID: 35813
			public static LocString FARMING = "Farm Build-Delivery";

			// Token: 0x04008BE6 RID: 35814
			public static LocString RESEARCH = "Research Delivery";

			// Token: 0x04008BE7 RID: 35815
			public static LocString POWER = "Generator Delivery";

			// Token: 0x04008BE8 RID: 35816
			public static LocString BUILDING = "Build Dig-Delivery";

			// Token: 0x04008BE9 RID: 35817
			public static LocString COOKING = "Cook Delivery";

			// Token: 0x04008BEA RID: 35818
			public static LocString FABRICATING = "Fabricate Delivery";

			// Token: 0x04008BEB RID: 35819
			public static LocString WIRING = "Wire Build-Delivery";

			// Token: 0x04008BEC RID: 35820
			public static LocString ART = "Art Build-Delivery";

			// Token: 0x04008BED RID: 35821
			public static LocString DOCTORING = "Treatment Delivery";

			// Token: 0x04008BEE RID: 35822
			public static LocString CONVEYOR = "Shipping Build";

			// Token: 0x04008BEF RID: 35823
			public static LocString COMPOST_FORMAT = "{Item}";

			// Token: 0x04008BF0 RID: 35824
			public static LocString ADVANCEDDOCTORSTATIONMEDICALSUPPLIES = "Serum Vial";

			// Token: 0x04008BF1 RID: 35825
			public static LocString DOCTORSTATIONMEDICALSUPPLIES = "Medical Pack";
		}

		// Token: 0x02001EC5 RID: 7877
		public class STATUSITEMS
		{
			// Token: 0x02002E8C RID: 11916
			public class ATTENTIONREQUIRED
			{
				// Token: 0x0400BED3 RID: 48851
				public static LocString NAME = "Attention Required!";

				// Token: 0x0400BED4 RID: 48852
				public static LocString TOOLTIP = "Something in my colony needs to be attended to";
			}

			// Token: 0x02002E8D RID: 11917
			public class SUBLIMATIONBLOCKED
			{
				// Token: 0x0400BED5 RID: 48853
				public static LocString NAME = "{SubElement} emission blocked";

				// Token: 0x0400BED6 RID: 48854
				public static LocString TOOLTIP = "This {Element} deposit is not exposed to air and cannot emit {SubElement}";
			}

			// Token: 0x02002E8E RID: 11918
			public class SUBLIMATIONOVERPRESSURE
			{
				// Token: 0x0400BED7 RID: 48855
				public static LocString NAME = "Inert";

				// Token: 0x0400BED8 RID: 48856
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Environmental ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is too high for this {Element} deposit to emit {SubElement}"
				});
			}

			// Token: 0x02002E8F RID: 11919
			public class SUBLIMATIONEMITTING
			{
				// Token: 0x0400BED9 RID: 48857
				public static LocString NAME = BUILDING.STATUSITEMS.EMITTINGGASAVG.NAME;

				// Token: 0x0400BEDA RID: 48858
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.EMITTINGGASAVG.TOOLTIP;
			}

			// Token: 0x02002E90 RID: 11920
			public class SPACE
			{
				// Token: 0x0400BEDB RID: 48859
				public static LocString NAME = "Space exposure";

				// Token: 0x0400BEDC RID: 48860
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This region is exposed to the vacuum of space and will result in the loss of ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" resources"
				});
			}

			// Token: 0x02002E91 RID: 11921
			public class EDIBLE
			{
				// Token: 0x0400BEDD RID: 48861
				public static LocString NAME = "Rations: {0}";

				// Token: 0x0400BEDE RID: 48862
				public static LocString TOOLTIP = "Can provide " + UI.FormatAsLink("{0}", "KCAL") + " of energy to Duplicants";
			}

			// Token: 0x02002E92 RID: 11922
			public class MARKEDFORDISINFECTION
			{
				// Token: 0x0400BEDF RID: 48863
				public static LocString NAME = "Disinfect Errand";

				// Token: 0x0400BEE0 RID: 48864
				public static LocString TOOLTIP = "Building will be disinfected once a Duplicant is available";
			}

			// Token: 0x02002E93 RID: 11923
			public class PENDINGCLEAR
			{
				// Token: 0x0400BEE1 RID: 48865
				public static LocString NAME = "Sweep Errand";

				// Token: 0x0400BEE2 RID: 48866
				public static LocString TOOLTIP = "Debris will be swept once a Duplicant is available";
			}

			// Token: 0x02002E94 RID: 11924
			public class PENDINGCLEARNOSTORAGE
			{
				// Token: 0x0400BEE3 RID: 48867
				public static LocString NAME = "Storage Unavailable";

				// Token: 0x0400BEE4 RID: 48868
				public static LocString TOOLTIP = "No available " + BUILDINGS.PREFABS.STORAGELOCKER.NAME + " can accept this item\n\nMake sure the filter on your storage is correctly set and there is sufficient space remaining";
			}

			// Token: 0x02002E95 RID: 11925
			public class MARKEDFORCOMPOST
			{
				// Token: 0x0400BEE5 RID: 48869
				public static LocString NAME = "Compost Errand";

				// Token: 0x0400BEE6 RID: 48870
				public static LocString TOOLTIP = "Object is marked and will be moved to " + BUILDINGS.PREFABS.COMPOST.NAME + " once a Duplicant is available";
			}

			// Token: 0x02002E96 RID: 11926
			public class NOCLEARLOCATIONSAVAILABLE
			{
				// Token: 0x0400BEE7 RID: 48871
				public static LocString NAME = "No Sweep Destination";

				// Token: 0x0400BEE8 RID: 48872
				public static LocString TOOLTIP = "There are no valid destinations for this object to be swept to";
			}

			// Token: 0x02002E97 RID: 11927
			public class PENDINGHARVEST
			{
				// Token: 0x0400BEE9 RID: 48873
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400BEEA RID: 48874
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x02002E98 RID: 11928
			public class PENDINGUPROOT
			{
				// Token: 0x0400BEEB RID: 48875
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400BEEC RID: 48876
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x02002E99 RID: 11929
			public class WAITINGFORDIG
			{
				// Token: 0x0400BEED RID: 48877
				public static LocString NAME = "Dig Errand";

				// Token: 0x0400BEEE RID: 48878
				public static LocString TOOLTIP = "Tile will be dug out once a Duplicant is available";
			}

			// Token: 0x02002E9A RID: 11930
			public class WAITINGFORMOP
			{
				// Token: 0x0400BEEF RID: 48879
				public static LocString NAME = "Mop Errand";

				// Token: 0x0400BEF0 RID: 48880
				public static LocString TOOLTIP = "Spill will be mopped once a Duplicant is available";
			}

			// Token: 0x02002E9B RID: 11931
			public class NOTMARKEDFORHARVEST
			{
				// Token: 0x0400BEF1 RID: 48881
				public static LocString NAME = "No Harvest Pending";

				// Token: 0x0400BEF2 RID: 48882
				public static LocString TOOLTIP = "Use the " + UI.FormatAsTool("Harvest Tool", global::Action.Harvest) + " to mark this plant for harvest";
			}

			// Token: 0x02002E9C RID: 11932
			public class GROWINGBRANCHES
			{
				// Token: 0x0400BEF3 RID: 48883
				public static LocString NAME = "Growing Branches";

				// Token: 0x0400BEF4 RID: 48884
				public static LocString TOOLTIP = "This tree is working hard to grow new branches right now";
			}

			// Token: 0x02002E9D RID: 11933
			public class CLUSTERMETEORREMAININGTRAVELTIME
			{
				// Token: 0x0400BEF5 RID: 48885
				public static LocString NAME = "Time to collision: {time}";

				// Token: 0x0400BEF6 RID: 48886
				public static LocString TOOLTIP = "The time remaining before this meteor reaches its destination";
			}

			// Token: 0x02002E9E RID: 11934
			public class ELEMENTALCATEGORY
			{
				// Token: 0x0400BEF7 RID: 48887
				public static LocString NAME = "{Category}";

				// Token: 0x0400BEF8 RID: 48888
				public static LocString TOOLTIP = "The selected object belongs to the <b>{Category}</b> resource category";
			}

			// Token: 0x02002E9F RID: 11935
			public class ELEMENTALMASS
			{
				// Token: 0x0400BEF9 RID: 48889
				public static LocString NAME = "{Mass}";

				// Token: 0x0400BEFA RID: 48890
				public static LocString TOOLTIP = "The selected object has a mass of <b>{Mass}</b>";
			}

			// Token: 0x02002EA0 RID: 11936
			public class ELEMENTALDISEASE
			{
				// Token: 0x0400BEFB RID: 48891
				public static LocString NAME = "{Disease}";

				// Token: 0x0400BEFC RID: 48892
				public static LocString TOOLTIP = "Current disease: {Disease}";
			}

			// Token: 0x02002EA1 RID: 11937
			public class ELEMENTALTEMPERATURE
			{
				// Token: 0x0400BEFD RID: 48893
				public static LocString NAME = "{Temp}";

				// Token: 0x0400BEFE RID: 48894
				public static LocString TOOLTIP = "The selected object is currently <b>{Temp}</b>";
			}

			// Token: 0x02002EA2 RID: 11938
			public class MARKEDFORCOMPOSTINSTORAGE
			{
				// Token: 0x0400BEFF RID: 48895
				public static LocString NAME = "Composted";

				// Token: 0x0400BF00 RID: 48896
				public static LocString TOOLTIP = "The selected object is currently in the compost";
			}

			// Token: 0x02002EA3 RID: 11939
			public class BURIEDITEM
			{
				// Token: 0x0400BF01 RID: 48897
				public static LocString NAME = "Buried Object";

				// Token: 0x0400BF02 RID: 48898
				public static LocString TOOLTIP = "Something seems to be hidden here";

				// Token: 0x0400BF03 RID: 48899
				public static LocString NOTIFICATION = "Buried object discovered";

				// Token: 0x0400BF04 RID: 48900
				public static LocString NOTIFICATION_TOOLTIP = "My Duplicants have uncovered a {Uncoverable}!\n\n" + UI.CLICK(UI.ClickType.Click) + " to jump to its location.";
			}

			// Token: 0x02002EA4 RID: 11940
			public class GENETICANALYSISCOMPLETED
			{
				// Token: 0x0400BF05 RID: 48901
				public static LocString NAME = "Genome Sequenced";

				// Token: 0x0400BF06 RID: 48902
				public static LocString TOOLTIP = "This Station has sequenced a new seed mutation";
			}

			// Token: 0x02002EA5 RID: 11941
			public class HEALTHSTATUS
			{
				// Token: 0x020031E5 RID: 12773
				public class PERFECT
				{
					// Token: 0x0400C74E RID: 51022
					public static LocString NAME = "None";

					// Token: 0x0400C74F RID: 51023
					public static LocString TOOLTIP = "This Duplicant is in peak condition";
				}

				// Token: 0x020031E6 RID: 12774
				public class ALRIGHT
				{
					// Token: 0x0400C750 RID: 51024
					public static LocString NAME = "None";

					// Token: 0x0400C751 RID: 51025
					public static LocString TOOLTIP = "This Duplicant is none the worse for wear";
				}

				// Token: 0x020031E7 RID: 12775
				public class SCUFFED
				{
					// Token: 0x0400C752 RID: 51026
					public static LocString NAME = "Minor";

					// Token: 0x0400C753 RID: 51027
					public static LocString TOOLTIP = "This Duplicant has a few scrapes and bruises";
				}

				// Token: 0x020031E8 RID: 12776
				public class INJURED
				{
					// Token: 0x0400C754 RID: 51028
					public static LocString NAME = "Moderate";

					// Token: 0x0400C755 RID: 51029
					public static LocString TOOLTIP = "This Duplicant needs some patching up";
				}

				// Token: 0x020031E9 RID: 12777
				public class CRITICAL
				{
					// Token: 0x0400C756 RID: 51030
					public static LocString NAME = "Severe";

					// Token: 0x0400C757 RID: 51031
					public static LocString TOOLTIP = "This Duplicant is in serious need of medical attention";
				}

				// Token: 0x020031EA RID: 12778
				public class INCAPACITATED
				{
					// Token: 0x0400C758 RID: 51032
					public static LocString NAME = "Paralyzing";

					// Token: 0x0400C759 RID: 51033
					public static LocString TOOLTIP = "This Duplicant will die if they do not receive medical attention";
				}

				// Token: 0x020031EB RID: 12779
				public class DEAD
				{
					// Token: 0x0400C75A RID: 51034
					public static LocString NAME = "Conclusive";

					// Token: 0x0400C75B RID: 51035
					public static LocString TOOLTIP = "This Duplicant won't be getting back up";
				}
			}

			// Token: 0x02002EA6 RID: 11942
			public class HIT
			{
				// Token: 0x0400BF07 RID: 48903
				public static LocString NAME = "{targetName} took {damageAmount} damage from {attackerName}'s attack!";
			}

			// Token: 0x02002EA7 RID: 11943
			public class OREMASS
			{
				// Token: 0x0400BF08 RID: 48904
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALMASS.NAME;

				// Token: 0x0400BF09 RID: 48905
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALMASS.TOOLTIP;
			}

			// Token: 0x02002EA8 RID: 11944
			public class ORETEMP
			{
				// Token: 0x0400BF0A RID: 48906
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.NAME;

				// Token: 0x0400BF0B RID: 48907
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.TOOLTIP;
			}

			// Token: 0x02002EA9 RID: 11945
			public class TREEFILTERABLETAGS
			{
				// Token: 0x0400BF0C RID: 48908
				public static LocString NAME = "{Tags}";

				// Token: 0x0400BF0D RID: 48909
				public static LocString TOOLTIP = "{Tags}";
			}

			// Token: 0x02002EAA RID: 11946
			public class SPOUTOVERPRESSURE
			{
				// Token: 0x0400BF0E RID: 48910
				public static LocString NAME = "Overpressure {StudiedDetails}";

				// Token: 0x0400BF0F RID: 48911
				public static LocString TOOLTIP = "Spout cannot vent due to high environmental pressure";

				// Token: 0x0400BF10 RID: 48912
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x02002EAB RID: 11947
			public class SPOUTEMITTING
			{
				// Token: 0x0400BF11 RID: 48913
				public static LocString NAME = "Venting {StudiedDetails}";

				// Token: 0x0400BF12 RID: 48914
				public static LocString TOOLTIP = "This geyser is erupting";

				// Token: 0x0400BF13 RID: 48915
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x02002EAC RID: 11948
			public class SPOUTPRESSUREBUILDING
			{
				// Token: 0x0400BF14 RID: 48916
				public static LocString NAME = "Rising pressure {StudiedDetails}";

				// Token: 0x0400BF15 RID: 48917
				public static LocString TOOLTIP = "This geyser's internal pressure is steadily building";

				// Token: 0x0400BF16 RID: 48918
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x02002EAD RID: 11949
			public class SPOUTIDLE
			{
				// Token: 0x0400BF17 RID: 48919
				public static LocString NAME = "Idle {StudiedDetails}";

				// Token: 0x0400BF18 RID: 48920
				public static LocString TOOLTIP = "This geyser is not currently erupting";

				// Token: 0x0400BF19 RID: 48921
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x02002EAE RID: 11950
			public class SPOUTDORMANT
			{
				// Token: 0x0400BF1A RID: 48922
				public static LocString NAME = "Dormant";

				// Token: 0x0400BF1B RID: 48923
				public static LocString TOOLTIP = "This geyser's geoactivity has halted\n\nIt won't erupt again for some time";
			}

			// Token: 0x02002EAF RID: 11951
			public class SPICEDFOOD
			{
				// Token: 0x0400BF1C RID: 48924
				public static LocString NAME = "Seasoned";

				// Token: 0x0400BF1D RID: 48925
				public static LocString TOOLTIP = "This food has been improved with spice from the " + BUILDINGS.PREFABS.SPICEGRINDER.NAME;
			}

			// Token: 0x02002EB0 RID: 11952
			public class PICKUPABLEUNREACHABLE
			{
				// Token: 0x0400BF1E RID: 48926
				public static LocString NAME = "Unreachable";

				// Token: 0x0400BF1F RID: 48927
				public static LocString TOOLTIP = "Duplicants cannot reach this object";
			}

			// Token: 0x02002EB1 RID: 11953
			public class PRIORITIZED
			{
				// Token: 0x0400BF20 RID: 48928
				public static LocString NAME = "High Priority";

				// Token: 0x0400BF21 RID: 48929
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Errand",
					UI.PST_KEYWORD,
					" has been marked as important and will be preferred over other pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002EB2 RID: 11954
			public class USING
			{
				// Token: 0x0400BF22 RID: 48930
				public static LocString NAME = "Using {Target}";

				// Token: 0x0400BF23 RID: 48931
				public static LocString TOOLTIP = "{Target} is currently in use";
			}

			// Token: 0x02002EB3 RID: 11955
			public class ORDERATTACK
			{
				// Token: 0x0400BF24 RID: 48932
				public static LocString NAME = "Pending Attack";

				// Token: 0x0400BF25 RID: 48933
				public static LocString TOOLTIP = "Waiting for a Duplicant to murderize this defenseless " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
			}

			// Token: 0x02002EB4 RID: 11956
			public class ORDERCAPTURE
			{
				// Token: 0x0400BF26 RID: 48934
				public static LocString NAME = "Pending Wrangle";

				// Token: 0x0400BF27 RID: 48935
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Waiting for a Duplicant to capture this ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"\n\nOnly Duplicants with the ",
					DUPLICANTS.ROLES.RANCHER.NAME,
					" skill can catch critters without traps"
				});
			}

			// Token: 0x02002EB5 RID: 11957
			public class OPERATING
			{
				// Token: 0x0400BF28 RID: 48936
				public static LocString NAME = "In Use";

				// Token: 0x0400BF29 RID: 48937
				public static LocString TOOLTIP = "This object is currently being used";
			}

			// Token: 0x02002EB6 RID: 11958
			public class CLEANING
			{
				// Token: 0x0400BF2A RID: 48938
				public static LocString NAME = "Cleaning";

				// Token: 0x0400BF2B RID: 48939
				public static LocString TOOLTIP = "This building is currently being cleaned";
			}

			// Token: 0x02002EB7 RID: 11959
			public class REGIONISBLOCKED
			{
				// Token: 0x0400BF2C RID: 48940
				public static LocString NAME = "Blocked";

				// Token: 0x0400BF2D RID: 48941
				public static LocString TOOLTIP = "Undug material is blocking off an essential tile";
			}

			// Token: 0x02002EB8 RID: 11960
			public class STUDIED
			{
				// Token: 0x0400BF2E RID: 48942
				public static LocString NAME = "Analysis Complete";

				// Token: 0x0400BF2F RID: 48943
				public static LocString TOOLTIP = "Information on this Natural Feature has been compiled below.";
			}

			// Token: 0x02002EB9 RID: 11961
			public class AWAITINGSTUDY
			{
				// Token: 0x0400BF30 RID: 48944
				public static LocString NAME = "Analysis Pending";

				// Token: 0x0400BF31 RID: 48945
				public static LocString TOOLTIP = "New information on this Natural Feature will be compiled once the field study is complete";
			}

			// Token: 0x02002EBA RID: 11962
			public class DURABILITY
			{
				// Token: 0x0400BF32 RID: 48946
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400BF33 RID: 48947
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x02002EBB RID: 11963
			public class STOREDITEMDURABILITY
			{
				// Token: 0x0400BF34 RID: 48948
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400BF35 RID: 48949
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x02002EBC RID: 11964
			public class ARTIFACTENTOMBED
			{
				// Token: 0x0400BF36 RID: 48950
				public static LocString NAME = "Entombed Artifact";

				// Token: 0x0400BF37 RID: 48951
				public static LocString TOOLTIP = "This artifact is trapped in an obscuring shell limiting its decor. A skilled artist can remove it at the " + BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME;
			}

			// Token: 0x02002EBD RID: 11965
			public class TEAROPEN
			{
				// Token: 0x0400BF38 RID: 48952
				public static LocString NAME = "Temporal Tear open";

				// Token: 0x0400BF39 RID: 48953
				public static LocString TOOLTIP = "An open passage through spacetime";
			}

			// Token: 0x02002EBE RID: 11966
			public class TEARCLOSED
			{
				// Token: 0x0400BF3A RID: 48954
				public static LocString NAME = "Temporal Tear closed";

				// Token: 0x0400BF3B RID: 48955
				public static LocString TOOLTIP = "Perhaps some technology could open the passage";
			}

			// Token: 0x02002EBF RID: 11967
			public class MARKEDFORMOVE
			{
				// Token: 0x0400BF3C RID: 48956
				public static LocString NAME = "Pending Move";

				// Token: 0x0400BF3D RID: 48957
				public static LocString TOOLTIP = "Waiting for a Duplicant to move this object";
			}

			// Token: 0x02002EC0 RID: 11968
			public class MOVESTORAGEUNREACHABLE
			{
				// Token: 0x0400BF3E RID: 48958
				public static LocString NAME = "Unreachable Move";

				// Token: 0x0400BF3F RID: 48959
				public static LocString TOOLTIP = "Duplicants cannot reach this object to move it";
			}
		}

		// Token: 0x02001EC6 RID: 7878
		public class POPFX
		{
			// Token: 0x04008BF2 RID: 35826
			public static LocString RESOURCE_EATEN = "Resource Eaten";
		}

		// Token: 0x02001EC7 RID: 7879
		public class NOTIFICATIONS
		{
			// Token: 0x02002EC1 RID: 11969
			public class BASICCONTROLS
			{
				// Token: 0x0400BF40 RID: 48960
				public static LocString NAME = "Tutorial: Basic Controls";

				// Token: 0x0400BF41 RID: 48961
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"• I can use ",
					UI.FormatAsHotKey(global::Action.PanLeft),
					" and ",
					UI.FormatAsHotKey(global::Action.PanRight),
					" to pan my view left and right, and ",
					UI.FormatAsHotKey(global::Action.PanUp),
					" and ",
					UI.FormatAsHotKey(global::Action.PanDown),
					" to pan up and down.\n\n• ",
					UI.FormatAsHotKey(global::Action.ZoomIn),
					" lets me zoom in, and ",
					UI.FormatAsHotKey(global::Action.ZoomOut),
					" zooms out.\n\n• ",
					UI.FormatAsHotKey(global::Action.CameraHome),
					" returns my view to the Printing Pod.\n\n• I can speed or slow my perception of time using the top left corner buttons, or by pressing ",
					UI.FormatAsHotKey(global::Action.SpeedUp),
					" or ",
					UI.FormatAsHotKey(global::Action.SlowDown),
					". Pressing ",
					UI.FormatAsHotKey(global::Action.TogglePause),
					" will pause the flow of time entirely.\n\n• I'll keep records of everything I discover in my personal DATABASE ",
					UI.FormatAsHotKey(global::Action.ManageDatabase),
					" to refer back to if I forget anything important."
				});

				// Token: 0x0400BF42 RID: 48962
				public static LocString MESSAGEBODYALT = string.Concat(new string[]
				{
					"• I can use ",
					UI.FormatAsHotKey(global::Action.AnalogCamera),
					" to pan my view.\n\n• ",
					UI.FormatAsHotKey(global::Action.ZoomIn),
					" lets me zoom in, and ",
					UI.FormatAsHotKey(global::Action.ZoomOut),
					" zooms out.\n\n• I can speed or slow my perception of time using the top left corner buttons, or by pressing ",
					UI.FormatAsHotKey(global::Action.CycleSpeed),
					". Pressing ",
					UI.FormatAsHotKey(global::Action.TogglePause),
					" will pause the flow of time entirely.\n\n• I'll keep records of everything I discover in my personal DATABASE ",
					UI.FormatAsHotKey(global::Action.ManageDatabase),
					" to refer back to if I forget anything important."
				});

				// Token: 0x0400BF43 RID: 48963
				public static LocString TOOLTIP = "Notes on using my HUD";
			}

			// Token: 0x02002EC2 RID: 11970
			public class CODEXUNLOCK
			{
				// Token: 0x0400BF44 RID: 48964
				public static LocString NAME = "New Log Entry";

				// Token: 0x0400BF45 RID: 48965
				public static LocString MESSAGEBODY = "I've added a new log entry to my Database";

				// Token: 0x0400BF46 RID: 48966
				public static LocString TOOLTIP = "I've added a new log entry to my Database";
			}

			// Token: 0x02002EC3 RID: 11971
			public class WELCOMEMESSAGE
			{
				// Token: 0x0400BF47 RID: 48967
				public static LocString NAME = "Tutorial: Colony Management";

				// Token: 0x0400BF48 RID: 48968
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"I can use the ",
					UI.FormatAsTool("Dig Tool", global::Action.Dig),
					" and the ",
					UI.FormatAsBuildMenuTab("Build Menu"),
					" in the lower left of the screen to begin planning my first construction tasks.\n\nOnce I've placed a few errands my Duplicants will automatically get to work, without me needing to direct them individually."
				});

				// Token: 0x0400BF49 RID: 48969
				public static LocString TOOLTIP = "Notes on getting Duplicants to do my bidding";
			}

			// Token: 0x02002EC4 RID: 11972
			public class STRESSMANAGEMENTMESSAGE
			{
				// Token: 0x0400BF4A RID: 48970
				public static LocString NAME = "Tutorial: Stress Management";

				// Token: 0x0400BF4B RID: 48971
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"At 100% ",
					UI.FormatAsLink("Stress", "STRESS"),
					", a Duplicant will have a nervous breakdown and be unable to work.\n\nBreakdowns can manifest in different colony-threatening ways, such as the destruction of buildings or the binge eating of food.\n\nI can select a Duplicant and mouse over ",
					UI.FormatAsLink("Stress", "STRESS"),
					" in their STATUS TAB to view their individual ",
					UI.FormatAsLink("Stress Factors", "STRESS"),
					", and hopefully minimize them before they become a problem."
				});

				// Token: 0x0400BF4C RID: 48972
				public static LocString TOOLTIP = "Notes on keeping Duplicants happy and productive";
			}

			// Token: 0x02002EC5 RID: 11973
			public class TASKPRIORITIESMESSAGE
			{
				// Token: 0x0400BF4D RID: 48973
				public static LocString NAME = "Tutorial: Priority";

				// Token: 0x0400BF4E RID: 48974
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants always perform errands in order of highest to lowest priority. They will harvest ",
					UI.FormatAsLink("Food", "FOOD"),
					" before they build, for example, or always build new structures before they mine materials.\n\nI can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set which Errand Types Duplicants may or may not perform, or to specialize skilled Duplicants for particular Errand Types."
				});

				// Token: 0x0400BF4F RID: 48975
				public static LocString TOOLTIP = "Notes on managing Duplicants' errands";
			}

			// Token: 0x02002EC6 RID: 11974
			public class MOPPINGMESSAGE
			{
				// Token: 0x0400BF50 RID: 48976
				public static LocString NAME = "Tutorial: Polluted Water";

				// Token: 0x0400BF51 RID: 48977
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" slowly emits ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					" which accelerates the spread of ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nDuplicants will also be ",
					UI.FormatAsLink("Stressed", "STRESS"),
					" by walking through Polluted Water, so I should have my Duplicants clean up spills by ",
					UI.CLICK(UI.ClickType.clicking),
					" and dragging the ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop)
				});

				// Token: 0x0400BF52 RID: 48978
				public static LocString TOOLTIP = "Notes on handling polluted materials";
			}

			// Token: 0x02002EC7 RID: 11975
			public class LOCOMOTIONMESSAGE
			{
				// Token: 0x0400BF53 RID: 48979
				public static LocString NAME = "Video: Duplicant Movement";

				// Token: 0x0400BF54 RID: 48980
				public static LocString MESSAGEBODY = "Duplicants have limited jumping and climbing abilities. They can only climb two tiles high and cannot fit into spaces shorter than two tiles, or cross gaps wider than one tile. I should keep this in mind while placing errands.\n\nTo check if an errand I've placed is accessible, I can select a Duplicant and " + UI.CLICK(UI.ClickType.click) + " <b>Show Navigation</b> to view all areas within their reach.";

				// Token: 0x0400BF55 RID: 48981
				public static LocString TOOLTIP = "Notes on my Duplicants' maneuverability";
			}

			// Token: 0x02002EC8 RID: 11976
			public class PRIORITIESMESSAGE
			{
				// Token: 0x0400BF56 RID: 48982
				public static LocString NAME = "Tutorial: Errand Priorities";

				// Token: 0x0400BF57 RID: 48983
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants will choose where to work based on the priority of the errands that I give them. I can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set their ",
					UI.PRE_KEYWORD,
					"Duplicant Priorities",
					UI.PST_KEYWORD,
					", and the ",
					UI.FormatAsTool("Priority Tool", global::Action.Prioritize),
					" to fine tune ",
					UI.PRE_KEYWORD,
					"Building Priority",
					UI.PST_KEYWORD,
					". Many buildings will also let me change their Priority level when I select them."
				});

				// Token: 0x0400BF58 RID: 48984
				public static LocString TOOLTIP = "Notes on my Duplicants' priorities";
			}

			// Token: 0x02002EC9 RID: 11977
			public class FETCHINGWATERMESSAGE
			{
				// Token: 0x0400BF59 RID: 48985
				public static LocString NAME = "Tutorial: Fetching Water";

				// Token: 0x0400BF5A RID: 48986
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"By building a ",
					UI.FormatAsLink("Pitcher Pump", "LIQUIDPUMPINGSTATION"),
					" from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					" over a pool of liquid, my Duplicants will be able to bottle it up and manually deliver it wherever it needs to go."
				});

				// Token: 0x0400BF5B RID: 48987
				public static LocString TOOLTIP = "Notes on liquid resource gathering";
			}

			// Token: 0x02002ECA RID: 11978
			public class SCHEDULEMESSAGE
			{
				// Token: 0x0400BF5C RID: 48988
				public static LocString NAME = "Tutorial: Scheduling";

				// Token: 0x0400BF5D RID: 48989
				public static LocString MESSAGEBODY = "My Duplicants will only eat, sleep, work, or bathe during the times I allot for such activities.\n\nTo make the best use of their time, I can open the " + UI.FormatAsManagementMenu("Schedule Tab", global::Action.ManageSchedule) + " to adjust the colony's schedule and plan how they should utilize their day.";

				// Token: 0x0400BF5E RID: 48990
				public static LocString TOOLTIP = "Notes on scheduling my Duplicants' time";
			}

			// Token: 0x02002ECB RID: 11979
			public class THERMALCOMFORT
			{
				// Token: 0x0400BF5F RID: 48991
				public static LocString NAME = "Tutorial: Duplicant Temperature";

				// Token: 0x0400BF60 RID: 48992
				public static LocString TOOLTIP = "Notes on helping Duplicants keep their cool";

				// Token: 0x0400BF61 RID: 48993
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Environments that are extremely ",
					UI.FormatAsLink("Hot", "HEAT"),
					" or ",
					UI.FormatAsLink("Cold", "HEAT"),
					" affect my Duplicants' internal body temperature and cause undue ",
					UI.FormatAsLink("Stress", "STRESS"),
					".\n\nOpening the ",
					UI.FormatAsOverlay("Temperature Overlay", global::Action.Overlay3),
					" and checking the <b>Thermal Tolerance</b> box allows me to view all areas where my Duplicants will feel discomfort and be unable to regulate their internal body temperature."
				});
			}

			// Token: 0x02002ECC RID: 11980
			public class TUTORIAL_OVERHEATING
			{
				// Token: 0x0400BF62 RID: 48994
				public static LocString NAME = "Tutorial: Building Temperature";

				// Token: 0x0400BF63 RID: 48995
				public static LocString TOOLTIP = "Notes on preventing building from breaking";

				// Token: 0x0400BF64 RID: 48996
				public static LocString MESSAGEBODY = "When constructing buildings, I should always take note of their " + UI.FormatAsLink("Overheat Temperature", "HEAT") + " and plan their locations accordingly. Maintaining low ambient temperatures and good ventilation in the colony will also help keep building temperatures down.\n\nIf I allow buildings to exceed their Overheat Temperature they will begin to take damage, and if left unattended, they will break down and be unusable until repaired.";
			}

			// Token: 0x02002ECD RID: 11981
			public class LOTS_OF_GERMS
			{
				// Token: 0x0400BF65 RID: 48997
				public static LocString NAME = "Tutorial: Germs and Disease";

				// Token: 0x0400BF66 RID: 48998
				public static LocString TOOLTIP = "Notes on Duplicant disease risks";

				// Token: 0x0400BF67 RID: 48999
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" such as ",
					UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
					" and ",
					UI.FormatAsLink("Slimelung", "SLIMESICKNESS"),
					" can cause ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" in my Duplicants. I can use the ",
					UI.FormatAsOverlay("Germ Overlay", global::Action.Overlay9),
					" to view all germ concentrations in my colony, and even detect the sources spawning them.\n\nBuilding Wash Basins from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8),
					" near colony toilets will tell my Duplicants they need to wash up."
				});
			}

			// Token: 0x02002ECE RID: 11982
			public class BEING_INFECTED
			{
				// Token: 0x0400BF68 RID: 49000
				public static LocString NAME = "Tutorial: Immune Systems";

				// Token: 0x0400BF69 RID: 49001
				public static LocString TOOLTIP = "Notes on keeping Duplicants in peak health";

				// Token: 0x0400BF6A RID: 49002
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When Duplicants come into contact with various ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", they'll need to expend points of ",
					UI.FormatAsLink("Immunity", "IMMUNE SYSTEM"),
					" to resist them and remain healthy. If repeated exposes causes their Immunity to drop to 0%, they'll be unable to resist germs and will contract the next disease they encounter.\n\nDoors with Access Permissions can be built from the BASE TAB<color=#F44A47> <b>[1]</b></color> of the ",
					UI.FormatAsLink("Build menu", "misc"),
					" to block Duplicants from entering biohazardous areas while they recover their spent immunity points."
				});
			}

			// Token: 0x02002ECF RID: 11983
			public class DISEASE_COOKING
			{
				// Token: 0x0400BF6B RID: 49003
				public static LocString NAME = "Tutorial: Food Safety";

				// Token: 0x0400BF6C RID: 49004
				public static LocString TOOLTIP = "Notes on managing food contamination";

				// Token: 0x0400BF6D RID: 49005
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsLink("Food", "FOOD"),
					" my Duplicants cook will only ever be as clean as the ingredients used to make it. Storing food in sterile or ",
					UI.FormatAsLink("Refrigerated", "REFRIGERATOR"),
					" environments will keep food free of ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", while carefully placed hygiene stations like ",
					BUILDINGS.PREFABS.WASHBASIN.NAME,
					" or ",
					BUILDINGS.PREFABS.SHOWER.NAME,
					" will prevent the cooks from infecting the food by handling it.\n\nDangerously contaminated food can be sent to compost by ",
					UI.CLICK(UI.ClickType.clicking),
					" the <b>Compost</b> button on the selected item."
				});
			}

			// Token: 0x02002ED0 RID: 11984
			public class SUITS
			{
				// Token: 0x0400BF6E RID: 49006
				public static LocString NAME = "Tutorial: Atmo Suits";

				// Token: 0x0400BF6F RID: 49007
				public static LocString TOOLTIP = "Notes on using atmo suits";

				// Token: 0x0400BF70 RID: 49008
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					" can be equipped to protect my Duplicants from environmental hazards like extreme ",
					UI.FormatAsLink("Heat", "Heat"),
					", airborne ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", or unbreathable ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					". In order to utilize these suits, I'll need to hook up an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" to an ",
					UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER"),
					" , then store one of the suits inside.\n\nDuplicants will equip a suit when they walk past the checkpoint in the chosen direction, and will unequip their suit when walking back the opposite way."
				});
			}

			// Token: 0x02002ED1 RID: 11985
			public class RADIATION
			{
				// Token: 0x0400BF71 RID: 49009
				public static LocString NAME = "Tutorial: Radiation";

				// Token: 0x0400BF72 RID: 49010
				public static LocString TOOLTIP = "Notes on managing radiation";

				// Token: 0x0400BF73 RID: 49011
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Objects such as ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" and ",
					UI.FormatAsLink("Beeta Hives", "BEE"),
					" emit a ",
					UI.FormatAsLink("Radioactive", "RADIOACTIVE"),
					" energy that can be toxic to my Duplicants.\n\nI can use the ",
					UI.FormatAsOverlay("Radiation Overlay"),
					" ",
					UI.FormatAsHotKey(global::Action.Overlay15),
					" to check the scope of the Radiation field. Building thick walls around radiation emitters will dampen the field and protect my Duplicants from getting ",
					UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
					" ."
				});
			}

			// Token: 0x02002ED2 RID: 11986
			public class SPACETRAVEL
			{
				// Token: 0x0400BF74 RID: 49012
				public static LocString NAME = "Tutorial: Space Travel";

				// Token: 0x0400BF75 RID: 49013
				public static LocString TOOLTIP = "Notes on traveling in space";

				// Token: 0x0400BF76 RID: 49014
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Building a rocket first requires constructing a ",
					UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
					" and adding modules from the menu. All components of the Rocket Checklist will need to be complete before being capable of launching.\n\nA ",
					UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE"),
					" needs to be built on the surface of a Planetoid in order to use the ",
					UI.PRE_KEYWORD,
					"Starmap Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageStarmap),
					" to see and set course for new destinations."
				});
			}

			// Token: 0x02002ED3 RID: 11987
			public class MORALE
			{
				// Token: 0x0400BF77 RID: 49015
				public static LocString NAME = "Video: Duplicant Morale";

				// Token: 0x0400BF78 RID: 49016
				public static LocString TOOLTIP = "Notes on Duplicant expectations";

				// Token: 0x0400BF79 RID: 49017
				public static LocString MESSAGEBODY = "Food, Rooms, Decor, and Recreation all have an effect on Duplicant Morale. Good experiences improve their Morale, while poor experiences lower it. When a Duplicant's Morale is below their Expectations, they will become Stressed.\n\nDuplicants' Expectations will get higher as they are given new Skills, and the colony will have to be improved to keep up their Morale. An overview of Morale and Stress can be viewed on the Vitals screen.";
			}

			// Token: 0x02002ED4 RID: 11988
			public class POWER
			{
				// Token: 0x0400BF7A RID: 49018
				public static LocString NAME = "Video: Power Circuits";

				// Token: 0x0400BF7B RID: 49019
				public static LocString TOOLTIP = "Notes on managing electricity";

				// Token: 0x0400BF7C RID: 49020
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Generators are considered \"Producers\" of Power, while the various buildings and machines in the colony are considered \"Consumers\". Each Consumer will pull a certain wattage from the power circuit it is connected to, which can be checked at any time by ",
					UI.CLICK(UI.ClickType.clicking),
					" the building and going to the Energy Tab.\n\nI can use the Power Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay2),
					" to quickly check the status of all my circuits. If the Consumers are taking more wattage than the Generators are creating, the Batteries will drain and there will be brownouts.\n\nAdditionally, if the Consumers are pulling more wattage through the Wires than the Wires can handle, they will overload and burn out. To correct both these situations, I will need to reorganize my Consumers onto separate circuits."
				});
			}

			// Token: 0x02002ED5 RID: 11989
			public class DIGGING
			{
				// Token: 0x0400BF7D RID: 49021
				public static LocString NAME = "Video: Digging for Resources";

				// Token: 0x0400BF7E RID: 49022
				public static LocString TOOLTIP = "Notes on buried riches";

				// Token: 0x0400BF7F RID: 49023
				public static LocString MESSAGEBODY = "Everything a colony needs to get going is found in the ground. Instructing Duplicants to dig out areas means we can find food, mine resources to build infrastructure, and clear space for the colony to grow. I can access the Dig Tool with " + UI.FormatAsHotKey(global::Action.Dig) + ", which allows me to select the area where I want my Duplicants to dig.\n\nDuplicants will need to gain the Superhard Digging skill to mine Abyssalite and the Superduperhard Digging skill to mine Diamond and Obsidian. Without the proper skills, these materials will be undiggable.";
			}

			// Token: 0x02002ED6 RID: 11990
			public class INSULATION
			{
				// Token: 0x0400BF80 RID: 49024
				public static LocString NAME = "Video: Insulation";

				// Token: 0x0400BF81 RID: 49025
				public static LocString TOOLTIP = "Notes on effective temperature management";

				// Token: 0x0400BF82 RID: 49026
				public static LocString MESSAGEBODY = "The temperature of an environment can have positive or negative effects on the well-being of my Duplicants, as well as the plants and critters in my colony. Selecting " + UI.FormatAsHotKey(global::Action.Overlay3) + " will open the Temperature Overlay where I can check for any hot or cold spots.\n\nI can use a Utility building like an Ice-E Fan or a Space Heater to make an area colder or warmer. However, I will have limited success changing the temperature of a room unless I build the area with insulating tiles to prevent cold or warm air from escaping.";
			}

			// Token: 0x02002ED7 RID: 11991
			public class PLUMBING
			{
				// Token: 0x0400BF83 RID: 49027
				public static LocString NAME = "Video: Plumbing and Ventilation";

				// Token: 0x0400BF84 RID: 49028
				public static LocString TOOLTIP = "Notes on connecting buildings with pipes";

				// Token: 0x0400BF85 RID: 49029
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When connecting pipes for plumbing, it is useful to have the Plumbing Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay6),
					" selected. Each building which requires plumbing must have their Building Intake connected to the Output Pipe from a source such as a Liquid Pump. Liquid Pumps must be submerged in liquid and attached to a power source to function.\n\nBuildings often output contaminated water which must flow out of the building through piping from the Output Pipe. The water can then be expelled through a Liquid Vent, or filtered through a Water Sieve for reuse.\n\nVentilation applies the same principles to gases. Select the Ventilation Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay7),
					" to see how gases are being moved around the colony."
				});
			}

			// Token: 0x02002ED8 RID: 11992
			public class NEW_AUTOMATION_WARNING
			{
				// Token: 0x0400BF86 RID: 49030
				public static LocString NAME = "New Automation Port";

				// Token: 0x0400BF87 RID: 49031
				public static LocString TOOLTIP = "This building has a new automation port and is unintentionally connected to an existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME;
			}

			// Token: 0x02002ED9 RID: 11993
			public class DTU
			{
				// Token: 0x0400BF88 RID: 49032
				public static LocString NAME = "Tutorial: Duplicant Thermal Units";

				// Token: 0x0400BF89 RID: 49033
				public static LocString TOOLTIP = "Notes on measuring heat energy";

				// Token: 0x0400BF8A RID: 49034
				public static LocString MESSAGEBODY = "My Duplicants measure heat energy in Duplicant Thermal Units or DTU.\n\n1 DTU = 1055.06 J";
			}

			// Token: 0x02002EDA RID: 11994
			public class NOMESSAGES
			{
				// Token: 0x0400BF8B RID: 49035
				public static LocString NAME = "";

				// Token: 0x0400BF8C RID: 49036
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EDB RID: 11995
			public class NOALERTS
			{
				// Token: 0x0400BF8D RID: 49037
				public static LocString NAME = "";

				// Token: 0x0400BF8E RID: 49038
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EDC RID: 11996
			public class NEWTRAIT
			{
				// Token: 0x0400BF8F RID: 49039
				public static LocString NAME = "{0} has developed a trait";

				// Token: 0x0400BF90 RID: 49040
				public static LocString TOOLTIP = "{0} has developed the trait(s):\n    • {1}";
			}

			// Token: 0x02002EDD RID: 11997
			public class RESEARCHCOMPLETE
			{
				// Token: 0x0400BF91 RID: 49041
				public static LocString NAME = "Research Complete";

				// Token: 0x0400BF92 RID: 49042
				public static LocString MESSAGEBODY = "Eureka! We've discovered {0} Technology.\n\nNew buildings have become available:\n  • {1}";

				// Token: 0x0400BF93 RID: 49043
				public static LocString TOOLTIP = "{0} research complete!";
			}

			// Token: 0x02002EDE RID: 11998
			public class WORLDDETECTED
			{
				// Token: 0x0400BF94 RID: 49044
				public static LocString NAME = "New " + UI.CLUSTERMAP.PLANETOID + " detected";

				// Token: 0x0400BF95 RID: 49045
				public static LocString MESSAGEBODY = "My Duplicants' astronomical efforts have uncovered a new " + UI.CLUSTERMAP.PLANETOID + ":\n{0}";

				// Token: 0x0400BF96 RID: 49046
				public static LocString TOOLTIP = "{0} discovered";
			}

			// Token: 0x02002EDF RID: 11999
			public class SKILL_POINT_EARNED
			{
				// Token: 0x0400BF97 RID: 49047
				public static LocString NAME = "{Duplicant} earned a skill point!";

				// Token: 0x0400BF98 RID: 49048
				public static LocString MESSAGEBODY = "These Duplicants have Skill Points that can be spent on new abilities:\n{0}";

				// Token: 0x0400BF99 RID: 49049
				public static LocString LINE = "\n• <b>{0}</b>";

				// Token: 0x0400BF9A RID: 49050
				public static LocString TOOLTIP = "{Duplicant} has been working hard and is ready to learn a new skill";
			}

			// Token: 0x02002EE0 RID: 12000
			public class DUPLICANTABSORBED
			{
				// Token: 0x0400BF9B RID: 49051
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400BF9C RID: 49052
				public static LocString MESSAGEBODY = "The Printing Pod is no longer available for printing.\nCountdown to the next production has been rebooted.";

				// Token: 0x0400BF9D RID: 49053
				public static LocString TOOLTIP = "Printing countdown rebooted";
			}

			// Token: 0x02002EE1 RID: 12001
			public class DUPLICANTDIED
			{
				// Token: 0x0400BF9E RID: 49054
				public static LocString NAME = "Duplicants have died";

				// Token: 0x0400BF9F RID: 49055
				public static LocString TOOLTIP = "These Duplicants have died:";
			}

			// Token: 0x02002EE2 RID: 12002
			public class FOODROT
			{
				// Token: 0x0400BFA0 RID: 49056
				public static LocString NAME = "Food has decayed";

				// Token: 0x0400BFA1 RID: 49057
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have rotted and are no longer edible:{0}";
			}

			// Token: 0x02002EE3 RID: 12003
			public class FOODSTALE
			{
				// Token: 0x0400BFA2 RID: 49058
				public static LocString NAME = "Food has become stale";

				// Token: 0x0400BFA3 RID: 49059
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have become stale and could rot if not stored:";
			}

			// Token: 0x02002EE4 RID: 12004
			public class YELLOWALERT
			{
				// Token: 0x0400BFA4 RID: 49060
				public static LocString NAME = "Yellow Alert";

				// Token: 0x0400BFA5 RID: 49061
				public static LocString TOOLTIP = "The colony has some top priority tasks to complete before resuming a normal schedule";
			}

			// Token: 0x02002EE5 RID: 12005
			public class REDALERT
			{
				// Token: 0x0400BFA6 RID: 49062
				public static LocString NAME = "Red Alert";

				// Token: 0x0400BFA7 RID: 49063
				public static LocString TOOLTIP = "The colony is prioritizing work over their individual well-being";
			}

			// Token: 0x02002EE6 RID: 12006
			public class REACTORMELTDOWN
			{
				// Token: 0x0400BFA8 RID: 49064
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400BFA9 RID: 49065
				public static LocString TOOLTIP = "A Research Reactor has overheated and is melting down! Extreme radiation is flooding the area";
			}

			// Token: 0x02002EE7 RID: 12007
			public class HEALING
			{
				// Token: 0x0400BFAA RID: 49066
				public static LocString NAME = "Healing";

				// Token: 0x0400BFAB RID: 49067
				public static LocString TOOLTIP = "This Duplicant is recovering from an injury";
			}

			// Token: 0x02002EE8 RID: 12008
			public class UNREACHABLEITEM
			{
				// Token: 0x0400BFAC RID: 49068
				public static LocString NAME = "Unreachable resources";

				// Token: 0x0400BFAD RID: 49069
				public static LocString TOOLTIP = "Duplicants cannot retrieve these resources:";
			}

			// Token: 0x02002EE9 RID: 12009
			public class INVALIDCONSTRUCTIONLOCATION
			{
				// Token: 0x0400BFAE RID: 49070
				public static LocString NAME = "Invalid construction location";

				// Token: 0x0400BFAF RID: 49071
				public static LocString TOOLTIP = "These buildings cannot be constructed in the planned areas:";
			}

			// Token: 0x02002EEA RID: 12010
			public class MISSINGMATERIALS
			{
				// Token: 0x0400BFB0 RID: 49072
				public static LocString NAME = "Missing materials";

				// Token: 0x0400BFB1 RID: 49073
				public static LocString TOOLTIP = "These resources are not available:";
			}

			// Token: 0x02002EEB RID: 12011
			public class BUILDINGOVERHEATED
			{
				// Token: 0x0400BFB2 RID: 49074
				public static LocString NAME = "Damage: Overheated";

				// Token: 0x0400BFB3 RID: 49075
				public static LocString TOOLTIP = "Extreme heat is damaging these buildings:";
			}

			// Token: 0x02002EEC RID: 12012
			public class TILECOLLAPSE
			{
				// Token: 0x0400BFB4 RID: 49076
				public static LocString NAME = "Ceiling Collapse!";

				// Token: 0x0400BFB5 RID: 49077
				public static LocString TOOLTIP = "Falling material fell on these Duplicants and displaced them:";
			}

			// Token: 0x02002EED RID: 12013
			public class NO_OXYGEN_GENERATOR
			{
				// Token: 0x0400BFB6 RID: 49078
				public static LocString NAME = "No " + UI.FormatAsLink("Oxygen Generator", "OXYGEN") + " built";

				// Token: 0x0400BFB7 RID: 49079
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is not producing any new ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					"\n\n",
					UI.FormatAsLink("Oxygen Diffusers", "MINERALDEOXIDIZER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Oxygen Tab", global::Action.Plan2)
				});
			}

			// Token: 0x02002EEE RID: 12014
			public class INSUFFICIENTOXYGENLASTCYCLE
			{
				// Token: 0x0400BFB8 RID: 49080
				public static LocString NAME = "Insufficient Oxygen generation";

				// Token: 0x0400BFB9 RID: 49081
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is consuming more ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" than it is producing, and will run out air if I do not increase production.\n\nI should check my existing oxygen production buildings to ensure they're operating correctly\n\n• ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" produced last cycle: {EmittingRate}\n• Consumed last cycle: {ConsumptionRate}"
				});
			}

			// Token: 0x02002EEF RID: 12015
			public class UNREFRIGERATEDFOOD
			{
				// Token: 0x0400BFBA RID: 49082
				public static LocString NAME = "Unrefrigerated Food";

				// Token: 0x0400BFBB RID: 49083
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items are stored but not refrigerated:\n";
			}

			// Token: 0x02002EF0 RID: 12016
			public class FOODLOW
			{
				// Token: 0x0400BFBC RID: 49084
				public static LocString NAME = "Food shortage";

				// Token: 0x0400BFBD RID: 49085
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony's ",
					UI.FormatAsLink("Food", "FOOD"),
					" reserves are low:\n\n    • {0} are currently available\n    • {1} is being consumed per cycle\n\n",
					UI.FormatAsLink("Microbe Mushers", "MICROBEMUSHER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Food Tab", global::Action.Plan4)
				});
			}

			// Token: 0x02002EF1 RID: 12017
			public class NO_MEDICAL_COTS
			{
				// Token: 0x0400BFBE RID: 49086
				public static LocString NAME = "No " + UI.FormatAsLink("Sick Bay", "DOCTORSTATION") + " built";

				// Token: 0x0400BFBF RID: 49087
				public static LocString TOOLTIP = "There is nowhere for sick Duplicants receive medical care\n\n" + UI.FormatAsLink("Sick Bays", "DOCTORSTATION") + " can be built from the " + UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8);
			}

			// Token: 0x02002EF2 RID: 12018
			public class NEEDTOILET
			{
				// Token: 0x0400BFC0 RID: 49088
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " built";

				// Token: 0x0400BFC1 RID: 49089
				public static LocString TOOLTIP = "My Duplicants have nowhere to relieve themselves\n\n" + UI.FormatAsLink("Outhouses", "OUTHOUSE") + " can be built from the " + UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5);
			}

			// Token: 0x02002EF3 RID: 12019
			public class NEEDFOOD
			{
				// Token: 0x0400BFC2 RID: 49090
				public static LocString NAME = "Colony requires a food source";

				// Token: 0x0400BFC3 RID: 49091
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony will exhaust their supplies without a new ",
					UI.FormatAsLink("Food", "FOOD"),
					" source\n\n",
					UI.FormatAsLink("Microbe Mushers", "MICROBEMUSHER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Food Tab", global::Action.Plan4)
				});
			}

			// Token: 0x02002EF4 RID: 12020
			public class HYGENE_NEEDED
			{
				// Token: 0x0400BFC4 RID: 49092
				public static LocString NAME = "No " + UI.FormatAsLink("Wash Basin", "WASHBASIN") + " built";

				// Token: 0x0400BFC5 RID: 49093
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" are spreading in the colony because my Duplicants have nowhere to clean up\n\n",
					UI.FormatAsLink("Wash Basins", "WASHBASIN"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8)
				});
			}

			// Token: 0x02002EF5 RID: 12021
			public class NEEDSLEEP
			{
				// Token: 0x0400BFC6 RID: 49094
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "COT") + " built";

				// Token: 0x0400BFC7 RID: 49095
				public static LocString TOOLTIP = "My Duplicants would appreciate a place to sleep\n\n" + UI.FormatAsLink("Cots", "COTS") + " can be built from the " + UI.FormatAsBuildMenuTab("Furniture Tab", global::Action.Plan9);
			}

			// Token: 0x02002EF6 RID: 12022
			public class NEEDENERGYSOURCE
			{
				// Token: 0x0400BFC8 RID: 49096
				public static LocString NAME = "Colony requires a " + UI.FormatAsLink("Power", "POWER") + " source";

				// Token: 0x0400BFC9 RID: 49097
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Power", "POWER"),
					" is required to operate electrical buildings\n\n",
					UI.FormatAsLink("Manual Generators", "MANUALGENERATOR"),
					" and ",
					UI.FormatAsLink("Wire", "WIRE"),
					" can be built from the ",
					UI.FormatAsLink("Power Tab", "[3]")
				});
			}

			// Token: 0x02002EF7 RID: 12023
			public class RESOURCEMELTED
			{
				// Token: 0x0400BFCA RID: 49098
				public static LocString NAME = "Resources melted";

				// Token: 0x0400BFCB RID: 49099
				public static LocString TOOLTIP = "These resources have melted:";
			}

			// Token: 0x02002EF8 RID: 12024
			public class VENTOVERPRESSURE
			{
				// Token: 0x0400BFCC RID: 49100
				public static LocString NAME = "Vent overpressurized";

				// Token: 0x0400BFCD RID: 49101
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" systems have exited the ideal ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" range:"
				});
			}

			// Token: 0x02002EF9 RID: 12025
			public class VENTBLOCKED
			{
				// Token: 0x0400BFCE RID: 49102
				public static LocString NAME = "Vent blocked";

				// Token: 0x0400BFCF RID: 49103
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x02002EFA RID: 12026
			public class OUTPUTBLOCKED
			{
				// Token: 0x0400BFD0 RID: 49104
				public static LocString NAME = "Output blocked";

				// Token: 0x0400BFD1 RID: 49105
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x02002EFB RID: 12027
			public class BROKENMACHINE
			{
				// Token: 0x0400BFD2 RID: 49106
				public static LocString NAME = "Building broken";

				// Token: 0x0400BFD3 RID: 49107
				public static LocString TOOLTIP = "These buildings have taken significant damage and are nonfunctional:";
			}

			// Token: 0x02002EFC RID: 12028
			public class STRUCTURALDAMAGE
			{
				// Token: 0x0400BFD4 RID: 49108
				public static LocString NAME = "Structural damage";

				// Token: 0x0400BFD5 RID: 49109
				public static LocString TOOLTIP = "These buildings' structural integrity has been compromised";
			}

			// Token: 0x02002EFD RID: 12029
			public class STRUCTURALCOLLAPSE
			{
				// Token: 0x0400BFD6 RID: 49110
				public static LocString NAME = "Structural collapse";

				// Token: 0x0400BFD7 RID: 49111
				public static LocString TOOLTIP = "These buildings have collapsed:";
			}

			// Token: 0x02002EFE RID: 12030
			public class GASCLOUDWARNING
			{
				// Token: 0x0400BFD8 RID: 49112
				public static LocString NAME = "A gas cloud approaches";

				// Token: 0x0400BFD9 RID: 49113
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A toxic ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" cloud will soon envelop the colony"
				});
			}

			// Token: 0x02002EFF RID: 12031
			public class GASCLOUDARRIVING
			{
				// Token: 0x0400BFDA RID: 49114
				public static LocString NAME = "The colony is entering a cloud of gas";

				// Token: 0x0400BFDB RID: 49115
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002F00 RID: 12032
			public class GASCLOUDPEAK
			{
				// Token: 0x0400BFDC RID: 49116
				public static LocString NAME = "The gas cloud is at its densest point";

				// Token: 0x0400BFDD RID: 49117
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002F01 RID: 12033
			public class GASCLOUDDEPARTING
			{
				// Token: 0x0400BFDE RID: 49118
				public static LocString NAME = "The gas cloud is receding";

				// Token: 0x0400BFDF RID: 49119
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002F02 RID: 12034
			public class GASCLOUDGONE
			{
				// Token: 0x0400BFE0 RID: 49120
				public static LocString NAME = "The colony is once again in open space";

				// Token: 0x0400BFE1 RID: 49121
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002F03 RID: 12035
			public class AVAILABLE
			{
				// Token: 0x0400BFE2 RID: 49122
				public static LocString NAME = "Resource available";

				// Token: 0x0400BFE3 RID: 49123
				public static LocString TOOLTIP = "These resources have become available:";
			}

			// Token: 0x02002F04 RID: 12036
			public class ALLOCATED
			{
				// Token: 0x0400BFE4 RID: 49124
				public static LocString NAME = "Resource allocated";

				// Token: 0x0400BFE5 RID: 49125
				public static LocString TOOLTIP = "These resources are reserved for a planned building:";
			}

			// Token: 0x02002F05 RID: 12037
			public class INCREASEDEXPECTATIONS
			{
				// Token: 0x0400BFE6 RID: 49126
				public static LocString NAME = "Duplicants' expectations increased";

				// Token: 0x0400BFE7 RID: 49127
				public static LocString TOOLTIP = "Duplicants require better amenities over time.\nThese Duplicants have increased their expectations:";
			}

			// Token: 0x02002F06 RID: 12038
			public class NEARLYDRY
			{
				// Token: 0x0400BFE8 RID: 49128
				public static LocString NAME = "Nearly dry";

				// Token: 0x0400BFE9 RID: 49129
				public static LocString TOOLTIP = "These Duplicants will dry off soon:";
			}

			// Token: 0x02002F07 RID: 12039
			public class IMMIGRANTSLEFT
			{
				// Token: 0x0400BFEA RID: 49130
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400BFEB RID: 49131
				public static LocString TOOLTIP = "The care packages have been disintegrated and printable Duplicants have been Oozed";
			}

			// Token: 0x02002F08 RID: 12040
			public class LEVELUP
			{
				// Token: 0x0400BFEC RID: 49132
				public static LocString NAME = "Attribute increase";

				// Token: 0x0400BFED RID: 49133
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" have improved:"
				});

				// Token: 0x0400BFEE RID: 49134
				public static LocString SUFFIX = " - {0} Skill Level modifier raised to +{1}";
			}

			// Token: 0x02002F09 RID: 12041
			public class RESETSKILL
			{
				// Token: 0x0400BFEF RID: 49135
				public static LocString NAME = "Skills reset";

				// Token: 0x0400BFF0 RID: 49136
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have had their ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" refunded:"
				});
			}

			// Token: 0x02002F0A RID: 12042
			public class BADROCKETPATH
			{
				// Token: 0x0400BFF1 RID: 49137
				public static LocString NAME = "Flight Path Obstructed";

				// Token: 0x0400BFF2 RID: 49138
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A rocket's flight path has been interrupted by a new astronomical discovery.\nOpen the ",
					UI.PRE_KEYWORD,
					"Starmap Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageStarmap),
					" to reassign rocket paths"
				});
			}

			// Token: 0x02002F0B RID: 12043
			public class SCHEDULE_CHANGED
			{
				// Token: 0x0400BFF3 RID: 49139
				public static LocString NAME = "{0}: {1}!";

				// Token: 0x0400BFF4 RID: 49140
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants assigned to ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" have started their <b>{1}</b> block.\n\n{2}\n\nOpen the ",
					UI.PRE_KEYWORD,
					"Schedule Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageSchedule),
					" to change blocks or assignments"
				});
			}

			// Token: 0x02002F0C RID: 12044
			public class GENESHUFFLER
			{
				// Token: 0x0400BFF5 RID: 49141
				public static LocString NAME = "Genes Shuffled";

				// Token: 0x0400BFF6 RID: 49142
				public static LocString TOOLTIP = "These Duplicants had their genetic makeup modified:";

				// Token: 0x0400BFF7 RID: 49143
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x02002F0D RID: 12045
			public class HEALINGTRAITGAIN
			{
				// Token: 0x0400BFF8 RID: 49144
				public static LocString NAME = "New trait";

				// Token: 0x0400BFF9 RID: 49145
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' injuries weren't set and healed improperly.\nThey developed ",
					UI.PRE_KEYWORD,
					"Traits",
					UI.PST_KEYWORD,
					" as a result:"
				});

				// Token: 0x0400BFFA RID: 49146
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x02002F0E RID: 12046
			public class COLONYLOST
			{
				// Token: 0x0400BFFB RID: 49147
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400BFFC RID: 49148
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x02002F0F RID: 12047
			public class FABRICATOREMPTY
			{
				// Token: 0x0400BFFD RID: 49149
				public static LocString NAME = "Fabricator idle";

				// Token: 0x0400BFFE RID: 49150
				public static LocString TOOLTIP = "These fabricators have no recipes queued:";
			}

			// Token: 0x02002F10 RID: 12048
			public class SUIT_DROPPED
			{
				// Token: 0x0400BFFF RID: 49151
				public static LocString NAME = "No Docks available";

				// Token: 0x0400C000 RID: 49152
				public static LocString TOOLTIP = "An exosuit was dropped because there were no empty docks available";
			}

			// Token: 0x02002F11 RID: 12049
			public class DEATH_SUFFOCATION
			{
				// Token: 0x0400C001 RID: 49153
				public static LocString NAME = "Duplicants suffocated";

				// Token: 0x0400C002 RID: 49154
				public static LocString TOOLTIP = "These Duplicants died from a lack of " + ELEMENTS.OXYGEN.NAME + ":";
			}

			// Token: 0x02002F12 RID: 12050
			public class DEATH_FROZENSOLID
			{
				// Token: 0x0400C003 RID: 49155
				public static LocString NAME = "Duplicants have frozen";

				// Token: 0x0400C004 RID: 49156
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extremely low ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02002F13 RID: 12051
			public class DEATH_OVERHEATING
			{
				// Token: 0x0400C005 RID: 49157
				public static LocString NAME = "Duplicants have overheated";

				// Token: 0x0400C006 RID: 49158
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extreme ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02002F14 RID: 12052
			public class DEATH_STARVATION
			{
				// Token: 0x0400C007 RID: 49159
				public static LocString NAME = "Duplicants have starved";

				// Token: 0x0400C008 RID: 49160
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from a lack of ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02002F15 RID: 12053
			public class DEATH_FELL
			{
				// Token: 0x0400C009 RID: 49161
				public static LocString NAME = "Duplicants splattered";

				// Token: 0x0400C00A RID: 49162
				public static LocString TOOLTIP = "These Duplicants fell to their deaths:";
			}

			// Token: 0x02002F16 RID: 12054
			public class DEATH_CRUSHED
			{
				// Token: 0x0400C00B RID: 49163
				public static LocString NAME = "Duplicants crushed";

				// Token: 0x0400C00C RID: 49164
				public static LocString TOOLTIP = "These Duplicants have been crushed:";
			}

			// Token: 0x02002F17 RID: 12055
			public class DEATH_SUFFOCATEDTANKEMPTY
			{
				// Token: 0x0400C00D RID: 49165
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400C00E RID: 49166
				public static LocString TOOLTIP = "These Duplicants were unable to reach " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and died:";
			}

			// Token: 0x02002F18 RID: 12056
			public class DEATH_SUFFOCATEDAIRTOOHOT
			{
				// Token: 0x0400C00F RID: 49167
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400C010 RID: 49168
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have asphyxiated in ",
					UI.PRE_KEYWORD,
					"Hot",
					UI.PST_KEYWORD,
					" air:"
				});
			}

			// Token: 0x02002F19 RID: 12057
			public class DEATH_SUFFOCATEDAIRTOOCOLD
			{
				// Token: 0x0400C011 RID: 49169
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400C012 RID: 49170
				public static LocString TOOLTIP = "These Duplicants have asphyxiated in " + UI.FormatAsLink("Cold", "HEAT") + " air:";
			}

			// Token: 0x02002F1A RID: 12058
			public class DEATH_DROWNED
			{
				// Token: 0x0400C013 RID: 49171
				public static LocString NAME = "Duplicants have drowned";

				// Token: 0x0400C014 RID: 49172
				public static LocString TOOLTIP = "These Duplicants have drowned:";
			}

			// Token: 0x02002F1B RID: 12059
			public class DEATH_ENTOUMBED
			{
				// Token: 0x0400C015 RID: 49173
				public static LocString NAME = "Duplicants have been entombed";

				// Token: 0x0400C016 RID: 49174
				public static LocString TOOLTIP = "These Duplicants are trapped and need assistance:";
			}

			// Token: 0x02002F1C RID: 12060
			public class DEATH_RAPIDDECOMPRESSION
			{
				// Token: 0x0400C017 RID: 49175
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400C018 RID: 49176
				public static LocString TOOLTIP = "These Duplicants died in a low pressure environment:";
			}

			// Token: 0x02002F1D RID: 12061
			public class DEATH_OVERPRESSURE
			{
				// Token: 0x0400C019 RID: 49177
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400C01A RID: 49178
				public static LocString TOOLTIP = "These Duplicants died in a high pressure environment:";
			}

			// Token: 0x02002F1E RID: 12062
			public class DEATH_POISONED
			{
				// Token: 0x0400C01B RID: 49179
				public static LocString NAME = "Duplicants poisoned";

				// Token: 0x0400C01C RID: 49180
				public static LocString TOOLTIP = "These Duplicants died as a result of poisoning:";
			}

			// Token: 0x02002F1F RID: 12063
			public class DEATH_DISEASE
			{
				// Token: 0x0400C01D RID: 49181
				public static LocString NAME = "Duplicants have succumbed to disease";

				// Token: 0x0400C01E RID: 49182
				public static LocString TOOLTIP = "These Duplicants died from an untreated " + UI.FormatAsLink("Disease", "DISEASE") + ":";
			}

			// Token: 0x02002F20 RID: 12064
			public class CIRCUIT_OVERLOADED
			{
				// Token: 0x0400C01F RID: 49183
				public static LocString NAME = "Circuit Overloaded";

				// Token: 0x0400C020 RID: 49184
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.WIRE.NAME + "s melted due to excessive current demands on their circuits";
			}

			// Token: 0x02002F21 RID: 12065
			public class LOGIC_CIRCUIT_OVERLOADED
			{
				// Token: 0x0400C021 RID: 49185
				public static LocString NAME = "Logic Circuit Overloaded";

				// Token: 0x0400C022 RID: 49186
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s melted due to more bits of data being sent over them than they can support";
			}

			// Token: 0x02002F22 RID: 12066
			public class DISCOVERED_SPACE
			{
				// Token: 0x0400C023 RID: 49187
				public static LocString NAME = "ALERT - Surface Breach";

				// Token: 0x0400C024 RID: 49188
				public static LocString TOOLTIP = "Amazing!\n\nMy Duplicants have managed to breach the surface of our rocky prison.\n\nI should be careful; the region is extremely inhospitable and I could easily lose resources to the vacuum of space.";
			}

			// Token: 0x02002F23 RID: 12067
			public class COLONY_ACHIEVEMENT_EARNED
			{
				// Token: 0x0400C025 RID: 49189
				public static LocString NAME = "Colony Achievement earned";

				// Token: 0x0400C026 RID: 49190
				public static LocString TOOLTIP = "The colony has earned a new achievement.";
			}

			// Token: 0x02002F24 RID: 12068
			public class WARP_PORTAL_DUPE_READY
			{
				// Token: 0x0400C027 RID: 49191
				public static LocString NAME = "Duplicant warp ready";

				// Token: 0x0400C028 RID: 49192
				public static LocString TOOLTIP = "{dupe} is ready to warp from the " + BUILDINGS.PREFABS.WARPPORTAL.NAME;
			}

			// Token: 0x02002F25 RID: 12069
			public class GENETICANALYSISCOMPLETE
			{
				// Token: 0x0400C029 RID: 49193
				public static LocString NAME = "Seed Analysis Complete";

				// Token: 0x0400C02A RID: 49194
				public static LocString MESSAGEBODY = "Deeply probing the genes of the {Plant} plant have led to the discovery of a promising new cultivatable mutation:\n\n<b>{Subspecies}</b>\n\n{Info}";

				// Token: 0x0400C02B RID: 49195
				public static LocString TOOLTIP = "{Plant} Analysis complete!";
			}

			// Token: 0x02002F26 RID: 12070
			public class NEWMUTANTSEED
			{
				// Token: 0x0400C02C RID: 49196
				public static LocString NAME = "New Mutant Seed Discovered";

				// Token: 0x0400C02D RID: 49197
				public static LocString TOOLTIP = "A new mutant variety of the {Plant} has been found. Analyze it at the " + BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME + " to learn more!";
			}

			// Token: 0x02002F27 RID: 12071
			public class DUPLICANT_CRASH_LANDED
			{
				// Token: 0x0400C02E RID: 49198
				public static LocString NAME = "Duplicant Crash Landed!";

				// Token: 0x0400C02F RID: 49199
				public static LocString TOOLTIP = "A Duplicant has successfully crashed an Escape Pod onto the surface of a nearby Planetoid.";
			}
		}

		// Token: 0x02001EC8 RID: 7880
		public class TUTORIAL
		{
			// Token: 0x04008BF3 RID: 35827
			public static LocString DONT_SHOW_AGAIN = "Don't Show Again";
		}

		// Token: 0x02001EC9 RID: 7881
		public class PLACERS
		{
			// Token: 0x02002F28 RID: 12072
			public class DIGPLACER
			{
				// Token: 0x0400C030 RID: 49200
				public static LocString NAME = "Dig";
			}

			// Token: 0x02002F29 RID: 12073
			public class MOPPLACER
			{
				// Token: 0x0400C031 RID: 49201
				public static LocString NAME = "Mop";
			}

			// Token: 0x02002F2A RID: 12074
			public class MOVEPICKUPABLEPLACER
			{
				// Token: 0x0400C032 RID: 49202
				public static LocString NAME = "Move Here";

				// Token: 0x0400C033 RID: 49203
				public static LocString PLACER_STATUS = "Next Destination";

				// Token: 0x0400C034 RID: 49204
				public static LocString PLACER_STATUS_TOOLTIP = "Click to see where this item will be moved to";
			}
		}

		// Token: 0x02001ECA RID: 7882
		public class MONUMENT_COMPLETE
		{
			// Token: 0x04008BF4 RID: 35828
			public static LocString NAME = "Great Monument";

			// Token: 0x04008BF5 RID: 35829
			public static LocString DESC = "A feat of artistic vision and expert engineering that will doubtless inspire Duplicants for thousands of cycles to come";
		}
	}
}
