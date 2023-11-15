using System;
using UnityEngine;

// Token: 0x0200080F RID: 2063
public class ClearTool : DragTool
{
	// Token: 0x06003B51 RID: 15185 RVA: 0x00149CE9 File Offset: 0x00147EE9
	public static void DestroyInstance()
	{
		ClearTool.Instance = null;
	}

	// Token: 0x06003B52 RID: 15186 RVA: 0x00149CF1 File Offset: 0x00147EF1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClearTool.Instance = this;
		this.interceptNumberKeysForPriority = true;
	}

	// Token: 0x06003B53 RID: 15187 RVA: 0x00149D06 File Offset: 0x00147F06
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003B54 RID: 15188 RVA: 0x00149D14 File Offset: 0x00147F14
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		GameObject gameObject = Grid.Objects[cell, 3];
		if (gameObject == null)
		{
			return;
		}
		ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
		while (objectLayerListItem != null)
		{
			GameObject gameObject2 = objectLayerListItem.gameObject;
			objectLayerListItem = objectLayerListItem.nextItem;
			if (!(gameObject2 == null) && !(gameObject2.GetComponent<MinionIdentity>() != null) && gameObject2.GetComponent<Clearable>().isClearable)
			{
				gameObject2.GetComponent<Clearable>().MarkForClear(false, false);
				Prioritizable component = gameObject2.GetComponent<Prioritizable>();
				if (component != null)
				{
					component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
				}
			}
		}
	}

	// Token: 0x06003B55 RID: 15189 RVA: 0x00149DAD File Offset: 0x00147FAD
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06003B56 RID: 15190 RVA: 0x00149DC5 File Offset: 0x00147FC5
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x0400272E RID: 10030
	public static ClearTool Instance;
}
