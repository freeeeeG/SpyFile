﻿using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000565 RID: 1381
public class DevToolResearchDebugger : DevTool
{
	// Token: 0x0600219B RID: 8603 RVA: 0x000B5F03 File Offset: 0x000B4103
	public DevToolResearchDebugger()
	{
		this.RequiresGameRunning = true;
	}

	// Token: 0x0600219C RID: 8604 RVA: 0x000B5F14 File Offset: 0x000B4114
	protected override void RenderTo(DevPanel panel)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			ImGui.Text("No Active Research");
			return;
		}
		ImGui.Text("Active Research");
		ImGui.Text("ID: " + activeResearch.tech.Id);
		ImGui.Text("Name: " + Util.StripTextFormatting(activeResearch.tech.Name));
		ImGui.Separator();
		ImGui.Text("Active Research Inventory");
		foreach (KeyValuePair<string, float> keyValuePair in new Dictionary<string, float>(activeResearch.progressInventory.PointsByTypeID))
		{
			if (activeResearch.tech.RequiresResearchType(keyValuePair.Key))
			{
				float num = activeResearch.tech.costsByResearchTypeID[keyValuePair.Key];
				float value = keyValuePair.Value;
				if (ImGui.Button("Fill"))
				{
					value = num;
				}
				ImGui.SameLine();
				ImGui.SetNextItemWidth(100f);
				ImGui.InputFloat(keyValuePair.Key, ref value, 1f, 10f);
				ImGui.SameLine();
				ImGui.Text(string.Format("of {0}", num));
				activeResearch.progressInventory.PointsByTypeID[keyValuePair.Key] = Mathf.Clamp(value, 0f, num);
			}
		}
		ImGui.Separator();
		ImGui.Text("Global Points Inventory");
		foreach (KeyValuePair<string, float> keyValuePair2 in Research.Instance.globalPointInventory.PointsByTypeID)
		{
			ImGui.Text(keyValuePair2.Key + ": " + keyValuePair2.Value.ToString());
		}
	}
}
