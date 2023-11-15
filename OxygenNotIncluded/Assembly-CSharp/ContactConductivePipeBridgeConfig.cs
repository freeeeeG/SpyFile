using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class ContactConductivePipeBridgeConfig : IBuildingConfig
{
	// Token: 0x0600015E RID: 350 RVA: 0x00009908 File Offset: 0x00007B08
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ContactConductivePipeBridge";
		int width = 3;
		int height = 1;
		string anim = "contactConductivePipeBridge_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.NoLiquidConduitAtOrigin;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.ObjectLayer = ObjectLayer.LiquidConduitConnection;
		buildingDef.SceneLayer = Grid.SceneLayer.LiquidConduitBridges;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.UseStructureTemperature = true;
		buildingDef.ReplacementTags = new List<Tag>();
		buildingDef.ReplacementTags.Add(GameTags.Pipes);
		buildingDef.ThermalConductivity = 2f;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "ContactConductivePipeBridge");
		return buildingDef;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00009A0A File Offset: 0x00007C0A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<StructureToStructureTemperature>();
		ContactConductivePipeBridge.Def def = go.AddOrGetDef<ContactConductivePipeBridge.Def>();
		def.pumpKGRate = 10f;
		def.type = ConduitType.Liquid;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00009A3F File Offset: 0x00007C3F
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireOutputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
	}

	// Token: 0x040000C2 RID: 194
	public const float LIQUID_CAPACITY_KG = 10f;

	// Token: 0x040000C3 RID: 195
	public const float GAS_CAPACITY_KG = 0.5f;

	// Token: 0x040000C4 RID: 196
	public const float TEMPERATURE_EXCHANGE_WITH_STORAGE_MODIFIER = 50f;

	// Token: 0x040000C5 RID: 197
	public const float BUILDING_TO_BUILDING_TEMPERATURE_SCALE = 0.001f;

	// Token: 0x040000C6 RID: 198
	public const string ID = "ContactConductivePipeBridge";

	// Token: 0x040000C7 RID: 199
	public const float NO_LIQUIDS_COOLDOWN = 1.5f;

	// Token: 0x040000C8 RID: 200
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
}
