using System;

namespace STRINGS
{
	// Token: 0x02000DAE RID: 3502
	public class COLONY_ACHIEVEMENTS
	{
		// Token: 0x0400511E RID: 20766
		public static LocString ACHIEVED_THIS_COLONY_TOOLTIP = "The current colony fulfilled this Initiative";

		// Token: 0x0400511F RID: 20767
		public static LocString NOT_ACHIEVED_THIS_COLONY = "The current colony hasn't fulfilled this Initiative";

		// Token: 0x04005120 RID: 20768
		public static LocString FAILED_THIS_COLONY = "The current colony cannot fulfill this Initiative";

		// Token: 0x04005121 RID: 20769
		public static LocString ACHIEVED_OTHER_COLONY_TOOLTIP = "This Initiative was fulfilled by a past colony";

		// Token: 0x04005122 RID: 20770
		public static LocString NOT_ACHIEVED_EVER = "This Initiative's never been fulfilled";

		// Token: 0x04005123 RID: 20771
		public static LocString PRE_VICTORY_MESSAGE_HEADER = "- ALERT -";

		// Token: 0x04005124 RID: 20772
		public static LocString PRE_VICTORY_MESSAGE_BODY = "IMPERATIVE ACHIEVED: {0}";

		// Token: 0x02001DE1 RID: 7649
		public static class DLC
		{
			// Token: 0x04008925 RID: 35109
			public static LocString EXPANSION1 = string.Concat(new string[]
			{
				UI.PRE_KEYWORD,
				"\n\n<i>",
				UI.DLC1.NAME,
				"</i>",
				UI.PST_KEYWORD,
				" DLC Achievement"
			});
		}

		// Token: 0x02001DE2 RID: 7650
		public class MISC_REQUIREMENTS
		{
			// Token: 0x04008926 RID: 35110
			public static LocString WINCONDITION_LEAVE = "The Great Escape";

			// Token: 0x04008927 RID: 35111
			public static LocString WINCONDITION_LEAVE_DESCRIPTION = "Ensure your colony's legacy by fulfilling the requirements of the Escape Imperative.";

			// Token: 0x04008928 RID: 35112
			public static LocString WINCONDITION_STAY = "Home Sweet Home";

			// Token: 0x04008929 RID: 35113
			public static LocString WINCONDITION_STAY_DESCRIPTION = "Establish your permanent home by fulfilling the requirements of the Colonize Imperative.";

			// Token: 0x0400892A RID: 35114
			public static LocString WINCONDITION_ARTIFACTS = "Cosmic Archaeology";

			// Token: 0x0400892B RID: 35115
			public static LocString WINCONDITION_ARTIFACTS_DESCRIPTION = "Uncover the past to secure your future by fullfilling the requirements of the Exploration Imperative.";

			// Token: 0x0400892C RID: 35116
			public static LocString NO_PLANTERBOX = "Locavore";

			// Token: 0x0400892D RID: 35117
			public static LocString NO_PLANTERBOX_DESCRIPTION = "Have Duplicants consume 400,000kcal of food without planting any seeds in Planter Boxes, Farm Tiles, or Hydroponic Farms.";

			// Token: 0x0400892E RID: 35118
			public static LocString EAT_MEAT = "Carnivore";

			// Token: 0x0400892F RID: 35119
			public static LocString EAT_MEAT_DESCRIPTION = "Have Duplicants eat 400,000kcal of critter meat before the 100th cycle.";

			// Token: 0x04008930 RID: 35120
			public static LocString BUILD_NATURE_RESERVES = "Some Reservations";

			// Token: 0x04008931 RID: 35121
			public static LocString BUILD_NATURE_RESERVES_DESCRIPTION = "Improve Duplicant Morale by designating {1} areas as {0}.";

			// Token: 0x04008932 RID: 35122
			public static LocString TWENTY_DUPES = "No Place Like Clone";

			// Token: 0x04008933 RID: 35123
			public static LocString TWENTY_DUPES_DESCRIPTION = "Have at least 20 living Duplicants living in the colony at one time.";

			// Token: 0x04008934 RID: 35124
			public static LocString SURVIVE_HUNDRED_CYCLES = "Turn of the Century";

			// Token: 0x04008935 RID: 35125
			public static LocString SURVIVE_HUNDRED_CYCLES_DESCRIPTION = "Reach cycle 100 with at least one living Duplicant.";

			// Token: 0x04008936 RID: 35126
			public static LocString TAME_GASSYMOO = "Moovin' On Up";

			// Token: 0x04008937 RID: 35127
			public static LocString TAME_GASSYMOO_DESCRIPTION = "Find and tame a Gassy Moo.";

			// Token: 0x04008938 RID: 35128
			public static LocString SIXKELVIN_BUILDING = "Not 0K, But Pretty Cool";

			// Token: 0x04008939 RID: 35129
			public static LocString SIXKELVIN_BUILDING_DESCRIPTION = "Reduce the temperature of a building to 6 Kelvin.";

			// Token: 0x0400893A RID: 35130
			public static LocString CLEAN_ENERGY = "Super Sustainable";

			// Token: 0x0400893B RID: 35131
			public static LocString CLEAN_ENERGY_DESCRIPTION = "Generate 240,000kJ of power without using coal, natural gas, petrol or wood generators.";

			// Token: 0x0400893C RID: 35132
			public static LocString BUILD_OUTSIDE_BIOME = "Outdoor Renovations";

			// Token: 0x0400893D RID: 35133
			public static LocString BUILD_OUTSIDE_BIOME_DESCRIPTION = "Construct a building outside the initial starting biome.";

			// Token: 0x0400893E RID: 35134
			public static LocString TUBE_TRAVEL_DISTANCE = "Totally Tubular";

			// Token: 0x0400893F RID: 35135
			public static LocString TUBE_TRAVEL_DISTANCE_DESCRIPTION = "Have Duplicants travel 10,000m by Transit Tube.";

			// Token: 0x04008940 RID: 35136
			public static LocString REACH_SPACE_ANY_DESTINATION = "Space Race";

			// Token: 0x04008941 RID: 35137
			public static LocString REACH_SPACE_ANY_DESTINATION_DESCRIPTION = "Launch your first rocket into space.";

			// Token: 0x04008942 RID: 35138
			public static LocString EQUIP_N_DUPES = "And Nowhere to Go";

			// Token: 0x04008943 RID: 35139
			public static LocString EQUIP_N_DUPES_DESCRIPTION = "Have {0} Duplicants wear non-default clothing simultaneously.";

			// Token: 0x04008944 RID: 35140
			public static LocString EXOSUIT_CYCLES = "Job Suitability";

			// Token: 0x04008945 RID: 35141
			public static LocString EXOSUIT_CYCLES_DESCRIPTION = "For {0} cycles in a row, have every Duplicant in the colony complete at least one chore while wearing an Exosuit.";

			// Token: 0x04008946 RID: 35142
			public static LocString HATCH_REFINEMENT = "Down the Hatch";

			// Token: 0x04008947 RID: 35143
			public static LocString HATCH_REFINEMENT_DESCRIPTION = "Produce {0} of refined metal by ranching Smooth Hatches.";

			// Token: 0x04008948 RID: 35144
			public static LocString VARIETY_OF_ROOMS = "Get a Room";

			// Token: 0x04008949 RID: 35145
			public static LocString VARIETY_OF_ROOMS_DESCRIPTION = "Build at least one of each of the following rooms in a single colony: A Nature Reserve, a Hospital, a Recreation Room, a Great Hall, a Bedroom, a Washroom, a Greenhouse and a Stable.";

			// Token: 0x0400894A RID: 35146
			public static LocString CURED_DISEASE = "They Got Better";

			// Token: 0x0400894B RID: 35147
			public static LocString CURED_DISEASE_DESCRIPTION = "Cure a sick Duplicant of disease.";

			// Token: 0x0400894C RID: 35148
			public static LocString SURVIVE_ONE_YEAR = "One Year, to be Exact";

			// Token: 0x0400894D RID: 35149
			public static LocString SURVIVE_ONE_YEAR_DESCRIPTION = "Reach cycle 365.25 with a single colony.";

			// Token: 0x0400894E RID: 35150
			public static LocString INSPECT_POI = "Ghosts of Gravitas";

			// Token: 0x0400894F RID: 35151
			public static LocString INSPECT_POI_DESCRIPTION = "Recover a Database entry by inspecting facility ruins.";

			// Token: 0x04008950 RID: 35152
			public static LocString CLEAR_FOW = "Pulling Back The Veil";

			// Token: 0x04008951 RID: 35153
			public static LocString CLEAR_FOW_DESCRIPTION = "Reveal 80% of map by exploring outside the starting biome.";

			// Token: 0x04008952 RID: 35154
			public static LocString EXPLORE_OIL_BIOME = "Slick";

			// Token: 0x04008953 RID: 35155
			public static LocString EXPLORE_OIL_BIOME_DESCRIPTION = "Enter an oil biome for the first time.";

			// Token: 0x04008954 RID: 35156
			public static LocString TAME_BASIC_CRITTERS = "Critter Whisperer";

			// Token: 0x04008955 RID: 35157
			public static LocString TAME_BASIC_CRITTERS_DESCRIPTION = "Find and tame one of every critter species in the world. Default morphs only.";

			// Token: 0x04008956 RID: 35158
			public static LocString HATCH_A_CRITTER = "Good Egg";

			// Token: 0x04008957 RID: 35159
			public static LocString HATCH_A_CRITTER_DESCRIPTION = "Hatch a new critter morph from an egg.";

			// Token: 0x04008958 RID: 35160
			public static LocString BUNKER_DOOR_DEFENSE = "Immovable Object";

			// Token: 0x04008959 RID: 35161
			public static LocString BUNKER_DOOR_DEFENSE_DESCRIPTION = "Block a meteor from hitting your base using a Bunker Door.";

			// Token: 0x0400895A RID: 35162
			public static LocString AUTOMATE_A_BUILDING = "Red Light, Green Light";

			// Token: 0x0400895B RID: 35163
			public static LocString AUTOMATE_A_BUILDING_DESCRIPTION = "Automate a building using sensors or switches from the Automation tab in the Build Menu.";

			// Token: 0x0400895C RID: 35164
			public static LocString COMPLETED_SKILL_BRANCH = "To Pay the Bills";

			// Token: 0x0400895D RID: 35165
			public static LocString COMPLETED_SKILL_BRANCH_DESCRIPTION = "Use a Duplicant's Skill Points to buy out an entire branch of the Skill Tree.";

			// Token: 0x0400895E RID: 35166
			public static LocString GENERATOR_TUNEUP = "Finely Tuned Machine";

			// Token: 0x0400895F RID: 35167
			public static LocString GENERATOR_TUNEUP_DESCRIPTION = "Perform {0} Tune Ups on power generators.";

			// Token: 0x04008960 RID: 35168
			public static LocString COMPLETED_RESEARCH = "Honorary Doctorate";

			// Token: 0x04008961 RID: 35169
			public static LocString COMPLETED_RESEARCH_DESCRIPTION = "Unlock every item in the Research Tree.";

			// Token: 0x04008962 RID: 35170
			public static LocString IDLE_DUPLICANTS = "Easy Livin'";

			// Token: 0x04008963 RID: 35171
			public static LocString IDLE_DUPLICANTS_DESCRIPTION = "Have Auto-Sweepers complete more deliveries to machines than Duplicants over 5 cycles.";

			// Token: 0x04008964 RID: 35172
			public static LocString COOKED_FOOD = "It's Not Raw";

			// Token: 0x04008965 RID: 35173
			public static LocString COOKED_FOOD_DESCRIPTION = "Have a Duplicant eat any cooked meal prepared at an Electric Grill or Gas Range.";

			// Token: 0x04008966 RID: 35174
			public static LocString PLUMBED_WASHROOMS = "Royal Flush";

			// Token: 0x04008967 RID: 35175
			public static LocString PLUMBED_WASHROOMS_DESCRIPTION = "Replace all the Outhouses and Wash Basins in your colony with Lavatories and Sinks.";

			// Token: 0x04008968 RID: 35176
			public static LocString BASIC_COMFORTS = "Bed and Bath";

			// Token: 0x04008969 RID: 35177
			public static LocString BASIC_COMFORTS_DESCRIPTION = "Have at least one toilet in the colony and a bed for every Duplicant.";

			// Token: 0x0400896A RID: 35178
			public static LocString BASIC_PUMPING = "Oxygen Not Occluded";

			// Token: 0x0400896B RID: 35179
			public static LocString BASIC_PUMPING_DESCRIPTION = "Distribute 1000" + UI.UNITSUFFIXES.MASS.KILOGRAM + " of Oxygen using gas vents.";

			// Token: 0x0400896C RID: 35180
			public static LocString MASTERPIECE_PAINTING = "Art Underground";

			// Token: 0x0400896D RID: 35181
			public static LocString MASTERPIECE_PAINTING_DESCRIPTION = "Have a Duplicant with the Masterworks skill paint a Masterpiece quality painting.";

			// Token: 0x0400896E RID: 35182
			public static LocString FIRST_TELEPORT = "First Teleport of Call";

			// Token: 0x0400896F RID: 35183
			public static LocString FIRST_TELEPORT_DESCRIPTION = "Teleport a Duplicant and defrost a Friend on another world.";

			// Token: 0x04008970 RID: 35184
			public static LocString SOFT_LAUNCH = "Soft Launch";

			// Token: 0x04008971 RID: 35185
			public static LocString SOFT_LAUNCH_DESCRIPTION = "Build a launchpad on a world without a teleporter.";

			// Token: 0x04008972 RID: 35186
			public static LocString LAND_ON_ALL_WORLDS = "Cluster Conquest";

			// Token: 0x04008973 RID: 35187
			public static LocString LAND_ON_ALL_WORLDS_DESCRIPTION = "Land dupes or rovers on all worlds in the cluster.";

			// Token: 0x04008974 RID: 35188
			public static LocString REACTOR_USAGE = "That's Rad!";

			// Token: 0x04008975 RID: 35189
			public static LocString REACTOR_USAGE_DESCRIPTION = "Run a Research Reactor at full capacity for {0} cycles.";

			// Token: 0x04008976 RID: 35190
			public static LocString GMO_OK = "GMO A-OK";

			// Token: 0x04008977 RID: 35191
			public static LocString GMO_OK_DESCRIPTION = "Successfully analyze at least one seed of all mutatable plants.";

			// Token: 0x04008978 RID: 35192
			public static LocString SWEETER_THAN_HONEY = "Sweeter Than Honey";

			// Token: 0x04008979 RID: 35193
			public static LocString SWEETER_THAN_HONEY_DESCRIPTION = "Extract Uranium from a Beeta hive without getting stung.";

			// Token: 0x0400897A RID: 35194
			public static LocString RADICAL_TRIP = "Radical Trip";

			// Token: 0x0400897B RID: 35195
			public static LocString RADICAL_TRIP_DESCRIPTION = "Have radbolts travel a cumulative {0}km.";

			// Token: 0x0400897C RID: 35196
			public static LocString MINE_THE_GAP = "Mine the Gap";

			// Token: 0x0400897D RID: 35197
			public static LocString MINE_THE_GAP_DESCRIPTION = "Mine 1,000,000kg from space POIs.";

			// Token: 0x0400897E RID: 35198
			public static LocString SURVIVE_IN_A_ROCKET = "Morale High Ground";

			// Token: 0x0400897F RID: 35199
			public static LocString SURVIVE_IN_A_ROCKET_DESCRIPTION = "Have the Duplicants in one rocket survive in space for {0} cycles in a row with a morale of {1} or higher.";

			// Token: 0x0200291E RID: 10526
			public class STATUS
			{
				// Token: 0x0400B142 RID: 45378
				public static LocString PLATFORM_UNLOCKING_DISABLED = "Platform achievements cannot be unlocked because a debug command was used in this colony. ";

				// Token: 0x0400B143 RID: 45379
				public static LocString PLATFORM_UNLOCKING_ENABLED = "Platform achievements can be unlocked.";

				// Token: 0x0400B144 RID: 45380
				public static LocString EXPAND_TOOLTIP = "<i>" + UI.CLICK(UI.ClickType.Click) + " to view progress</i>";

				// Token: 0x0400B145 RID: 45381
				public static LocString CYCLE_NUMBER = "Cycle: {0} / {1}";

				// Token: 0x0400B146 RID: 45382
				public static LocString REMAINING_CYCLES = "Cycles remaining: {0} / {1}";

				// Token: 0x0400B147 RID: 45383
				public static LocString FRACTIONAL_CYCLE = "Cycle: {0:0.##} / {1:0.##}";

				// Token: 0x0400B148 RID: 45384
				public static LocString LAUNCHED_ROCKET = "Launched a Rocket into Space";

				// Token: 0x0400B149 RID: 45385
				public static LocString LAUNCHED_ROCKET_TO_WORMHOLE = "Sent a Duplicant on a one-way mission to the furthest Starmap destination";

				// Token: 0x0400B14A RID: 45386
				public static LocString BUILT_A_ROOM = "Built a {0}.";

				// Token: 0x0400B14B RID: 45387
				public static LocString BUILT_N_ROOMS = "Built {0}: {1} / {2}";

				// Token: 0x0400B14C RID: 45388
				public static LocString CALORIE_SURPLUS = "Calorie surplus: {0} / {1}";

				// Token: 0x0400B14D RID: 45389
				public static LocString TECH_RESEARCHED = "Tech researched: {0} / {1}";

				// Token: 0x0400B14E RID: 45390
				public static LocString SKILL_BRANCH = "Unlocked an entire branch of the skill tree";

				// Token: 0x0400B14F RID: 45391
				public static LocString CLOTHE_DUPES = "Duplicants in clothing: {0} / {1}";

				// Token: 0x0400B150 RID: 45392
				public static LocString KELVIN_COOLING = "Coldest building: {0:##0.#}K";

				// Token: 0x0400B151 RID: 45393
				public static LocString NO_FARM_TILES = "No farmed plants";

				// Token: 0x0400B152 RID: 45394
				public static LocString CALORIES_FROM_MEAT = "Calories from meat: {0} / {1}";

				// Token: 0x0400B153 RID: 45395
				public static LocString CONSUME_CALORIES = "Calories: {0} / {1}";

				// Token: 0x0400B154 RID: 45396
				public static LocString CONSUME_ITEM = "Consume something prepared at {0}";

				// Token: 0x0400B155 RID: 45397
				public static LocString PREPARED_SEPARATOR = " or ";

				// Token: 0x0400B156 RID: 45398
				public static LocString BUILT_OUTSIDE_START = "Built outside the starting biome";

				// Token: 0x0400B157 RID: 45399
				public static LocString TRAVELED_IN_TUBES = "Distance: {0:n} m / {1:n} m";

				// Token: 0x0400B158 RID: 45400
				public static LocString ENTER_OIL_BIOME = "Entered an oil biome";

				// Token: 0x0400B159 RID: 45401
				public static LocString VENTED_MASS = "Vented: {0} / {1}";

				// Token: 0x0400B15A RID: 45402
				public static LocString BUILT_ONE_TOILET = "Built one toilet";

				// Token: 0x0400B15B RID: 45403
				public static LocString BUILING_BEDS = "Built beds: {0} ({1} Needed)";

				// Token: 0x0400B15C RID: 45404
				public static LocString BUILT_ONE_BED_PER_DUPLICANT = "Built one bed for each Duplicant";

				// Token: 0x0400B15D RID: 45405
				public static LocString UPGRADE_ALL_BUILDINGS = "All {0} upgraded to {1}";

				// Token: 0x0400B15E RID: 45406
				public static LocString AUTOMATE_A_BUILDING = "Automated a building";

				// Token: 0x0400B15F RID: 45407
				public static LocString CREATE_A_PAINTING = "Created a masterpiece painting";

				// Token: 0x0400B160 RID: 45408
				public static LocString INVESTIGATE_A_POI = "Inspected a ruin";

				// Token: 0x0400B161 RID: 45409
				public static LocString HATCH_A_MORPH = "Hatched a critter morph";

				// Token: 0x0400B162 RID: 45410
				public static LocString CURED_DISEASE = "Cured a disease";

				// Token: 0x0400B163 RID: 45411
				public static LocString CHORES_OF_TYPE = "{2} errands: {0} / {1}";

				// Token: 0x0400B164 RID: 45412
				public static LocString REVEALED = "Revealed: {0:0.##}% / {1:0.##}%";

				// Token: 0x0400B165 RID: 45413
				public static LocString POOP_PRODUCTION = "Poop production: {0} / {1}";

				// Token: 0x0400B166 RID: 45414
				public static LocString BLOCKED_A_COMET = "Blocked a meteor with a Bunker Door";

				// Token: 0x0400B167 RID: 45415
				public static LocString POPULATION = "Population: {0} / {1}";

				// Token: 0x0400B168 RID: 45416
				public static LocString TAME_A_CRITTER = "Tamed a {0}";

				// Token: 0x0400B169 RID: 45417
				public static LocString ARM_PERFORMANCE = "Auto-Sweepers outperformed dupes for cycles: {0} / {1}";

				// Token: 0x0400B16A RID: 45418
				public static LocString ARM_VS_DUPE_FETCHES = "Deliveries this cycle: Auto-Sweepers: {1} Duplicants: {2}";

				// Token: 0x0400B16B RID: 45419
				public static LocString EXOSUIT_CYCLES = "All Dupes completed an Exosuit errand for cycles: {0} / {1}";

				// Token: 0x0400B16C RID: 45420
				public static LocString EXOSUIT_THIS_CYCLE = "Dupes who completed Exosuit errands this cycle: {0} / {1}";

				// Token: 0x0400B16D RID: 45421
				public static LocString GENERATE_POWER = "Energy generated: {0} / {1}";

				// Token: 0x0400B16E RID: 45422
				public static LocString NO_BUILDING = "Never built a {0}";

				// Token: 0x0400B16F RID: 45423
				public static LocString MORALE = "{0} morale: {1}";

				// Token: 0x0400B170 RID: 45424
				public static LocString COLLECT_ARTIFACTS = "Study different Terrestrial Artifacts at the Artifact Analysis Station.\nUnique Terrestrial Artifacts studied: {collectedCount} / {neededCount}";

				// Token: 0x0400B171 RID: 45425
				public static LocString COLLECT_SPACE_ARTIFACTS = "Study different Space Artifacts at the Artifact Analysis Station.\nUnique Space Artifacts studied: {collectedCount} / {neededCount}";

				// Token: 0x0400B172 RID: 45426
				public static LocString ESTABLISH_COLONIES = "Establish colonies on {goalBaseCount} asteroids by building and activating Mini-Pods.\nColonies established: {baseCount} / {neededCount}.";

				// Token: 0x0400B173 RID: 45427
				public static LocString OPEN_TEMPORAL_TEAR = "Open the Temporal Tear by finding and activating the Temporal Tear Opener";

				// Token: 0x0400B174 RID: 45428
				public static LocString TELEPORT_DUPLICANT = "Teleport a Duplicant to another world";

				// Token: 0x0400B175 RID: 45429
				public static LocString DEFROST_DUPLICANT = "Defrost a Duplicant";

				// Token: 0x0400B176 RID: 45430
				public static LocString BUILD_A_LAUNCHPAD = "Build a launchpad on a new world without a teleporter";

				// Token: 0x0400B177 RID: 45431
				public static LocString LAND_DUPES_ON_ALL_WORLDS = "Duplicants or rovers landed on {0} of {1} planetoids";

				// Token: 0x0400B178 RID: 45432
				public static LocString RUN_A_REACTOR = "Reactor running for cycles: {0} / {1}";

				// Token: 0x0400B179 RID: 45433
				public static LocString ANALYZE_SEED = "Analyze {0} mutant";

				// Token: 0x0400B17A RID: 45434
				public static LocString GET_URANIUM_WITHOUT_STING = "Got uranium out of a Beeta hive without getting stung";

				// Token: 0x0400B17B RID: 45435
				public static LocString RADBOLT_TRAVEL = "Radbolts travelled: {0:n} m / {1:n} m";

				// Token: 0x0400B17C RID: 45436
				public static LocString MINE_SPACE_POI = "Mined: {0:n} / {1:n} kg";

				// Token: 0x0400B17D RID: 45437
				public static LocString SURVIVE_SPACE = "Duplicants in {3} have ended each cycle in space with at least {0} morale for: {1} / {2} cycles";

				// Token: 0x0400B17E RID: 45438
				public static LocString SURVIVE_SPACE_COMPLETE = "Duplicants survived in space with at least {0} morale for {1} cycles.";

				// Token: 0x0400B17F RID: 45439
				public static LocString HARVEST_HIVE = "Uranium extracted from a Beeta hive without getting stung";
			}
		}

		// Token: 0x02001DE3 RID: 7651
		public class THRIVING
		{
			// Token: 0x04008980 RID: 35200
			public static LocString NAME = "Home Sweet Home";

			// Token: 0x04008981 RID: 35201
			public static LocString MYLOGNAME = "This Is Our Home";

			// Token: 0x04008982 RID: 35202
			public static LocString DESCRIPTION = "";

			// Token: 0x04008983 RID: 35203
			public static LocString MESSAGE_TITLE = "THIS IS OUR HOME";

			// Token: 0x04008984 RID: 35204
			public static LocString MESSAGE_BODY = "Few civilizations throughout time have had the privilege of understanding their origins. The one thing that matters is that we are here now, and we make the best of the world we've been given. I am proud to say...\n\nThis asteroid is our home.";

			// Token: 0x0200291F RID: 10527
			public class VIDEO_TEXT
			{
				// Token: 0x0400B180 RID: 45440
				public static LocString FIRST = "Few civilizations throughout time have had the privilege of understanding their origins.";

				// Token: 0x0400B181 RID: 45441
				public static LocString SECOND = "The only thing that matters is that we are here now, and we make the best of the world we've been given. I am proud to say...";

				// Token: 0x0400B182 RID: 45442
				public static LocString THIRD = "This asteroid is our home.";
			}

			// Token: 0x02002920 RID: 10528
			public class REQUIREMENTS
			{
				// Token: 0x0400B183 RID: 45443
				public static LocString BUILT_MONUMENT = "Build a Great Monument";

				// Token: 0x0400B184 RID: 45444
				public static LocString BUILT_MONUMENT_DESCRIPTION = string.Concat(new string[]
				{
					"Build all three sections of a ",
					UI.PRE_KEYWORD,
					"Great Monument",
					UI.PST_KEYWORD,
					" to mark the colony as your home"
				});

				// Token: 0x0400B185 RID: 45445
				public static LocString MINIMUM_DUPLICANTS = "Print {0} Duplicants";

				// Token: 0x0400B186 RID: 45446
				public static LocString MINIMUM_DUPLICANTS_DESCRIPTION = "The colony must have <b>{0}</b> or more living Duplicants";

				// Token: 0x0400B187 RID: 45447
				public static LocString MINIMUM_MORALE = "Maintain {0} Morale";

				// Token: 0x0400B188 RID: 45448
				public static LocString MINIMUM_MORALE_DESCRIPTION = string.Concat(new string[]
				{
					"All Duplicants must have ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" of 16 or higher"
				});

				// Token: 0x0400B189 RID: 45449
				public static LocString MINIMUM_CYCLE = "Survive {0} Cycles";

				// Token: 0x0400B18A RID: 45450
				public static LocString MINIMUM_CYCLE_DESCRIPTION = "The colony must survive a minimum of <b>{0}</b> cycles";
			}
		}

		// Token: 0x02001DE4 RID: 7652
		public class DISTANT_PLANET_REACHED
		{
			// Token: 0x04008985 RID: 35205
			public static LocString NAME = "The Great Escape";

			// Token: 0x04008986 RID: 35206
			public static LocString MYLOGNAME = "A Colony's Hope";

			// Token: 0x04008987 RID: 35207
			public static LocString DESCRIPTION = "";

			// Token: 0x04008988 RID: 35208
			public static LocString MESSAGE_TITLE = "A COLONY'S HOPE";

			// Token: 0x04008989 RID: 35209
			public static LocString MESSAGE_BODY = "Our homeworld in this universe is gone, replaced by the skeleton of a planet and a wound in the sky... But I hold out hope that other worlds exist out there, tucked away in other dimensions. I sent my Duplicant through the Temporal Tear carrying that hope on their shoulders... Perhaps one day they'll find a place to call home, and begin a thriving colony all their own.";

			// Token: 0x0400898A RID: 35210
			public static LocString MESSAGE_TITLE_DLC1 = "A DIMENSIONAL ADVENTURE";

			// Token: 0x0400898B RID: 35211
			public static LocString MESSAGE_BODY_DLC1 = "We have always viewed the Temporal Tear as a phenomenon to fear but, like the civilizations before us, the call to adventure asks us to confront our anxiety and leap into the unknown. As a radical action of hope, I have sent enough Duplicants through the Temporal Tear to start another colony, explore dimensions beyond ours and plant the seeds of life throughout time and space.";

			// Token: 0x02002921 RID: 10529
			public class VIDEO_TEXT
			{
				// Token: 0x0400B18B RID: 45451
				public static LocString FIRST = "Our homeworld in this universe is gone, replaced by the skeleton of a planet and a wound in the sky... But I hold out hope that other worlds exist out there, tucked away in other dimensions.";

				// Token: 0x0400B18C RID: 45452
				public static LocString SECOND = "I sent my Duplicant through the Temporal Tear carrying that hope on their shoulders... Perhaps one day they'll find a place to call home, and begin a thriving colony all their own.";
			}

			// Token: 0x02002922 RID: 10530
			public class VIDEO_TEXT_DLC1
			{
				// Token: 0x0400B18D RID: 45453
				public static LocString FIRST = "DLC1";

				// Token: 0x0400B18E RID: 45454
				public static LocString SECOND = "DLC1";
			}

			// Token: 0x02002923 RID: 10531
			public class REQUIREMENTS
			{
				// Token: 0x0400B18F RID: 45455
				public static LocString REACHED_SPACE_DESTINATION = "Breach the {0}";

				// Token: 0x0400B190 RID: 45456
				public static LocString REACHED_SPACE_DESTINATION_DESCRIPTION = "Send a Duplicant on a one-way mission to the furthest Starmap destination";

				// Token: 0x0400B191 RID: 45457
				public static LocString OPEN_TEMPORAL_TEAR = "Open the Temporal Tear";
			}
		}

		// Token: 0x02001DE5 RID: 7653
		public class STUDY_ARTIFACTS
		{
			// Token: 0x0400898C RID: 35212
			public static LocString NAME = "Cosmic Archaeology";

			// Token: 0x0400898D RID: 35213
			public static LocString MYLOGNAME = "Artifacts";

			// Token: 0x0400898E RID: 35214
			public static LocString DESCRIPTION = "";

			// Token: 0x0400898F RID: 35215
			public static LocString MESSAGE_TITLE = "LINK TO OUR PAST";

			// Token: 0x04008990 RID: 35216
			public static LocString MESSAGE_BODY = "In exploring this corner of the universe we have found and assembled a collection of artifacts from another civilization. Studying these artifacts can give us a greater understanding of who we are and where we come from. Only by learning about the past can we build a brighter future, one where we learn from the mistakes of our predecessors.";

			// Token: 0x04008991 RID: 35217
			public static LocString MESSAGE_TITLE_DLC1 = "DLC1";

			// Token: 0x04008992 RID: 35218
			public static LocString MESSAGE_BODY_DLC1 = "DLC1";

			// Token: 0x02002924 RID: 10532
			public class VIDEO_TEXT
			{
				// Token: 0x0400B192 RID: 45458
				public static LocString FIRST = "Our homeworld in this universe is gone, replaced by the skeleton of a planet and a wound in the sky... But I hold out hope that other worlds exist out there, tucked away in other dimensions.";

				// Token: 0x0400B193 RID: 45459
				public static LocString SECOND = "I sent my Duplicant through the Temporal Tear carrying that hope on their shoulders... Perhaps one day they'll find a place to call home, and begin a thriving colony all their own.";
			}

			// Token: 0x02002925 RID: 10533
			public class VIDEO_TEXT_DLC1
			{
				// Token: 0x0400B194 RID: 45460
				public static LocString FIRST = "DLC1";

				// Token: 0x0400B195 RID: 45461
				public static LocString SECOND = "DLC1";
			}

			// Token: 0x02002926 RID: 10534
			public class REQUIREMENTS
			{
				// Token: 0x0400B196 RID: 45462
				public static LocString STUDY_ARTIFACTS = "Study {artifactCount} Terrestrial Artifacts";

				// Token: 0x0400B197 RID: 45463
				public static LocString STUDY_ARTIFACTS_DESCRIPTION = "Study {artifactCount} Terrestrial Artifacts at the Artifact Analysis Station";

				// Token: 0x0400B198 RID: 45464
				public static LocString STUDY_SPACE_ARTIFACTS = "Study {artifactCount} Space Artifacts";

				// Token: 0x0400B199 RID: 45465
				public static LocString STUDY_SPACE_ARTIFACTS_DESCRIPTION = "Study {artifactCount} Space Artifacts at the Artifact Analysis Station";

				// Token: 0x0400B19A RID: 45466
				public static LocString SEVERAL_COLONIES = "Establish several colonies";

				// Token: 0x0400B19B RID: 45467
				public static LocString SEVERAL_COLONIES_DESCRIPTION = "Establish colonies on {count} asteroids by building and activating Mini-Pods";
			}
		}
	}
}
