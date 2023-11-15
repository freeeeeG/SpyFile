using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002EF RID: 751
public class PropGravitasDisplay4Config : IEntityConfig
{
	// Token: 0x06000F32 RID: 3890 RVA: 0x00052C2A File Offset: 0x00050E2A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x00052C34 File Offset: 0x00050E34
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDisplay4";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDISPLAY4.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDISPLAY4.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_display4_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06000F34 RID: 3892 RVA: 0x00052CD9 File Offset: 0x00050ED9
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x00052CF0 File Offset: 0x00050EF0
	public void OnSpawn(GameObject inst)
	{
	}
}
