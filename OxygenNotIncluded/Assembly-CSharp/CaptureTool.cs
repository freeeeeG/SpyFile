using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200080E RID: 2062
public class CaptureTool : DragTool
{
	// Token: 0x06003B4C RID: 15180 RVA: 0x00149B68 File Offset: 0x00147D68
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		CaptureTool.MarkForCapture(regularizedPos, regularizedPos2, true);
	}

	// Token: 0x06003B4D RID: 15181 RVA: 0x00149BB0 File Offset: 0x00147DB0
	public static void MarkForCapture(Vector2 min, Vector2 max, bool mark)
	{
		foreach (Capturable capturable in Components.Capturables.Items)
		{
			Vector2 vector = Grid.PosToXY(capturable.transform.GetPosition());
			if (vector.x >= min.x && vector.x < max.x && vector.y >= min.y && vector.y < max.y)
			{
				if (capturable.allowCapture)
				{
					PrioritySetting lastSelectedPriority = ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority();
					capturable.MarkForCapture(mark, lastSelectedPriority, true);
				}
				else if (mark)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.TOOLS.CAPTURE.NOT_CAPTURABLE, null, capturable.transform.GetPosition(), 1.5f, false, false);
				}
			}
		}
	}

	// Token: 0x06003B4E RID: 15182 RVA: 0x00149CB0 File Offset: 0x00147EB0
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06003B4F RID: 15183 RVA: 0x00149CC8 File Offset: 0x00147EC8
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}
}
