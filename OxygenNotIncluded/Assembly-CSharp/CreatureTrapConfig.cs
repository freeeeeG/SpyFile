using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class CreatureTrapConfig : IBuildingConfig
{
	// Token: 0x06000197 RID: 407 RVA: 0x0000BA90 File Offset: 0x00009C90
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureTrap", 2, 1, "creaturetrap_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.PLASTICS, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Floodable = false;
		return buildingDef;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000BAF4 File Offset: 0x00009CF4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(CreatureTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Walker,
			GameTags.Creatures.Hoverer
		};
		trapTrigger.trappedOffset = new Vector2(0.5f, 0f);
		go.AddOrGet<Trap>();
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000BB66 File Offset: 0x00009D66
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040000F9 RID: 249
	public const string ID = "CreatureTrap";

	// Token: 0x040000FA RID: 250
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}
