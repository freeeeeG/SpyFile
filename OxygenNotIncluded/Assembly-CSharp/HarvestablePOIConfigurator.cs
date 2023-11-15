using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200099E RID: 2462
[AddComponentMenu("KMonoBehaviour/scripts/HarvestablePOIConfigurator")]
public class HarvestablePOIConfigurator : KMonoBehaviour
{
	// Token: 0x0600494F RID: 18767 RVA: 0x0019D268 File Offset: 0x0019B468
	public static HarvestablePOIConfigurator.HarvestablePOIType FindType(HashedString typeId)
	{
		HarvestablePOIConfigurator.HarvestablePOIType harvestablePOIType = null;
		if (typeId != HashedString.Invalid)
		{
			harvestablePOIType = HarvestablePOIConfigurator._poiTypes.Find((HarvestablePOIConfigurator.HarvestablePOIType t) => t.id == typeId);
		}
		if (harvestablePOIType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a harvestable poi with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return harvestablePOIType;
	}

	// Token: 0x06004950 RID: 18768 RVA: 0x0019D2D1 File Offset: 0x0019B4D1
	public HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x06004951 RID: 18769 RVA: 0x0019D2EC File Offset: 0x0019B4EC
	private HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
		ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
		Vector3 position = ClusterGrid.Instance.GetPosition(component);
		KRandom randomSource = new KRandom(globalWorldSeed + (int)position.x + (int)position.y);
		return new HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration
		{
			typeId = typeId,
			capacityRoll = this.Roll(randomSource, min, max),
			rechargeRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x06004952 RID: 18770 RVA: 0x0019D35B File Offset: 0x0019B55B
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x04003036 RID: 12342
	private static List<HarvestablePOIConfigurator.HarvestablePOIType> _poiTypes;

	// Token: 0x04003037 RID: 12343
	public HashedString presetType;

	// Token: 0x04003038 RID: 12344
	public float presetMin;

	// Token: 0x04003039 RID: 12345
	public float presetMax = 1f;

	// Token: 0x02001816 RID: 6166
	public class HarvestablePOIType
	{
		// Token: 0x06009086 RID: 36998 RVA: 0x00325838 File Offset: 0x00323A38
		public HarvestablePOIType(string id, Dictionary<SimHashes, float> harvestableElements, float poiCapacityMin = 54000f, float poiCapacityMax = 81000f, float poiRechargeMin = 30000f, float poiRechargeMax = 60000f, bool canProvideArtifacts = true, List<string> orbitalObject = null, int maxNumOrbitingObjects = 20, string dlcID = "EXPANSION1_ID")
		{
			this.id = id;
			this.idHash = id;
			this.harvestableElements = harvestableElements;
			this.poiCapacityMin = poiCapacityMin;
			this.poiCapacityMax = poiCapacityMax;
			this.poiRechargeMin = poiRechargeMin;
			this.poiRechargeMax = poiRechargeMax;
			this.canProvideArtifacts = canProvideArtifacts;
			this.orbitalObject = orbitalObject;
			this.maxNumOrbitingObjects = maxNumOrbitingObjects;
			this.dlcID = dlcID;
			if (HarvestablePOIConfigurator._poiTypes == null)
			{
				HarvestablePOIConfigurator._poiTypes = new List<HarvestablePOIConfigurator.HarvestablePOIType>();
			}
			HarvestablePOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x040070ED RID: 28909
		public string id;

		// Token: 0x040070EE RID: 28910
		public HashedString idHash;

		// Token: 0x040070EF RID: 28911
		public Dictionary<SimHashes, float> harvestableElements;

		// Token: 0x040070F0 RID: 28912
		public float poiCapacityMin;

		// Token: 0x040070F1 RID: 28913
		public float poiCapacityMax;

		// Token: 0x040070F2 RID: 28914
		public float poiRechargeMin;

		// Token: 0x040070F3 RID: 28915
		public float poiRechargeMax;

		// Token: 0x040070F4 RID: 28916
		public bool canProvideArtifacts;

		// Token: 0x040070F5 RID: 28917
		public string dlcID;

		// Token: 0x040070F6 RID: 28918
		public List<string> orbitalObject;

		// Token: 0x040070F7 RID: 28919
		public int maxNumOrbitingObjects;
	}

	// Token: 0x02001817 RID: 6167
	[Serializable]
	public class HarvestablePOIInstanceConfiguration
	{
		// Token: 0x06009087 RID: 36999 RVA: 0x003258C0 File Offset: 0x00323AC0
		private void Init()
		{
			if (this.didInit)
			{
				return;
			}
			this.didInit = true;
			this.poiTotalCapacity = MathUtil.ReRange(this.capacityRoll, 0f, 1f, this.poiType.poiCapacityMin, this.poiType.poiCapacityMax);
			this.poiRecharge = MathUtil.ReRange(this.rechargeRoll, 0f, 1f, this.poiType.poiRechargeMin, this.poiType.poiRechargeMax);
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06009088 RID: 37000 RVA: 0x0032593F File Offset: 0x00323B3F
		public HarvestablePOIConfigurator.HarvestablePOIType poiType
		{
			get
			{
				return HarvestablePOIConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x06009089 RID: 37001 RVA: 0x0032594C File Offset: 0x00323B4C
		public Dictionary<SimHashes, float> GetElementsWithWeights()
		{
			this.Init();
			return this.poiType.harvestableElements;
		}

		// Token: 0x0600908A RID: 37002 RVA: 0x0032595F File Offset: 0x00323B5F
		public bool CanProvideArtifacts()
		{
			this.Init();
			return this.poiType.canProvideArtifacts;
		}

		// Token: 0x0600908B RID: 37003 RVA: 0x00325972 File Offset: 0x00323B72
		public float GetMaxCapacity()
		{
			this.Init();
			return this.poiTotalCapacity;
		}

		// Token: 0x0600908C RID: 37004 RVA: 0x00325980 File Offset: 0x00323B80
		public float GetRechargeTime()
		{
			this.Init();
			return this.poiRecharge;
		}

		// Token: 0x040070F8 RID: 28920
		public HashedString typeId;

		// Token: 0x040070F9 RID: 28921
		private bool didInit;

		// Token: 0x040070FA RID: 28922
		public float capacityRoll;

		// Token: 0x040070FB RID: 28923
		public float rechargeRoll;

		// Token: 0x040070FC RID: 28924
		private float poiTotalCapacity;

		// Token: 0x040070FD RID: 28925
		private float poiRecharge;
	}
}
