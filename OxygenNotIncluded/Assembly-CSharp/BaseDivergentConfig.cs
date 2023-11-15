﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200008D RID: 141
public static class BaseDivergentConfig
{
	// Token: 0x06000299 RID: 665 RVA: 0x00013198 File Offset: 0x00011398
	public static GameObject BaseDivergent(string id, string name, string desc, float mass, string anim_file, string traitId, bool is_baby, float num_tended_per_cycle = 8f, string symbolOverridePrefix = null, string cropTendingEffect = "DivergentCropTended", int meatAmount = 1, bool is_pacifist = true)
	{
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		string navGridName = "WalkerNavGrid1x1";
		if (is_baby)
		{
			navGridName = "WalkerBabyNavGrid";
		}
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Pest, traitId, navGridName, NavType.Floor, 32, 2f, "Meat", meatAmount, true, false, 283.15f, 293.15f, 243.15f, 343.15f);
		if (symbolOverridePrefix != null)
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbolOverridePrefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["DivergentBeetle"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		gameObject.AddOrGetDef<BurrowMonitor.Def>();
		gameObject.AddOrGetDef<CropTendingMonitor.Def>().numCropsTendedPerCycle = num_tended_per_cycle;
		gameObject.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Creatures.Walker, false);
		component.prefabInitFn += delegate(GameObject inst)
		{
			inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
		};
		CropTendingStates.Def def = new CropTendingStates.Def();
		def.effectId = cropTendingEffect;
		def.interests.Add("WormPlant", 10);
		def.animSetOverrides.Add("WormPlant", new CropTendingStates.AnimSet
		{
			crop_tending_pre = "wormwood_tending_pre",
			crop_tending = "wormwood_tending",
			crop_tending_pst = "wormwood_tending_pst",
			hide_symbols_after_pre = new string[]
			{
				"flower",
				"flower_wilted"
			}
		});
		def.ignoreEffectGroup = BaseDivergentConfig.ignoreEffectGroup;
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new GrowUpStates.Def(), is_baby, -1).Add(new TrappedStates.Def(), true, -1).Add(new IncubatingStates.Def(), is_baby, -1).Add(new BaggedStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new FleeStates.Def(), true, -1).Add(new AttackStates.Def("eat_pre", "eat_pst", null), !is_baby && !is_pacifist, -1).PushInterruptGroup().Add(new CreatureSleepStates.Def(), true, -1).Add(new FixedCaptureStates.Def(), true, -1).Add(new RanchedStates.Def(), !is_baby, -1).Add(new LayEggStates.Def(), !is_baby, -1).Add(new EatStates.Def(), true, -1).Add(new DrinkMilkStates.Def(), true, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true, -1).Add(new CallAdultStates.Def(), is_baby, -1).Add(def, !is_baby, -1).PopInterruptGroup().Add(new IdleStates.Def(), true, -1);
		gameObject.AddOrGetDef<DrinkMilkMonitor.Def>();
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.DivergentSpecies, symbolOverridePrefix);
		return gameObject;
	}

	// Token: 0x0600029A RID: 666 RVA: 0x000134D4 File Offset: 0x000116D4
	public static List<Diet.Info> BasicSulfurDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(SimHashes.Sulfur.CreateTag());
		return new List<Diet.Info>
		{
			new Diet.Info(hashSet, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false)
		};
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00013514 File Offset: 0x00011714
	public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diet_infos, float referenceCaloriesPerKg, float minPoopSizeInKg)
	{
		Diet diet = new Diet(diet_infos.ToArray());
		CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minPoopSizeInCalories = referenceCaloriesPerKg * minPoopSizeInKg;
		prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		return prefab;
	}

	// Token: 0x04000191 RID: 401
	public const float CROP_TENDED_MULTIPLIER_DURATION = 600f;

	// Token: 0x04000192 RID: 402
	public const float CROP_TENDED_MULTIPLIER_EFFECT = 0.05f;

	// Token: 0x04000193 RID: 403
	public static string[] ignoreEffectGroup = new string[]
	{
		"DivergentCropTended",
		"DivergentCropTendedWorm"
	};
}
