using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200034C RID: 844
public class SpiceGrinderConfig : IBuildingConfig
{
	// Token: 0x06001139 RID: 4409 RVA: 0x0005CD3C File Offset: 0x0005AF3C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpiceGrinder";
		int width = 2;
		int height = 3;
		string anim = "spice_grinder_kanim";
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

	// Token: 0x0600113A RID: 4410 RVA: 0x0005CDBC File Offset: 0x0005AFBC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.SpiceStation, false);
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x0005CDD8 File Offset: 0x0005AFD8
	public override void DoPostConfigureComplete(GameObject go)
	{
		SpiceGrinder.InitializeSpices();
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGetDef<SpiceGrinder.Def>();
		go.AddOrGet<SpiceGrinderWorkable>();
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<TreeFilterable>().uiHeight = TreeFilterable.UISideScreenHeight.Short;
		go.AddOrGet<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, SpiceGrinderConfig.STORAGE_PRIORITY));
		Storage storage = go.AddComponent<Storage>();
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.storageFilters = new List<Tag>
		{
			GameTags.Edible
		};
		storage.allowItemRemoval = false;
		storage.capacityKg = 1f;
		storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.Building;
		storage.showCapacityStatusItem = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showSideScreenTitleBar = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		Storage storage2 = go.AddComponent<Storage>();
		storage2.showInUI = true;
		storage2.showDescriptor = true;
		storage2.storageFilters = new List<Tag>
		{
			GameTags.Seed
		};
		storage2.allowItemRemoval = false;
		storage2.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage2.fetchCategory = Storage.FetchCategory.Building;
		storage2.showCapacityStatusItem = true;
		storage2.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Kitchen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
	}

	// Token: 0x0400095E RID: 2398
	public const string ID = "SpiceGrinder";

	// Token: 0x0400095F RID: 2399
	public static Tag MATERIAL_FOR_TINKER = GameTags.CropSeed;

	// Token: 0x04000960 RID: 2400
	public static Tag TINKER_TOOLS = FarmStationToolsConfig.tag;

	// Token: 0x04000961 RID: 2401
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x04000962 RID: 2402
	public const float OUTPUT_TEMPERATURE = 313.15f;

	// Token: 0x04000963 RID: 2403
	public const float WORK_TIME_PER_1000KCAL = 5f;

	// Token: 0x04000964 RID: 2404
	public const short SPICE_CAPACITY_PER_INGREDIENT = 10;

	// Token: 0x04000965 RID: 2405
	public const string PrimaryColorSymbol = "stripe_anim2";

	// Token: 0x04000966 RID: 2406
	public const string SecondaryColorSymbol = "stripe_anim1";

	// Token: 0x04000967 RID: 2407
	public const string GrinderColorSymbol = "grinder";

	// Token: 0x04000968 RID: 2408
	public static StatusItem SpicedStatus = Db.Get().MiscStatusItems.SpicedFood;

	// Token: 0x04000969 RID: 2409
	private static int STORAGE_PRIORITY = Chore.DefaultPrioritySetting.priority_value - 1;
}
