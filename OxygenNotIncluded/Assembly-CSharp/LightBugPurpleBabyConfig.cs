using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000107 RID: 263
[EntityConfigOrder(2)]
public class LightBugPurpleBabyConfig : IEntityConfig
{
	// Token: 0x060004F7 RID: 1271 RVA: 0x000231A3 File Offset: 0x000213A3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x000231AA File Offset: 0x000213AA
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPurpleConfig.CreateLightBug("LightBugPurpleBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugPurple", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x000231E8 File Offset: 0x000213E8
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x000231EA File Offset: 0x000213EA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400035E RID: 862
	public const string ID = "LightBugPurpleBaby";
}
