﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class CookingStationConfig : IBuildingConfig
{
	// Token: 0x06000178 RID: 376 RVA: 0x0000A688 File Offset: 0x00008888
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CookingStation";
		int width = 3;
		int height = 2;
		string anim = "cookstation_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		return buildingDef;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000A70C File Offset: 0x0000890C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		CookingStation cookingStation = go.AddOrGet<CookingStation>();
		cookingStation.heatedTemperature = 368.15f;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_cookstation_kanim")
		};
		cookingStation.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		Prioritizable.AddRef(go);
		go.AddOrGet<DropAllWorkable>();
		this.ConfigureRecipes();
		go.AddOrGetDef<PoweredController.Def>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, cookingStation);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CookTop, false);
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000A7A4 File Offset: 0x000089A4
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicPlantFood", 3f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("PickledMeal", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		PickledMealConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array, array2), array, array2)
		{
			time = FOOD.RECIPES.SMALL_COOK_TIME,
			description = ITEMS.FOOD.PICKLEDMEAL.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"CookingStation"
			},
			sortOrder = 21
		};
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("MushBar", 1f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("FriedMushBar".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		FriedMushBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array3, array4), array3, array4)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.FRIEDMUSHBAR.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"CookingStation"
			},
			sortOrder = 1
		};
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(MushroomConfig.ID, 1f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("FriedMushroom", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		FriedMushroomConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array5, array6), array5, array6)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.FRIEDMUSHROOM.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"CookingStation"
			},
			sortOrder = 20
		};
		ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("RawEgg", 1f),
			new ComplexRecipe.RecipeElement("ColdWheatSeed", 2f)
		};
		ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Pancakes", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		CookedEggConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array7, array8), array7, array8)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.PANCAKES.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"CookingStation"
			},
			sortOrder = 20
		};
		ComplexRecipe.RecipeElement[] array9 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Meat", 2f)
		};
		ComplexRecipe.RecipeElement[] array10 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("CookedMeat", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		CookedMeatConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array9, array10), array9, array10)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.COOKEDMEAT.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"CookingStation"
			},
			sortOrder = 21
		};
		ComplexRecipe.RecipeElement[] array11 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("FishMeat", 1f)
		};
		ComplexRecipe.RecipeElement[] array12 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("CookedFish", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		CookedMeatConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array11, array12), array11, array12)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.COOKEDMEAT.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			fabricators = new List<Tag>
			{
				"CookingStation"
			},
			sortOrder = 22
		};
		ComplexRecipe.RecipeElement[] array13 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("ShellfishMeat", 1f)
		};
		ComplexRecipe.RecipeElement[] array14 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("CookedFish", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		CookedMeatConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array13, array14), array13, array14)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.COOKEDMEAT.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			fabricators = new List<Tag>
			{
				"CookingStation"
			},
			sortOrder = 22
		};
		ComplexRecipe.RecipeElement[] array15 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(PrickleFruitConfig.ID, 1f)
		};
		ComplexRecipe.RecipeElement[] array16 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("GrilledPrickleFruit", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		GrilledPrickleFruitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array15, array16), array15, array16)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.GRILLEDPRICKLEFRUIT.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"CookingStation"
			},
			sortOrder = 20
		};
		if (DlcManager.IsExpansion1Active())
		{
			ComplexRecipe.RecipeElement[] array17 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(SwampFruitConfig.ID, 1f)
			};
			ComplexRecipe.RecipeElement[] array18 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("SwampDelights", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
			};
			CookedEggConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array17, array18), array17, array18)
			{
				time = FOOD.RECIPES.STANDARD_COOK_TIME,
				description = ITEMS.FOOD.SWAMPDELIGHTS.RECIPEDESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
				fabricators = new List<Tag>
				{
					"CookingStation"
				},
				sortOrder = 20
			};
		}
		ComplexRecipe.RecipeElement[] array19 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("ColdWheatSeed", 3f)
		};
		ComplexRecipe.RecipeElement[] array20 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("ColdWheatBread", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		ColdWheatBreadConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array19, array20), array19, array20)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.COLDWHEATBREAD.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"CookingStation"
			},
			sortOrder = 50
		};
		ComplexRecipe.RecipeElement[] array21 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("RawEgg", 1f)
		};
		ComplexRecipe.RecipeElement[] array22 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("CookedEgg", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		CookedEggConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array21, array22), array21, array22)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.COOKEDEGG.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"CookingStation"
			},
			sortOrder = 1
		};
		if (DlcManager.IsExpansion1Active())
		{
			ComplexRecipe.RecipeElement[] array23 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("WormBasicFruit", 1f)
			};
			ComplexRecipe.RecipeElement[] array24 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("WormBasicFood", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
			};
			WormBasicFoodConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array23, array24), array23, array24)
			{
				time = FOOD.RECIPES.STANDARD_COOK_TIME,
				description = ITEMS.FOOD.WORMBASICFOOD.RECIPEDESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
				fabricators = new List<Tag>
				{
					"CookingStation"
				},
				sortOrder = 20
			};
		}
		if (DlcManager.IsExpansion1Active())
		{
			ComplexRecipe.RecipeElement[] array25 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("WormSuperFruit", 8f),
				new ComplexRecipe.RecipeElement("Sucrose".ToTag(), 4f)
			};
			ComplexRecipe.RecipeElement[] array26 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("WormSuperFood", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
			};
			WormSuperFoodConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array25, array26), array25, array26)
			{
				time = FOOD.RECIPES.STANDARD_COOK_TIME,
				description = ITEMS.FOOD.WORMSUPERFOOD.RECIPEDESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
				fabricators = new List<Tag>
				{
					"CookingStation"
				},
				sortOrder = 20
			};
		}
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000B01C File Offset: 0x0000921C
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040000EE RID: 238
	public const string ID = "CookingStation";
}
