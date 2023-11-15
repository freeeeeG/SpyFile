using System;

namespace STRINGS
{
	// Token: 0x02000DAA RID: 3498
	public class GAMEPLAY_EVENTS
	{
		// Token: 0x04005118 RID: 20760
		public static LocString CANCELED = "{0} Canceled";

		// Token: 0x04005119 RID: 20761
		public static LocString CANCELED_TOOLTIP = "The {0} event was canceled";

		// Token: 0x0400511A RID: 20762
		public static LocString DEFAULT_OPTION_NAME = "Okay";

		// Token: 0x0400511B RID: 20763
		public static LocString DEFAULT_OPTION_CONSIDER_NAME = "Let me think about it";

		// Token: 0x0400511C RID: 20764
		public static LocString CHAIN_EVENT_TOOLTIP = "This event is a chain event.";

		// Token: 0x0400511D RID: 20765
		public static LocString BONUS_EVENT_DESCRIPTION = "{effects} for {duration}";

		// Token: 0x02001DD6 RID: 7638
		public class LOCATIONS
		{
			// Token: 0x04008913 RID: 35091
			public static LocString NONE_AVAILABLE = "No location currently available";

			// Token: 0x04008914 RID: 35092
			public static LocString SUN = "The Sun";

			// Token: 0x04008915 RID: 35093
			public static LocString SURFACE = "Planetary Surface";

			// Token: 0x04008916 RID: 35094
			public static LocString PRINTING_POD = BUILDINGS.PREFABS.HEADQUARTERS.NAME;

			// Token: 0x04008917 RID: 35095
			public static LocString COLONY_WIDE = "Colonywide";
		}

		// Token: 0x02001DD7 RID: 7639
		public class TIMES
		{
			// Token: 0x04008918 RID: 35096
			public static LocString NOW = "Right now";

			// Token: 0x04008919 RID: 35097
			public static LocString IN_CYCLES = "In {0} cycles";

			// Token: 0x0400891A RID: 35098
			public static LocString UNKNOWN = "Sometime";
		}

		// Token: 0x02001DD8 RID: 7640
		public class EVENT_TYPES
		{
			// Token: 0x0200275F RID: 10079
			public class PARTY
			{
				// Token: 0x0400AD71 RID: 44401
				public static LocString NAME = "Party";

				// Token: 0x0400AD72 RID: 44402
				public static LocString DESCRIPTION = "THIS EVENT IS NOT WORKING\n{host} is throwing a birthday party for {dupe}. Make sure there is an available " + ROOMS.TYPES.REC_ROOM.NAME + " for the party.\n\nSocial events are good for Duplicant morale. Rejecting this party will hurt {host} and {dupe}'s fragile ego.";

				// Token: 0x0400AD73 RID: 44403
				public static LocString CANCELED_NO_ROOM_TITLE = "Party Canceled";

				// Token: 0x0400AD74 RID: 44404
				public static LocString CANCELED_NO_ROOM_DESCRIPTION = "The party was canceled because no " + ROOMS.TYPES.REC_ROOM.NAME + " was available.";

				// Token: 0x0400AD75 RID: 44405
				public static LocString UNDERWAY = "Party Happening";

				// Token: 0x0400AD76 RID: 44406
				public static LocString UNDERWAY_TOOLTIP = "There's a party going on";

				// Token: 0x0400AD77 RID: 44407
				public static LocString ACCEPT_OPTION_NAME = "Allow the party to happen";

				// Token: 0x0400AD78 RID: 44408
				public static LocString ACCEPT_OPTION_DESC = "Party goers will get {goodEffect}";

				// Token: 0x0400AD79 RID: 44409
				public static LocString ACCEPT_OPTION_INVALID_TOOLTIP = "A cake must be built for this event to take place.";

				// Token: 0x0400AD7A RID: 44410
				public static LocString REJECT_OPTION_NAME = "Cancel the party";

				// Token: 0x0400AD7B RID: 44411
				public static LocString REJECT_OPTION_DESC = "{host} and {dupe} gain {badEffect}";
			}

			// Token: 0x02002760 RID: 10080
			public class ECLIPSE
			{
				// Token: 0x0400AD7C RID: 44412
				public static LocString NAME = "Eclipse";

				// Token: 0x0400AD7D RID: 44413
				public static LocString DESCRIPTION = "A celestial object has obscured the sunlight";
			}

			// Token: 0x02002761 RID: 10081
			public class SOLAR_FLARE
			{
				// Token: 0x0400AD7E RID: 44414
				public static LocString NAME = "Solar Storm";

				// Token: 0x0400AD7F RID: 44415
				public static LocString DESCRIPTION = "A solar flare is headed this way";
			}

			// Token: 0x02002762 RID: 10082
			public class CREATURE_SPAWN
			{
				// Token: 0x0400AD80 RID: 44416
				public static LocString NAME = "Critter Infestation";

				// Token: 0x0400AD81 RID: 44417
				public static LocString DESCRIPTION = "There was a massive influx of destructive critters";
			}

			// Token: 0x02002763 RID: 10083
			public class SATELLITE_CRASH
			{
				// Token: 0x0400AD82 RID: 44418
				public static LocString NAME = "Satellite Crash";

				// Token: 0x0400AD83 RID: 44419
				public static LocString DESCRIPTION = "Mysterious space junk has crashed into the surface.\n\nIt may contain useful resources or information, but it may also be dangerous. Approach with caution.";
			}

			// Token: 0x02002764 RID: 10084
			public class FOOD_FIGHT
			{
				// Token: 0x0400AD84 RID: 44420
				public static LocString NAME = "Food Fight";

				// Token: 0x0400AD85 RID: 44421
				public static LocString DESCRIPTION = "Duplicants will throw food at each other for recreation\n\nIt may be wasteful, but everyone who participates will benefit from a major stress reduction.";

				// Token: 0x0400AD86 RID: 44422
				public static LocString UNDERWAY = "Food Fight";

				// Token: 0x0400AD87 RID: 44423
				public static LocString UNDERWAY_TOOLTIP = "There is a food fight happening now";

				// Token: 0x0400AD88 RID: 44424
				public static LocString ACCEPT_OPTION_NAME = "Dupes start preparing to fight.";

				// Token: 0x0400AD89 RID: 44425
				public static LocString ACCEPT_OPTION_DETAILS = "(Plus morale)";

				// Token: 0x0400AD8A RID: 44426
				public static LocString REJECT_OPTION_NAME = "No food fight today";

				// Token: 0x0400AD8B RID: 44427
				public static LocString REJECT_OPTION_DETAILS = "Sadface";
			}

			// Token: 0x02002765 RID: 10085
			public class PLANT_BLIGHT
			{
				// Token: 0x0400AD8C RID: 44428
				public static LocString NAME = "Plant Blight: {plant}";

				// Token: 0x0400AD8D RID: 44429
				public static LocString DESCRIPTION = "Our {plant} crops have been afflicted by a fungal sickness!\n\nI must get the Duplicants to uproot and compost the sick plants to save our farms.";

				// Token: 0x0400AD8E RID: 44430
				public static LocString SUCCESS = "Blight Managed: {plant}";

				// Token: 0x0400AD8F RID: 44431
				public static LocString SUCCESS_TOOLTIP = "All the blighted {plant} plants have been dealt with, halting the infection.";
			}

			// Token: 0x02002766 RID: 10086
			public class CRYOFRIEND
			{
				// Token: 0x0400AD90 RID: 44432
				public static LocString NAME = "New Event: A Frozen Friend";

				// Token: 0x0400AD91 RID: 44433
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} has made an amazing discovery! A barely working ",
					BUILDINGS.PREFABS.CRYOTANK.NAME,
					" has been uncovered containing a {friend} inside in a frozen state.\n\n{dupe} was successful in thawing {friend} and this encounter has filled both Duplicants with a sense of hope, something they will desperately need to keep their ",
					UI.FormatAsLink("Morale", "MORALE"),
					" up when facing the dangers ahead."
				});

				// Token: 0x0400AD92 RID: 44434
				public static LocString BUTTON = "{friend} is thawed!";
			}

			// Token: 0x02002767 RID: 10087
			public class WARPWORLDREVEAL
			{
				// Token: 0x0400AD93 RID: 44435
				public static LocString NAME = "New Event: Personnel Teleporter";

				// Token: 0x0400AD94 RID: 44436
				public static LocString DESCRIPTION = "I've discovered a functioning teleportation device with a pre-programmed destination.\n\nIt appears to go to another " + UI.CLUSTERMAP.PLANETOID + ", and I'm fairly certain there's a return device on the other end.\n\nI could send a Duplicant through safely if I desired.";

				// Token: 0x0400AD95 RID: 44437
				public static LocString BUTTON = "See Destination";
			}

			// Token: 0x02002768 RID: 10088
			public class ARTIFACT_REVEAL
			{
				// Token: 0x0400AD96 RID: 44438
				public static LocString NAME = "New Event: Artifact Analyzed";

				// Token: 0x0400AD97 RID: 44439
				public static LocString DESCRIPTION = "An artifact from a past civilization was analyzed.\n\n{desc}";

				// Token: 0x0400AD98 RID: 44440
				public static LocString BUTTON = "Close";
			}
		}

		// Token: 0x02001DD9 RID: 7641
		public class BONUS
		{
			// Token: 0x02002769 RID: 10089
			public class BONUSDREAM1
			{
				// Token: 0x0400AD99 RID: 44441
				public static LocString NAME = "Good Dream";

				// Token: 0x0400AD9A RID: 44442
				public static LocString DESCRIPTION = "I've observed many improvements to {dupe}'s demeanor today. Analysis indicates unusually high amounts of dopamine in their system. There's a good chance this is due to an exceptionally good dream and analysis indicates that current sleeping conditions may have contributed to this occurrence.\n\nFurther improvements to sleeping conditions may have additional positive effects to the " + UI.FormatAsLink("Morale", "MORALE") + " of {dupe} and other Duplicants.";

				// Token: 0x0400AD9B RID: 44443
				public static LocString CHAIN_TOOLTIP = "Improving the living conditions of {dupe} will lead to more good dreams.";
			}

			// Token: 0x0200276A RID: 10090
			public class BONUSDREAM2
			{
				// Token: 0x0400AD9C RID: 44444
				public static LocString NAME = "Really Good Dream";

				// Token: 0x0400AD9D RID: 44445
				public static LocString DESCRIPTION = "{dupe} had another really good dream and the resulting release of dopamine has made this Duplicant energetic and full of possibilities! This is an encouraging byproduct of improving the living conditions of the colony.\n\nBased on these observations, building a better sleeping area for my Duplicants will have a similar effect on their " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x0200276B RID: 10091
			public class BONUSDREAM3
			{
				// Token: 0x0400AD9E RID: 44446
				public static LocString NAME = "Great Dream";

				// Token: 0x0400AD9F RID: 44447
				public static LocString DESCRIPTION = "I have detected a distinct spring in {dupe}'s step today. There is a good chance that this Duplicant had another great dream last night. Such incidents are further indications that working on the care and comfort of the colony is not a waste of time.\n\nI do wonder though: What do Duplicants dream of?";
			}

			// Token: 0x0200276C RID: 10092
			public class BONUSDREAM4
			{
				// Token: 0x0400ADA0 RID: 44448
				public static LocString NAME = "Amazing Dream";

				// Token: 0x0400ADA1 RID: 44449
				public static LocString DESCRIPTION = "{dupe}'s dream last night must have been simply amazing! Their dopamine levels are at an all time high. Based on these results, it can be safely assumed that improving the living conditions of my Duplicants will reduce " + UI.FormatAsLink("Stress", "STRESS") + " and have similar positive effects on their well-being.\n\nObservations such as this are an integral and enjoyable part of science. When I see my Duplicants happy, I can't help but share in some of their joy.";
			}

			// Token: 0x0200276D RID: 10093
			public class BONUSTOILET1
			{
				// Token: 0x0400ADA2 RID: 44450
				public static LocString NAME = "Small Comforts";

				// Token: 0x0400ADA3 RID: 44451
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} recently visited an Outhouse and appears to have appreciated the small comforts based on the marked increase to their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nHigh ",
					UI.FormatAsLink("Morale", "MORALE"),
					" has been linked to a better work ethic and greater enthusiasm for complex jobs, which are essential in building a successful new colony."
				});
			}

			// Token: 0x0200276E RID: 10094
			public class BONUSTOILET2
			{
				// Token: 0x0400ADA4 RID: 44452
				public static LocString NAME = "Greater Comforts";

				// Token: 0x0400ADA5 RID: 44453
				public static LocString DESCRIPTION = "{dupe} used a Lavatory and analysis shows a decided improvement to this Duplicant's " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nAs my colony grows and expands, it's important not to ignore the benefits of giving my Duplicants a pleasant place to relieve themselves.";
			}

			// Token: 0x0200276F RID: 10095
			public class BONUSTOILET3
			{
				// Token: 0x0400ADA6 RID: 44454
				public static LocString NAME = "Small Luxury";

				// Token: 0x0400ADA7 RID: 44455
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} visited a ",
					ROOMS.TYPES.LATRINE.NAME,
					" and experienced luxury unlike they anything this Duplicant had previously experienced as analysis has revealed yet another boost to their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nIt is unclear whether this development is a result of increased hygiene or whether there is something else inherently about working plumbing which would improve ",
					UI.FormatAsLink("Morale", "MORALE"),
					" in this way. Further analysis is needed."
				});
			}

			// Token: 0x02002770 RID: 10096
			public class BONUSTOILET4
			{
				// Token: 0x0400ADA8 RID: 44456
				public static LocString NAME = "Greater Luxury";

				// Token: 0x0400ADA9 RID: 44457
				public static LocString DESCRIPTION = "{dupe} visited a Washroom and the experience has left this Duplicant with significantly improved " + UI.FormatAsLink("Morale", "MORALE") + ". Analysis indicates this improvement should continue for many cycles.\n\nThe relationship of my Duplicants and their surroundings is an interesting aspect of colony life. I should continue to watch future developments in this department closely.";
			}

			// Token: 0x02002771 RID: 10097
			public class BONUSRESEARCH
			{
				// Token: 0x0400ADAA RID: 44458
				public static LocString NAME = "Inspired Learner";

				// Token: 0x0400ADAB RID: 44459
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Analysis indicates that the appearance of a ",
					UI.PRE_KEYWORD,
					"Research Station",
					UI.PST_KEYWORD,
					" has inspired {dupe} and heightened their brain activity on a cellular level.\n\nBrain stimulation is important if my Duplicants are going to adapt and innovate in their increasingly harsh environment."
				});
			}

			// Token: 0x02002772 RID: 10098
			public class BONUSDIGGING1
			{
				// Token: 0x0400ADAC RID: 44460
				public static LocString NAME = "Hot Diggity!";

				// Token: 0x0400ADAD RID: 44461
				public static LocString DESCRIPTION = "Some interesting data has revealed that {dupe} has had a marked increase in physical abilities, an increase that cannot entirely be attributed to the usual improvements that occur after regular physical activity.\n\nBased on previous observations this Duplicant's positive associations with digging appear to account for this additional physical boost.\n\nThis would mean the personal preferences of my Duplicants are directly correlated to how hard they work. How interesting...";
			}

			// Token: 0x02002773 RID: 10099
			public class BONUSSTORAGE
			{
				// Token: 0x0400ADAE RID: 44462
				public static LocString NAME = "Something in Store";

				// Token: 0x0400ADAF RID: 44463
				public static LocString DESCRIPTION = "Data indicates that {dupe}'s activity in storing something in a Storage Bin has led to an increase in this Duplicant's physical strength as well as an overall improvement to their general demeanor.\n\nThere have been many studies connecting organization with an increase in well-being. It is possible this explains {dupe}'s " + UI.FormatAsLink("Morale", "MORALE") + " improvements.";
			}

			// Token: 0x02002774 RID: 10100
			public class BONUSBUILDER
			{
				// Token: 0x0400ADB0 RID: 44464
				public static LocString NAME = "Accomplished Builder";

				// Token: 0x0400ADB1 RID: 44465
				public static LocString DESCRIPTION = "{dupe} has been hard at work building many structures crucial to the future of the colony. It seems this activity has improved this Duplicant's budding construction and mechanical skills beyond what my models predicted.\n\nWhether this increase in ability is due to them learning new skills or simply gaining self-confidence I cannot say, but this unexpected development is a welcome surprise development.";
			}

			// Token: 0x02002775 RID: 10101
			public class BONUSOXYGEN
			{
				// Token: 0x0400ADB2 RID: 44466
				public static LocString NAME = "Fresh Air";

				// Token: 0x0400ADB3 RID: 44467
				public static LocString DESCRIPTION = "{dupe} is experiencing a sudden unexpected improvement to their physical prowess which appears to be a result of exposure to elevated levels of oxygen from passing by an Oxygen Diffuser.\n\nObservations such as this are important in documenting just how beneficial having access to oxygen is to my colony.";
			}

			// Token: 0x02002776 RID: 10102
			public class BONUSALGAE
			{
				// Token: 0x0400ADB4 RID: 44468
				public static LocString NAME = "Fresh Algae Smell";

				// Token: 0x0400ADB5 RID: 44469
				public static LocString DESCRIPTION = "{dupe}'s recent proximity to an Algae Terrarium has left them feeling refreshed and exuberant and is correlated to an increase in their physical attributes. It is unclear whether these physical improvements came from the excess of oxygen or the invigorating smell of algae.\n\nIt's curious that I find myself nostalgic for the smell of algae growing in a lab. But how could this be...?";
			}

			// Token: 0x02002777 RID: 10103
			public class BONUSGENERATOR
			{
				// Token: 0x0400ADB6 RID: 44470
				public static LocString NAME = "Exercised";

				// Token: 0x0400ADB7 RID: 44471
				public static LocString DESCRIPTION = "{dupe} ran in a Manual Generator and the physical activity appears to have given this Duplicant increased strength and sense of well-being.\n\nWhile not the primary reason for building Manual Generators, I am very pleased to see my Duplicants reaping the " + UI.FormatAsLink("Stress", "STRESS") + " relieving benefits to physical activity.";
			}

			// Token: 0x02002778 RID: 10104
			public class BONUSDOOR
			{
				// Token: 0x0400ADB8 RID: 44472
				public static LocString NAME = "Open and Shut";

				// Token: 0x0400ADB9 RID: 44473
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"The act of closing a door has apparently lead to a decrease in the ",
					UI.FormatAsLink("Stress", "STRESS"),
					" levels of {dupe}, as well as decreased the exposure of this Duplicant to harmful ",
					UI.FormatAsLink("Germs", "GERMS"),
					".\n\nWhile it may be more efficient to group all my Duplicants together in common sleeping quarters, it's important to remember the mental benefits to privacy and space to express their individuality."
				});
			}

			// Token: 0x02002779 RID: 10105
			public class BONUSHITTHEBOOKS
			{
				// Token: 0x0400ADBA RID: 44474
				public static LocString NAME = "Hit the Books";

				// Token: 0x0400ADBB RID: 44475
				public static LocString DESCRIPTION = "{dupe}'s recent Research errand has resulted in a significant increase to this Duplicant's brain activity. The discovery of newly found knowledge has given {dupe} an invigorating jolt of excitement.\n\nI am all too familiar with this feeling.";
			}

			// Token: 0x0200277A RID: 10106
			public class BONUSLITWORKSPACE
			{
				// Token: 0x0400ADBC RID: 44476
				public static LocString NAME = "Lit-erally Great";

				// Token: 0x0400ADBD RID: 44477
				public static LocString DESCRIPTION = "{dupe}'s recent time in a well-lit area has greatly improved this Duplicant's ability to work with, and on, machinery.\n\nThis supports the prevailing theory that a well-lit workspace has many benefits beyond just improving my Duplicant's ability to see.";
			}

			// Token: 0x0200277B RID: 10107
			public class BONUSTALKER
			{
				// Token: 0x0400ADBE RID: 44478
				public static LocString NAME = "Big Small Talker";

				// Token: 0x0400ADBF RID: 44479
				public static LocString DESCRIPTION = "{dupe}'s recent conversation with another Duplicant shows a correlation to improved serotonin and " + UI.FormatAsLink("Morale", "MORALE") + " levels in this Duplicant. It is very possible that small talk with a co-worker, however short and seemingly insignificant, will make my Duplicant's feel connected to the colony as a whole.\n\nAs the colony gets bigger and more sophisticated, I must ensure that the opportunity for such connections continue, for the good of my Duplicants' mental well being.";
			}
		}
	}
}
