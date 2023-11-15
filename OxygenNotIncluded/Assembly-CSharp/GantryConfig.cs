using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class GantryConfig : IBuildingConfig
{
	// Token: 0x06000841 RID: 2113 RVA: 0x00030448 File Offset: 0x0002E648
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Gantry";
		int width = 6;
		int height = 2;
		string anim = "gantry_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 3200f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 1f);
		buildingDef.ObjectLayer = ObjectLayer.Gantry;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.Entombable = true;
		buildingDef.IsFoundation = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(-2, 0);
		buildingDef.EnergyConsumptionWhenActive = 1200f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.AudioCategory = "Metal";
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(Gantry.PORT_ID, new CellOffset(-1, 1), STRINGS.BUILDINGS.PREFABS.GANTRY.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GANTRY.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GANTRY.LOGIC_PORT_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x00030558 File Offset: 0x0002E758
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00030570 File Offset: 0x0002E770
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Gantry>();
		go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = GantryConfig.SOLID_OFFSETS;
		FakeFloorAdder fakeFloorAdder = go.AddOrGet<FakeFloorAdder>();
		fakeFloorAdder.floorOffsets = new CellOffset[]
		{
			new CellOffset(0, 1),
			new CellOffset(1, 1),
			new CellOffset(2, 1),
			new CellOffset(3, 1)
		};
		fakeFloorAdder.initiallyActive = false;
		UnityEngine.Object.DestroyImmediate(go.GetComponent<LogicOperationalController>());
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x000305F1 File Offset: 0x0002E7F1
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = GantryConfig.SOLID_OFFSETS;
	}

	// Token: 0x0400054A RID: 1354
	public const string ID = "Gantry";

	// Token: 0x0400054B RID: 1355
	private static readonly CellOffset[] SOLID_OFFSETS = new CellOffset[]
	{
		new CellOffset(-2, 1),
		new CellOffset(-1, 1)
	};
}
