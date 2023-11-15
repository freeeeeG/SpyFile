﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200027B RID: 635
public class EggConfig
{
	// Token: 0x06000CDA RID: 3290 RVA: 0x000477C8 File Offset: 0x000459C8
	public static GameObject CreateEgg(string id, string name, string desc, Tag creature_id, string anim, float mass, int egg_sort_order, float base_incubation_rate)
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(id, name, desc, mass, true, Assets.GetAnim(anim), "idle", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, true, 0, SimHashes.Creature, null);
		gameObject.AddOrGet<KBoxCollider2D>().offset = new Vector2f(0f, 0.36f);
		gameObject.AddOrGet<Pickupable>().sortOrder = SORTORDER.EGGS + egg_sort_order;
		gameObject.AddOrGet<Effects>();
		KPrefabID kprefabID = gameObject.AddOrGet<KPrefabID>();
		kprefabID.AddTag(GameTags.Egg, false);
		kprefabID.AddTag(GameTags.IncubatableEgg, false);
		kprefabID.AddTag(GameTags.PedestalDisplayable, false);
		IncubationMonitor.Def def = gameObject.AddOrGetDef<IncubationMonitor.Def>();
		def.spawnedCreature = creature_id;
		def.baseIncubationRate = base_incubation_rate;
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
		UnityEngine.Object.Destroy(gameObject.GetComponent<EntitySplitter>());
		Assets.AddPrefab(gameObject.GetComponent<KPrefabID>());
		string arg = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RESULT_DESCRIPTION, name);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(id, 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("RawEgg", 0.5f * mass, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
			new ComplexRecipe.RecipeElement("EggShell", 0.5f * mass, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID(id, "RawEgg");
		string text = ComplexRecipeManager.MakeRecipeID("EggCracker", array, array2);
		ComplexRecipe complexRecipe = new ComplexRecipe(text, array, array2);
		complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, name, arg);
		complexRecipe.fabricators = new List<Tag>
		{
			"EggCracker"
		};
		complexRecipe.time = 5f;
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		return gameObject;
	}
}
