using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FA RID: 762
public class PropGravitasShelfConfig : IEntityConfig
{
	// Token: 0x06000F6A RID: 3946 RVA: 0x00053626 File Offset: 0x00051826
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x00053630 File Offset: 0x00051830
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasShelf";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSHELF.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASSHELF.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_shelf_kanim"), "off", Grid.SceneLayer.Building, 2, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x000536C3 File Offset: 0x000518C3
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x000536DA File Offset: 0x000518DA
	public void OnSpawn(GameObject inst)
	{
	}
}
