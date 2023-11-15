using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C0B RID: 3083
public class ConditionListSideScreen : SideScreenContent
{
	// Token: 0x060061AC RID: 25004 RVA: 0x00241828 File Offset: 0x0023FA28
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x060061AD RID: 25005 RVA: 0x0024182B File Offset: 0x0023FA2B
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target != null)
		{
			this.targetConditionSet = target.GetComponent<IProcessConditionSet>();
		}
	}

	// Token: 0x060061AE RID: 25006 RVA: 0x00241849 File Offset: 0x0023FA49
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.Refresh();
		}
	}

	// Token: 0x060061AF RID: 25007 RVA: 0x0024185C File Offset: 0x0023FA5C
	private void Refresh()
	{
		bool flag = false;
		List<ProcessCondition> conditionSet = this.targetConditionSet.GetConditionSet(ProcessCondition.ProcessConditionType.All);
		foreach (ProcessCondition key in conditionSet)
		{
			if (!this.rows.ContainsKey(key))
			{
				flag = true;
				break;
			}
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.rows)
		{
			if (!conditionSet.Contains(keyValuePair.Key))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.Rebuild();
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair2 in this.rows)
		{
			ConditionListSideScreen.SetRowState(keyValuePair2.Value, keyValuePair2.Key);
		}
	}

	// Token: 0x060061B0 RID: 25008 RVA: 0x00241970 File Offset: 0x0023FB70
	public static void SetRowState(GameObject row, ProcessCondition condition)
	{
		HierarchyReferences component = row.GetComponent<HierarchyReferences>();
		ProcessCondition.Status status = condition.EvaluateCondition();
		component.GetReference<LocText>("Label").text = condition.GetStatusMessage(status);
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.failedColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.failedColor;
			break;
		case ProcessCondition.Status.Warning:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.warningColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.warningColor;
			break;
		case ProcessCondition.Status.Ready:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.readyColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.readyColor;
			break;
		}
		component.GetReference<Image>("Check").gameObject.SetActive(status == ProcessCondition.Status.Ready);
		component.GetReference<Image>("Dash").gameObject.SetActive(false);
		row.GetComponent<ToolTip>().SetSimpleTooltip(condition.GetStatusTooltip(status));
	}

	// Token: 0x060061B1 RID: 25009 RVA: 0x00241A7C File Offset: 0x0023FC7C
	private void Rebuild()
	{
		this.ClearRows();
		this.BuildRows();
	}

	// Token: 0x060061B2 RID: 25010 RVA: 0x00241A8C File Offset: 0x0023FC8C
	private void ClearRows()
	{
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
	}

	// Token: 0x060061B3 RID: 25011 RVA: 0x00241AF0 File Offset: 0x0023FCF0
	private void BuildRows()
	{
		foreach (ProcessCondition processCondition in this.targetConditionSet.GetConditionSet(ProcessCondition.ProcessConditionType.All))
		{
			if (processCondition.ShowInUI())
			{
				GameObject value = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
				this.rows.Add(processCondition, value);
			}
		}
	}

	// Token: 0x04004292 RID: 17042
	public GameObject rowPrefab;

	// Token: 0x04004293 RID: 17043
	public GameObject rowContainer;

	// Token: 0x04004294 RID: 17044
	[Tooltip("This list is indexed by the ProcessCondition.Status enum")]
	public static Color readyColor = Color.black;

	// Token: 0x04004295 RID: 17045
	public static Color failedColor = Color.red;

	// Token: 0x04004296 RID: 17046
	public static Color warningColor = new Color(1f, 0.3529412f, 0f, 1f);

	// Token: 0x04004297 RID: 17047
	private IProcessConditionSet targetConditionSet;

	// Token: 0x04004298 RID: 17048
	private Dictionary<ProcessCondition, GameObject> rows = new Dictionary<ProcessCondition, GameObject>();
}
