﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200009A RID: 154
public static class BaseOilFloaterConfig
{
	// Token: 0x060002BA RID: 698 RVA: 0x000156F8 File Offset: 0x000138F8
	public static GameObject BaseOilFloater(string id, string name, string desc, string anim_file, string traitId, float warnLowTemp, float warnHighTemp, bool is_baby, string symbolOverridePrefix = null)
	{
		float mass = 50f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, (warnLowTemp + warnHighTemp) / 2f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Hoverer, false);
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Pest, traitId, "FloaterNavGrid", NavType.Hover, 32, 2f, "Meat", 2, true, false, warnLowTemp, warnHighTemp, warnLowTemp - 15f, warnHighTemp + 20f);
		if (!string.IsNullOrEmpty(symbolOverridePrefix))
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbolOverridePrefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = CREATURES.SORTING.CRITTER_ORDER["Oilfloater"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGetDef<ThreatMonitor.Def>();
		gameObject.AddOrGetDef<SubmergedMonitor.Def>();
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = true;
		gameObject.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		string inhaleSound = "OilFloater_intake_air";
		if (is_baby)
		{
			inhaleSound = "OilFloaterBaby_intake_air";
		}
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new GrowUpStates.Def(), is_baby, -1).Add(new TrappedStates.Def(), true, -1).Add(new IncubatingStates.Def(), is_baby, -1).Add(new BaggedStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).PushInterruptGroup().Add(new CreatureSleepStates.Def(), true, -1).Add(new FixedCaptureStates.Def(), true, -1).Add(new RanchedStates.Def(), !is_baby, -1).Add(new LayEggStates.Def(), !is_baby, -1).Add(new InhaleStates.Def
		{
			inhaleSound = inhaleSound
		}, true, -1).Add(new DrinkMilkStates.Def(), true, -1).Add(new SameSpotPoopStates.Def(), true, -1).Add(new CallAdultStates.Def(), is_baby, -1).PopInterruptGroup().Add(new IdleStates.Def(), true, -1);
		gameObject.AddOrGetDef<DrinkMilkMonitor.Def>();
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.OilFloaterSpecies, symbolOverridePrefix);
		string sound = "OilFloater_move_LP";
		if (is_baby)
		{
			sound = "OilFloaterBaby_move_LP";
		}
		gameObject.AddOrGet<OilFloaterMovementSound>().sound = sound;
		return gameObject;
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00015964 File Offset: 0x00013B64
	public static GameObject SetupDiet(GameObject prefab, Tag consumed_tag, Tag producedTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced, float minPoopSizeInKg)
	{
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				consumed_tag
			}, producedTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false)
		});
		CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minPoopSizeInCalories = minPoopSizeInKg * caloriesPerKg;
		prefab.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>().diet = diet;
		return prefab;
	}
}
