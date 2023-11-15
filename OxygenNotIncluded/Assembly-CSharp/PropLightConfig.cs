using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FC RID: 764
public class PropLightConfig : IEntityConfig
{
	// Token: 0x06000F74 RID: 3956 RVA: 0x000537F5 File Offset: 0x000519F5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x000537FC File Offset: 0x000519FC
	public GameObject CreatePrefab()
	{
		string id = "PropLight";
		string name = STRINGS.BUILDINGS.PREFABS.PROPLIGHT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPLIGHT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("setpiece_light_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x000538A8 File Offset: 0x00051AA8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x000538AA File Offset: 0x00051AAA
	public void OnSpawn(GameObject inst)
	{
	}
}
