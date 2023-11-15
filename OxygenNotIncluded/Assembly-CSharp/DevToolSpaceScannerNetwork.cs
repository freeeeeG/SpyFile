using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x0200056A RID: 1386
public class DevToolSpaceScannerNetwork : DevTool
{
	// Token: 0x060021B1 RID: 8625 RVA: 0x000B890C File Offset: 0x000B6B0C
	public DevToolSpaceScannerNetwork()
	{
		this.tableDrawer = ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry>.New().Column("WorldId", (DevToolSpaceScannerNetwork.Entry e) => e.worldId).Column("Network Quality (0->1)", (DevToolSpaceScannerNetwork.Entry e) => e.networkQuality).Column("Targets Detected", (DevToolSpaceScannerNetwork.Entry e) => e.targetsString).FixedHeight(300f).Build();
	}

	// Token: 0x060021B2 RID: 8626 RVA: 0x000B89B4 File Offset: 0x000B6BB4
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("Game instance is null");
			return;
		}
		if (Game.Instance.spaceScannerNetworkManager == null)
		{
			ImGui.Text("SpaceScannerNetworkQualityManager instance is null");
			return;
		}
		if (ClusterManager.Instance == null)
		{
			ImGui.Text("ClusterManager instance is null");
			return;
		}
		if (ImGui.CollapsingHeader("Worlds Data"))
		{
			this.tableDrawer.Draw(DevToolSpaceScannerNetwork.GetData());
		}
		if (ImGui.CollapsingHeader("Full DevToolSpaceScannerNetwork Info"))
		{
			ImGuiEx.DrawObject(Game.Instance.spaceScannerNetworkManager, null);
		}
	}

	// Token: 0x060021B3 RID: 8627 RVA: 0x000B8A48 File Offset: 0x000B6C48
	public static IEnumerable<DevToolSpaceScannerNetwork.Entry> GetData()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			yield return new DevToolSpaceScannerNetwork.Entry(worldContainer.id, Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(worldContainer.id), DevToolSpaceScannerNetwork.GetTargetsString(worldContainer));
		}
		IEnumerator<WorldContainer> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060021B4 RID: 8628 RVA: 0x000B8A54 File Offset: 0x000B6C54
	public static string GetTargetsString(WorldContainer world)
	{
		SpaceScannerWorldData spaceScannerWorldData;
		if (!Game.Instance.spaceScannerNetworkManager.DEBUG_GetWorldIdToDataMap().TryGetValue(world.id, out spaceScannerWorldData))
		{
			return "<none>";
		}
		if (spaceScannerWorldData.targetIdsDetected.Count == 0)
		{
			return "<none>";
		}
		return string.Join(",", spaceScannerWorldData.targetIdsDetected);
	}

	// Token: 0x0400132F RID: 4911
	private ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry> tableDrawer;

	// Token: 0x020011FC RID: 4604
	public readonly struct Entry
	{
		// Token: 0x06007B5C RID: 31580 RVA: 0x002DDCC0 File Offset: 0x002DBEC0
		public Entry(int worldId, float networkQuality, string targetsString)
		{
			this.worldId = worldId;
			this.networkQuality = networkQuality;
			this.targetsString = targetsString;
		}

		// Token: 0x04005E1E RID: 24094
		public readonly int worldId;

		// Token: 0x04005E1F RID: 24095
		public readonly float networkQuality;

		// Token: 0x04005E20 RID: 24096
		public readonly string targetsString;
	}
}
