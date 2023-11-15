using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

// Token: 0x0200054A RID: 1354
public class DevPanel
{
	// Token: 0x1700018A RID: 394
	// (get) Token: 0x060020F4 RID: 8436 RVA: 0x000AFB9E File Offset: 0x000ADD9E
	// (set) Token: 0x060020F5 RID: 8437 RVA: 0x000AFBA6 File Offset: 0x000ADDA6
	public bool isRequestingToClose { get; private set; }

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x060020F6 RID: 8438 RVA: 0x000AFBAF File Offset: 0x000ADDAF
	// (set) Token: 0x060020F7 RID: 8439 RVA: 0x000AFBB7 File Offset: 0x000ADDB7
	public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowPosition { get; private set; }

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x060020F8 RID: 8440 RVA: 0x000AFBC0 File Offset: 0x000ADDC0
	// (set) Token: 0x060020F9 RID: 8441 RVA: 0x000AFBC8 File Offset: 0x000ADDC8
	public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowSize { get; private set; }

	// Token: 0x060020FA RID: 8442 RVA: 0x000AFBD4 File Offset: 0x000ADDD4
	public DevPanel(DevTool devTool, DevPanelList manager)
	{
		this.manager = manager;
		this.devTools = new List<DevTool>();
		this.devTools.Add(devTool);
		this.currentDevToolIndex = 0;
		this.initialDevToolType = devTool.GetType();
		manager.Internal_InitPanelId(this.initialDevToolType, out this.uniquePanelId, out this.idPostfixNumber);
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x000AFC30 File Offset: 0x000ADE30
	public void PushValue<T>(T value) where T : class
	{
		this.PushDevTool(new DevToolObjectViewer<T>(() => value));
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x000AFC61 File Offset: 0x000ADE61
	public void PushValue<T>(Func<T> value)
	{
		this.PushDevTool(new DevToolObjectViewer<T>(value));
	}

	// Token: 0x060020FD RID: 8445 RVA: 0x000AFC6F File Offset: 0x000ADE6F
	public void PushDevTool<T>() where T : DevTool, new()
	{
		this.PushDevTool(Activator.CreateInstance<T>());
	}

	// Token: 0x060020FE RID: 8446 RVA: 0x000AFC84 File Offset: 0x000ADE84
	public void PushDevTool(DevTool devTool)
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			this.manager.AddPanelFor(devTool);
			return;
		}
		for (int i = this.devTools.Count - 1; i > this.currentDevToolIndex; i--)
		{
			this.devTools[i].Internal_Uninit();
			this.devTools.RemoveAt(i);
		}
		this.devTools.Add(devTool);
		this.currentDevToolIndex = this.devTools.Count - 1;
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x000AFD04 File Offset: 0x000ADF04
	public bool NavGoBack()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(-1);
		if (option.IsNone())
		{
			return false;
		}
		this.currentDevToolIndex = option.Unwrap();
		return true;
	}

	// Token: 0x06002100 RID: 8448 RVA: 0x000AFD34 File Offset: 0x000ADF34
	public bool NavGoForward()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(1);
		if (option.IsNone())
		{
			return false;
		}
		this.currentDevToolIndex = option.Unwrap();
		return true;
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x000AFD62 File Offset: 0x000ADF62
	public DevTool GetCurrentDevTool()
	{
		return this.devTools[this.currentDevToolIndex];
	}

	// Token: 0x06002102 RID: 8450 RVA: 0x000AFD78 File Offset: 0x000ADF78
	public Option<int> TryGetDevToolIndexByOffset(int offsetFromCurrentIndex)
	{
		int num = this.currentDevToolIndex + offsetFromCurrentIndex;
		if (num < 0)
		{
			return Option.None;
		}
		if (num >= this.devTools.Count)
		{
			return Option.None;
		}
		return num;
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x000AFDBC File Offset: 0x000ADFBC
	public void RenderPanel()
	{
		DevTool currentDevTool = this.GetCurrentDevTool();
		currentDevTool.Internal_TryInit();
		if (currentDevTool.isRequestingToClosePanel)
		{
			this.isRequestingToClose = true;
			return;
		}
		ImGuiWindowFlags flags;
		this.ConfigureImGuiWindowFor(currentDevTool, out flags);
		bool flag = true;
		if (ImGui.Begin(currentDevTool.Name + "###ID_" + this.uniquePanelId, ref flag, flags))
		{
			if (!flag)
			{
				this.isRequestingToClose = true;
				ImGui.End();
				return;
			}
			if (ImGui.BeginMenuBar())
			{
				this.DrawNavigation();
				ImGui.SameLine(0f, 20f);
				this.DrawMenuBarContents();
				ImGui.EndMenuBar();
			}
			currentDevTool.DoImGui(this);
			if (this.GetCurrentDevTool() != currentDevTool)
			{
				ImGui.SetScrollY(0f);
			}
		}
		ImGui.End();
		if (this.GetCurrentDevTool().isRequestingToClosePanel)
		{
			this.isRequestingToClose = true;
		}
	}

	// Token: 0x06002104 RID: 8452 RVA: 0x000AFE7C File Offset: 0x000AE07C
	private void DrawNavigation()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(-1);
		if (ImGuiEx.Button(" < ", option.IsSome()))
		{
			this.currentDevToolIndex = option.Unwrap();
		}
		if (option.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go back to " + this.devTools[option.Unwrap()].Name);
		}
		else
		{
			ImGuiEx.TooltipForPrevious("Go back");
		}
		ImGui.SameLine(0f, 5f);
		Option<int> option2 = this.TryGetDevToolIndexByOffset(1);
		if (ImGuiEx.Button(" > ", option2.IsSome()))
		{
			this.currentDevToolIndex = option2.Unwrap();
		}
		if (option2.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go forward to " + this.devTools[option2.Unwrap()].Name);
			return;
		}
		ImGuiEx.TooltipForPrevious("Go forward");
	}

	// Token: 0x06002105 RID: 8453 RVA: 0x000AFF5D File Offset: 0x000AE15D
	private void DrawMenuBarContents()
	{
	}

	// Token: 0x06002106 RID: 8454 RVA: 0x000AFF60 File Offset: 0x000AE160
	private void ConfigureImGuiWindowFor(DevTool currentDevTool, out ImGuiWindowFlags drawFlags)
	{
		drawFlags = (ImGuiWindowFlags.MenuBar | currentDevTool.drawFlags);
		if (this.nextImGuiWindowPosition.HasValue)
		{
			ValueTuple<Vector2, ImGuiCond> value = this.nextImGuiWindowPosition.Value;
			Vector2 item = value.Item1;
			ImGuiCond item2 = value.Item2;
			ImGui.SetNextWindowPos(item, item2);
			this.nextImGuiWindowPosition = default(Option<ValueTuple<Vector2, ImGuiCond>>);
		}
		if (this.nextImGuiWindowSize.HasValue)
		{
			Vector2 item3 = this.nextImGuiWindowSize.Value.Item1;
			ImGui.SetNextWindowSize(item3);
			this.nextImGuiWindowSize = default(Option<ValueTuple<Vector2, ImGuiCond>>);
		}
	}

	// Token: 0x06002107 RID: 8455 RVA: 0x000AFFF7 File Offset: 0x000AE1F7
	public void SetPosition(Vector2 position, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowPosition = new ValueTuple<Vector2, ImGuiCond>(position, condition);
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x000B000B File Offset: 0x000AE20B
	public void SetSize(Vector2 size, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowSize = new ValueTuple<Vector2, ImGuiCond>(size, condition);
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x000B001F File Offset: 0x000AE21F
	public void Close()
	{
		this.isRequestingToClose = true;
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x000B0028 File Offset: 0x000AE228
	public void Internal_Uninit()
	{
		foreach (DevTool devTool in this.devTools)
		{
			devTool.Internal_Uninit();
		}
	}

	// Token: 0x0400129E RID: 4766
	public readonly string uniquePanelId;

	// Token: 0x0400129F RID: 4767
	public readonly DevPanelList manager;

	// Token: 0x040012A0 RID: 4768
	public readonly Type initialDevToolType;

	// Token: 0x040012A1 RID: 4769
	public readonly uint idPostfixNumber;

	// Token: 0x040012A2 RID: 4770
	private List<DevTool> devTools;

	// Token: 0x040012A3 RID: 4771
	private int currentDevToolIndex;
}
