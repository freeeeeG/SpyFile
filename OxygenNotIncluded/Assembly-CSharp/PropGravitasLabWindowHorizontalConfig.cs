using System;
using TUNING;
using UnityEngine;

// Token: 0x020002F8 RID: 760
public class PropGravitasLabWindowHorizontalConfig : IBuildingConfig
{
	// Token: 0x06000F60 RID: 3936 RVA: 0x00053459 File Offset: 0x00051659
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x00053460 File Offset: 0x00051660
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PropGravitasLabWindowHorizontal";
		int width = 3;
		int height = 2;
		string anim = "gravitas_lab_window_horizontal_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier_TINY = BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
		string[] glasses = MATERIALS.GLASSES;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier_TINY, glasses, melting_point, build_location_rule, DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DefaultAnimState = "on";
		buildingDef.ObjectLayer = ObjectLayer.Backwall;
		buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x000534F4 File Offset: 0x000516F4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		go.GetComponent<PrimaryElement>().SetElement(SimHashes.Glass, true);
		go.GetComponent<PrimaryElement>().Temperature = 273f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x0005355B File Offset: 0x0005175B
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000882 RID: 2178
	public const string ID = "PropGravitasLabWindowHorizontal";
}
