using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class PropTableConfig : IEntityConfig
{
	// Token: 0x06000F96 RID: 3990 RVA: 0x00054044 File Offset: 0x00052244
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x0005404C File Offset: 0x0005224C
	public GameObject CreatePrefab()
	{
		string id = "PropTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("table_breakroom_kanim"), "off", Grid.SceneLayer.Building, 3, 1, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x000540E1 File Offset: 0x000522E1
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x000540F8 File Offset: 0x000522F8
	public void OnSpawn(GameObject inst)
	{
	}
}
