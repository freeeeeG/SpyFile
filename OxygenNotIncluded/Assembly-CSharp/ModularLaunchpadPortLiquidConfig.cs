using System;
using UnityEngine;

// Token: 0x020002A0 RID: 672
public class ModularLaunchpadPortLiquidConfig : IBuildingConfig
{
	// Token: 0x06000DB9 RID: 3513 RVA: 0x0004C7C6 File Offset: 0x0004A9C6
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0004C7CD File Offset: 0x0004A9CD
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquid", "conduit_port_liquid_loader_kanim", ConduitType.Liquid, true, 2, 2);
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x0004C7E2 File Offset: 0x0004A9E2
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, true);
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x0004C7F2 File Offset: 0x0004A9F2
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x040007E9 RID: 2025
	public const string ID = "ModularLaunchpadPortLiquid";
}
