using System;
using ImGuiNET;
using STRINGS;
using UnityEngine;

// Token: 0x02000573 RID: 1395
public class DevToolWarning
{
	// Token: 0x060021F6 RID: 8694 RVA: 0x000BA6B7 File Offset: 0x000B88B7
	public DevToolWarning()
	{
		this.Name = UI.FRONTEND.DEVTOOLS.TITLE;
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x000BA6CF File Offset: 0x000B88CF
	public void DrawMenuBar()
	{
		if (ImGui.BeginMainMenuBar())
		{
			ImGui.Checkbox(this.Name, ref this.ShouldDrawWindow);
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x000BA6F0 File Offset: 0x000B88F0
	public void DrawWindow(out bool isOpen)
	{
		ImGuiWindowFlags flags = ImGuiWindowFlags.None;
		isOpen = true;
		if (ImGui.Begin(this.Name + "###ID_DevToolWarning", ref isOpen, flags))
		{
			if (!isOpen)
			{
				ImGui.End();
				return;
			}
			ImGui.SetWindowSize(new Vector2(500f, 250f));
			ImGui.TextWrapped(UI.FRONTEND.DEVTOOLS.WARNING);
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Checkbox(UI.FRONTEND.DEVTOOLS.DONTSHOW, ref this.showAgain);
			if (ImGui.Button(UI.FRONTEND.DEVTOOLS.BUTTON))
			{
				if (this.showAgain)
				{
					KPlayerPrefs.SetInt("ShowDevtools", 1);
				}
				DevToolManager.Instance.UserAcceptedWarning = true;
				isOpen = false;
			}
			ImGui.End();
		}
	}

	// Token: 0x04001346 RID: 4934
	private bool showAgain;

	// Token: 0x04001347 RID: 4935
	public string Name;

	// Token: 0x04001348 RID: 4936
	public bool ShouldDrawWindow;
}
