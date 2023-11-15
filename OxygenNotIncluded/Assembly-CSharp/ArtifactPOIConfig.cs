using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200025E RID: 606
public class ArtifactPOIConfig : IMultiEntityConfig
{
	// Token: 0x06000C2B RID: 3115 RVA: 0x000455AC File Offset: 0x000437AC
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (ArtifactPOIConfig.ArtifactPOIParams artifactPOIParams in this.GenerateConfigs())
		{
			list.Add(ArtifactPOIConfig.CreateArtifactPOI(artifactPOIParams.id, artifactPOIParams.anim, Strings.Get(artifactPOIParams.nameStringKey), Strings.Get(artifactPOIParams.descStringKey), artifactPOIParams.poiType.idHash));
		}
		return list;
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x00045644 File Offset: 0x00043844
	public static GameObject CreateArtifactPOI(string id, string anim, string name, string desc, HashedString poiType)
	{
		GameObject gameObject = EntityTemplates.CreateEntity(id, id, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<ArtifactPOIConfigurator>().presetType = poiType;
		ArtifactPOIClusterGridEntity artifactPOIClusterGridEntity = gameObject.AddOrGet<ArtifactPOIClusterGridEntity>();
		artifactPOIClusterGridEntity.m_name = name;
		artifactPOIClusterGridEntity.m_Anim = anim;
		gameObject.AddOrGetDef<ArtifactPOIStates.Def>();
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextSpaceEntry));
		return gameObject;
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x00045699 File Offset: 0x00043899
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x0004569B File Offset: 0x0004389B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x000456A0 File Offset: 0x000438A0
	private List<ArtifactPOIConfig.ArtifactPOIParams> GenerateConfigs()
	{
		List<ArtifactPOIConfig.ArtifactPOIParams> list = new List<ArtifactPOIConfig.ArtifactPOIParams>();
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_1", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation1", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_2", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation2", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_3", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation3", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_4", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation4", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_5", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation5", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_6", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation6", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_7", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation7", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_8", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation8", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("russels_teapot", new ArtifactPOIConfigurator.ArtifactPOIType("RussellsTeapot", "artifact_TeaPot", true, 30000f, 60000f, "EXPANSION1_ID")));
		list.RemoveAll((ArtifactPOIConfig.ArtifactPOIParams poi) => !poi.poiType.dlcID.IsNullOrWhiteSpace() && !DlcManager.IsContentActive(poi.poiType.dlcID));
		return list;
	}

	// Token: 0x0400073D RID: 1853
	public const string GravitasSpaceStation1 = "GravitasSpaceStation1";

	// Token: 0x0400073E RID: 1854
	public const string GravitasSpaceStation2 = "GravitasSpaceStation2";

	// Token: 0x0400073F RID: 1855
	public const string GravitasSpaceStation3 = "GravitasSpaceStation3";

	// Token: 0x04000740 RID: 1856
	public const string GravitasSpaceStation4 = "GravitasSpaceStation4";

	// Token: 0x04000741 RID: 1857
	public const string GravitasSpaceStation5 = "GravitasSpaceStation5";

	// Token: 0x04000742 RID: 1858
	public const string GravitasSpaceStation6 = "GravitasSpaceStation6";

	// Token: 0x04000743 RID: 1859
	public const string GravitasSpaceStation7 = "GravitasSpaceStation7";

	// Token: 0x04000744 RID: 1860
	public const string GravitasSpaceStation8 = "GravitasSpaceStation8";

	// Token: 0x04000745 RID: 1861
	public const string RussellsTeapot = "RussellsTeapot";

	// Token: 0x02000F69 RID: 3945
	public struct ArtifactPOIParams
	{
		// Token: 0x06007209 RID: 29193 RVA: 0x002BEAC0 File Offset: 0x002BCCC0
		public ArtifactPOIParams(string anim, ArtifactPOIConfigurator.ArtifactPOIType poiType)
		{
			this.id = "ArtifactSpacePOI_" + poiType.id;
			this.anim = anim;
			this.nameStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.ARTIFACT_POI." + poiType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.ARTIFACT_POI." + poiType.id.ToUpper() + ".DESC");
			this.poiType = poiType;
		}

		// Token: 0x040055DB RID: 21979
		public string id;

		// Token: 0x040055DC RID: 21980
		public string anim;

		// Token: 0x040055DD RID: 21981
		public StringKey nameStringKey;

		// Token: 0x040055DE RID: 21982
		public StringKey descStringKey;

		// Token: 0x040055DF RID: 21983
		public ArtifactPOIConfigurator.ArtifactPOIType poiType;
	}
}
