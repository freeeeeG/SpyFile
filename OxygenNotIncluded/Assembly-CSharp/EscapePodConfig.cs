using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class EscapePodConfig : IEntityConfig
{
	// Token: 0x06000263 RID: 611 RVA: 0x00011276 File Offset: 0x0000F476
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00011280 File Offset: 0x0000F480
	public GameObject CreatePrefab()
	{
		string id = "EscapePod";
		string name = STRINGS.BUILDINGS.PREFABS.ESCAPEPOD.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.ESCAPEPOD.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("escape_pod_kanim"), "grounded", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<KBatchedAnimController>().fgLayer = Grid.SceneLayer.BuildingFront;
		TravellingCargoLander.Def def = gameObject.AddOrGetDef<TravellingCargoLander.Def>();
		def.landerWidth = 1;
		def.landingSpeed = 15f;
		def.deployOnLanding = true;
		CargoDropperMinion.Def def2 = gameObject.AddOrGetDef<CargoDropperMinion.Def>();
		def2.kAnimName = "anim_interacts_escape_pod_kanim";
		def2.animName = "deploying";
		def2.animLayer = Grid.SceneLayer.BuildingUse;
		def2.notifyOnJettison = true;
		BallisticClusterGridEntity ballisticClusterGridEntity = gameObject.AddOrGet<BallisticClusterGridEntity>();
		ballisticClusterGridEntity.clusterAnimName = "escape_pod01_kanim";
		ballisticClusterGridEntity.isWorldEntity = true;
		ballisticClusterGridEntity.nameKey = new StringKey("STRINGS.BUILDINGS.PREFABS.ESCAPEPOD.NAME");
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		clusterDestinationSelector.canNavigateFogOfWar = true;
		gameObject.AddOrGet<ClusterTraveler>();
		gameObject.AddOrGet<MinionStorage>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Deconstructable>().audioSize = "large";
		return gameObject;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x000113B0 File Offset: 0x0000F5B0
	public void OnPrefabInit(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		component.ApplyToCells = false;
		component.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000266 RID: 614 RVA: 0x000113CE File Offset: 0x0000F5CE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000159 RID: 345
	public const string ID = "EscapePod";

	// Token: 0x0400015A RID: 346
	public const float MASS = 100f;
}
