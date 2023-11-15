using System;
using TUNING;
using UnityEngine;

// Token: 0x020002C7 RID: 711
public class ParkSignConfig : IBuildingConfig
{
	// Token: 0x06000E75 RID: 3701 RVA: 0x0004FDD4 File Offset: 0x0004DFD4
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ParkSign", 1, 2, "parksign_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.ANY_BUILDABLE, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.NONE, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		return buildingDef;
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x0004FE2E File Offset: 0x0004E02E
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Park, false);
		go.AddOrGet<ParkSign>();
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x0004FE48 File Offset: 0x0004E048
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000852 RID: 2130
	public const string ID = "ParkSign";
}
