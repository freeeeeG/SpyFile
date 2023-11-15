using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class GravitasPedestalConfig : IBuildingConfig
{
	// Token: 0x06000902 RID: 2306 RVA: 0x00035274 File Offset: 0x00033474
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0003527C File Offset: 0x0003347C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasPedestal";
		int width = 1;
		int height = 2;
		string anim = "gravitas_pedestal_nice_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.DefaultAnimState = "pedestal_nice";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "small";
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00035304 File Offset: 0x00033504
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
		go.AddOrGet<PedestalArtifactSpawner>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00035385 File Offset: 0x00033585
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040005A4 RID: 1444
	public const string ID = "GravitasPedestal";
}
