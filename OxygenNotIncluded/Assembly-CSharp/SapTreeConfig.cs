using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000157 RID: 343
public class SapTreeConfig : IEntityConfig
{
	// Token: 0x060006AD RID: 1709 RVA: 0x0002BBA8 File Offset: 0x00029DA8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x0002BBB0 File Offset: 0x00029DB0
	public GameObject CreatePrefab()
	{
		string id = "SapTree";
		string name = STRINGS.CREATURES.SPECIES.SAPTREE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SAPTREE.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = SapTreeConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_sap_tree_kanim"), "idle", Grid.SceneLayer.BuildingFront, 5, 5, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, new List<Tag>
		{
			GameTags.Decoration
		}, 293f);
		SapTree.Def def = gameObject.AddOrGetDef<SapTree.Def>();
		def.foodSenseArea = new Vector2I(5, 1);
		def.massEatRate = 0.05f;
		def.kcalorieToKGConversionRatio = 0.005f;
		def.stomachSize = 5f;
		def.oozeRate = 2f;
		def.oozeOffsets = new List<Vector3>
		{
			new Vector3(-2f, 2f),
			new Vector3(2f, 1f)
		};
		def.attackSenseArea = new Vector2I(5, 5);
		def.attackCooldown = 5f;
		gameObject.AddOrGet<Storage>();
		FactionAlignment factionAlignment = gameObject.AddOrGet<FactionAlignment>();
		factionAlignment.Alignment = FactionManager.FactionID.Hostile;
		factionAlignment.canBePlayerTargeted = false;
		gameObject.AddOrGet<RangedAttackable>();
		gameObject.AddWeapon(5f, 5f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.AreaOfEffect, 1, 2f);
		gameObject.AddOrGet<WiltCondition>();
		gameObject.AddOrGet<TemperatureVulnerable>().Configure(173.15f, 0f, 373.15f, 1023.15f);
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x0002BD32 File Offset: 0x00029F32
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0002BD34 File Offset: 0x00029F34
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004AC RID: 1196
	public const string ID = "SapTree";

	// Token: 0x040004AD RID: 1197
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER5;

	// Token: 0x040004AE RID: 1198
	private const int WIDTH = 5;

	// Token: 0x040004AF RID: 1199
	private const int HEIGHT = 5;

	// Token: 0x040004B0 RID: 1200
	private const int ATTACK_RADIUS = 2;

	// Token: 0x040004B1 RID: 1201
	public const float MASS_EAT_RATE = 0.05f;

	// Token: 0x040004B2 RID: 1202
	public const float KCAL_TO_KG_RATIO = 0.005f;
}
