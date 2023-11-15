using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class MoleConfig : IEntityConfig
{
	// Token: 0x060004FC RID: 1276 RVA: 0x000231F4 File Offset: 0x000213F4
	public static GameObject CreateMole(string id, string name, string desc, string anim_file, bool is_baby = false)
	{
		GameObject gameObject = BaseMoleConfig.BaseMole(id, name, STRINGS.CREATURES.SPECIES.MOLE.DESC, "MoleBaseTrait", anim_file, is_baby, null, 10);
		gameObject.AddTag(GameTags.Creatures.Digger);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MoleTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("MoleBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MoleTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MoleTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet diet = new Diet(BaseMoleConfig.SimpleOreDiet(new List<Tag>
		{
			SimHashes.Regolith.CreateTag(),
			SimHashes.Dirt.CreateTag(),
			SimHashes.IronOre.CreateTag()
		}, MoleConfig.CALORIES_PER_KG_OF_DIRT, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL).ToArray());
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minPoopSizeInCalories = MoleConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
		gameObject.AddOrGet<LoopingSounds>();
		foreach (HashedString hash in MoleTuning.GINGER_SYMBOL_NAMES)
		{
			gameObject.GetComponent<KAnimControllerBase>().SetSymbolVisiblity(hash, false);
		}
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x000233C6 File Offset: 0x000215C6
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x000233D0 File Offset: 0x000215D0
	public GameObject CreatePrefab()
	{
		GameObject prefab = MoleConfig.CreateMole("Mole", STRINGS.CREATURES.SPECIES.MOLE.NAME, STRINGS.CREATURES.SPECIES.MOLE.DESC, "driller_kanim", false);
		string eggId = "MoleEgg";
		string eggName = STRINGS.CREATURES.SPECIES.MOLE.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.MOLE.DESC;
		string egg_anim = "egg_driller_kanim";
		float egg_MASS = MoleTuning.EGG_MASS;
		string baby_id = "MoleBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		int egg_SORT_ORDER = MoleConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, MoleTuning.EGG_CHANCES_BASE, egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x0002344D File Offset: 0x0002164D
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0002344F File Offset: 0x0002164F
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00023458 File Offset: 0x00021658
	public static void SetSpawnNavType(GameObject inst)
	{
		int cell = Grid.PosToCell(inst);
		Navigator component = inst.GetComponent<Navigator>();
		Pickupable component2 = inst.GetComponent<Pickupable>();
		if (component != null && (component2 == null || component2.storage == null))
		{
			if (Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Solid);
				inst.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.FXFront));
				inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.FXFront);
				return;
			}
			inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		}
	}

	// Token: 0x0400035F RID: 863
	public const string ID = "Mole";

	// Token: 0x04000360 RID: 864
	public const string BASE_TRAIT_ID = "MoleBaseTrait";

	// Token: 0x04000361 RID: 865
	public const string EGG_ID = "MoleEgg";

	// Token: 0x04000362 RID: 866
	private static float MIN_POOP_SIZE_IN_CALORIES = 2400000f;

	// Token: 0x04000363 RID: 867
	private static float CALORIES_PER_KG_OF_DIRT = 1000f;

	// Token: 0x04000364 RID: 868
	public static int EGG_SORT_ORDER = 800;
}
