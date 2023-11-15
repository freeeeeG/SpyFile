using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200080B RID: 2059
public class BrushTool : InterfaceTool
{
	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x06003B09 RID: 15113 RVA: 0x0014853D File Offset: 0x0014673D
	public bool Dragging
	{
		get
		{
			return this.dragging;
		}
	}

	// Token: 0x06003B0A RID: 15114 RVA: 0x00148545 File Offset: 0x00146745
	protected virtual void PlaySound()
	{
	}

	// Token: 0x06003B0B RID: 15115 RVA: 0x00148547 File Offset: 0x00146747
	protected virtual void clearVisitedCells()
	{
		this.visitedCells.Clear();
	}

	// Token: 0x06003B0C RID: 15116 RVA: 0x00148554 File Offset: 0x00146754
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.dragging = false;
	}

	// Token: 0x06003B0D RID: 15117 RVA: 0x00148564 File Offset: 0x00146764
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06003B0E RID: 15118 RVA: 0x001485CC File Offset: 0x001467CC
	public virtual void SetBrushSize(int radius)
	{
		if (radius == this.brushRadius)
		{
			return;
		}
		this.brushRadius = radius;
		this.brushOffsets.Clear();
		for (int i = 0; i < this.brushRadius * 2; i++)
		{
			for (int j = 0; j < this.brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), new Vector2((float)this.brushRadius, (float)this.brushRadius)) < (float)this.brushRadius - 0.8f)
				{
					this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
				}
			}
		}
	}

	// Token: 0x06003B0F RID: 15119 RVA: 0x0014866D File Offset: 0x0014686D
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06003B10 RID: 15120 RVA: 0x00148690 File Offset: 0x00146890
	protected override void OnPrefabInit()
	{
		Game.Instance.Subscribe(1634669191, new Action<object>(this.OnTutorialOpened));
		base.OnPrefabInit();
		if (this.visualizer != null)
		{
			this.visualizer = global::Util.KInstantiate(this.visualizer, null, null);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer = global::Util.KInstantiate(this.areaVisualizer, null, null);
			this.areaVisualizer.SetActive(false);
			this.areaVisualizer.GetComponent<RectTransform>().SetParent(base.transform);
			this.areaVisualizer.GetComponent<Renderer>().material.color = this.areaColour;
		}
	}

	// Token: 0x06003B11 RID: 15121 RVA: 0x00148743 File Offset: 0x00146943
	protected override void OnCmpEnable()
	{
		this.dragging = false;
	}

	// Token: 0x06003B12 RID: 15122 RVA: 0x0014874C File Offset: 0x0014694C
	protected override void OnCmpDisable()
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(false);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
		}
	}

	// Token: 0x06003B13 RID: 15123 RVA: 0x00148782 File Offset: 0x00146982
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		cursor_pos -= this.placementPivot;
		this.dragging = true;
		this.downPos = cursor_pos;
		if (!KInputManager.currentControllerIsGamepad)
		{
			KScreenManager.Instance.SetEventSystemEnabled(false);
		}
		else
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(true, null);
		}
		this.Paint();
	}

	// Token: 0x06003B14 RID: 15124 RVA: 0x001487C4 File Offset: 0x001469C4
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		cursor_pos -= this.placementPivot;
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		BrushTool.DragAxis dragAxis = this.dragAxis;
		if (dragAxis == BrushTool.DragAxis.Horizontal)
		{
			cursor_pos.y = this.downPos.y;
			this.dragAxis = BrushTool.DragAxis.None;
			return;
		}
		if (dragAxis != BrushTool.DragAxis.Vertical)
		{
			return;
		}
		cursor_pos.x = this.downPos.x;
		this.dragAxis = BrushTool.DragAxis.None;
	}

	// Token: 0x06003B15 RID: 15125 RVA: 0x0014884C File Offset: 0x00146A4C
	protected virtual string GetConfirmSound()
	{
		return "Tile_Confirm";
	}

	// Token: 0x06003B16 RID: 15126 RVA: 0x00148853 File Offset: 0x00146A53
	protected virtual string GetDragSound()
	{
		return "Tile_Drag";
	}

	// Token: 0x06003B17 RID: 15127 RVA: 0x0014885A File Offset: 0x00146A5A
	public override string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x06003B18 RID: 15128 RVA: 0x00148864 File Offset: 0x00146A64
	private static int GetGridDistance(int cell, int center_cell)
	{
		Vector2I u = Grid.CellToXY(cell);
		Vector2I v = Grid.CellToXY(center_cell);
		Vector2I vector2I = u - v;
		return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
	}

	// Token: 0x06003B19 RID: 15129 RVA: 0x0014889C File Offset: 0x00146A9C
	private void Paint()
	{
		int count = this.visitedCells.Count;
		foreach (int num in this.cellsInRadius)
		{
			if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId && (!Grid.Foundation[num] || this.affectFoundation))
			{
				this.OnPaintCell(num, Grid.GetCellDistance(this.currentCell, num));
			}
		}
		if (this.lastCell != this.currentCell)
		{
			this.PlayDragSound();
		}
		if (count < this.visitedCells.Count)
		{
			this.PlaySound();
		}
	}

	// Token: 0x06003B1A RID: 15130 RVA: 0x00148960 File Offset: 0x00146B60
	protected virtual void PlayDragSound()
	{
		string dragSound = this.GetDragSound();
		if (!string.IsNullOrEmpty(dragSound))
		{
			string sound = GlobalAssets.GetSound(dragSound, false);
			if (sound != null)
			{
				Vector3 pos = Grid.CellToPos(this.currentCell);
				pos.z = 0f;
				int cellDistance = Grid.GetCellDistance(Grid.PosToCell(this.downPos), this.currentCell);
				EventInstance instance = SoundEvent.BeginOneShot(sound, pos, 1f, false);
				instance.setParameterByName("tileCount", (float)cellDistance, false);
				SoundEvent.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06003B1B RID: 15131 RVA: 0x001489E0 File Offset: 0x00146BE0
	public override void OnMouseMove(Vector3 cursorPos)
	{
		int num = Grid.PosToCell(cursorPos);
		this.currentCell = num;
		base.OnMouseMove(cursorPos);
		this.cellsInRadius.Clear();
		foreach (Vector2 vector in this.brushOffsets)
		{
			int num2 = Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y));
			if (Grid.IsValidCell(num2) && (int)Grid.WorldIdx[num2] == ClusterManager.Instance.activeWorldId)
			{
				this.cellsInRadius.Add(Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y)));
			}
		}
		if (!this.dragging)
		{
			return;
		}
		this.Paint();
		this.lastCell = this.currentCell;
	}

	// Token: 0x06003B1C RID: 15132 RVA: 0x00148AD0 File Offset: 0x00146CD0
	protected virtual void OnPaintCell(int cell, int distFromOrigin)
	{
		if (!this.visitedCells.Contains(cell))
		{
			this.visitedCells.Add(cell);
		}
	}

	// Token: 0x06003B1D RID: 15133 RVA: 0x00148AEC File Offset: 0x00146CEC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.DragStraight))
		{
			this.dragAxis = BrushTool.DragAxis.None;
		}
		else if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriortyKeysDown(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06003B1E RID: 15134 RVA: 0x00148B1F File Offset: 0x00146D1F
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.DragStraight))
		{
			this.dragAxis = BrushTool.DragAxis.Invalid;
		}
		else if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriorityKeysUp(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06003B1F RID: 15135 RVA: 0x00148B54 File Offset: 0x00146D54
	private void HandlePriortyKeysDown(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 > action || action > global::Action.Plan10 || !e.TryConsume(action))
		{
			return;
		}
		int num = action - global::Action.Plan1 + 1;
		if (num <= 9)
		{
			ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, num), true);
			return;
		}
		ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1), true);
	}

	// Token: 0x06003B20 RID: 15136 RVA: 0x00148BB8 File Offset: 0x00146DB8
	private void HandlePriorityKeysUp(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 <= action && action <= global::Action.Plan10)
		{
			e.TryConsume(action);
		}
	}

	// Token: 0x06003B21 RID: 15137 RVA: 0x00148BDE File Offset: 0x00146DDE
	public override void OnFocus(bool focus)
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = focus;
		base.OnFocus(focus);
	}

	// Token: 0x06003B22 RID: 15138 RVA: 0x00148C08 File Offset: 0x00146E08
	private void OnTutorialOpened(object data)
	{
		this.dragging = false;
	}

	// Token: 0x06003B23 RID: 15139 RVA: 0x00148C11 File Offset: 0x00146E11
	public override bool ShowHoverUI()
	{
		return this.dragging || base.ShowHoverUI();
	}

	// Token: 0x06003B24 RID: 15140 RVA: 0x00148C23 File Offset: 0x00146E23
	public override void LateUpdate()
	{
		base.LateUpdate();
	}

	// Token: 0x04002711 RID: 10001
	[SerializeField]
	private Texture2D brushCursor;

	// Token: 0x04002712 RID: 10002
	[SerializeField]
	private GameObject areaVisualizer;

	// Token: 0x04002713 RID: 10003
	[SerializeField]
	private Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04002714 RID: 10004
	protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x04002715 RID: 10005
	protected Vector3 placementPivot;

	// Token: 0x04002716 RID: 10006
	protected bool interceptNumberKeysForPriority;

	// Token: 0x04002717 RID: 10007
	protected List<Vector2> brushOffsets = new List<Vector2>();

	// Token: 0x04002718 RID: 10008
	protected bool affectFoundation;

	// Token: 0x04002719 RID: 10009
	private bool dragging;

	// Token: 0x0400271A RID: 10010
	protected int brushRadius = -1;

	// Token: 0x0400271B RID: 10011
	private BrushTool.DragAxis dragAxis = BrushTool.DragAxis.Invalid;

	// Token: 0x0400271C RID: 10012
	protected Vector3 downPos;

	// Token: 0x0400271D RID: 10013
	protected int currentCell;

	// Token: 0x0400271E RID: 10014
	protected int lastCell;

	// Token: 0x0400271F RID: 10015
	protected List<int> visitedCells = new List<int>();

	// Token: 0x04002720 RID: 10016
	protected HashSet<int> cellsInRadius = new HashSet<int>();

	// Token: 0x020015E7 RID: 5607
	private enum DragAxis
	{
		// Token: 0x040069DE RID: 27102
		Invalid = -1,
		// Token: 0x040069DF RID: 27103
		None,
		// Token: 0x040069E0 RID: 27104
		Horizontal,
		// Token: 0x040069E1 RID: 27105
		Vertical
	}
}
