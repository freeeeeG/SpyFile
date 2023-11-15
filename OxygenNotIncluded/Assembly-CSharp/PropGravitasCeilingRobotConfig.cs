using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002EB RID: 747
public class PropGravitasCeilingRobotConfig : IEntityConfig
{
	// Token: 0x06000F1E RID: 3870 RVA: 0x0005290C File Offset: 0x00050B0C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x00052914 File Offset: 0x00050B14
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasCeilingRobot";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCEILINGROBOT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCEILINGROBOT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_ceiling_robot_kanim"), "off", Grid.SceneLayer.Building, 2, 4, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x000529A7 File Offset: 0x00050BA7
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x000529BE File Offset: 0x00050BBE
	public void OnSpawn(GameObject inst)
	{
	}
}
