using System;
using TUNING;
using UnityEngine;

// Token: 0x02000383 RID: 899
public class WoodGasGeneratorConfig : IBuildingConfig
{
	// Token: 0x06001286 RID: 4742 RVA: 0x00063744 File Offset: 0x00061944
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WoodGasGenerator";
		int width = 2;
		int height = 2;
		string anim = "generatorwood_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] construction_materials = all_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 300f;
		buildingDef.GeneratorBaseCapacity = 20000f;
		buildingDef.ExhaustKilowattsWhenActive = 8f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		return buildingDef;
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x000637F4 File Offset: 0x000619F4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<LoopingSounds>();
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		float max_stored_input_mass = 720f;
		go.AddOrGet<LoopingSounds>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = WoodLogConfig.TAG;
		manualDeliveryKG.capacity = 360f;
		manualDeliveryKG.refillMass = 180f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
		energyGenerator.powerDistributionOrder = 8;
		energyGenerator.hasMeter = true;
		energyGenerator.formula = EnergyGenerator.CreateSimpleFormula(WoodLogConfig.TAG, 1.2f, max_stored_input_mass, SimHashes.CarbonDioxide, 0.17f, false, new CellOffset(0, 1), 383.15f);
		Tinkerable.MakePowerTinkerable(go);
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000A01 RID: 2561
	public const string ID = "WoodGasGenerator";

	// Token: 0x04000A02 RID: 2562
	private const float BRANCHES_PER_GENERATOR = 8f;

	// Token: 0x04000A03 RID: 2563
	public const float CONSUMPTION_RATE = 1.2f;

	// Token: 0x04000A04 RID: 2564
	private const float WOOD_PER_REFILL = 360f;

	// Token: 0x04000A05 RID: 2565
	private const SimHashes EXHAUST_ELEMENT_GAS = SimHashes.CarbonDioxide;

	// Token: 0x04000A06 RID: 2566
	private const SimHashes EXHAUST_ELEMENT_GAS2 = SimHashes.Syngas;

	// Token: 0x04000A07 RID: 2567
	public const float CO2_EXHAUST_RATE = 0.17f;

	// Token: 0x04000A08 RID: 2568
	private const int WIDTH = 2;

	// Token: 0x04000A09 RID: 2569
	private const int HEIGHT = 2;
}
