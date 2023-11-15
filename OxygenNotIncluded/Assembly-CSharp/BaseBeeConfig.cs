﻿using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000087 RID: 135
public static class BaseBeeConfig
{
	// Token: 0x0600028A RID: 650 RVA: 0x00012110 File Offset: 0x00010310
	public static GameObject BaseBee(string id, string name, string desc, string anim_file, string traitId, EffectorValues decor, bool is_baby, string symbolOverridePrefix = null)
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, 5f, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, default(EffectorValues), SimHashes.Creature, null, CREATURES.TEMPERATURE.FREEZING_3);
		string text = "FlyerNavGrid1x1";
		NavType navType = NavType.Hover;
		int num = 5;
		if (is_baby)
		{
			text = "WalkerBabyNavGrid";
			navType = NavType.Floor;
			num = 1;
		}
		GameObject template = gameObject;
		FactionManager.FactionID faction = FactionManager.FactionID.Hostile;
		string navGridName = text;
		NavType navType2 = navType;
		int max_probing_radius = 32;
		float moveSpeed = (float)num;
		string onDeathDropID = "Meat";
		int onDeathDropCount = 0;
		bool drownVulnerable = true;
		bool entombVulnerable = true;
		float freezing_ = CREATURES.TEMPERATURE.FREEZING_10;
		EntityTemplates.ExtendEntityToBasicCreature(template, faction, traitId, navGridName, navType2, max_probing_radius, moveSpeed, onDeathDropID, onDeathDropCount, drownVulnerable, entombVulnerable, CREATURES.TEMPERATURE.FREEZING_9, CREATURES.TEMPERATURE.FREEZING_1, freezing_, CREATURES.TEMPERATURE.FREEZING);
		if (symbolOverridePrefix != null)
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbolOverridePrefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = CREATURES.SORTING.CRITTER_ORDER["Bee"];
		pickupable.sortOrder = sortOrder;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.prefabInitFn += delegate(GameObject inst)
		{
			inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
		};
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGetDef<ThreatMonitor.Def>();
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, false, false);
		gameObject.AddOrGetDef<AgeMonitor.Def>();
		Bee bee = gameObject.AddOrGet<Bee>();
		RadiationEmitter radiationEmitter = gameObject.AddComponent<RadiationEmitter>();
		radiationEmitter.emitRate = 0.1f;
		if (!is_baby)
		{
			component.AddTag(GameTags.Creatures.Flyer, false);
			bee.radiationOutputAmount = 240f;
			radiationEmitter.radiusProportionalToRads = false;
			radiationEmitter.emitRadiusX = 3;
			radiationEmitter.emitRadiusY = 3;
			radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			gameObject.AddOrGetDef<SubmergedMonitor.Def>();
			gameObject.AddWeapon(2f, 3f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		}
		else
		{
			bee.radiationOutputAmount = 120f;
			radiationEmitter.radiusProportionalToRads = false;
			radiationEmitter.emitRadiusX = 2;
			radiationEmitter.emitRadiusY = 2;
			radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
			gameObject.AddOrGetDef<BeeHiveMonitor.Def>();
			gameObject.AddOrGet<Trappable>();
			EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		}
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = CREATURES.SPACE_REQUIREMENTS.TIER1;
		gameObject.AddOrGetDef<BeeHappinessMonitor.Def>();
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
		elementConsumer.consumptionRate = 0.1f;
		elementConsumer.consumptionRadius = 3;
		elementConsumer.showInStatusPanel = true;
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
		elementConsumer.isRequired = false;
		elementConsumer.storeOnConsume = false;
		elementConsumer.showDescriptor = true;
		elementConsumer.EnableConsumption(false);
		gameObject.AddOrGetDef<BeeSleepMonitor.Def>();
		gameObject.AddOrGetDef<BeeForagingMonitor.Def>();
		gameObject.AddOrGet<Storage>();
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new GrowUpStates.Def(), true, -1).Add(new TrappedStates.Def(), true, -1).Add(new BaggedStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new BeeSleepStates.Def(), true, -1).Add(new FleeStates.Def(), true, -1).Add(new AttackStates.Def("attack_pre", "attack_pst", new CellOffset[]
		{
			new CellOffset(0, 1),
			new CellOffset(1, 1),
			new CellOffset(-1, 1)
		}), !is_baby, -1).Add(new FixedCaptureStates.Def(), true, -1).Add(new BeeMakeHiveStates.Def(), true, -1).Add(new BeeForageStates.Def(SimHashes.UraniumOre.CreateTag(), BeeHiveTuning.ORE_DELIVERY_AMOUNT), true, -1).Add(new BuzzStates.Def(), true, -1);
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.BeetaSpecies, symbolOverridePrefix);
		return gameObject;
	}

	// Token: 0x0600028B RID: 651 RVA: 0x000124A0 File Offset: 0x000106A0
	public static void SetupLoopingSounds(GameObject inst)
	{
		inst.GetComponent<LoopingSounds>().StartSound(GlobalAssets.GetSound("Bee_wings_LP", false));
	}
}
