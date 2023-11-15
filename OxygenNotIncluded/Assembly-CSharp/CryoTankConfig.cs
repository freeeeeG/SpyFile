using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032A RID: 810
public class CryoTankConfig : IEntityConfig
{
	// Token: 0x06001078 RID: 4216 RVA: 0x000593E1 File Offset: 0x000575E1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x000593E8 File Offset: 0x000575E8
	public GameObject CreatePrefab()
	{
		string id = "CryoTank";
		string name = STRINGS.BUILDINGS.PREFABS.CRYOTANK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.CRYOTANK.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("cryo_chamber_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KAnimControllerBase>().SetFGLayer(Grid.SceneLayer.BuildingFront);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		CryoTank cryoTank = gameObject.AddOrGet<CryoTank>();
		cryoTank.overrideAnim = "anim_interacts_cryo_activation_kanim";
		cryoTank.dropOffset = new CellOffset(1, 0);
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("cryotank_warning", UI.USERMENUACTIONS.READLORE.SEARCH_CRYO_TANK));
		gameObject.AddOrGet<Demolishable>().allowDemolition = false;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x000594DF File Offset: 0x000576DF
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x000594E1 File Offset: 0x000576E1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000912 RID: 2322
	public const string ID = "CryoTank";
}
