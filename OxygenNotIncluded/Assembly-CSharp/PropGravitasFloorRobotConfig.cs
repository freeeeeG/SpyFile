using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public class PropGravitasFloorRobotConfig : IEntityConfig
{
	// Token: 0x06000F3D RID: 3901 RVA: 0x00052E58 File Offset: 0x00051058
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x00052E60 File Offset: 0x00051060
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasFloorRobot";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFLOORROBOT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFLOORROBOT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_floor_robot_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06000F3F RID: 3903 RVA: 0x00052EF9 File Offset: 0x000510F9
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x00052F10 File Offset: 0x00051110
	public void OnSpawn(GameObject inst)
	{
	}
}
