using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class GraveConfig : IBuildingConfig
{
	// Token: 0x060008D5 RID: 2261 RVA: 0x000345B4 File Offset: 0x000327B4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Grave";
		int width = 1;
		int height = 2;
		string anim = "gravestone_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		return buildingDef;
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00034620 File Offset: 0x00032820
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GraveConfig.STORAGE_OVERRIDE_ANIM_FILES = new KAnimFile[]
		{
			Assets.GetAnim("anim_bury_dupe_kanim")
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(GraveConfig.StorageModifiers);
		storage.overrideAnims = GraveConfig.STORAGE_OVERRIDE_ANIM_FILES;
		storage.workAnims = GraveConfig.STORAGE_WORK_ANIMS;
		storage.workingPstComplete = new HashedString[]
		{
			GraveConfig.STORAGE_PST_ANIM
		};
		storage.synchronizeAnims = false;
		storage.useGunForDelivery = false;
		storage.workAnimPlayMode = KAnim.PlayMode.Once;
		go.AddOrGet<Grave>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x000346B3 File Offset: 0x000328B3
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000590 RID: 1424
	public const string ID = "Grave";

	// Token: 0x04000591 RID: 1425
	private static KAnimFile[] STORAGE_OVERRIDE_ANIM_FILES;

	// Token: 0x04000592 RID: 1426
	private static readonly HashedString[] STORAGE_WORK_ANIMS = new HashedString[]
	{
		"working_pre"
	};

	// Token: 0x04000593 RID: 1427
	private static readonly HashedString STORAGE_PST_ANIM = HashedString.Invalid;

	// Token: 0x04000594 RID: 1428
	private static readonly List<Storage.StoredItemModifier> StorageModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};
}
