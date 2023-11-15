using System;
using UnityEngine;

// Token: 0x02000813 RID: 2067
public class DeconstructTool : FilteredDragTool
{
	// Token: 0x06003B78 RID: 15224 RVA: 0x0014A8F5 File Offset: 0x00148AF5
	public static void DestroyInstance()
	{
		DeconstructTool.Instance = null;
	}

	// Token: 0x06003B79 RID: 15225 RVA: 0x0014A8FD File Offset: 0x00148AFD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DeconstructTool.Instance = this;
	}

	// Token: 0x06003B7A RID: 15226 RVA: 0x0014A90B File Offset: 0x00148B0B
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003B7B RID: 15227 RVA: 0x0014A918 File Offset: 0x00148B18
	protected override string GetConfirmSound()
	{
		return "Tile_Confirm_NegativeTool";
	}

	// Token: 0x06003B7C RID: 15228 RVA: 0x0014A91F File Offset: 0x00148B1F
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x0014A926 File Offset: 0x00148B26
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		this.DeconstructCell(cell);
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x0014A930 File Offset: 0x00148B30
	public void DeconstructCell(int cell)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
				if (base.IsActiveLayer(filterLayerFromGameObject))
				{
					gameObject.Trigger(-790448070, null);
					Prioritizable component = gameObject.GetComponent<Prioritizable>();
					if (component != null)
					{
						component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x0014A9A2 File Offset: 0x00148BA2
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06003B80 RID: 15232 RVA: 0x0014A9BA File Offset: 0x00148BBA
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04002739 RID: 10041
	public static DeconstructTool Instance;
}
