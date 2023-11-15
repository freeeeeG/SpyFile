using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000823 RID: 2083
public class SandboxClearFloorTool : BrushTool
{
	// Token: 0x06003C4F RID: 15439 RVA: 0x0014E809 File Offset: 0x0014CA09
	public static void DestroyInstance()
	{
		SandboxClearFloorTool.instance = null;
	}

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06003C50 RID: 15440 RVA: 0x0014E811 File Offset: 0x0014CA11
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06003C51 RID: 15441 RVA: 0x0014E81D File Offset: 0x0014CA1D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxClearFloorTool.instance = this;
	}

	// Token: 0x06003C52 RID: 15442 RVA: 0x0014E82B File Offset: 0x0014CA2B
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x0014E832 File Offset: 0x0014CA32
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x0014E840 File Offset: 0x0014CA40
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.BrushSize"), true);
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x0014E8A3 File Offset: 0x0014CAA3
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06003C56 RID: 15446 RVA: 0x0014E8BC File Offset: 0x0014CABC
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x0014E924 File Offset: 0x0014CB24
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x0014E92D File Offset: 0x0014CB2D
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x0014E948 File Offset: 0x0014CB48
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		bool flag = false;
		using (List<Pickupable>.Enumerator enumerator = Components.Pickupables.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Pickupable pickup = enumerator.Current;
				if (!(pickup.storage != null) && Grid.PosToCell(pickup) == cell && Components.LiveMinionIdentities.Items.Find((MinionIdentity match) => match.gameObject == pickup.gameObject) == null)
				{
					if (!flag)
					{
						KFMOD.PlayOneShot(this.soundPath, pickup.gameObject.transform.GetPosition(), 1f);
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.SANDBOXTOOLS.CLEARFLOOR.DELETED, pickup.transform, 1.5f, false);
						flag = true;
					}
					Util.KDestroyGameObject(pickup.gameObject);
				}
			}
		}
	}

	// Token: 0x04002793 RID: 10131
	public static SandboxClearFloorTool instance;

	// Token: 0x04002794 RID: 10132
	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor", false);
}
