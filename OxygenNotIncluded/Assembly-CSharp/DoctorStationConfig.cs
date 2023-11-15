using System;
using TUNING;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class DoctorStationConfig : IBuildingConfig
{
	// Token: 0x060001DF RID: 479 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DoctorStation";
		int width = 3;
		int height = 2;
		string anim = "treatment_chair_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000D100 File Offset: 0x0000B300
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Clinic, false);
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x0000D11C File Offset: 0x0000B31C
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		Tag supplyTagForStation = MedicineInfo.GetSupplyTagForStation("DoctorStation");
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = supplyTagForStation;
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		DoctorStation doctorStation = go.AddOrGet<DoctorStation>();
		doctorStation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_treatment_chair_sick_kanim")
		};
		doctorStation.workLayer = Grid.SceneLayer.BuildingFront;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Hospital.Id;
		roomTracker.requirement = RoomTracker.Requirement.CustomRecommended;
		roomTracker.customStatusItemID = Db.Get().BuildingStatusItems.ClinicOutsideHospital.Id;
		DoctorStationDoctorWorkable doctorStationDoctorWorkable = go.AddOrGet<DoctorStationDoctorWorkable>();
		doctorStationDoctorWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_treatment_chair_doctor_kanim")
		};
		doctorStationDoctorWorkable.SetWorkTime(40f);
		doctorStationDoctorWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanDoctor.Id;
	}

	// Token: 0x04000127 RID: 295
	public const string ID = "DoctorStation";
}
