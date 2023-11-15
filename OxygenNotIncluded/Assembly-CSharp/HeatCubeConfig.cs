using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000282 RID: 642
public class HeatCubeConfig : IEntityConfig
{
	// Token: 0x06000CFF RID: 3327 RVA: 0x000489DF File Offset: 0x00046BDF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x000489E8 File Offset: 0x00046BE8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("HeatCube", "Heat Cube", "A cube that holds heat.", 1000f, true, Assets.GetAnim("artifacts_kanim"), "idle_tallstone", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, SORTORDER.BUILDINGELEMENTS, SimHashes.Diamond, new List<Tag>
		{
			GameTags.MiscPickupable,
			GameTags.IndustrialIngredient
		});
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x00048A56 File Offset: 0x00046C56
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x00048A58 File Offset: 0x00046C58
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000792 RID: 1938
	public const string ID = "HeatCube";
}
