using System;
using TUNING;
using UnityEngine;

// Token: 0x0200031F RID: 799
public class RocketInteriorPowerPlugConfig : IBuildingConfig
{
	// Token: 0x06001043 RID: 4163 RVA: 0x00058338 File Offset: 0x00056538
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x00058340 File Offset: 0x00056540
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorPowerPlug";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.RequiresPowerOutput = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "RocketInteriorPowerPlug");
		return buildingDef;
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x000583F4 File Offset: 0x000565F4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x00058416 File Offset: 0x00056616
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<OperationalController.Def>();
		go.AddOrGet<WireUtilitySemiVirtualNetworkLink>().link1 = new CellOffset(0, 0);
	}

	// Token: 0x040008F5 RID: 2293
	public const string ID = "RocketInteriorPowerPlug";
}
