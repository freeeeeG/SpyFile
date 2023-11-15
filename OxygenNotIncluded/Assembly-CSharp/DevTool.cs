using System;
using ImGuiNET;

// Token: 0x0200054C RID: 1356
public abstract class DevTool
{
	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06002114 RID: 8468 RVA: 0x000B0330 File Offset: 0x000AE530
	// (remove) Token: 0x06002115 RID: 8469 RVA: 0x000B0368 File Offset: 0x000AE568
	public event System.Action OnInit;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06002116 RID: 8470 RVA: 0x000B03A0 File Offset: 0x000AE5A0
	// (remove) Token: 0x06002117 RID: 8471 RVA: 0x000B03D8 File Offset: 0x000AE5D8
	public event System.Action OnUninit;

	// Token: 0x06002118 RID: 8472 RVA: 0x000B040D File Offset: 0x000AE60D
	public DevTool()
	{
		this.Name = DevToolUtil.GenerateDevToolName(this);
	}

	// Token: 0x06002119 RID: 8473 RVA: 0x000B0421 File Offset: 0x000AE621
	public void DoImGui(DevPanel panel)
	{
		if (this.RequiresGameRunning && Game.Instance == null)
		{
			ImGui.Text("Game must be loaded to use this devtool.");
			return;
		}
		this.RenderTo(panel);
	}

	// Token: 0x0600211A RID: 8474 RVA: 0x000B044A File Offset: 0x000AE64A
	public void ClosePanel()
	{
		this.isRequestingToClosePanel = true;
	}

	// Token: 0x0600211B RID: 8475
	protected abstract void RenderTo(DevPanel panel);

	// Token: 0x0600211C RID: 8476 RVA: 0x000B0453 File Offset: 0x000AE653
	public void Internal_TryInit()
	{
		if (this.didInit)
		{
			return;
		}
		this.didInit = true;
		if (this.OnInit != null)
		{
			this.OnInit();
		}
	}

	// Token: 0x0600211D RID: 8477 RVA: 0x000B0478 File Offset: 0x000AE678
	public void Internal_Uninit()
	{
		if (this.OnUninit != null)
		{
			this.OnUninit();
		}
	}

	// Token: 0x040012A6 RID: 4774
	public string Name;

	// Token: 0x040012A7 RID: 4775
	public bool RequiresGameRunning;

	// Token: 0x040012A8 RID: 4776
	public bool isRequestingToClosePanel;

	// Token: 0x040012A9 RID: 4777
	public ImGuiWindowFlags drawFlags;

	// Token: 0x040012AC RID: 4780
	private bool didInit;
}
