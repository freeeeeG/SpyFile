﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000244 RID: 580
public class MedicalCotConfig : IBuildingConfig
{
	// Token: 0x06000BA7 RID: 2983 RVA: 0x0004161C File Offset: 0x0003F81C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MedicalCot";
		int width = 3;
		int height = 2;
		string anim = "medical_cot_kanim";
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

	// Token: 0x06000BA8 RID: 2984 RVA: 0x00041674 File Offset: 0x0003F874
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Clinic, false);
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x00041690 File Offset: 0x0003F890
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BedType, false);
		Clinic clinic = go.AddOrGet<Clinic>();
		clinic.doctorVisitInterval = 300f;
		clinic.workerInjuredAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_healing_bed_kanim")
		};
		clinic.workerDiseasedAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_med_cot_sick_kanim")
		};
		clinic.workLayer = Grid.SceneLayer.BuildingFront;
		string text = "MedicalCot";
		string text2 = "MedicalCotDoctored";
		clinic.healthEffect = text;
		clinic.doctoredHealthEffect = text2;
		clinic.diseaseEffect = text;
		clinic.doctoredDiseaseEffect = text2;
		clinic.doctoredPlaceholderEffect = "DoctoredOffCotEffect";
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Hospital.Id;
		roomTracker.requirement = RoomTracker.Requirement.CustomRecommended;
		roomTracker.customStatusItemID = Db.Get().BuildingStatusItems.ClinicOutsideHospital.Id;
		go.AddOrGet<Sleepable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_med_cot_sick_kanim")
		};
		DoctorChoreWorkable doctorChoreWorkable = go.AddOrGet<DoctorChoreWorkable>();
		doctorChoreWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_med_cot_doctor_kanim")
		};
		doctorChoreWorkable.workTime = 45f;
		go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.Clinic.Id;
	}

	// Token: 0x040006D2 RID: 1746
	public const string ID = "MedicalCot";
}
