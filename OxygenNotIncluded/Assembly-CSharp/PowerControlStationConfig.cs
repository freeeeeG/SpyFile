using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020002D4 RID: 724
public class PowerControlStationConfig : IBuildingConfig
{
	// Token: 0x06000EAF RID: 3759 RVA: 0x00050F20 File Offset: 0x0004F120
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PowerControlStation";
		int width = 2;
		int height = 4;
		string anim = "electricianworkdesk_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x00050FA0 File Offset: 0x0004F1A0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerStation, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x00050FCC File Offset: 0x0004F1CC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 50f;
		storage.showInUI = true;
		storage.storageFilters = new List<Tag>
		{
			PowerControlStationConfig.MATERIAL_FOR_TINKER
		};
		TinkerStation tinkerStation = go.AddOrGet<TinkerStation>();
		tinkerStation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_electricianworkdesk_kanim")
		};
		tinkerStation.inputMaterial = PowerControlStationConfig.MATERIAL_FOR_TINKER;
		tinkerStation.massPerTinker = 5f;
		tinkerStation.outputPrefab = PowerControlStationConfig.TINKER_TOOLS;
		tinkerStation.outputTemperature = 308.15f;
		tinkerStation.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
		tinkerStation.choreType = Db.Get().ChoreTypes.PowerFabricate.IdHash;
		tinkerStation.useFilteredStorage = true;
		tinkerStation.fetchChoreType = Db.Get().ChoreTypes.PowerFetch.IdHash;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			TinkerStation component = game_object.GetComponent<TinkerStation>();
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		};
	}

	// Token: 0x04000875 RID: 2165
	public const string ID = "PowerControlStation";

	// Token: 0x04000876 RID: 2166
	public static Tag MATERIAL_FOR_TINKER = GameTags.RefinedMetal;

	// Token: 0x04000877 RID: 2167
	public static Tag TINKER_TOOLS = PowerStationToolsConfig.tag;

	// Token: 0x04000878 RID: 2168
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x04000879 RID: 2169
	public static string ROLE_PERK = "CanPowerTinker";

	// Token: 0x0400087A RID: 2170
	public const float OUTPUT_TEMPERATURE = 308.15f;
}
