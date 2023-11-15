using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F5 RID: 757
public class PropGravitasLabTableConfig : IEntityConfig
{
	// Token: 0x06000F51 RID: 3921 RVA: 0x00053176 File Offset: 0x00051376
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x00053180 File Offset: 0x00051380
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasLabTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASLABTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASLABTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_lab_table_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x00053219 File Offset: 0x00051419
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x00053230 File Offset: 0x00051430
	public void OnSpawn(GameObject inst)
	{
	}
}
