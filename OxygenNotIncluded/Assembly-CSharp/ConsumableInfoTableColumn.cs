using System;
using UnityEngine;

// Token: 0x02000B69 RID: 2921
public class ConsumableInfoTableColumn : CheckboxTableColumn
{
	// Token: 0x06005A55 RID: 23125 RVA: 0x00211E14 File Offset: 0x00210014
	public ConsumableInfoTableColumn(IConsumableUIItem consumable_info, Action<IAssignableIdentity, GameObject> load_value_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, Func<GameObject, string> get_header_label) : base(load_value_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip, on_sort_tooltip, () => DebugHandler.InstantBuildMode || ConsumerManager.instance.isDiscovered(consumable_info.ConsumableId.ToTag()))
	{
		this.consumable_info = consumable_info;
		this.get_header_label = get_header_label;
	}

	// Token: 0x06005A56 RID: 23126 RVA: 0x00211E60 File Offset: 0x00210060
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		GameObject headerWidget = base.GetHeaderWidget(parent);
		if (headerWidget.GetComponentInChildren<LocText>() != null)
		{
			headerWidget.GetComponentInChildren<LocText>().text = this.get_header_label(headerWidget);
		}
		headerWidget.GetComponentInChildren<MultiToggle>().gameObject.SetActive(false);
		return headerWidget;
	}

	// Token: 0x04003D28 RID: 15656
	public IConsumableUIItem consumable_info;

	// Token: 0x04003D29 RID: 15657
	public Func<GameObject, string> get_header_label;
}
