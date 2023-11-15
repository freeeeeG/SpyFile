using System;
using UnityEngine;

// Token: 0x0200029E RID: 670
public class ModularLaunchpadPortGasConfig : IBuildingConfig
{
	// Token: 0x06000DAF RID: 3503 RVA: 0x0004C74C File Offset: 0x0004A94C
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x0004C753 File Offset: 0x0004A953
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortGas", "conduit_port_gas_loader_kanim", ConduitType.Gas, true, 2, 2);
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x0004C768 File Offset: 0x0004A968
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Gas, 1f, true);
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x0004C778 File Offset: 0x0004A978
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x040007E7 RID: 2023
	public const string ID = "ModularLaunchpadPortGas";
}
