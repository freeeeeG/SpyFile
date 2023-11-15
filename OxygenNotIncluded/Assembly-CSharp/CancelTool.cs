using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200080D RID: 2061
public class CancelTool : FilteredDragTool
{
	// Token: 0x06003B44 RID: 15172 RVA: 0x00149A7B File Offset: 0x00147C7B
	public static void DestroyInstance()
	{
		CancelTool.Instance = null;
	}

	// Token: 0x06003B45 RID: 15173 RVA: 0x00149A83 File Offset: 0x00147C83
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CancelTool.Instance = this;
	}

	// Token: 0x06003B46 RID: 15174 RVA: 0x00149A91 File Offset: 0x00147C91
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		base.GetDefaultFilters(filters);
		filters.Add(ToolParameterMenu.FILTERLAYERS.CLEANANDCLEAR, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.DIGPLACER, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x06003B47 RID: 15175 RVA: 0x00149AB2 File Offset: 0x00147CB2
	protected override string GetConfirmSound()
	{
		return "Tile_Confirm_NegativeTool";
	}

	// Token: 0x06003B48 RID: 15176 RVA: 0x00149AB9 File Offset: 0x00147CB9
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06003B49 RID: 15177 RVA: 0x00149AC0 File Offset: 0x00147CC0
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
				if (base.IsActiveLayer(filterLayerFromGameObject))
				{
					gameObject.Trigger(2127324410, null);
				}
			}
		}
	}

	// Token: 0x06003B4A RID: 15178 RVA: 0x00149B10 File Offset: 0x00147D10
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		AttackTool.MarkForAttack(regularizedPos, regularizedPos2, false);
		CaptureTool.MarkForCapture(regularizedPos, regularizedPos2, false);
	}

	// Token: 0x0400272D RID: 10029
	public static CancelTool Instance;
}
