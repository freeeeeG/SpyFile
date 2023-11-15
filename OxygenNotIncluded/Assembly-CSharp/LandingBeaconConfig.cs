using System;
using TUNING;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class LandingBeaconConfig : IBuildingConfig
{
	// Token: 0x060009F7 RID: 2551 RVA: 0x00039A48 File Offset: 0x00037C48
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x00039A50 File Offset: 0x00037C50
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LandingBeacon";
		int width = 1;
		int height = 3;
		string anim = "landing_beacon_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 398.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		return buildingDef;
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x00039AEB File Offset: 0x00037CEB
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGetDef<LandingBeacon.Def>();
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x00039B0C File Offset: 0x00037D0C
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x00039B14 File Offset: 0x00037D14
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00039B1C File Offset: 0x00037D1C
	public override void DoPostConfigureComplete(GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x00039B24 File Offset: 0x00037D24
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = new Func<int, bool>(LandingBeaconConfig.BeaconSkyVisibility);
		};
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x00039B64 File Offset: 0x00037D64
	private static bool BeaconSkyVisibility(int cell)
	{
		DebugUtil.DevAssert(ClusterManager.Instance != null, "beacon assumes DLC", null);
		if (Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != 255)
		{
			int num = (int)ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]).maximumBounds.y;
			int num2 = cell;
			while (Grid.CellRow(num2) <= num)
			{
				if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
				{
					return false;
				}
				num2 = Grid.CellAbove(num2);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000631 RID: 1585
	public const string ID = "LandingBeacon";

	// Token: 0x04000632 RID: 1586
	public const int LANDING_ACCURACY = 3;
}
