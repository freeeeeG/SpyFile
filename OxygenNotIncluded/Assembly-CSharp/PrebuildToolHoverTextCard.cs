using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A79 RID: 2681
public class PrebuildToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06005163 RID: 20835 RVA: 0x001CEED8 File Offset: 0x001CD0D8
	public override void UpdateHoverElements(List<KSelectable> selected)
	{
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		if (!this.errorMessage.IsNullOrWhiteSpace())
		{
			bool flag = true;
			foreach (string text in this.errorMessage.Split(new char[]
			{
				'\n'
			}))
			{
				if (!flag)
				{
					hoverTextDrawer.NewLine(26);
				}
				hoverTextDrawer.DrawText(text.ToUpper(), this.HoverTextStyleSettings[flag ? 0 : 1]);
				flag = false;
			}
		}
		hoverTextDrawer.NewLine(26);
		if (KInputManager.currentControllerIsGamepad)
		{
			hoverTextDrawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseRight, false), 20);
		}
		else
		{
			hoverTextDrawer.DrawIcon(instance.GetSprite("icon_mouse_right"), 20);
		}
		hoverTextDrawer.DrawText(this.backStr, this.Styles_Instruction.Standard);
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04003570 RID: 13680
	public string errorMessage;

	// Token: 0x04003571 RID: 13681
	public BuildingDef currentDef;
}
