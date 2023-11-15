using System;
using TUNING;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class OxygenMaskMarkerConfig : IBuildingConfig
{
	// Token: 0x06000E5A RID: 3674 RVA: 0x0004F3AC File Offset: 0x0004D5AC
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x0004F3B4 File Offset: 0x0004D5B4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxygenMaskMarker";
		int width = 1;
		int height = 2;
		string anim = "oxygen_checkpoint_arrow_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = raw_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "OxygenMaskMarker");
		return buildingDef;
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x0004F41C File Offset: 0x0004D61C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("OxygenMaskLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasOxygenMask;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("OxygenMaskMarker"),
			new Tag("OxygenMaskLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x0004F492 File Offset: 0x0004D692
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400083E RID: 2110
	public const string ID = "OxygenMaskMarker";
}
