using System;
using ImGuiNET;

// Token: 0x02000566 RID: 1382
public class DevToolSaveGameInfo : DevTool
{
	// Token: 0x0600219E RID: 8606 RVA: 0x000B6118 File Offset: 0x000B4318
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("No game loaded");
			return;
		}
		ImGui.Text("Seed: " + CustomGameSettings.Instance.GetSettingsCoordinate());
		ImGui.Text("Generated: " + Game.Instance.dateGenerated);
		ImGui.Text("DebugWasUsed: " + Game.Instance.debugWasUsed.ToString());
		ImGui.PushItemWidth(100f);
		ImGui.NewLine();
		ImGui.Text("Changelists played on");
		ImGui.InputText("Search", ref this.clSearch, 10U);
		ImGui.PopItemWidth();
		foreach (uint num in Game.Instance.changelistsPlayedOn)
		{
			if (this.clSearch.IsNullOrWhiteSpace() || num.ToString().Contains(this.clSearch))
			{
				ImGui.Text(num.ToString());
			}
		}
		ImGui.NewLine();
		if (ImGui.Button("Open Story Manager"))
		{
			DevToolUtil.Open<DevToolStoryManager>();
		}
	}

	// Token: 0x0400130D RID: 4877
	private string clSearch = "";
}
