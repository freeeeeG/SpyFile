using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200081D RID: 2077
public class MopTool : DragTool
{
	// Token: 0x06003C0C RID: 15372 RVA: 0x0014D6EF File Offset: 0x0014B8EF
	public static void DestroyInstance()
	{
		MopTool.Instance = null;
	}

	// Token: 0x06003C0D RID: 15373 RVA: 0x0014D6F7 File Offset: 0x0014B8F7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Placer = Assets.GetPrefab(new Tag("MopPlacer"));
		this.interceptNumberKeysForPriority = true;
		MopTool.Instance = this;
	}

	// Token: 0x06003C0E RID: 15374 RVA: 0x0014D721 File Offset: 0x0014B921
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C0F RID: 15375 RVA: 0x0014D730 File Offset: 0x0014B930
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			if (DebugHandler.InstantBuildMode)
			{
				if (Grid.IsValidCell(cell))
				{
					Moppable.MopCell(cell, 1000000f, null);
					return;
				}
			}
			else
			{
				GameObject gameObject = Grid.Objects[cell, 8];
				if (!Grid.Solid[cell] && gameObject == null && Grid.Element[cell].IsLiquid)
				{
					bool flag = Grid.Solid[Grid.CellBelow(cell)];
					bool flag2 = Grid.Mass[cell] <= MopTool.maxMopAmt;
					if (flag && flag2)
					{
						gameObject = Util.KInstantiate(this.Placer, null, null);
						Grid.Objects[cell, 8] = gameObject;
						Vector3 vector = Grid.CellToPosCBC(cell, this.visualizerLayer);
						float num = -0.15f;
						vector.z += num;
						gameObject.transform.SetPosition(vector);
						gameObject.SetActive(true);
					}
					else
					{
						string text = UI.TOOLS.MOP.TOO_MUCH_LIQUID;
						if (!flag)
						{
							text = UI.TOOLS.MOP.NOT_ON_FLOOR;
						}
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, text, null, Grid.CellToPosCBC(cell, this.visualizerLayer), 1.5f, false, false);
					}
				}
				if (gameObject != null)
				{
					Prioritizable component = gameObject.GetComponent<Prioritizable>();
					if (component != null)
					{
						component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x06003C10 RID: 15376 RVA: 0x0014D899 File Offset: 0x0014BA99
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06003C11 RID: 15377 RVA: 0x0014D8B1 File Offset: 0x0014BAB1
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x0400277D RID: 10109
	private GameObject Placer;

	// Token: 0x0400277E RID: 10110
	public static MopTool Instance;

	// Token: 0x0400277F RID: 10111
	private SimHashes Element;

	// Token: 0x04002780 RID: 10112
	public static float maxMopAmt = 150f;
}
