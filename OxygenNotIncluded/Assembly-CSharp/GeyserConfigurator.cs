using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x020007DF RID: 2015
[AddComponentMenu("KMonoBehaviour/scripts/GeyserConfigurator")]
public class GeyserConfigurator : KMonoBehaviour
{
	// Token: 0x060038FF RID: 14591 RVA: 0x0013CC8C File Offset: 0x0013AE8C
	public static GeyserConfigurator.GeyserType FindType(HashedString typeId)
	{
		GeyserConfigurator.GeyserType geyserType = null;
		if (typeId != HashedString.Invalid)
		{
			geyserType = GeyserConfigurator.geyserTypes.Find((GeyserConfigurator.GeyserType t) => t.id == typeId);
		}
		if (geyserType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a geyser with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return geyserType;
	}

	// Token: 0x06003900 RID: 14592 RVA: 0x0013CCF5 File Offset: 0x0013AEF5
	public GeyserConfigurator.GeyserInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x06003901 RID: 14593 RVA: 0x0013CD10 File Offset: 0x0013AF10
	private GeyserConfigurator.GeyserInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		KRandom randomSource = new KRandom(SaveLoader.Instance.clusterDetailSave.globalWorldSeed + (int)base.transform.GetPosition().x + (int)base.transform.GetPosition().y);
		return new GeyserConfigurator.GeyserInstanceConfiguration
		{
			typeId = typeId,
			rateRoll = this.Roll(randomSource, min, max),
			iterationLengthRoll = this.Roll(randomSource, 0f, 1f),
			iterationPercentRoll = this.Roll(randomSource, min, max),
			yearLengthRoll = this.Roll(randomSource, 0f, 1f),
			yearPercentRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x0013CDBD File Offset: 0x0013AFBD
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x040025C1 RID: 9665
	private static List<GeyserConfigurator.GeyserType> geyserTypes;

	// Token: 0x040025C2 RID: 9666
	public HashedString presetType;

	// Token: 0x040025C3 RID: 9667
	public float presetMin;

	// Token: 0x040025C4 RID: 9668
	public float presetMax = 1f;

	// Token: 0x02001594 RID: 5524
	public enum GeyserShape
	{
		// Token: 0x04006909 RID: 26889
		Gas,
		// Token: 0x0400690A RID: 26890
		Liquid,
		// Token: 0x0400690B RID: 26891
		Molten
	}

	// Token: 0x02001595 RID: 5525
	public class GeyserType
	{
		// Token: 0x06008805 RID: 34821 RVA: 0x0030D8A8 File Offset: 0x0030BAA8
		public GeyserType(string id, SimHashes element, GeyserConfigurator.GeyserShape shape, float temperature, float minRatePerCycle, float maxRatePerCycle, float maxPressure, float minIterationLength = 60f, float maxIterationLength = 1140f, float minIterationPercent = 0.1f, float maxIterationPercent = 0.9f, float minYearLength = 15000f, float maxYearLength = 135000f, float minYearPercent = 0.4f, float maxYearPercent = 0.8f, float geyserTemperature = 372.15f, string DlcID = "")
		{
			this.id = id;
			this.idHash = id;
			this.element = element;
			this.shape = shape;
			this.temperature = temperature;
			this.minRatePerCycle = minRatePerCycle;
			this.maxRatePerCycle = maxRatePerCycle;
			this.maxPressure = maxPressure;
			this.minIterationLength = minIterationLength;
			this.maxIterationLength = maxIterationLength;
			this.minIterationPercent = minIterationPercent;
			this.maxIterationPercent = maxIterationPercent;
			this.minYearLength = minYearLength;
			this.maxYearLength = maxYearLength;
			this.minYearPercent = minYearPercent;
			this.maxYearPercent = maxYearPercent;
			this.DlcID = DlcID;
			this.geyserTemperature = geyserTemperature;
			if (GeyserConfigurator.geyserTypes == null)
			{
				GeyserConfigurator.geyserTypes = new List<GeyserConfigurator.GeyserType>();
			}
			GeyserConfigurator.geyserTypes.Add(this);
		}

		// Token: 0x06008806 RID: 34822 RVA: 0x0030D973 File Offset: 0x0030BB73
		public GeyserConfigurator.GeyserType AddDisease(SimUtil.DiseaseInfo diseaseInfo)
		{
			this.diseaseInfo = diseaseInfo;
			return this;
		}

		// Token: 0x06008807 RID: 34823 RVA: 0x0030D980 File Offset: 0x0030BB80
		public GeyserType()
		{
			this.id = "Blank";
			this.element = SimHashes.Void;
			this.temperature = 0f;
			this.minRatePerCycle = 0f;
			this.maxRatePerCycle = 0f;
			this.maxPressure = 0f;
			this.minIterationLength = 0f;
			this.maxIterationLength = 0f;
			this.minIterationPercent = 0f;
			this.maxIterationPercent = 0f;
			this.minYearLength = 0f;
			this.maxYearLength = 0f;
			this.minYearPercent = 0f;
			this.maxYearPercent = 0f;
			this.geyserTemperature = 0f;
			this.DlcID = "";
		}

		// Token: 0x0400690C RID: 26892
		public string id;

		// Token: 0x0400690D RID: 26893
		public HashedString idHash;

		// Token: 0x0400690E RID: 26894
		public SimHashes element;

		// Token: 0x0400690F RID: 26895
		public GeyserConfigurator.GeyserShape shape;

		// Token: 0x04006910 RID: 26896
		public float temperature;

		// Token: 0x04006911 RID: 26897
		public float minRatePerCycle;

		// Token: 0x04006912 RID: 26898
		public float maxRatePerCycle;

		// Token: 0x04006913 RID: 26899
		public float maxPressure;

		// Token: 0x04006914 RID: 26900
		public SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;

		// Token: 0x04006915 RID: 26901
		public float minIterationLength;

		// Token: 0x04006916 RID: 26902
		public float maxIterationLength;

		// Token: 0x04006917 RID: 26903
		public float minIterationPercent;

		// Token: 0x04006918 RID: 26904
		public float maxIterationPercent;

		// Token: 0x04006919 RID: 26905
		public float minYearLength;

		// Token: 0x0400691A RID: 26906
		public float maxYearLength;

		// Token: 0x0400691B RID: 26907
		public float minYearPercent;

		// Token: 0x0400691C RID: 26908
		public float maxYearPercent;

		// Token: 0x0400691D RID: 26909
		public float geyserTemperature;

		// Token: 0x0400691E RID: 26910
		public string DlcID;

		// Token: 0x0400691F RID: 26911
		public const string BLANK_ID = "Blank";

		// Token: 0x04006920 RID: 26912
		public const SimHashes BLANK_ELEMENT = SimHashes.Void;

		// Token: 0x04006921 RID: 26913
		public const string BLANK_DLCID = "";
	}

	// Token: 0x02001596 RID: 5526
	[Serializable]
	public class GeyserInstanceConfiguration
	{
		// Token: 0x06008808 RID: 34824 RVA: 0x0030DA4E File Offset: 0x0030BC4E
		public Geyser.GeyserModification GetModifier()
		{
			return this.modifier;
		}

		// Token: 0x06008809 RID: 34825 RVA: 0x0030DA58 File Offset: 0x0030BC58
		public void Init(bool reinit = false)
		{
			if (this.didInit && !reinit)
			{
				return;
			}
			this.didInit = true;
			this.scaledRate = this.Resample(this.rateRoll, this.geyserType.minRatePerCycle, this.geyserType.maxRatePerCycle);
			this.scaledIterationLength = this.Resample(this.iterationLengthRoll, this.geyserType.minIterationLength, this.geyserType.maxIterationLength);
			this.scaledIterationPercent = this.Resample(this.iterationPercentRoll, this.geyserType.minIterationPercent, this.geyserType.maxIterationPercent);
			this.scaledYearLength = this.Resample(this.yearLengthRoll, this.geyserType.minYearLength, this.geyserType.maxYearLength);
			this.scaledYearPercent = this.Resample(this.yearPercentRoll, this.geyserType.minYearPercent, this.geyserType.maxYearPercent);
		}

		// Token: 0x0600880A RID: 34826 RVA: 0x0030DB40 File Offset: 0x0030BD40
		public void SetModifier(Geyser.GeyserModification modifier)
		{
			this.modifier = modifier;
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x0600880B RID: 34827 RVA: 0x0030DB49 File Offset: 0x0030BD49
		public GeyserConfigurator.GeyserType geyserType
		{
			get
			{
				return GeyserConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x0600880C RID: 34828 RVA: 0x0030DB58 File Offset: 0x0030BD58
		private float GetModifiedValue(float geyserVariable, float modifier, Geyser.ModificationMethod method)
		{
			float num = geyserVariable;
			if (method != Geyser.ModificationMethod.Values)
			{
				if (method == Geyser.ModificationMethod.Percentages)
				{
					num += geyserVariable * modifier;
				}
			}
			else
			{
				num += modifier;
			}
			return num;
		}

		// Token: 0x0600880D RID: 34829 RVA: 0x0030DB7B File Offset: 0x0030BD7B
		public float GetMaxPressure()
		{
			return this.GetModifiedValue(this.geyserType.maxPressure, this.modifier.maxPressureModifier, Geyser.maxPressureModificationMethod);
		}

		// Token: 0x0600880E RID: 34830 RVA: 0x0030DB9E File Offset: 0x0030BD9E
		public float GetIterationLength()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledIterationLength, this.modifier.iterationDurationModifier, Geyser.IterationDurationModificationMethod);
		}

		// Token: 0x0600880F RID: 34831 RVA: 0x0030DBC3 File Offset: 0x0030BDC3
		public float GetIterationPercent()
		{
			this.Init(false);
			return Mathf.Clamp(this.GetModifiedValue(this.scaledIterationPercent, this.modifier.iterationPercentageModifier, Geyser.IterationPercentageModificationMethod), 0f, 1f);
		}

		// Token: 0x06008810 RID: 34832 RVA: 0x0030DBF7 File Offset: 0x0030BDF7
		public float GetOnDuration()
		{
			return this.GetIterationLength() * this.GetIterationPercent();
		}

		// Token: 0x06008811 RID: 34833 RVA: 0x0030DC06 File Offset: 0x0030BE06
		public float GetOffDuration()
		{
			return this.GetIterationLength() * (1f - this.GetIterationPercent());
		}

		// Token: 0x06008812 RID: 34834 RVA: 0x0030DC1B File Offset: 0x0030BE1B
		public float GetMassPerCycle()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledRate, this.modifier.massPerCycleModifier, Geyser.massModificationMethod);
		}

		// Token: 0x06008813 RID: 34835 RVA: 0x0030DC40 File Offset: 0x0030BE40
		public float GetEmitRate()
		{
			float num = 600f / this.GetIterationLength();
			return this.GetMassPerCycle() / num / this.GetOnDuration();
		}

		// Token: 0x06008814 RID: 34836 RVA: 0x0030DC69 File Offset: 0x0030BE69
		public float GetYearLength()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledYearLength, this.modifier.yearDurationModifier, Geyser.yearDurationModificationMethod);
		}

		// Token: 0x06008815 RID: 34837 RVA: 0x0030DC8E File Offset: 0x0030BE8E
		public float GetYearPercent()
		{
			this.Init(false);
			return Mathf.Clamp(this.GetModifiedValue(this.scaledYearPercent, this.modifier.yearPercentageModifier, Geyser.yearPercentageModificationMethod), 0f, 1f);
		}

		// Token: 0x06008816 RID: 34838 RVA: 0x0030DCC2 File Offset: 0x0030BEC2
		public float GetYearOnDuration()
		{
			return this.GetYearLength() * this.GetYearPercent();
		}

		// Token: 0x06008817 RID: 34839 RVA: 0x0030DCD1 File Offset: 0x0030BED1
		public float GetYearOffDuration()
		{
			return this.GetYearLength() * (1f - this.GetYearPercent());
		}

		// Token: 0x06008818 RID: 34840 RVA: 0x0030DCE6 File Offset: 0x0030BEE6
		public SimHashes GetElement()
		{
			if (!this.modifier.modifyElement || this.modifier.newElement == (SimHashes)0)
			{
				return this.geyserType.element;
			}
			return this.modifier.newElement;
		}

		// Token: 0x06008819 RID: 34841 RVA: 0x0030DD19 File Offset: 0x0030BF19
		public float GetTemperature()
		{
			return this.GetModifiedValue(this.geyserType.temperature, this.modifier.temperatureModifier, Geyser.temperatureModificationMethod);
		}

		// Token: 0x0600881A RID: 34842 RVA: 0x0030DD3C File Offset: 0x0030BF3C
		public byte GetDiseaseIdx()
		{
			return this.geyserType.diseaseInfo.idx;
		}

		// Token: 0x0600881B RID: 34843 RVA: 0x0030DD4E File Offset: 0x0030BF4E
		public int GetDiseaseCount()
		{
			return this.geyserType.diseaseInfo.count;
		}

		// Token: 0x0600881C RID: 34844 RVA: 0x0030DD60 File Offset: 0x0030BF60
		public float GetAverageEmission()
		{
			float num = this.GetEmitRate() * this.GetOnDuration();
			return this.GetYearOnDuration() / this.GetIterationLength() * num / this.GetYearLength();
		}

		// Token: 0x0600881D RID: 34845 RVA: 0x0030DD94 File Offset: 0x0030BF94
		private float Resample(float t, float min, float max)
		{
			float num = 6f;
			float num2 = 0.002472623f;
			float num3 = t * (1f - num2 * 2f) + num2;
			return (-Mathf.Log(1f / num3 - 1f) + num) / (num * 2f) * (max - min) + min;
		}

		// Token: 0x04006922 RID: 26914
		public HashedString typeId;

		// Token: 0x04006923 RID: 26915
		public float rateRoll;

		// Token: 0x04006924 RID: 26916
		public float iterationLengthRoll;

		// Token: 0x04006925 RID: 26917
		public float iterationPercentRoll;

		// Token: 0x04006926 RID: 26918
		public float yearLengthRoll;

		// Token: 0x04006927 RID: 26919
		public float yearPercentRoll;

		// Token: 0x04006928 RID: 26920
		public float scaledRate;

		// Token: 0x04006929 RID: 26921
		public float scaledIterationLength;

		// Token: 0x0400692A RID: 26922
		public float scaledIterationPercent;

		// Token: 0x0400692B RID: 26923
		public float scaledYearLength;

		// Token: 0x0400692C RID: 26924
		public float scaledYearPercent;

		// Token: 0x0400692D RID: 26925
		private bool didInit;

		// Token: 0x0400692E RID: 26926
		private Geyser.GeyserModification modifier;
	}
}
