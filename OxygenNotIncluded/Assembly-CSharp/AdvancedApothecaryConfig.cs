using System;
using TUNING;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class AdvancedApothecaryConfig : IBuildingConfig
{
	// Token: 0x06000042 RID: 66 RVA: 0x00003B60 File Offset: 0x00001D60
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003B68 File Offset: 0x00001D68
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AdvancedApothecary";
		int width = 3;
		int height = 3;
		string anim = "medicine_nuclear_kanim";
		int hitpoints = 250;
		float construction_time = 240f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 2);
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003C04 File Offset: 0x00001E04
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.capacity = 400f;
		highEnergyParticleStorage.showCapacityStatusItem = true;
		go.AddOrGet<HighEnergyParticlePort>().requireOperational = false;
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		AdvancedApothecary fabricator = go.AddOrGet<AdvancedApothecary>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
		go.AddOrGet<ComplexFabricatorWorkable>();
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		ActiveParticleConsumer.Def def = go.AddOrGetDef<ActiveParticleConsumer.Def>();
		def.activeConsumptionRate = 1f;
		def.minParticlesForOperational = 1f;
		def.meterSymbolName = null;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003C99 File Offset: 0x00001E99
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x04000034 RID: 52
	public const string ID = "AdvancedApothecary";

	// Token: 0x04000035 RID: 53
	public const float PARTICLE_CAPACITY = 400f;
}
