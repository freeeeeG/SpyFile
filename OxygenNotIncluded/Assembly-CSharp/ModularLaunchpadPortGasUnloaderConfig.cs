using System;
using UnityEngine;

// Token: 0x0200029F RID: 671
public class ModularLaunchpadPortGasUnloaderConfig : IBuildingConfig
{
	// Token: 0x06000DB4 RID: 3508 RVA: 0x0004C789 File Offset: 0x0004A989
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x0004C790 File Offset: 0x0004A990
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortGasUnloader", "conduit_port_gas_unloader_kanim", ConduitType.Gas, false, 2, 3);
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x0004C7A5 File Offset: 0x0004A9A5
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Gas, 1f, false);
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x0004C7B5 File Offset: 0x0004A9B5
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x040007E8 RID: 2024
	public const string ID = "ModularLaunchpadPortGasUnloader";
}
