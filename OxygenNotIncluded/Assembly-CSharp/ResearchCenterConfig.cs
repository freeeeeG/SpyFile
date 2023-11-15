﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class ResearchCenterConfig : IBuildingConfig
{
	// Token: 0x06000FC9 RID: 4041 RVA: 0x000550DC File Offset: 0x000532DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ResearchCenter";
		int width = 2;
		int height = 2;
		string anim = "research_center_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x0005516C File Offset: 0x0005336C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = ResearchCenterConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 150f;
		manualDeliveryKG.capacity = 750f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
		researchCenter.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research_center_kanim")
		};
		researchCenter.research_point_type_id = "basic";
		researchCenter.inputMaterial = ResearchCenterConfig.INPUT_MATERIAL;
		researchCenter.mass_per_point = 50f;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(ResearchCenterConfig.INPUT_MATERIAL, 1.1111112f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x0005527A File Offset: 0x0005347A
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040008AB RID: 2219
	public const float BASE_SECONDS_PER_POINT = 45f;

	// Token: 0x040008AC RID: 2220
	public const float MASS_PER_POINT = 50f;

	// Token: 0x040008AD RID: 2221
	public const float BASE_MASS_PER_SECOND = 1.1111112f;

	// Token: 0x040008AE RID: 2222
	public static readonly Tag INPUT_MATERIAL = GameTags.Dirt;

	// Token: 0x040008AF RID: 2223
	public const float CAPACITY = 750f;

	// Token: 0x040008B0 RID: 2224
	public const string ID = "ResearchCenter";
}
