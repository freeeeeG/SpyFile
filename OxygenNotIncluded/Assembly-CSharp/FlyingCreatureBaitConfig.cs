﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class FlyingCreatureBaitConfig : IBuildingConfig
{
	// Token: 0x06000705 RID: 1797 RVA: 0x0002D628 File Offset: 0x0002B828
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FlyingCreatureBait", 1, 2, "airborne_critter_bait_kanim", 10, 10f, new float[]
		{
			50f,
			10f
		}, new string[]
		{
			"Metal",
			"FlyingCritterEdible"
		}, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Deprecated = true;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x0002D6A7 File Offset: 0x0002B8A7
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<CreatureBait>();
		go.AddTag(GameTags.OneTimeUseLure);
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0002D6BB File Offset: 0x0002B8BB
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x0002D6BD File Offset: 0x0002B8BD
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0002D6C0 File Offset: 0x0002B8C0
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGet<SymbolOverrideController>().applySymbolOverridesEveryFrame = true;
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		Prioritizable.AddRef(go);
	}

	// Token: 0x040004E2 RID: 1250
	public const string ID = "FlyingCreatureBait";
}
