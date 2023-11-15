using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000327 RID: 807
public class ScoutLanderConfig : IEntityConfig
{
	// Token: 0x0600106A RID: 4202 RVA: 0x00058FD5 File Offset: 0x000571D5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x00058FDC File Offset: 0x000571DC
	public GameObject CreatePrefab()
	{
		string id = "ScoutLander";
		string name = STRINGS.BUILDINGS.PREFABS.SCOUTLANDER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.SCOUTLANDER.DESC;
		float mass = 400f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_scout_cargo_lander_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddOrGetDef<CargoLander.Def>().previewTag = "ScoutLander_Preview".ToTag();
		gameObject.AddOrGetDef<CargoDropperStorage.Def>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<Operational>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.showInUI = true;
		storage.allowItemRemoval = false;
		storage.capacityKg = 2000f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		gameObject.AddOrGet<Deconstructable>().audioSize = "large";
		gameObject.AddOrGet<Storable>();
		Placeable placeable = gameObject.AddOrGet<Placeable>();
		placeable.kAnimName = "rocket_scout_cargo_lander_kanim";
		placeable.animName = "place";
		placeable.placementRules = new List<Placeable.PlacementRules>
		{
			Placeable.PlacementRules.OnFoundation,
			Placeable.PlacementRules.VisibleToSpace,
			Placeable.PlacementRules.RestrictToWorld
		};
		placeable.checkRootCellOnly = true;
		EntityTemplates.CreateAndRegisterPreview("ScoutLander_Preview", Assets.GetAnim("rocket_scout_cargo_lander_kanim"), "place", ObjectLayer.Building, 3, 3);
		return gameObject;
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x00059111 File Offset: 0x00057311
	public void OnPrefabInit(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		component.ApplyToCells = false;
		component.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x0005912F File Offset: 0x0005732F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400090D RID: 2317
	public const string ID = "ScoutLander";

	// Token: 0x0400090E RID: 2318
	public const string PREVIEW_ID = "ScoutLander_Preview";

	// Token: 0x0400090F RID: 2319
	public const float MASS = 400f;
}
