using System;
using TUNING;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class IceSculptureConfig : IBuildingConfig
{
	// Token: 0x06000962 RID: 2402 RVA: 0x0003788C File Offset: 0x00035A8C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "IceSculpture";
		int width = 2;
		int height = 2;
		string anim = "icesculpture_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] construction_materials = new string[]
		{
			"Ice"
		};
		float melting_point = 273.15f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, new EffectorValues
		{
			amount = 20,
			radius = 8
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00037931 File Offset: 0x00035B31
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00037950 File Offset: 0x00035B50
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x040005E9 RID: 1513
	public const string ID = "IceSculpture";
}
