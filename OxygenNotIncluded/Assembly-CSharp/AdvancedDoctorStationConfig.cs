﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class AdvancedDoctorStationConfig : IBuildingConfig
{
	// Token: 0x06000047 RID: 71 RVA: 0x00003CAC File Offset: 0x00001EAC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AdvancedDoctorStation";
		int width = 2;
		int height = 3;
		string anim = "bed_medical_kanim";
		int hitpoints = 100;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003D25 File Offset: 0x00001F25
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Clinic, false);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00003D40 File Offset: 0x00001F40
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		Tag supplyTagForStation = MedicineInfo.GetSupplyTagForStation("AdvancedDoctorStation");
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
			Assets.GetAnim("anim_interacts_medical_bed_kanim")
		};
		doctorStation.workLayer = Grid.SceneLayer.BuildingFront;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Hospital.Id;
		roomTracker.requirement = RoomTracker.Requirement.CustomRecommended;
		roomTracker.customStatusItemID = Db.Get().BuildingStatusItems.ClinicOutsideHospital.Id;
		DoctorStationDoctorWorkable doctorStationDoctorWorkable = go.AddOrGet<DoctorStationDoctorWorkable>();
		doctorStationDoctorWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_medical_bed_doctor_kanim")
		};
		doctorStationDoctorWorkable.SetWorkTime(60f);
		doctorStationDoctorWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanAdvancedMedicine.Id;
	}

	// Token: 0x04000036 RID: 54
	public const string ID = "AdvancedDoctorStation";
}
