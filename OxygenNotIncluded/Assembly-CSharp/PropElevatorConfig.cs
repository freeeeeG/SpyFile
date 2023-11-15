using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002DC RID: 732
public class PropElevatorConfig : IEntityConfig
{
	// Token: 0x06000ED3 RID: 3795 RVA: 0x00051958 File Offset: 0x0004FB58
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x00051960 File Offset: 0x0004FB60
	public GameObject CreatePrefab()
	{
		string id = "PropElevator";
		string name = STRINGS.BUILDINGS.PREFABS.PROPELEVATOR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPELEVATOR.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_elevator_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06000ED5 RID: 3797 RVA: 0x00051A0C File Offset: 0x0004FC0C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x00051A10 File Offset: 0x0004FC10
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
