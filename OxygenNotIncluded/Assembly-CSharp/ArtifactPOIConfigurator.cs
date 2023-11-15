using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000989 RID: 2441
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIConfigurator")]
public class ArtifactPOIConfigurator : KMonoBehaviour
{
	// Token: 0x06004806 RID: 18438 RVA: 0x001966D8 File Offset: 0x001948D8
	public static ArtifactPOIConfigurator.ArtifactPOIType FindType(HashedString typeId)
	{
		ArtifactPOIConfigurator.ArtifactPOIType artifactPOIType = null;
		if (typeId != HashedString.Invalid)
		{
			artifactPOIType = ArtifactPOIConfigurator._poiTypes.Find((ArtifactPOIConfigurator.ArtifactPOIType t) => t.id == typeId);
		}
		if (artifactPOIType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a harvestable poi with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return artifactPOIType;
	}

	// Token: 0x06004807 RID: 18439 RVA: 0x00196741 File Offset: 0x00194941
	public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x06004808 RID: 18440 RVA: 0x0019675C File Offset: 0x0019495C
	private ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
		ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
		Vector3 position = ClusterGrid.Instance.GetPosition(component);
		KRandom randomSource = new KRandom(globalWorldSeed + (int)position.x + (int)position.y);
		return new ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration
		{
			typeId = typeId,
			rechargeRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x06004809 RID: 18441 RVA: 0x001967BC File Offset: 0x001949BC
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x04002FBE RID: 12222
	private static List<ArtifactPOIConfigurator.ArtifactPOIType> _poiTypes;

	// Token: 0x04002FBF RID: 12223
	public static ArtifactPOIConfigurator.ArtifactPOIType defaultArtifactPoiType = new ArtifactPOIConfigurator.ArtifactPOIType("HarvestablePOIArtifacts", null, false, 30000f, 60000f, "EXPANSION1_ID");

	// Token: 0x04002FC0 RID: 12224
	public HashedString presetType;

	// Token: 0x04002FC1 RID: 12225
	public float presetMin;

	// Token: 0x04002FC2 RID: 12226
	public float presetMax = 1f;

	// Token: 0x020017F8 RID: 6136
	public class ArtifactPOIType
	{
		// Token: 0x06009003 RID: 36867 RVA: 0x00324628 File Offset: 0x00322828
		public ArtifactPOIType(string id, string harvestableArtifactID = null, bool destroyOnHarvest = false, float poiRechargeTimeMin = 30000f, float poiRechargeTimeMax = 60000f, string dlcID = "EXPANSION1_ID")
		{
			this.id = id;
			this.idHash = id;
			this.harvestableArtifactID = harvestableArtifactID;
			this.destroyOnHarvest = destroyOnHarvest;
			this.poiRechargeTimeMin = poiRechargeTimeMin;
			this.poiRechargeTimeMax = poiRechargeTimeMax;
			this.dlcID = dlcID;
			if (ArtifactPOIConfigurator._poiTypes == null)
			{
				ArtifactPOIConfigurator._poiTypes = new List<ArtifactPOIConfigurator.ArtifactPOIType>();
			}
			ArtifactPOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x0400707F RID: 28799
		public string id;

		// Token: 0x04007080 RID: 28800
		public HashedString idHash;

		// Token: 0x04007081 RID: 28801
		public string harvestableArtifactID;

		// Token: 0x04007082 RID: 28802
		public bool destroyOnHarvest;

		// Token: 0x04007083 RID: 28803
		public float poiRechargeTimeMin;

		// Token: 0x04007084 RID: 28804
		public float poiRechargeTimeMax;

		// Token: 0x04007085 RID: 28805
		public string dlcID;

		// Token: 0x04007086 RID: 28806
		public List<string> orbitalObject = new List<string>
		{
			Db.Get().OrbitalTypeCategories.gravitas.Id
		};
	}

	// Token: 0x020017F9 RID: 6137
	[Serializable]
	public class ArtifactPOIInstanceConfiguration
	{
		// Token: 0x06009004 RID: 36868 RVA: 0x003246B8 File Offset: 0x003228B8
		private void Init()
		{
			if (this.didInit)
			{
				return;
			}
			this.didInit = true;
			this.poiRechargeTime = MathUtil.ReRange(this.rechargeRoll, 0f, 1f, this.poiType.poiRechargeTimeMin, this.poiType.poiRechargeTimeMax);
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06009005 RID: 36869 RVA: 0x00324706 File Offset: 0x00322906
		public ArtifactPOIConfigurator.ArtifactPOIType poiType
		{
			get
			{
				return ArtifactPOIConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x06009006 RID: 36870 RVA: 0x00324713 File Offset: 0x00322913
		public bool DestroyOnHarvest()
		{
			this.Init();
			return this.poiType.destroyOnHarvest;
		}

		// Token: 0x06009007 RID: 36871 RVA: 0x00324726 File Offset: 0x00322926
		public string GetArtifactID()
		{
			this.Init();
			return this.poiType.harvestableArtifactID;
		}

		// Token: 0x06009008 RID: 36872 RVA: 0x00324739 File Offset: 0x00322939
		public float GetRechargeTime()
		{
			this.Init();
			return this.poiRechargeTime;
		}

		// Token: 0x04007087 RID: 28807
		public HashedString typeId;

		// Token: 0x04007088 RID: 28808
		private bool didInit;

		// Token: 0x04007089 RID: 28809
		public float rechargeRoll;

		// Token: 0x0400708A RID: 28810
		private float poiRechargeTime;
	}
}
