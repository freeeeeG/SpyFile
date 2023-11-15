using System;
using TUNING;
using UnityEngine;

// Token: 0x02000304 RID: 772
public class RadiationLightConfig : IBuildingConfig
{
	// Token: 0x06000FA0 RID: 4000 RVA: 0x000541C2 File Offset: 0x000523C2
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x000541CC File Offset: 0x000523CC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RadiationLight";
		int width = 1;
		int height = 1;
		string anim = "radiation_lamp_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnWall;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x0005424C File Offset: 0x0005244C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = 50f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = this.FUEL_ELEMENT;
		manualDeliveryKG.capacity = 50f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
		radiationEmitter.emitAngle = 90f;
		radiationEmitter.emitDirection = 0f;
		radiationEmitter.emissionOffset = Vector3.right;
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = 16;
		radiationEmitter.emitRadiusY = 4;
		radiationEmitter.emitRads = 240f;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(this.FUEL_ELEMENT, 0.016666668f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.008333334f, this.WASTE_ELEMENT, 0f, false, true, 0f, 0.5f, 0.5f, byte.MaxValue, 0, true)
		};
		ElementDropper elementDropper = go.AddOrGet<ElementDropper>();
		elementDropper.emitTag = this.WASTE_ELEMENT.CreateTag();
		elementDropper.emitMass = 5f;
		RadiationLight radiationLight = go.AddComponent<RadiationLight>();
		radiationLight.elementToConsume = this.FUEL_ELEMENT;
		radiationLight.consumptionRate = 0.016666668f;
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x000543C2 File Offset: 0x000525C2
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x000543C4 File Offset: 0x000525C4
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x04000887 RID: 2183
	public const string ID = "RadiationLight";

	// Token: 0x04000888 RID: 2184
	private Tag FUEL_ELEMENT = SimHashes.UraniumOre.CreateTag();

	// Token: 0x04000889 RID: 2185
	private SimHashes WASTE_ELEMENT = SimHashes.DepletedUranium;

	// Token: 0x0400088A RID: 2186
	private const float FUEL_PER_CYCLE = 10f;

	// Token: 0x0400088B RID: 2187
	private const float CYCLES_PER_REFILL = 5f;

	// Token: 0x0400088C RID: 2188
	private const float FUEL_TO_WASTE_RATIO = 0.5f;

	// Token: 0x0400088D RID: 2189
	private const float FUEL_STORAGE_AMOUNT = 50f;

	// Token: 0x0400088E RID: 2190
	private const float FUEL_CONSUMPTION_RATE = 0.016666668f;

	// Token: 0x0400088F RID: 2191
	private const short RAD_LIGHT_SIZE_X = 16;

	// Token: 0x04000890 RID: 2192
	private const short RAD_LIGHT_SIZE_Y = 4;
}
