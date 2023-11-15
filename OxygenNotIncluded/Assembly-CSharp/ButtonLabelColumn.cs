using System;
using UnityEngine;

// Token: 0x02000B62 RID: 2914
public class ButtonLabelColumn : LabelTableColumn
{
	// Token: 0x06005A21 RID: 23073 RVA: 0x002101EB File Offset: 0x0020E3EB
	public ButtonLabelColumn(Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, string> get_value_action, Action<GameObject> on_click_action, Action<GameObject> on_double_click_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, bool whiteText = false) : base(on_load_action, get_value_action, sort_comparison, on_tooltip, on_sort_tooltip, 128, false)
	{
		this.on_click_action = on_click_action;
		this.on_double_click_action = on_double_click_action;
		this.whiteText = whiteText;
	}

	// Token: 0x06005A22 RID: 23074 RVA: 0x00210218 File Offset: 0x0020E418
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.whiteText ? Assets.UIPrefabs.TableScreenWidgets.ButtonLabelWhite : Assets.UIPrefabs.TableScreenWidgets.ButtonLabel, parent, true);
		if (this.on_click_action != null)
		{
			widget_go.GetComponent<KButton>().onClick += delegate()
			{
				this.on_click_action(widget_go);
			};
		}
		if (this.on_double_click_action != null)
		{
			widget_go.GetComponent<KButton>().onDoubleClick += delegate()
			{
				this.on_double_click_action(widget_go);
			};
		}
		return widget_go;
	}

	// Token: 0x06005A23 RID: 23075 RVA: 0x002102B5 File Offset: 0x0020E4B5
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return base.GetHeaderWidget(parent);
	}

	// Token: 0x06005A24 RID: 23076 RVA: 0x002102C0 File Offset: 0x0020E4C0
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.whiteText ? Assets.UIPrefabs.TableScreenWidgets.ButtonLabelWhite : Assets.UIPrefabs.TableScreenWidgets.ButtonLabel, parent, true);
		ToolTip tt = widget_go.GetComponent<ToolTip>();
		tt.OnToolTip = (() => this.GetTooltip(tt));
		if (this.on_click_action != null)
		{
			widget_go.GetComponent<KButton>().onClick += delegate()
			{
				this.on_click_action(widget_go);
			};
		}
		if (this.on_double_click_action != null)
		{
			widget_go.GetComponent<KButton>().onDoubleClick += delegate()
			{
				this.on_double_click_action(widget_go);
			};
		}
		return widget_go;
	}

	// Token: 0x04003D15 RID: 15637
	private Action<GameObject> on_click_action;

	// Token: 0x04003D16 RID: 15638
	private Action<GameObject> on_double_click_action;

	// Token: 0x04003D17 RID: 15639
	private bool whiteText;
}
