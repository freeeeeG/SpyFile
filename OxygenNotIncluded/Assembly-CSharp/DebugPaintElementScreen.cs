using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AE8 RID: 2792
public class DebugPaintElementScreen : KScreen
{
	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x060055FB RID: 22011 RVA: 0x001F4AF9 File Offset: 0x001F2CF9
	// (set) Token: 0x060055FC RID: 22012 RVA: 0x001F4B00 File Offset: 0x001F2D00
	public static DebugPaintElementScreen Instance { get; private set; }

	// Token: 0x060055FD RID: 22013 RVA: 0x001F4B08 File Offset: 0x001F2D08
	public static void DestroyInstance()
	{
		DebugPaintElementScreen.Instance = null;
	}

	// Token: 0x060055FE RID: 22014 RVA: 0x001F4B10 File Offset: 0x001F2D10
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DebugPaintElementScreen.Instance = this;
		this.SetupLocText();
		this.inputFields.Add(this.massPressureInput);
		this.inputFields.Add(this.temperatureInput);
		this.inputFields.Add(this.diseaseCountInput);
		this.inputFields.Add(this.filterInput);
		foreach (KInputTextField kinputTextField in this.inputFields)
		{
			kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
			{
				base.isEditing = true;
			}));
			kinputTextField.onEndEdit.AddListener(delegate(string value)
			{
				base.isEditing = false;
			});
		}
		base.gameObject.SetActive(false);
		this.activateOnSpawn = true;
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060055FF RID: 22015 RVA: 0x001F4C04 File Offset: 0x001F2E04
	private void SetupLocText()
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("Title").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TITLE;
		component.GetReference<LocText>("ElementLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
		component.GetReference<LocText>("MassLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.MASS_KG;
		component.GetReference<LocText>("TemperatureLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TEMPERATURE_KELVIN;
		component.GetReference<LocText>("DiseaseLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
		component.GetReference<LocText>("DiseaseCountLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE_COUNT;
		component.GetReference<LocText>("AddFoWMaskLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ADD_FOW_MASK;
		component.GetReference<LocText>("RemoveFoWMaskLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.REMOVE_FOW_MASK;
		this.elementButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
		this.diseaseButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
		this.paintButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.PAINT;
		this.fillButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.FILL;
		this.spawnButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SPAWN_ALL;
		this.sampleButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SAMPLE;
		this.storeButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.STORE;
		this.affectBuildings.transform.parent.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.BUILDINGS;
		this.affectCells.transform.parent.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.CELLS;
	}

	// Token: 0x06005600 RID: 22016 RVA: 0x001F4DF8 File Offset: 0x001F2FF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.element = SimHashes.Ice;
		this.diseaseIdx = byte.MaxValue;
		this.ConfigureElements();
		List<string> list = new List<string>();
		list.Insert(0, "None");
		foreach (Disease disease in Db.Get().Diseases.resources)
		{
			list.Add(disease.Name);
		}
		this.diseasePopup.SetOptions(list.ToArray());
		KPopupMenu kpopupMenu = this.diseasePopup;
		kpopupMenu.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(this.OnSelectDisease));
		this.SelectDiseaseOption((int)this.diseaseIdx);
		this.paintButton.onClick += this.OnClickPaint;
		this.fillButton.onClick += this.OnClickFill;
		this.sampleButton.onClick += this.OnClickSample;
		this.storeButton.onClick += this.OnClickStore;
		if (SaveGame.Instance.worldGenSpawner.SpawnsRemain())
		{
			this.spawnButton.onClick += this.OnClickSpawn;
		}
		KPopupMenu kpopupMenu2 = this.elementPopup;
		kpopupMenu2.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu2.OnSelect, new Action<string, int>(this.OnSelectElement));
		this.elementButton.onClick += this.elementPopup.OnClick;
		this.diseaseButton.onClick += this.diseasePopup.OnClick;
	}

	// Token: 0x06005601 RID: 22017 RVA: 0x001F4FB4 File Offset: 0x001F31B4
	private void FilterElements(string filterValue)
	{
		if (string.IsNullOrEmpty(filterValue))
		{
			foreach (KButtonMenu.ButtonInfo buttonInfo in this.elementPopup.GetButtons())
			{
				buttonInfo.uibutton.gameObject.SetActive(true);
			}
			return;
		}
		filterValue = this.filter.ToLower();
		foreach (KButtonMenu.ButtonInfo buttonInfo2 in this.elementPopup.GetButtons())
		{
			buttonInfo2.uibutton.gameObject.SetActive(buttonInfo2.text.ToLower().Contains(filterValue));
		}
	}

	// Token: 0x06005602 RID: 22018 RVA: 0x001F5080 File Offset: 0x001F3280
	private void ConfigureElements()
	{
		if (this.filter != null)
		{
			this.filter = this.filter.ToLower();
		}
		List<DebugPaintElementScreen.ElemDisplayInfo> list = new List<DebugPaintElementScreen.ElemDisplayInfo>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.name != "Element Not Loaded" && element.substance != null && element.substance.showInEditor && (string.IsNullOrEmpty(this.filter) || element.name.ToLower().Contains(this.filter)))
			{
				list.Add(new DebugPaintElementScreen.ElemDisplayInfo
				{
					id = element.id,
					displayStr = element.name + " (" + element.GetStateString() + ")"
				});
			}
		}
		list.Sort((DebugPaintElementScreen.ElemDisplayInfo a, DebugPaintElementScreen.ElemDisplayInfo b) => a.displayStr.CompareTo(b.displayStr));
		if (string.IsNullOrEmpty(this.filter))
		{
			SimHashes[] array = new SimHashes[]
			{
				SimHashes.SlimeMold,
				SimHashes.Vacuum,
				SimHashes.Dirt,
				SimHashes.CarbonDioxide,
				SimHashes.Water,
				SimHashes.Oxygen
			};
			for (int i = 0; i < array.Length; i++)
			{
				Element element2 = ElementLoader.FindElementByHash(array[i]);
				list.Insert(0, new DebugPaintElementScreen.ElemDisplayInfo
				{
					id = element2.id,
					displayStr = element2.name + " (" + element2.GetStateString() + ")"
				});
			}
		}
		this.options_list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (DebugPaintElementScreen.ElemDisplayInfo elemDisplayInfo in list)
		{
			list2.Add(elemDisplayInfo.displayStr);
			this.options_list.Add(elemDisplayInfo.id.ToString());
		}
		this.elementPopup.SetOptions(list2);
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].id == this.element)
			{
				this.elementPopup.SelectOption(list2[j], j);
			}
		}
		this.elementPopup.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0f, 1f);
	}

	// Token: 0x06005603 RID: 22019 RVA: 0x001F5300 File Offset: 0x001F3500
	private void OnClickSpawn()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.SetDiscovered(true);
		}
		SaveGame.Instance.worldGenSpawner.SpawnEverything();
		this.spawnButton.GetComponent<KButton>().isInteractable = false;
	}

	// Token: 0x06005604 RID: 22020 RVA: 0x001F5370 File Offset: 0x001F3570
	private void OnClickPaint()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.ReplaceSubstance);
	}

	// Token: 0x06005605 RID: 22021 RVA: 0x001F5395 File Offset: 0x001F3595
	private void OnClickStore()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.StoreSubstance);
	}

	// Token: 0x06005606 RID: 22022 RVA: 0x001F53BA File Offset: 0x001F35BA
	private void OnClickSample()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.Sample);
	}

	// Token: 0x06005607 RID: 22023 RVA: 0x001F53DF File Offset: 0x001F35DF
	private void OnClickFill()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		DebugTool.Instance.Activate(DebugTool.Type.FillReplaceSubstance);
	}

	// Token: 0x06005608 RID: 22024 RVA: 0x001F53FE File Offset: 0x001F35FE
	private void OnSelectElement(string str, int index)
	{
		this.element = (SimHashes)Enum.Parse(typeof(SimHashes), this.options_list[index]);
		this.elementButton.GetComponentInChildren<LocText>().text = str;
	}

	// Token: 0x06005609 RID: 22025 RVA: 0x001F5437 File Offset: 0x001F3637
	private void OnSelectElement(SimHashes element)
	{
		this.element = element;
		this.elementButton.GetComponentInChildren<LocText>().text = ElementLoader.FindElementByHash(element).name;
	}

	// Token: 0x0600560A RID: 22026 RVA: 0x001F545C File Offset: 0x001F365C
	private void OnSelectDisease(string str, int index)
	{
		this.diseaseIdx = byte.MaxValue;
		for (int i = 0; i < Db.Get().Diseases.Count; i++)
		{
			if (Db.Get().Diseases[i].Name == str)
			{
				this.diseaseIdx = (byte)i;
			}
		}
		this.SelectDiseaseOption((int)this.diseaseIdx);
	}

	// Token: 0x0600560B RID: 22027 RVA: 0x001F54C0 File Offset: 0x001F36C0
	private void SelectDiseaseOption(int diseaseIdx)
	{
		if (diseaseIdx == 255)
		{
			this.diseaseButton.GetComponentInChildren<LocText>().text = "None";
			return;
		}
		string name = Db.Get().Diseases[diseaseIdx].Name;
		this.diseaseButton.GetComponentInChildren<LocText>().text = name;
	}

	// Token: 0x0600560C RID: 22028 RVA: 0x001F5514 File Offset: 0x001F3714
	private void OnChangeFOWReveal()
	{
		if (this.paintPreventFOWReveal.isOn)
		{
			this.paintAllowFOWReveal.isOn = false;
		}
		if (this.paintAllowFOWReveal.isOn)
		{
			this.paintPreventFOWReveal.isOn = false;
		}
		this.set_prevent_fow_reveal = this.paintPreventFOWReveal.isOn;
		this.set_allow_fow_reveal = this.paintAllowFOWReveal.isOn;
	}

	// Token: 0x0600560D RID: 22029 RVA: 0x001F5578 File Offset: 0x001F3778
	public void OnChangeMassPressure()
	{
		float num;
		try
		{
			num = Convert.ToSingle(this.massPressureInput.text);
		}
		catch
		{
			num = -1f;
		}
		this.mass = num;
	}

	// Token: 0x0600560E RID: 22030 RVA: 0x001F55B8 File Offset: 0x001F37B8
	public void OnChangeTemperature()
	{
		float num;
		try
		{
			num = Convert.ToSingle(this.temperatureInput.text);
		}
		catch
		{
			num = -1f;
		}
		this.temperature = num;
	}

	// Token: 0x0600560F RID: 22031 RVA: 0x001F55F8 File Offset: 0x001F37F8
	public void OnDiseaseCountChange()
	{
		int num;
		int.TryParse(this.diseaseCountInput.text, out num);
		this.diseaseCount = num;
	}

	// Token: 0x06005610 RID: 22032 RVA: 0x001F561F File Offset: 0x001F381F
	public void OnElementsFilterEdited(string new_filter)
	{
		this.filter = (string.IsNullOrEmpty(this.filterInput.text) ? null : this.filterInput.text);
		this.FilterElements(this.filter);
	}

	// Token: 0x06005611 RID: 22033 RVA: 0x001F5654 File Offset: 0x001F3854
	public void SampleCell(int cell)
	{
		this.massPressureInput.text = (Grid.Pressure[cell] * 0.010000001f).ToString();
		this.temperatureInput.text = Grid.Temperature[cell].ToString();
		this.OnSelectElement(ElementLoader.GetElementID(Grid.Element[cell].tag));
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
	}

	// Token: 0x040039B5 RID: 14773
	[Header("Current State")]
	public SimHashes element;

	// Token: 0x040039B6 RID: 14774
	[NonSerialized]
	public float mass = 1000f;

	// Token: 0x040039B7 RID: 14775
	[NonSerialized]
	public float temperature = -1f;

	// Token: 0x040039B8 RID: 14776
	[NonSerialized]
	public bool set_prevent_fow_reveal;

	// Token: 0x040039B9 RID: 14777
	[NonSerialized]
	public bool set_allow_fow_reveal;

	// Token: 0x040039BA RID: 14778
	[NonSerialized]
	public int diseaseCount;

	// Token: 0x040039BB RID: 14779
	public byte diseaseIdx;

	// Token: 0x040039BC RID: 14780
	[Header("Popup Buttons")]
	[SerializeField]
	private KButton elementButton;

	// Token: 0x040039BD RID: 14781
	[SerializeField]
	private KButton diseaseButton;

	// Token: 0x040039BE RID: 14782
	[Header("Popup Menus")]
	[SerializeField]
	private KPopupMenu elementPopup;

	// Token: 0x040039BF RID: 14783
	[SerializeField]
	private KPopupMenu diseasePopup;

	// Token: 0x040039C0 RID: 14784
	[Header("Value Inputs")]
	[SerializeField]
	private KInputTextField massPressureInput;

	// Token: 0x040039C1 RID: 14785
	[SerializeField]
	private KInputTextField temperatureInput;

	// Token: 0x040039C2 RID: 14786
	[SerializeField]
	private KInputTextField diseaseCountInput;

	// Token: 0x040039C3 RID: 14787
	[SerializeField]
	private KInputTextField filterInput;

	// Token: 0x040039C4 RID: 14788
	[Header("Tool Buttons")]
	[SerializeField]
	private KButton paintButton;

	// Token: 0x040039C5 RID: 14789
	[SerializeField]
	private KButton fillButton;

	// Token: 0x040039C6 RID: 14790
	[SerializeField]
	private KButton sampleButton;

	// Token: 0x040039C7 RID: 14791
	[SerializeField]
	private KButton spawnButton;

	// Token: 0x040039C8 RID: 14792
	[SerializeField]
	private KButton storeButton;

	// Token: 0x040039C9 RID: 14793
	[Header("Parameter Toggles")]
	public Toggle paintElement;

	// Token: 0x040039CA RID: 14794
	public Toggle paintMass;

	// Token: 0x040039CB RID: 14795
	public Toggle paintTemperature;

	// Token: 0x040039CC RID: 14796
	public Toggle paintDisease;

	// Token: 0x040039CD RID: 14797
	public Toggle paintDiseaseCount;

	// Token: 0x040039CE RID: 14798
	public Toggle affectBuildings;

	// Token: 0x040039CF RID: 14799
	public Toggle affectCells;

	// Token: 0x040039D0 RID: 14800
	public Toggle paintPreventFOWReveal;

	// Token: 0x040039D1 RID: 14801
	public Toggle paintAllowFOWReveal;

	// Token: 0x040039D2 RID: 14802
	private List<KInputTextField> inputFields = new List<KInputTextField>();

	// Token: 0x040039D3 RID: 14803
	private List<string> options_list = new List<string>();

	// Token: 0x040039D4 RID: 14804
	private string filter;

	// Token: 0x020019FF RID: 6655
	private struct ElemDisplayInfo
	{
		// Token: 0x040077FB RID: 30715
		public SimHashes id;

		// Token: 0x040077FC RID: 30716
		public string displayStr;
	}
}
