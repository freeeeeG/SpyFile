﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ImGuiNET;
using UnityEngine;

// Token: 0x0200056F RID: 1391
public class DevToolStringsTable : DevTool
{
	// Token: 0x060021DC RID: 8668 RVA: 0x000B9CA4 File Offset: 0x000B7EA4
	protected override void RenderTo(DevPanel panel)
	{
		if (this.m_cached_entries == null)
		{
			this.m_cached_entries = new List<ValueTuple<string, string>>();
			DevToolStringsTable.RegenerateCacheWithFilter(this.m_cached_entries, this.m_search_filter);
		}
		if (ImGui.CollapsingHeader(string.Format("Entries ({0})###ID_LocStringEntries", this.m_cached_entries.Count), ImGuiTreeNodeFlags.DefaultOpen))
		{
			if (ImGuiEx.InputFilter("Filter", ref this.m_search_filter, 50U))
			{
				DevToolStringsTable.RegenerateCacheWithFilter(this.m_cached_entries, this.m_search_filter);
			}
			ImGui.Columns(2, "LocStrings");
			ImGui.Text("Key");
			ImGui.NextColumn();
			ImGui.Text("Value");
			ImGui.NextColumn();
			ImGui.Separator();
			int num = Mathf.Min(3000, this.m_cached_entries.Count);
			for (int i = 0; i < num; i++)
			{
				ValueTuple<string, string> valueTuple = this.m_cached_entries[i];
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				if (ImGui.Selectable(string.Format("{0}###ID_{1}_key", item, i)))
				{
					this.m_search_filter = item;
					DevToolStringsTable.RegenerateCacheWithFilter(this.m_cached_entries, this.m_search_filter);
					break;
				}
				ImGuiEx.TooltipForPrevious(item ?? "");
				ImGui.NextColumn();
				if (ImGui.Selectable(string.Format("{0}###ID_{1}_value", item2, i)))
				{
					this.m_search_filter = item2;
					DevToolStringsTable.RegenerateCacheWithFilter(this.m_cached_entries, this.m_search_filter);
					break;
				}
				ImGuiEx.TooltipForPrevious(item2 ?? "");
				ImGui.NextColumn();
			}
			ImGui.Columns(1);
			if (this.m_cached_entries.Count > 3000)
			{
				ImGui.Separator();
				ImGui.Text(string.Format("* Stopped drawing entries because there are too many to draw (limit: {0}, current: {1}) *", 3000, this.m_cached_entries.Count));
			}
		}
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x000B9E64 File Offset: 0x000B8064
	public static void RegenerateCacheWithFilter([TupleElementNames(new string[]
	{
		"id",
		"value"
	})] List<ValueTuple<string, string>> cached_entries, string filter)
	{
		cached_entries.Clear();
		if (!string.IsNullOrWhiteSpace(filter))
		{
			string normalized_filter = filter.ToLowerInvariant().Trim();
			Strings.VisitEntries(delegate(string id, string value)
			{
				if (!id.ToLowerInvariant().Contains(normalized_filter) && !value.ToLowerInvariant().Contains(normalized_filter))
				{
					return;
				}
				cached_entries.Add(new ValueTuple<string, string>(id, value));
			});
			return;
		}
		Strings.VisitEntries(delegate(string id, string value)
		{
			cached_entries.Add(new ValueTuple<string, string>(id, value));
		});
	}

	// Token: 0x0400133F RID: 4927
	[TupleElementNames(new string[]
	{
		"id",
		"value"
	})]
	private List<ValueTuple<string, string>> m_cached_entries;

	// Token: 0x04001340 RID: 4928
	private const int MAX_ENTRIES_TO_DRAW = 3000;

	// Token: 0x04001341 RID: 4929
	private string m_search_filter = "";
}
