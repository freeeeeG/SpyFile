using System;
using TUNING;
using UnityEngine;

// Token: 0x02000255 RID: 597
public class MilkFeederConfig : IBuildingConfig
{
	// Token: 0x06000BFD RID: 3069 RVA: 0x00043A78 File Offset: 0x00041C78
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MilkFeeder";
		int width = 3;
		int height = 3;
		string anim = "critter_milk_feeder_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x00043AF6 File Offset: 0x00041CF6
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x00043AF8 File Offset: 0x00041CF8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<LogicOperationalController>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 80f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Milk);
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x00043BC1 File Offset: 0x00041DC1
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<MilkFeeder.Def>();
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x00043BCA File Offset: 0x00041DCA
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x0400071D RID: 1821
	public const string ID = "MilkFeeder";

	// Token: 0x0400071E RID: 1822
	public const string HAD_CONSUMED_MILK_RECENTLY_EFFECT_ID = "HadMilk";

	// Token: 0x0400071F RID: 1823
	public const float EFFECT_DURATION_IN_SECONDS = 600f;

	// Token: 0x04000720 RID: 1824
	public static readonly CellOffset DRINK_FROM_OFFSET = new CellOffset(1, 0);

	// Token: 0x04000721 RID: 1825
	public static readonly Tag MILK_TAG = SimHashes.Milk.CreateTag();

	// Token: 0x04000722 RID: 1826
	public const float UNITS_OF_MILK_CONSUMED_PER_FEEDING = 5f;
}
