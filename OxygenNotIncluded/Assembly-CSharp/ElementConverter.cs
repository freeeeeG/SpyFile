using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000787 RID: 1927
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConverter : StateMachineComponent<ElementConverter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600355B RID: 13659 RVA: 0x00120CBB File Offset: 0x0011EEBB
	public void SetWorkSpeedMultiplier(float speed)
	{
		this.workSpeedMultiplier = speed;
	}

	// Token: 0x0600355C RID: 13660 RVA: 0x00120CC4 File Offset: 0x0011EEC4
	public void SetConsumedElementActive(Tag elementId, bool active)
	{
		int i = 0;
		while (i < this.consumedElements.Length)
		{
			if (!(this.consumedElements[i].Tag != elementId))
			{
				this.consumedElements[i].IsActive = active;
				if (!this.ShowInUI)
				{
					break;
				}
				ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
				if (active)
				{
					base.smi.AddStatusItem<ElementConverter.ConsumedElement, Tag>(consumedElement, consumedElement.Tag, ElementConverter.ElementConverterInput, this.consumedElementStatusHandles);
					return;
				}
				base.smi.RemoveStatusItem<Tag>(consumedElement.Tag, this.consumedElementStatusHandles);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600355D RID: 13661 RVA: 0x00120D60 File Offset: 0x0011EF60
	public void SetOutputElementActive(SimHashes element, bool active)
	{
		int i = 0;
		while (i < this.outputElements.Length)
		{
			if (this.outputElements[i].elementHash == element)
			{
				this.outputElements[i].IsActive = active;
				ElementConverter.OutputElement outputElement = this.outputElements[i];
				if (active)
				{
					base.smi.AddStatusItem<ElementConverter.OutputElement, SimHashes>(outputElement, outputElement.elementHash, ElementConverter.ElementConverterOutput, this.outputElementStatusHandles);
					return;
				}
				base.smi.RemoveStatusItem<SimHashes>(outputElement.elementHash, this.outputElementStatusHandles);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x00120DEC File Offset: 0x0011EFEC
	public void SetStorage(Storage storage)
	{
		this.storage = storage;
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x0600355F RID: 13663 RVA: 0x00120DF5 File Offset: 0x0011EFF5
	// (set) Token: 0x06003560 RID: 13664 RVA: 0x00120DFD File Offset: 0x0011EFFD
	public float OutputMultiplier
	{
		get
		{
			return this.outputMultiplier;
		}
		set
		{
			this.outputMultiplier = value;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x06003561 RID: 13665 RVA: 0x00120E06 File Offset: 0x0011F006
	public float AverageConvertRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.outputElements[0].accumulator);
		}
	}

	// Token: 0x06003562 RID: 13666 RVA: 0x00120E28 File Offset: 0x0011F028
	public bool HasEnoughMass(Tag tag, bool includeInactive = false)
	{
		bool result = false;
		List<GameObject> items = this.storage.items;
		foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
		{
			if (!(tag != consumedElement.Tag) && (includeInactive || consumedElement.IsActive))
			{
				float num = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(tag))
					{
						num += gameObject.GetComponent<PrimaryElement>().Mass;
					}
				}
				result = (num >= consumedElement.MassConsumptionRate);
				break;
			}
		}
		return result;
	}

	// Token: 0x06003563 RID: 13667 RVA: 0x00120EE0 File Offset: 0x0011F0E0
	public bool HasEnoughMassToStartConverting(bool includeInactive = false)
	{
		float speedMultiplier = this.GetSpeedMultiplier();
		float num = 1f * speedMultiplier;
		bool flag = includeInactive || this.consumedElements.Length == 0;
		bool flag2 = true;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (includeInactive || consumedElement.IsActive)
			{
				float num2 = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag))
					{
						num2 += gameObject.GetComponent<PrimaryElement>().Mass;
					}
				}
				if (num2 < consumedElement.MassConsumptionRate * num)
				{
					flag2 = false;
					break;
				}
			}
		}
		return flag && flag2;
	}

	// Token: 0x06003564 RID: 13668 RVA: 0x00120FC8 File Offset: 0x0011F1C8
	public bool CanConvertAtAll()
	{
		bool flag = this.consumedElements.Length == 0;
		bool flag2 = true;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (consumedElement.IsActive)
			{
				bool flag3 = false;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag) && gameObject.GetComponent<PrimaryElement>().Mass > 0f)
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					flag2 = false;
					break;
				}
			}
		}
		return flag && flag2;
	}

	// Token: 0x06003565 RID: 13669 RVA: 0x00121087 File Offset: 0x0011F287
	private float GetSpeedMultiplier()
	{
		return this.machinerySpeedAttribute.GetTotalValue() * this.workSpeedMultiplier;
	}

	// Token: 0x06003566 RID: 13670 RVA: 0x0012109C File Offset: 0x0011F29C
	private void ConvertMass()
	{
		float speedMultiplier = this.GetSpeedMultiplier();
		float num = 1f * speedMultiplier;
		bool flag = this.consumedElements.Length == 0;
		float num2 = 1f;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (consumedElement.IsActive)
			{
				float num3 = consumedElement.MassConsumptionRate * num * num2;
				if (num3 <= 0f)
				{
					num2 = 0f;
					break;
				}
				float num4 = 0f;
				for (int j = 0; j < this.storage.items.Count; j++)
				{
					GameObject gameObject = this.storage.items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag))
					{
						PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
						float num5 = Mathf.Min(num3, component.Mass);
						num4 += num5 / num3;
					}
				}
				num2 = Mathf.Min(num2, num4);
			}
		}
		if (!flag || num2 <= 0f)
		{
			return;
		}
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		diseaseInfo.idx = byte.MaxValue;
		diseaseInfo.count = 0;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		for (int k = 0; k < this.consumedElements.Length; k++)
		{
			ElementConverter.ConsumedElement consumedElement2 = this.consumedElements[k];
			if (consumedElement2.IsActive)
			{
				float num9 = consumedElement2.MassConsumptionRate * num * num2;
				Game.Instance.accumulators.Accumulate(consumedElement2.Accumulator, num9);
				for (int l = 0; l < this.storage.items.Count; l++)
				{
					GameObject gameObject2 = this.storage.items[l];
					if (!(gameObject2 == null))
					{
						if (gameObject2.HasTag(consumedElement2.Tag))
						{
							PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
							component2.KeepZeroMassObject = true;
							float num10 = Mathf.Min(num9, component2.Mass);
							int num11 = (int)(num10 / component2.Mass * (float)component2.DiseaseCount);
							float num12 = num10 * component2.Element.specificHeatCapacity;
							num8 += num12;
							num7 += num12 * component2.Temperature;
							component2.Mass -= num10;
							component2.ModifyDiseaseCount(-num11, "ElementConverter.ConvertMass");
							num6 += num10;
							diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo.idx, diseaseInfo.count, component2.DiseaseIdx, num11);
							num9 -= num10;
							if (num9 <= 0f)
							{
								break;
							}
						}
						if (num9 <= 0f)
						{
							global::Debug.Assert(num9 <= 0f);
						}
					}
				}
			}
		}
		float num13 = (num8 > 0f) ? (num7 / num8) : 0f;
		if (this.onConvertMass != null && num6 > 0f)
		{
			this.onConvertMass(num6);
		}
		for (int m = 0; m < this.outputElements.Length; m++)
		{
			ElementConverter.OutputElement outputElement = this.outputElements[m];
			if (outputElement.IsActive)
			{
				SimUtil.DiseaseInfo diseaseInfo2 = diseaseInfo;
				if (this.totalDiseaseWeight <= 0f)
				{
					diseaseInfo2.idx = byte.MaxValue;
					diseaseInfo2.count = 0;
				}
				else
				{
					float num14 = outputElement.diseaseWeight / this.totalDiseaseWeight;
					diseaseInfo2.count = (int)((float)diseaseInfo2.count * num14);
				}
				if (outputElement.addedDiseaseIdx != 255)
				{
					diseaseInfo2 = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo2, new SimUtil.DiseaseInfo
					{
						idx = outputElement.addedDiseaseIdx,
						count = outputElement.addedDiseaseCount
					});
				}
				float num15 = outputElement.massGenerationRate * this.OutputMultiplier * num * num2;
				Game.Instance.accumulators.Accumulate(outputElement.accumulator, num15);
				float temperature;
				if (outputElement.useEntityTemperature || (num13 == 0f && outputElement.minOutputTemperature == 0f))
				{
					temperature = base.GetComponent<PrimaryElement>().Temperature;
				}
				else
				{
					temperature = Mathf.Max(outputElement.minOutputTemperature, num13);
				}
				Element element = ElementLoader.FindElementByHash(outputElement.elementHash);
				if (outputElement.storeOutput)
				{
					PrimaryElement primaryElement = this.storage.AddToPrimaryElement(outputElement.elementHash, num15, temperature);
					if (primaryElement == null)
					{
						if (element.IsGas)
						{
							this.storage.AddGasChunk(outputElement.elementHash, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, true);
						}
						else if (element.IsLiquid)
						{
							this.storage.AddLiquid(outputElement.elementHash, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, true);
						}
						else
						{
							GameObject go = element.substance.SpawnResource(base.transform.GetPosition(), num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, false, false);
							this.storage.Store(go, true, false, true, false);
						}
					}
					else
					{
						primaryElement.AddDisease(diseaseInfo2.idx, diseaseInfo2.count, "ElementConverter.ConvertMass");
					}
				}
				else
				{
					Vector3 vector = new Vector3(base.transform.GetPosition().x + outputElement.outputElementOffset.x, base.transform.GetPosition().y + outputElement.outputElementOffset.y, 0f);
					int num16 = Grid.PosToCell(vector);
					if (element.IsLiquid)
					{
						FallingWater.instance.AddParticle(num16, element.idx, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, false, false, false);
					}
					else if (element.IsSolid)
					{
						element.substance.SpawnResource(vector, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, false, false, false);
					}
					else
					{
						SimMessages.AddRemoveSubstance(num16, outputElement.elementHash, CellEventLogger.Instance.OxygenModifierSimUpdate, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, -1);
					}
				}
				if (outputElement.elementHash == SimHashes.Oxygen || outputElement.elementHash == SimHashes.ContaminatedOxygen)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num15, base.gameObject.GetProperName(), null);
				}
			}
		}
		this.storage.Trigger(-1697596308, base.gameObject);
	}

	// Token: 0x06003567 RID: 13671 RVA: 0x00121708 File Offset: 0x0011F908
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Attributes attributes = base.gameObject.GetAttributes();
		this.machinerySpeedAttribute = attributes.Add(Db.Get().Attributes.MachinerySpeed);
		if (ElementConverter.ElementConverterInput == null)
		{
			ElementConverter.ElementConverterInput = new StatusItem("ElementConverterInput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				ElementConverter.ConsumedElement consumedElement = (ElementConverter.ConsumedElement)data;
				str = str.Replace("{ElementTypes}", consumedElement.Name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedByTag(consumedElement.Tag, consumedElement.Rate, GameUtil.TimeSlice.PerSecond));
				return str;
			});
		}
		if (ElementConverter.ElementConverterOutput == null)
		{
			ElementConverter.ElementConverterOutput = new StatusItem("ElementConverterOutput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				ElementConverter.OutputElement outputElement = (ElementConverter.OutputElement)data;
				str = str.Replace("{ElementTypes}", outputElement.Name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(outputElement.Rate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			});
		}
	}

	// Token: 0x06003568 RID: 13672 RVA: 0x001217E8 File Offset: 0x0011F9E8
	public void SetAllConsumedActive(bool active)
	{
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			this.consumedElements[i].IsActive = active;
		}
		base.smi.sm.canConvert.Set(active, base.smi, false);
	}

	// Token: 0x06003569 RID: 13673 RVA: 0x00121838 File Offset: 0x0011FA38
	public void SetConsumedActive(Tag id, bool active)
	{
		bool flag = this.consumedElements.Length == 0;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ref ElementConverter.ConsumedElement ptr = ref this.consumedElements[i];
			if (ptr.Tag == id)
			{
				ptr.IsActive = active;
				if (active)
				{
					flag = true;
					break;
				}
			}
			flag |= ptr.IsActive;
		}
		base.smi.sm.canConvert.Set(flag, base.smi, false);
	}

	// Token: 0x0600356A RID: 13674 RVA: 0x001218B4 File Offset: 0x0011FAB4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			this.consumedElements[i].Accumulator = Game.Instance.accumulators.Add("ElementsConsumed", this);
		}
		this.totalDiseaseWeight = 0f;
		for (int j = 0; j < this.outputElements.Length; j++)
		{
			this.outputElements[j].accumulator = Game.Instance.accumulators.Add("OutputElements", this);
			this.totalDiseaseWeight += this.outputElements[j].diseaseWeight;
		}
		base.smi.StartSM();
	}

	// Token: 0x0600356B RID: 13675 RVA: 0x00121970 File Offset: 0x0011FB70
	protected override void OnCleanUp()
	{
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			Game.Instance.accumulators.Remove(this.consumedElements[i].Accumulator);
		}
		for (int j = 0; j < this.outputElements.Length; j++)
		{
			Game.Instance.accumulators.Remove(this.outputElements[j].accumulator);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600356C RID: 13676 RVA: 0x001219EC File Offset: 0x0011FBEC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (!this.showDescriptors)
		{
			return list;
		}
		if (this.consumedElements != null)
		{
			foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
			{
				if (consumedElement.IsActive)
				{
					Descriptor item = default(Descriptor);
					item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, consumedElement.Name, GameUtil.GetFormattedMass(consumedElement.MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, consumedElement.Name, GameUtil.GetFormattedMass(consumedElement.MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
					list.Add(item);
				}
			}
		}
		if (this.outputElements != null)
		{
			foreach (ElementConverter.OutputElement outputElement in this.outputElements)
			{
				if (outputElement.IsActive)
				{
					LocString loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_INPUTTEMP;
					LocString loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_INPUTTEMP;
					if (outputElement.useEntityTemperature)
					{
						loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP;
						loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP;
					}
					else if (outputElement.minOutputTemperature > 0f)
					{
						loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINTEMP;
						loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINTEMP;
					}
					Descriptor item2 = new Descriptor(string.Format(loc_string, outputElement.Name, GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(loc_string2, outputElement.Name, GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false);
					list.Add(item2);
				}
			}
		}
		return list;
	}

	// Token: 0x040020A2 RID: 8354
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040020A3 RID: 8355
	[MyCmpReq]
	private Storage storage;

	// Token: 0x040020A4 RID: 8356
	public Action<float> onConvertMass;

	// Token: 0x040020A5 RID: 8357
	private float totalDiseaseWeight = float.MaxValue;

	// Token: 0x040020A6 RID: 8358
	public Operational.State OperationalRequirement = Operational.State.Active;

	// Token: 0x040020A7 RID: 8359
	private AttributeInstance machinerySpeedAttribute;

	// Token: 0x040020A8 RID: 8360
	private float workSpeedMultiplier = 1f;

	// Token: 0x040020A9 RID: 8361
	public bool showDescriptors = true;

	// Token: 0x040020AA RID: 8362
	private const float BASE_INTERVAL = 1f;

	// Token: 0x040020AB RID: 8363
	public ElementConverter.ConsumedElement[] consumedElements;

	// Token: 0x040020AC RID: 8364
	public ElementConverter.OutputElement[] outputElements;

	// Token: 0x040020AD RID: 8365
	public bool ShowInUI = true;

	// Token: 0x040020AE RID: 8366
	private float outputMultiplier = 1f;

	// Token: 0x040020AF RID: 8367
	private Dictionary<Tag, Guid> consumedElementStatusHandles = new Dictionary<Tag, Guid>();

	// Token: 0x040020B0 RID: 8368
	private Dictionary<SimHashes, Guid> outputElementStatusHandles = new Dictionary<SimHashes, Guid>();

	// Token: 0x040020B1 RID: 8369
	private static StatusItem ElementConverterInput;

	// Token: 0x040020B2 RID: 8370
	private static StatusItem ElementConverterOutput;

	// Token: 0x0200150B RID: 5387
	[DebuggerDisplay("{tag} {massConsumptionRate}")]
	[Serializable]
	public struct ConsumedElement
	{
		// Token: 0x060086B7 RID: 34487 RVA: 0x0030912D File Offset: 0x0030732D
		public ConsumedElement(Tag tag, float kgPerSecond, bool isActive = true)
		{
			this.Tag = tag;
			this.MassConsumptionRate = kgPerSecond;
			this.IsActive = isActive;
			this.Accumulator = HandleVector<int>.InvalidHandle;
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x060086B8 RID: 34488 RVA: 0x0030914F File Offset: 0x0030734F
		public string Name
		{
			get
			{
				return this.Tag.ProperName();
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x060086B9 RID: 34489 RVA: 0x0030915C File Offset: 0x0030735C
		public float Rate
		{
			get
			{
				return Game.Instance.accumulators.GetAverageRate(this.Accumulator);
			}
		}

		// Token: 0x04006722 RID: 26402
		public Tag Tag;

		// Token: 0x04006723 RID: 26403
		public float MassConsumptionRate;

		// Token: 0x04006724 RID: 26404
		public bool IsActive;

		// Token: 0x04006725 RID: 26405
		public HandleVector<int>.Handle Accumulator;
	}

	// Token: 0x0200150C RID: 5388
	[Serializable]
	public struct OutputElement
	{
		// Token: 0x060086BA RID: 34490 RVA: 0x00309174 File Offset: 0x00307374
		public OutputElement(float kgPerSecond, SimHashes element, float minOutputTemperature, bool useEntityTemperature = false, bool storeOutput = false, float outputElementOffsetx = 0f, float outputElementOffsety = 0.5f, float diseaseWeight = 1f, byte addedDiseaseIdx = 255, int addedDiseaseCount = 0, bool isActive = true)
		{
			this.elementHash = element;
			this.minOutputTemperature = minOutputTemperature;
			this.useEntityTemperature = useEntityTemperature;
			this.storeOutput = storeOutput;
			this.massGenerationRate = kgPerSecond;
			this.outputElementOffset = new Vector2(outputElementOffsetx, outputElementOffsety);
			this.accumulator = HandleVector<int>.InvalidHandle;
			this.diseaseWeight = diseaseWeight;
			this.addedDiseaseIdx = addedDiseaseIdx;
			this.addedDiseaseCount = addedDiseaseCount;
			this.IsActive = isActive;
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x060086BB RID: 34491 RVA: 0x003091E0 File Offset: 0x003073E0
		public string Name
		{
			get
			{
				return ElementLoader.FindElementByHash(this.elementHash).tag.ProperName();
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x060086BC RID: 34492 RVA: 0x003091F7 File Offset: 0x003073F7
		public float Rate
		{
			get
			{
				return Game.Instance.accumulators.GetAverageRate(this.accumulator);
			}
		}

		// Token: 0x04006726 RID: 26406
		public bool IsActive;

		// Token: 0x04006727 RID: 26407
		public SimHashes elementHash;

		// Token: 0x04006728 RID: 26408
		public float minOutputTemperature;

		// Token: 0x04006729 RID: 26409
		public bool useEntityTemperature;

		// Token: 0x0400672A RID: 26410
		public float massGenerationRate;

		// Token: 0x0400672B RID: 26411
		public bool storeOutput;

		// Token: 0x0400672C RID: 26412
		public Vector2 outputElementOffset;

		// Token: 0x0400672D RID: 26413
		public HandleVector<int>.Handle accumulator;

		// Token: 0x0400672E RID: 26414
		public float diseaseWeight;

		// Token: 0x0400672F RID: 26415
		public byte addedDiseaseIdx;

		// Token: 0x04006730 RID: 26416
		public int addedDiseaseCount;
	}

	// Token: 0x0200150D RID: 5389
	public class StatesInstance : GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.GameInstance
	{
		// Token: 0x060086BD RID: 34493 RVA: 0x0030920E File Offset: 0x0030740E
		public StatesInstance(ElementConverter smi) : base(smi)
		{
			this.selectable = base.GetComponent<KSelectable>();
		}

		// Token: 0x060086BE RID: 34494 RVA: 0x00309224 File Offset: 0x00307424
		public void AddStatusItems()
		{
			if (!base.master.ShowInUI)
			{
				return;
			}
			foreach (ElementConverter.ConsumedElement consumedElement in base.master.consumedElements)
			{
				if (consumedElement.IsActive)
				{
					this.AddStatusItem<ElementConverter.ConsumedElement, Tag>(consumedElement, consumedElement.Tag, ElementConverter.ElementConverterInput, base.master.consumedElementStatusHandles);
				}
			}
			foreach (ElementConverter.OutputElement outputElement in base.master.outputElements)
			{
				if (outputElement.IsActive)
				{
					this.AddStatusItem<ElementConverter.OutputElement, SimHashes>(outputElement, outputElement.elementHash, ElementConverter.ElementConverterOutput, base.master.outputElementStatusHandles);
				}
			}
		}

		// Token: 0x060086BF RID: 34495 RVA: 0x003092D4 File Offset: 0x003074D4
		public void RemoveStatusItems()
		{
			if (!base.master.ShowInUI)
			{
				return;
			}
			for (int i = 0; i < base.master.consumedElements.Length; i++)
			{
				ElementConverter.ConsumedElement consumedElement = base.master.consumedElements[i];
				this.RemoveStatusItem<Tag>(consumedElement.Tag, base.master.consumedElementStatusHandles);
			}
			for (int j = 0; j < base.master.outputElements.Length; j++)
			{
				ElementConverter.OutputElement outputElement = base.master.outputElements[j];
				this.RemoveStatusItem<SimHashes>(outputElement.elementHash, base.master.outputElementStatusHandles);
			}
			base.master.consumedElementStatusHandles.Clear();
			base.master.outputElementStatusHandles.Clear();
		}

		// Token: 0x060086C0 RID: 34496 RVA: 0x00309394 File Offset: 0x00307594
		public void AddStatusItem<ElementType, IDType>(ElementType element, IDType id, StatusItem status, Dictionary<IDType, Guid> collection)
		{
			Guid value = this.selectable.AddStatusItem(status, element);
			collection[id] = value;
		}

		// Token: 0x060086C1 RID: 34497 RVA: 0x003093C0 File Offset: 0x003075C0
		public void RemoveStatusItem<IDType>(IDType id, Dictionary<IDType, Guid> collection)
		{
			Guid guid;
			if (!collection.TryGetValue(id, out guid))
			{
				return;
			}
			this.selectable.RemoveStatusItem(guid, false);
		}

		// Token: 0x060086C2 RID: 34498 RVA: 0x003093E8 File Offset: 0x003075E8
		public void OnOperationalRequirementChanged(object data)
		{
			Operational operational = data as Operational;
			bool value = (operational == null) ? ((bool)data) : operational.IsActive;
			base.sm.canConvert.Set(value, this, false);
		}

		// Token: 0x04006731 RID: 26417
		private KSelectable selectable;
	}

	// Token: 0x0200150E RID: 5390
	public class States : GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter>
	{
		// Token: 0x060086C3 RID: 34499 RVA: 0x00309428 File Offset: 0x00307628
		private bool ValidateStateTransition(ElementConverter.StatesInstance smi, bool _)
		{
			bool flag = smi.GetCurrentState() == smi.sm.disabled;
			if (smi.master.operational == null)
			{
				return flag;
			}
			bool flag2 = smi.master.consumedElements.Length == 0;
			bool flag3 = this.canConvert.Get(smi);
			int num = 0;
			while (!flag2 && num < smi.master.consumedElements.Length)
			{
				flag2 = smi.master.consumedElements[num].IsActive;
				num++;
			}
			if (flag3 && !flag2)
			{
				this.canConvert.Set(false, smi, true);
				return false;
			}
			return smi.master.operational.MeetsRequirements(smi.master.OperationalRequirement) == flag;
		}

		// Token: 0x060086C4 RID: 34500 RVA: 0x003094E4 File Offset: 0x003076E4
		private void OnEnterRoot(ElementConverter.StatesInstance smi)
		{
			int eventForState = (int)Operational.GetEventForState(smi.master.OperationalRequirement);
			smi.Subscribe(eventForState, new Action<object>(smi.OnOperationalRequirementChanged));
		}

		// Token: 0x060086C5 RID: 34501 RVA: 0x00309518 File Offset: 0x00307718
		private void OnExitRoot(ElementConverter.StatesInstance smi)
		{
			int eventForState = (int)Operational.GetEventForState(smi.master.OperationalRequirement);
			smi.Unsubscribe(eventForState, new Action<object>(smi.OnOperationalRequirementChanged));
		}

		// Token: 0x060086C6 RID: 34502 RVA: 0x0030954C File Offset: 0x0030774C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.Enter(new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback(this.OnEnterRoot)).Exit(new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback(this.OnExitRoot));
			this.disabled.ParamTransition<bool>(this.canConvert, this.converting, new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>.Callback(this.ValidateStateTransition));
			this.converting.Enter("AddStatusItems", delegate(ElementConverter.StatesInstance smi)
			{
				smi.AddStatusItems();
			}).Exit("RemoveStatusItems", delegate(ElementConverter.StatesInstance smi)
			{
				smi.RemoveStatusItems();
			}).ParamTransition<bool>(this.canConvert, this.disabled, new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>.Callback(this.ValidateStateTransition)).Update("ConvertMass", delegate(ElementConverter.StatesInstance smi, float dt)
			{
				smi.master.ConvertMass();
			}, UpdateRate.SIM_1000ms, true);
		}

		// Token: 0x04006732 RID: 26418
		public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State disabled;

		// Token: 0x04006733 RID: 26419
		public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State converting;

		// Token: 0x04006734 RID: 26420
		public StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.BoolParameter canConvert;
	}
}
