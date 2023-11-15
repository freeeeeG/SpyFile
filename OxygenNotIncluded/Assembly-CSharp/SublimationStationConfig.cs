using System;
using TUNING;
using UnityEngine;

// Token: 0x02000359 RID: 857
public class SublimationStationConfig : IBuildingConfig
{
	// Token: 0x06001187 RID: 4487 RVA: 0x0005E441 File Offset: 0x0005C641
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x0005E448 File Offset: 0x0005C648
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SublimationStation";
		int width = 2;
		int height = 1;
		string anim = "sublimation_station_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.Breakable = true;
		return buildingDef;
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x0005E4E8 File Offset: 0x0005C6E8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		CellOffset cellOffset = new CellOffset(0, 0);
		Electrolyzer electrolyzer = go.AddOrGet<Electrolyzer>();
		electrolyzer.maxMass = 1.8f;
		electrolyzer.hasMeter = false;
		electrolyzer.emissionOffset = cellOffset;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 600f;
		storage.showInUI = true;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(SimHashes.ToxicSand.CreateTag(), 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.66f, SimHashes.ContaminatedOxygen, 303.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true)
		};
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = SimHashes.ToxicSand.CreateTag();
		manualDeliveryKG.capacity = 600f;
		manualDeliveryKG.refillMass = 240f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
	}

	// Token: 0x0600118A RID: 4490 RVA: 0x0005E5F9 File Offset: 0x0005C7F9
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x0400098A RID: 2442
	public const string ID = "SublimationStation";

	// Token: 0x0400098B RID: 2443
	private const float DIRT_CONSUME_RATE = 1f;

	// Token: 0x0400098C RID: 2444
	private const float DIRT_STORAGE = 600f;

	// Token: 0x0400098D RID: 2445
	private const float OXYGEN_GENERATION_RATE = 0.66f;

	// Token: 0x0400098E RID: 2446
	private const float OXYGEN_TEMPERATURE = 303.15f;
}
