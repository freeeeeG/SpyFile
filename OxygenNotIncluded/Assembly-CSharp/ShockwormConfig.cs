using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class ShockwormConfig : IEntityConfig
{
	// Token: 0x0600059E RID: 1438 RVA: 0x00025F36 File Offset: 0x00024136
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00025F40 File Offset: 0x00024140
	public GameObject CreatePrefab()
	{
		string id = "ShockWorm";
		string name = STRINGS.CREATURES.SPECIES.SHOCKWORM.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SHOCKWORM.DESC;
		float mass = 50f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("shockworm_kanim"), "idle", Grid.SceneLayer.Creatures, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		FactionManager.FactionID faction = FactionManager.FactionID.Hostile;
		string initialTraitID = null;
		string navGridName = "FlyerNavGrid1x2";
		NavType navType = NavType.Hover;
		int max_probing_radius = 32;
		float moveSpeed = 2f;
		string onDeathDropID = "Meat";
		int onDeathDropCount = 3;
		bool drownVulnerable = true;
		bool entombVulnerable = true;
		float freezing_ = TUNING.CREATURES.TEMPERATURE.FREEZING_2;
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, faction, initialTraitID, navGridName, navType, max_probing_radius, moveSpeed, onDeathDropID, onDeathDropCount, drownVulnerable, entombVulnerable, TUNING.CREATURES.TEMPERATURE.FREEZING_1, TUNING.CREATURES.TEMPERATURE.HOT_1, freezing_, TUNING.CREATURES.TEMPERATURE.HOT_2);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddWeapon(3f, 6f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.AreaOfEffect, 10, 4f).AddEffect("WasAttacked", 1f);
		SoundEventVolumeCache.instance.AddVolume("shockworm_kanim", "Shockworm_attack_arc", NOISE_POLLUTION.CREATURES.TIER6);
		return gameObject;
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0002601F File Offset: 0x0002421F
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x00026021 File Offset: 0x00024221
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003DF RID: 991
	public const string ID = "ShockWorm";
}
