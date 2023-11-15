using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000FF RID: 255
[EntityConfigOrder(2)]
public class LightBugBabyConfig : IEntityConfig
{
	// Token: 0x060004C7 RID: 1223 RVA: 0x00022773 File Offset: 0x00020973
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0002277C File Offset: 0x0002097C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugConfig.CreateLightBug("LightBugBaby", CREATURES.SPECIES.LIGHTBUG.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBug", null, false, 5f);
		gameObject.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
		return gameObject;
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x000227D6 File Offset: 0x000209D6
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x000227D8 File Offset: 0x000209D8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000342 RID: 834
	public const string ID = "LightBugBaby";
}
