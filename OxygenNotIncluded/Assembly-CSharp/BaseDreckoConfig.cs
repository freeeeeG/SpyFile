﻿using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200008F RID: 143
public static class BaseDreckoConfig
{
	// Token: 0x0600029E RID: 670 RVA: 0x00013660 File Offset: 0x00011860
	public static GameObject BaseDrecko(string id, string name, string desc, string anim_file, string trait_id, bool is_baby, string symbol_override_prefix, float warnLowTemp, float warnHighTemp)
	{
		float mass = 200f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, (warnLowTemp + warnHighTemp) / 2f);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Creatures.Walker, false);
		component.prefabInitFn += delegate(GameObject inst)
		{
			inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
		};
		string navGridName = "DreckoNavGrid";
		if (is_baby)
		{
			navGridName = "DreckoBabyNavGrid";
		}
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Pest, trait_id, navGridName, NavType.Floor, 32, 1f, "Meat", 2, true, false, warnLowTemp, warnHighTemp, warnLowTemp - 20f, warnHighTemp + 20f);
		if (!string.IsNullOrEmpty(symbol_override_prefix))
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbol_override_prefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Drecko"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
		gameObject.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new GrowUpStates.Def(), is_baby, -1).Add(new TrappedStates.Def(), true, -1).Add(new IncubatingStates.Def(), is_baby, -1).Add(new BaggedStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new FleeStates.Def(), true, -1).Add(new AttackStates.Def("eat_pre", "eat_pst", null), !is_baby, -1).PushInterruptGroup().Add(new FixedCaptureStates.Def(), true, -1).Add(new RanchedStates.Def(), !is_baby, -1).Add(new LayEggStates.Def(), !is_baby, -1).Add(new EatStates.Def(), true, -1).Add(new DrinkMilkStates.Def
		{
			shouldBeBehindMilkTank = is_baby
		}, true, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true, -1).Add(new CallAdultStates.Def(), is_baby, -1).PopInterruptGroup().Add(new CreatureSleepStates.Def(), true, -1).Add(new IdleStates.Def
		{
			customIdleAnim = new IdleStates.Def.IdleAnimCallback(BaseDreckoConfig.CustomIdleAnim)
		}, true, -1);
		gameObject.AddOrGetDef<DrinkMilkMonitor.Def>();
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.DreckoSpecies, symbol_override_prefix);
		return gameObject;
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00013920 File Offset: 0x00011B20
	private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
	{
		CellOffset offset = new CellOffset(0, -1);
		bool facing = smi.GetComponent<Facing>().GetFacing();
		NavType currentNavType = smi.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType != NavType.Floor)
		{
			if (currentNavType == NavType.Ceiling)
			{
				offset = (facing ? new CellOffset(1, 1) : new CellOffset(-1, 1));
			}
		}
		else
		{
			offset = (facing ? new CellOffset(1, -1) : new CellOffset(-1, -1));
		}
		HashedString result = "idle_loop";
		int num = Grid.OffsetCell(Grid.PosToCell(smi), offset);
		if (Grid.IsValidCell(num) && !Grid.Solid[num])
		{
			pre_anim = "idle_loop_hang_pre";
			result = "idle_loop_hang";
		}
		return result;
	}
}
