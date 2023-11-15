using System;
using TUNING;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class AdvancedResearchCenterConfig : IBuildingConfig
{
	// Token: 0x0600004B RID: 75 RVA: 0x00003E7C File Offset: 0x0000207C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AdvancedResearchCenter";
		int width = 3;
		int height = 3;
		string anim = "research_center2_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00003F0C File Offset: 0x0000210C
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
		manualDeliveryKG.RequestedItemTag = AdvancedResearchCenterConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 150f;
		manualDeliveryKG.capacity = 750f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
		researchCenter.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research2_kanim")
		};
		researchCenter.research_point_type_id = "advanced";
		researchCenter.inputMaterial = AdvancedResearchCenterConfig.INPUT_MATERIAL;
		researchCenter.mass_per_point = 50f;
		researchCenter.requiredSkillPerk = Db.Get().SkillPerks.AllowAdvancedResearch.Id;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(AdvancedResearchCenterConfig.INPUT_MATERIAL, 0.8333333f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00004034 File Offset: 0x00002234
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000037 RID: 55
	public const string ID = "AdvancedResearchCenter";

	// Token: 0x04000038 RID: 56
	public const float BASE_SECONDS_PER_POINT = 60f;

	// Token: 0x04000039 RID: 57
	public const float MASS_PER_POINT = 50f;

	// Token: 0x0400003A RID: 58
	public const float BASE_MASS_PER_SECOND = 0.8333333f;

	// Token: 0x0400003B RID: 59
	public const float CAPACITY = 750f;

	// Token: 0x0400003C RID: 60
	public static readonly Tag INPUT_MATERIAL = GameTags.Water;
}
