using System;
using System.Collections.Generic;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000D90 RID: 3472
	public class DUPLICANTSTATS
	{
		// Token: 0x06006C16 RID: 27670 RVA: 0x002A9584 File Offset: 0x002A7784
		public static DUPLICANTSTATS.TraitVal GetTraitVal(string id)
		{
			foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.SPECIALTRAITS)
			{
				if (id == traitVal.id)
				{
					return traitVal;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal2 in DUPLICANTSTATS.GOODTRAITS)
			{
				if (id == traitVal2.id)
				{
					return traitVal2;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal3 in DUPLICANTSTATS.BADTRAITS)
			{
				if (id == traitVal3.id)
				{
					return traitVal3;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal4 in DUPLICANTSTATS.CONGENITALTRAITS)
			{
				if (id == traitVal4.id)
				{
					return traitVal4;
				}
			}
			DebugUtil.Assert(true, "Could not find TraitVal with ID: " + id);
			return DUPLICANTSTATS.INVALID_TRAIT_VAL;
		}

		// Token: 0x04004F75 RID: 20341
		public const float DEFAULT_MASS = 30f;

		// Token: 0x04004F76 RID: 20342
		public const float PEE_FUSE_TIME = 120f;

		// Token: 0x04004F77 RID: 20343
		public const float PEE_PER_FLOOR_PEE = 2f;

		// Token: 0x04004F78 RID: 20344
		public const float PEE_PER_TOILET_PEE = 6.7f;

		// Token: 0x04004F79 RID: 20345
		public const string PEE_DISEASE = "FoodPoisoning";

		// Token: 0x04004F7A RID: 20346
		public const int DISEASE_PER_PEE = 100000;

		// Token: 0x04004F7B RID: 20347
		public const int DISEASE_PER_VOMIT = 100000;

		// Token: 0x04004F7C RID: 20348
		public const float KCAL2JOULES = 4184f;

		// Token: 0x04004F7D RID: 20349
		public const float COOLING_EFFICIENCY = 0.08f;

		// Token: 0x04004F7E RID: 20350
		public const float DUPLICANT_COOLING_KILOWATTS = 0.5578667f;

		// Token: 0x04004F7F RID: 20351
		public const float WARMING_EFFICIENCY = 0.08f;

		// Token: 0x04004F80 RID: 20352
		public const float DUPLICANT_WARMING_KILOWATTS = 0.5578667f;

		// Token: 0x04004F81 RID: 20353
		public const float HEAT_GENERATION_EFFICIENCY = 0.012f;

		// Token: 0x04004F82 RID: 20354
		public const float DUPLICANT_BASE_GENERATION_KILOWATTS = 0.08368001f;

		// Token: 0x04004F83 RID: 20355
		public const float STANDARD_STRESS_PENALTY = 0.016666668f;

		// Token: 0x04004F84 RID: 20356
		public const float STANDARD_STRESS_BONUS = -0.033333335f;

		// Token: 0x04004F85 RID: 20357
		public const float RANCHING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		// Token: 0x04004F86 RID: 20358
		public const float FARMING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		// Token: 0x04004F87 RID: 20359
		public const float POWER_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.025f;

		// Token: 0x04004F88 RID: 20360
		public const float RANCHING_CAPTURABLE_MULTIPLIER_BONUS_PER_POINT = 0.05f;

		// Token: 0x04004F89 RID: 20361
		public const float STRESS_BELOW_EXPECTATIONS_FOOD = 0.25f;

		// Token: 0x04004F8A RID: 20362
		public const float STRESS_ABOVE_EXPECTATIONS_FOOD = -0.5f;

		// Token: 0x04004F8B RID: 20363
		public const float STANDARD_STRESS_PENALTY_SECOND = 0.25f;

		// Token: 0x04004F8C RID: 20364
		public const float STANDARD_STRESS_BONUS_SECOND = -0.5f;

		// Token: 0x04004F8D RID: 20365
		public const float RECOVER_BREATH_DELTA = 3f;

		// Token: 0x04004F8E RID: 20366
		public const float TRAVEL_TIME_WARNING_THRESHOLD = 0.4f;

		// Token: 0x04004F8F RID: 20367
		public static readonly string[] ALL_ATTRIBUTES = new string[]
		{
			"Strength",
			"Caring",
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Botanist",
			"Art",
			"Ranching",
			"Athletics",
			"SpaceNavigation"
		};

		// Token: 0x04004F90 RID: 20368
		public static readonly string[] DISTRIBUTED_ATTRIBUTES = new string[]
		{
			"Strength",
			"Caring",
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Botanist",
			"Art",
			"Ranching"
		};

		// Token: 0x04004F91 RID: 20369
		public static readonly string[] ROLLED_ATTRIBUTES = new string[]
		{
			"Athletics"
		};

		// Token: 0x04004F92 RID: 20370
		public static readonly int[] APTITUDE_ATTRIBUTE_BONUSES = new int[]
		{
			7,
			3,
			1
		};

		// Token: 0x04004F93 RID: 20371
		public static int ROLLED_ATTRIBUTE_MAX = 5;

		// Token: 0x04004F94 RID: 20372
		public static float ROLLED_ATTRIBUTE_POWER = 4f;

		// Token: 0x04004F95 RID: 20373
		public static Dictionary<string, List<string>> ARCHETYPE_TRAIT_EXCLUSIONS = new Dictionary<string, List<string>>
		{
			{
				"Mining",
				new List<string>
				{
					"Anemic",
					"DiggingDown",
					"Narcolepsy"
				}
			},
			{
				"Building",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"ConstructionDown",
					"DiggingDown",
					"Narcolepsy"
				}
			},
			{
				"Farming",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"BotanistDown",
					"RanchingDown",
					"Narcolepsy"
				}
			},
			{
				"Ranching",
				new List<string>
				{
					"RanchingDown",
					"BotanistDown",
					"Narcolepsy"
				}
			},
			{
				"Cooking",
				new List<string>
				{
					"NoodleArms",
					"CookingDown"
				}
			},
			{
				"Art",
				new List<string>
				{
					"ArtDown",
					"DecorDown"
				}
			},
			{
				"Research",
				new List<string>
				{
					"SlowLearner"
				}
			},
			{
				"Suits",
				new List<string>
				{
					"Anemic",
					"NoodleArms"
				}
			},
			{
				"Hauling",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"Narcolepsy"
				}
			},
			{
				"Technicals",
				new List<string>
				{
					"MachineryDown"
				}
			},
			{
				"MedicalAid",
				new List<string>
				{
					"CaringDown",
					"WeakImmuneSystem"
				}
			},
			{
				"Basekeeping",
				new List<string>
				{
					"Anemic",
					"NoodleArms"
				}
			},
			{
				"Rocketry",
				new List<string>()
			}
		};

		// Token: 0x04004F96 RID: 20374
		public static int RARITY_LEGENDARY = 5;

		// Token: 0x04004F97 RID: 20375
		public static int RARITY_EPIC = 4;

		// Token: 0x04004F98 RID: 20376
		public static int RARITY_RARE = 3;

		// Token: 0x04004F99 RID: 20377
		public static int RARITY_UNCOMMON = 2;

		// Token: 0x04004F9A RID: 20378
		public static int RARITY_COMMON = 1;

		// Token: 0x04004F9B RID: 20379
		public static int NO_STATPOINT_BONUS = 0;

		// Token: 0x04004F9C RID: 20380
		public static int TINY_STATPOINT_BONUS = 1;

		// Token: 0x04004F9D RID: 20381
		public static int SMALL_STATPOINT_BONUS = 2;

		// Token: 0x04004F9E RID: 20382
		public static int MEDIUM_STATPOINT_BONUS = 3;

		// Token: 0x04004F9F RID: 20383
		public static int LARGE_STATPOINT_BONUS = 4;

		// Token: 0x04004FA0 RID: 20384
		public static int HUGE_STATPOINT_BONUS = 5;

		// Token: 0x04004FA1 RID: 20385
		public static int COMMON = 1;

		// Token: 0x04004FA2 RID: 20386
		public static int UNCOMMON = 2;

		// Token: 0x04004FA3 RID: 20387
		public static int RARE = 3;

		// Token: 0x04004FA4 RID: 20388
		public static int EPIC = 4;

		// Token: 0x04004FA5 RID: 20389
		public static int LEGENDARY = 5;

		// Token: 0x04004FA6 RID: 20390
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(1, 1);

		// Token: 0x04004FA7 RID: 20391
		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(2, 1);

		// Token: 0x04004FA8 RID: 20392
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(1, 2);

		// Token: 0x04004FA9 RID: 20393
		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(2, 2);

		// Token: 0x04004FAA RID: 20394
		public static global::Tuple<int, int> TRAITS_THREE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(3, 1);

		// Token: 0x04004FAB RID: 20395
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_THREE_NEGATIVE = new global::Tuple<int, int>(1, 3);

		// Token: 0x04004FAC RID: 20396
		public static int MIN_STAT_POINTS = 0;

		// Token: 0x04004FAD RID: 20397
		public static int MAX_STAT_POINTS = 0;

		// Token: 0x04004FAE RID: 20398
		public static int MAX_TRAITS = 4;

		// Token: 0x04004FAF RID: 20399
		public static int APTITUDE_BONUS = 1;

		// Token: 0x04004FB0 RID: 20400
		public static List<int> RARITY_DECK = new List<int>
		{
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_EPIC,
			DUPLICANTSTATS.RARITY_EPIC,
			DUPLICANTSTATS.RARITY_LEGENDARY
		};

		// Token: 0x04004FB1 RID: 20401
		public static List<int> rarityDeckActive = new List<int>(DUPLICANTSTATS.RARITY_DECK);

		// Token: 0x04004FB2 RID: 20402
		public static List<global::Tuple<int, int>> POD_TRAIT_CONFIGURATIONS_DECK = new List<global::Tuple<int, int>>
		{
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_THREE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_THREE_NEGATIVE
		};

		// Token: 0x04004FB3 RID: 20403
		public static List<global::Tuple<int, int>> podTraitConfigurationsActive = new List<global::Tuple<int, int>>(DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK);

		// Token: 0x04004FB4 RID: 20404
		public static readonly List<string> CONTRACTEDTRAITS_HEALING = new List<string>
		{
			"IrritableBowel",
			"Aggressive",
			"SlowLearner",
			"WeakImmuneSystem",
			"Snorer",
			"CantDig"
		};

		// Token: 0x04004FB5 RID: 20405
		public static readonly List<DUPLICANTSTATS.TraitVal> CONGENITALTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "None"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Joshua",
				mutuallyExclusiveTraits = new List<string>
				{
					"ScaredyCat",
					"Aggressive"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Ellie",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				mutuallyExclusiveTraits = new List<string>
				{
					"InteriorDecorator",
					"MouthBreather",
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Stinky",
				mutuallyExclusiveTraits = new List<string>
				{
					"Flatulence",
					"InteriorDecorator"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Liam",
				mutuallyExclusiveTraits = new List<string>
				{
					"Flatulence",
					"InteriorDecorator"
				}
			}
		};

		// Token: 0x04004FB6 RID: 20406
		public static readonly DUPLICANTSTATS.TraitVal INVALID_TRAIT_VAL = new DUPLICANTSTATS.TraitVal
		{
			id = "INVALID"
		};

		// Token: 0x04004FB7 RID: 20407
		public static readonly List<DUPLICANTSTATS.TraitVal> BADTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantResearch",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Research"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantDig",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Mining"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantCook",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Cooking"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantBuild",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Building"
				},
				mutuallyExclusiveTraits = new List<string>
				{
					"GrantSkill_Engineering1"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Hemophobia",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"MedicalAid"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ScaredyCat",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Mining"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ConstructionDown",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"ConstructionUp",
					"CantBuild"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RanchingDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"RanchingUp"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CaringDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BotanistDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ArtDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CookingDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Foodie",
					"CantCook"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MachineryDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DiggingDown",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"MoleHands",
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SlowLearner",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"FastLearner",
					"CantResearch"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NoodleArms",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DecorDown",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Anemic",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Flatulence",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "IrritableBowel",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Snorer",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MouthBreather",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SmallBladder",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CalorieBurner",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "WeakImmuneSystem",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Allergies",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NightLight",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Narcolepsy",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			}
		};

		// Token: 0x04004FB8 RID: 20408
		public static readonly List<DUPLICANTSTATS.TraitVal> STRESSTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Aggressive",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StressVomiter",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "UglyCrier",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BingeEater",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Banshee",
				dlcId = ""
			}
		};

		// Token: 0x04004FB9 RID: 20409
		public static readonly List<DUPLICANTSTATS.TraitVal> JOYTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "BalloonArtist",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SparkleStreaker",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StickerBomber",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SuperProductive",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "HappySinger",
				dlcId = ""
			}
		};

		// Token: 0x04004FBA RID: 20410
		public static readonly List<DUPLICANTSTATS.TraitVal> GENESHUFFLERTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Regeneration",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DeeperDiversLungs",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SunnyDisposition",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RockCrusher",
				dlcId = ""
			}
		};

		// Token: 0x04004FBB RID: 20411
		public static readonly List<DUPLICANTSTATS.TraitVal> SPECIALTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "AncientKnowledge",
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "EXPANSION1_ID",
				doNotGenerateTrait = true,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantResearch",
					"CantBuild",
					"CantCook",
					"CantDig",
					"Hemophobia",
					"ScaredyCat",
					"Anemic",
					"SlowLearner",
					"NoodleArms",
					"ConstructionDown",
					"RanchingDown",
					"DiggingDown",
					"MachineryDown",
					"CookingDown",
					"ArtDown",
					"CaringDown",
					"BotanistDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Chatty",
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				doNotGenerateTrait = true
			}
		};

		// Token: 0x04004FBC RID: 20412
		public static readonly List<DUPLICANTSTATS.TraitVal> GOODTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Twinkletoes",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Anemic"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StrongArm",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"NoodleArms"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Greasemonkey",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"MachineryDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DiversLung",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"MouthBreather"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "IronGut",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StrongImmuneSystem",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"WeakImmuneSystem"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "EarlyBird",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"NightOwl"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NightOwl",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"EarlyBird"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Meteorphile",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MoleHands",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig",
					"DiggingDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "FastLearner",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"SlowLearner",
					"CantResearch"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "InteriorDecorator",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured",
					"ArtDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Uncultured",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"InteriorDecorator"
				},
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Art"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SimpleTastes",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Foodie"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Foodie",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"SimpleTastes",
					"CantCook",
					"CookingDown"
				},
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Cooking"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BedsideManner",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia",
					"CaringDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DecorUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"DecorDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Thriver",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GreenThumb",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"BotanistDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ConstructionUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"ConstructionDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RanchingUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"RanchingDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Loner",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StarryEyed",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GlowStick",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RadiationEater",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining3",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Farming2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Ranching1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Cooking1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantCook"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting3",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Suits1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Technicals2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Engineering1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Basekeeping2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Anemic"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Medicine2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia"
				}
			}
		};

		// Token: 0x04004FBD RID: 20413
		public static readonly List<DUPLICANTSTATS.TraitVal> NEEDTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Claustrophobic",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "PrefersWarmer",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"PrefersColder"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "PrefersColder",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"PrefersWarmer"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SensitiveFeet",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Fashionable",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Climacophobic",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SolitarySleeper",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			}
		};

		// Token: 0x02001C88 RID: 7304
		public class BASESTATS
		{
			// Token: 0x04008188 RID: 33160
			public const float STAMINA_USED_PER_SECOND = -0.11666667f;

			// Token: 0x04008189 RID: 33161
			public const float MAX_CALORIES = 4000000f;

			// Token: 0x0400818A RID: 33162
			public const float CALORIES_BURNED_PER_CYCLE = -1000000f;

			// Token: 0x0400818B RID: 33163
			public const float CALORIES_BURNED_PER_SECOND = -1666.6666f;

			// Token: 0x0400818C RID: 33164
			public const float GUESSTIMATE_CALORIES_PER_CYCLE = -1600000f;

			// Token: 0x0400818D RID: 33165
			public const float GUESSTIMATE_CALORIES_BURNED_PER_SECOND = -1666.6666f;

			// Token: 0x0400818E RID: 33166
			public const float TRANSIT_TUBE_TRAVEL_SPEED = 18f;

			// Token: 0x0400818F RID: 33167
			public const float OXYGEN_USED_PER_SECOND = 0.1f;

			// Token: 0x04008190 RID: 33168
			public const float OXYGEN_TO_CO2_CONVERSION = 0.02f;

			// Token: 0x04008191 RID: 33169
			public const float LOW_OXYGEN_THRESHOLD = 0.52f;

			// Token: 0x04008192 RID: 33170
			public const float NO_OXYGEN_THRESHOLD = 0.05f;

			// Token: 0x04008193 RID: 33171
			public const float MIN_CO2_TO_EMIT = 0.02f;

			// Token: 0x04008194 RID: 33172
			public const float BLADDER_INCREASE_PER_SECOND = 0.16666667f;

			// Token: 0x04008195 RID: 33173
			public const float DECOR_EXPECTATION = 0f;

			// Token: 0x04008196 RID: 33174
			public const float FOOD_QUALITY_EXPECTATION = 0f;

			// Token: 0x04008197 RID: 33175
			public const float RECREATION_EXPECTATION = 2f;

			// Token: 0x04008198 RID: 33176
			public const float MAX_PROFESSION_DECOR_EXPECTATION = 75f;

			// Token: 0x04008199 RID: 33177
			public const float MAX_PROFESSION_FOOD_EXPECTATION = 0f;

			// Token: 0x0400819A RID: 33178
			public const int MAX_UNDERWATER_TRAVEL_COST = 8;

			// Token: 0x0400819B RID: 33179
			public const float TOILET_EFFICIENCY = 1f;

			// Token: 0x0400819C RID: 33180
			public const float ROOM_TEMPERATURE_PREFERENCE = 0f;

			// Token: 0x0400819D RID: 33181
			public const int BUILDING_DAMAGE_ACTING_OUT = 100;

			// Token: 0x0400819E RID: 33182
			public const float IMMUNE_LEVEL_MAX = 100f;

			// Token: 0x0400819F RID: 33183
			public const float IMMUNE_LEVEL_RECOVERY = 0.025f;

			// Token: 0x040081A0 RID: 33184
			public const float CARRY_CAPACITY = 200f;

			// Token: 0x040081A1 RID: 33185
			public const float HIT_POINTS = 100f;

			// Token: 0x040081A2 RID: 33186
			public const float RADIATION_RESISTANCE = 0f;
		}

		// Token: 0x02001C89 RID: 7305
		public class RADIATION_DIFFICULTY_MODIFIERS
		{
			// Token: 0x040081A3 RID: 33187
			public static float HARDEST = 0.33f;

			// Token: 0x040081A4 RID: 33188
			public static float HARDER = 0.66f;

			// Token: 0x040081A5 RID: 33189
			public static float DEFAULT = 1f;

			// Token: 0x040081A6 RID: 33190
			public static float EASIER = 2f;

			// Token: 0x040081A7 RID: 33191
			public static float EASIEST = 100f;
		}

		// Token: 0x02001C8A RID: 7306
		public class RADIATION_EXPOSURE_LEVELS
		{
			// Token: 0x040081A8 RID: 33192
			public const float LOW = 100f;

			// Token: 0x040081A9 RID: 33193
			public const float MODERATE = 300f;

			// Token: 0x040081AA RID: 33194
			public const float HIGH = 600f;

			// Token: 0x040081AB RID: 33195
			public const float DEADLY = 900f;
		}

		// Token: 0x02001C8B RID: 7307
		public class CALORIES
		{
			// Token: 0x040081AC RID: 33196
			public const float SATISFIED_THRESHOLD = 0.95f;

			// Token: 0x040081AD RID: 33197
			public const float HUNGRY_THRESHOLD = 0.825f;

			// Token: 0x040081AE RID: 33198
			public const float STARVING_THRESHOLD = 0.25f;
		}

		// Token: 0x02001C8C RID: 7308
		public class TEMPERATURE
		{
			// Token: 0x040081AF RID: 33199
			public const float SKIN_THICKNESS = 0.002f;

			// Token: 0x040081B0 RID: 33200
			public const float SURFACE_AREA = 1f;

			// Token: 0x040081B1 RID: 33201
			public const float GROUND_TRANSFER_SCALE = 0f;

			// Token: 0x0200224B RID: 8779
			public class EXTERNAL
			{
				// Token: 0x04009947 RID: 39239
				public const float THRESHOLD_COLD = 283.15f;

				// Token: 0x04009948 RID: 39240
				public const float THRESHOLD_HOT = 306.15f;

				// Token: 0x04009949 RID: 39241
				public const float THRESHOLD_SCALDING = 345f;
			}

			// Token: 0x0200224C RID: 8780
			public class INTERNAL
			{
				// Token: 0x0400994A RID: 39242
				public const float IDEAL = 310.15f;

				// Token: 0x0400994B RID: 39243
				public const float THRESHOLD_HYPOTHERMIA = 308.15f;

				// Token: 0x0400994C RID: 39244
				public const float THRESHOLD_HYPERTHERMIA = 312.15f;

				// Token: 0x0400994D RID: 39245
				public const float THRESHOLD_FATAL_HOT = 320.15f;

				// Token: 0x0400994E RID: 39246
				public const float THRESHOLD_FATAL_COLD = 300.15f;
			}

			// Token: 0x0200224D RID: 8781
			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				// Token: 0x0400994F RID: 39247
				public const float SKINNY = -0.005f;

				// Token: 0x04009950 RID: 39248
				public const float PUDGY = 0.005f;
			}
		}

		// Token: 0x02001C8D RID: 7309
		public class NOISE
		{
			// Token: 0x040081B2 RID: 33202
			public const int THRESHOLD_PEACEFUL = 0;

			// Token: 0x040081B3 RID: 33203
			public const int THRESHOLD_QUIET = 36;

			// Token: 0x040081B4 RID: 33204
			public const int THRESHOLD_TOSS_AND_TURN = 45;

			// Token: 0x040081B5 RID: 33205
			public const int THRESHOLD_WAKE_UP = 60;

			// Token: 0x040081B6 RID: 33206
			public const int THRESHOLD_MINOR_REACTION = 80;

			// Token: 0x040081B7 RID: 33207
			public const int THRESHOLD_MAJOR_REACTION = 106;

			// Token: 0x040081B8 RID: 33208
			public const int THRESHOLD_EXTREME_REACTION = 125;
		}

		// Token: 0x02001C8E RID: 7310
		public class BREATH
		{
			// Token: 0x040081B9 RID: 33209
			private const float BREATH_BAR_TOTAL_SECONDS = 110f;

			// Token: 0x040081BA RID: 33210
			private const float RETREAT_AT_SECONDS = 80f;

			// Token: 0x040081BB RID: 33211
			private const float SUFFOCATION_WARN_AT_SECONDS = 50f;

			// Token: 0x040081BC RID: 33212
			public const float BREATH_BAR_TOTAL_AMOUNT = 100f;

			// Token: 0x040081BD RID: 33213
			public const float RETREAT_AMOUNT = 72.72727f;

			// Token: 0x040081BE RID: 33214
			public const float SUFFOCATE_AMOUNT = 45.454548f;

			// Token: 0x040081BF RID: 33215
			public const float BREATH_RATE = 0.90909094f;
		}

		// Token: 0x02001C8F RID: 7311
		public class LIGHT
		{
			// Token: 0x040081C0 RID: 33216
			public const int LUX_SUNBURN = 72000;

			// Token: 0x040081C1 RID: 33217
			public const float SUNBURN_DELAY_TIME = 120f;

			// Token: 0x040081C2 RID: 33218
			public const int LUX_PLEASANT_LIGHT = 40000;

			// Token: 0x040081C3 RID: 33219
			public const float LIGHT_WORK_EFFICIENCY_BONUS = 0.15f;

			// Token: 0x040081C4 RID: 33220
			public const int NO_LIGHT = 0;

			// Token: 0x040081C5 RID: 33221
			public const int VERY_LOW_LIGHT = 1;

			// Token: 0x040081C6 RID: 33222
			public const int LOW_LIGHT = 100;

			// Token: 0x040081C7 RID: 33223
			public const int MEDIUM_LIGHT = 1000;

			// Token: 0x040081C8 RID: 33224
			public const int HIGH_LIGHT = 10000;

			// Token: 0x040081C9 RID: 33225
			public const int VERY_HIGH_LIGHT = 50000;

			// Token: 0x040081CA RID: 33226
			public const int MAX_LIGHT = 100000;
		}

		// Token: 0x02001C90 RID: 7312
		public class MOVEMENT
		{
			// Token: 0x040081CB RID: 33227
			public static float NEUTRAL = 1f;

			// Token: 0x040081CC RID: 33228
			public static float BONUS_1 = 1.1f;

			// Token: 0x040081CD RID: 33229
			public static float BONUS_2 = 1.25f;

			// Token: 0x040081CE RID: 33230
			public static float BONUS_3 = 1.5f;

			// Token: 0x040081CF RID: 33231
			public static float BONUS_4 = 1.75f;

			// Token: 0x040081D0 RID: 33232
			public static float PENALTY_1 = 0.9f;

			// Token: 0x040081D1 RID: 33233
			public static float PENALTY_2 = 0.75f;

			// Token: 0x040081D2 RID: 33234
			public static float PENALTY_3 = 0.5f;

			// Token: 0x040081D3 RID: 33235
			public static float PENALTY_4 = 0.25f;
		}

		// Token: 0x02001C91 RID: 7313
		public class QOL_STRESS
		{
			// Token: 0x040081D4 RID: 33236
			public const float ABOVE_EXPECTATIONS = -0.016666668f;

			// Token: 0x040081D5 RID: 33237
			public const float AT_EXPECTATIONS = -0.008333334f;

			// Token: 0x040081D6 RID: 33238
			public const float MIN_STRESS = -0.033333335f;

			// Token: 0x0200224E RID: 8782
			public class BELOW_EXPECTATIONS
			{
				// Token: 0x04009951 RID: 39249
				public const float EASY = 0.0033333334f;

				// Token: 0x04009952 RID: 39250
				public const float NEUTRAL = 0.004166667f;

				// Token: 0x04009953 RID: 39251
				public const float HARD = 0.008333334f;

				// Token: 0x04009954 RID: 39252
				public const float VERYHARD = 0.016666668f;
			}

			// Token: 0x0200224F RID: 8783
			public class MAX_STRESS
			{
				// Token: 0x04009955 RID: 39253
				public const float EASY = 0.016666668f;

				// Token: 0x04009956 RID: 39254
				public const float NEUTRAL = 0.041666668f;

				// Token: 0x04009957 RID: 39255
				public const float HARD = 0.05f;

				// Token: 0x04009958 RID: 39256
				public const float VERYHARD = 0.083333336f;
			}
		}

		// Token: 0x02001C92 RID: 7314
		public class COMBAT
		{
			// Token: 0x040081D7 RID: 33239
			public const Health.HealthState FLEE_THRESHOLD = Health.HealthState.Critical;

			// Token: 0x02002250 RID: 8784
			public class BASICWEAPON
			{
				// Token: 0x04009959 RID: 39257
				public const float ATTACKS_PER_SECOND = 2f;

				// Token: 0x0400995A RID: 39258
				public const float MIN_DAMAGE_PER_HIT = 1f;

				// Token: 0x0400995B RID: 39259
				public const float MAX_DAMAGE_PER_HIT = 1f;

				// Token: 0x0400995C RID: 39260
				public const AttackProperties.TargetType TARGET_TYPE = AttackProperties.TargetType.Single;

				// Token: 0x0400995D RID: 39261
				public const AttackProperties.DamageType DAMAGE_TYPE = AttackProperties.DamageType.Standard;

				// Token: 0x0400995E RID: 39262
				public const int MAX_HITS = 1;

				// Token: 0x0400995F RID: 39263
				public const float AREA_OF_EFFECT_RADIUS = 0f;
			}
		}

		// Token: 0x02001C93 RID: 7315
		public class CLOTHING
		{
			// Token: 0x02002251 RID: 8785
			public class DECOR_MODIFICATION
			{
				// Token: 0x04009960 RID: 39264
				public const int NEGATIVE_SIGNIFICANT = -30;

				// Token: 0x04009961 RID: 39265
				public const int NEGATIVE_MILD = -10;

				// Token: 0x04009962 RID: 39266
				public const int BASIC = -5;

				// Token: 0x04009963 RID: 39267
				public const int POSITIVE_MILD = 10;

				// Token: 0x04009964 RID: 39268
				public const int POSITIVE_SIGNIFICANT = 30;

				// Token: 0x04009965 RID: 39269
				public const int POSITIVE_MAJOR = 40;
			}

			// Token: 0x02002252 RID: 8786
			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				// Token: 0x04009966 RID: 39270
				public const float THIN = 0.0005f;

				// Token: 0x04009967 RID: 39271
				public const float BASIC = 0.0025f;

				// Token: 0x04009968 RID: 39272
				public const float THICK = 0.01f;
			}

			// Token: 0x02002253 RID: 8787
			public class SWEAT_EFFICIENCY_MULTIPLIER
			{
				// Token: 0x04009969 RID: 39273
				public const float DIMINISH_SIGNIFICANT = -2.5f;

				// Token: 0x0400996A RID: 39274
				public const float DIMINISH_MILD = -1.25f;

				// Token: 0x0400996B RID: 39275
				public const float NEUTRAL = 0f;

				// Token: 0x0400996C RID: 39276
				public const float IMPROVE = 2f;
			}
		}

		// Token: 0x02001C94 RID: 7316
		public class DISTRIBUTIONS
		{
			// Token: 0x06009D6F RID: 40303 RVA: 0x003503CB File Offset: 0x0034E5CB
			public static int[] GetRandomDistribution()
			{
				return DUPLICANTSTATS.DISTRIBUTIONS.TYPES[UnityEngine.Random.Range(0, DUPLICANTSTATS.DISTRIBUTIONS.TYPES.Count)];
			}

			// Token: 0x040081D8 RID: 33240
			public static readonly List<int[]> TYPES = new List<int[]>
			{
				new int[]
				{
					5,
					4,
					4,
					3,
					3,
					2,
					1
				},
				new int[]
				{
					5,
					3,
					2,
					1
				},
				new int[]
				{
					5,
					2,
					2,
					1
				},
				new int[]
				{
					5,
					1
				},
				new int[]
				{
					5,
					3,
					1
				},
				new int[]
				{
					3,
					3,
					3,
					3,
					1
				},
				new int[]
				{
					4
				},
				new int[]
				{
					3
				},
				new int[]
				{
					2
				},
				new int[]
				{
					1
				}
			};
		}

		// Token: 0x02001C95 RID: 7317
		public struct TraitVal
		{
			// Token: 0x040081D9 RID: 33241
			public string id;

			// Token: 0x040081DA RID: 33242
			public int statBonus;

			// Token: 0x040081DB RID: 33243
			public int impact;

			// Token: 0x040081DC RID: 33244
			public int rarity;

			// Token: 0x040081DD RID: 33245
			public string dlcId;

			// Token: 0x040081DE RID: 33246
			public List<string> mutuallyExclusiveTraits;

			// Token: 0x040081DF RID: 33247
			public List<HashedString> mutuallyExclusiveAptitudes;

			// Token: 0x040081E0 RID: 33248
			public bool doNotGenerateTrait;
		}

		// Token: 0x02001C96 RID: 7318
		public class ATTRIBUTE_LEVELING
		{
			// Token: 0x040081E1 RID: 33249
			public static int MAX_GAINED_ATTRIBUTE_LEVEL = 20;

			// Token: 0x040081E2 RID: 33250
			public static int TARGET_MAX_LEVEL_CYCLE = 400;

			// Token: 0x040081E3 RID: 33251
			public static float EXPERIENCE_LEVEL_POWER = 1.7f;

			// Token: 0x040081E4 RID: 33252
			public static float FULL_EXPERIENCE = 1f;

			// Token: 0x040081E5 RID: 33253
			public static float ALL_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.8f;

			// Token: 0x040081E6 RID: 33254
			public static float MOST_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.5f;

			// Token: 0x040081E7 RID: 33255
			public static float PART_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.25f;

			// Token: 0x040081E8 RID: 33256
			public static float BARELY_EVER_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.1f;
		}

		// Token: 0x02001C97 RID: 7319
		public class ROOM
		{
			// Token: 0x040081E9 RID: 33257
			public const float LABORATORY_RESEARCH_EFFICIENCY_BONUS = 0.1f;
		}
	}
}
