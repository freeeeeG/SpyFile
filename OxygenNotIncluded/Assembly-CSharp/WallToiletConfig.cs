using System;
using TUNING;
using UnityEngine;

// Token: 0x02000372 RID: 882
public class WallToiletConfig : IBuildingConfig
{
	// Token: 0x0600122E RID: 4654 RVA: 0x00061F40 File Offset: 0x00060140
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x00061F48 File Offset: 0x00060148
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WallToilet";
		int width = 1;
		int height = 3;
		string anim = "toilet_wall_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.WallFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.DiseaseCellVisName = "FoodPoisoning";
		buildingDef.UtilityOutputOffset = new CellOffset(-2, 0);
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x00061FF8 File Offset: 0x000601F8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ToiletType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.FlushToiletType, false);
		FlushToilet flushToilet = go.AddOrGet<FlushToilet>();
		flushToilet.massConsumedPerUse = 2.5f;
		flushToilet.massEmittedPerUse = 9.2f;
		flushToilet.newPeeTemperature = 310.15f;
		flushToilet.diseaseId = "FoodPoisoning";
		flushToilet.diseasePerFlush = 100000;
		flushToilet.diseaseOnDupePerFlush = 20000;
		flushToilet.requireOutput = false;
		flushToilet.meterOffset = Meter.Offset.Infront;
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_toilet_wall_kanim")
		};
		ToiletWorkableUse toiletWorkableUse = go.AddOrGet<ToiletWorkableUse>();
		toiletWorkableUse.overrideAnims = overrideAnims;
		toiletWorkableUse.workLayer = Grid.SceneLayer.BuildingUse;
		toiletWorkableUse.resetProgressOnStop = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 2.5f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		AutoStorageDropper.Def def = go.AddOrGetDef<AutoStorageDropper.Def>();
		def.dropOffset = new CellOffset(-2, 0);
		def.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		def.invertElementFilter = true;
		def.blockedBySubstantialLiquid = true;
		def.fxOffset = new Vector3(0.5f, 0f, 0f);
		def.leftFx = new AutoStorageDropper.DropperFxConfig
		{
			animFile = "liquidleak_kanim",
			animName = "side",
			flipX = true,
			layer = Grid.SceneLayer.BuildingBack
		};
		def.rightFx = new AutoStorageDropper.DropperFxConfig
		{
			animFile = "liquidleak_kanim",
			animName = "side",
			flipX = false,
			layer = Grid.SceneLayer.BuildingBack
		};
		def.delay = 0f;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 12.5f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
		ownable.canBePublic = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x000621EE File Offset: 0x000603EE
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040009D9 RID: 2521
	private const float WATER_USAGE = 2.5f;

	// Token: 0x040009DA RID: 2522
	public const string ID = "WallToilet";
}
