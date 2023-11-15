using System;
using TUNING;
using UnityEngine;

// Token: 0x02000345 RID: 837
public class SolidTransferArmConfig : IBuildingConfig
{
	// Token: 0x06001101 RID: 4353 RVA: 0x0005BB6C File Offset: 0x00059D6C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SolidTransferArm", 3, 1, "conveyor_transferarm_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidTransferArm");
		return buildingDef;
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x0005BC12 File Offset: 0x00059E12
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Operational>();
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0005BC22 File Offset: 0x00059E22
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		SolidTransferArmConfig.AddVisualizer(go, true);
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x0005BC2B File Offset: 0x00059E2B
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		SolidTransferArmConfig.AddVisualizer(go, false);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x0005BC53 File Offset: 0x00059E53
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<SolidTransferArm>().pickupRange = 4;
		SolidTransferArmConfig.AddVisualizer(go, false);
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x0005BC70 File Offset: 0x00059E70
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.OriginOffset = new Vector2I(0, 0);
		rangeVisualizer.RangeMin.x = -4;
		rangeVisualizer.RangeMin.y = -4;
		rangeVisualizer.RangeMax.x = 4;
		rangeVisualizer.RangeMax.y = 4;
		rangeVisualizer.BlockingTileVisible = true;
	}

	// Token: 0x0400093E RID: 2366
	public const string ID = "SolidTransferArm";

	// Token: 0x0400093F RID: 2367
	private const int RANGE = 4;
}
