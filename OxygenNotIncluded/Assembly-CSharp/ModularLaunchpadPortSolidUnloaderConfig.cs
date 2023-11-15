using System;
using UnityEngine;

// Token: 0x020002A3 RID: 675
public class ModularLaunchpadPortSolidUnloaderConfig : IBuildingConfig
{
	// Token: 0x06000DC8 RID: 3528 RVA: 0x0004C87D File Offset: 0x0004AA7D
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0004C884 File Offset: 0x0004AA84
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortSolidUnloader", "conduit_port_solid_unloader_kanim", ConduitType.Solid, false, 2, 3);
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x0004C899 File Offset: 0x0004AA99
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Solid, 20f, false);
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x0004C8A9 File Offset: 0x0004AAA9
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x040007EC RID: 2028
	public const string ID = "ModularLaunchpadPortSolidUnloader";
}
