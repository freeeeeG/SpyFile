﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class CommandModuleConfig : IBuildingConfig
{
	// Token: 0x06000132 RID: 306 RVA: 0x00008E18 File Offset: 0x00007018
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_VANILLA_ONLY;
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00008E20 File Offset: 0x00007020
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CommandModule";
		int width = 5;
		int height = 5;
		string anim = "rocket_command_module_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] command_MODULE_MASS = TUNING.BUILDINGS.ROCKETRY_MASS_KG.COMMAND_MODULE_MASS;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, command_MODULE_MASS, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("TriggerLaunch", new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH, STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH_ACTIVE, STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH_INACTIVE, false, false)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("LaunchReady", new CellOffset(0, 2), STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY, STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY_ACTIVE, STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00008F5C File Offset: 0x0000715C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		LaunchConditionManager launchConditionManager = go.AddOrGet<LaunchConditionManager>();
		launchConditionManager.triggerPort = "TriggerLaunch";
		launchConditionManager.statusPort = "LaunchReady";
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		go.AddOrGet<CommandModule>();
		go.AddOrGet<CommandModuleWorkable>();
		go.AddOrGet<RocketCommandConditions>();
		go.AddOrGet<MinionStorage>();
		go.AddOrGet<ArtifactFinder>();
		go.AddOrGet<LaunchableRocket>();
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000900C File Offset: 0x0000720C
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_command_module_bg_kanim", false);
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.RocketCommandModule.Id;
		ownable.canBePublic = false;
		go.AddOrGet<CharacterOverlay>().shouldShowName = true;
	}

	// Token: 0x040000B2 RID: 178
	public const string ID = "CommandModule";

	// Token: 0x040000B3 RID: 179
	private const string TRIGGER_LAUNCH_PORT_ID = "TriggerLaunch";

	// Token: 0x040000B4 RID: 180
	private const string LAUNCH_READY_PORT_ID = "LaunchReady";
}
