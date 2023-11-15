using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E6 RID: 742
public class PropFacilityHangingLightConfig : IEntityConfig
{
	// Token: 0x06000F05 RID: 3845 RVA: 0x000523F8 File Offset: 0x000505F8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x00052400 File Offset: 0x00050600
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityHangingLight";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYLAMP.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYLAMP.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_light_kanim"), "off", Grid.SceneLayer.Building, 1, 4, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06000F07 RID: 3847 RVA: 0x000524AC File Offset: 0x000506AC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x000524B0 File Offset: 0x000506B0
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
