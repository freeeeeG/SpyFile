using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class GlomConfig : IEntityConfig
{
	// Token: 0x06000472 RID: 1138 RVA: 0x000212EE File Offset: 0x0001F4EE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x000212F8 File Offset: 0x0001F4F8
	public GameObject CreatePrefab()
	{
		string text = STRINGS.CREATURES.SPECIES.GLOM.NAME;
		string id = "Glom";
		string name = text;
		string desc = STRINGS.CREATURES.SPECIES.GLOM.DESC;
		float mass = 25f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("glom_kanim"), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		Db.Get().CreateTrait("GlomBaseTrait", text, text, null, false, null, true, true).Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, text, false, false, true));
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Creatures.Walker, false);
		component.AddTag(GameTags.OriginalCreature, false);
		component.prefabInitFn += delegate(GameObject inst)
		{
			inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
		};
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Pest, "GlomBaseTrait", "WalkerNavGrid1x1", NavType.Floor, 32, 2f, "", 0, true, true, 293.15f, 393.15f, 273.15f, 423.15f);
		gameObject.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Glom"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<ThreatMonitor.Def>();
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		ElementDropperMonitor.Def def = gameObject.AddOrGetDef<ElementDropperMonitor.Def>();
		def.dirtyEmitElement = SimHashes.ContaminatedOxygen;
		def.dirtyProbabilityPercent = 25f;
		def.dirtyCellToTargetMass = 1f;
		def.dirtyMassPerDirty = 0.2f;
		def.dirtyMassReleaseOnDeath = 3f;
		def.emitDiseaseIdx = Db.Get().Diseases.GetIndex("SlimeLung");
		def.emitDiseasePerKg = 1000f;
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.GetComponent<LoopingSounds>().updatePosition = true;
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_movement_short", NOISE_POLLUTION.CREATURES.TIER2);
		SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_jump", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_land", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_expel", NOISE_POLLUTION.CREATURES.TIER4);
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, false, false);
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new TrappedStates.Def(), true, -1).Add(new BaggedStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new FleeStates.Def(), true, -1).Add(new DropElementStates.Def(), true, -1).Add(new IdleStates.Def(), true, -1);
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.GlomSpecies, null);
		return gameObject;
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x000215F0 File Offset: 0x0001F7F0
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x000215F2 File Offset: 0x0001F7F2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000302 RID: 770
	public const string ID = "Glom";

	// Token: 0x04000303 RID: 771
	public const string BASE_TRAIT_ID = "GlomBaseTrait";

	// Token: 0x04000304 RID: 772
	public const SimHashes dirtyEmitElement = SimHashes.ContaminatedOxygen;

	// Token: 0x04000305 RID: 773
	public const float dirtyProbabilityPercent = 25f;

	// Token: 0x04000306 RID: 774
	public const float dirtyCellToTargetMass = 1f;

	// Token: 0x04000307 RID: 775
	public const float dirtyMassPerDirty = 0.2f;

	// Token: 0x04000308 RID: 776
	public const float dirtyMassReleaseOnDeath = 3f;

	// Token: 0x04000309 RID: 777
	public const string emitDisease = "SlimeLung";

	// Token: 0x0400030A RID: 778
	public const int emitDiseasePerKg = 1000;
}
