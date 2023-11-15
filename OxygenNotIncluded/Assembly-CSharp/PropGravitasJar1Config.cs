using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class PropGravitasJar1Config : IEntityConfig
{
	// Token: 0x06000F47 RID: 3911 RVA: 0x00052FD8 File Offset: 0x000511D8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x00052FE0 File Offset: 0x000511E0
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasJar1";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_jar1_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextDimensionalLore));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x00053085 File Offset: 0x00051285
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x0005309C File Offset: 0x0005129C
	public void OnSpawn(GameObject inst)
	{
	}
}
