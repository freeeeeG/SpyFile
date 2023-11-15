using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002EE RID: 750
public class PropGravitasDeskPodiumConfig : IEntityConfig
{
	// Token: 0x06000F2D RID: 3885 RVA: 0x00052B5C File Offset: 0x00050D5C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x00052B64 File Offset: 0x00050D64
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDeskPodium";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESKPODIUM.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESKPODIUM.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_desk_podium_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextDeskPodiumEntry));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x00052C09 File Offset: 0x00050E09
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x00052C20 File Offset: 0x00050E20
	public void OnSpawn(GameObject inst)
	{
	}
}
