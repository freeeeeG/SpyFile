﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class PropGravitasLabWindowConfig : IBuildingConfig
{
	// Token: 0x06000F5B RID: 3931 RVA: 0x0005334D File Offset: 0x0005154D
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x00053354 File Offset: 0x00051554
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PropGravitasLabWindow";
		int width = 2;
		int height = 3;
		string anim = "gravitas_lab_window_kanim";
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

	// Token: 0x06000F5D RID: 3933 RVA: 0x000533E8 File Offset: 0x000515E8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		go.GetComponent<PrimaryElement>().SetElement(SimHashes.Glass, true);
		go.GetComponent<PrimaryElement>().Temperature = 273f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x0005344F File Offset: 0x0005164F
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000881 RID: 2177
	public const string ID = "PropGravitasLabWindow";
}
