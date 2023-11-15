using System;
using UnityEngine;

// Token: 0x020002A1 RID: 673
public class ModularLaunchpadPortLiquidUnloaderConfig : IBuildingConfig
{
	// Token: 0x06000DBE RID: 3518 RVA: 0x0004C803 File Offset: 0x0004AA03
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x0004C80A File Offset: 0x0004AA0A
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquidUnloader", "conduit_port_liquid_unloader_kanim", ConduitType.Liquid, false, 2, 3);
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x0004C81F File Offset: 0x0004AA1F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, false);
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x0004C82F File Offset: 0x0004AA2F
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x040007EA RID: 2026
	public const string ID = "ModularLaunchpadPortLiquidUnloader";
}
