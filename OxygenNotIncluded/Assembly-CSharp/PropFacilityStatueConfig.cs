using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E8 RID: 744
public class PropFacilityStatueConfig : IEntityConfig
{
	// Token: 0x06000F0F RID: 3855 RVA: 0x00052600 File Offset: 0x00050800
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x00052608 File Offset: 0x00050808
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityStatue";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYSTATUE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYSTATUE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_statue_kanim"), "off", Grid.SceneLayer.Building, 5, 9, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06000F11 RID: 3857 RVA: 0x000526B5 File Offset: 0x000508B5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x000526B8 File Offset: 0x000508B8
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
