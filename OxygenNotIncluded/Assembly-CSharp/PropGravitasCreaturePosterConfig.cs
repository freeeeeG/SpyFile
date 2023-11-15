using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class PropGravitasCreaturePosterConfig : IEntityConfig
{
	// Token: 0x06000F23 RID: 3875 RVA: 0x000529C8 File Offset: 0x00050BC8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x000529D0 File Offset: 0x00050BD0
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasCreaturePoster";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCREATUREPOSTER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCREATUREPOSTER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_poster_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("storytrait_crittermanipulator_workiversary", UI.USERMENUACTIONS.READLORE.SEARCH_PROPGRAVITASCREATUREPOSTER));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x00052A7D File Offset: 0x00050C7D
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x00052A94 File Offset: 0x00050C94
	public void OnSpawn(GameObject inst)
	{
	}
}
