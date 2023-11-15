using System;
using STRINGS;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class MachinePartsConfig : IEntityConfig
{
	// Token: 0x06000990 RID: 2448 RVA: 0x00037E71 File Offset: 0x00036071
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x00037E78 File Offset: 0x00036078
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("MachineParts", ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.DESC, 5f, true, Assets.GetAnim("buildingrelocate_kanim"), "idle", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, null);
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x00037ED2 File Offset: 0x000360D2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x00037ED4 File Offset: 0x000360D4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000604 RID: 1540
	public const string ID = "MachineParts";

	// Token: 0x04000605 RID: 1541
	public static readonly Tag TAG = TagManager.Create("MachineParts");

	// Token: 0x04000606 RID: 1542
	public const float MASS = 5f;
}
