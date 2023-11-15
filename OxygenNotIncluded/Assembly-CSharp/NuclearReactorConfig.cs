using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class NuclearReactorConfig : IBuildingConfig
{
	// Token: 0x06000DEF RID: 3567 RVA: 0x0004D220 File Offset: 0x0004B420
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x0004D228 File Offset: 0x0004B428
	public override BuildingDef CreateBuildingDef()
	{
		string id = "NuclearReactor";
		int width = 5;
		int height = 6;
		string anim = "generatornuclear_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 0f;
		buildingDef.GeneratorBaseCapacity = 10000f;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.ThermalConductivity = 0.1f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Overheatable = false;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.UtilityInputOffset = new CellOffset(-2, 2);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("CONTROL_FUEL_DELIVERY", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.INPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.INPUT_PORT_INACTIVE, false, true)
		};
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Breakable = false;
		buildingDef.Invincible = true;
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x0004D36C File Offset: 0x0004B56C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		UnityEngine.Object.Destroy(go.GetComponent<BuildingEnabledButton>());
		RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = 25;
		radiationEmitter.emitRadiusY = 25;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emissionOffset = new Vector3(0f, 2f, 0f);
		Storage storage = go.AddComponent<Storage>();
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.RequestedItemTag = ElementLoader.FindElementByHash(SimHashes.EnrichedUranium).tag;
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
		manualDeliveryKG.capacity = 180f;
		manualDeliveryKG.MinimumMass = 0.5f;
		go.AddOrGet<Reactor>();
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 90f;
		conduitConsumer.capacityTag = GameTags.AnyWater;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x0004D4EA File Offset: 0x0004B6EA
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddTag(GameTags.CorrosionProof);
	}

	// Token: 0x040007F8 RID: 2040
	public const string ID = "NuclearReactor";

	// Token: 0x040007F9 RID: 2041
	private const float FUEL_CAPACITY = 180f;

	// Token: 0x040007FA RID: 2042
	public const float VENT_STEAM_TEMPERATURE = 673.15f;

	// Token: 0x040007FB RID: 2043
	public const float MELT_DOWN_TEMPERATURE = 3000f;

	// Token: 0x040007FC RID: 2044
	public const float MAX_VENT_PRESSURE = 150f;

	// Token: 0x040007FD RID: 2045
	public const float INCREASED_CONDUCTION_SCALE = 5f;

	// Token: 0x040007FE RID: 2046
	public const float REACTION_STRENGTH = 100f;

	// Token: 0x040007FF RID: 2047
	public const int RADIATION_EMITTER_RANGE = 25;

	// Token: 0x04000800 RID: 2048
	public const float OPERATIONAL_RADIATOR_INTENSITY = 2400f;

	// Token: 0x04000801 RID: 2049
	public const float MELT_DOWN_RADIATOR_INTENSITY = 4800f;

	// Token: 0x04000802 RID: 2050
	public const float FUEL_CONSUMPTION_SPEED = 0.016666668f;

	// Token: 0x04000803 RID: 2051
	public const float BEGIN_REACTION_MASS = 0.5f;

	// Token: 0x04000804 RID: 2052
	public const float STOP_REACTION_MASS = 0.25f;

	// Token: 0x04000805 RID: 2053
	public const float DUMP_WASTE_AMOUNT = 100f;

	// Token: 0x04000806 RID: 2054
	public const float WASTE_MASS_MULTIPLIER = 100f;

	// Token: 0x04000807 RID: 2055
	public const float REACTION_MASS_TARGET = 60f;

	// Token: 0x04000808 RID: 2056
	public const float COOLANT_AMOUNT = 30f;

	// Token: 0x04000809 RID: 2057
	public const float COOLANT_CAPACITY = 90f;

	// Token: 0x0400080A RID: 2058
	public const float MINIMUM_COOLANT_MASS = 30f;

	// Token: 0x0400080B RID: 2059
	public const float WASTE_GERMS_PER_KG = 50f;

	// Token: 0x0400080C RID: 2060
	public const float PST_MELTDOWN_COOLING_TIME = 3000f;

	// Token: 0x0400080D RID: 2061
	public const string INPUT_PORT_ID = "CONTROL_FUEL_DELIVERY";
}
