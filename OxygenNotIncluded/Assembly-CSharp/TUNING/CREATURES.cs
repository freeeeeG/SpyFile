using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;

namespace TUNING
{
	// Token: 0x02000D97 RID: 3479
	public class CREATURES
	{
		// Token: 0x04005002 RID: 20482
		public const float WILD_GROWTH_RATE_MODIFIER = 0.25f;

		// Token: 0x04005003 RID: 20483
		public const int DEFAULT_PROBING_RADIUS = 32;

		// Token: 0x04005004 RID: 20484
		public const float FERTILITY_TIME_BY_LIFESPAN = 0.6f;

		// Token: 0x04005005 RID: 20485
		public const float INCUBATION_TIME_BY_LIFESPAN = 0.2f;

		// Token: 0x04005006 RID: 20486
		public const float INCUBATOR_INCUBATION_MULTIPLIER = 4f;

		// Token: 0x04005007 RID: 20487
		public const float WILD_CALORIE_BURN_RATIO = 0.25f;

		// Token: 0x04005008 RID: 20488
		public const float HUG_INCUBATION_MULTIPLIER = 1f;

		// Token: 0x04005009 RID: 20489
		public const float VIABILITY_LOSS_RATE = -0.016666668f;

		// Token: 0x0400500A RID: 20490
		public const float STATERPILLAR_POWER_CHARGE_LOSS_RATE = -0.055555556f;

		// Token: 0x02001CA2 RID: 7330
		public class HITPOINTS
		{
			// Token: 0x0400824F RID: 33359
			public const float TIER0 = 5f;

			// Token: 0x04008250 RID: 33360
			public const float TIER1 = 25f;

			// Token: 0x04008251 RID: 33361
			public const float TIER2 = 50f;

			// Token: 0x04008252 RID: 33362
			public const float TIER3 = 100f;

			// Token: 0x04008253 RID: 33363
			public const float TIER4 = 150f;

			// Token: 0x04008254 RID: 33364
			public const float TIER5 = 200f;

			// Token: 0x04008255 RID: 33365
			public const float TIER6 = 400f;
		}

		// Token: 0x02001CA3 RID: 7331
		public class MASS_KG
		{
			// Token: 0x04008256 RID: 33366
			public const float TIER0 = 5f;

			// Token: 0x04008257 RID: 33367
			public const float TIER1 = 25f;

			// Token: 0x04008258 RID: 33368
			public const float TIER2 = 50f;

			// Token: 0x04008259 RID: 33369
			public const float TIER3 = 100f;

			// Token: 0x0400825A RID: 33370
			public const float TIER4 = 200f;

			// Token: 0x0400825B RID: 33371
			public const float TIER5 = 400f;
		}

		// Token: 0x02001CA4 RID: 7332
		public class TEMPERATURE
		{
			// Token: 0x0400825C RID: 33372
			public static float FREEZING_10 = 173f;

			// Token: 0x0400825D RID: 33373
			public static float FREEZING_9 = 183f;

			// Token: 0x0400825E RID: 33374
			public static float FREEZING_3 = 243f;

			// Token: 0x0400825F RID: 33375
			public static float FREEZING_2 = 253f;

			// Token: 0x04008260 RID: 33376
			public static float FREEZING_1 = 263f;

			// Token: 0x04008261 RID: 33377
			public static float FREEZING = 273f;

			// Token: 0x04008262 RID: 33378
			public static float COOL = 283f;

			// Token: 0x04008263 RID: 33379
			public static float MODERATE = 293f;

			// Token: 0x04008264 RID: 33380
			public static float HOT = 303f;

			// Token: 0x04008265 RID: 33381
			public static float HOT_1 = 313f;

			// Token: 0x04008266 RID: 33382
			public static float HOT_2 = 323f;

			// Token: 0x04008267 RID: 33383
			public static float HOT_3 = 333f;
		}

		// Token: 0x02001CA5 RID: 7333
		public class LIFESPAN
		{
			// Token: 0x04008268 RID: 33384
			public const float TIER0 = 5f;

			// Token: 0x04008269 RID: 33385
			public const float TIER1 = 25f;

			// Token: 0x0400826A RID: 33386
			public const float TIER2 = 75f;

			// Token: 0x0400826B RID: 33387
			public const float TIER3 = 100f;

			// Token: 0x0400826C RID: 33388
			public const float TIER4 = 150f;

			// Token: 0x0400826D RID: 33389
			public const float TIER5 = 200f;

			// Token: 0x0400826E RID: 33390
			public const float TIER6 = 400f;
		}

		// Token: 0x02001CA6 RID: 7334
		public class CONVERSION_EFFICIENCY
		{
			// Token: 0x0400826F RID: 33391
			public static float BAD_2 = 0.1f;

			// Token: 0x04008270 RID: 33392
			public static float BAD_1 = 0.25f;

			// Token: 0x04008271 RID: 33393
			public static float NORMAL = 0.5f;

			// Token: 0x04008272 RID: 33394
			public static float GOOD_1 = 0.75f;

			// Token: 0x04008273 RID: 33395
			public static float GOOD_2 = 0.95f;

			// Token: 0x04008274 RID: 33396
			public static float GOOD_3 = 1f;
		}

		// Token: 0x02001CA7 RID: 7335
		public class SPACE_REQUIREMENTS
		{
			// Token: 0x04008275 RID: 33397
			public static int TIER1 = 4;

			// Token: 0x04008276 RID: 33398
			public static int TIER2 = 8;

			// Token: 0x04008277 RID: 33399
			public static int TIER3 = 12;

			// Token: 0x04008278 RID: 33400
			public static int TIER4 = 16;
		}

		// Token: 0x02001CA8 RID: 7336
		public class EGG_CHANCE_MODIFIERS
		{
			// Token: 0x06009D87 RID: 40327 RVA: 0x00350F27 File Offset: 0x0034F127
			private static System.Action CreateDietaryModifier(string id, Tag eggTag, HashSet<Tag> foodTags, float modifierPerCal)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.DIET.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.DIET.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in foodTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							inst.gameObject.Subscribe(-2038961714, delegate(object data)
							{
								CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
								if (foodTags.Contains(caloriesConsumedEvent.tag))
								{
									inst.AddBreedingChance(eggType, caloriesConsumedEvent.calories * modifierPerCal);
								}
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x06009D88 RID: 40328 RVA: 0x00350F55 File Offset: 0x0034F155
			private static System.Action CreateDietaryModifier(string id, Tag eggTag, Tag foodTag, float modifierPerCal)
			{
				return CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier(id, eggTag, new HashSet<Tag>
				{
					foodTag
				}, modifierPerCal);
			}

			// Token: 0x06009D89 RID: 40329 RVA: 0x00350F6C File Offset: 0x0034F16C
			private static System.Action CreateNearbyCreatureModifier(string id, Tag eggTag, Tag nearbyCreatureBaby, Tag nearbyCreatureAdult, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = (modifierPerSecond < 0f) ? CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.NAME : CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.NAME;
					string text2 = (modifierPerSecond < 0f) ? CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.DESC : CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string descStr) => string.Format(descStr, nearbyCreatureAdult.ProperName())));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							NearbyCreatureMonitor.Instance instance = inst.gameObject.GetSMI<NearbyCreatureMonitor.Instance>();
							if (instance == null)
							{
								instance = new NearbyCreatureMonitor.Instance(inst.master);
								instance.StartSM();
							}
							instance.OnUpdateNearbyCreatures += delegate(float dt, List<KPrefabID> creatures, List<KPrefabID> eggs)
							{
								bool flag = false;
								foreach (KPrefabID kprefabID in creatures)
								{
									if (kprefabID.PrefabTag == nearbyCreatureBaby || kprefabID.PrefabTag == nearbyCreatureAdult)
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
									return;
								}
								if (alsoInvert)
								{
									inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
								}
							};
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x06009D8A RID: 40330 RVA: 0x00350FAC File Offset: 0x0034F1AC
			private static System.Action CreateElementCreatureModifier(string id, Tag eggTag, Tag element, float modifierPerSecond, bool alsoInvert, bool checkSubstantialLiquid, string tooltipOverride = null)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							if (tooltipOverride == null)
							{
								return string.Format(descStr, ElementLoader.GetElement(element).name);
							}
							return tooltipOverride;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							CritterElementMonitor.Instance instance = inst.gameObject.GetSMI<CritterElementMonitor.Instance>();
							if (instance == null)
							{
								instance = new CritterElementMonitor.Instance(inst.master);
								instance.StartSM();
							}
							instance.OnUpdateEggChances += delegate(float dt)
							{
								int num = Grid.PosToCell(inst);
								if (!Grid.IsValidCell(num))
								{
									return;
								}
								if (Grid.Element[num].HasTag(element) && (!checkSubstantialLiquid || Grid.IsSubstantialLiquid(num, 0.35f)))
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
									return;
								}
								if (alsoInvert)
								{
									inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
								}
							};
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x06009D8B RID: 40331 RVA: 0x00350FFD File Offset: 0x0034F1FD
			private static System.Action CreateCropTendedModifier(string id, Tag eggTag, HashSet<Tag> cropTags, float modifierPerEvent)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.CROPTENDING.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.CROPTENDING.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in cropTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							inst.gameObject.Subscribe(90606262, delegate(object data)
							{
								CropTendingStates.CropTendingEventData cropTendingEventData = (CropTendingStates.CropTendingEventData)data;
								if (cropTags.Contains(cropTendingEventData.cropId))
								{
									inst.AddBreedingChance(eggType, modifierPerEvent);
								}
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x06009D8C RID: 40332 RVA: 0x0035102B File Offset: 0x0034F22B
			private static System.Action CreateTemperatureModifier(string id, Tag eggTag, float minTemp, float maxTemp, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.NAME;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = null;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string src) => string.Format(CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.DESC, GameUtil.GetFormattedTemperature(minTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(maxTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							TemperatureVulnerable component = inst.master.GetComponent<TemperatureVulnerable>();
							if (component != null)
							{
								component.OnTemperature += delegate(float dt, float newTemp)
								{
									if (newTemp > minTemp && newTemp < maxTemp)
									{
										inst.AddBreedingChance(eggType, dt * modifierPerSecond);
										return;
									}
									if (alsoInvert)
									{
										inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
									}
								};
								return;
							}
							DebugUtil.LogErrorArgs(new object[]
							{
								"Ack! Trying to add temperature modifier",
								id,
								"to",
								inst.master.name,
								"but it's not temperature vulnerable!"
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x04008279 RID: 33401
			public static List<System.Action> MODIFIER_CREATORS = new List<System.Action>
			{
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchHard", "HatchHardEgg".ToTag(), SimHashes.SedimentaryRock.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchVeggie", "HatchVeggieEgg".ToTag(), SimHashes.Dirt.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchMetal", "HatchMetalEgg".ToTag(), HatchMetalConfig.METAL_ORE_TAGS, 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaBalance", "PuftAlphaEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), -0.00025f, true),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyOxylite", "PuftOxyliteEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyBleachstone", "PuftBleachstoneEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterHighTemp", "OilfloaterHighTempEgg".ToTag(), 373.15f, 523.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterDecor", "OilfloaterDecorEgg".ToTag(), 293.15f, 333.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugOrange", "LightBugOrangeEgg".ToTag(), "GrilledPrickleFruit".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPurple", "LightBugPurpleEgg".ToTag(), "FriedMushroom".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPink", "LightBugPinkEgg".ToTag(), "SpiceBread".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlue", "LightBugBlueEgg".ToTag(), "Salsa".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlack", "LightBugBlackEgg".ToTag(), SimHashes.Phosphorus.CreateTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugCrystal", "LightBugCrystalEgg".ToTag(), "CookedMeat".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuTropical", "PacuTropicalEgg".ToTag(), 308.15f, 353.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuCleaner", "PacuCleanerEgg".ToTag(), 243.15f, 278.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("DreckoPlastic", "DreckoPlasticEgg".ToTag(), "BasicSingleHarvestPlant".ToTag(), 0.025f / DreckoTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("SquirrelHug", "SquirrelHugEgg".ToTag(), BasicFabricMaterialPlantConfig.ID.ToTag(), 0.025f / SquirrelTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateCropTendedModifier("DivergentWorm", "DivergentWormEgg".ToTag(), new HashSet<Tag>
				{
					"WormPlant".ToTag(),
					"SuperWormPlant".ToTag()
				}, 0.05f / (float)DivergentTuning.TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeLumber", "CrabWoodEgg".ToTag(), SimHashes.Ethanol.CreateTag(), 0.00025f, true, true, null),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeFreshWater", "CrabFreshWaterEgg".ToTag(), SimHashes.Water.CreateTag(), 0.00025f, true, true, null),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("MoleDelicacy", "MoleDelicacyEgg".ToTag(), MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MIN, MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MAX, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarGas", "StaterpillarGasEgg".ToTag(), GameTags.Unbreathable, 0.00025f, true, false, CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.UNBREATHABLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarLiquid", "StaterpillarLiquidEgg".ToTag(), GameTags.Liquid, 0.00025f, true, false, CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.LIQUID)
			};
		}

		// Token: 0x02001CA9 RID: 7337
		public class SORTING
		{
			// Token: 0x0400827A RID: 33402
			public static Dictionary<string, int> CRITTER_ORDER = new Dictionary<string, int>
			{
				{
					"Hatch",
					10
				},
				{
					"Puft",
					20
				},
				{
					"Drecko",
					30
				},
				{
					"Squirrel",
					40
				},
				{
					"Pacu",
					50
				},
				{
					"Oilfloater",
					60
				},
				{
					"LightBug",
					70
				},
				{
					"Crab",
					80
				},
				{
					"DivergentBeetle",
					90
				},
				{
					"Staterpillar",
					100
				},
				{
					"Mole",
					110
				},
				{
					"Bee",
					120
				},
				{
					"Moo",
					130
				},
				{
					"Glom",
					140
				}
			};
		}
	}
}
