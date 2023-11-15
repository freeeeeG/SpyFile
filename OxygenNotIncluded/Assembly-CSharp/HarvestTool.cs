using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200081B RID: 2075
public class HarvestTool : DragTool
{
	// Token: 0x06003BDB RID: 15323 RVA: 0x0014C690 File Offset: 0x0014A890
	public static void DestroyInstance()
	{
		HarvestTool.Instance = null;
	}

	// Token: 0x06003BDC RID: 15324 RVA: 0x0014C698 File Offset: 0x0014A898
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		HarvestTool.Instance = this;
		this.options.Add("HARVEST_WHEN_READY", ToolParameterMenu.ToggleState.On);
		this.options.Add("DO_NOT_HARVEST", ToolParameterMenu.ToggleState.Off);
		this.viewMode = OverlayModes.Harvest.ID;
	}

	// Token: 0x06003BDD RID: 15325 RVA: 0x0014C6D4 File Offset: 0x0014A8D4
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			foreach (HarvestDesignatable harvestDesignatable in Components.HarvestDesignatables.Items)
			{
				OccupyArea area = harvestDesignatable.area;
				if (Grid.PosToCell(harvestDesignatable) == cell || (area != null && area.CheckIsOccupying(cell)))
				{
					if (this.options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On)
					{
						harvestDesignatable.SetHarvestWhenReady(true);
					}
					else if (this.options["DO_NOT_HARVEST"] == ToolParameterMenu.ToggleState.On)
					{
						harvestDesignatable.SetHarvestWhenReady(false);
					}
					Prioritizable component = harvestDesignatable.GetComponent<Prioritizable>();
					if (component != null)
					{
						component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x0014C7B0 File Offset: 0x0014A9B0
	public void Update()
	{
		MeshRenderer componentInChildren = this.visualizer.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null)
		{
			if (this.options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On)
			{
				componentInChildren.material.mainTexture = this.visualizerTextures[0];
				return;
			}
			if (this.options["DO_NOT_HARVEST"] == ToolParameterMenu.ToggleState.On)
			{
				componentInChildren.material.mainTexture = this.visualizerTextures[1];
			}
		}
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x0014C81D File Offset: 0x0014AA1D
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x0014C826 File Offset: 0x0014AA26
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
		ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.options);
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x0014C853 File Offset: 0x0014AA53
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
		ToolMenu.Instance.toolParameterMenu.ClearMenu();
	}

	// Token: 0x0400275D RID: 10077
	public GameObject Placer;

	// Token: 0x0400275E RID: 10078
	public static HarvestTool Instance;

	// Token: 0x0400275F RID: 10079
	public Texture2D[] visualizerTextures;

	// Token: 0x04002760 RID: 10080
	private Dictionary<string, ToolParameterMenu.ToggleState> options = new Dictionary<string, ToolParameterMenu.ToggleState>();
}
