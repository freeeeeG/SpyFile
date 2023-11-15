using System;
using TUNING;
using UnityEngine;

// Token: 0x02000333 RID: 819
public class SmallSculptureConfig : IBuildingConfig
{
	// Token: 0x060010A7 RID: 4263 RVA: 0x0005A1E4 File Offset: 0x000583E4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SmallSculpture";
		int width = 1;
		int height = 2;
		string anim = "sculpture_1x2_kanim";
		int hitpoints = 10;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 5,
			radius = 4
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

	// Token: 0x060010A8 RID: 4264 RVA: 0x0005A27F File Offset: 0x0005847F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x0005A29E File Offset: 0x0005849E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x0400091E RID: 2334
	public const string ID = "SmallSculpture";
}
