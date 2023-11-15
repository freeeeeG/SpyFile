using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B6D RID: 2925
public class TableColumn : IRender1000ms
{
	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x06005AA9 RID: 23209 RVA: 0x00214D2A File Offset: 0x00212F2A
	public bool isRevealed
	{
		get
		{
			return this.revealed == null || this.revealed();
		}
	}

	// Token: 0x06005AAA RID: 23210 RVA: 0x00214D44 File Offset: 0x00212F44
	public TableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip = null, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip = null, Func<bool> revealed = null, bool should_refresh_columns = false, string scrollerID = "")
	{
		this.on_load_action = on_load_action;
		this.sort_comparer = sort_comparison;
		this.on_tooltip = on_tooltip;
		this.on_sort_tooltip = on_sort_tooltip;
		this.revealed = revealed;
		this.scrollerID = scrollerID;
		if (should_refresh_columns)
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}
	}

	// Token: 0x06005AAB RID: 23211 RVA: 0x00214DA0 File Offset: 0x00212FA0
	protected string GetTooltip(ToolTip tool_tip_instance)
	{
		GameObject gameObject = tool_tip_instance.gameObject;
		HierarchyReferences component = tool_tip_instance.GetComponent<HierarchyReferences>();
		if (component != null && component.HasReference("Widget"))
		{
			gameObject = component.GetReference("Widget").gameObject;
		}
		TableRow tableRow = null;
		foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
		{
			if (keyValuePair.Value == gameObject)
			{
				tableRow = keyValuePair.Key;
				break;
			}
		}
		if (tableRow != null && this.on_tooltip != null)
		{
			this.on_tooltip(tableRow.GetIdentity(), gameObject, tool_tip_instance);
		}
		return "";
	}

	// Token: 0x06005AAC RID: 23212 RVA: 0x00214E68 File Offset: 0x00213068
	protected string GetSortTooltip(ToolTip sort_tooltip_instance)
	{
		GameObject gameObject = sort_tooltip_instance.transform.parent.gameObject;
		TableRow tableRow = null;
		foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
		{
			if (keyValuePair.Value == gameObject)
			{
				tableRow = keyValuePair.Key;
				break;
			}
		}
		if (tableRow != null && this.on_sort_tooltip != null)
		{
			this.on_sort_tooltip(tableRow.GetIdentity(), gameObject, sort_tooltip_instance);
		}
		return "";
	}

	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x06005AAD RID: 23213 RVA: 0x00214F0C File Offset: 0x0021310C
	public bool isDirty
	{
		get
		{
			return this.dirty;
		}
	}

	// Token: 0x06005AAE RID: 23214 RVA: 0x00214F14 File Offset: 0x00213114
	public bool ContainsWidget(GameObject widget)
	{
		return this.widgets_by_row.ContainsValue(widget);
	}

	// Token: 0x06005AAF RID: 23215 RVA: 0x00214F22 File Offset: 0x00213122
	public virtual GameObject GetMinionWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06005AB0 RID: 23216 RVA: 0x00214F2F File Offset: 0x0021312F
	public virtual GameObject GetHeaderWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06005AB1 RID: 23217 RVA: 0x00214F3C File Offset: 0x0021313C
	public virtual GameObject GetDefaultWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06005AB2 RID: 23218 RVA: 0x00214F49 File Offset: 0x00213149
	public void Render1000ms(float dt)
	{
		this.MarkDirty(null, TableScreen.ResultValues.False);
	}

	// Token: 0x06005AB3 RID: 23219 RVA: 0x00214F53 File Offset: 0x00213153
	public void MarkDirty(GameObject triggering_obj = null, TableScreen.ResultValues triggering_object_state = TableScreen.ResultValues.False)
	{
		this.dirty = true;
	}

	// Token: 0x06005AB4 RID: 23220 RVA: 0x00214F5C File Offset: 0x0021315C
	public void MarkClean()
	{
		this.dirty = false;
	}

	// Token: 0x04003D38 RID: 15672
	public Action<IAssignableIdentity, GameObject> on_load_action;

	// Token: 0x04003D39 RID: 15673
	public Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip;

	// Token: 0x04003D3A RID: 15674
	public Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip;

	// Token: 0x04003D3B RID: 15675
	public Comparison<IAssignableIdentity> sort_comparer;

	// Token: 0x04003D3C RID: 15676
	public Dictionary<TableRow, GameObject> widgets_by_row = new Dictionary<TableRow, GameObject>();

	// Token: 0x04003D3D RID: 15677
	public string scrollerID;

	// Token: 0x04003D3E RID: 15678
	public TableScreen screen;

	// Token: 0x04003D3F RID: 15679
	public MultiToggle column_sort_toggle;

	// Token: 0x04003D40 RID: 15680
	private Func<bool> revealed;

	// Token: 0x04003D41 RID: 15681
	protected bool dirty;
}
