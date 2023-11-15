using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BF8 RID: 3064
public class ArtifactAnalysisSideScreen : SideScreenContent
{
	// Token: 0x060060EB RID: 24811 RVA: 0x0023CE18 File Offset: 0x0023B018
	public override string GetTitle()
	{
		if (this.targetArtifactStation != null)
		{
			return string.Format(base.GetTitle(), this.targetArtifactStation.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x060060EC RID: 24812 RVA: 0x0023CE45 File Offset: 0x0023B045
	public override void ClearTarget()
	{
		this.targetArtifactStation = null;
		base.ClearTarget();
	}

	// Token: 0x060060ED RID: 24813 RVA: 0x0023CE54 File Offset: 0x0023B054
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<ArtifactAnalysisStation.StatesInstance>() != null;
	}

	// Token: 0x060060EE RID: 24814 RVA: 0x0023CE60 File Offset: 0x0023B060
	private void RefreshRows()
	{
		if (this.undiscoveredRow == null)
		{
			this.undiscoveredRow = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
			HierarchyReferences component = this.undiscoveredRow.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED);
			component.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED_TOOLTIP);
			component.GetReference<Image>("icon").sprite = Assets.GetSprite("unknown");
			component.GetReference<Image>("icon").color = Color.grey;
		}
		List<string> analyzedArtifactIDs = ArtifactSelector.Instance.GetAnalyzedArtifactIDs();
		this.undiscoveredRow.SetActive(analyzedArtifactIDs.Count == 0);
		foreach (string text in analyzedArtifactIDs)
		{
			if (!this.rows.ContainsKey(text))
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
				this.rows.Add(text, gameObject);
				GameObject artifactPrefab = Assets.GetPrefab(text);
				HierarchyReferences component2 = gameObject.GetComponent<HierarchyReferences>();
				component2.GetReference<LocText>("label").SetText(artifactPrefab.GetProperName());
				component2.GetReference<Image>("icon").sprite = Def.GetUISprite(artifactPrefab, text, false).first;
				component2.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenEvent(artifactPrefab);
				};
			}
		}
	}

	// Token: 0x060060EF RID: 24815 RVA: 0x0023D010 File Offset: 0x0023B210
	private void OpenEvent(GameObject artifactPrefab)
	{
		SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.ArtifactReveal, -1, null).smi as SimpleEvent.StatesInstance;
		statesInstance.artifact = artifactPrefab;
		artifactPrefab.GetComponent<KPrefabID>();
		artifactPrefab.GetComponent<InfoDescription>();
		string text = artifactPrefab.PrefabID().Name.ToUpper();
		text = text.Replace("ARTIFACT_", "");
		string key = "STRINGS.UI.SPACEARTIFACTS." + text + ".ARTIFACT";
		string text2 = string.Format("<b>{0}</b>", artifactPrefab.GetProperName());
		StringEntry stringEntry;
		Strings.TryGet(key, out stringEntry);
		if (stringEntry != null && !stringEntry.String.IsNullOrWhiteSpace())
		{
			text2 = text2 + "\n\n" + stringEntry.String;
		}
		if (text2 != null && !text2.IsNullOrWhiteSpace())
		{
			statesInstance.SetTextParameter("desc", text2);
		}
		statesInstance.ShowEventPopup();
	}

	// Token: 0x060060F0 RID: 24816 RVA: 0x0023D0E6 File Offset: 0x0023B2E6
	public override void SetTarget(GameObject target)
	{
		this.targetArtifactStation = target;
		base.SetTarget(target);
		this.RefreshRows();
	}

	// Token: 0x04004208 RID: 16904
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x04004209 RID: 16905
	private GameObject targetArtifactStation;

	// Token: 0x0400420A RID: 16906
	[SerializeField]
	private GameObject rowContainer;

	// Token: 0x0400420B RID: 16907
	private Dictionary<string, GameObject> rows = new Dictionary<string, GameObject>();

	// Token: 0x0400420C RID: 16908
	private GameObject undiscoveredRow;
}
