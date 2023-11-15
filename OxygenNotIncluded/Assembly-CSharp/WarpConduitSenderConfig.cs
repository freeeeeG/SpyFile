﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000374 RID: 884
public class WarpConduitSenderConfig : IBuildingConfig
{
	// Token: 0x0600123B RID: 4667 RVA: 0x00062474 File Offset: 0x00060674
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x0006247C File Offset: 0x0006067C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WarpConduitSender";
		int width = 4;
		int height = 3;
		string anim = "warp_conduit_sender_kanim";
		int hitpoints = 250;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.CanMove = true;
		buildingDef.Invincible = true;
		return buildingDef;
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x000624F3 File Offset: 0x000606F3
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.liquidInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.gasInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.solidInputPort;
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x00062528 File Offset: 0x00060728
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		WarpConduitSender warpConduitSender = go.AddOrGet<WarpConduitSender>();
		warpConduitSender.liquidPortInfo = this.liquidInputPort;
		warpConduitSender.gasPortInfo = this.gasInputPort;
		warpConduitSender.solidPortInfo = this.solidInputPort;
		warpConduitSender.gasStorage = go.AddComponent<Storage>();
		warpConduitSender.gasStorage.showInUI = false;
		warpConduitSender.gasStorage.capacityKg = 1f;
		warpConduitSender.liquidStorage = go.AddComponent<Storage>();
		warpConduitSender.liquidStorage.showInUI = false;
		warpConduitSender.liquidStorage.capacityKg = 10f;
		warpConduitSender.solidStorage = go.AddComponent<Storage>();
		warpConduitSender.solidStorage.showInUI = false;
		warpConduitSender.solidStorage.capacityKg = 100f;
		Activatable activatable = go.AddOrGet<Activatable>();
		activatable.synchronizeAnims = true;
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_warp_conduit_sender_kanim")
		};
		activatable.workAnims = new HashedString[]
		{
			"sending_pre",
			"sending_loop"
		};
		activatable.workingPstComplete = new HashedString[]
		{
			"sending_pst"
		};
		activatable.workingPstFailed = new HashedString[]
		{
			"sending_pre"
		};
		activatable.SetWorkTime(30f);
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x000626A8 File Offset: 0x000608A8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingCellVisualizer>();
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.GetComponent<Activatable>().requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x000626DC File Offset: 0x000608DC
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x000626F4 File Offset: 0x000608F4
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x040009DF RID: 2527
	public const string ID = "WarpConduitSender";

	// Token: 0x040009E0 RID: 2528
	private ConduitPortInfo gasInputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 1));

	// Token: 0x040009E1 RID: 2529
	private ConduitPortInfo liquidInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(1, 1));

	// Token: 0x040009E2 RID: 2530
	private ConduitPortInfo solidInputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(2, 1));
}
