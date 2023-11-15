using System;
using UnityEngine;

// Token: 0x020002A2 RID: 674
public class ModularLaunchpadPortSolidConfig : IBuildingConfig
{
	// Token: 0x06000DC3 RID: 3523 RVA: 0x0004C840 File Offset: 0x0004AA40
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x0004C847 File Offset: 0x0004AA47
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortSolid", "conduit_port_solid_loader_kanim", ConduitType.Solid, true, 2, 2);
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x0004C85C File Offset: 0x0004AA5C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Solid, 20f, true);
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x0004C86C File Offset: 0x0004AA6C
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x040007EB RID: 2027
	public const string ID = "ModularLaunchpadPortSolid";
}
