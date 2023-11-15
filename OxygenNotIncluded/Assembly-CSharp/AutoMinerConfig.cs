using System;
using TUNING;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class AutoMinerConfig : IBuildingConfig
{
	// Token: 0x0600007F RID: 127 RVA: 0x000054F0 File Offset: 0x000036F0
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AutoMiner", 2, 2, "auto_miner_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "AutoMiner");
		return buildingDef;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00005597 File Offset: 0x00003797
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<Operational>();
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<MiningSounds>();
	}

	// Token: 0x06000081 RID: 129 RVA: 0x000055BF File Offset: 0x000037BF
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		AutoMinerConfig.AddVisualizer(go, true);
	}

	// Token: 0x06000082 RID: 130 RVA: 0x000055C8 File Offset: 0x000037C8
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		AutoMinerConfig.AddVisualizer(go, false);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x000055D4 File Offset: 0x000037D4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		AutoMiner autoMiner = go.AddOrGet<AutoMiner>();
		autoMiner.x = -7;
		autoMiner.y = 0;
		autoMiner.width = 16;
		autoMiner.height = 9;
		autoMiner.vision_offset = new CellOffset(0, 1);
		AutoMinerConfig.AddVisualizer(go, false);
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00005620 File Offset: 0x00003820
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMin.x = -7;
		rangeVisualizer.RangeMin.y = -1;
		rangeVisualizer.RangeMax.x = 8;
		rangeVisualizer.RangeMax.y = 7;
		rangeVisualizer.OriginOffset = new Vector2I(0, 1);
		rangeVisualizer.BlockingTileVisible = false;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(AutoMiner.DigBlockingCB);
		};
	}

	// Token: 0x04000064 RID: 100
	public const string ID = "AutoMiner";

	// Token: 0x04000065 RID: 101
	private const int RANGE = 7;

	// Token: 0x04000066 RID: 102
	private const int X = -7;

	// Token: 0x04000067 RID: 103
	private const int Y = 0;

	// Token: 0x04000068 RID: 104
	private const int WIDTH = 16;

	// Token: 0x04000069 RID: 105
	private const int HEIGHT = 9;

	// Token: 0x0400006A RID: 106
	private const int VISION_OFFSET = 1;
}
