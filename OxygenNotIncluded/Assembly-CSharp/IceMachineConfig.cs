using System;
using TUNING;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class IceMachineConfig : IBuildingConfig
{
	// Token: 0x0600095E RID: 2398 RVA: 0x00037700 File Offset: 0x00035900
	public override BuildingDef CreateBuildingDef()
	{
		string id = "IceMachine";
		int width = 2;
		int height = 3;
		string anim = "freezerator_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = this.energyConsumption;
		buildingDef.ExhaustKilowattsWhenActive = 4f;
		buildingDef.SelfHeatKilowattsWhenActive = 12f;
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x00037788 File Offset: 0x00035988
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		storage.showInUI = true;
		storage.capacityKg = 30f;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		storage2.showInUI = true;
		storage2.capacityKg = 150f;
		storage2.allowItemRemoval = true;
		storage2.ignoreSourcePriority = true;
		storage2.allowUIItemRemoval = true;
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		IceMachine iceMachine = go.AddOrGet<IceMachine>();
		iceMachine.SetStorages(storage, storage2);
		iceMachine.targetTemperature = 253.15f;
		iceMachine.heatRemovalRate = 20f;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = GameTags.Water;
		manualDeliveryKG.capacity = 30f;
		manualDeliveryKG.refillMass = 6f;
		manualDeliveryKG.MinimumMass = 10f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00037874 File Offset: 0x00035A74
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040005DF RID: 1503
	public const string ID = "IceMachine";

	// Token: 0x040005E0 RID: 1504
	private const float WATER_STORAGE = 30f;

	// Token: 0x040005E1 RID: 1505
	private const float ICE_STORAGE = 150f;

	// Token: 0x040005E2 RID: 1506
	private const float WATER_INPUT_RATE = 0.5f;

	// Token: 0x040005E3 RID: 1507
	private const float ICE_OUTPUT_RATE = 0.5f;

	// Token: 0x040005E4 RID: 1508
	private const float ICE_PER_LOAD = 30f;

	// Token: 0x040005E5 RID: 1509
	private const float TARGET_ICE_TEMP = 253.15f;

	// Token: 0x040005E6 RID: 1510
	private const float KDTU_TRANSFER_RATE = 20f;

	// Token: 0x040005E7 RID: 1511
	private const float THERMAL_CONSERVATION = 0.8f;

	// Token: 0x040005E8 RID: 1512
	private float energyConsumption = 60f;
}
