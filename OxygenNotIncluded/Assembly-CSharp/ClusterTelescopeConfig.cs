using System;
using TUNING;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class ClusterTelescopeConfig : IBuildingConfig
{
	// Token: 0x06000113 RID: 275 RVA: 0x000086F0 File Offset: 0x000068F0
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x000086F8 File Offset: 0x000068F8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ClusterTelescope";
		int width = 3;
		int height = 3;
		string anim = "telescope_low_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00008788 File Offset: 0x00006988
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
		def.clearScanCellRadius = 4;
		def.workableOverrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_low_kanim")
		};
		def.skyVisibilityInfo = ClusterTelescopeConfig.SKY_VISIBILITY_INFO;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0000880D File Offset: 0x00006A0D
	public override void DoPostConfigureComplete(GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00008815 File Offset: 0x00006A15
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x06000118 RID: 280 RVA: 0x0000881D File Offset: 0x00006A1D
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00008825 File Offset: 0x00006A25
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 1;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 4;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x040000A2 RID: 162
	public const string ID = "ClusterTelescope";

	// Token: 0x040000A3 RID: 163
	public const int SCAN_RADIUS = 4;

	// Token: 0x040000A4 RID: 164
	public const int VERTICAL_SCAN_OFFSET = 1;

	// Token: 0x040000A5 RID: 165
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 1), 4, new CellOffset(0, 1), 4, 0);
}
