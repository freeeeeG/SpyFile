using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class FishTrapConfig : IBuildingConfig
{
	// Token: 0x060005F6 RID: 1526 RVA: 0x000277BC File Offset: 0x000259BC
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishTrap", 1, 2, "fishtrap_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.PLASTICS, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Floodable = false;
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0002781C File Offset: 0x00025A1C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(FishTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Swimmer
		};
		trapTrigger.trappedOffset = new Vector2(0f, 1f);
		go.AddOrGet<Trap>();
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x00027884 File Offset: 0x00025A84
	public override void DoPostConfigureComplete(GameObject go)
	{
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		def.initialLures = new Tag[]
		{
			GameTags.Creatures.FishTrapLure
		};
	}

	// Token: 0x0400042B RID: 1067
	public const string ID = "FishTrap";

	// Token: 0x0400042C RID: 1068
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}
