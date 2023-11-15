﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000096 RID: 150
public static class BaseMoleConfig
{
	// Token: 0x060002B0 RID: 688 RVA: 0x00014D34 File Offset: 0x00012F34
	public static GameObject BaseMole(string id, string name, string desc, string traitId, string anim_file, bool is_baby, string symbolOverridePrefix = null, int on_death_drop_count = 10)
	{
		float mass = 25f;
		EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, none, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Pest, traitId, "DiggerNavGrid", NavType.Floor, 32, 2f, "Meat", on_death_drop_count, true, false, 123.149994f, 673.15f, 73.149994f, 773.15f);
		if (symbolOverridePrefix != null)
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbolOverridePrefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Mole"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<DiggerMonitor.Def>().depthToDig = MoleTuning.DEPTH_TO_HIDE;
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker, false);
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new DiggerStates.Def(), true, -1).Add(new GrowUpStates.Def(), is_baby, -1).Add(new TrappedStates.Def(), true, -1).Add(new IncubatingStates.Def(), is_baby, -1).Add(new BaggedStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new FleeStates.Def(), true, -1).Add(new AttackStates.Def("eat_pre", "eat_pst", null), !is_baby, -1).PushInterruptGroup().Add(new FixedCaptureStates.Def(), true, -1).Add(new RanchedStates.Def(), !is_baby, -1).Add(new LayEggStates.Def(), !is_baby, -1).Add(new CreatureSleepStates.Def(), true, -1).Add(new EatStates.Def(), true, -1).Add(new DrinkMilkStates.Def
		{
			shouldBeBehindMilkTank = is_baby
		}, true, -1).Add(new NestingPoopState.Def(is_baby ? Tag.Invalid : SimHashes.Regolith.CreateTag()), true, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true, -1).PopInterruptGroup().Add(new IdleStates.Def
		{
			customIdleAnim = new IdleStates.Def.IdleAnimCallback(BaseMoleConfig.CustomIdleAnim)
		}, true, -1);
		gameObject.AddOrGetDef<DrinkMilkMonitor.Def>();
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.MoleSpecies, symbolOverridePrefix);
		return gameObject;
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00014FBC File Offset: 0x000131BC
	public static List<Diet.Info> SimpleOreDiet(List<Tag> elementTags, float caloriesPerKg, float producedConversionRate)
	{
		List<Diet.Info> list = new List<Diet.Info>();
		foreach (Tag tag in elementTags)
		{
			list.Add(new Diet.Info(new HashSet<Tag>
			{
				tag
			}, tag, caloriesPerKg, producedConversionRate, null, 0f, true, false));
		}
		return list;
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00015030 File Offset: 0x00013230
	private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
	{
		if (smi.gameObject.GetComponent<Navigator>().CurrentNavType == NavType.Solid)
		{
			int num = UnityEngine.Random.Range(0, BaseMoleConfig.SolidIdleAnims.Length);
			return BaseMoleConfig.SolidIdleAnims[num];
		}
		if (smi.gameObject.GetDef<BabyMonitor.Def>() != null && UnityEngine.Random.Range(0, 100) >= 90)
		{
			return "drill_fail";
		}
		return "idle_loop";
	}

	// Token: 0x040001BB RID: 443
	private static readonly string[] SolidIdleAnims = new string[]
	{
		"idle1",
		"idle2",
		"idle3",
		"idle4"
	};
}
