using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public class LandingPodConfig : IEntityConfig
{
	// Token: 0x06000A00 RID: 2560 RVA: 0x00039BF2 File Offset: 0x00037DF2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x00039BFC File Offset: 0x00037DFC
	public GameObject CreatePrefab()
	{
		string id = "LandingPod";
		string name = STRINGS.BUILDINGS.PREFABS.LANDING_POD.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.LANDING_POD.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_puft_pod_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<PodLander>();
		gameObject.AddOrGet<MinionStorage>();
		return gameObject;
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x00039C6B File Offset: 0x00037E6B
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x00039C82 File Offset: 0x00037E82
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000633 RID: 1587
	public const string ID = "LandingPod";
}
