using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000367 RID: 871
public class ThermalBlockConfig : IBuildingConfig
{
	// Token: 0x060011CF RID: 4559 RVA: 0x000606AC File Offset: 0x0005E8AC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ThermalBlock";
		int width = 1;
		int height = 1;
		string anim = "thermalblock_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] any_BUILDABLE = MATERIALS.ANY_BUILDABLE;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, any_BUILDABLE, melting_point, build_location_rule, DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.DefaultAnimState = "off";
		buildingDef.ObjectLayer = ObjectLayer.Backwall;
		buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
		buildingDef.ReplacementLayer = ObjectLayer.ReplacementBackwall;
		buildingDef.ReplacementCandidateLayers = new List<ObjectLayer>
		{
			ObjectLayer.FoundationTile,
			ObjectLayer.Backwall
		};
		buildingDef.ReplacementTags = new List<Tag>
		{
			GameTags.FloorTiles,
			GameTags.Backwall
		};
		return buildingDef;
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x0006077D File Offset: 0x0005E97D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		go.AddComponent<ZoneTile>();
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x000607A7 File Offset: 0x0005E9A7
	public override void DoPostConfigureComplete(GameObject go)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Backwall, false);
		component.prefabSpawnFn += delegate(GameObject game_object)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
			StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
			int cell = Grid.PosToCell(game_object);
			payload.OverrideExtents(new Extents(cell, ThermalBlockConfig.overrideOffsets));
			GameComps.StructureTemperatures.SetPayload(handle, ref payload);
		};
	}

	// Token: 0x040009B4 RID: 2484
	public const string ID = "ThermalBlock";

	// Token: 0x040009B5 RID: 2485
	private static readonly CellOffset[] overrideOffsets = new CellOffset[]
	{
		new CellOffset(-1, -1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(1, 1)
	};
}
