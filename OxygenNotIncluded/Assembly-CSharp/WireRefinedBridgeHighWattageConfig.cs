using System;
using TUNING;
using UnityEngine;

// Token: 0x02000380 RID: 896
public class WireRefinedBridgeHighWattageConfig : WireBridgeHighWattageConfig
{
	// Token: 0x0600127A RID: 4730 RVA: 0x000635B8 File Offset: 0x000617B8
	protected override string GetID()
	{
		return "WireRefinedBridgeHighWattage";
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x000635C0 File Offset: 0x000617C0
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = base.CreateBuildingDef();
		buildingDef.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("heavywatttile_conductive_kanim")
		};
		buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridgeHighWattage");
		return buildingDef;
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x00063628 File Offset: 0x00061828
	protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = base.AddNetworkLink(go);
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max50000;
		return wireUtilityNetworkLink;
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x00063638 File Offset: 0x00061838
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	// Token: 0x040009FE RID: 2558
	public new const string ID = "WireRefinedBridgeHighWattage";
}
