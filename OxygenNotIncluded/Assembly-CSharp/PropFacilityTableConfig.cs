using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E9 RID: 745
public class PropFacilityTableConfig : IEntityConfig
{
	// Token: 0x06000F14 RID: 3860 RVA: 0x00052704 File Offset: 0x00050904
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0005270C File Offset: 0x0005090C
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_table_kanim"), "off", Grid.SceneLayer.Building, 4, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x000527B8 File Offset: 0x000509B8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x000527BC File Offset: 0x000509BC
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
