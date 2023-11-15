using System;
using UnityEngine;

// Token: 0x02000809 RID: 2057
public class AttackTool : DragTool
{
	// Token: 0x06003AEE RID: 15086 RVA: 0x001473A8 File Offset: 0x001455A8
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		AttackTool.MarkForAttack(regularizedPos, regularizedPos2, true);
	}

	// Token: 0x06003AEF RID: 15087 RVA: 0x001473F0 File Offset: 0x001455F0
	public static void MarkForAttack(Vector2 min, Vector2 max, bool mark)
	{
		foreach (FactionAlignment factionAlignment in Components.FactionAlignments.Items)
		{
			Vector2 vector = Grid.PosToXY(factionAlignment.transform.GetPosition());
			if (vector.x >= min.x && vector.x < max.x && vector.y >= min.y && vector.y < max.y)
			{
				if (mark)
				{
					if (FactionManager.Instance.GetDisposition(FactionManager.FactionID.Duplicant, factionAlignment.Alignment) != FactionManager.Disposition.Assist)
					{
						factionAlignment.SetPlayerTargeted(true);
						Prioritizable component = factionAlignment.GetComponent<Prioritizable>();
						if (component != null)
						{
							component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
						}
					}
				}
				else
				{
					factionAlignment.gameObject.Trigger(2127324410, null);
				}
			}
		}
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x001474EC File Offset: 0x001456EC
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x00147504 File Offset: 0x00145704
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}
}
