﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002EA RID: 746
public class PropFacilityWallDegreeConfig : IEntityConfig
{
	// Token: 0x06000F19 RID: 3865 RVA: 0x00052808 File Offset: 0x00050A08
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x00052810 File Offset: 0x00050A10
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityWallDegree";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYWALLDEGREE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYWALLDEGREE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_degree_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06000F1B RID: 3867 RVA: 0x000528BC File Offset: 0x00050ABC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x000528C0 File Offset: 0x00050AC0
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
