using System;
using TUNING;
using UnityEngine;

// Token: 0x0200035D RID: 861
public class SuitMarkerConfig : IBuildingConfig
{
	// Token: 0x0600119D RID: 4509 RVA: 0x0005F37C File Offset: 0x0005D57C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SuitMarker";
		int width = 1;
		int height = 3;
		string anim = "changingarea_arrow_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "SuitMarker");
		return buildingDef;
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x0005F3E4 File Offset: 0x0005D5E4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("SuitLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasAtmoSuit;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("SuitMarker"),
			new Tag("SuitLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x0600119F RID: 4511 RVA: 0x0005F45A File Offset: 0x0005D65A
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000995 RID: 2453
	public const string ID = "SuitMarker";
}
