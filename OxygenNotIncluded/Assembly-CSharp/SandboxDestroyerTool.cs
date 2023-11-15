using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000825 RID: 2085
public class SandboxDestroyerTool : BrushTool
{
	// Token: 0x06003C66 RID: 15462 RVA: 0x0014EC8D File Offset: 0x0014CE8D
	public static void DestroyInstance()
	{
		SandboxDestroyerTool.instance = null;
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x06003C67 RID: 15463 RVA: 0x0014EC95 File Offset: 0x0014CE95
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06003C68 RID: 15464 RVA: 0x0014ECA1 File Offset: 0x0014CEA1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxDestroyerTool.instance = this;
		this.affectFoundation = true;
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x0014ECB6 File Offset: 0x0014CEB6
	protected override string GetDragSound()
	{
		return "SandboxTool_Delete_Add";
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x0014ECBD File Offset: 0x0014CEBD
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x0014ECCA File Offset: 0x0014CECA
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x0014ED01 File Offset: 0x0014CF01
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06003C6D RID: 15469 RVA: 0x0014ED1C File Offset: 0x0014CF1C
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

	// Token: 0x06003C6E RID: 15470 RVA: 0x0014EDD4 File Offset: 0x0014CFD4
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Delete", false));
	}

	// Token: 0x06003C6F RID: 15471 RVA: 0x0014EDED File Offset: 0x0014CFED
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06003C70 RID: 15472 RVA: 0x0014EDF8 File Offset: 0x0014CFF8
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.SandBoxTool, 0f, 0f, byte.MaxValue, 0, index);
		HashSetPool<GameObject, SandboxDestroyerTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxDestroyerTool>.Allocate();
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			if (Grid.PosToCell(pickupable) == cell)
			{
				pooledHashSet.Add(pickupable.gameObject);
			}
		}
		foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
		{
			if (Grid.PosToCell(buildingComplete) == cell)
			{
				pooledHashSet.Add(buildingComplete.gameObject);
			}
		}
		if (Grid.Objects[cell, 1] != null)
		{
			pooledHashSet.Add(Grid.Objects[cell, 1]);
		}
		foreach (Crop crop in Components.Crops.Items)
		{
			if (Grid.PosToCell(crop) == cell)
			{
				pooledHashSet.Add(crop.gameObject);
			}
		}
		foreach (Health health in Components.Health.Items)
		{
			if (Grid.PosToCell(health) == cell)
			{
				pooledHashSet.Add(health.gameObject);
			}
		}
		foreach (Comet comet in Components.Meteors.GetItems((int)Grid.WorldIdx[cell]))
		{
			if (!comet.IsNullOrDestroyed() && Grid.PosToCell(comet) == cell)
			{
				pooledHashSet.Add(comet.gameObject);
			}
		}
		foreach (GameObject original in pooledHashSet)
		{
			Util.KDestroyGameObject(original);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x04002797 RID: 10135
	public static SandboxDestroyerTool instance;

	// Token: 0x04002798 RID: 10136
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04002799 RID: 10137
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
}
