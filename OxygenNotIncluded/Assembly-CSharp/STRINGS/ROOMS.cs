using System;

namespace STRINGS
{
	// Token: 0x02000DAB RID: 3499
	public class ROOMS
	{
		// Token: 0x02001DDA RID: 7642
		public class CATEGORY
		{
			// Token: 0x0200277C RID: 10108
			public class NONE
			{
				// Token: 0x0400ADC0 RID: 44480
				public static LocString NAME = "None";
			}

			// Token: 0x0200277D RID: 10109
			public class FOOD
			{
				// Token: 0x0400ADC1 RID: 44481
				public static LocString NAME = "Dining";
			}

			// Token: 0x0200277E RID: 10110
			public class SLEEP
			{
				// Token: 0x0400ADC2 RID: 44482
				public static LocString NAME = "Sleep";
			}

			// Token: 0x0200277F RID: 10111
			public class RECREATION
			{
				// Token: 0x0400ADC3 RID: 44483
				public static LocString NAME = "Recreation";
			}

			// Token: 0x02002780 RID: 10112
			public class BATHROOM
			{
				// Token: 0x0400ADC4 RID: 44484
				public static LocString NAME = "Washroom";
			}

			// Token: 0x02002781 RID: 10113
			public class HOSPITAL
			{
				// Token: 0x0400ADC5 RID: 44485
				public static LocString NAME = "Medical";
			}

			// Token: 0x02002782 RID: 10114
			public class INDUSTRIAL
			{
				// Token: 0x0400ADC6 RID: 44486
				public static LocString NAME = "Industrial";
			}

			// Token: 0x02002783 RID: 10115
			public class AGRICULTURAL
			{
				// Token: 0x0400ADC7 RID: 44487
				public static LocString NAME = "Agriculture";
			}

			// Token: 0x02002784 RID: 10116
			public class PARK
			{
				// Token: 0x0400ADC8 RID: 44488
				public static LocString NAME = "Parks";
			}

			// Token: 0x02002785 RID: 10117
			public class SCIENCE
			{
				// Token: 0x0400ADC9 RID: 44489
				public static LocString NAME = "Science";
			}
		}

		// Token: 0x02001DDB RID: 7643
		public class TYPES
		{
			// Token: 0x0400891B RID: 35099
			public static LocString CONFLICTED = "Conflicted Room";

			// Token: 0x02002786 RID: 10118
			public class NEUTRAL
			{
				// Token: 0x0400ADCA RID: 44490
				public static LocString NAME = "Miscellaneous Room";

				// Token: 0x0400ADCB RID: 44491
				public static LocString DESCRIPTION = "An enclosed space with plenty of potential and no dedicated use.";

				// Token: 0x0400ADCC RID: 44492
				public static LocString EFFECT = "- No effect";

				// Token: 0x0400ADCD RID: 44493
				public static LocString TOOLTIP = "This area has walls and doors but no dedicated use";
			}

			// Token: 0x02002787 RID: 10119
			public class LATRINE
			{
				// Token: 0x0400ADCE RID: 44494
				public static LocString NAME = "Latrine";

				// Token: 0x0400ADCF RID: 44495
				public static LocString DESCRIPTION = "It's a step up from doing one's business in full view of the rest of the colony.\n\nUsing a toilet in an enclosed room will improve Duplicants' Morale.";

				// Token: 0x0400ADD0 RID: 44496
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400ADD1 RID: 44497
				public static LocString TOOLTIP = "Using a toilet in an enclosed room will improve Duplicants' Morale";
			}

			// Token: 0x02002788 RID: 10120
			public class PLUMBEDBATHROOM
			{
				// Token: 0x0400ADD2 RID: 44498
				public static LocString NAME = "Washroom";

				// Token: 0x0400ADD3 RID: 44499
				public static LocString DESCRIPTION = "A sanctuary of personal hygiene.\n\nUsing a fully plumbed Washroom will improve Duplicants' Morale.";

				// Token: 0x0400ADD4 RID: 44500
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400ADD5 RID: 44501
				public static LocString TOOLTIP = "Using a fully plumbed Washroom will improve Duplicants' Morale";
			}

			// Token: 0x02002789 RID: 10121
			public class BARRACKS
			{
				// Token: 0x0400ADD6 RID: 44502
				public static LocString NAME = "Barracks";

				// Token: 0x0400ADD7 RID: 44503
				public static LocString DESCRIPTION = "A basic communal sleeping area for up-and-coming colonies.\n\nSleeping in Barracks will improve Duplicants' Morale.";

				// Token: 0x0400ADD8 RID: 44504
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400ADD9 RID: 44505
				public static LocString TOOLTIP = "Sleeping in Barracks will improve Duplicants' Morale";
			}

			// Token: 0x0200278A RID: 10122
			public class BEDROOM
			{
				// Token: 0x0400ADDA RID: 44506
				public static LocString NAME = "Luxury Barracks";

				// Token: 0x0400ADDB RID: 44507
				public static LocString DESCRIPTION = "An upscale communal sleeping area full of things that greatly enhance quality of rest for occupants.\n\nSleeping in a Luxury Barracks will improve Duplicants' Morale.";

				// Token: 0x0400ADDC RID: 44508
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400ADDD RID: 44509
				public static LocString TOOLTIP = "Sleeping in a Luxury Barracks will improve Duplicants' Morale";
			}

			// Token: 0x0200278B RID: 10123
			public class PRIVATE_BEDROOM
			{
				// Token: 0x0400ADDE RID: 44510
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400ADDF RID: 44511
				public static LocString DESCRIPTION = "A comfortable, roommate-free retreat where tired Duplicants can get uninterrupted rest.\n\nSleeping in a Private Bedroom will greatly improve Duplicants' Morale.";

				// Token: 0x0400ADE0 RID: 44512
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400ADE1 RID: 44513
				public static LocString TOOLTIP = "Sleeping in a Private Bedroom will greatly improve Duplicants' Morale";
			}

			// Token: 0x0200278C RID: 10124
			public class MESSHALL
			{
				// Token: 0x0400ADE2 RID: 44514
				public static LocString NAME = "Mess Hall";

				// Token: 0x0400ADE3 RID: 44515
				public static LocString DESCRIPTION = "A simple dining room setup that's easy to improve upon.\n\nEating at a mess table in a Mess Hall will increase Duplicants' Morale.";

				// Token: 0x0400ADE4 RID: 44516
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400ADE5 RID: 44517
				public static LocString TOOLTIP = "Eating at a Mess Table in a Mess Hall will improve Duplicants' Morale";
			}

			// Token: 0x0200278D RID: 10125
			public class KITCHEN
			{
				// Token: 0x0400ADE6 RID: 44518
				public static LocString NAME = "Kitchen";

				// Token: 0x0400ADE7 RID: 44519
				public static LocString DESCRIPTION = "A cooking area equipped to take meals to the next level.\n\nAdding ingredients from a Spice Grinder to foods cooked on an Electric Grill or Gas Range provides a variety of positive benefits.";

				// Token: 0x0400ADE8 RID: 44520
				public static LocString EFFECT = "- Enables Spice Grinder use";

				// Token: 0x0400ADE9 RID: 44521
				public static LocString TOOLTIP = "Using a Spice Grinder in a Kitchen adds benefits to foods cooked on Electric Grill or Gas Range";
			}

			// Token: 0x0200278E RID: 10126
			public class GREATHALL
			{
				// Token: 0x0400ADEA RID: 44522
				public static LocString NAME = "Great Hall";

				// Token: 0x0400ADEB RID: 44523
				public static LocString DESCRIPTION = "A great place to eat, with great decor and great company. Great!\n\nEating in a Great Hall will significantly improve Duplicants' Morale.";

				// Token: 0x0400ADEC RID: 44524
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400ADED RID: 44525
				public static LocString TOOLTIP = "Eating in a Great Hall will significantly improve Duplicants' Morale";
			}

			// Token: 0x0200278F RID: 10127
			public class HOSPITAL
			{
				// Token: 0x0400ADEE RID: 44526
				public static LocString NAME = "Hospital";

				// Token: 0x0400ADEF RID: 44527
				public static LocString DESCRIPTION = "A dedicated medical facility that helps minimize recovery time.\n\nSick Duplicants assigned to medical buildings located within a Hospital are also less likely to spread Disease.";

				// Token: 0x0400ADF0 RID: 44528
				public static LocString EFFECT = "- Quarantine sick Duplicants";

				// Token: 0x0400ADF1 RID: 44529
				public static LocString TOOLTIP = "Sick Duplicants assigned to medical buildings located within a Hospital are less likely to spread Disease";
			}

			// Token: 0x02002790 RID: 10128
			public class MASSAGE_CLINIC
			{
				// Token: 0x0400ADF2 RID: 44530
				public static LocString NAME = "Massage Clinic";

				// Token: 0x0400ADF3 RID: 44531
				public static LocString DESCRIPTION = "A soothing space with a very relaxing ambience, especially when well-decorated.\n\nReceiving massages at a Massage Clinic will significantly improve Stress reduction.";

				// Token: 0x0400ADF4 RID: 44532
				public static LocString EFFECT = "- Massage stress relief bonus";

				// Token: 0x0400ADF5 RID: 44533
				public static LocString TOOLTIP = "Receiving massages at a Massage Clinic will significantly improve Stress reduction";
			}

			// Token: 0x02002791 RID: 10129
			public class POWER_PLANT
			{
				// Token: 0x0400ADF6 RID: 44534
				public static LocString NAME = "Power Plant";

				// Token: 0x0400ADF7 RID: 44535
				public static LocString DESCRIPTION = "The perfect place for Duplicants to flex their Electrical Engineering skills.\n\nGenerators built within a Power Plant can be tuned up using power control stations to improve their power production.";

				// Token: 0x0400ADF8 RID: 44536
				public static LocString EFFECT = "- Enables Power Control Station use";

				// Token: 0x0400ADF9 RID: 44537
				public static LocString TOOLTIP = "Generators built within a Power Plant can be tuned up using Power Control Stations to improve their power production";
			}

			// Token: 0x02002792 RID: 10130
			public class MACHINE_SHOP
			{
				// Token: 0x0400ADFA RID: 44538
				public static LocString NAME = "Machine Shop";

				// Token: 0x0400ADFB RID: 44539
				public static LocString DESCRIPTION = "It smells like elbow grease.\n\nDuplicants working in a Machine Shop can maintain buildings and increase their production speed.";

				// Token: 0x0400ADFC RID: 44540
				public static LocString EFFECT = "- Increased fabrication efficiency";

				// Token: 0x0400ADFD RID: 44541
				public static LocString TOOLTIP = "Duplicants working in a Machine Shop can maintain buildings and increase their production speed";
			}

			// Token: 0x02002793 RID: 10131
			public class FARM
			{
				// Token: 0x0400ADFE RID: 44542
				public static LocString NAME = "Greenhouse";

				// Token: 0x0400ADFF RID: 44543
				public static LocString DESCRIPTION = "An enclosed agricultural space best utilized by Duplicants with Crop Tending skills.\n\nCrops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed.";

				// Token: 0x0400AE00 RID: 44544
				public static LocString EFFECT = "- Enables Farm Station use";

				// Token: 0x0400AE01 RID: 44545
				public static LocString TOOLTIP = "Crops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed";
			}

			// Token: 0x02002794 RID: 10132
			public class CREATUREPEN
			{
				// Token: 0x0400AE02 RID: 44546
				public static LocString NAME = "Stable";

				// Token: 0x0400AE03 RID: 44547
				public static LocString DESCRIPTION = "Critters don't mind it here, as long as things don't get too overcrowded.\n\nStabled critters can be tended at a Grooming Station to hasten their domestication and increase their production.";

				// Token: 0x0400AE04 RID: 44548
				public static LocString EFFECT = "- Enables Grooming Station use";

				// Token: 0x0400AE05 RID: 44549
				public static LocString TOOLTIP = "Stabled critters can be tended at a Grooming Station to hasten their domestication and increase their production";
			}

			// Token: 0x02002795 RID: 10133
			public class REC_ROOM
			{
				// Token: 0x0400AE06 RID: 44550
				public static LocString NAME = "Recreation Room";

				// Token: 0x0400AE07 RID: 44551
				public static LocString DESCRIPTION = "Where Duplicants go to mingle with off-duty peers and indulge in a little R&R.\n\nScheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room.";

				// Token: 0x0400AE08 RID: 44552
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400AE09 RID: 44553
				public static LocString TOOLTIP = "Scheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room";
			}

			// Token: 0x02002796 RID: 10134
			public class PARK
			{
				// Token: 0x0400AE0A RID: 44554
				public static LocString NAME = "Park";

				// Token: 0x0400AE0B RID: 44555
				public static LocString DESCRIPTION = "A little greenery goes a long way.\n\nPassing through natural spaces throughout the day will raise the Morale of Duplicants.";

				// Token: 0x0400AE0C RID: 44556
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400AE0D RID: 44557
				public static LocString TOOLTIP = "Passing through natural spaces throughout the day will raise the Morale of Duplicants";
			}

			// Token: 0x02002797 RID: 10135
			public class NATURERESERVE
			{
				// Token: 0x0400AE0E RID: 44558
				public static LocString NAME = "Nature Reserve";

				// Token: 0x0400AE0F RID: 44559
				public static LocString DESCRIPTION = "A lot of greenery goes an even longer way.\n\nPassing through a Nature Reserve will grant higher Morale bonuses to Duplicants than a Park.";

				// Token: 0x0400AE10 RID: 44560
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400AE11 RID: 44561
				public static LocString TOOLTIP = "A Nature Reserve will grant higher Morale bonuses to Duplicants than a Park";
			}

			// Token: 0x02002798 RID: 10136
			public class LABORATORY
			{
				// Token: 0x0400AE12 RID: 44562
				public static LocString NAME = "Laboratory";

				// Token: 0x0400AE13 RID: 44563
				public static LocString DESCRIPTION = "Where wild hypotheses meet rigorous scientific experimentation.\n\nScience stations built in a Laboratory function more efficiently.\n\nA Laboratory enables the use of the Geotuner and the Mission Control Station.";

				// Token: 0x0400AE14 RID: 44564
				public static LocString EFFECT = "- Efficiency bonus";

				// Token: 0x0400AE15 RID: 44565
				public static LocString TOOLTIP = "Science buildings built in a Laboratory function more efficiently\n\nA Laboratory enables Geotuner and Mission Control Station use";
			}

			// Token: 0x02002799 RID: 10137
			public class PRIVATE_BATHROOM
			{
				// Token: 0x0400AE16 RID: 44566
				public static LocString NAME = "Private Bathroom";

				// Token: 0x0400AE17 RID: 44567
				public static LocString DESCRIPTION = "Finally, a place to truly be alone with one's thoughts.\n\nDuplicants relieve even more Stress when using the toilet in a Private Bathroom than in a Latrine.";

				// Token: 0x0400AE18 RID: 44568
				public static LocString EFFECT = "- Stress relief bonus";

				// Token: 0x0400AE19 RID: 44569
				public static LocString TOOLTIP = "Duplicants relieve even more stress when using the toilet in a Private Bathroom than in a Latrine";
			}
		}

		// Token: 0x02001DDC RID: 7644
		public class CRITERIA
		{
			// Token: 0x0400891C RID: 35100
			public static LocString HEADER = "<b>Requirements:</b>";

			// Token: 0x0400891D RID: 35101
			public static LocString NEUTRAL_TYPE = "Enclosed by wall tile";

			// Token: 0x0400891E RID: 35102
			public static LocString POSSIBLE_TYPES_HEADER = "Possible Room Types";

			// Token: 0x0400891F RID: 35103
			public static LocString NO_TYPE_CONFLICTS = "Remove conflicting buildings";

			// Token: 0x04008920 RID: 35104
			public static LocString DECOR_ITEM_CLASS = "Decor Item";

			// Token: 0x0200279A RID: 10138
			public class CRITERIA_FAILED
			{
				// Token: 0x0400AE1A RID: 44570
				public static LocString MISSING_BUILDING = "Missing {0}";

				// Token: 0x0400AE1B RID: 44571
				public static LocString FAILED = "{0}";
			}

			// Token: 0x0200279B RID: 10139
			public class CEILING_HEIGHT
			{
				// Token: 0x0400AE1C RID: 44572
				public static LocString NAME = "Minimum height: {0} tiles";

				// Token: 0x0400AE1D RID: 44573
				public static LocString DESCRIPTION = "Must have a ceiling height of at least {0} tiles";
			}

			// Token: 0x0200279C RID: 10140
			public class MINIMUM_SIZE
			{
				// Token: 0x0400AE1E RID: 44574
				public static LocString NAME = "Minimum size: {0} tiles";

				// Token: 0x0400AE1F RID: 44575
				public static LocString DESCRIPTION = "Must have an area of at least {0} tiles";
			}

			// Token: 0x0200279D RID: 10141
			public class MAXIMUM_SIZE
			{
				// Token: 0x0400AE20 RID: 44576
				public static LocString NAME = "Maximum size: {0} tiles";

				// Token: 0x0400AE21 RID: 44577
				public static LocString DESCRIPTION = "Must have an area no larger than {0} tiles";
			}

			// Token: 0x0200279E RID: 10142
			public class HAS_BED
			{
				// Token: 0x0400AE22 RID: 44578
				public static LocString NAME = "One or more beds";

				// Token: 0x0400AE23 RID: 44579
				public static LocString DESCRIPTION = "Requires at least one Cot or Comfy Bed";
			}

			// Token: 0x0200279F RID: 10143
			public class HAS_LUXURY_BED
			{
				// Token: 0x0400AE24 RID: 44580
				public static LocString NAME = "One or more Comfy Beds";

				// Token: 0x0400AE25 RID: 44581
				public static LocString DESCRIPTION = "Requires at least one Comfy Bed";
			}

			// Token: 0x020027A0 RID: 10144
			public class LUXURY_BED_SINGLE
			{
				// Token: 0x0400AE26 RID: 44582
				public static LocString NAME = "Single Comfy Bed";

				// Token: 0x0400AE27 RID: 44583
				public static LocString DESCRIPTION = "Must have no more than one Comfy Bed";
			}

			// Token: 0x020027A1 RID: 10145
			public class BED_SINGLE
			{
				// Token: 0x0400AE28 RID: 44584
				public static LocString NAME = "Single bed";

				// Token: 0x0400AE29 RID: 44585
				public static LocString DESCRIPTION = "Must have no more than one Cot or Comfy Bed";
			}

			// Token: 0x020027A2 RID: 10146
			public class IS_BACKWALLED
			{
				// Token: 0x0400AE2A RID: 44586
				public static LocString NAME = "Has backwall tiles";

				// Token: 0x0400AE2B RID: 44587
				public static LocString DESCRIPTION = "Must be covered in backwall tiles";
			}

			// Token: 0x020027A3 RID: 10147
			public class NO_COTS
			{
				// Token: 0x0400AE2C RID: 44588
				public static LocString NAME = "No Cots";

				// Token: 0x0400AE2D RID: 44589
				public static LocString DESCRIPTION = "Room cannot contain a Cot";
			}

			// Token: 0x020027A4 RID: 10148
			public class NO_LUXURY_BEDS
			{
				// Token: 0x0400AE2E RID: 44590
				public static LocString NAME = "No Comfy Beds";

				// Token: 0x0400AE2F RID: 44591
				public static LocString DESCRIPTION = "Room cannot contain a Comfy Bed";
			}

			// Token: 0x020027A5 RID: 10149
			public class BED_MULTIPLE
			{
				// Token: 0x0400AE30 RID: 44592
				public static LocString NAME = "Beds";

				// Token: 0x0400AE31 RID: 44593
				public static LocString DESCRIPTION = "Requires two or more Cots or Comfy Beds";
			}

			// Token: 0x020027A6 RID: 10150
			public class BUILDING_DECOR_POSITIVE
			{
				// Token: 0x0400AE32 RID: 44594
				public static LocString NAME = "Positive decor";

				// Token: 0x0400AE33 RID: 44595
				public static LocString DESCRIPTION = "Requires at least one building with positive decor";
			}

			// Token: 0x020027A7 RID: 10151
			public class DECORATIVE_ITEM
			{
				// Token: 0x0400AE34 RID: 44596
				public static LocString NAME = "Decor item ({0})";

				// Token: 0x0400AE35 RID: 44597
				public static LocString DESCRIPTION = "Requires {0} or more Decor items";
			}

			// Token: 0x020027A8 RID: 10152
			public class DECORATIVE_ITEM_N
			{
				// Token: 0x0400AE36 RID: 44598
				public static LocString NAME = "Decor item: +{0} Decor";

				// Token: 0x0400AE37 RID: 44599
				public static LocString DESCRIPTION = "Requires a decorative item with a minimum Decor value of {0}";
			}

			// Token: 0x020027A9 RID: 10153
			public class CLINIC
			{
				// Token: 0x0400AE38 RID: 44600
				public static LocString NAME = "Medical equipment";

				// Token: 0x0400AE39 RID: 44601
				public static LocString DESCRIPTION = "Requires one or more Sick Bays or Disease Clinics";
			}

			// Token: 0x020027AA RID: 10154
			public class POWER_STATION
			{
				// Token: 0x0400AE3A RID: 44602
				public static LocString NAME = "Power Control Station";

				// Token: 0x0400AE3B RID: 44603
				public static LocString DESCRIPTION = "Requires a single Power Control Station";
			}

			// Token: 0x020027AB RID: 10155
			public class FARM_STATION
			{
				// Token: 0x0400AE3C RID: 44604
				public static LocString NAME = "Farm Station";

				// Token: 0x0400AE3D RID: 44605
				public static LocString DESCRIPTION = "Requires a single Farm Station";
			}

			// Token: 0x020027AC RID: 10156
			public class CREATURE_RELOCATOR
			{
				// Token: 0x0400AE3E RID: 44606
				public static LocString NAME = "Critter Relocator";

				// Token: 0x0400AE3F RID: 44607
				public static LocString DESCRIPTION = "Requires a single Critter Drop-Off";
			}

			// Token: 0x020027AD RID: 10157
			public class CREATURE_FEEDER
			{
				// Token: 0x0400AE40 RID: 44608
				public static LocString NAME = "Critter Feeder";

				// Token: 0x0400AE41 RID: 44609
				public static LocString DESCRIPTION = "Requires a single Critter Feeder";
			}

			// Token: 0x020027AE RID: 10158
			public class RANCH_STATION
			{
				// Token: 0x0400AE42 RID: 44610
				public static LocString NAME = "Grooming Station";

				// Token: 0x0400AE43 RID: 44611
				public static LocString DESCRIPTION = "Requires a single Grooming Station";
			}

			// Token: 0x020027AF RID: 10159
			public class SPICE_STATION
			{
				// Token: 0x0400AE44 RID: 44612
				public static LocString NAME = "Spice Grinder";

				// Token: 0x0400AE45 RID: 44613
				public static LocString DESCRIPTION = "Requires a single Spice Grinder";
			}

			// Token: 0x020027B0 RID: 10160
			public class COOK_TOP
			{
				// Token: 0x0400AE46 RID: 44614
				public static LocString NAME = "Electric Grill or Gas Range";

				// Token: 0x0400AE47 RID: 44615
				public static LocString DESCRIPTION = "Requires a single Electric Grill or Gas Range";
			}

			// Token: 0x020027B1 RID: 10161
			public class REFRIGERATOR
			{
				// Token: 0x0400AE48 RID: 44616
				public static LocString NAME = "Refrigerator";

				// Token: 0x0400AE49 RID: 44617
				public static LocString DESCRIPTION = "Requires a single Refrigerator";
			}

			// Token: 0x020027B2 RID: 10162
			public class REC_BUILDING
			{
				// Token: 0x0400AE4A RID: 44618
				public static LocString NAME = "Recreational building";

				// Token: 0x0400AE4B RID: 44619
				public static LocString DESCRIPTION = "Requires one or more recreational buildings";
			}

			// Token: 0x020027B3 RID: 10163
			public class PARK_BUILDING
			{
				// Token: 0x0400AE4C RID: 44620
				public static LocString NAME = "Park Sign";

				// Token: 0x0400AE4D RID: 44621
				public static LocString DESCRIPTION = "Requires one or more Park Signs";
			}

			// Token: 0x020027B4 RID: 10164
			public class MACHINE_SHOP
			{
				// Token: 0x0400AE4E RID: 44622
				public static LocString NAME = "Mechanics Station";

				// Token: 0x0400AE4F RID: 44623
				public static LocString DESCRIPTION = "Requires requires one or more Mechanics Stations";
			}

			// Token: 0x020027B5 RID: 10165
			public class FOOD_BOX
			{
				// Token: 0x0400AE50 RID: 44624
				public static LocString NAME = "Food storage";

				// Token: 0x0400AE51 RID: 44625
				public static LocString DESCRIPTION = "Requires one or more Ration Boxes or Refrigerators";
			}

			// Token: 0x020027B6 RID: 10166
			public class LIGHT
			{
				// Token: 0x0400AE52 RID: 44626
				public static LocString NAME = "Light source";

				// Token: 0x0400AE53 RID: 44627
				public static LocString DESCRIPTION = "Requires one or more light sources";
			}

			// Token: 0x020027B7 RID: 10167
			public class DESTRESSING_BUILDING
			{
				// Token: 0x0400AE54 RID: 44628
				public static LocString NAME = "De-Stressing Building";

				// Token: 0x0400AE55 RID: 44629
				public static LocString DESCRIPTION = "Requires one or more De-Stressing Building";
			}

			// Token: 0x020027B8 RID: 10168
			public class MASSAGE_TABLE
			{
				// Token: 0x0400AE56 RID: 44630
				public static LocString NAME = "Massage Table";

				// Token: 0x0400AE57 RID: 44631
				public static LocString DESCRIPTION = "Requires one or more Massage Tables";
			}

			// Token: 0x020027B9 RID: 10169
			public class MESS_STATION_SINGLE
			{
				// Token: 0x0400AE58 RID: 44632
				public static LocString NAME = "Mess Table";

				// Token: 0x0400AE59 RID: 44633
				public static LocString DESCRIPTION = "Requires a single Mess Table";
			}

			// Token: 0x020027BA RID: 10170
			public class NO_MESS_STATION
			{
				// Token: 0x0400AE5A RID: 44634
				public static LocString NAME = "No Mess Table";

				// Token: 0x0400AE5B RID: 44635
				public static LocString DESCRIPTION = "Cannot contain a Mess Table";
			}

			// Token: 0x020027BB RID: 10171
			public class MESS_STATION_MULTIPLE
			{
				// Token: 0x0400AE5C RID: 44636
				public static LocString NAME = "Mess Tables";

				// Token: 0x0400AE5D RID: 44637
				public static LocString DESCRIPTION = "Requires two or more Mess Tables";
			}

			// Token: 0x020027BC RID: 10172
			public class RESEARCH_STATION
			{
				// Token: 0x0400AE5E RID: 44638
				public static LocString NAME = "Research station";

				// Token: 0x0400AE5F RID: 44639
				public static LocString DESCRIPTION = "Requires one or more Research Stations or Super Computers";
			}

			// Token: 0x020027BD RID: 10173
			public class TOILET
			{
				// Token: 0x0400AE60 RID: 44640
				public static LocString NAME = "Toilet";

				// Token: 0x0400AE61 RID: 44641
				public static LocString DESCRIPTION = "Requires one or more Outhouses or Lavatories";
			}

			// Token: 0x020027BE RID: 10174
			public class FLUSH_TOILET
			{
				// Token: 0x0400AE62 RID: 44642
				public static LocString NAME = "Flush Toilet";

				// Token: 0x0400AE63 RID: 44643
				public static LocString DESCRIPTION = "Requires one or more Lavatories";
			}

			// Token: 0x020027BF RID: 10175
			public class NO_OUTHOUSES
			{
				// Token: 0x0400AE64 RID: 44644
				public static LocString NAME = "No Outhouses";

				// Token: 0x0400AE65 RID: 44645
				public static LocString DESCRIPTION = "Cannot contain basic Outhouses";
			}

			// Token: 0x020027C0 RID: 10176
			public class WASH_STATION
			{
				// Token: 0x0400AE66 RID: 44646
				public static LocString NAME = "Wash station";

				// Token: 0x0400AE67 RID: 44647
				public static LocString DESCRIPTION = "Requires one or more Wash Basins, Sinks, Hand Sanitizers, or Showers";
			}

			// Token: 0x020027C1 RID: 10177
			public class ADVANCED_WASH_STATION
			{
				// Token: 0x0400AE68 RID: 44648
				public static LocString NAME = "Plumbed wash station";

				// Token: 0x0400AE69 RID: 44649
				public static LocString DESCRIPTION = "Requires one or more Sinks, Hand Sanitizers, or Showers";
			}

			// Token: 0x020027C2 RID: 10178
			public class NO_INDUSTRIAL_MACHINERY
			{
				// Token: 0x0400AE6A RID: 44650
				public static LocString NAME = "No industrial machinery";

				// Token: 0x0400AE6B RID: 44651
				public static LocString DESCRIPTION = "Cannot contain any building labeled Industrial Machinery";
			}

			// Token: 0x020027C3 RID: 10179
			public class WILDANIMAL
			{
				// Token: 0x0400AE6C RID: 44652
				public static LocString NAME = "Wildlife";

				// Token: 0x0400AE6D RID: 44653
				public static LocString DESCRIPTION = "Requires at least one wild critter";
			}

			// Token: 0x020027C4 RID: 10180
			public class WILDANIMALS
			{
				// Token: 0x0400AE6E RID: 44654
				public static LocString NAME = "More wildlife";

				// Token: 0x0400AE6F RID: 44655
				public static LocString DESCRIPTION = "Requires two or more wild critters";
			}

			// Token: 0x020027C5 RID: 10181
			public class WILDPLANT
			{
				// Token: 0x0400AE70 RID: 44656
				public static LocString NAME = "Two wild plants";

				// Token: 0x0400AE71 RID: 44657
				public static LocString DESCRIPTION = "Requires two or more wild plants";
			}

			// Token: 0x020027C6 RID: 10182
			public class WILDPLANTS
			{
				// Token: 0x0400AE72 RID: 44658
				public static LocString NAME = "Four wild plants";

				// Token: 0x0400AE73 RID: 44659
				public static LocString DESCRIPTION = "Requires four or more wild plants";
			}

			// Token: 0x020027C7 RID: 10183
			public class SCIENCE_BUILDING
			{
				// Token: 0x0400AE74 RID: 44660
				public static LocString NAME = "Science building";

				// Token: 0x0400AE75 RID: 44661
				public static LocString DESCRIPTION = "Requires one or more science buildings";
			}

			// Token: 0x020027C8 RID: 10184
			public class SCIENCE_BUILDINGS
			{
				// Token: 0x0400AE76 RID: 44662
				public static LocString NAME = "Two science buildings";

				// Token: 0x0400AE77 RID: 44663
				public static LocString DESCRIPTION = "Requires two or more science buildings";
			}
		}

		// Token: 0x02001DDD RID: 7645
		public class DETAILS
		{
			// Token: 0x04008921 RID: 35105
			public static LocString HEADER = "Room Details";

			// Token: 0x020027C9 RID: 10185
			public class ASSIGNED_TO
			{
				// Token: 0x0400AE78 RID: 44664
				public static LocString NAME = "<b>Assignments:</b>\n{0}";

				// Token: 0x0400AE79 RID: 44665
				public static LocString UNASSIGNED = "Unassigned";
			}

			// Token: 0x020027CA RID: 10186
			public class AVERAGE_TEMPERATURE
			{
				// Token: 0x0400AE7A RID: 44666
				public static LocString NAME = "Average temperature: {0}";
			}

			// Token: 0x020027CB RID: 10187
			public class AVERAGE_ATMO_MASS
			{
				// Token: 0x0400AE7B RID: 44667
				public static LocString NAME = "Average air pressure: {0}";
			}

			// Token: 0x020027CC RID: 10188
			public class SIZE
			{
				// Token: 0x0400AE7C RID: 44668
				public static LocString NAME = "Room size: {0} Tiles";
			}

			// Token: 0x020027CD RID: 10189
			public class BUILDING_COUNT
			{
				// Token: 0x0400AE7D RID: 44669
				public static LocString NAME = "Buildings: {0}";
			}

			// Token: 0x020027CE RID: 10190
			public class CREATURE_COUNT
			{
				// Token: 0x0400AE7E RID: 44670
				public static LocString NAME = "Critters: {0}";
			}

			// Token: 0x020027CF RID: 10191
			public class PLANT_COUNT
			{
				// Token: 0x0400AE7F RID: 44671
				public static LocString NAME = "Plants: {0}";
			}
		}

		// Token: 0x02001DDE RID: 7646
		public class EFFECTS
		{
			// Token: 0x04008922 RID: 35106
			public static LocString HEADER = "<b>Effects:</b>";
		}
	}
}
