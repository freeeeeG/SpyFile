using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F2 RID: 754
public class PropGravitasHandScannerConfig : IEntityConfig
{
	// Token: 0x06000F42 RID: 3906 RVA: 0x00052F1A File Offset: 0x0005111A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x00052F24 File Offset: 0x00051124
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasHandScanner";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASHANDSCANNER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASHANDSCANNER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_hand_scanner_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x00052FB7 File Offset: 0x000511B7
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00052FCE File Offset: 0x000511CE
	public void OnSpawn(GameObject inst)
	{
	}
}
