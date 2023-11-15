using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C97 RID: 3223
public class GroupSelectorHeaderWidget : MonoBehaviour
{
	// Token: 0x060066A0 RID: 26272 RVA: 0x00264194 File Offset: 0x00262394
	public void Initialize(object widget_id, IList<GroupSelectorWidget.ItemData> options, GroupSelectorHeaderWidget.ItemCallbacks item_callbacks)
	{
		GroupSelectorHeaderWidget.<>c__DisplayClass11_0 CS$<>8__locals1 = new GroupSelectorHeaderWidget.<>c__DisplayClass11_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.widget_id = widget_id;
		this.widgetID = CS$<>8__locals1.widget_id;
		this.options = options;
		this.itemCallbacks = item_callbacks;
		if (this.itemCallbacks.getTitleHoverText != null)
		{
			this.label.GetComponent<ToolTip>().OnToolTip = (() => CS$<>8__locals1.<>4__this.itemCallbacks.getTitleHoverText(CS$<>8__locals1.widget_id));
		}
		bool adding_item2 = true;
		Func<object, IList<int>> <>9__5;
		Func<object, object, string> <>9__6;
		this.addItemButton.onClick += delegate()
		{
			GroupSelectorHeaderWidget <>4__this = CS$<>8__locals1.<>4__this;
			Vector3 position = CS$<>8__locals1.<>4__this.addItemButton.transform.GetPosition();
			Func<object, IList<int>> display_list_query;
			if ((display_list_query = <>9__5) == null)
			{
				display_list_query = (<>9__5 = ((object widget_go) => CS$<>8__locals1.<>4__this.itemCallbacks.getHeaderButtonOptions(widget_go, adding_item2)));
			}
			Action<object> onItemAdded = CS$<>8__locals1.<>4__this.itemCallbacks.onItemAdded;
			Func<object, object, string> get_item_hover_text;
			if ((get_item_hover_text = <>9__6) == null)
			{
				get_item_hover_text = (<>9__6 = ((object widget_go, object item_data) => CS$<>8__locals1.<>4__this.itemCallbacks.getItemHoverText(widget_go, adding_item2, item_data)));
			}
			<>4__this.RebuildSubPanel(position, display_list_query, onItemAdded, get_item_hover_text);
		};
		bool adding_item = false;
		Func<object, IList<int>> <>9__8;
		Func<object, object, string> <>9__9;
		this.removeItemButton.onClick += delegate()
		{
			GroupSelectorHeaderWidget <>4__this = CS$<>8__locals1.<>4__this;
			Vector3 position = CS$<>8__locals1.<>4__this.removeItemButton.transform.GetPosition();
			Func<object, IList<int>> display_list_query;
			if ((display_list_query = <>9__8) == null)
			{
				display_list_query = (<>9__8 = ((object widget_go) => CS$<>8__locals1.<>4__this.itemCallbacks.getHeaderButtonOptions(widget_go, adding_item)));
			}
			Action<object> onItemRemoved = CS$<>8__locals1.<>4__this.itemCallbacks.onItemRemoved;
			Func<object, object, string> get_item_hover_text;
			if ((get_item_hover_text = <>9__9) == null)
			{
				get_item_hover_text = (<>9__9 = ((object widget_go, object item_data) => CS$<>8__locals1.<>4__this.itemCallbacks.getItemHoverText(widget_go, adding_item, item_data)));
			}
			<>4__this.RebuildSubPanel(position, display_list_query, onItemRemoved, get_item_hover_text);
		};
		this.sortButton.onClick += delegate()
		{
			GroupSelectorHeaderWidget <>4__this = CS$<>8__locals1.<>4__this;
			Vector3 position = CS$<>8__locals1.<>4__this.sortButton.transform.GetPosition();
			Func<object, IList<int>> getValidSortOptionIndices = CS$<>8__locals1.<>4__this.itemCallbacks.getValidSortOptionIndices;
			Action<object> on_item_selected;
			if ((on_item_selected = CS$<>8__locals1.<>9__10) == null)
			{
				on_item_selected = (CS$<>8__locals1.<>9__10 = delegate(object item_data)
				{
					CS$<>8__locals1.<>4__this.itemCallbacks.onSort(CS$<>8__locals1.<>4__this.widgetID, item_data);
				});
			}
			Func<object, object, string> get_item_hover_text;
			if ((get_item_hover_text = CS$<>8__locals1.<>9__11) == null)
			{
				get_item_hover_text = (CS$<>8__locals1.<>9__11 = ((object widget_go, object item_data) => CS$<>8__locals1.<>4__this.itemCallbacks.getSortHoverText(item_data)));
			}
			<>4__this.RebuildSubPanel(position, getValidSortOptionIndices, on_item_selected, get_item_hover_text);
		};
		if (this.itemCallbacks.getTitleButtonHoverText != null)
		{
			this.addItemButton.GetComponent<ToolTip>().OnToolTip = (() => CS$<>8__locals1.<>4__this.itemCallbacks.getTitleButtonHoverText(CS$<>8__locals1.widget_id, true));
			this.removeItemButton.GetComponent<ToolTip>().OnToolTip = (() => CS$<>8__locals1.<>4__this.itemCallbacks.getTitleButtonHoverText(CS$<>8__locals1.widget_id, false));
		}
	}

	// Token: 0x060066A1 RID: 26273 RVA: 0x002642AC File Offset: 0x002624AC
	private void RebuildSubPanel(Vector3 pos, Func<object, IList<int>> display_list_query, Action<object> on_item_selected, Func<object, object, string> get_item_hover_text)
	{
		this.itemsPanel.gameObject.transform.SetPosition(pos + new Vector3(2f, 2f, 0f));
		IList<int> list = display_list_query(this.widgetID);
		if (list.Count > 0)
		{
			this.ClearSubPanelOptions();
			foreach (int idx2 in list)
			{
				int idx = idx2;
				GroupSelectorWidget.ItemData itemData = this.options[idx];
				GameObject gameObject = Util.KInstantiateUI(this.itemTemplate, this.itemsPanel.gameObject, true);
				KButton component = gameObject.GetComponent<KButton>();
				component.fgImage.sprite = this.options[idx].sprite;
				component.onClick += delegate()
				{
					on_item_selected(this.options[idx].userData);
					this.RebuildSubPanel(pos, display_list_query, on_item_selected, get_item_hover_text);
				};
				if (get_item_hover_text != null)
				{
					gameObject.GetComponent<ToolTip>().OnToolTip = (() => get_item_hover_text(this.widgetID, this.options[idx].userData));
				}
			}
			this.itemsPanel.GetComponent<GridLayoutGroup>().constraintCount = Mathf.Min(this.numExpectedPanelColumns, this.itemsPanel.childCount);
			this.itemsPanel.gameObject.SetActive(true);
			this.itemsPanel.GetComponent<Selectable>().Select();
			return;
		}
		this.CloseSubPanel();
	}

	// Token: 0x060066A2 RID: 26274 RVA: 0x0026446C File Offset: 0x0026266C
	public void CloseSubPanel()
	{
		this.ClearSubPanelOptions();
		this.itemsPanel.gameObject.SetActive(false);
	}

	// Token: 0x060066A3 RID: 26275 RVA: 0x00264488 File Offset: 0x00262688
	private void ClearSubPanelOptions()
	{
		foreach (object obj in this.itemsPanel.transform)
		{
			Util.KDestroyGameObject(((Transform)obj).gameObject);
		}
	}

	// Token: 0x040046C8 RID: 18120
	public LocText label;

	// Token: 0x040046C9 RID: 18121
	[SerializeField]
	private GameObject itemTemplate;

	// Token: 0x040046CA RID: 18122
	[SerializeField]
	private RectTransform itemsPanel;

	// Token: 0x040046CB RID: 18123
	[SerializeField]
	private KButton addItemButton;

	// Token: 0x040046CC RID: 18124
	[SerializeField]
	private KButton removeItemButton;

	// Token: 0x040046CD RID: 18125
	[SerializeField]
	private KButton sortButton;

	// Token: 0x040046CE RID: 18126
	[SerializeField]
	private int numExpectedPanelColumns = 3;

	// Token: 0x040046CF RID: 18127
	private object widgetID;

	// Token: 0x040046D0 RID: 18128
	private GroupSelectorHeaderWidget.ItemCallbacks itemCallbacks;

	// Token: 0x040046D1 RID: 18129
	private IList<GroupSelectorWidget.ItemData> options;

	// Token: 0x02001BBA RID: 7098
	public struct ItemCallbacks
	{
		// Token: 0x04007DB2 RID: 32178
		public Func<object, string> getTitleHoverText;

		// Token: 0x04007DB3 RID: 32179
		public Func<object, bool, string> getTitleButtonHoverText;

		// Token: 0x04007DB4 RID: 32180
		public Func<object, bool, IList<int>> getHeaderButtonOptions;

		// Token: 0x04007DB5 RID: 32181
		public Action<object> onItemAdded;

		// Token: 0x04007DB6 RID: 32182
		public Action<object> onItemRemoved;

		// Token: 0x04007DB7 RID: 32183
		public Func<object, bool, object, string> getItemHoverText;

		// Token: 0x04007DB8 RID: 32184
		public Func<object, IList<int>> getValidSortOptionIndices;

		// Token: 0x04007DB9 RID: 32185
		public Func<object, string> getSortHoverText;

		// Token: 0x04007DBA RID: 32186
		public Action<object, object> onSort;
	}
}
