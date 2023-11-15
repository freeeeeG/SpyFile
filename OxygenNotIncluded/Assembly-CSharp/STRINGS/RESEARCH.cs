using System;

namespace STRINGS
{
	// Token: 0x02000DB0 RID: 3504
	public class RESEARCH
	{
		// Token: 0x02001DEF RID: 7663
		public class MESSAGING
		{
			// Token: 0x040089B7 RID: 35255
			public static LocString NORESEARCHSELECTED = "No research selected";

			// Token: 0x040089B8 RID: 35256
			public static LocString RESEARCHTYPEREQUIRED = "{0} required";

			// Token: 0x040089B9 RID: 35257
			public static LocString RESEARCHTYPEALSOREQUIRED = "{0} also required";

			// Token: 0x040089BA RID: 35258
			public static LocString NO_RESEARCHER_SKILL = "No Researchers assigned";

			// Token: 0x040089BB RID: 35259
			public static LocString NO_RESEARCHER_SKILL_TOOLTIP = "The selected research focus requires {ResearchType} to complete\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " and teach a Duplicant the {ResearchType} Skill to use this building";

			// Token: 0x040089BC RID: 35260
			public static LocString MISSING_RESEARCH_STATION = "Missing Research Station";

			// Token: 0x040089BD RID: 35261
			public static LocString MISSING_RESEARCH_STATION_TOOLTIP = "The selected research focus requires a {0} to perform\n\nOpen the " + UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10) + " of the Build Menu to construct one";

			// Token: 0x02002A35 RID: 10805
			public static class DLC
			{
				// Token: 0x0400B404 RID: 46084
				public static LocString EXPANSION1 = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"\n\n<i>",
					UI.DLC1.NAME,
					"</i>",
					UI.PST_KEYWORD,
					" DLC Content"
				});
			}
		}

		// Token: 0x02001DF0 RID: 7664
		public class TYPES
		{
			// Token: 0x040089BE RID: 35262
			public static LocString MISSINGRECIPEDESC = "Missing Recipe Description";

			// Token: 0x02002A36 RID: 10806
			public class ALPHA
			{
				// Token: 0x0400B405 RID: 46085
				public static LocString NAME = "Novice Research";

				// Token: 0x0400B406 RID: 46086
				public static LocString DESC = UI.FormatAsLink("Novice Research", "RESEARCH") + " is required to unlock basic technologies.\nIt can be conducted at a " + UI.FormatAsLink("Research Station", "RESEARCHCENTER") + ".";

				// Token: 0x0400B407 RID: 46087
				public static LocString RECIPEDESC = "Unlocks rudimentary technologies.";
			}

			// Token: 0x02002A37 RID: 10807
			public class BETA
			{
				// Token: 0x0400B408 RID: 46088
				public static LocString NAME = "Advanced Research";

				// Token: 0x0400B409 RID: 46089
				public static LocString DESC = UI.FormatAsLink("Advanced Research", "RESEARCH") + " is required to unlock improved technologies.\nIt can be conducted at a " + UI.FormatAsLink("Super Computer", "ADVANCEDRESEARCHCENTER") + ".";

				// Token: 0x0400B40A RID: 46090
				public static LocString RECIPEDESC = "Unlocks improved technologies.";
			}

			// Token: 0x02002A38 RID: 10808
			public class GAMMA
			{
				// Token: 0x0400B40B RID: 46091
				public static LocString NAME = "Interstellar Research";

				// Token: 0x0400B40C RID: 46092
				public static LocString DESC = UI.FormatAsLink("Interstellar Research", "RESEARCH") + " is required to unlock space technologies.\nIt can be conducted at a " + UI.FormatAsLink("Virtual Planetarium", "COSMICRESEARCHCENTER") + ".";

				// Token: 0x0400B40D RID: 46093
				public static LocString RECIPEDESC = "Unlocks cutting-edge technologies.";
			}

			// Token: 0x02002A39 RID: 10809
			public class DELTA
			{
				// Token: 0x0400B40E RID: 46094
				public static LocString NAME = "Applied Sciences Research";

				// Token: 0x0400B40F RID: 46095
				public static LocString DESC = UI.FormatAsLink("Applied Sciences Research", "RESEARCH") + " is required to unlock materials science technologies.\nIt can be conducted at a " + UI.FormatAsLink("Materials Study Terminal", "NUCLEARRESEARCHCENTER") + ".";

				// Token: 0x0400B410 RID: 46096
				public static LocString RECIPEDESC = "Unlocks next wave technologies.";
			}

			// Token: 0x02002A3A RID: 10810
			public class ORBITAL
			{
				// Token: 0x0400B411 RID: 46097
				public static LocString NAME = "Data Analysis Research";

				// Token: 0x0400B412 RID: 46098
				public static LocString DESC = UI.FormatAsLink("Data Analysis Research", "RESEARCH") + " is required to unlock Data Analysis technologies.\nIt can be conducted at a " + UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER") + ".";

				// Token: 0x0400B413 RID: 46099
				public static LocString RECIPEDESC = "Unlocks out-of-this-world technologies.";
			}
		}

		// Token: 0x02001DF1 RID: 7665
		public class OTHER_TECH_ITEMS
		{
			// Token: 0x02002A3B RID: 10811
			public class AUTOMATION_OVERLAY
			{
				// Token: 0x0400B414 RID: 46100
				public static LocString NAME = UI.FormatAsOverlay("Automation Overlay");

				// Token: 0x0400B415 RID: 46101
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Automation Overlay") + ".";
			}

			// Token: 0x02002A3C RID: 10812
			public class SUITS_OVERLAY
			{
				// Token: 0x0400B416 RID: 46102
				public static LocString NAME = UI.FormatAsOverlay("Exosuit Overlay");

				// Token: 0x0400B417 RID: 46103
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Exosuit Overlay") + ".";
			}

			// Token: 0x02002A3D RID: 10813
			public class JET_SUIT
			{
				// Token: 0x0400B418 RID: 46104
				public static LocString NAME = UI.PRE_KEYWORD + "Jet Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400B419 RID: 46105
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Jet Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02002A3E RID: 10814
			public class OXYGEN_MASK
			{
				// Token: 0x0400B41A RID: 46106
				public static LocString NAME = UI.PRE_KEYWORD + "Oxygen Mask" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400B41B RID: 46107
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Oxygen Masks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02002A3F RID: 10815
			public class LEAD_SUIT
			{
				// Token: 0x0400B41C RID: 46108
				public static LocString NAME = UI.PRE_KEYWORD + "Lead Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400B41D RID: 46109
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Lead Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02002A40 RID: 10816
			public class ATMO_SUIT
			{
				// Token: 0x0400B41E RID: 46110
				public static LocString NAME = UI.PRE_KEYWORD + "Atmo Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400B41F RID: 46111
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Atmo Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02002A41 RID: 10817
			public class BETA_RESEARCH_POINT
			{
				// Token: 0x0400B420 RID: 46112
				public static LocString NAME = UI.PRE_KEYWORD + "Advanced Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400B421 RID: 46113
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Advanced Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02002A42 RID: 10818
			public class GAMMA_RESEARCH_POINT
			{
				// Token: 0x0400B422 RID: 46114
				public static LocString NAME = UI.PRE_KEYWORD + "Interstellar Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400B423 RID: 46115
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Interstellar Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02002A43 RID: 10819
			public class DELTA_RESEARCH_POINT
			{
				// Token: 0x0400B424 RID: 46116
				public static LocString NAME = UI.PRE_KEYWORD + "Materials Science Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400B425 RID: 46117
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Materials Science Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02002A44 RID: 10820
			public class ORBITAL_RESEARCH_POINT
			{
				// Token: 0x0400B426 RID: 46118
				public static LocString NAME = UI.PRE_KEYWORD + "Data Analysis Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400B427 RID: 46119
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Data Analysis Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02002A45 RID: 10821
			public class CONVEYOR_OVERLAY
			{
				// Token: 0x0400B428 RID: 46120
				public static LocString NAME = UI.FormatAsOverlay("Conveyor Overlay");

				// Token: 0x0400B429 RID: 46121
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Conveyor Overlay") + ".";
			}
		}

		// Token: 0x02001DF2 RID: 7666
		public class TREES
		{
			// Token: 0x040089BF RID: 35263
			public static LocString TITLE_FOOD = "Food";

			// Token: 0x040089C0 RID: 35264
			public static LocString TITLE_POWER = "Power";

			// Token: 0x040089C1 RID: 35265
			public static LocString TITLE_SOLIDS = "Solid Material";

			// Token: 0x040089C2 RID: 35266
			public static LocString TITLE_COLONYDEVELOPMENT = "Colony Development";

			// Token: 0x040089C3 RID: 35267
			public static LocString TITLE_RADIATIONTECH = "Radiation Technologies";

			// Token: 0x040089C4 RID: 35268
			public static LocString TITLE_MEDICINE = "Medicine";

			// Token: 0x040089C5 RID: 35269
			public static LocString TITLE_LIQUIDS = "Liquids";

			// Token: 0x040089C6 RID: 35270
			public static LocString TITLE_GASES = "Gases";

			// Token: 0x040089C7 RID: 35271
			public static LocString TITLE_SUITS = "Exosuits";

			// Token: 0x040089C8 RID: 35272
			public static LocString TITLE_DECOR = "Decor";

			// Token: 0x040089C9 RID: 35273
			public static LocString TITLE_COMPUTERS = "Computers";

			// Token: 0x040089CA RID: 35274
			public static LocString TITLE_ROCKETS = "Rocketry";
		}

		// Token: 0x02001DF3 RID: 7667
		public class TECHS
		{
			// Token: 0x02002A46 RID: 10822
			public class JOBS
			{
				// Token: 0x0400B42A RID: 46122
				public static LocString NAME = UI.FormatAsLink("Employment", "JOBS");

				// Token: 0x0400B42B RID: 46123
				public static LocString DESC = "Exchange the skill points earned by Duplicants for new traits and abilities.";
			}

			// Token: 0x02002A47 RID: 10823
			public class IMPROVEDOXYGEN
			{
				// Token: 0x0400B42C RID: 46124
				public static LocString NAME = UI.FormatAsLink("Air Systems", "IMPROVEDOXYGEN");

				// Token: 0x0400B42D RID: 46125
				public static LocString DESC = "Maintain clean, breathable air in the colony.";
			}

			// Token: 0x02002A48 RID: 10824
			public class FARMINGTECH
			{
				// Token: 0x0400B42E RID: 46126
				public static LocString NAME = UI.FormatAsLink("Basic Farming", "FARMINGTECH");

				// Token: 0x0400B42F RID: 46127
				public static LocString DESC = "Learn the introductory principles of " + UI.FormatAsLink("Plant", "PLANTS") + " domestication.";
			}

			// Token: 0x02002A49 RID: 10825
			public class AGRICULTURE
			{
				// Token: 0x0400B430 RID: 46128
				public static LocString NAME = UI.FormatAsLink("Agriculture", "AGRICULTURE");

				// Token: 0x0400B431 RID: 46129
				public static LocString DESC = "Master the agricultural art of crop raising.";
			}

			// Token: 0x02002A4A RID: 10826
			public class RANCHING
			{
				// Token: 0x0400B432 RID: 46130
				public static LocString NAME = UI.FormatAsLink("Ranching", "RANCHING");

				// Token: 0x0400B433 RID: 46131
				public static LocString DESC = "Tame and care for wild critters.";
			}

			// Token: 0x02002A4B RID: 10827
			public class ANIMALCONTROL
			{
				// Token: 0x0400B434 RID: 46132
				public static LocString NAME = UI.FormatAsLink("Animal Control", "ANIMALCONTROL");

				// Token: 0x0400B435 RID: 46133
				public static LocString DESC = "Useful techniques to manage critter populations in the colony.";
			}

			// Token: 0x02002A4C RID: 10828
			public class DAIRYOPERATION
			{
				// Token: 0x0400B436 RID: 46134
				public static LocString NAME = UI.FormatAsLink("Brackene Flow", "DAIRYOPERATION");

				// Token: 0x0400B437 RID: 46135
				public static LocString DESC = "Advanced production, processing and distribution of this fluid resource.";
			}

			// Token: 0x02002A4D RID: 10829
			public class FOODREPURPOSING
			{
				// Token: 0x0400B438 RID: 46136
				public static LocString NAME = UI.FormatAsLink("Food Repurposing", "FOODREPURPOSING");

				// Token: 0x0400B439 RID: 46137
				public static LocString DESC = string.Concat(new string[]
				{
					"Blend that leftover ",
					UI.FormatAsLink("Food", "FOOD"),
					" into a ",
					UI.FormatAsLink("Morale", "MORALE"),
					"-boosting slurry."
				});
			}

			// Token: 0x02002A4E RID: 10830
			public class FINEDINING
			{
				// Token: 0x0400B43A RID: 46138
				public static LocString NAME = UI.FormatAsLink("Meal Preparation", "FINEDINING");

				// Token: 0x0400B43B RID: 46139
				public static LocString DESC = "Prepare more nutritious " + UI.FormatAsLink("Food", "FOOD") + " and store it longer before spoiling.";
			}

			// Token: 0x02002A4F RID: 10831
			public class FINERDINING
			{
				// Token: 0x0400B43C RID: 46140
				public static LocString NAME = UI.FormatAsLink("Gourmet Meal Preparation", "FINERDINING");

				// Token: 0x0400B43D RID: 46141
				public static LocString DESC = "Raise colony Morale by cooking the most delicious, high-quality " + UI.FormatAsLink("Foods", "FOOD") + ".";
			}

			// Token: 0x02002A50 RID: 10832
			public class GASPIPING
			{
				// Token: 0x0400B43E RID: 46142
				public static LocString NAME = UI.FormatAsLink("Ventilation", "GASPIPING");

				// Token: 0x0400B43F RID: 46143
				public static LocString DESC = "Rudimentary technologies for installing " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure.";
			}

			// Token: 0x02002A51 RID: 10833
			public class IMPROVEDGASPIPING
			{
				// Token: 0x0400B440 RID: 46144
				public static LocString NAME = UI.FormatAsLink("Improved Ventilation", "IMPROVEDGASPIPING");

				// Token: 0x0400B441 RID: 46145
				public static LocString DESC = UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.";
			}

			// Token: 0x02002A52 RID: 10834
			public class FLOWREDIRECTION
			{
				// Token: 0x0400B442 RID: 46146
				public static LocString NAME = UI.FormatAsLink("Flow Redirection", "FLOWREDIRECTION");

				// Token: 0x0400B443 RID: 46147
				public static LocString DESC = "Balance on irrigated concave platforms for a " + UI.FormatAsLink("Morale", "MORALE") + " boost.";
			}

			// Token: 0x02002A53 RID: 10835
			public class LIQUIDDISTRIBUTION
			{
				// Token: 0x0400B444 RID: 46148
				public static LocString NAME = UI.FormatAsLink("Liquid Distribution", "LIQUIDDISTRIBUTION");

				// Token: 0x0400B445 RID: 46149
				public static LocString DESC = "Internal rocket hookups for liquid resources.";
			}

			// Token: 0x02002A54 RID: 10836
			public class TEMPERATUREMODULATION
			{
				// Token: 0x0400B446 RID: 46150
				public static LocString NAME = UI.FormatAsLink("Temperature Modulation", "TEMPERATUREMODULATION");

				// Token: 0x0400B447 RID: 46151
				public static LocString DESC = "Precise " + UI.FormatAsLink("Temperature", "HEAT") + " altering technologies to keep my colony at the perfect Kelvin.";
			}

			// Token: 0x02002A55 RID: 10837
			public class HVAC
			{
				// Token: 0x0400B448 RID: 46152
				public static LocString NAME = UI.FormatAsLink("HVAC", "HVAC");

				// Token: 0x0400B449 RID: 46153
				public static LocString DESC = string.Concat(new string[]
				{
					"Regulate ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" in the colony for ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" cultivation and Duplicant comfort."
				});
			}

			// Token: 0x02002A56 RID: 10838
			public class GASDISTRIBUTION
			{
				// Token: 0x0400B44A RID: 46154
				public static LocString NAME = UI.FormatAsLink("Gas Distribution", "GASDISTRIBUTION");

				// Token: 0x0400B44B RID: 46155
				public static LocString DESC = "Internal rocket hookups for gas resources.";
			}

			// Token: 0x02002A57 RID: 10839
			public class LIQUIDTEMPERATURE
			{
				// Token: 0x0400B44C RID: 46156
				public static LocString NAME = UI.FormatAsLink("Liquid Tuning", "LIQUIDTEMPERATURE");

				// Token: 0x0400B44D RID: 46157
				public static LocString DESC = string.Concat(new string[]
				{
					"Easily manipulate ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" ",
					UI.FormatAsLink("Heat", "Temperatures"),
					" with these temperature regulating technologies."
				});
			}

			// Token: 0x02002A58 RID: 10840
			public class INSULATION
			{
				// Token: 0x0400B44E RID: 46158
				public static LocString NAME = UI.FormatAsLink("Insulation", "INSULATION");

				// Token: 0x0400B44F RID: 46159
				public static LocString DESC = "Improve " + UI.FormatAsLink("Heat", "Heat") + " distribution within the colony and guard buildings from extreme temperatures.";
			}

			// Token: 0x02002A59 RID: 10841
			public class PRESSUREMANAGEMENT
			{
				// Token: 0x0400B450 RID: 46160
				public static LocString NAME = UI.FormatAsLink("Pressure Management", "PRESSUREMANAGEMENT");

				// Token: 0x0400B451 RID: 46161
				public static LocString DESC = "Unlock technologies to manage colony pressure and atmosphere.";
			}

			// Token: 0x02002A5A RID: 10842
			public class PORTABLEGASSES
			{
				// Token: 0x0400B452 RID: 46162
				public static LocString NAME = UI.FormatAsLink("Portable Gases", "PORTABLEGASSES");

				// Token: 0x0400B453 RID: 46163
				public static LocString DESC = "Unlock technologies to easily move gases around your colony.";
			}

			// Token: 0x02002A5B RID: 10843
			public class DIRECTEDAIRSTREAMS
			{
				// Token: 0x0400B454 RID: 46164
				public static LocString NAME = UI.FormatAsLink("Decontamination", "DIRECTEDAIRSTREAMS");

				// Token: 0x0400B455 RID: 46165
				public static LocString DESC = "Instruments to help reduce " + UI.FormatAsLink("Germ", "DISEASE") + " spread within the base.";
			}

			// Token: 0x02002A5C RID: 10844
			public class LIQUIDFILTERING
			{
				// Token: 0x0400B456 RID: 46166
				public static LocString NAME = UI.FormatAsLink("Liquid-Based Refinement Processes", "LIQUIDFILTERING");

				// Token: 0x0400B457 RID: 46167
				public static LocString DESC = "Use pumped " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " to filter out unwanted elements.";
			}

			// Token: 0x02002A5D RID: 10845
			public class LIQUIDPIPING
			{
				// Token: 0x0400B458 RID: 46168
				public static LocString NAME = UI.FormatAsLink("Plumbing", "LIQUIDPIPING");

				// Token: 0x0400B459 RID: 46169
				public static LocString DESC = "Rudimentary technologies for installing " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure.";
			}

			// Token: 0x02002A5E RID: 10846
			public class IMPROVEDLIQUIDPIPING
			{
				// Token: 0x0400B45A RID: 46170
				public static LocString NAME = UI.FormatAsLink("Improved Plumbing", "IMPROVEDLIQUIDPIPING");

				// Token: 0x0400B45B RID: 46171
				public static LocString DESC = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.";
			}

			// Token: 0x02002A5F RID: 10847
			public class PRECISIONPLUMBING
			{
				// Token: 0x0400B45C RID: 46172
				public static LocString NAME = UI.FormatAsLink("Advanced Caffeination", "PRECISIONPLUMBING");

				// Token: 0x0400B45D RID: 46173
				public static LocString DESC = "Let Duplicants relax after a long day of subterranean digging with a shot of warm beanjuice.";
			}

			// Token: 0x02002A60 RID: 10848
			public class SANITATIONSCIENCES
			{
				// Token: 0x0400B45E RID: 46174
				public static LocString NAME = UI.FormatAsLink("Sanitation", "SANITATIONSCIENCES");

				// Token: 0x0400B45F RID: 46175
				public static LocString DESC = "Make daily ablutions less of a hassle.";
			}

			// Token: 0x02002A61 RID: 10849
			public class ADVANCEDSANITATION
			{
				// Token: 0x0400B460 RID: 46176
				public static LocString NAME = UI.FormatAsLink("Advanced Sanitation", "ADVANCEDSANITATION");

				// Token: 0x0400B461 RID: 46177
				public static LocString DESC = "Clean up dirty Duplicants.";
			}

			// Token: 0x02002A62 RID: 10850
			public class MEDICINEI
			{
				// Token: 0x0400B462 RID: 46178
				public static LocString NAME = UI.FormatAsLink("Pharmacology", "MEDICINEI");

				// Token: 0x0400B463 RID: 46179
				public static LocString DESC = "Compound natural cures to fight the most common " + UI.FormatAsLink("Sicknesses", "SICKNESSES") + " that plague Duplicants.";
			}

			// Token: 0x02002A63 RID: 10851
			public class MEDICINEII
			{
				// Token: 0x0400B464 RID: 46180
				public static LocString NAME = UI.FormatAsLink("Medical Equipment", "MEDICINEII");

				// Token: 0x0400B465 RID: 46181
				public static LocString DESC = "The basic necessities doctors need to facilitate patient care.";
			}

			// Token: 0x02002A64 RID: 10852
			public class MEDICINEIII
			{
				// Token: 0x0400B466 RID: 46182
				public static LocString NAME = UI.FormatAsLink("Pathogen Diagnostics", "MEDICINEIII");

				// Token: 0x0400B467 RID: 46183
				public static LocString DESC = "Stop Germs at the source using special medical automation technology.";
			}

			// Token: 0x02002A65 RID: 10853
			public class MEDICINEIV
			{
				// Token: 0x0400B468 RID: 46184
				public static LocString NAME = UI.FormatAsLink("Micro-Targeted Medicine", "MEDICINEIV");

				// Token: 0x0400B469 RID: 46185
				public static LocString DESC = "State of the art equipment to conquer the most stubborn of illnesses.";
			}

			// Token: 0x02002A66 RID: 10854
			public class ADVANCEDFILTRATION
			{
				// Token: 0x0400B46A RID: 46186
				public static LocString NAME = UI.FormatAsLink("Filtration", "ADVANCEDFILTRATION");

				// Token: 0x0400B46B RID: 46187
				public static LocString DESC = string.Concat(new string[]
				{
					"Basic technologies for filtering ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x02002A67 RID: 10855
			public class POWERREGULATION
			{
				// Token: 0x0400B46C RID: 46188
				public static LocString NAME = UI.FormatAsLink("Power Regulation", "POWERREGULATION");

				// Token: 0x0400B46D RID: 46189
				public static LocString DESC = "Prevent wasted " + UI.FormatAsLink("Power", "POWER") + " with improved electrical tools.";
			}

			// Token: 0x02002A68 RID: 10856
			public class COMBUSTION
			{
				// Token: 0x0400B46E RID: 46190
				public static LocString NAME = UI.FormatAsLink("Internal Combustion", "COMBUSTION");

				// Token: 0x0400B46F RID: 46191
				public static LocString DESC = "Fuel-powered generators for crude yet powerful " + UI.FormatAsLink("Power", "POWER") + " production.";
			}

			// Token: 0x02002A69 RID: 10857
			public class IMPROVEDCOMBUSTION
			{
				// Token: 0x0400B470 RID: 46192
				public static LocString NAME = UI.FormatAsLink("Fossil Fuels", "IMPROVEDCOMBUSTION");

				// Token: 0x0400B471 RID: 46193
				public static LocString DESC = "Burn dirty fuels for exceptional " + UI.FormatAsLink("Power", "POWER") + " production.";
			}

			// Token: 0x02002A6A RID: 10858
			public class INTERIORDECOR
			{
				// Token: 0x0400B472 RID: 46194
				public static LocString NAME = UI.FormatAsLink("Interior Decor", "INTERIORDECOR");

				// Token: 0x0400B473 RID: 46195
				public static LocString DESC = UI.FormatAsLink("Decor", "DECOR") + " boosting items to counteract the gloom of underground living.";
			}

			// Token: 0x02002A6B RID: 10859
			public class ARTISTRY
			{
				// Token: 0x0400B474 RID: 46196
				public static LocString NAME = UI.FormatAsLink("Artistic Expression", "ARTISTRY");

				// Token: 0x0400B475 RID: 46197
				public static LocString DESC = "Majorly improve " + UI.FormatAsLink("Decor", "DECOR") + " by giving Duplicants the tools of artistic and emotional expression.";
			}

			// Token: 0x02002A6C RID: 10860
			public class CLOTHING
			{
				// Token: 0x0400B476 RID: 46198
				public static LocString NAME = UI.FormatAsLink("Textile Production", "CLOTHING");

				// Token: 0x0400B477 RID: 46199
				public static LocString DESC = "Bring Duplicants the " + UI.FormatAsLink("Morale", "MORALE") + " boosting benefits of soft, cushy fabrics.";
			}

			// Token: 0x02002A6D RID: 10861
			public class ACOUSTICS
			{
				// Token: 0x0400B478 RID: 46200
				public static LocString NAME = UI.FormatAsLink("Sound Amplifiers", "ACOUSTICS");

				// Token: 0x0400B479 RID: 46201
				public static LocString DESC = "Precise control of the audio spectrum allows Duplicants to get funky.";
			}

			// Token: 0x02002A6E RID: 10862
			public class SPACEPOWER
			{
				// Token: 0x0400B47A RID: 46202
				public static LocString NAME = UI.FormatAsLink("Space Power", "SPACEPOWER");

				// Token: 0x0400B47B RID: 46203
				public static LocString DESC = "It's like power... in space!";
			}

			// Token: 0x02002A6F RID: 10863
			public class AMPLIFIERS
			{
				// Token: 0x0400B47C RID: 46204
				public static LocString NAME = UI.FormatAsLink("Power Amplifiers", "AMPLIFIERS");

				// Token: 0x0400B47D RID: 46205
				public static LocString DESC = "Further increased efficacy of " + UI.FormatAsLink("Power", "POWER") + " management to prevent those wasted joules.";
			}

			// Token: 0x02002A70 RID: 10864
			public class LUXURY
			{
				// Token: 0x0400B47E RID: 46206
				public static LocString NAME = UI.FormatAsLink("Home Luxuries", "LUXURY");

				// Token: 0x0400B47F RID: 46207
				public static LocString DESC = "Luxury amenities for advanced " + UI.FormatAsLink("Stress", "STRESS") + " reduction.";
			}

			// Token: 0x02002A71 RID: 10865
			public class ENVIRONMENTALAPPRECIATION
			{
				// Token: 0x0400B480 RID: 46208
				public static LocString NAME = UI.FormatAsLink("Environmental Appreciation", "ENVIRONMENTALAPPRECIATION");

				// Token: 0x0400B481 RID: 46209
				public static LocString DESC = string.Concat(new string[]
				{
					"Improve ",
					UI.FormatAsLink("Morale", "MORALE"),
					" by lazing around in ",
					UI.FormatAsLink("Light", "LIGHT"),
					" with a high Lux value."
				});
			}

			// Token: 0x02002A72 RID: 10866
			public class FINEART
			{
				// Token: 0x0400B482 RID: 46210
				public static LocString NAME = UI.FormatAsLink("Fine Art", "FINEART");

				// Token: 0x0400B483 RID: 46211
				public static LocString DESC = "Broader options for artistic " + UI.FormatAsLink("Decor", "DECOR") + " improvements.";
			}

			// Token: 0x02002A73 RID: 10867
			public class REFRACTIVEDECOR
			{
				// Token: 0x0400B484 RID: 46212
				public static LocString NAME = UI.FormatAsLink("High Culture", "REFRACTIVEDECOR");

				// Token: 0x0400B485 RID: 46213
				public static LocString DESC = "New methods for working with extremely high quality art materials.";
			}

			// Token: 0x02002A74 RID: 10868
			public class RENAISSANCEART
			{
				// Token: 0x0400B486 RID: 46214
				public static LocString NAME = UI.FormatAsLink("Renaissance Art", "RENAISSANCEART");

				// Token: 0x0400B487 RID: 46215
				public static LocString DESC = "The kind of art that culture legacies are made of.";
			}

			// Token: 0x02002A75 RID: 10869
			public class GLASSFURNISHINGS
			{
				// Token: 0x0400B488 RID: 46216
				public static LocString NAME = UI.FormatAsLink("Glass Blowing", "GLASSFURNISHINGS");

				// Token: 0x0400B489 RID: 46217
				public static LocString DESC = "The decorative benefits of glass are both apparent and transparent.";
			}

			// Token: 0x02002A76 RID: 10870
			public class SCREENS
			{
				// Token: 0x0400B48A RID: 46218
				public static LocString NAME = UI.FormatAsLink("New Media", "SCREENS");

				// Token: 0x0400B48B RID: 46219
				public static LocString DESC = "High tech displays with lots of pretty colors.";
			}

			// Token: 0x02002A77 RID: 10871
			public class ADVANCEDPOWERREGULATION
			{
				// Token: 0x0400B48C RID: 46220
				public static LocString NAME = UI.FormatAsLink("Advanced Power Regulation", "ADVANCEDPOWERREGULATION");

				// Token: 0x0400B48D RID: 46221
				public static LocString DESC = "Circuit components required for large scale " + UI.FormatAsLink("Power", "POWER") + " management.";
			}

			// Token: 0x02002A78 RID: 10872
			public class PLASTICS
			{
				// Token: 0x0400B48E RID: 46222
				public static LocString NAME = UI.FormatAsLink("Plastic Manufacturing", "PLASTICS");

				// Token: 0x0400B48F RID: 46223
				public static LocString DESC = "Stable, lightweight, durable. Plastics are useful for a wide array of applications.";
			}

			// Token: 0x02002A79 RID: 10873
			public class SUITS
			{
				// Token: 0x0400B490 RID: 46224
				public static LocString NAME = UI.FormatAsLink("Hazard Protection", "SUITS");

				// Token: 0x0400B491 RID: 46225
				public static LocString DESC = "Vital gear for surviving in extreme conditions and environments.";
			}

			// Token: 0x02002A7A RID: 10874
			public class DISTILLATION
			{
				// Token: 0x0400B492 RID: 46226
				public static LocString NAME = UI.FormatAsLink("Distillation", "DISTILLATION");

				// Token: 0x0400B493 RID: 46227
				public static LocString DESC = "Distill difficult mixtures down to their most useful parts.";
			}

			// Token: 0x02002A7B RID: 10875
			public class CATALYTICS
			{
				// Token: 0x0400B494 RID: 46228
				public static LocString NAME = UI.FormatAsLink("Catalytics", "CATALYTICS");

				// Token: 0x0400B495 RID: 46229
				public static LocString DESC = "Advanced gas manipulation using unique catalysts.";
			}

			// Token: 0x02002A7C RID: 10876
			public class ADVANCEDRESEARCH
			{
				// Token: 0x0400B496 RID: 46230
				public static LocString NAME = UI.FormatAsLink("Advanced Research", "ADVANCEDRESEARCH");

				// Token: 0x0400B497 RID: 46231
				public static LocString DESC = "The tools my colony needs to conduct more advanced, in-depth research.";
			}

			// Token: 0x02002A7D RID: 10877
			public class SPACEPROGRAM
			{
				// Token: 0x0400B498 RID: 46232
				public static LocString NAME = UI.FormatAsLink("Space Program", "SPACEPROGRAM");

				// Token: 0x0400B499 RID: 46233
				public static LocString DESC = "The first steps in getting a Duplicant to space.";
			}

			// Token: 0x02002A7E RID: 10878
			public class CRASHPLAN
			{
				// Token: 0x0400B49A RID: 46234
				public static LocString NAME = UI.FormatAsLink("Crash Plan", "CRASHPLAN");

				// Token: 0x0400B49B RID: 46235
				public static LocString DESC = "What goes up, must come down.";
			}

			// Token: 0x02002A7F RID: 10879
			public class DURABLELIFESUPPORT
			{
				// Token: 0x0400B49C RID: 46236
				public static LocString NAME = UI.FormatAsLink("Durable Life Support", "DURABLELIFESUPPORT");

				// Token: 0x0400B49D RID: 46237
				public static LocString DESC = "Improved devices for extended missions into space.";
			}

			// Token: 0x02002A80 RID: 10880
			public class ARTIFICIALFRIENDS
			{
				// Token: 0x0400B49E RID: 46238
				public static LocString NAME = UI.FormatAsLink("Artificial Friends", "ARTIFICIALFRIENDS");

				// Token: 0x0400B49F RID: 46239
				public static LocString DESC = "Sweeping advances in companion technology.";
			}

			// Token: 0x02002A81 RID: 10881
			public class ROBOTICTOOLS
			{
				// Token: 0x0400B4A0 RID: 46240
				public static LocString NAME = UI.FormatAsLink("Robotic Tools", "ROBOTICTOOLS");

				// Token: 0x0400B4A1 RID: 46241
				public static LocString DESC = "The goal of every great civilization is to one day make itself obsolete.";
			}

			// Token: 0x02002A82 RID: 10882
			public class LOGICCONTROL
			{
				// Token: 0x0400B4A2 RID: 46242
				public static LocString NAME = UI.FormatAsLink("Smart Home", "LOGICCONTROL");

				// Token: 0x0400B4A3 RID: 46243
				public static LocString DESC = "Switches that grant full control of building operations within the colony.";
			}

			// Token: 0x02002A83 RID: 10883
			public class LOGICCIRCUITS
			{
				// Token: 0x0400B4A4 RID: 46244
				public static LocString NAME = UI.FormatAsLink("Advanced Automation", "LOGICCIRCUITS");

				// Token: 0x0400B4A5 RID: 46245
				public static LocString DESC = "The only limit to colony automation is my own imagination.";
			}

			// Token: 0x02002A84 RID: 10884
			public class PARALLELAUTOMATION
			{
				// Token: 0x0400B4A6 RID: 46246
				public static LocString NAME = UI.FormatAsLink("Parallel Automation", "PARALLELAUTOMATION");

				// Token: 0x0400B4A7 RID: 46247
				public static LocString DESC = "Multi-wire automation at a fraction of the space.";
			}

			// Token: 0x02002A85 RID: 10885
			public class MULTIPLEXING
			{
				// Token: 0x0400B4A8 RID: 46248
				public static LocString NAME = UI.FormatAsLink("Multiplexing", "MULTIPLEXING");

				// Token: 0x0400B4A9 RID: 46249
				public static LocString DESC = "More choices for Automation signal distribution.";
			}

			// Token: 0x02002A86 RID: 10886
			public class VALVEMINIATURIZATION
			{
				// Token: 0x0400B4AA RID: 46250
				public static LocString NAME = UI.FormatAsLink("Valve Miniaturization", "VALVEMINIATURIZATION");

				// Token: 0x0400B4AB RID: 46251
				public static LocString DESC = "Smaller, more efficient pumps for those low-throughput situations.";
			}

			// Token: 0x02002A87 RID: 10887
			public class HYDROCARBONPROPULSION
			{
				// Token: 0x0400B4AC RID: 46252
				public static LocString NAME = UI.FormatAsLink("Hydrocarbon Propulsion", "HYDROCARBONPROPULSION");

				// Token: 0x0400B4AD RID: 46253
				public static LocString DESC = "Low-range rocket engines with lots of smoke.";
			}

			// Token: 0x02002A88 RID: 10888
			public class BETTERHYDROCARBONPROPULSION
			{
				// Token: 0x0400B4AE RID: 46254
				public static LocString NAME = UI.FormatAsLink("Improved Hydrocarbon Propulsion", "BETTERHYDROCARBONPROPULSION");

				// Token: 0x0400B4AF RID: 46255
				public static LocString DESC = "Mid-range rocket engines with lots of smoke.";
			}

			// Token: 0x02002A89 RID: 10889
			public class PRETTYGOODCONDUCTORS
			{
				// Token: 0x0400B4B0 RID: 46256
				public static LocString NAME = UI.FormatAsLink("Low-Resistance Conductors", "PRETTYGOODCONDUCTORS");

				// Token: 0x0400B4B1 RID: 46257
				public static LocString DESC = "Pure-core wires that can handle more " + UI.FormatAsLink("Electrical", "POWER") + " current without overloading.";
			}

			// Token: 0x02002A8A RID: 10890
			public class RENEWABLEENERGY
			{
				// Token: 0x0400B4B2 RID: 46258
				public static LocString NAME = UI.FormatAsLink("Renewable Energy", "RENEWABLEENERGY");

				// Token: 0x0400B4B3 RID: 46259
				public static LocString DESC = "Clean, sustainable " + UI.FormatAsLink("Power", "POWER") + " production that produces little to no waste.";
			}

			// Token: 0x02002A8B RID: 10891
			public class BASICREFINEMENT
			{
				// Token: 0x0400B4B4 RID: 46260
				public static LocString NAME = UI.FormatAsLink("Brute-Force Refinement", "BASICREFINEMENT");

				// Token: 0x0400B4B5 RID: 46261
				public static LocString DESC = "Low-tech refinement methods for producing clay and renewable sources of sand.";
			}

			// Token: 0x02002A8C RID: 10892
			public class REFINEDOBJECTS
			{
				// Token: 0x0400B4B6 RID: 46262
				public static LocString NAME = UI.FormatAsLink("Refined Renovations", "REFINEDOBJECTS");

				// Token: 0x0400B4B7 RID: 46263
				public static LocString DESC = "Improve base infrastructure with new objects crafted from " + UI.FormatAsLink("Refined Metals", "REFINEDMETAL") + ".";
			}

			// Token: 0x02002A8D RID: 10893
			public class GENERICSENSORS
			{
				// Token: 0x0400B4B8 RID: 46264
				public static LocString NAME = UI.FormatAsLink("Generic Sensors", "GENERICSENSORS");

				// Token: 0x0400B4B9 RID: 46265
				public static LocString DESC = "Drive automation in a variety of new, inventive ways.";
			}

			// Token: 0x02002A8E RID: 10894
			public class DUPETRAFFICCONTROL
			{
				// Token: 0x0400B4BA RID: 46266
				public static LocString NAME = UI.FormatAsLink("Computing", "DUPETRAFFICCONTROL");

				// Token: 0x0400B4BB RID: 46267
				public static LocString DESC = "Virtually extend the boundaries of Duplicant imagination.";
			}

			// Token: 0x02002A8F RID: 10895
			public class ADVANCEDSCANNERS
			{
				// Token: 0x0400B4BC RID: 46268
				public static LocString NAME = UI.FormatAsLink("Sensitive Microimaging", "ADVANCEDSCANNERS");

				// Token: 0x0400B4BD RID: 46269
				public static LocString DESC = "Computerized systems do the looking, so Duplicants don't have to.";
			}

			// Token: 0x02002A90 RID: 10896
			public class SMELTING
			{
				// Token: 0x0400B4BE RID: 46270
				public static LocString NAME = UI.FormatAsLink("Smelting", "SMELTING");

				// Token: 0x0400B4BF RID: 46271
				public static LocString DESC = "High temperatures facilitate the production of purer, special use metal resources.";
			}

			// Token: 0x02002A91 RID: 10897
			public class TRAVELTUBES
			{
				// Token: 0x0400B4C0 RID: 46272
				public static LocString NAME = UI.FormatAsLink("Transit Tubes", "TRAVELTUBES");

				// Token: 0x0400B4C1 RID: 46273
				public static LocString DESC = "A wholly futuristic way to move Duplicants around the base.";
			}

			// Token: 0x02002A92 RID: 10898
			public class SMARTSTORAGE
			{
				// Token: 0x0400B4C2 RID: 46274
				public static LocString NAME = UI.FormatAsLink("Smart Storage", "SMARTSTORAGE");

				// Token: 0x0400B4C3 RID: 46275
				public static LocString DESC = "Completely automate the storage of solid resources.";
			}

			// Token: 0x02002A93 RID: 10899
			public class SOLIDTRANSPORT
			{
				// Token: 0x0400B4C4 RID: 46276
				public static LocString NAME = UI.FormatAsLink("Solid Transport", "SOLIDTRANSPORT");

				// Token: 0x0400B4C5 RID: 46277
				public static LocString DESC = "Free Duplicants from the drudgery of day-to-day material deliveries with new methods of automation.";
			}

			// Token: 0x02002A94 RID: 10900
			public class SOLIDMANAGEMENT
			{
				// Token: 0x0400B4C6 RID: 46278
				public static LocString NAME = UI.FormatAsLink("Solid Management", "SOLIDMANAGEMENT");

				// Token: 0x0400B4C7 RID: 46279
				public static LocString DESC = "Make solid decisions in " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " sorting.";
			}

			// Token: 0x02002A95 RID: 10901
			public class SOLIDDISTRIBUTION
			{
				// Token: 0x0400B4C8 RID: 46280
				public static LocString NAME = UI.FormatAsLink("Solid Distribution", "SOLIDDISTRIBUTION");

				// Token: 0x0400B4C9 RID: 46281
				public static LocString DESC = "Internal rocket hookups for " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources.";
			}

			// Token: 0x02002A96 RID: 10902
			public class HIGHTEMPFORGING
			{
				// Token: 0x0400B4CA RID: 46282
				public static LocString NAME = UI.FormatAsLink("Superheated Forging", "HIGHTEMPFORGING");

				// Token: 0x0400B4CB RID: 46283
				public static LocString DESC = "Craft entirely new materials by harnessing the most extreme temperatures.";
			}

			// Token: 0x02002A97 RID: 10903
			public class HIGHPRESSUREFORGING
			{
				// Token: 0x0400B4CC RID: 46284
				public static LocString NAME = UI.FormatAsLink("Pressurized Forging", "HIGHPRESSUREFORGING");

				// Token: 0x0400B4CD RID: 46285
				public static LocString DESC = "High pressure diamond forging.";
			}

			// Token: 0x02002A98 RID: 10904
			public class RADIATIONPROTECTION
			{
				// Token: 0x0400B4CE RID: 46286
				public static LocString NAME = UI.FormatAsLink("Radiation Protection", "RADIATIONPROTECTION");

				// Token: 0x0400B4CF RID: 46287
				public static LocString DESC = "Shield Duplicants from dangerous amounts of radiation.";
			}

			// Token: 0x02002A99 RID: 10905
			public class SKYDETECTORS
			{
				// Token: 0x0400B4D0 RID: 46288
				public static LocString NAME = UI.FormatAsLink("Celestial Detection", "SKYDETECTORS");

				// Token: 0x0400B4D1 RID: 46289
				public static LocString DESC = "Turn Duplicants' eyes to the skies and discover what undiscovered wonders await out there.";
			}

			// Token: 0x02002A9A RID: 10906
			public class JETPACKS
			{
				// Token: 0x0400B4D2 RID: 46290
				public static LocString NAME = UI.FormatAsLink("Jetpacks", "JETPACKS");

				// Token: 0x0400B4D3 RID: 46291
				public static LocString DESC = "Objectively the most stylish way for Duplicants to get around.";
			}

			// Token: 0x02002A9B RID: 10907
			public class BASICROCKETRY
			{
				// Token: 0x0400B4D4 RID: 46292
				public static LocString NAME = UI.FormatAsLink("Introductory Rocketry", "BASICROCKETRY");

				// Token: 0x0400B4D5 RID: 46293
				public static LocString DESC = "Everything required for launching the colony's very first space program.";
			}

			// Token: 0x02002A9C RID: 10908
			public class ENGINESI
			{
				// Token: 0x0400B4D6 RID: 46294
				public static LocString NAME = UI.FormatAsLink("Solid Fuel Combustion", "ENGINESI");

				// Token: 0x0400B4D7 RID: 46295
				public static LocString DESC = "Rockets that fly further, longer.";
			}

			// Token: 0x02002A9D RID: 10909
			public class ENGINESII
			{
				// Token: 0x0400B4D8 RID: 46296
				public static LocString NAME = UI.FormatAsLink("Hydrocarbon Combustion", "ENGINESII");

				// Token: 0x0400B4D9 RID: 46297
				public static LocString DESC = "Delve deeper into the vastness of space than ever before.";
			}

			// Token: 0x02002A9E RID: 10910
			public class ENGINESIII
			{
				// Token: 0x0400B4DA RID: 46298
				public static LocString NAME = UI.FormatAsLink("Cryofuel Combustion", "ENGINESIII");

				// Token: 0x0400B4DB RID: 46299
				public static LocString DESC = "With this technology, the sky is your oyster. Go exploring!";
			}

			// Token: 0x02002A9F RID: 10911
			public class CRYOFUELPROPULSION
			{
				// Token: 0x0400B4DC RID: 46300
				public static LocString NAME = UI.FormatAsLink("Cryofuel Propulsion", "CRYOFUELPROPULSION");

				// Token: 0x0400B4DD RID: 46301
				public static LocString DESC = "A semi-powerful engine to propel you further into the galaxy.";
			}

			// Token: 0x02002AA0 RID: 10912
			public class NUCLEARPROPULSION
			{
				// Token: 0x0400B4DE RID: 46302
				public static LocString NAME = UI.FormatAsLink("Radbolt Propulsion", "NUCLEARPROPULSION");

				// Token: 0x0400B4DF RID: 46303
				public static LocString DESC = "Radical technology to get you to the stars.";
			}

			// Token: 0x02002AA1 RID: 10913
			public class ADVANCEDRESOURCEEXTRACTION
			{
				// Token: 0x0400B4E0 RID: 46304
				public static LocString NAME = UI.FormatAsLink("Advanced Resource Extraction", "ADVANCEDRESOURCEEXTRACTION");

				// Token: 0x0400B4E1 RID: 46305
				public static LocString DESC = "Bring back souvieners from the stars.";
			}

			// Token: 0x02002AA2 RID: 10914
			public class CARGOI
			{
				// Token: 0x0400B4E2 RID: 46306
				public static LocString NAME = UI.FormatAsLink("Solid Cargo", "CARGOI");

				// Token: 0x0400B4E3 RID: 46307
				public static LocString DESC = "Make extra use of journeys into space by mining and storing useful resources.";
			}

			// Token: 0x02002AA3 RID: 10915
			public class CARGOII
			{
				// Token: 0x0400B4E4 RID: 46308
				public static LocString NAME = UI.FormatAsLink("Liquid and Gas Cargo", "CARGOII");

				// Token: 0x0400B4E5 RID: 46309
				public static LocString DESC = "Extract precious liquids and gases from the far reaches of space, and return with them to the colony.";
			}

			// Token: 0x02002AA4 RID: 10916
			public class CARGOIII
			{
				// Token: 0x0400B4E6 RID: 46310
				public static LocString NAME = UI.FormatAsLink("Unique Cargo", "CARGOIII");

				// Token: 0x0400B4E7 RID: 46311
				public static LocString DESC = "Allow Duplicants to take their friends to see the stars... or simply bring souvenirs back from their travels.";
			}

			// Token: 0x02002AA5 RID: 10917
			public class NOTIFICATIONSYSTEMS
			{
				// Token: 0x0400B4E8 RID: 46312
				public static LocString NAME = UI.FormatAsLink("Notification Systems", "NOTIFICATIONSYSTEMS");

				// Token: 0x0400B4E9 RID: 46313
				public static LocString DESC = "Get all the news you need to know about your complex colony.";
			}

			// Token: 0x02002AA6 RID: 10918
			public class NUCLEARREFINEMENT
			{
				// Token: 0x0400B4EA RID: 46314
				public static LocString NAME = UI.FormatAsLink("Radiation Refinement", "NUCLEAR");

				// Token: 0x0400B4EB RID: 46315
				public static LocString DESC = "Refine uranium and generate radiation.";
			}

			// Token: 0x02002AA7 RID: 10919
			public class NUCLEARRESEARCH
			{
				// Token: 0x0400B4EC RID: 46316
				public static LocString NAME = UI.FormatAsLink("Materials Science Research", "ATOMIC");

				// Token: 0x0400B4ED RID: 46317
				public static LocString DESC = "Harness sub-atomic particles to study the properties of matter.";
			}

			// Token: 0x02002AA8 RID: 10920
			public class ADVANCEDNUCLEARRESEARCH
			{
				// Token: 0x0400B4EE RID: 46318
				public static LocString NAME = UI.FormatAsLink("More Materials Science Research", "ATOMIC");

				// Token: 0x0400B4EF RID: 46319
				public static LocString DESC = "Harness sub-atomic particles to study the properties of matter even more.";
			}

			// Token: 0x02002AA9 RID: 10921
			public class NUCLEARSTORAGE
			{
				// Token: 0x0400B4F0 RID: 46320
				public static LocString NAME = UI.FormatAsLink("Radbolt Containment", "ATOMIC");

				// Token: 0x0400B4F1 RID: 46321
				public static LocString DESC = "Build a quality cache of radbolts.";
			}

			// Token: 0x02002AAA RID: 10922
			public class SOLIDSPACE
			{
				// Token: 0x0400B4F2 RID: 46322
				public static LocString NAME = UI.FormatAsLink("Solid Control", "SOLIDSPACE");

				// Token: 0x0400B4F3 RID: 46323
				public static LocString DESC = "Transport and sort " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources.";
			}

			// Token: 0x02002AAB RID: 10923
			public class HIGHVELOCITYTRANSPORT
			{
				// Token: 0x0400B4F4 RID: 46324
				public static LocString NAME = UI.FormatAsLink("High Velocity Transport", "HIGHVELOCITY");

				// Token: 0x0400B4F5 RID: 46325
				public static LocString DESC = "Hurl things through space.";
			}

			// Token: 0x02002AAC RID: 10924
			public class MONUMENTS
			{
				// Token: 0x0400B4F6 RID: 46326
				public static LocString NAME = UI.FormatAsLink("Monuments", "MONUMENTS");

				// Token: 0x0400B4F7 RID: 46327
				public static LocString DESC = "Monumental art projects.";
			}

			// Token: 0x02002AAD RID: 10925
			public class BIOENGINEERING
			{
				// Token: 0x0400B4F8 RID: 46328
				public static LocString NAME = UI.FormatAsLink("Bioengineering", "BIOENGINEERING");

				// Token: 0x0400B4F9 RID: 46329
				public static LocString DESC = "Mutation station.";
			}

			// Token: 0x02002AAE RID: 10926
			public class SPACECOMBUSTION
			{
				// Token: 0x0400B4FA RID: 46330
				public static LocString NAME = UI.FormatAsLink("Advanced Combustion", "SPACECOMBUSTION");

				// Token: 0x0400B4FB RID: 46331
				public static LocString DESC = "Sweet advancements in rocket engines.";
			}

			// Token: 0x02002AAF RID: 10927
			public class HIGHVELOCITYDESTRUCTION
			{
				// Token: 0x0400B4FC RID: 46332
				public static LocString NAME = UI.FormatAsLink("High Velocity Destruction", "HIGHVELOCITYDESTRUCTION");

				// Token: 0x0400B4FD RID: 46333
				public static LocString DESC = "Mine the skies.";
			}

			// Token: 0x02002AB0 RID: 10928
			public class SPACEGAS
			{
				// Token: 0x0400B4FE RID: 46334
				public static LocString NAME = UI.FormatAsLink("Advanced Gas Flow", "SPACEGAS");

				// Token: 0x0400B4FF RID: 46335
				public static LocString DESC = UI.FormatAsLink("Gas", "ELEMENTS_GASSES") + " engines and transportation for rockets.";
			}
		}
	}
}
