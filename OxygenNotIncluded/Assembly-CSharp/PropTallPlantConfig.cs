using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000303 RID: 771
public class PropTallPlantConfig : IEntityConfig
{
	// Token: 0x06000F9B RID: 3995 RVA: 0x00054102 File Offset: 0x00052302
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x0005410C File Offset: 0x0005230C
	public GameObject CreatePrefab()
	{
		string id = "PropTallPlant";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTALLPLANT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTALLPLANT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_tall_plant_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Polypropylene, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x000541A1 File Offset: 0x000523A1
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x000541B8 File Offset: 0x000523B8
	public void OnSpawn(GameObject inst)
	{
	}
}
