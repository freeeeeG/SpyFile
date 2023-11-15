using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000824 RID: 2084
public class SandboxCritterTool : BrushTool
{
	// Token: 0x06003C5B RID: 15451 RVA: 0x0014EA81 File Offset: 0x0014CC81
	public static void DestroyInstance()
	{
		SandboxCritterTool.instance = null;
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x0014EA89 File Offset: 0x0014CC89
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxCritterTool.instance = this;
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x0014EA97 File Offset: 0x0014CC97
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06003C5E RID: 15454 RVA: 0x0014EA9E File Offset: 0x0014CC9E
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C5F RID: 15455 RVA: 0x0014EAAB File Offset: 0x0014CCAB
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue(6f, true);
	}

	// Token: 0x06003C60 RID: 15456 RVA: 0x0014EAE2 File Offset: 0x0014CCE2
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x0014EAFC File Offset: 0x0014CCFC
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x0014EB64 File Offset: 0x0014CD64
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x0014EB6D File Offset: 0x0014CD6D
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06003C64 RID: 15460 RVA: 0x0014EB88 File Offset: 0x0014CD88
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		HashSetPool<GameObject, SandboxCritterTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxCritterTool>.Allocate();
		foreach (Health health in Components.Health.Items)
		{
			if (Grid.PosToCell(health) == cell && health.GetComponent<KPrefabID>().HasTag(GameTags.Creature))
			{
				pooledHashSet.Add(health.gameObject);
			}
		}
		foreach (GameObject gameObject in pooledHashSet)
		{
			KFMOD.PlayOneShot(this.soundPath, gameObject.gameObject.transform.GetPosition(), 1f);
			Util.KDestroyGameObject(gameObject);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x04002795 RID: 10133
	public static SandboxCritterTool instance;

	// Token: 0x04002796 RID: 10134
	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor", false);
}
