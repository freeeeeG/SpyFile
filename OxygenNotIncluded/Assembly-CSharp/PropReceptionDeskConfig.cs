using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class PropReceptionDeskConfig : IEntityConfig
{
	// Token: 0x06000F79 RID: 3961 RVA: 0x000538B4 File Offset: 0x00051AB4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x000538BC File Offset: 0x00051ABC
	public GameObject CreatePrefab()
	{
		string id = "PropReceptionDesk";
		string name = STRINGS.BUILDINGS.PREFABS.PROPRECEPTIONDESK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPRECEPTIONDESK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_reception_kanim"), "off", Grid.SceneLayer.Building, 5, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("email_pens", UI.USERMENUACTIONS.READLORE.SEARCH_ELLIESDESK));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x00053982 File Offset: 0x00051B82
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x00053984 File Offset: 0x00051B84
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
