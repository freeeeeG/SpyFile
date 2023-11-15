using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002CE RID: 718
public class DigPlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x06000E92 RID: 3730 RVA: 0x00050858 File Offset: 0x0004EA58
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(DigPlacerConfig.ID, MISC.PLACERS.DIGPLACER.NAME, Assets.instance.digPlacerAssets.materials[0]);
		Diggable diggable = gameObject.AddOrGet<Diggable>();
		diggable.workTime = 5f;
		diggable.synchronizeAnims = false;
		diggable.workAnims = new HashedString[]
		{
			"place",
			"release"
		};
		diggable.materials = Assets.instance.digPlacerAssets.materials;
		diggable.materialDisplay = gameObject.GetComponentInChildren<MeshRenderer>(true);
		gameObject.AddOrGet<CancellableDig>();
		return gameObject;
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x000508FD File Offset: 0x0004EAFD
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x000508FF File Offset: 0x0004EAFF
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000863 RID: 2147
	public static string ID = "DigPlacer";

	// Token: 0x02000F7F RID: 3967
	[Serializable]
	public class DigPlacerAssets
	{
		// Token: 0x04005611 RID: 22033
		public Material[] materials;
	}
}
