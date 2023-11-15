using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FE RID: 766
public class PropSkeletonConfig : IEntityConfig
{
	// Token: 0x06000F7E RID: 3966 RVA: 0x000539D0 File Offset: 0x00051BD0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x000539D8 File Offset: 0x00051BD8
	public GameObject CreatePrefab()
	{
		string id = "PropSkeleton";
		string name = STRINGS.BUILDINGS.PREFABS.PROPSKELETON.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPSKELETON.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER5;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("skeleton_poi_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Creature, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x00053A6B File Offset: 0x00051C6B
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x00053A82 File Offset: 0x00051C82
	public void OnSpawn(GameObject inst)
	{
	}
}
