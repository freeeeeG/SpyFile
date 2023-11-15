﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C40 RID: 3136
public class ResearchSideScreen : SideScreenContent
{
	// Token: 0x0600634F RID: 25423 RVA: 0x0024B2FC File Offset: 0x002494FC
	public ResearchSideScreen()
	{
		this.refreshDisplayStateDelegate = new Action<object>(this.RefreshDisplayState);
	}

	// Token: 0x06006350 RID: 25424 RVA: 0x0024B318 File Offset: 0x00249518
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectResearchButton.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleResearch();
		};
		Research.Instance.Subscribe(-1914338957, this.refreshDisplayStateDelegate);
		Research.Instance.Subscribe(-125623018, this.refreshDisplayStateDelegate);
		this.RefreshDisplayState(null);
	}

	// Token: 0x06006351 RID: 25425 RVA: 0x0024B388 File Offset: 0x00249588
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RefreshDisplayState(null);
		this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
		this.target.gameObject.Subscribe(-1852328367, this.refreshDisplayStateDelegate);
		this.target.gameObject.Subscribe(-592767678, this.refreshDisplayStateDelegate);
	}

	// Token: 0x06006352 RID: 25426 RVA: 0x0024B3F4 File Offset: 0x002495F4
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.target)
		{
			this.target.gameObject.Unsubscribe(-1852328367, this.refreshDisplayStateDelegate);
			this.target.gameObject.Unsubscribe(187661686, this.refreshDisplayStateDelegate);
			this.target = null;
		}
	}

	// Token: 0x06006353 RID: 25427 RVA: 0x0024B454 File Offset: 0x00249654
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Research.Instance.Unsubscribe(-1914338957, this.refreshDisplayStateDelegate);
		Research.Instance.Unsubscribe(-125623018, this.refreshDisplayStateDelegate);
		if (this.target)
		{
			this.target.gameObject.Unsubscribe(-1852328367, this.refreshDisplayStateDelegate);
			this.target.gameObject.Unsubscribe(187661686, this.refreshDisplayStateDelegate);
			this.target = null;
		}
	}

	// Token: 0x06006354 RID: 25428 RVA: 0x0024B4DB File Offset: 0x002496DB
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ResearchCenter>() != null || target.GetComponent<NuclearResearchCenter>() != null;
	}

	// Token: 0x06006355 RID: 25429 RVA: 0x0024B4FC File Offset: 0x002496FC
	private void RefreshDisplayState(object data = null)
	{
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		string text = "";
		ResearchCenter component = SelectTool.Instance.selected.GetComponent<ResearchCenter>();
		NuclearResearchCenter component2 = SelectTool.Instance.selected.GetComponent<NuclearResearchCenter>();
		if (component != null)
		{
			text = component.research_point_type_id;
		}
		if (component2 != null)
		{
			text = component2.researchTypeID;
		}
		if (component == null && component2 == null)
		{
			return;
		}
		this.researchButtonIcon.sprite = Research.Instance.researchTypes.GetResearchType(text).sprite;
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.DescriptionText.text = "<b>" + UI.UISIDESCREENS.RESEARCHSIDESCREEN.NOSELECTEDRESEARCH + "</b>";
			return;
		}
		string text2 = "";
		if (!activeResearch.tech.costsByResearchTypeID.ContainsKey(text) || activeResearch.tech.costsByResearchTypeID[text] <= 0f)
		{
			text2 += "<color=#7f7f7f>";
		}
		text2 = text2 + "<b>" + activeResearch.tech.Name + "</b>";
		if (!activeResearch.tech.costsByResearchTypeID.ContainsKey(text) || activeResearch.tech.costsByResearchTypeID[text] <= 0f)
		{
			text2 += "</color>";
		}
		foreach (KeyValuePair<string, float> keyValuePair in activeResearch.tech.costsByResearchTypeID)
		{
			if (keyValuePair.Value != 0f)
			{
				bool flag = keyValuePair.Key == text;
				text2 += "\n   ";
				text2 += "<b>";
				if (!flag)
				{
					text2 += "<color=#7f7f7f>";
				}
				text2 = string.Concat(new string[]
				{
					text2,
					"- ",
					Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).name,
					": ",
					activeResearch.progressInventory.PointsByTypeID[keyValuePair.Key].ToString(),
					"/",
					activeResearch.tech.costsByResearchTypeID[keyValuePair.Key].ToString()
				});
				if (!flag)
				{
					text2 += "</color>";
				}
				text2 += "</b>";
			}
		}
		this.DescriptionText.text = text2;
	}

	// Token: 0x040043C5 RID: 17349
	public KButton selectResearchButton;

	// Token: 0x040043C6 RID: 17350
	public Image researchButtonIcon;

	// Token: 0x040043C7 RID: 17351
	public GameObject content;

	// Token: 0x040043C8 RID: 17352
	private GameObject target;

	// Token: 0x040043C9 RID: 17353
	private Action<object> refreshDisplayStateDelegate;

	// Token: 0x040043CA RID: 17354
	public LocText DescriptionText;
}
