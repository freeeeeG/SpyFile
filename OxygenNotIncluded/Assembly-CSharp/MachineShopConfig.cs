using System;
using TUNING;
using UnityEngine;

// Token: 0x02000239 RID: 569
public class MachineShopConfig : IBuildingConfig
{
	// Token: 0x06000B69 RID: 2921 RVA: 0x00040508 File Offset: 0x0003E708
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MachineShop";
		int width = 4;
		int height = 2;
		string anim = "machineshop_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0004057D File Offset: 0x0003E77D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.MachineShopType, false);
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00040597 File Offset: 0x0003E797
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040006AB RID: 1707
	public const string ID = "MachineShop";

	// Token: 0x040006AC RID: 1708
	public static readonly Tag MATERIAL_FOR_TINKER = GameTags.RefinedMetal;

	// Token: 0x040006AD RID: 1709
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x040006AE RID: 1710
	public static readonly string ROLE_PERK = "IncreaseMachinery";
}
