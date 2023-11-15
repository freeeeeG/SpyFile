using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Klei;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02000843 RID: 2115
public class ElementLoader
{
	// Token: 0x06003D8A RID: 15754 RVA: 0x00155B74 File Offset: 0x00153D74
	public static List<ElementLoader.ElementEntry> CollectElementsFromYAML()
	{
		List<ElementLoader.ElementEntry> list = new List<ElementLoader.ElementEntry>();
		ListPool<FileHandle, ElementLoader>.PooledList pooledList = ListPool<FileHandle, ElementLoader>.Allocate();
		FileSystem.GetFiles(FileSystem.Normalize(ElementLoader.path), "*.yaml", pooledList);
		ListPool<YamlIO.Error, ElementLoader>.PooledList errors = ListPool<YamlIO.Error, ElementLoader>.Allocate();
		YamlIO.ErrorHandler <>9__0;
		foreach (FileHandle fileHandle in pooledList)
		{
			string full_path = fileHandle.full_path;
			YamlIO.ErrorHandler handle_error;
			if ((handle_error = <>9__0) == null)
			{
				handle_error = (<>9__0 = delegate(YamlIO.Error error, bool force_log_as_warning)
				{
					errors.Add(error);
				});
			}
			ElementLoader.ElementEntryCollection elementEntryCollection = YamlIO.LoadFile<ElementLoader.ElementEntryCollection>(full_path, handle_error, null);
			if (elementEntryCollection != null)
			{
				list.AddRange(elementEntryCollection.elements);
			}
		}
		pooledList.Recycle();
		if (Global.Instance != null && Global.Instance.modManager != null)
		{
			Global.Instance.modManager.HandleErrors(errors);
		}
		errors.Recycle();
		return list;
	}

	// Token: 0x06003D8B RID: 15755 RVA: 0x00155C6C File Offset: 0x00153E6C
	public static void Load(ref Hashtable substanceList, Dictionary<string, SubstanceTable> substanceTablesByDlc)
	{
		ElementLoader.elements = new List<Element>();
		ElementLoader.elementTable = new Dictionary<int, Element>();
		ElementLoader.elementTagTable = new Dictionary<Tag, Element>();
		foreach (ElementLoader.ElementEntry elementEntry in ElementLoader.CollectElementsFromYAML())
		{
			int num = Hash.SDBMLower(elementEntry.elementId);
			if (!ElementLoader.elementTable.ContainsKey(num) && substanceTablesByDlc.ContainsKey(elementEntry.dlcId))
			{
				Element element = new Element();
				element.id = (SimHashes)num;
				element.name = Strings.Get(elementEntry.localizationID);
				element.nameUpperCase = element.name.ToUpper();
				element.description = Strings.Get(elementEntry.description);
				element.tag = TagManager.Create(elementEntry.elementId, element.name);
				ElementLoader.CopyEntryToElement(elementEntry, element);
				ElementLoader.elements.Add(element);
				ElementLoader.elementTable[num] = element;
				ElementLoader.elementTagTable[element.tag] = element;
				if (!ElementLoader.ManifestSubstanceForElement(element, ref substanceList, substanceTablesByDlc[elementEntry.dlcId]))
				{
					global::Debug.LogWarning("Missing substance for element: " + element.id.ToString());
				}
			}
		}
		ElementLoader.FinaliseElementsTable(ref substanceList);
		WorldGen.SetupDefaultElements();
	}

	// Token: 0x06003D8C RID: 15756 RVA: 0x00155DE4 File Offset: 0x00153FE4
	private static void CopyEntryToElement(ElementLoader.ElementEntry entry, Element elem)
	{
		Hash.SDBMLower(entry.elementId);
		elem.tag = TagManager.Create(entry.elementId.ToString());
		elem.specificHeatCapacity = entry.specificHeatCapacity;
		elem.thermalConductivity = entry.thermalConductivity;
		elem.molarMass = entry.molarMass;
		elem.strength = entry.strength;
		elem.disabled = entry.isDisabled;
		elem.dlcId = entry.dlcId;
		elem.flow = entry.flow;
		elem.maxMass = entry.maxMass;
		elem.maxCompression = entry.liquidCompression;
		elem.viscosity = entry.speed;
		elem.minHorizontalFlow = entry.minHorizontalFlow;
		elem.minVerticalFlow = entry.minVerticalFlow;
		elem.solidSurfaceAreaMultiplier = entry.solidSurfaceAreaMultiplier;
		elem.liquidSurfaceAreaMultiplier = entry.liquidSurfaceAreaMultiplier;
		elem.gasSurfaceAreaMultiplier = entry.gasSurfaceAreaMultiplier;
		elem.state = entry.state;
		elem.hardness = entry.hardness;
		elem.lowTemp = entry.lowTemp;
		elem.lowTempTransitionTarget = (SimHashes)Hash.SDBMLower(entry.lowTempTransitionTarget);
		elem.highTemp = entry.highTemp;
		elem.highTempTransitionTarget = (SimHashes)Hash.SDBMLower(entry.highTempTransitionTarget);
		elem.highTempTransitionOreID = (SimHashes)Hash.SDBMLower(entry.highTempTransitionOreId);
		elem.highTempTransitionOreMassConversion = entry.highTempTransitionOreMassConversion;
		elem.lowTempTransitionOreID = (SimHashes)Hash.SDBMLower(entry.lowTempTransitionOreId);
		elem.lowTempTransitionOreMassConversion = entry.lowTempTransitionOreMassConversion;
		elem.sublimateId = (SimHashes)Hash.SDBMLower(entry.sublimateId);
		elem.convertId = (SimHashes)Hash.SDBMLower(entry.convertId);
		elem.sublimateFX = (SpawnFXHashes)Hash.SDBMLower(entry.sublimateFx);
		elem.sublimateRate = entry.sublimateRate;
		elem.sublimateEfficiency = entry.sublimateEfficiency;
		elem.sublimateProbability = entry.sublimateProbability;
		elem.offGasPercentage = entry.offGasPercentage;
		elem.lightAbsorptionFactor = entry.lightAbsorptionFactor;
		elem.radiationAbsorptionFactor = entry.radiationAbsorptionFactor;
		elem.radiationPer1000Mass = entry.radiationPer1000Mass;
		elem.toxicity = entry.toxicity;
		elem.elementComposition = entry.composition;
		Tag phaseTag = TagManager.Create(entry.state.ToString());
		elem.materialCategory = ElementLoader.CreateMaterialCategoryTag(elem.id, phaseTag, entry.materialCategory);
		elem.oreTags = ElementLoader.CreateOreTags(elem.materialCategory, phaseTag, entry.tags);
		elem.buildMenuSort = entry.buildMenuSort;
		Sim.PhysicsData defaultValues = default(Sim.PhysicsData);
		defaultValues.temperature = entry.defaultTemperature;
		defaultValues.mass = entry.defaultMass;
		defaultValues.pressure = entry.defaultPressure;
		switch (entry.state)
		{
		case Element.State.Gas:
			GameTags.GasElements.Add(elem.tag);
			defaultValues.mass = 1f;
			elem.maxMass = 1.8f;
			break;
		case Element.State.Liquid:
			GameTags.LiquidElements.Add(elem.tag);
			break;
		case Element.State.Solid:
			GameTags.SolidElements.Add(elem.tag);
			break;
		}
		elem.defaultValues = defaultValues;
	}

	// Token: 0x06003D8D RID: 15757 RVA: 0x001560E8 File Offset: 0x001542E8
	private static bool ManifestSubstanceForElement(Element elem, ref Hashtable substanceList, SubstanceTable substanceTable)
	{
		elem.substance = null;
		if (substanceList.ContainsKey(elem.id))
		{
			elem.substance = (substanceList[elem.id] as Substance);
			return false;
		}
		if (substanceTable != null)
		{
			elem.substance = substanceTable.GetSubstance(elem.id);
		}
		if (elem.substance == null)
		{
			elem.substance = new Substance();
			substanceTable.GetList().Add(elem.substance);
		}
		elem.substance.elementID = elem.id;
		elem.substance.renderedByWorld = elem.IsSolid;
		elem.substance.idx = substanceList.Count;
		if (elem.substance.uiColour == ElementLoader.noColour)
		{
			int count = ElementLoader.elements.Count;
			int idx = elem.substance.idx;
			elem.substance.uiColour = Color.HSVToRGB((float)idx / (float)count, 1f, 1f);
		}
		string name = UI.StripLinkFormatting(elem.name);
		elem.substance.name = name;
		elem.substance.nameTag = elem.tag;
		elem.substance.audioConfig = ElementsAudio.Instance.GetConfigForElement(elem.id);
		substanceList.Add(elem.id, elem.substance);
		return true;
	}

	// Token: 0x06003D8E RID: 15758 RVA: 0x00156258 File Offset: 0x00154458
	public static Element FindElementByName(string name)
	{
		Element result;
		try
		{
			result = ElementLoader.FindElementByHash((SimHashes)Enum.Parse(typeof(SimHashes), name));
		}
		catch
		{
			result = ElementLoader.FindElementByHash((SimHashes)Hash.SDBMLower(name));
		}
		return result;
	}

	// Token: 0x06003D8F RID: 15759 RVA: 0x001562A4 File Offset: 0x001544A4
	public static Element FindElementByHash(SimHashes hash)
	{
		Element result = null;
		ElementLoader.elementTable.TryGetValue((int)hash, out result);
		return result;
	}

	// Token: 0x06003D90 RID: 15760 RVA: 0x001562C4 File Offset: 0x001544C4
	public static ushort GetElementIndex(SimHashes hash)
	{
		Element element = null;
		ElementLoader.elementTable.TryGetValue((int)hash, out element);
		if (element != null)
		{
			return element.idx;
		}
		return ushort.MaxValue;
	}

	// Token: 0x06003D91 RID: 15761 RVA: 0x001562F0 File Offset: 0x001544F0
	public static Element GetElement(Tag tag)
	{
		Element result;
		ElementLoader.elementTagTable.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x0015630C File Offset: 0x0015450C
	public static SimHashes GetElementID(Tag tag)
	{
		Element element;
		ElementLoader.elementTagTable.TryGetValue(tag, out element);
		if (element != null)
		{
			return element.id;
		}
		return SimHashes.Vacuum;
	}

	// Token: 0x06003D93 RID: 15763 RVA: 0x00156338 File Offset: 0x00154538
	private static SimHashes GetID(int column, int row, string[,] grid, SimHashes defaultValue = SimHashes.Vacuum)
	{
		if (column >= grid.GetLength(0) || row > grid.GetLength(1))
		{
			global::Debug.LogError(string.Format("Could not find element at loc [{0},{1}] grid is only [{2},{3}]", new object[]
			{
				column,
				row,
				grid.GetLength(0),
				grid.GetLength(1)
			}));
			return defaultValue;
		}
		string text = grid[column, row];
		if (text == null || text == "")
		{
			return defaultValue;
		}
		object obj = null;
		try
		{
			obj = Enum.Parse(typeof(SimHashes), text);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Could not find element {0}: {1}", text, ex.ToString()));
			return defaultValue;
		}
		return (SimHashes)obj;
	}

	// Token: 0x06003D94 RID: 15764 RVA: 0x00156404 File Offset: 0x00154604
	private static SpawnFXHashes GetSpawnFX(int column, int row, string[,] grid)
	{
		if (column >= grid.GetLength(0) || row > grid.GetLength(1))
		{
			global::Debug.LogError(string.Format("Could not find SpawnFXHashes at loc [{0},{1}] grid is only [{2},{3}]", new object[]
			{
				column,
				row,
				grid.GetLength(0),
				grid.GetLength(1)
			}));
			return SpawnFXHashes.None;
		}
		string text = grid[column, row];
		if (text == null || text == "")
		{
			return SpawnFXHashes.None;
		}
		object obj = null;
		try
		{
			obj = Enum.Parse(typeof(SpawnFXHashes), text);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Could not find FX {0}: {1}", text, ex.ToString()));
			return SpawnFXHashes.None;
		}
		return (SpawnFXHashes)obj;
	}

	// Token: 0x06003D95 RID: 15765 RVA: 0x001564D0 File Offset: 0x001546D0
	private static Tag CreateMaterialCategoryTag(SimHashes element_id, Tag phaseTag, string materialCategoryField)
	{
		if (!string.IsNullOrEmpty(materialCategoryField))
		{
			Tag tag = TagManager.Create(materialCategoryField);
			if (!GameTags.MaterialCategories.Contains(tag) && !GameTags.IgnoredMaterialCategories.Contains(tag))
			{
				global::Debug.LogWarningFormat("Element {0} has category {1}, but that isn't in GameTags.MaterialCategores!", new object[]
				{
					element_id,
					materialCategoryField
				});
			}
			return tag;
		}
		return phaseTag;
	}

	// Token: 0x06003D96 RID: 15766 RVA: 0x00156528 File Offset: 0x00154728
	private static Tag[] CreateOreTags(Tag materialCategory, Tag phaseTag, string[] ore_tags_split)
	{
		List<Tag> list = new List<Tag>();
		if (ore_tags_split != null)
		{
			foreach (string text in ore_tags_split)
			{
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(TagManager.Create(text));
				}
			}
		}
		list.Add(phaseTag);
		if (materialCategory.IsValid && !list.Contains(materialCategory))
		{
			list.Add(materialCategory);
		}
		return list.ToArray();
	}

	// Token: 0x06003D97 RID: 15767 RVA: 0x0015658C File Offset: 0x0015478C
	private static void FinaliseElementsTable(ref Hashtable substanceList)
	{
		foreach (Element element in ElementLoader.elements)
		{
			if (element != null)
			{
				if (element.substance == null)
				{
					global::Debug.LogWarning("Skipping finalise for missing element: " + element.id.ToString());
				}
				else
				{
					global::Debug.Assert(element.substance.nameTag.IsValid);
					if (element.thermalConductivity == 0f)
					{
						element.state |= Element.State.TemperatureInsulated;
					}
					if (element.strength == 0f)
					{
						element.state |= Element.State.Unbreakable;
					}
					if (element.IsSolid)
					{
						Element element2 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
						if (element2 != null)
						{
							element.highTempTransition = element2;
						}
					}
					else if (element.IsLiquid)
					{
						Element element3 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
						if (element3 != null)
						{
							element.highTempTransition = element3;
						}
						Element element4 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
						if (element4 != null)
						{
							element.lowTempTransition = element4;
						}
					}
					else if (element.IsGas)
					{
						Element element5 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
						if (element5 != null)
						{
							element.lowTempTransition = element5;
						}
					}
				}
			}
		}
		ElementLoader.elements = (from e in ElementLoader.elements
		orderby (int)(e.state & Element.State.Solid) descending, e.id
		select e).ToList<Element>();
		for (int i = 0; i < ElementLoader.elements.Count; i++)
		{
			if (ElementLoader.elements[i].substance != null)
			{
				ElementLoader.elements[i].substance.idx = i;
			}
			ElementLoader.elements[i].idx = (ushort)i;
		}
	}

	// Token: 0x06003D98 RID: 15768 RVA: 0x00156794 File Offset: 0x00154994
	private static void ValidateElements()
	{
		global::Debug.Log("------ Start Validating Elements ------");
		foreach (Element element in ElementLoader.elements)
		{
			string text = string.Format("{0} ({1})", element.tag.ProperNameStripLink(), element.state);
			if (element.IsLiquid && element.sublimateId != (SimHashes)0)
			{
				global::Debug.Assert(element.sublimateRate == 0f, text + ": Liquids don't use sublimateRate, use offGasPercentage instead.");
				global::Debug.Assert(element.offGasPercentage > 0f, text + ": Missing offGasPercentage");
			}
			if (element.IsSolid && element.sublimateId != (SimHashes)0)
			{
				global::Debug.Assert(element.offGasPercentage == 0f, text + ": Solids don't use offGasPercentage, use sublimateRate instead.");
				global::Debug.Assert(element.sublimateRate > 0f, text + ": Missing sublimationRate");
				global::Debug.Assert(element.sublimateRate * element.sublimateEfficiency > 0.001f, text + ": Sublimation rate and efficiency will result in gas that will be obliterated because its less than 1g. Increase these values and use sublimateProbability if you want a low amount of sublimation");
			}
			if (element.highTempTransition != null && element.highTempTransition.lowTempTransition == element)
			{
				global::Debug.Assert(element.highTemp >= element.highTempTransition.lowTemp, text + ": highTemp is higher than transition element's (" + element.highTempTransition.tag.ProperNameStripLink() + ") lowTemp");
			}
			global::Debug.Assert(element.defaultValues.mass <= element.maxMass, text + ": Default mass should be less than max mass");
			if (false)
			{
				if (element.IsSolid && element.highTempTransition != null && element.highTempTransition.IsLiquid && element.defaultValues.mass > element.highTempTransition.maxMass)
				{
					global::Debug.LogWarning(string.Format("{0} defaultMass {1} > {2}: maxMass {3}", new object[]
					{
						text,
						element.defaultValues.mass,
						element.highTempTransition.tag.ProperNameStripLink(),
						element.highTempTransition.maxMass
					}));
				}
				if (element.defaultValues.mass < element.maxMass && element.IsLiquid)
				{
					global::Debug.LogWarning(string.Format("{0} has defaultMass: {1} and maxMass {2}", element.tag.ProperNameStripLink(), element.defaultValues.mass, element.maxMass));
				}
			}
		}
		global::Debug.Log("------ End Validating Elements ------");
	}

	// Token: 0x0400281E RID: 10270
	public static List<Element> elements;

	// Token: 0x0400281F RID: 10271
	public static Dictionary<int, Element> elementTable;

	// Token: 0x04002820 RID: 10272
	public static Dictionary<Tag, Element> elementTagTable;

	// Token: 0x04002821 RID: 10273
	private static string path = Application.streamingAssetsPath + "/elements/";

	// Token: 0x04002822 RID: 10274
	private static readonly Color noColour = new Color(0f, 0f, 0f, 0f);

	// Token: 0x02001614 RID: 5652
	public class ElementEntryCollection
	{
		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x0600894D RID: 35149 RVA: 0x003119E8 File Offset: 0x0030FBE8
		// (set) Token: 0x0600894E RID: 35150 RVA: 0x003119F0 File Offset: 0x0030FBF0
		public ElementLoader.ElementEntry[] elements { get; set; }
	}

	// Token: 0x02001615 RID: 5653
	public class ElementComposition
	{
		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06008951 RID: 35153 RVA: 0x00311A09 File Offset: 0x0030FC09
		// (set) Token: 0x06008952 RID: 35154 RVA: 0x00311A11 File Offset: 0x0030FC11
		public string elementID { get; set; }

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06008953 RID: 35155 RVA: 0x00311A1A File Offset: 0x0030FC1A
		// (set) Token: 0x06008954 RID: 35156 RVA: 0x00311A22 File Offset: 0x0030FC22
		public float percentage { get; set; }
	}

	// Token: 0x02001616 RID: 5654
	public class ElementEntry
	{
		// Token: 0x06008955 RID: 35157 RVA: 0x00311A2B File Offset: 0x0030FC2B
		public ElementEntry()
		{
			this.lowTemp = 0f;
			this.highTemp = 10000f;
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06008956 RID: 35158 RVA: 0x00311A49 File Offset: 0x0030FC49
		// (set) Token: 0x06008957 RID: 35159 RVA: 0x00311A51 File Offset: 0x0030FC51
		public string elementId { get; set; }

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06008958 RID: 35160 RVA: 0x00311A5A File Offset: 0x0030FC5A
		// (set) Token: 0x06008959 RID: 35161 RVA: 0x00311A62 File Offset: 0x0030FC62
		public float specificHeatCapacity { get; set; }

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x0600895A RID: 35162 RVA: 0x00311A6B File Offset: 0x0030FC6B
		// (set) Token: 0x0600895B RID: 35163 RVA: 0x00311A73 File Offset: 0x0030FC73
		public float thermalConductivity { get; set; }

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x0600895C RID: 35164 RVA: 0x00311A7C File Offset: 0x0030FC7C
		// (set) Token: 0x0600895D RID: 35165 RVA: 0x00311A84 File Offset: 0x0030FC84
		public float solidSurfaceAreaMultiplier { get; set; }

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x0600895E RID: 35166 RVA: 0x00311A8D File Offset: 0x0030FC8D
		// (set) Token: 0x0600895F RID: 35167 RVA: 0x00311A95 File Offset: 0x0030FC95
		public float liquidSurfaceAreaMultiplier { get; set; }

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06008960 RID: 35168 RVA: 0x00311A9E File Offset: 0x0030FC9E
		// (set) Token: 0x06008961 RID: 35169 RVA: 0x00311AA6 File Offset: 0x0030FCA6
		public float gasSurfaceAreaMultiplier { get; set; }

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06008962 RID: 35170 RVA: 0x00311AAF File Offset: 0x0030FCAF
		// (set) Token: 0x06008963 RID: 35171 RVA: 0x00311AB7 File Offset: 0x0030FCB7
		public float defaultMass { get; set; }

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06008964 RID: 35172 RVA: 0x00311AC0 File Offset: 0x0030FCC0
		// (set) Token: 0x06008965 RID: 35173 RVA: 0x00311AC8 File Offset: 0x0030FCC8
		public float defaultTemperature { get; set; }

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06008966 RID: 35174 RVA: 0x00311AD1 File Offset: 0x0030FCD1
		// (set) Token: 0x06008967 RID: 35175 RVA: 0x00311AD9 File Offset: 0x0030FCD9
		public float defaultPressure { get; set; }

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06008968 RID: 35176 RVA: 0x00311AE2 File Offset: 0x0030FCE2
		// (set) Token: 0x06008969 RID: 35177 RVA: 0x00311AEA File Offset: 0x0030FCEA
		public float molarMass { get; set; }

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x0600896A RID: 35178 RVA: 0x00311AF3 File Offset: 0x0030FCF3
		// (set) Token: 0x0600896B RID: 35179 RVA: 0x00311AFB File Offset: 0x0030FCFB
		public float lightAbsorptionFactor { get; set; }

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x0600896C RID: 35180 RVA: 0x00311B04 File Offset: 0x0030FD04
		// (set) Token: 0x0600896D RID: 35181 RVA: 0x00311B0C File Offset: 0x0030FD0C
		public float radiationAbsorptionFactor { get; set; }

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x0600896E RID: 35182 RVA: 0x00311B15 File Offset: 0x0030FD15
		// (set) Token: 0x0600896F RID: 35183 RVA: 0x00311B1D File Offset: 0x0030FD1D
		public float radiationPer1000Mass { get; set; }

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06008970 RID: 35184 RVA: 0x00311B26 File Offset: 0x0030FD26
		// (set) Token: 0x06008971 RID: 35185 RVA: 0x00311B2E File Offset: 0x0030FD2E
		public string lowTempTransitionTarget { get; set; }

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06008972 RID: 35186 RVA: 0x00311B37 File Offset: 0x0030FD37
		// (set) Token: 0x06008973 RID: 35187 RVA: 0x00311B3F File Offset: 0x0030FD3F
		public float lowTemp { get; set; }

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06008974 RID: 35188 RVA: 0x00311B48 File Offset: 0x0030FD48
		// (set) Token: 0x06008975 RID: 35189 RVA: 0x00311B50 File Offset: 0x0030FD50
		public string highTempTransitionTarget { get; set; }

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06008976 RID: 35190 RVA: 0x00311B59 File Offset: 0x0030FD59
		// (set) Token: 0x06008977 RID: 35191 RVA: 0x00311B61 File Offset: 0x0030FD61
		public float highTemp { get; set; }

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06008978 RID: 35192 RVA: 0x00311B6A File Offset: 0x0030FD6A
		// (set) Token: 0x06008979 RID: 35193 RVA: 0x00311B72 File Offset: 0x0030FD72
		public string lowTempTransitionOreId { get; set; }

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x0600897A RID: 35194 RVA: 0x00311B7B File Offset: 0x0030FD7B
		// (set) Token: 0x0600897B RID: 35195 RVA: 0x00311B83 File Offset: 0x0030FD83
		public float lowTempTransitionOreMassConversion { get; set; }

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x0600897C RID: 35196 RVA: 0x00311B8C File Offset: 0x0030FD8C
		// (set) Token: 0x0600897D RID: 35197 RVA: 0x00311B94 File Offset: 0x0030FD94
		public string highTempTransitionOreId { get; set; }

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x0600897E RID: 35198 RVA: 0x00311B9D File Offset: 0x0030FD9D
		// (set) Token: 0x0600897F RID: 35199 RVA: 0x00311BA5 File Offset: 0x0030FDA5
		public float highTempTransitionOreMassConversion { get; set; }

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06008980 RID: 35200 RVA: 0x00311BAE File Offset: 0x0030FDAE
		// (set) Token: 0x06008981 RID: 35201 RVA: 0x00311BB6 File Offset: 0x0030FDB6
		public string sublimateId { get; set; }

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06008982 RID: 35202 RVA: 0x00311BBF File Offset: 0x0030FDBF
		// (set) Token: 0x06008983 RID: 35203 RVA: 0x00311BC7 File Offset: 0x0030FDC7
		public string sublimateFx { get; set; }

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06008984 RID: 35204 RVA: 0x00311BD0 File Offset: 0x0030FDD0
		// (set) Token: 0x06008985 RID: 35205 RVA: 0x00311BD8 File Offset: 0x0030FDD8
		public float sublimateRate { get; set; }

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06008986 RID: 35206 RVA: 0x00311BE1 File Offset: 0x0030FDE1
		// (set) Token: 0x06008987 RID: 35207 RVA: 0x00311BE9 File Offset: 0x0030FDE9
		public float sublimateEfficiency { get; set; }

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06008988 RID: 35208 RVA: 0x00311BF2 File Offset: 0x0030FDF2
		// (set) Token: 0x06008989 RID: 35209 RVA: 0x00311BFA File Offset: 0x0030FDFA
		public float sublimateProbability { get; set; }

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x0600898A RID: 35210 RVA: 0x00311C03 File Offset: 0x0030FE03
		// (set) Token: 0x0600898B RID: 35211 RVA: 0x00311C0B File Offset: 0x0030FE0B
		public float offGasPercentage { get; set; }

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x0600898C RID: 35212 RVA: 0x00311C14 File Offset: 0x0030FE14
		// (set) Token: 0x0600898D RID: 35213 RVA: 0x00311C1C File Offset: 0x0030FE1C
		public string materialCategory { get; set; }

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x0600898E RID: 35214 RVA: 0x00311C25 File Offset: 0x0030FE25
		// (set) Token: 0x0600898F RID: 35215 RVA: 0x00311C2D File Offset: 0x0030FE2D
		public string[] tags { get; set; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06008990 RID: 35216 RVA: 0x00311C36 File Offset: 0x0030FE36
		// (set) Token: 0x06008991 RID: 35217 RVA: 0x00311C3E File Offset: 0x0030FE3E
		public bool isDisabled { get; set; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06008992 RID: 35218 RVA: 0x00311C47 File Offset: 0x0030FE47
		// (set) Token: 0x06008993 RID: 35219 RVA: 0x00311C4F File Offset: 0x0030FE4F
		public float strength { get; set; }

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06008994 RID: 35220 RVA: 0x00311C58 File Offset: 0x0030FE58
		// (set) Token: 0x06008995 RID: 35221 RVA: 0x00311C60 File Offset: 0x0030FE60
		public float maxMass { get; set; }

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06008996 RID: 35222 RVA: 0x00311C69 File Offset: 0x0030FE69
		// (set) Token: 0x06008997 RID: 35223 RVA: 0x00311C71 File Offset: 0x0030FE71
		public byte hardness { get; set; }

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06008998 RID: 35224 RVA: 0x00311C7A File Offset: 0x0030FE7A
		// (set) Token: 0x06008999 RID: 35225 RVA: 0x00311C82 File Offset: 0x0030FE82
		public float toxicity { get; set; }

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x0600899A RID: 35226 RVA: 0x00311C8B File Offset: 0x0030FE8B
		// (set) Token: 0x0600899B RID: 35227 RVA: 0x00311C93 File Offset: 0x0030FE93
		public float liquidCompression { get; set; }

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x0600899C RID: 35228 RVA: 0x00311C9C File Offset: 0x0030FE9C
		// (set) Token: 0x0600899D RID: 35229 RVA: 0x00311CA4 File Offset: 0x0030FEA4
		public float speed { get; set; }

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x0600899E RID: 35230 RVA: 0x00311CAD File Offset: 0x0030FEAD
		// (set) Token: 0x0600899F RID: 35231 RVA: 0x00311CB5 File Offset: 0x0030FEB5
		public float minHorizontalFlow { get; set; }

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060089A0 RID: 35232 RVA: 0x00311CBE File Offset: 0x0030FEBE
		// (set) Token: 0x060089A1 RID: 35233 RVA: 0x00311CC6 File Offset: 0x0030FEC6
		public float minVerticalFlow { get; set; }

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060089A2 RID: 35234 RVA: 0x00311CCF File Offset: 0x0030FECF
		// (set) Token: 0x060089A3 RID: 35235 RVA: 0x00311CD7 File Offset: 0x0030FED7
		public string convertId { get; set; }

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060089A4 RID: 35236 RVA: 0x00311CE0 File Offset: 0x0030FEE0
		// (set) Token: 0x060089A5 RID: 35237 RVA: 0x00311CE8 File Offset: 0x0030FEE8
		public float flow { get; set; }

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060089A6 RID: 35238 RVA: 0x00311CF1 File Offset: 0x0030FEF1
		// (set) Token: 0x060089A7 RID: 35239 RVA: 0x00311CF9 File Offset: 0x0030FEF9
		public int buildMenuSort { get; set; }

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x060089A8 RID: 35240 RVA: 0x00311D02 File Offset: 0x0030FF02
		// (set) Token: 0x060089A9 RID: 35241 RVA: 0x00311D0A File Offset: 0x0030FF0A
		public Element.State state { get; set; }

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060089AA RID: 35242 RVA: 0x00311D13 File Offset: 0x0030FF13
		// (set) Token: 0x060089AB RID: 35243 RVA: 0x00311D1B File Offset: 0x0030FF1B
		public string localizationID { get; set; }

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060089AC RID: 35244 RVA: 0x00311D24 File Offset: 0x0030FF24
		// (set) Token: 0x060089AD RID: 35245 RVA: 0x00311D2C File Offset: 0x0030FF2C
		public string dlcId { get; set; }

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x060089AE RID: 35246 RVA: 0x00311D35 File Offset: 0x0030FF35
		// (set) Token: 0x060089AF RID: 35247 RVA: 0x00311D3D File Offset: 0x0030FF3D
		public ElementLoader.ElementComposition[] composition { get; set; }

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x060089B0 RID: 35248 RVA: 0x00311D46 File Offset: 0x0030FF46
		// (set) Token: 0x060089B1 RID: 35249 RVA: 0x00311D71 File Offset: 0x0030FF71
		public string description
		{
			get
			{
				return this.description_backing ?? ("STRINGS.ELEMENTS." + this.elementId.ToString().ToUpper() + ".DESC");
			}
			set
			{
				this.description_backing = value;
			}
		}

		// Token: 0x04006A9F RID: 27295
		private string description_backing;
	}
}
