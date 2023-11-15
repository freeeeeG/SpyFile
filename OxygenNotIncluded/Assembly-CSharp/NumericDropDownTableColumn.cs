using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B63 RID: 2915
public class NumericDropDownTableColumn : TableColumn
{
	// Token: 0x06005A25 RID: 23077 RVA: 0x00210385 File Offset: 0x0020E585
	public NumericDropDownTableColumn(object user_data, List<TMP_Dropdown.OptionData> options, Action<IAssignableIdentity, GameObject> on_load_action, Action<GameObject, int> set_value_action, Comparison<IAssignableIdentity> sort_comparer, NumericDropDownTableColumn.ToolTipCallbacks callbacks, Func<bool> revealed = null) : base(on_load_action, sort_comparer, callbacks.headerTooltip, callbacks.headerSortTooltip, revealed, false, "")
	{
		this.userData = user_data;
		this.set_value_action = set_value_action;
		this.options = options;
		this.callbacks = callbacks;
	}

	// Token: 0x06005A26 RID: 23078 RVA: 0x002103C4 File Offset: 0x0020E5C4
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06005A27 RID: 23079 RVA: 0x002103CD File Offset: 0x0020E5CD
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06005A28 RID: 23080 RVA: 0x002103D8 File Offset: 0x0020E5D8
	private GameObject GetWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.NumericDropDown, parent, true);
		TMP_Dropdown componentInChildren = widget_go.transform.GetComponentInChildren<TMP_Dropdown>();
		componentInChildren.options = this.options;
		componentInChildren.onValueChanged.AddListener(delegate(int new_value)
		{
			this.set_value_action(widget_go, new_value);
		});
		ToolTip tt = widget_go.transform.GetComponentInChildren<ToolTip>();
		if (tt != null)
		{
			tt.OnToolTip = (() => this.GetTooltip(tt));
		}
		return widget_go;
	}

	// Token: 0x06005A29 RID: 23081 RVA: 0x00210484 File Offset: 0x0020E684
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		NumericDropDownTableColumn.<>c__DisplayClass9_0 CS$<>8__locals1 = new NumericDropDownTableColumn.<>c__DisplayClass9_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.DropDownHeader, parent, true);
		HierarchyReferences component = CS$<>8__locals1.widget_go.GetComponent<HierarchyReferences>();
		Component reference = component.GetReference("Label");
		MultiToggle componentInChildren = reference.GetComponentInChildren<MultiToggle>(true);
		this.column_sort_toggle = componentInChildren;
		MultiToggle multiToggle = componentInChildren;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			CS$<>8__locals1.<>4__this.screen.SetSortComparison(CS$<>8__locals1.<>4__this.sort_comparer, CS$<>8__locals1.<>4__this);
			CS$<>8__locals1.<>4__this.screen.SortRows();
		}));
		ToolTip tt2 = reference.GetComponent<ToolTip>();
		tt2.enabled = true;
		tt2.OnToolTip = delegate()
		{
			CS$<>8__locals1.<>4__this.callbacks.headerTooltip(null, CS$<>8__locals1.widget_go, tt2);
			return "";
		};
		ToolTip tt3 = componentInChildren.transform.GetComponent<ToolTip>();
		tt3.OnToolTip = delegate()
		{
			CS$<>8__locals1.<>4__this.callbacks.headerSortTooltip(null, CS$<>8__locals1.widget_go, tt3);
			return "";
		};
		Component reference2 = component.GetReference("DropDown");
		TMP_Dropdown componentInChildren2 = reference2.GetComponentInChildren<TMP_Dropdown>();
		componentInChildren2.options = this.options;
		componentInChildren2.onValueChanged.AddListener(delegate(int new_value)
		{
			CS$<>8__locals1.<>4__this.set_value_action(CS$<>8__locals1.widget_go, new_value);
		});
		ToolTip tt = reference2.GetComponent<ToolTip>();
		tt.OnToolTip = delegate()
		{
			CS$<>8__locals1.<>4__this.callbacks.headerDropdownTooltip(null, CS$<>8__locals1.widget_go, tt);
			return "";
		};
		LayoutElement component2 = CS$<>8__locals1.widget_go.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component2.preferredWidth = (component2.minWidth = 83f);
		return CS$<>8__locals1.widget_go;
	}

	// Token: 0x04003D18 RID: 15640
	public object userData;

	// Token: 0x04003D19 RID: 15641
	private NumericDropDownTableColumn.ToolTipCallbacks callbacks;

	// Token: 0x04003D1A RID: 15642
	private Action<GameObject, int> set_value_action;

	// Token: 0x04003D1B RID: 15643
	private List<TMP_Dropdown.OptionData> options;

	// Token: 0x02001A84 RID: 6788
	public class ToolTipCallbacks
	{
		// Token: 0x040079BE RID: 31166
		public Action<IAssignableIdentity, GameObject, ToolTip> headerTooltip;

		// Token: 0x040079BF RID: 31167
		public Action<IAssignableIdentity, GameObject, ToolTip> headerSortTooltip;

		// Token: 0x040079C0 RID: 31168
		public Action<IAssignableIdentity, GameObject, ToolTip> headerDropdownTooltip;
	}
}
