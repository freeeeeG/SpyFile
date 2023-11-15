using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000826 RID: 2086
public class SandboxFOWTool : BrushTool
{
	// Token: 0x06003C72 RID: 15474 RVA: 0x0014F12A File Offset: 0x0014D32A
	public static void DestroyInstance()
	{
		SandboxFOWTool.instance = null;
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06003C73 RID: 15475 RVA: 0x0014F132 File Offset: 0x0014D332
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06003C74 RID: 15476 RVA: 0x0014F13E File Offset: 0x0014D33E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxFOWTool.instance = this;
	}

	// Token: 0x06003C75 RID: 15477 RVA: 0x0014F14C File Offset: 0x0014D34C
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06003C76 RID: 15478 RVA: 0x0014F153 File Offset: 0x0014D353
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C77 RID: 15479 RVA: 0x0014F160 File Offset: 0x0014D360
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	// Token: 0x06003C78 RID: 15480 RVA: 0x0014F197 File Offset: 0x0014D397
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.ev.release();
	}

	// Token: 0x06003C79 RID: 15481 RVA: 0x0014F1BC File Offset: 0x0014D3BC
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06003C7A RID: 15482 RVA: 0x0014F274 File Offset: 0x0014D474
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06003C7B RID: 15483 RVA: 0x0014F27D File Offset: 0x0014D47D
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		Grid.Reveal(cell, byte.MaxValue, true);
	}

	// Token: 0x06003C7C RID: 15484 RVA: 0x0014F294 File Offset: 0x0014D494
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		int intSetting = this.settings.GetIntSetting("SandboxTools.BrushSize");
		this.ev = KFMOD.CreateInstance(GlobalAssets.GetSound("SandboxTool_Reveal", false));
		this.ev.setParameterByName("BrushSize", (float)intSetting, false);
		this.ev.start();
	}

	// Token: 0x06003C7D RID: 15485 RVA: 0x0014F2EF File Offset: 0x0014D4EF
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
		this.ev.stop(STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x0400279A RID: 10138
	public static SandboxFOWTool instance;

	// Token: 0x0400279B RID: 10139
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x0400279C RID: 10140
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x0400279D RID: 10141
	private EventInstance ev;
}
