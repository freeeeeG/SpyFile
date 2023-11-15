using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E0 RID: 736
public class PropFacilityCouchConfig : IEntityConfig
{
	// Token: 0x06000EE7 RID: 3815 RVA: 0x00051D68 File Offset: 0x0004FF68
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x00051D70 File Offset: 0x0004FF70
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityCouch";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCOUCH.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCOUCH.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_couch_kanim"), "off", Grid.SceneLayer.Building, 4, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x00051E1C File Offset: 0x0005001C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x00051E20 File Offset: 0x00050020
	public void OnSpawn(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		int cell = Grid.PosToCell(inst);
		foreach (CellOffset offset in component.OccupiedCellsOffsets)
		{
			Grid.GravitasFacility[Grid.OffsetCell(cell, offset)] = true;
		}
	}
}
