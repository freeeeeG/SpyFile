using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BB9 RID: 3001
public class OverlayLegend : KScreen
{
	// Token: 0x06005DCF RID: 24015 RVA: 0x00225308 File Offset: 0x00223508
	[ContextMenu("Set all fonts color")]
	public void SetAllFontsColor()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				if (overlayInfo.infoUnits[i].fontColor == Color.clear)
				{
					overlayInfo.infoUnits[i].fontColor = Color.white;
				}
			}
		}
	}

	// Token: 0x06005DD0 RID: 24016 RVA: 0x002253A0 File Offset: 0x002235A0
	[ContextMenu("Set all tooltips")]
	public void SetAllTooltips()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			string text = overlayInfo.name;
			text = text.Replace("NAME", "");
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				string text2 = overlayInfo.infoUnits[i].description;
				text2 = text2.Replace(text, "");
				text2 = text + "TOOLTIPS." + text2;
				overlayInfo.infoUnits[i].tooltip = text2;
			}
		}
	}

	// Token: 0x06005DD1 RID: 24017 RVA: 0x00225464 File Offset: 0x00223664
	[ContextMenu("Set Sliced for empty icons")]
	public void SetSlicedForEmptyIcons()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				if (overlayInfo.infoUnits[i].icon == this.emptySprite)
				{
					overlayInfo.infoUnits[i].sliceIcon = true;
				}
			}
		}
	}

	// Token: 0x06005DD2 RID: 24018 RVA: 0x002254F8 File Offset: 0x002236F8
	protected override void OnSpawn()
	{
		base.ConsumeMouseScroll = true;
		base.OnSpawn();
		if (OverlayLegend.Instance == null)
		{
			OverlayLegend.Instance = this;
			this.activeUnitObjs = new List<GameObject>();
			this.inactiveUnitObjs = new List<GameObject>();
			foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
			{
				overlayInfo.name = Strings.Get(overlayInfo.name);
				for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
				{
					overlayInfo.infoUnits[i].description = Strings.Get(overlayInfo.infoUnits[i].description);
					if (!string.IsNullOrEmpty(overlayInfo.infoUnits[i].tooltip))
					{
						overlayInfo.infoUnits[i].tooltip = Strings.Get(overlayInfo.infoUnits[i].tooltip);
					}
				}
			}
			base.GetComponent<LayoutElement>().minWidth = (float)(DlcManager.FeatureClusterSpaceEnabled() ? 322 : 288);
			this.ClearLegend();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06005DD3 RID: 24019 RVA: 0x00225654 File Offset: 0x00223854
	protected override void OnLoadLevel()
	{
		OverlayLegend.Instance = null;
		this.activeDiagrams.Clear();
		UnityEngine.Object.Destroy(base.gameObject);
		base.OnLoadLevel();
	}

	// Token: 0x06005DD4 RID: 24020 RVA: 0x00225678 File Offset: 0x00223878
	private void SetLegend(OverlayLegend.OverlayInfo overlayInfo)
	{
		if (overlayInfo == null)
		{
			this.ClearLegend();
			return;
		}
		if (!overlayInfo.isProgrammaticallyPopulated && (overlayInfo.infoUnits == null || overlayInfo.infoUnits.Count == 0))
		{
			this.ClearLegend();
			return;
		}
		this.Show(true);
		this.title.text = overlayInfo.name;
		if (overlayInfo.isProgrammaticallyPopulated)
		{
			this.PopulateGeneratedLegend(overlayInfo, false);
		}
		else
		{
			this.PopulateOverlayInfoUnits(overlayInfo, false);
		}
		this.ConfigureUIHeight();
	}

	// Token: 0x06005DD5 RID: 24021 RVA: 0x002256EC File Offset: 0x002238EC
	public void SetLegend(OverlayModes.Mode mode, bool refreshing = false)
	{
		if (this.currentMode != null && this.currentMode.ViewMode() == mode.ViewMode() && !refreshing)
		{
			return;
		}
		this.ClearLegend();
		OverlayLegend.OverlayInfo legend = this.overlayInfoList.Find((OverlayLegend.OverlayInfo ol) => ol.mode == mode.ViewMode());
		this.currentMode = mode;
		this.SetLegend(legend);
	}

	// Token: 0x06005DD6 RID: 24022 RVA: 0x00225760 File Offset: 0x00223960
	public GameObject GetFreeUnitObject()
	{
		if (this.inactiveUnitObjs.Count == 0)
		{
			this.inactiveUnitObjs.Add(Util.KInstantiateUI(this.unitPrefab, this.inactiveUnitsParent, false));
		}
		GameObject gameObject = this.inactiveUnitObjs[0];
		this.inactiveUnitObjs.RemoveAt(0);
		this.activeUnitObjs.Add(gameObject);
		return gameObject;
	}

	// Token: 0x06005DD7 RID: 24023 RVA: 0x002257C0 File Offset: 0x002239C0
	private void RemoveActiveObjects()
	{
		while (this.activeUnitObjs.Count > 0)
		{
			this.activeUnitObjs[0].transform.Find("Icon").GetComponent<Image>().enabled = false;
			this.activeUnitObjs[0].GetComponentInChildren<LocText>().enabled = false;
			this.activeUnitObjs[0].transform.SetParent(this.inactiveUnitsParent.transform);
			this.activeUnitObjs[0].SetActive(false);
			this.inactiveUnitObjs.Add(this.activeUnitObjs[0]);
			this.activeUnitObjs.RemoveAt(0);
		}
	}

	// Token: 0x06005DD8 RID: 24024 RVA: 0x00225878 File Offset: 0x00223A78
	public void ClearLegend()
	{
		this.RemoveActiveObjects();
		for (int i = 0; i < this.activeDiagrams.Count; i++)
		{
			if (this.activeDiagrams[i] != null)
			{
				UnityEngine.Object.Destroy(this.activeDiagrams[i]);
			}
		}
		this.activeDiagrams.Clear();
		Vector2 sizeDelta = this.diagramsParent.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.y = 0f;
		this.diagramsParent.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		this.Show(false);
	}

	// Token: 0x06005DD9 RID: 24025 RVA: 0x00225908 File Offset: 0x00223B08
	public OverlayLegend.OverlayInfo GetOverlayInfo(OverlayModes.Mode mode)
	{
		for (int i = 0; i < this.overlayInfoList.Count; i++)
		{
			if (this.overlayInfoList[i].mode == mode.ViewMode())
			{
				return this.overlayInfoList[i];
			}
		}
		return null;
	}

	// Token: 0x06005DDA RID: 24026 RVA: 0x00225958 File Offset: 0x00223B58
	private void PopulateOverlayInfoUnits(OverlayLegend.OverlayInfo overlayInfo, bool isRefresh = false)
	{
		if (overlayInfo.infoUnits != null && overlayInfo.infoUnits.Count > 0)
		{
			this.activeUnitsParent.SetActive(true);
			using (List<OverlayLegend.OverlayInfoUnit>.Enumerator enumerator = overlayInfo.infoUnits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					OverlayLegend.OverlayInfoUnit overlayInfoUnit = enumerator.Current;
					GameObject freeUnitObject = this.GetFreeUnitObject();
					if (overlayInfoUnit.icon != null)
					{
						Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
						component.gameObject.SetActive(true);
						component.sprite = overlayInfoUnit.icon;
						component.color = overlayInfoUnit.color;
						component.enabled = true;
						component.type = (overlayInfoUnit.sliceIcon ? Image.Type.Sliced : Image.Type.Simple);
					}
					else
					{
						freeUnitObject.transform.Find("Icon").gameObject.SetActive(false);
					}
					if (!string.IsNullOrEmpty(overlayInfoUnit.description))
					{
						LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
						componentInChildren.text = string.Format(overlayInfoUnit.description, overlayInfoUnit.formatData);
						componentInChildren.color = overlayInfoUnit.fontColor;
						componentInChildren.enabled = true;
					}
					ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
					if (!string.IsNullOrEmpty(overlayInfoUnit.tooltip))
					{
						component2.toolTip = string.Format(overlayInfoUnit.tooltip, overlayInfoUnit.tooltipFormatData);
						component2.enabled = true;
					}
					else
					{
						component2.enabled = false;
					}
					freeUnitObject.SetActive(true);
					freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
				}
				goto IL_180;
			}
		}
		this.activeUnitsParent.SetActive(false);
		IL_180:
		if (!isRefresh)
		{
			if (overlayInfo.diagrams != null && overlayInfo.diagrams.Count > 0)
			{
				this.diagramsParent.SetActive(true);
				using (List<GameObject>.Enumerator enumerator2 = overlayInfo.diagrams.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameObject original = enumerator2.Current;
						GameObject item = Util.KInstantiateUI(original, this.diagramsParent, false);
						this.activeDiagrams.Add(item);
					}
					return;
				}
			}
			this.diagramsParent.SetActive(false);
		}
	}

	// Token: 0x06005DDB RID: 24027 RVA: 0x00225B94 File Offset: 0x00223D94
	private void PopulateGeneratedLegend(OverlayLegend.OverlayInfo info, bool isRefresh = false)
	{
		if (isRefresh)
		{
			this.RemoveActiveObjects();
		}
		if (info.infoUnits != null && info.infoUnits.Count > 0)
		{
			this.PopulateOverlayInfoUnits(info, isRefresh);
		}
		List<LegendEntry> customLegendData = this.currentMode.GetCustomLegendData();
		if (customLegendData != null)
		{
			this.activeUnitsParent.SetActive(true);
			using (List<LegendEntry>.Enumerator enumerator = customLegendData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LegendEntry legendEntry = enumerator.Current;
					GameObject freeUnitObject = this.GetFreeUnitObject();
					Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
					component.gameObject.SetActive(legendEntry.displaySprite);
					component.sprite = legendEntry.sprite;
					component.color = legendEntry.colour;
					component.enabled = true;
					component.type = Image.Type.Simple;
					LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
					componentInChildren.text = legendEntry.name;
					componentInChildren.color = Color.white;
					componentInChildren.enabled = true;
					ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
					component2.enabled = (legendEntry.desc != null || legendEntry.desc_arg != null);
					component2.toolTip = ((legendEntry.desc_arg == null) ? legendEntry.desc : string.Format(legendEntry.desc, legendEntry.desc_arg));
					freeUnitObject.SetActive(true);
					freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
				}
				goto IL_157;
			}
		}
		this.activeUnitsParent.SetActive(false);
		IL_157:
		if (!isRefresh && this.currentMode.legendFilters != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.toolParameterMenuPrefab, this.diagramsParent, false);
			this.activeDiagrams.Add(gameObject);
			this.diagramsParent.SetActive(true);
			this.filterMenu = gameObject.GetComponent<ToolParameterMenu>();
			this.filterMenu.PopulateMenu(this.currentMode.legendFilters);
			this.filterMenu.onParametersChanged += this.OnFiltersChanged;
			this.OnFiltersChanged();
		}
	}

	// Token: 0x06005DDC RID: 24028 RVA: 0x00225D84 File Offset: 0x00223F84
	private void OnFiltersChanged()
	{
		this.currentMode.OnFiltersChanged();
		this.PopulateGeneratedLegend(this.GetOverlayInfo(this.currentMode), true);
		Game.Instance.ForceOverlayUpdate(false);
	}

	// Token: 0x06005DDD RID: 24029 RVA: 0x00225DAF File Offset: 0x00223FAF
	private void DisableOverlay()
	{
		this.filterMenu.onParametersChanged -= this.OnFiltersChanged;
		this.filterMenu.ClearMenu();
		this.filterMenu.gameObject.SetActive(false);
		this.filterMenu = null;
	}

	// Token: 0x06005DDE RID: 24030 RVA: 0x00225DEC File Offset: 0x00223FEC
	private void ConfigureUIHeight()
	{
		this.scrollRectLayout.enabled = false;
		this.scrollRectLayout.GetComponent<VerticalLayoutGroup>().enabled = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		this.scrollRectLayout.preferredWidth = this.scrollRectLayout.rectTransform().sizeDelta.x;
		float y = this.scrollRectLayout.rectTransform().sizeDelta.y;
		this.scrollRectLayout.preferredHeight = Mathf.Min(y, 512f);
		this.scrollRectLayout.GetComponent<VerticalLayoutGroup>().enabled = false;
		this.scrollRectLayout.enabled = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
	}

	// Token: 0x04003F2F RID: 16175
	public static OverlayLegend Instance;

	// Token: 0x04003F30 RID: 16176
	[SerializeField]
	private LocText title;

	// Token: 0x04003F31 RID: 16177
	[SerializeField]
	private Sprite emptySprite;

	// Token: 0x04003F32 RID: 16178
	[SerializeField]
	private List<OverlayLegend.OverlayInfo> overlayInfoList;

	// Token: 0x04003F33 RID: 16179
	[SerializeField]
	private GameObject unitPrefab;

	// Token: 0x04003F34 RID: 16180
	[SerializeField]
	private GameObject activeUnitsParent;

	// Token: 0x04003F35 RID: 16181
	[SerializeField]
	private GameObject diagramsParent;

	// Token: 0x04003F36 RID: 16182
	[SerializeField]
	private GameObject inactiveUnitsParent;

	// Token: 0x04003F37 RID: 16183
	[SerializeField]
	private GameObject toolParameterMenuPrefab;

	// Token: 0x04003F38 RID: 16184
	[SerializeField]
	private LayoutElement scrollRectLayout;

	// Token: 0x04003F39 RID: 16185
	private ToolParameterMenu filterMenu;

	// Token: 0x04003F3A RID: 16186
	private OverlayModes.Mode currentMode;

	// Token: 0x04003F3B RID: 16187
	private List<GameObject> inactiveUnitObjs;

	// Token: 0x04003F3C RID: 16188
	private List<GameObject> activeUnitObjs;

	// Token: 0x04003F3D RID: 16189
	private List<GameObject> activeDiagrams = new List<GameObject>();

	// Token: 0x02001AF7 RID: 6903
	[Serializable]
	public class OverlayInfoUnit
	{
		// Token: 0x060098AC RID: 39084 RVA: 0x00342934 File Offset: 0x00340B34
		public OverlayInfoUnit(Sprite icon, string description, Color color, Color fontColor, object formatData = null, bool sliceIcon = false)
		{
			this.icon = icon;
			this.description = description;
			this.color = color;
			this.fontColor = fontColor;
			this.formatData = formatData;
			this.sliceIcon = sliceIcon;
		}

		// Token: 0x04007B31 RID: 31537
		public Sprite icon;

		// Token: 0x04007B32 RID: 31538
		public string description;

		// Token: 0x04007B33 RID: 31539
		public string tooltip;

		// Token: 0x04007B34 RID: 31540
		public Color color;

		// Token: 0x04007B35 RID: 31541
		public Color fontColor;

		// Token: 0x04007B36 RID: 31542
		public object formatData;

		// Token: 0x04007B37 RID: 31543
		public object tooltipFormatData;

		// Token: 0x04007B38 RID: 31544
		public bool sliceIcon;
	}

	// Token: 0x02001AF8 RID: 6904
	[Serializable]
	public class OverlayInfo
	{
		// Token: 0x04007B39 RID: 31545
		public string name;

		// Token: 0x04007B3A RID: 31546
		public HashedString mode;

		// Token: 0x04007B3B RID: 31547
		public List<OverlayLegend.OverlayInfoUnit> infoUnits;

		// Token: 0x04007B3C RID: 31548
		public List<GameObject> diagrams;

		// Token: 0x04007B3D RID: 31549
		public bool isProgrammaticallyPopulated;
	}
}
