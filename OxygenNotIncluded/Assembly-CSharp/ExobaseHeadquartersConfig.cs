﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class ExobaseHeadquartersConfig : IBuildingConfig
{
	// Token: 0x06000272 RID: 626 RVA: 0x000117FF File Offset: 0x0000F9FF
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00011808 File Offset: 0x0000FA08
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ExobaseHeadquarters";
		int width = 3;
		int height = 3;
		string anim = "porta_pod_y_kanim";
		int hitpoints = 250;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] all_MINERALS = MATERIALS.ALL_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER5, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = 400f;
		buildingDef.ShowInBuildMenu = true;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.OnePerWorld = true;
		SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_LP", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_open", NOISE_POLLUTION.NOISY.TIER4);
		SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_close", NOISE_POLLUTION.NOISY.TIER4);
		return buildingDef;
	}

	// Token: 0x06000274 RID: 628 RVA: 0x000118DC File Offset: 0x0000FADC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		LoreBearerUtil.AddLoreTo(go);
		Telepad telepad = go.AddOrGet<Telepad>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Telepad, false);
		telepad.startingSkillPoints = 1f;
		SocialGatheringPoint socialGatheringPoint = go.AddOrGet<SocialGatheringPoint>();
		socialGatheringPoint.choreOffsets = new CellOffset[]
		{
			new CellOffset(-1, 0),
			new CellOffset(-2, 0),
			new CellOffset(2, 0),
			new CellOffset(3, 0),
			new CellOffset(0, 0),
			new CellOffset(1, 0)
		};
		socialGatheringPoint.choreCount = 4;
		socialGatheringPoint.basePriority = RELAXATION.PRIORITY.TIER0;
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.Color = LIGHT2D.HEADQUARTERS_COLOR;
		light2D.Range = 5f;
		light2D.Offset = LIGHT2D.EXOBASE_HEADQUARTERS_OFFSET;
		light2D.overlayColour = LIGHT2D.HEADQUARTERS_OVERLAYCOLOR;
		light2D.shape = global::LightShape.Circle;
		light2D.drawOverlay = true;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.Experimental, false);
		RoleStation roleStation = go.AddOrGet<RoleStation>();
		roleStation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_hqbase_skill_upgrade_kanim")
		};
		roleStation.workAnims = new HashedString[]
		{
			"upgrade"
		};
		roleStation.workingPstComplete = null;
		roleStation.workingPstFailed = null;
		Activatable activatable = go.AddOrGet<Activatable>();
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_porta_pod_kanim")
		};
		activatable.workAnims = new HashedString[]
		{
			"activate_pre",
			"activate_loop"
		};
		activatable.workingPstComplete = new HashedString[]
		{
			"activate_pst"
		};
		activatable.workingPstFailed = new HashedString[]
		{
			"activate_pre"
		};
		activatable.SetWorkTime(15f);
	}

	// Token: 0x06000275 RID: 629 RVA: 0x00011AD3 File Offset: 0x0000FCD3
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000164 RID: 356
	public const string ID = "ExobaseHeadquarters";
}
