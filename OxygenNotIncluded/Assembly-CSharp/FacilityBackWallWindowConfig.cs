using System;
using TUNING;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class FacilityBackWallWindowConfig : IBuildingConfig
{
	// Token: 0x0600027B RID: 635 RVA: 0x00011C28 File Offset: 0x0000FE28
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FacilityBackWallWindow";
		int width = 1;
		int height = 6;
		string anim = "gravitas_window_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] glasses = MATERIALS.GLASSES;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, glasses, melting_point, build_location_rule, DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DefaultAnimState = "off";
		buildingDef.ObjectLayer = ObjectLayer.Backwall;
		buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00011CBC File Offset: 0x0000FEBC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		go.GetComponent<PrimaryElement>().SetElement(SimHashes.Glass, true);
		go.GetComponent<PrimaryElement>().Temperature = 273f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00011D23 File Offset: 0x0000FF23
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000166 RID: 358
	public const string ID = "FacilityBackWallWindow";
}
