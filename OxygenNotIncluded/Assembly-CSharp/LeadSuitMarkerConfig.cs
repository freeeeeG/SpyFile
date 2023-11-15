using System;
using TUNING;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class LeadSuitMarkerConfig : IBuildingConfig
{
	// Token: 0x06000A0F RID: 2575 RVA: 0x0003A0A7 File Offset: 0x000382A7
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0003A0B0 File Offset: 0x000382B0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LeadSuitMarker";
		int width = 2;
		int height = 4;
		string anim = "changingarea_radiation_arrow_kanim";
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
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "LeadSuitMarker");
		return buildingDef;
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x0003A124 File Offset: 0x00038324
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("LeadSuitLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasLeadSuit;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("LeadSuitMarker"),
			new Tag("LeadSuitLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0003A19B File Offset: 0x0003839B
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400063A RID: 1594
	public const string ID = "LeadSuitMarker";
}
