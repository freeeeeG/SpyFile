using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020009BA RID: 2490
[SerializationConfig(MemberSerialization.OptIn)]
public class SpaceDestination
{
	// Token: 0x06004A26 RID: 18982 RVA: 0x001A1B10 File Offset: 0x0019FD10
	private static global::Tuple<SimHashes, MathUtil.MinMax> GetRareElement(SimHashes id)
	{
		foreach (global::Tuple<SimHashes, MathUtil.MinMax> tuple in SpaceDestination.RARE_ELEMENTS)
		{
			if (tuple.first == id)
			{
				return tuple;
			}
		}
		return null;
	}

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x06004A27 RID: 18983 RVA: 0x001A1B6C File Offset: 0x0019FD6C
	public int OneBasedDistance
	{
		get
		{
			return this.distance + 1;
		}
	}

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06004A28 RID: 18984 RVA: 0x001A1B76 File Offset: 0x0019FD76
	public float CurrentMass
	{
		get
		{
			return (float)this.GetDestinationType().minimumMass + this.availableMass;
		}
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06004A29 RID: 18985 RVA: 0x001A1B8B File Offset: 0x0019FD8B
	public float AvailableMass
	{
		get
		{
			return this.availableMass;
		}
	}

	// Token: 0x06004A2A RID: 18986 RVA: 0x001A1B94 File Offset: 0x0019FD94
	public SpaceDestination(int id, string type, int distance)
	{
		this.id = id;
		this.type = type;
		this.distance = distance;
		SpaceDestinationType destinationType = this.GetDestinationType();
		this.availableMass = (float)(destinationType.maxiumMass - destinationType.minimumMass);
		this.GenerateSurfaceElements();
		this.GenerateResearchOpportunities();
	}

	// Token: 0x06004A2B RID: 18987 RVA: 0x001A1C10 File Offset: 0x0019FE10
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 9))
		{
			SpaceDestinationType destinationType = this.GetDestinationType();
			this.availableMass = (float)(destinationType.maxiumMass - destinationType.minimumMass);
		}
	}

	// Token: 0x06004A2C RID: 18988 RVA: 0x001A1C4F File Offset: 0x0019FE4F
	public SpaceDestinationType GetDestinationType()
	{
		return Db.Get().SpaceDestinationTypes.Get(this.type);
	}

	// Token: 0x06004A2D RID: 18989 RVA: 0x001A1C68 File Offset: 0x0019FE68
	public SpaceDestination.ResearchOpportunity TryCompleteResearchOpportunity()
	{
		foreach (SpaceDestination.ResearchOpportunity researchOpportunity in this.researchOpportunities)
		{
			if (researchOpportunity.TryComplete(this))
			{
				return researchOpportunity;
			}
		}
		return null;
	}

	// Token: 0x06004A2E RID: 18990 RVA: 0x001A1CC4 File Offset: 0x0019FEC4
	public void GenerateSurfaceElements()
	{
		foreach (KeyValuePair<SimHashes, MathUtil.MinMax> keyValuePair in this.GetDestinationType().elementTable)
		{
			this.recoverableElements.Add(keyValuePair.Key, UnityEngine.Random.value);
		}
	}

	// Token: 0x06004A2F RID: 18991 RVA: 0x001A1D2C File Offset: 0x0019FF2C
	public SpacecraftManager.DestinationAnalysisState AnalysisState()
	{
		return SpacecraftManager.instance.GetDestinationAnalysisState(this);
	}

	// Token: 0x06004A30 RID: 18992 RVA: 0x001A1D3C File Offset: 0x0019FF3C
	public void GenerateResearchOpportunities()
	{
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.UPPERATMO, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.LOWERATMO, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.MAGNETICFIELD, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SURFACE, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SUBSURFACE, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		float num = 0f;
		foreach (global::Tuple<float, int> tuple in SpaceDestination.RARE_ELEMENT_CHANCES)
		{
			num += tuple.first;
		}
		float num2 = UnityEngine.Random.value * num;
		int num3 = 0;
		foreach (global::Tuple<float, int> tuple2 in SpaceDestination.RARE_ELEMENT_CHANCES)
		{
			num2 -= tuple2.first;
			if (num2 <= 0f)
			{
				num3 = tuple2.second;
			}
		}
		for (int i = 0; i < num3; i++)
		{
			this.researchOpportunities[UnityEngine.Random.Range(0, this.researchOpportunities.Count)].discoveredRareResource = SpaceDestination.RARE_ELEMENTS[UnityEngine.Random.Range(0, SpaceDestination.RARE_ELEMENTS.Count)].first;
		}
		if (UnityEngine.Random.value < 0.33f)
		{
			int index = UnityEngine.Random.Range(0, this.researchOpportunities.Count);
			this.researchOpportunities[index].discoveredRareItem = SpaceDestination.RARE_ITEMS[UnityEngine.Random.Range(0, SpaceDestination.RARE_ITEMS.Count)].first;
		}
	}

	// Token: 0x06004A31 RID: 18993 RVA: 0x001A1F34 File Offset: 0x001A0134
	public float GetResourceValue(SimHashes resource, float roll)
	{
		if (this.GetDestinationType().elementTable.ContainsKey(resource))
		{
			return this.GetDestinationType().elementTable[resource].Lerp(roll);
		}
		if (SpaceDestinationTypes.extendedElementTable.ContainsKey(resource))
		{
			return SpaceDestinationTypes.extendedElementTable[resource].Lerp(roll);
		}
		return 0f;
	}

	// Token: 0x06004A32 RID: 18994 RVA: 0x001A1F98 File Offset: 0x001A0198
	public Dictionary<SimHashes, float> GetMissionResourceResult(float totalCargoSpace, float reservedMass, bool solids = true, bool liquids = true, bool gasses = true)
	{
		Dictionary<SimHashes, float> dictionary = new Dictionary<SimHashes, float>();
		float num = 0f;
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && gasses))
			{
				num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value);
			}
		}
		float num2 = Mathf.Min(this.CurrentMass + reservedMass - (float)this.GetDestinationType().minimumMass, totalCargoSpace);
		foreach (KeyValuePair<SimHashes, float> keyValuePair2 in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair2.Key).IsSolid && solids) || (ElementLoader.FindElementByHash(keyValuePair2.Key).IsLiquid && liquids) || (ElementLoader.FindElementByHash(keyValuePair2.Key).IsGas && gasses))
			{
				float value = num2 * (this.GetResourceValue(keyValuePair2.Key, keyValuePair2.Value) / num);
				dictionary.Add(keyValuePair2.Key, value);
			}
		}
		return dictionary;
	}

	// Token: 0x06004A33 RID: 18995 RVA: 0x001A210C File Offset: 0x001A030C
	public Dictionary<Tag, int> GetRecoverableEntities()
	{
		Dictionary<Tag, int> dictionary = new Dictionary<Tag, int>();
		Dictionary<string, int> recoverableEntities = this.GetDestinationType().recoverableEntities;
		if (recoverableEntities != null)
		{
			foreach (KeyValuePair<string, int> keyValuePair in recoverableEntities)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		return dictionary;
	}

	// Token: 0x06004A34 RID: 18996 RVA: 0x001A2184 File Offset: 0x001A0384
	public Dictionary<Tag, int> GetMissionEntityResult()
	{
		return this.GetRecoverableEntities();
	}

	// Token: 0x06004A35 RID: 18997 RVA: 0x001A218C File Offset: 0x001A038C
	public float ReserveResources(CargoBay bay)
	{
		float num = 0f;
		if (bay != null)
		{
			Storage component = bay.GetComponent<Storage>();
			foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
			{
				if (this.HasElementType(bay.storageType))
				{
					num += component.capacityKg;
					this.availableMass = Mathf.Max(0f, this.availableMass - component.capacityKg);
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x06004A36 RID: 18998 RVA: 0x001A2228 File Offset: 0x001A0428
	public bool HasElementType(CargoBay.CargoType type)
	{
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && type == CargoBay.CargoType.Solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && type == CargoBay.CargoType.Liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && type == CargoBay.CargoType.Gasses))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004A37 RID: 18999 RVA: 0x001A22C0 File Offset: 0x001A04C0
	public void Replenish(float dt)
	{
		SpaceDestinationType destinationType = this.GetDestinationType();
		if (this.CurrentMass < (float)destinationType.maxiumMass)
		{
			this.availableMass += destinationType.replishmentPerSim1000ms;
		}
	}

	// Token: 0x06004A38 RID: 19000 RVA: 0x001A22F8 File Offset: 0x001A04F8
	public float GetAvailableResourcesPercentage(CargoBay.CargoType cargoType)
	{
		float num = 0f;
		float totalMass = this.GetTotalMass();
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && cargoType == CargoBay.CargoType.Solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && cargoType == CargoBay.CargoType.Liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && cargoType == CargoBay.CargoType.Gasses))
			{
				num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value) / totalMass;
			}
		}
		return num;
	}

	// Token: 0x06004A39 RID: 19001 RVA: 0x001A23B0 File Offset: 0x001A05B0
	public float GetTotalMass()
	{
		float num = 0f;
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value);
		}
		return num;
	}

	// Token: 0x040030CC RID: 12492
	private const int MASS_TO_RECOVER_AMOUNT = 1000;

	// Token: 0x040030CD RID: 12493
	private static List<global::Tuple<float, int>> RARE_ELEMENT_CHANCES = new List<global::Tuple<float, int>>
	{
		new global::Tuple<float, int>(1f, 0),
		new global::Tuple<float, int>(0.33f, 1),
		new global::Tuple<float, int>(0.03f, 2)
	};

	// Token: 0x040030CE RID: 12494
	private static readonly List<global::Tuple<SimHashes, MathUtil.MinMax>> RARE_ELEMENTS = new List<global::Tuple<SimHashes, MathUtil.MinMax>>
	{
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Katairite, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Niobium, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Fullerene, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Isoresin, new MathUtil.MinMax(1f, 10f))
	};

	// Token: 0x040030CF RID: 12495
	private const float RARE_ITEM_CHANCE = 0.33f;

	// Token: 0x040030D0 RID: 12496
	private static readonly List<global::Tuple<string, MathUtil.MinMax>> RARE_ITEMS = new List<global::Tuple<string, MathUtil.MinMax>>
	{
		new global::Tuple<string, MathUtil.MinMax>("GeneShufflerRecharge", new MathUtil.MinMax(1f, 2f))
	};

	// Token: 0x040030D1 RID: 12497
	[Serialize]
	public int id;

	// Token: 0x040030D2 RID: 12498
	[Serialize]
	public string type;

	// Token: 0x040030D3 RID: 12499
	public bool startAnalyzed;

	// Token: 0x040030D4 RID: 12500
	[Serialize]
	public int distance;

	// Token: 0x040030D5 RID: 12501
	[Serialize]
	public float activePeriod = 20f;

	// Token: 0x040030D6 RID: 12502
	[Serialize]
	public float inactivePeriod = 10f;

	// Token: 0x040030D7 RID: 12503
	[Serialize]
	public float startingOrbitPercentage;

	// Token: 0x040030D8 RID: 12504
	[Serialize]
	public Dictionary<SimHashes, float> recoverableElements = new Dictionary<SimHashes, float>();

	// Token: 0x040030D9 RID: 12505
	[Serialize]
	public List<SpaceDestination.ResearchOpportunity> researchOpportunities = new List<SpaceDestination.ResearchOpportunity>();

	// Token: 0x040030DA RID: 12506
	[Serialize]
	private float availableMass;

	// Token: 0x02001844 RID: 6212
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ResearchOpportunity
	{
		// Token: 0x06009158 RID: 37208 RVA: 0x00329753 File Offset: 0x00327953
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (this.discoveredRareResource == (SimHashes)0)
			{
				this.discoveredRareResource = SimHashes.Void;
			}
			if (this.dataValue > 50)
			{
				this.dataValue = 50;
			}
		}

		// Token: 0x06009159 RID: 37209 RVA: 0x0032977A File Offset: 0x0032797A
		public ResearchOpportunity(string description, int pointValue)
		{
			this.description = description;
			this.dataValue = pointValue;
		}

		// Token: 0x0600915A RID: 37210 RVA: 0x0032979C File Offset: 0x0032799C
		public bool TryComplete(SpaceDestination destination)
		{
			if (!this.completed)
			{
				this.completed = true;
				if (this.discoveredRareResource != SimHashes.Void && !destination.recoverableElements.ContainsKey(this.discoveredRareResource))
				{
					destination.recoverableElements.Add(this.discoveredRareResource, UnityEngine.Random.value);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0400717D RID: 29053
		[Serialize]
		public string description;

		// Token: 0x0400717E RID: 29054
		[Serialize]
		public int dataValue;

		// Token: 0x0400717F RID: 29055
		[Serialize]
		public bool completed;

		// Token: 0x04007180 RID: 29056
		[Serialize]
		public SimHashes discoveredRareResource = SimHashes.Void;

		// Token: 0x04007181 RID: 29057
		[Serialize]
		public string discoveredRareItem;
	}
}
