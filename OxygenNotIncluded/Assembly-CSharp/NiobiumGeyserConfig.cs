using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class NiobiumGeyserConfig : IEntityConfig
{
	// Token: 0x0600068E RID: 1678 RVA: 0x0002B020 File Offset: 0x00029220
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0002B028 File Offset: 0x00029228
	public GameObject CreatePrefab()
	{
		GeyserConfigurator.GeyserType geyserType = new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 3500f, 800f, 1600f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "");
		GameObject gameObject = GeyserGenericConfig.CreateGeyser("NiobiumGeyser", "geyser_molten_niobium_kanim", 3, 3, CREATURES.SPECIES.GEYSER.MOLTEN_NIOBIUM.NAME, CREATURES.SPECIES.GEYSER.MOLTEN_NIOBIUM.DESC, geyserType.idHash, geyserType.geyserTemperature);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		return gameObject;
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0002B0CE File Offset: 0x000292CE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0002B0D0 File Offset: 0x000292D0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000498 RID: 1176
	public const string ID = "NiobiumGeyser";
}
