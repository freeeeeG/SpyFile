using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000818 RID: 2072
public class EmptyPipeTool : FilteredDragTool
{
	// Token: 0x06003BC1 RID: 15297 RVA: 0x0014BEFC File Offset: 0x0014A0FC
	public static void DestroyInstance()
	{
		EmptyPipeTool.Instance = null;
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x0014BF04 File Offset: 0x0014A104
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		EmptyPipeTool.Instance = this;
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x0014BF14 File Offset: 0x0014A114
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			if (base.IsActiveLayer((ObjectLayer)i))
			{
				GameObject gameObject = Grid.Objects[cell, i];
				if (!(gameObject == null))
				{
					IEmptyConduitWorkable component = gameObject.GetComponent<IEmptyConduitWorkable>();
					if (!component.IsNullOrDestroyed())
					{
						if (DebugHandler.InstantBuildMode)
						{
							component.EmptyContents();
						}
						else
						{
							component.MarkForEmptying();
							Prioritizable component2 = gameObject.GetComponent<Prioritizable>();
							if (component2 != null)
							{
								component2.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x0014BF96 File Offset: 0x0014A196
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06003BC5 RID: 15301 RVA: 0x0014BFAE File Offset: 0x0014A1AE
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x06003BC6 RID: 15302 RVA: 0x0014BFC7 File Offset: 0x0014A1C7
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x04002754 RID: 10068
	public static EmptyPipeTool Instance;
}
