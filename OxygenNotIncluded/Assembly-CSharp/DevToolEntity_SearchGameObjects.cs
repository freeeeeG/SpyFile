using System;
using ImGuiNET;

// Token: 0x0200055A RID: 1370
public class DevToolEntity_SearchGameObjects : DevTool
{
	// Token: 0x06002161 RID: 8545 RVA: 0x000B3785 File Offset: 0x000B1985
	public DevToolEntity_SearchGameObjects(Action<DevToolEntityTarget> onSelectionMadeFn)
	{
		this.onSelectionMadeFn = onSelectionMadeFn;
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x000B3794 File Offset: 0x000B1994
	protected override void RenderTo(DevPanel panel)
	{
		ImGui.Text("Not implemented yet");
	}

	// Token: 0x040012CD RID: 4813
	private Action<DevToolEntityTarget> onSelectionMadeFn;
}
