using System;
using TUNING;
using UnityEngine;

// Token: 0x0200037F RID: 895
public class WireRefinedBridgeConfig : WireBridgeConfig
{
	// Token: 0x06001276 RID: 4726 RVA: 0x0006353E File Offset: 0x0006173E
	protected override string GetID()
	{
		return "WireRefinedBridge";
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x00063548 File Offset: 0x00061748
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = base.CreateBuildingDef();
		buildingDef.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("utilityelectricbridgeconductive_kanim")
		};
		buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridge");
		return buildingDef;
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x000635A0 File Offset: 0x000617A0
	protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = base.AddNetworkLink(go);
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max2000;
		return wireUtilityNetworkLink;
	}

	// Token: 0x040009FD RID: 2557
	public new const string ID = "WireRefinedBridge";
}
