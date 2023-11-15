using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F9 RID: 761
public class PropGravitasRoboticTableConfig : IEntityConfig
{
	// Token: 0x06000F65 RID: 3941 RVA: 0x00053565 File Offset: 0x00051765
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x0005356C File Offset: 0x0005176C
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasRobitcTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASROBTICTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASROBTICTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_robotic_table_kanim"), "off", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x00053605 File Offset: 0x00051805
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x0005361C File Offset: 0x0005181C
	public void OnSpawn(GameObject inst)
	{
	}
}
