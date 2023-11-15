using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F4 RID: 756
public class PropGravitasJar2Config : IEntityConfig
{
	// Token: 0x06000F4C RID: 3916 RVA: 0x000530A6 File Offset: 0x000512A6
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x000530B0 File Offset: 0x000512B0
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasJar2";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR2.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASJAR2.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_jar2_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06000F4E RID: 3918 RVA: 0x00053155 File Offset: 0x00051355
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x0005316C File Offset: 0x0005136C
	public void OnSpawn(GameObject inst)
	{
	}
}
