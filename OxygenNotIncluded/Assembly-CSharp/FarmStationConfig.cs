﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class FarmStationConfig : IBuildingConfig
{
	// Token: 0x0600027F RID: 639 RVA: 0x00011D30 File Offset: 0x0000FF30
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FarmStation";
		int width = 2;
		int height = 3;
		string anim = "planttender_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00011DB0 File Offset: 0x0000FFB0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.FarmStationType, false);
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00011DCC File Offset: 0x0000FFCC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = FarmStationConfig.MATERIAL_FOR_TINKER;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.capacity = 50f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
		TinkerStation tinkerStation = go.AddOrGet<TinkerStation>();
		tinkerStation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_planttender_kanim")
		};
		tinkerStation.inputMaterial = FarmStationConfig.MATERIAL_FOR_TINKER;
		tinkerStation.massPerTinker = 5f;
		tinkerStation.outputPrefab = FarmStationConfig.TINKER_TOOLS;
		tinkerStation.outputTemperature = 308.15f;
		tinkerStation.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		tinkerStation.choreType = Db.Get().ChoreTypes.FarmingFabricate.IdHash;
		tinkerStation.fetchChoreType = Db.Get().ChoreTypes.FarmFetch.IdHash;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			TinkerStation component = game_object.GetComponent<TinkerStation>();
			component.AttributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
			component.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		};
	}

	// Token: 0x04000167 RID: 359
	public const string ID = "FarmStation";

	// Token: 0x04000168 RID: 360
	public static Tag MATERIAL_FOR_TINKER = GameTags.Fertilizer;

	// Token: 0x04000169 RID: 361
	public static Tag TINKER_TOOLS = FarmStationToolsConfig.tag;

	// Token: 0x0400016A RID: 362
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x0400016B RID: 363
	public const float OUTPUT_TEMPERATURE = 308.15f;
}
