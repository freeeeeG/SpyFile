using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public class ItemPedestalConfig : IBuildingConfig
{
	// Token: 0x060009BD RID: 2493 RVA: 0x00038644 File Offset: 0x00036844
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ItemPedestal";
		int width = 1;
		int height = 2;
		string anim = "pedestal_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.DefaultAnimState = "pedestal";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "small";
		return buildingDef;
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x000386C4 File Offset: 0x000368C4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>(new Storage.StoredItemModifier[]
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Preserve
		}));
		Prioritizable.AddRef(go);
		SingleEntityReceptacle singleEntityReceptacle = go.AddOrGet<SingleEntityReceptacle>();
		singleEntityReceptacle.AddDepositTag(GameTags.PedestalDisplayable);
		singleEntityReceptacle.occupyingObjectRelativePosition = new Vector3(0f, 1.2f, -1f);
		go.AddOrGet<DecorProvider>();
		go.AddOrGet<ItemPedestal>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0003873E File Offset: 0x0003693E
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000615 RID: 1557
	public const string ID = "ItemPedestal";
}
