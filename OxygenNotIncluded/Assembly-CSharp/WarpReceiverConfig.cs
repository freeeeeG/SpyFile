using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000376 RID: 886
public class WarpReceiverConfig : IEntityConfig
{
	// Token: 0x06001248 RID: 4680 RVA: 0x000628BB File Offset: 0x00060ABB
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x000628C4 File Offset: 0x00060AC4
	public GameObject CreatePrefab()
	{
		string id = WarpReceiverConfig.ID;
		string name = STRINGS.BUILDINGS.PREFABS.WARPRECEIVER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.WARPRECEIVER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("warp_portal_receiver_kanim"), "idle", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddTag(GameTags.NotRoomAssignable);
		gameObject.AddTag(GameTags.WarpTech);
		gameObject.AddTag(GameTags.Gravitas);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGet<WarpReceiver>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Prioritizable>();
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("notes_AI", UI.USERMENUACTIONS.READLORE.SEARCH_TELEPORTER_RECEIVER));
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
		return gameObject;
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x000629B4 File Offset: 0x00060BB4
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<WarpReceiver>().workLayer = Grid.SceneLayer.Building;
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		inst.GetComponent<Deconstructable>();
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x000629DF File Offset: 0x00060BDF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009E4 RID: 2532
	public static string ID = "WarpReceiver";
}
