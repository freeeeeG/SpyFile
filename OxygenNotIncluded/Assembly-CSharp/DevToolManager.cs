using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ImGuiNET;
using STRINGS;
using UnityEngine;

// Token: 0x0200055D RID: 1373
public class DevToolManager
{
	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06002173 RID: 8563 RVA: 0x000B520D File Offset: 0x000B340D
	public bool Show
	{
		get
		{
			return this.showImGui;
		}
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x000B5218 File Offset: 0x000B3418
	public DevToolManager()
	{
		DevToolManager.Instance = this;
		this.RegisterDevTool<DevToolSimDebug>("Debuggers/Sim Debug");
		this.RegisterDevTool<DevToolStateMachineDebug>("Debuggers/State Machine");
		this.RegisterDevTool<DevToolSaveGameInfo>("Debuggers/Save Game Info");
		this.RegisterDevTool<DevToolPrintingPodDebug>("Debuggers/Printing Pod Debug");
		this.RegisterDevTool<DevToolBigBaseMutations>("Debuggers/Big Base Mutation Utilities");
		this.RegisterDevTool<DevToolNavGrid>("Debuggers/Nav Grid");
		this.RegisterDevTool<DevToolResearchDebugger>("Debuggers/Research");
		this.RegisterDevTool<DevToolStatusItems>("Debuggers/StatusItems");
		this.RegisterDevTool<DevToolUI>("Debuggers/UI");
		this.RegisterDevTool<DevToolUnlockedIds>("Debuggers/UnlockedIds List");
		this.RegisterDevTool<DevToolStringsTable>("Debuggers/StringsTable");
		this.RegisterDevTool<DevToolChoreDebugger>("Debuggers/Chore");
		this.RegisterDevTool<DevToolBatchedAnimDebug>("Debuggers/Batched Anim");
		this.RegisterDevTool<DevTool_StoryTraits_Reveal>("Debuggers/Story Traits Reveal");
		this.RegisterDevTool<DevTool_StoryTrait_CritterManipulator>("Debuggers/Story Trait - Critter Manipulator");
		this.RegisterDevTool<DevToolAnimEventManager>("Debuggers/Anim Event Manager");
		this.RegisterDevTool<DevToolSceneBrowser>("Scene/Browser");
		this.RegisterDevTool<DevToolSceneInspector>("Scene/Inspector");
		this.menuNodes.AddAction("Help/" + UI.FRONTEND.DEVTOOLS.TITLE.text, delegate
		{
			this.warning.ShouldDrawWindow = true;
		});
		this.RegisterDevTool<DevToolCommandPalette>("Help/Command Palette");
		this.RegisterAdditionalDevToolsByReflection();
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x000B5376 File Offset: 0x000B3576
	public void Init()
	{
		this.UserAcceptedWarning = (KPlayerPrefs.GetInt("ShowDevtools", 0) == 1);
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x000B538C File Offset: 0x000B358C
	private void RegisterDevTool<T>(string location) where T : DevTool, new()
	{
		this.menuNodes.AddAction(location, delegate
		{
			this.panels.AddPanelFor<T>();
		});
		this.dontAutomaticallyRegisterTypes.Add(typeof(T));
		this.devToolNameDict[typeof(T)] = Path.GetFileName(location);
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x000B53E4 File Offset: 0x000B35E4
	private void RegisterAdditionalDevToolsByReflection()
	{
		using (List<Type>.Enumerator enumerator = ReflectionUtil.CollectTypesThatInheritOrImplement<DevTool>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Type type = enumerator.Current;
				if (!type.IsAbstract && !this.dontAutomaticallyRegisterTypes.Contains(type) && ReflectionUtil.HasDefaultConstructor(type))
				{
					this.menuNodes.AddAction("Debuggers/" + DevToolUtil.GenerateDevToolName(type), delegate
					{
						this.panels.AddPanelFor((DevTool)Activator.CreateInstance(type));
					});
				}
			}
		}
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x000B54A0 File Offset: 0x000B36A0
	public void UpdateShouldShowTools()
	{
		if (!DebugHandler.enabled)
		{
			this.showImGui = false;
			return;
		}
		bool flag = Input.GetKeyDown(KeyCode.BackQuote) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl));
		if (!this.toggleKeyWasDown && flag)
		{
			this.showImGui = !this.showImGui;
		}
		this.toggleKeyWasDown = flag;
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x000B5508 File Offset: 0x000B3708
	public void UpdateTools()
	{
		if (!DebugHandler.enabled)
		{
			return;
		}
		if (this.showImGui)
		{
			if (this.warning.ShouldDrawWindow)
			{
				this.warning.DrawWindow(out this.warning.ShouldDrawWindow);
			}
			if (!this.UserAcceptedWarning)
			{
				this.warning.DrawMenuBar();
			}
			else
			{
				this.DrawMenu();
				this.panels.Render();
				if (this.showImguiState)
				{
					if (ImGui.Begin("ImGui state", ref this.showImguiState))
					{
						ImGui.Checkbox("ImGui.GetIO().WantCaptureMouse", ImGui.GetIO().WantCaptureMouse);
						ImGui.Checkbox("ImGui.GetIO().WantCaptureKeyboard", ImGui.GetIO().WantCaptureKeyboard);
					}
					ImGui.End();
				}
				if (this.showImguiDemo)
				{
					ImGui.ShowDemoWindow(ref this.showImguiDemo);
				}
			}
		}
		this.UpdateConsumingGameInputs();
		this.UpdateShortcuts();
	}

	// Token: 0x0600217A RID: 8570 RVA: 0x000B55DF File Offset: 0x000B37DF
	private void UpdateShortcuts()
	{
		if (this.showImGui && this.UserAcceptedWarning)
		{
			this.<UpdateShortcuts>g__DoUpdate|24_0();
		}
	}

	// Token: 0x0600217B RID: 8571 RVA: 0x000B55F8 File Offset: 0x000B37F8
	private void DrawMenu()
	{
		this.menuFontSize.InitializeIfNeeded();
		if (ImGui.BeginMainMenuBar())
		{
			this.menuNodes.Draw();
			this.menuFontSize.DrawMenu();
			if (ImGui.BeginMenu("IMGUI"))
			{
				ImGui.Checkbox("ImGui state", ref this.showImguiState);
				ImGui.Checkbox("ImGui Demo", ref this.showImguiDemo);
				ImGui.EndMenu();
			}
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x000B5668 File Offset: 0x000B3868
	private unsafe void UpdateConsumingGameInputs()
	{
		this.doesImGuiWantInput = false;
		if (this.showImGui)
		{
			this.doesImGuiWantInput = (*ImGui.GetIO().WantCaptureMouse || *ImGui.GetIO().WantCaptureKeyboard);
			if (!this.prevDoesImGuiWantInput && this.doesImGuiWantInput)
			{
				DevToolManager.<UpdateConsumingGameInputs>g__OnInputEnterImGui|26_0();
			}
			if (this.prevDoesImGuiWantInput && !this.doesImGuiWantInput)
			{
				DevToolManager.<UpdateConsumingGameInputs>g__OnInputExitImGui|26_1();
			}
		}
		if (this.prevShowImGui && this.prevDoesImGuiWantInput && !this.showImGui)
		{
			DevToolManager.<UpdateConsumingGameInputs>g__OnInputExitImGui|26_1();
		}
		this.prevShowImGui = this.showImGui;
		this.prevDoesImGuiWantInput = this.doesImGuiWantInput;
		KInputManager.devToolFocus = (this.showImGui && this.doesImGuiWantInput);
	}

	// Token: 0x0600217F RID: 8575 RVA: 0x000B573C File Offset: 0x000B393C
	[CompilerGenerated]
	private void <UpdateShortcuts>g__DoUpdate|24_0()
	{
		if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Space))
		{
			DevToolCommandPalette.Init();
			this.showImGui = true;
		}
		if (Input.GetKeyDown(KeyCode.Comma))
		{
			DevToolUI.PingHoveredObject();
			this.showImGui = true;
		}
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x000B578C File Offset: 0x000B398C
	[CompilerGenerated]
	internal static void <UpdateConsumingGameInputs>g__OnInputEnterImGui|26_0()
	{
		UnityMouseCatcherUI.SetEnabled(true);
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.GetControllerCount(); i++)
		{
			inputManager.GetController(i).HandleCancelInput();
		}
	}

	// Token: 0x06002181 RID: 8577 RVA: 0x000B57C2 File Offset: 0x000B39C2
	[CompilerGenerated]
	internal static void <UpdateConsumingGameInputs>g__OnInputExitImGui|26_1()
	{
		UnityMouseCatcherUI.SetEnabled(false);
	}

	// Token: 0x040012EE RID: 4846
	public const string SHOW_DEVTOOLS = "ShowDevtools";

	// Token: 0x040012EF RID: 4847
	public static DevToolManager Instance;

	// Token: 0x040012F0 RID: 4848
	private bool toggleKeyWasDown;

	// Token: 0x040012F1 RID: 4849
	private bool showImGui;

	// Token: 0x040012F2 RID: 4850
	private bool prevShowImGui;

	// Token: 0x040012F3 RID: 4851
	private bool doesImGuiWantInput;

	// Token: 0x040012F4 RID: 4852
	private bool prevDoesImGuiWantInput;

	// Token: 0x040012F5 RID: 4853
	private bool showImguiState;

	// Token: 0x040012F6 RID: 4854
	private bool showImguiDemo;

	// Token: 0x040012F7 RID: 4855
	public bool UserAcceptedWarning;

	// Token: 0x040012F8 RID: 4856
	private DevToolWarning warning = new DevToolWarning();

	// Token: 0x040012F9 RID: 4857
	private DevToolMenuFontSize menuFontSize = new DevToolMenuFontSize();

	// Token: 0x040012FA RID: 4858
	public DevPanelList panels = new DevPanelList();

	// Token: 0x040012FB RID: 4859
	public DevToolMenuNodeList menuNodes = new DevToolMenuNodeList();

	// Token: 0x040012FC RID: 4860
	public Dictionary<Type, string> devToolNameDict = new Dictionary<Type, string>();

	// Token: 0x040012FD RID: 4861
	private HashSet<Type> dontAutomaticallyRegisterTypes = new HashSet<Type>();
}
