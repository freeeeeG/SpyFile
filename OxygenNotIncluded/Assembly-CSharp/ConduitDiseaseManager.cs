using System;
using Klei;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;

// Token: 0x020006F2 RID: 1778
public class ConduitDiseaseManager : KCompactedVector<ConduitDiseaseManager.Data>
{
	// Token: 0x060030C3 RID: 12483 RVA: 0x00102A58 File Offset: 0x00100C58
	private static ElemGrowthInfo GetGrowthInfo(byte disease_idx, ushort elem_idx)
	{
		ElemGrowthInfo result;
		if (disease_idx != 255)
		{
			result = Db.Get().Diseases[(int)disease_idx].elemGrowthInfo[(int)elem_idx];
		}
		else
		{
			result = Disease.DEFAULT_GROWTH_INFO;
		}
		return result;
	}

	// Token: 0x060030C4 RID: 12484 RVA: 0x00102A92 File Offset: 0x00100C92
	public ConduitDiseaseManager(ConduitTemperatureManager temperature_manager) : base(0)
	{
		this.temperatureManager = temperature_manager;
	}

	// Token: 0x060030C5 RID: 12485 RVA: 0x00102AA4 File Offset: 0x00100CA4
	public HandleVector<int>.Handle Allocate(HandleVector<int>.Handle temperature_handle, ref ConduitFlow.ConduitContents contents)
	{
		ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
		ConduitDiseaseManager.Data initial_data = new ConduitDiseaseManager.Data(temperature_handle, elementIndex, contents.mass, contents.diseaseIdx, contents.diseaseCount);
		return base.Allocate(initial_data);
	}

	// Token: 0x060030C6 RID: 12486 RVA: 0x00102AE0 File Offset: 0x00100CE0
	public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
	{
		ConduitDiseaseManager.Data data = base.GetData(handle);
		data.diseaseCount = contents.diseaseCount;
		if (contents.diseaseIdx != data.diseaseIdx)
		{
			data.diseaseIdx = contents.diseaseIdx;
			ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
			data.growthInfo = ConduitDiseaseManager.GetGrowthInfo(contents.diseaseIdx, elementIndex);
		}
		base.SetData(handle, data);
	}

	// Token: 0x060030C7 RID: 12487 RVA: 0x00102B44 File Offset: 0x00100D44
	public void Sim200ms(float dt)
	{
		using (new KProfiler.Region("ConduitDiseaseManager.SimUpdate", null))
		{
			for (int i = 0; i < this.data.Count; i++)
			{
				ConduitDiseaseManager.Data data = this.data[i];
				if (data.diseaseIdx != 255)
				{
					float num = data.accumulatedError;
					num += data.growthInfo.CalculateDiseaseCountDelta(data.diseaseCount, data.mass, dt);
					Disease disease = Db.Get().Diseases[(int)data.diseaseIdx];
					float num2 = Disease.HalfLifeToGrowthRate(Disease.CalculateRangeHalfLife(this.temperatureManager.GetTemperature(data.temperatureHandle), ref disease.temperatureRange, ref disease.temperatureHalfLives), dt);
					num += (float)data.diseaseCount * num2 - (float)data.diseaseCount;
					int num3 = (int)num;
					data.accumulatedError = num - (float)num3;
					data.diseaseCount += num3;
					if (data.diseaseCount <= 0)
					{
						data.diseaseCount = 0;
						data.diseaseIdx = byte.MaxValue;
						data.accumulatedError = 0f;
					}
					this.data[i] = data;
				}
			}
		}
	}

	// Token: 0x060030C8 RID: 12488 RVA: 0x00102C94 File Offset: 0x00100E94
	public void ModifyDiseaseCount(HandleVector<int>.Handle h, int disease_count_delta)
	{
		ConduitDiseaseManager.Data data = base.GetData(h);
		data.diseaseCount = Math.Max(0, data.diseaseCount + disease_count_delta);
		if (data.diseaseCount == 0)
		{
			data.diseaseIdx = byte.MaxValue;
		}
		base.SetData(h, data);
	}

	// Token: 0x060030C9 RID: 12489 RVA: 0x00102CDC File Offset: 0x00100EDC
	public void AddDisease(HandleVector<int>.Handle h, byte disease_idx, int disease_count)
	{
		ConduitDiseaseManager.Data data = base.GetData(h);
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, disease_count, data.diseaseIdx, data.diseaseCount);
		data.diseaseIdx = diseaseInfo.idx;
		data.diseaseCount = diseaseInfo.count;
		base.SetData(h, data);
	}

	// Token: 0x04001D55 RID: 7509
	private ConduitTemperatureManager temperatureManager;

	// Token: 0x02001439 RID: 5177
	public struct Data
	{
		// Token: 0x060083FC RID: 33788 RVA: 0x00300938 File Offset: 0x002FEB38
		public Data(HandleVector<int>.Handle temperature_handle, ushort elem_idx, float mass, byte disease_idx, int disease_count)
		{
			this.diseaseIdx = disease_idx;
			this.elemIdx = elem_idx;
			this.mass = mass;
			this.diseaseCount = disease_count;
			this.accumulatedError = 0f;
			this.temperatureHandle = temperature_handle;
			this.growthInfo = ConduitDiseaseManager.GetGrowthInfo(disease_idx, elem_idx);
		}

		// Token: 0x040064B3 RID: 25779
		public byte diseaseIdx;

		// Token: 0x040064B4 RID: 25780
		public ushort elemIdx;

		// Token: 0x040064B5 RID: 25781
		public int diseaseCount;

		// Token: 0x040064B6 RID: 25782
		public float accumulatedError;

		// Token: 0x040064B7 RID: 25783
		public float mass;

		// Token: 0x040064B8 RID: 25784
		public HandleVector<int>.Handle temperatureHandle;

		// Token: 0x040064B9 RID: 25785
		public ElemGrowthInfo growthInfo;
	}
}
