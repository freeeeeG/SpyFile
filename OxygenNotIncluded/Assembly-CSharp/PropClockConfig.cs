using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002DA RID: 730
public class PropClockConfig : IEntityConfig
{
	// Token: 0x06000EC9 RID: 3785 RVA: 0x000517C7 File Offset: 0x0004F9C7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x000517D0 File Offset: 0x0004F9D0
	public GameObject CreatePrefab()
	{
		string id = "PropClock";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCLOCK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCLOCK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("clock_poi_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x0005187C File Offset: 0x0004FA7C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x0005187E File Offset: 0x0004FA7E
	public void OnSpawn(GameObject inst)
	{
	}
}
