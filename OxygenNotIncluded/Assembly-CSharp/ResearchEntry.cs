using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000BD2 RID: 3026
[AddComponentMenu("KMonoBehaviour/scripts/ResearchEntry")]
public class ResearchEntry : KMonoBehaviour
{
	// Token: 0x06005F16 RID: 24342 RVA: 0x0022E924 File Offset: 0x0022CB24
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.techLineMap = new Dictionary<Tech, UILineRenderer>();
		this.BG.color = this.defaultColor;
		foreach (Tech tech in this.targetTech.requiredTech)
		{
			float num = this.targetTech.width / 2f + 18f;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			if (tech.center.y > this.targetTech.center.y + 2f)
			{
				zero = new Vector2(0f, 20f);
				zero2 = new Vector2(0f, -20f);
			}
			else if (tech.center.y < this.targetTech.center.y - 2f)
			{
				zero = new Vector2(0f, -20f);
				zero2 = new Vector2(0f, 20f);
			}
			UILineRenderer component = Util.KInstantiateUI(this.linePrefab, this.lineContainer.gameObject, true).GetComponent<UILineRenderer>();
			float num2 = 32f;
			component.Points = new Vector2[]
			{
				new Vector2(0f, 0f) + zero,
				new Vector2(-num2, 0f) + zero,
				new Vector2(-num2, tech.center.y - this.targetTech.center.y) + zero2,
				new Vector2(-(this.targetTech.center.x - num - (tech.center.x + num)) + 2f, tech.center.y - this.targetTech.center.y) + zero2
			};
			component.LineThickness = (float)this.lineThickness_inactive;
			component.color = this.inactiveLineColor;
			this.techLineMap.Add(tech, component);
		}
		this.QueueStateChanged(false);
		if (this.targetTech != null)
		{
			using (List<TechInstance>.Enumerator enumerator2 = Research.Instance.GetResearchQueue().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.tech == this.targetTech)
					{
						this.QueueStateChanged(true);
					}
				}
			}
		}
	}

	// Token: 0x06005F17 RID: 24343 RVA: 0x0022EBE4 File Offset: 0x0022CDE4
	public void SetTech(Tech newTech)
	{
		if (newTech == null)
		{
			global::Debug.LogError("The research provided is null!");
			return;
		}
		if (this.targetTech == newTech)
		{
			return;
		}
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			if (newTech.costsByResearchTypeID.ContainsKey(researchType.id) && newTech.costsByResearchTypeID[researchType.id] > 0f)
			{
				GameObject gameObject = Util.KInstantiateUI(this.progressBarPrefab, this.progressBarContainer.gameObject, true);
				Image image = gameObject.GetComponentsInChildren<Image>()[2];
				Image component = gameObject.transform.Find("Icon").GetComponent<Image>();
				image.color = researchType.color;
				component.sprite = researchType.sprite;
				this.progressBarsByResearchTypeID[researchType.id] = gameObject;
			}
		}
		if (this.researchScreen == null)
		{
			this.researchScreen = base.transform.parent.GetComponentInParent<ResearchScreen>();
		}
		if (newTech.IsComplete())
		{
			this.ResearchCompleted(false);
		}
		this.targetTech = newTech;
		this.researchName.text = this.targetTech.Name;
		string text = "";
		foreach (TechItem techItem in this.targetTech.unlockedItems)
		{
			HierarchyReferences component2 = this.GetFreeIcon().GetComponent<HierarchyReferences>();
			if (text != "")
			{
				text += ", ";
			}
			text += techItem.Name;
			component2.GetReference<KImage>("Icon").sprite = techItem.UISprite();
			component2.GetReference<KImage>("Background");
			component2.GetReference<KImage>("DLCOverlay").gameObject.SetActive(!DlcManager.IsValidForVanilla(techItem.dlcIds));
			string text2 = string.Format("{0}\n{1}", techItem.Name, techItem.description);
			if (!DlcManager.IsValidForVanilla(techItem.dlcIds))
			{
				text2 += RESEARCH.MESSAGING.DLC.EXPANSION1;
			}
			component2.GetComponent<ToolTip>().toolTip = text2;
		}
		text = string.Format(UI.RESEARCHSCREEN_UNLOCKSTOOLTIP, text);
		this.researchName.GetComponent<ToolTip>().toolTip = string.Format("{0}\n{1}\n\n{2}", this.targetTech.Name, this.targetTech.desc, text);
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		this.toggle.onPointerEnter += delegate()
		{
			this.researchScreen.TurnEverythingOff();
			this.OnHover(true, this.targetTech);
		};
		this.toggle.soundPlayer.AcceptClickCondition = (() => !this.targetTech.IsComplete());
		this.toggle.onPointerExit += delegate()
		{
			this.researchScreen.TurnEverythingOff();
		};
	}

	// Token: 0x06005F18 RID: 24344 RVA: 0x0022EEF0 File Offset: 0x0022D0F0
	public void SetEverythingOff()
	{
		if (!this.isOn)
		{
			return;
		}
		this.borderHighlight.gameObject.SetActive(false);
		foreach (KeyValuePair<Tech, UILineRenderer> keyValuePair in this.techLineMap)
		{
			keyValuePair.Value.LineThickness = (float)this.lineThickness_inactive;
			keyValuePair.Value.color = this.inactiveLineColor;
		}
		this.isOn = false;
	}

	// Token: 0x06005F19 RID: 24345 RVA: 0x0022EF84 File Offset: 0x0022D184
	public void SetEverythingOn()
	{
		if (this.isOn)
		{
			return;
		}
		this.UpdateProgressBars();
		this.borderHighlight.gameObject.SetActive(true);
		foreach (KeyValuePair<Tech, UILineRenderer> keyValuePair in this.techLineMap)
		{
			keyValuePair.Value.LineThickness = (float)this.lineThickness_active;
			keyValuePair.Value.color = this.activeLineColor;
		}
		base.transform.SetAsLastSibling();
		this.isOn = true;
	}

	// Token: 0x06005F1A RID: 24346 RVA: 0x0022F028 File Offset: 0x0022D228
	public void OnHover(bool entered, Tech hoverSource)
	{
		this.SetEverythingOn();
		foreach (Tech tech in this.targetTech.requiredTech)
		{
			ResearchEntry entry = this.researchScreen.GetEntry(tech);
			if (entry != null)
			{
				entry.OnHover(entered, this.targetTech);
			}
		}
	}

	// Token: 0x06005F1B RID: 24347 RVA: 0x0022F0A4 File Offset: 0x0022D2A4
	private void OnResearchClicked()
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null && activeResearch.tech != this.targetTech)
		{
			this.researchScreen.CancelResearch();
		}
		Research.Instance.SetActiveResearch(this.targetTech, true);
		if (DebugHandler.InstantBuildMode)
		{
			Research.Instance.CompleteQueue();
		}
		this.UpdateProgressBars();
	}

	// Token: 0x06005F1C RID: 24348 RVA: 0x0022F100 File Offset: 0x0022D300
	private void OnResearchCanceled()
	{
		if (this.targetTech.IsComplete())
		{
			return;
		}
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		this.researchScreen.CancelResearch();
		Research.Instance.CancelResearch(this.targetTech, true);
	}

	// Token: 0x06005F1D RID: 24349 RVA: 0x0022F15C File Offset: 0x0022D35C
	public void QueueStateChanged(bool isSelected)
	{
		if (isSelected)
		{
			if (!this.targetTech.IsComplete())
			{
				this.toggle.isOn = true;
				this.BG.color = this.pendingColor;
				this.titleBG.color = this.pendingHeaderColor;
				this.toggle.ClearOnClick();
				this.toggle.onClick += this.OnResearchCanceled;
			}
			else
			{
				this.toggle.isOn = false;
			}
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
			{
				keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
			}
			Image[] componentsInChildren = this.iconPanel.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.StandardUIMaterial;
			}
			return;
		}
		if (this.targetTech.IsComplete())
		{
			this.toggle.isOn = false;
			this.BG.color = this.completedColor;
			this.titleBG.color = this.completedHeaderColor;
			this.defaultColor = this.completedColor;
			this.toggle.ClearOnClick();
			foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.progressBarsByResearchTypeID)
			{
				keyValuePair2.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
			}
			Image[] componentsInChildren = this.iconPanel.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.StandardUIMaterial;
			}
			return;
		}
		this.toggle.isOn = false;
		this.BG.color = this.defaultColor;
		this.titleBG.color = this.incompleteHeaderColor;
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		foreach (KeyValuePair<string, GameObject> keyValuePair3 in this.progressBarsByResearchTypeID)
		{
			keyValuePair3.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = new Color(0.52156866f, 0.52156866f, 0.52156866f);
		}
	}

	// Token: 0x06005F1E RID: 24350 RVA: 0x0022F400 File Offset: 0x0022D600
	public void UpdateFilterState(bool state)
	{
		this.filterLowlight.gameObject.SetActive(!state);
	}

	// Token: 0x06005F1F RID: 24351 RVA: 0x0022F423 File Offset: 0x0022D623
	public void SetPercentage(float percent)
	{
	}

	// Token: 0x06005F20 RID: 24352 RVA: 0x0022F428 File Offset: 0x0022D628
	public void UpdateProgressBars()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
		{
			Transform child = keyValuePair.Value.transform.GetChild(0);
			float fillAmount;
			if (this.targetTech.IsComplete())
			{
				fillAmount = 1f;
				child.GetComponentInChildren<LocText>().text = this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString() + "/" + this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString();
			}
			else
			{
				TechInstance orAdd = Research.Instance.GetOrAdd(this.targetTech);
				if (orAdd == null)
				{
					continue;
				}
				child.GetComponentInChildren<LocText>().text = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key].ToString() + "/" + this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString();
				fillAmount = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key] / this.targetTech.costsByResearchTypeID[keyValuePair.Key];
			}
			child.GetComponentsInChildren<Image>()[2].fillAmount = fillAmount;
			child.GetComponent<ToolTip>().SetSimpleTooltip(Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).description);
		}
	}

	// Token: 0x06005F21 RID: 24353 RVA: 0x0022F5E0 File Offset: 0x0022D7E0
	private GameObject GetFreeIcon()
	{
		GameObject gameObject = Util.KInstantiateUI(this.iconPrefab, this.iconPanel, false);
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06005F22 RID: 24354 RVA: 0x0022F5FB File Offset: 0x0022D7FB
	private Image GetFreeLine()
	{
		return Util.KInstantiateUI<Image>(this.linePrefab.gameObject, base.gameObject, false);
	}

	// Token: 0x06005F23 RID: 24355 RVA: 0x0022F614 File Offset: 0x0022D814
	public void ResearchCompleted(bool notify = true)
	{
		this.BG.color = this.completedColor;
		this.titleBG.color = this.completedHeaderColor;
		this.defaultColor = this.completedColor;
		if (notify)
		{
			this.unlockedTechMetric[ResearchEntry.UnlockedTechKey] = this.targetTech.Id;
			ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedTechMetric, "ResearchCompleted");
		}
		this.toggle.ClearOnClick();
		if (notify)
		{
			ResearchCompleteMessage message = new ResearchCompleteMessage(this.targetTech);
			MusicManager.instance.PlaySong("Stinger_ResearchComplete", false);
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x04004050 RID: 16464
	[Header("Labels")]
	[SerializeField]
	private LocText researchName;

	// Token: 0x04004051 RID: 16465
	[Header("Transforms")]
	[SerializeField]
	private Transform progressBarContainer;

	// Token: 0x04004052 RID: 16466
	[SerializeField]
	private Transform lineContainer;

	// Token: 0x04004053 RID: 16467
	[Header("Prefabs")]
	[SerializeField]
	private GameObject iconPanel;

	// Token: 0x04004054 RID: 16468
	[SerializeField]
	private GameObject iconPrefab;

	// Token: 0x04004055 RID: 16469
	[SerializeField]
	private GameObject linePrefab;

	// Token: 0x04004056 RID: 16470
	[SerializeField]
	private GameObject progressBarPrefab;

	// Token: 0x04004057 RID: 16471
	[Header("Graphics")]
	[SerializeField]
	private Image BG;

	// Token: 0x04004058 RID: 16472
	[SerializeField]
	private Image titleBG;

	// Token: 0x04004059 RID: 16473
	[SerializeField]
	private Image borderHighlight;

	// Token: 0x0400405A RID: 16474
	[SerializeField]
	private Image filterHighlight;

	// Token: 0x0400405B RID: 16475
	[SerializeField]
	private Image filterLowlight;

	// Token: 0x0400405C RID: 16476
	[SerializeField]
	private Sprite hoverBG;

	// Token: 0x0400405D RID: 16477
	[SerializeField]
	private Sprite completedBG;

	// Token: 0x0400405E RID: 16478
	[Header("Colors")]
	[SerializeField]
	private Color defaultColor = Color.blue;

	// Token: 0x0400405F RID: 16479
	[SerializeField]
	private Color completedColor = Color.yellow;

	// Token: 0x04004060 RID: 16480
	[SerializeField]
	private Color pendingColor = Color.magenta;

	// Token: 0x04004061 RID: 16481
	[SerializeField]
	private Color completedHeaderColor = Color.grey;

	// Token: 0x04004062 RID: 16482
	[SerializeField]
	private Color incompleteHeaderColor = Color.grey;

	// Token: 0x04004063 RID: 16483
	[SerializeField]
	private Color pendingHeaderColor = Color.grey;

	// Token: 0x04004064 RID: 16484
	private Sprite defaultBG;

	// Token: 0x04004065 RID: 16485
	[MyCmpGet]
	private KToggle toggle;

	// Token: 0x04004066 RID: 16486
	private ResearchScreen researchScreen;

	// Token: 0x04004067 RID: 16487
	private Dictionary<Tech, UILineRenderer> techLineMap;

	// Token: 0x04004068 RID: 16488
	private Tech targetTech;

	// Token: 0x04004069 RID: 16489
	private bool isOn = true;

	// Token: 0x0400406A RID: 16490
	private Coroutine fadeRoutine;

	// Token: 0x0400406B RID: 16491
	public Color activeLineColor;

	// Token: 0x0400406C RID: 16492
	public Color inactiveLineColor;

	// Token: 0x0400406D RID: 16493
	public int lineThickness_active = 6;

	// Token: 0x0400406E RID: 16494
	public int lineThickness_inactive = 2;

	// Token: 0x0400406F RID: 16495
	public Material StandardUIMaterial;

	// Token: 0x04004070 RID: 16496
	private Dictionary<string, GameObject> progressBarsByResearchTypeID = new Dictionary<string, GameObject>();

	// Token: 0x04004071 RID: 16497
	public static readonly string UnlockedTechKey = "UnlockedTech";

	// Token: 0x04004072 RID: 16498
	private Dictionary<string, object> unlockedTechMetric = new Dictionary<string, object>
	{
		{
			ResearchEntry.UnlockedTechKey,
			null
		}
	};
}
