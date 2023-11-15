using System;
using TUNING;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class DevLifeSupportConfig : IBuildingConfig
{
	// Token: 0x060001BD RID: 445 RVA: 0x0000C4F8 File Offset: 0x0000A6F8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevLifeSupport";
		int width = 1;
		int height = 1;
		string anim = "dev_life_support_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER3, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000C56C File Offset: 0x0000A76C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = 200f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		CellOffset cellOffset = new CellOffset(0, 1);
		ElementEmitter elementEmitter = go.AddOrGet<ElementEmitter>();
		elementEmitter.outputElement = new ElementConverter.OutputElement(50.000004f, SimHashes.Oxygen, 303.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true);
		elementEmitter.emissionFrequency = 1f;
		elementEmitter.maxPressure = 1.5f;
		PassiveElementConsumer passiveElementConsumer = go.AddOrGet<PassiveElementConsumer>();
		passiveElementConsumer.elementToConsume = SimHashes.CarbonDioxide;
		passiveElementConsumer.consumptionRate = 50.000004f;
		passiveElementConsumer.capacityKG = 50.000004f;
		passiveElementConsumer.consumptionRadius = 10;
		passiveElementConsumer.showInStatusPanel = true;
		passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
		passiveElementConsumer.isRequired = false;
		passiveElementConsumer.storeOnConsume = false;
		passiveElementConsumer.showDescriptor = false;
		passiveElementConsumer.ignoreActiveChanged = true;
		go.AddOrGet<DevLifeSupport>();
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000C673 File Offset: 0x0000A873
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000111 RID: 273
	public const string ID = "DevLifeSupport";

	// Token: 0x04000112 RID: 274
	private const float OXYGEN_GENERATION_RATE = 50.000004f;

	// Token: 0x04000113 RID: 275
	private const float OXYGEN_TEMPERATURE = 303.15f;

	// Token: 0x04000114 RID: 276
	private const float OXYGEN_MAX_PRESSURE = 1.5f;

	// Token: 0x04000115 RID: 277
	private const float CO2_CONSUMPTION_RATE = 50.000004f;
}
