using System;
using TUNING;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class CrownMouldingConfig : IBuildingConfig
{
	// Token: 0x060001A0 RID: 416 RVA: 0x0000BCBC File Offset: 0x00009EBC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CrownMoulding";
		int width = 1;
		int height = 1;
		string anim = "crown_moulding_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 5,
			radius = 3
		}, none, 0.2f);
		buildingDef.DefaultAnimState = "S_U";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		return buildingDef;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000BD50 File Offset: 0x00009F50
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000BD6A File Offset: 0x00009F6A
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040000FC RID: 252
	public const string ID = "CrownMoulding";
}
