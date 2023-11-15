using System;
using TUNING;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class ArtifactAnalysisStationConfig : IBuildingConfig
{
	// Token: 0x0600006D RID: 109 RVA: 0x00004DE8 File Offset: 0x00002FE8
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00004DF0 File Offset: 0x00002FF0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ArtifactAnalysisStation";
		int width = 4;
		int height = 4;
		string anim = "artifact_analysis_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00004E74 File Offset: 0x00003074
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<ArtifactAnalysisStation.Def>();
		go.AddOrGet<ArtifactAnalysisStationWorkable>();
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.RequestedItemTag = GameTags.CharmedArtifact;
		manualDeliveryKG.refillMass = 1f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.capacity = 1f;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00004F01 File Offset: 0x00003101
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000058 RID: 88
	public const string ID = "ArtifactAnalysisStation";

	// Token: 0x04000059 RID: 89
	public const float WORK_TIME = 150f;
}
