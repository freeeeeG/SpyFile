using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public class PropFacilityPaintingConfig : IEntityConfig
{
	// Token: 0x06000F0A RID: 3850 RVA: 0x000524FC File Offset: 0x000506FC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x00052504 File Offset: 0x00050704
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityPainting";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYPAINTING.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYPAINTING.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_painting_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06000F0C RID: 3852 RVA: 0x000525B0 File Offset: 0x000507B0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x000525B4 File Offset: 0x000507B4
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
