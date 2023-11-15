using System;
using UnityEngine;

// Token: 0x0200025F RID: 607
public class AsteroidConfig : IEntityConfig
{
	// Token: 0x06000C31 RID: 3121 RVA: 0x00045867 File Offset: 0x00043A67
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x00045870 File Offset: 0x00043A70
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("Asteroid", "Asteroid", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<WorldInventory>();
		gameObject.AddOrGet<WorldContainer>();
		gameObject.AddOrGet<AsteroidGridEntity>();
		gameObject.AddOrGet<OrbitalMechanics>();
		gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
		gameObject.AddOrGetDef<AlertStateManager.Def>();
		return gameObject;
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x000458BE File Offset: 0x00043ABE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x000458C0 File Offset: 0x00043AC0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000746 RID: 1862
	public const string ID = "Asteroid";
}
