using System;
using UnityEngine;

// Token: 0x02000B66 RID: 2918
public class PrioritizeRowTableColumn : TableColumn
{
	// Token: 0x06005A33 RID: 23091 RVA: 0x0021091C File Offset: 0x0020EB1C
	public PrioritizeRowTableColumn(object user_data, Action<object, int> on_change_priority, Func<object, int, string> on_hover_widget) : base(null, null, null, null, null, false, "")
	{
		this.userData = user_data;
		this.onChangePriority = on_change_priority;
		this.onHoverWidget = on_hover_widget;
	}

	// Token: 0x06005A34 RID: 23092 RVA: 0x00210944 File Offset: 0x0020EB44
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06005A35 RID: 23093 RVA: 0x0021094D File Offset: 0x0020EB4D
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06005A36 RID: 23094 RVA: 0x00210956 File Offset: 0x0020EB56
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowHeaderWidget, parent, true);
	}

	// Token: 0x06005A37 RID: 23095 RVA: 0x00210970 File Offset: 0x0020EB70
	private GameObject GetWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowWidget, parent, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		this.ConfigureButton(component, "UpButton", 1, gameObject);
		this.ConfigureButton(component, "DownButton", -1, gameObject);
		return gameObject;
	}

	// Token: 0x06005A38 RID: 23096 RVA: 0x002109B8 File Offset: 0x0020EBB8
	private void ConfigureButton(HierarchyReferences refs, string ref_id, int delta, GameObject widget_go)
	{
		KButton kbutton = refs.GetReference(ref_id) as KButton;
		kbutton.onClick += delegate()
		{
			this.onChangePriority(widget_go, delta);
		};
		ToolTip component = kbutton.GetComponent<ToolTip>();
		if (component != null)
		{
			component.OnToolTip = (() => this.onHoverWidget(widget_go, delta));
		}
	}

	// Token: 0x04003D24 RID: 15652
	public object userData;

	// Token: 0x04003D25 RID: 15653
	private Action<object, int> onChangePriority;

	// Token: 0x04003D26 RID: 15654
	private Func<object, int, string> onHoverWidget;
}
