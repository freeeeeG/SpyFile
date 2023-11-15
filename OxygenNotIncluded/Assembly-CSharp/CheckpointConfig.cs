using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class CheckpointConfig : IBuildingConfig
{
	// Token: 0x060000FD RID: 253 RVA: 0x00007D54 File Offset: 0x00005F54
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Checkpoint";
		int width = 1;
		int height = 3;
		string anim = "checkpoint_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.Floodable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 2);
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(Checkpoint.PORT_ID, new CellOffset(0, 2), STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT_INACTIVE, true, false)
		};
		return buildingDef;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00007E25 File Offset: 0x00006025
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Checkpoint>();
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00007E2E File Offset: 0x0000602E
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000091 RID: 145
	public const string ID = "Checkpoint";
}
