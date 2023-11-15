using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002DB RID: 731
public class PropDeskConfig : IEntityConfig
{
	// Token: 0x06000ECE RID: 3790 RVA: 0x00051888 File Offset: 0x0004FA88
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x00051890 File Offset: 0x0004FA90
	public GameObject CreatePrefab()
	{
		string id = "PropDesk";
		string name = STRINGS.BUILDINGS.PREFABS.PROPDESK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPDESK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("setpiece_desk_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextEmail));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x00051937 File Offset: 0x0004FB37
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x0005194E File Offset: 0x0004FB4E
	public void OnSpawn(GameObject inst)
	{
	}
}
