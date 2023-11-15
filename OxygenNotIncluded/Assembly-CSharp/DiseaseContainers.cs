﻿using System;
using System.Collections.Generic;
using Database;
using Klei;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using UnityEngine;

// Token: 0x0200076D RID: 1901
public class DiseaseContainers : KGameObjectSplitComponentManager<DiseaseHeader, DiseaseContainer>
{
	// Token: 0x0600347D RID: 13437 RVA: 0x00119DE4 File Offset: 0x00117FE4
	public HandleVector<int>.Handle Add(GameObject go, byte disease_idx, int disease_count)
	{
		DiseaseHeader diseaseHeader = new DiseaseHeader
		{
			diseaseIdx = disease_idx,
			diseaseCount = disease_count,
			primaryElement = go.GetComponent<PrimaryElement>()
		};
		DiseaseContainer diseaseContainer = new DiseaseContainer(go, diseaseHeader.primaryElement.Element.idx);
		if (disease_idx != 255)
		{
			this.EvaluateGrowthConstants(diseaseHeader, ref diseaseContainer);
		}
		return base.Add(go, diseaseHeader, ref diseaseContainer);
	}

	// Token: 0x0600347E RID: 13438 RVA: 0x00119E4C File Offset: 0x0011804C
	protected override void OnCleanUp(HandleVector<int>.Handle h)
	{
		AutoDisinfectable autoDisinfectable = base.GetPayload(h).autoDisinfectable;
		if (autoDisinfectable != null)
		{
			AutoDisinfectableManager.Instance.RemoveAutoDisinfectable(autoDisinfectable);
		}
		base.OnCleanUp(h);
	}

	// Token: 0x0600347F RID: 13439 RVA: 0x00119E84 File Offset: 0x00118084
	public override void Sim200ms(float dt)
	{
		ListPool<int, DiseaseContainers>.PooledList pooledList = ListPool<int, DiseaseContainers>.Allocate();
		pooledList.Capacity = Math.Max(pooledList.Capacity, this.headers.Count);
		for (int i = 0; i < this.headers.Count; i++)
		{
			DiseaseHeader diseaseHeader = this.headers[i];
			if (diseaseHeader.diseaseIdx != 255 && diseaseHeader.primaryElement != null)
			{
				pooledList.Add(i);
			}
		}
		bool radiation_enabled = Sim.IsRadiationEnabled();
		foreach (int index in pooledList)
		{
			DiseaseContainer diseaseContainer = this.payloads[index];
			DiseaseHeader diseaseHeader2 = this.headers[index];
			Disease disease = Db.Get().Diseases[(int)diseaseHeader2.diseaseIdx];
			float num = DiseaseContainers.CalculateDelta(diseaseHeader2, ref diseaseContainer, disease, dt, radiation_enabled);
			num += diseaseContainer.accumulatedError;
			int num2 = (int)num;
			diseaseContainer.accumulatedError = num - (float)num2;
			bool flag = diseaseHeader2.diseaseCount > diseaseContainer.overpopulationCount;
			bool flag2 = diseaseHeader2.diseaseCount + num2 > diseaseContainer.overpopulationCount;
			if (flag != flag2)
			{
				this.EvaluateGrowthConstants(diseaseHeader2, ref diseaseContainer);
			}
			diseaseHeader2.diseaseCount += num2;
			if (diseaseHeader2.diseaseCount <= 0)
			{
				diseaseContainer.accumulatedError = 0f;
				diseaseHeader2.diseaseCount = 0;
				diseaseHeader2.diseaseIdx = byte.MaxValue;
			}
			this.headers[index] = diseaseHeader2;
			this.payloads[index] = diseaseContainer;
		}
		pooledList.Recycle();
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x0011A044 File Offset: 0x00118244
	private static float CalculateDelta(DiseaseHeader header, ref DiseaseContainer container, Disease disease, float dt, bool radiation_enabled)
	{
		return DiseaseContainers.CalculateDelta(header.diseaseCount, container.elemIdx, header.primaryElement.Mass, Grid.PosToCell(header.primaryElement.transform.GetPosition()), header.primaryElement.Temperature, container.instanceGrowthRate, disease, dt, radiation_enabled);
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x0011A098 File Offset: 0x00118298
	public static float CalculateDelta(int disease_count, ushort element_idx, float mass, int environment_cell, float temperature, float tags_multiplier_base, Disease disease, float dt, bool radiation_enabled)
	{
		float num = 0f;
		ElemGrowthInfo elemGrowthInfo = disease.elemGrowthInfo[(int)element_idx];
		num += elemGrowthInfo.CalculateDiseaseCountDelta(disease_count, mass, dt);
		float num2 = Disease.HalfLifeToGrowthRate(Disease.CalculateRangeHalfLife(temperature, ref disease.temperatureRange, ref disease.temperatureHalfLives), dt);
		num += (float)disease_count * num2 - (float)disease_count;
		float num3 = Mathf.Pow(tags_multiplier_base, dt);
		num += (float)disease_count * num3 - (float)disease_count;
		if (Grid.IsValidCell(environment_cell))
		{
			ushort num4 = Grid.ElementIdx[environment_cell];
			ElemExposureInfo elemExposureInfo = disease.elemExposureInfo[(int)num4];
			num += elemExposureInfo.CalculateExposureDiseaseCountDelta(disease_count, dt);
			if (radiation_enabled)
			{
				float num5 = Grid.Radiation[environment_cell];
				if (num5 > 0f)
				{
					num -= num5 * disease.radiationKillRate;
				}
			}
		}
		return num;
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x0011A15C File Offset: 0x0011835C
	public int ModifyDiseaseCount(HandleVector<int>.Handle h, int disease_count_delta)
	{
		DiseaseHeader header = base.GetHeader(h);
		header.diseaseCount = Math.Max(0, header.diseaseCount + disease_count_delta);
		if (header.diseaseCount == 0)
		{
			header.diseaseIdx = byte.MaxValue;
			DiseaseContainer payload = base.GetPayload(h);
			payload.accumulatedError = 0f;
			base.SetPayload(h, ref payload);
		}
		base.SetHeader(h, header);
		return header.diseaseCount;
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x0011A1C8 File Offset: 0x001183C8
	public int AddDisease(HandleVector<int>.Handle h, byte disease_idx, int disease_count)
	{
		DiseaseHeader diseaseHeader;
		DiseaseContainer diseaseContainer;
		base.GetData(h, out diseaseHeader, out diseaseContainer);
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, disease_count, diseaseHeader.diseaseIdx, diseaseHeader.diseaseCount);
		bool flag = diseaseHeader.diseaseIdx != diseaseInfo.idx;
		diseaseHeader.diseaseIdx = diseaseInfo.idx;
		diseaseHeader.diseaseCount = diseaseInfo.count;
		if (flag && diseaseInfo.idx != 255)
		{
			this.EvaluateGrowthConstants(diseaseHeader, ref diseaseContainer);
			base.SetData(h, diseaseHeader, ref diseaseContainer);
		}
		else
		{
			base.SetHeader(h, diseaseHeader);
		}
		if (flag)
		{
			diseaseHeader.primaryElement.Trigger(-283306403, null);
		}
		return diseaseHeader.diseaseCount;
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x0011A268 File Offset: 0x00118468
	private void GetVisualDiseaseIdxAndCount(DiseaseHeader header, ref DiseaseContainer payload, out int disease_idx, out int disease_count)
	{
		if (payload.visualDiseaseProvider == null)
		{
			disease_idx = (int)header.diseaseIdx;
			disease_count = header.diseaseCount;
			return;
		}
		disease_idx = 255;
		disease_count = 0;
		HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(payload.visualDiseaseProvider);
		if (handle != HandleVector<int>.InvalidHandle)
		{
			DiseaseHeader header2 = GameComps.DiseaseContainers.GetHeader(handle);
			disease_idx = (int)header2.diseaseIdx;
			disease_count = header2.diseaseCount;
		}
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x0011A2DC File Offset: 0x001184DC
	public void UpdateOverlayColours()
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Diseases diseases = Db.Get().Diseases;
		Color32 color = new Color32(0, 0, 0, byte.MaxValue);
		for (int i = 0; i < this.headers.Count; i++)
		{
			DiseaseContainer diseaseContainer = this.payloads[i];
			DiseaseHeader diseaseHeader = this.headers[i];
			KBatchedAnimController controller = diseaseContainer.controller;
			if (controller != null)
			{
				Color32 c = color;
				Vector3 position = controller.transform.GetPosition();
				if (visibleArea.Min <= position && position <= visibleArea.Max)
				{
					int num = 0;
					int num2 = 255;
					int num3 = 0;
					this.GetVisualDiseaseIdxAndCount(diseaseHeader, ref diseaseContainer, out num2, out num3);
					if (num2 != 255)
					{
						c = GlobalAssets.Instance.colorSet.GetColorByName(diseases[num2].overlayColourName);
						num = num3;
					}
					if (diseaseContainer.isContainer)
					{
						List<GameObject> items = diseaseHeader.primaryElement.GetComponent<Storage>().items;
						for (int j = 0; j < items.Count; j++)
						{
							GameObject gameObject = items[j];
							if (gameObject != null)
							{
								HandleVector<int>.Handle handle = base.GetHandle(gameObject);
								if (handle.IsValid())
								{
									DiseaseHeader header = base.GetHeader(handle);
									if (header.diseaseCount > num && header.diseaseIdx != 255)
									{
										num = header.diseaseCount;
										c = GlobalAssets.Instance.colorSet.GetColorByName(diseases[(int)header.diseaseIdx].overlayColourName);
									}
								}
							}
						}
					}
					c.a = SimUtil.DiseaseCountToAlpha254(num);
					if (diseaseContainer.conduitType != ConduitType.None)
					{
						ConduitFlow flowManager = Conduit.GetFlowManager(diseaseContainer.conduitType);
						int cell = Grid.PosToCell(position);
						ConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
						if (contents.diseaseIdx != 255 && contents.diseaseCount > num)
						{
							num = contents.diseaseCount;
							c = GlobalAssets.Instance.colorSet.GetColorByName(diseases[(int)contents.diseaseIdx].overlayColourName);
							c.a = byte.MaxValue;
						}
					}
				}
				controller.OverlayColour = c;
			}
		}
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x0011A524 File Offset: 0x00118724
	private void EvaluateGrowthConstants(DiseaseHeader header, ref DiseaseContainer container)
	{
		Disease disease = Db.Get().Diseases[(int)header.diseaseIdx];
		KPrefabID component = header.primaryElement.GetComponent<KPrefabID>();
		ElemGrowthInfo elemGrowthInfo = disease.elemGrowthInfo[(int)header.diseaseIdx];
		container.overpopulationCount = (int)(elemGrowthInfo.maxCountPerKG * header.primaryElement.Mass);
		container.instanceGrowthRate = disease.GetGrowthRateForTags(component.Tags, header.diseaseCount > container.overpopulationCount);
	}

	// Token: 0x06003487 RID: 13447 RVA: 0x0011A5A0 File Offset: 0x001187A0
	public override void Clear()
	{
		base.Clear();
		for (int i = 0; i < this.payloads.Count; i++)
		{
			this.payloads[i].Clear();
		}
		this.headers.Clear();
		this.payloads.Clear();
		this.handles.Clear();
	}
}
