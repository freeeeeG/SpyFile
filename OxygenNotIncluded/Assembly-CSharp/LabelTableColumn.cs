using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B61 RID: 2913
public class LabelTableColumn : TableColumn
{
	// Token: 0x06005A1D RID: 23069 RVA: 0x0021000A File Offset: 0x0020E20A
	public LabelTableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, string> get_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, int widget_width = 128, bool should_refresh_columns = false) : base(on_load_action, sort_comparison, on_tooltip, on_sort_tooltip, null, should_refresh_columns, "")
	{
		this.get_value_action = get_value_action;
		this.widget_width = widget_width;
	}

	// Token: 0x06005A1E RID: 23070 RVA: 0x0021003C File Offset: 0x0020E23C
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Label, parent, true);
		LayoutElement component = gameObject.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component.preferredWidth = (component.minWidth = (float)this.widget_width);
		return gameObject;
	}

	// Token: 0x06005A1F RID: 23071 RVA: 0x00210080 File Offset: 0x0020E280
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Label, parent, true);
		ToolTip tt = gameObject.GetComponent<ToolTip>();
		tt.OnToolTip = (() => this.GetTooltip(tt));
		LayoutElement component = gameObject.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component.preferredWidth = (component.minWidth = (float)this.widget_width);
		return gameObject;
	}

	// Token: 0x06005A20 RID: 23072 RVA: 0x002100F8 File Offset: 0x0020E2F8
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		GameObject widget_go = null;
		widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.LabelHeader, parent, true);
		MultiToggle componentInChildren = widget_go.GetComponentInChildren<MultiToggle>(true);
		this.column_sort_toggle = componentInChildren;
		MultiToggle multiToggle = componentInChildren;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.screen.SetSortComparison(this.sort_comparer, this);
			this.screen.SortRows();
		}));
		ToolTip tt = widget_go.GetComponent<ToolTip>();
		tt.OnToolTip = delegate()
		{
			this.on_tooltip(null, widget_go, tt);
			return "";
		};
		tt = widget_go.GetComponentInChildren<MultiToggle>().GetComponent<ToolTip>();
		tt.OnToolTip = delegate()
		{
			this.on_sort_tooltip(null, widget_go, tt);
			return "";
		};
		LayoutElement component = widget_go.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component.preferredWidth = (component.minWidth = (float)this.widget_width);
		return widget_go;
	}

	// Token: 0x04003D13 RID: 15635
	public Func<IAssignableIdentity, GameObject, string> get_value_action;

	// Token: 0x04003D14 RID: 15636
	private int widget_width = 128;
}
