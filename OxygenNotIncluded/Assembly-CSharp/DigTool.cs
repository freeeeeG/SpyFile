using System;
using UnityEngine;

// Token: 0x02000814 RID: 2068
public class DigTool : DragTool
{
	// Token: 0x06003B82 RID: 15234 RVA: 0x0014A9DB File Offset: 0x00148BDB
	public static void DestroyInstance()
	{
		DigTool.Instance = null;
	}

	// Token: 0x06003B83 RID: 15235 RVA: 0x0014A9E3 File Offset: 0x00148BE3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DigTool.Instance = this;
	}

	// Token: 0x06003B84 RID: 15236 RVA: 0x0014A9F1 File Offset: 0x00148BF1
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		InterfaceTool.ActiveConfig.DigAction.Uproot(cell);
		InterfaceTool.ActiveConfig.DigAction.Dig(cell, distFromOrigin);
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x0014AA14 File Offset: 0x00148C14
	public static GameObject PlaceDig(int cell, int animationDelay = 0)
	{
		if (Grid.Solid[cell] && !Grid.Foundation[cell] && Grid.Objects[cell, 7] == null)
		{
			for (int i = 0; i < 45; i++)
			{
				if (Grid.Objects[cell, i] != null && Grid.Objects[cell, i].GetComponent<Constructable>() != null)
				{
					return null;
				}
			}
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")), null, null);
			gameObject.SetActive(true);
			Grid.Objects[cell, 7] = gameObject;
			Vector3 vector = Grid.CellToPosCBC(cell, DigTool.Instance.visualizerLayer);
			float num = -0.15f;
			vector.z += num;
			gameObject.transform.SetPosition(vector);
			gameObject.GetComponentInChildren<EasingAnimations>().PlayAnimation("ScaleUp", Mathf.Max(0f, (float)animationDelay * 0.02f));
			return gameObject;
		}
		if (Grid.Objects[cell, 7] != null)
		{
			return Grid.Objects[cell, 7];
		}
		return null;
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x0014AB38 File Offset: 0x00148D38
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x0014AB50 File Offset: 0x00148D50
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x0400273A RID: 10042
	public static DigTool Instance;
}
