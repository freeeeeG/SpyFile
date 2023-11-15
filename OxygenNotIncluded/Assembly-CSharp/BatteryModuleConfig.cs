using System;
using TUNING;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class BatteryModuleConfig : IBuildingConfig
{
	// Token: 0x060000A1 RID: 161 RVA: 0x00005EA8 File Offset: 0x000040A8
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00005EB0 File Offset: 0x000040B0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BatteryModule";
		int width = 3;
		int height = 2;
		string anim = "rocket_battery_pack_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] hollow_TIER = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "grounded";
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.PowerInputOffset = BatteryModuleConfig.PLUG_OFFSET;
		buildingDef.PowerOutputOffset = BatteryModuleConfig.PLUG_OFFSET;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.UseWhitePowerOutputConnectorColour = true;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		return buildingDef;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00005F70 File Offset: 0x00004170
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddComponent<RequireInputs>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 2), GameTags.Rocket, null)
		};
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00005FDC File Offset: 0x000041DC
	public override void DoPostConfigureComplete(GameObject go)
	{
		Prioritizable.AddRef(go);
		ModuleBattery moduleBattery = go.AddOrGet<ModuleBattery>();
		moduleBattery.capacity = 100000f;
		moduleBattery.joulesLostPerSecond = 0.6666667f;
		WireUtilitySemiVirtualNetworkLink wireUtilitySemiVirtualNetworkLink = go.AddOrGet<WireUtilitySemiVirtualNetworkLink>();
		wireUtilitySemiVirtualNetworkLink.link1 = BatteryModuleConfig.PLUG_OFFSET;
		wireUtilitySemiVirtualNetworkLink.visualizeOnly = true;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR, 0f, 0f);
	}

	// Token: 0x0400006E RID: 110
	public const string ID = "BatteryModule";

	// Token: 0x0400006F RID: 111
	public const float NUM_CAPSULES = 3f;

	// Token: 0x04000070 RID: 112
	private static readonly CellOffset PLUG_OFFSET = new CellOffset(-1, 0);
}
