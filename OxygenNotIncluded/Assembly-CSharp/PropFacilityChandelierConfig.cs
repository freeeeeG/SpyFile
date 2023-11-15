using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002DF RID: 735
public class PropFacilityChandelierConfig : IEntityConfig
{
	// Token: 0x06000EE2 RID: 3810 RVA: 0x00051C64 File Offset: 0x0004FE64
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x00051C6C File Offset: 0x0004FE6C
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityChandelier";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHANDELIER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHANDELIER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_chandelier_kanim"), "off", Grid.SceneLayer.Building, 5, 7, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06000EE4 RID: 3812 RVA: 0x00051D18 File Offset: 0x0004FF18
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x00051D1C File Offset: 0x0004FF1C
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
