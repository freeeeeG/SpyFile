using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BF7 RID: 3063
public class ArtableSelectionSideScreen : SideScreenContent
{
	// Token: 0x060060E0 RID: 24800 RVA: 0x0023C94C File Offset: 0x0023AB4C
	public override bool IsValidForTarget(GameObject target)
	{
		Artable component = target.GetComponent<Artable>();
		return !(component == null) && !(component.CurrentStage == "Default");
	}

	// Token: 0x060060E1 RID: 24801 RVA: 0x0023C980 File Offset: 0x0023AB80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.applyButton.onClick += delegate()
		{
			this.target.SetUserChosenTargetState(this.selectedStage);
			SelectTool.Instance.Select(null, true);
		};
		this.clearButton.onClick += delegate()
		{
			this.selectedStage = "";
			this.target.SetDefault();
			SelectTool.Instance.Select(null, true);
		};
	}

	// Token: 0x060060E2 RID: 24802 RVA: 0x0023C9B8 File Offset: 0x0023ABB8
	public override void SetTarget(GameObject target)
	{
		if (this.workCompleteSub != -1)
		{
			target.Unsubscribe(this.workCompleteSub);
			this.workCompleteSub = -1;
		}
		base.SetTarget(target);
		this.target = target.GetComponent<Artable>();
		this.workCompleteSub = target.Subscribe(-2011693419, new Action<object>(this.OnRefreshTarget));
		this.OnRefreshTarget(null);
	}

	// Token: 0x060060E3 RID: 24803 RVA: 0x0023CA18 File Offset: 0x0023AC18
	public override void ClearTarget()
	{
		this.target.Unsubscribe(-2011693419);
		this.workCompleteSub = -1;
		base.ClearTarget();
	}

	// Token: 0x060060E4 RID: 24804 RVA: 0x0023CA37 File Offset: 0x0023AC37
	private void OnRefreshTarget(object data = null)
	{
		if (this.target == null)
		{
			return;
		}
		this.GenerateStateButtons();
		this.selectedStage = this.target.CurrentStage;
		this.RefreshButtons();
	}

	// Token: 0x060060E5 RID: 24805 RVA: 0x0023CA68 File Offset: 0x0023AC68
	public void GenerateStateButtons()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.buttons)
		{
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.buttons.Clear();
		foreach (ArtableStage artableStage in Db.GetArtableStages().GetPrefabStages(this.target.GetComponent<KPrefabID>().PrefabID()))
		{
			if (!(artableStage.id == "Default"))
			{
				GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
				Sprite sprite = artableStage.GetPermitPresentationInfo().sprite;
				MultiToggle component = gameObject.GetComponent<MultiToggle>();
				component.GetComponent<ToolTip>().SetSimpleTooltip(artableStage.Name);
				component.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = sprite;
				this.buttons.Add(artableStage.id, component);
			}
		}
	}

	// Token: 0x060060E6 RID: 24806 RVA: 0x0023CBA0 File Offset: 0x0023ADA0
	private void RefreshButtons()
	{
		List<ArtableStage> prefabStages = Db.GetArtableStages().GetPrefabStages(this.target.GetComponent<KPrefabID>().PrefabID());
		ArtableStage artableStage = prefabStages.Find((ArtableStage match) => match.id == this.target.CurrentStage);
		int num = 0;
		using (Dictionary<string, MultiToggle>.Enumerator enumerator = this.buttons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ArtableSelectionSideScreen.<>c__DisplayClass16_0 CS$<>8__locals1 = new ArtableSelectionSideScreen.<>c__DisplayClass16_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.kvp = enumerator.Current;
				ArtableStage stage = prefabStages.Find((ArtableStage match) => match.id == CS$<>8__locals1.kvp.Key);
				if (stage != null && artableStage != null && stage.statusItem.StatusType != artableStage.statusItem.StatusType)
				{
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(false);
				}
				else if (!stage.IsUnlocked())
				{
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(false);
				}
				else
				{
					num++;
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(true);
					CS$<>8__locals1.kvp.Value.ChangeState((this.selectedStage == CS$<>8__locals1.kvp.Key) ? 1 : 0);
					MultiToggle value = CS$<>8__locals1.kvp.Value;
					value.onClick = (System.Action)Delegate.Combine(value.onClick, new System.Action(delegate()
					{
						CS$<>8__locals1.<>4__this.selectedStage = stage.id;
						CS$<>8__locals1.<>4__this.RefreshButtons();
					}));
				}
			}
		}
		this.scrollTransoform.GetComponent<LayoutElement>().preferredHeight = (float)((num > 3) ? 200 : 100);
	}

	// Token: 0x040041FE RID: 16894
	private Artable target;

	// Token: 0x040041FF RID: 16895
	public KButton applyButton;

	// Token: 0x04004200 RID: 16896
	public KButton clearButton;

	// Token: 0x04004201 RID: 16897
	public GameObject stateButtonPrefab;

	// Token: 0x04004202 RID: 16898
	private Dictionary<string, MultiToggle> buttons = new Dictionary<string, MultiToggle>();

	// Token: 0x04004203 RID: 16899
	[SerializeField]
	private RectTransform scrollTransoform;

	// Token: 0x04004204 RID: 16900
	private string selectedStage = "";

	// Token: 0x04004205 RID: 16901
	private const int INVALID_SUBSCRIPTION = -1;

	// Token: 0x04004206 RID: 16902
	private int workCompleteSub = -1;

	// Token: 0x04004207 RID: 16903
	[SerializeField]
	private RectTransform buttonContainer;
}
