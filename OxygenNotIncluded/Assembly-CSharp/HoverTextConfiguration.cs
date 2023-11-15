using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A63 RID: 2659
[AddComponentMenu("KMonoBehaviour/scripts/HoverTextConfiguration")]
public class HoverTextConfiguration : KMonoBehaviour
{
	// Token: 0x06005066 RID: 20582 RVA: 0x001C7E07 File Offset: 0x001C6007
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ConfigureHoverScreen();
	}

	// Token: 0x06005067 RID: 20583 RVA: 0x001C7E15 File Offset: 0x001C6015
	protected virtual void ConfigureTitle(HoverTextScreen screen)
	{
		if (string.IsNullOrEmpty(this.ToolName))
		{
			this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
		}
	}

	// Token: 0x06005068 RID: 20584 RVA: 0x001C7E3F File Offset: 0x001C603F
	protected void DrawTitle(HoverTextScreen screen, HoverTextDrawer drawer)
	{
		drawer.DrawText(this.ToolName, this.ToolTitleTextStyle);
	}

	// Token: 0x06005069 RID: 20585 RVA: 0x001C7E54 File Offset: 0x001C6054
	protected void DrawInstructions(HoverTextScreen screen, HoverTextDrawer drawer)
	{
		TextStyleSetting standard = this.Styles_Instruction.Standard;
		drawer.NewLine(26);
		if (KInputManager.currentControllerIsGamepad)
		{
			drawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseLeft, false), 20);
		}
		else
		{
			drawer.DrawIcon(screen.GetSprite("icon_mouse_left"), 20);
		}
		drawer.DrawText(this.ActionName, standard);
		drawer.AddIndent(8);
		if (KInputManager.currentControllerIsGamepad)
		{
			drawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseRight, false), 20);
		}
		else
		{
			drawer.DrawIcon(screen.GetSprite("icon_mouse_right"), 20);
		}
		drawer.DrawText(this.backStr, standard);
	}

	// Token: 0x0600506A RID: 20586 RVA: 0x001C7EF8 File Offset: 0x001C60F8
	public virtual void ConfigureHoverScreen()
	{
		if (!string.IsNullOrEmpty(this.ActionStringKey))
		{
			this.ActionName = Strings.Get(this.ActionStringKey);
		}
		HoverTextScreen instance = HoverTextScreen.Instance;
		this.ConfigureTitle(instance);
		this.backStr = UI.TOOLS.GENERIC.BACK.ToString().ToUpper();
	}

	// Token: 0x0600506B RID: 20587 RVA: 0x001C7F4C File Offset: 0x001C614C
	public virtual void UpdateHoverElements(List<KSelectable> hover_objects)
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
		this.DrawTitle(instance, hoverTextDrawer);
		this.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x040034A0 RID: 13472
	public TextStyleSetting[] HoverTextStyleSettings;

	// Token: 0x040034A1 RID: 13473
	public string ToolNameStringKey = "";

	// Token: 0x040034A2 RID: 13474
	public string ActionStringKey = "";

	// Token: 0x040034A3 RID: 13475
	[HideInInspector]
	public string ActionName = "";

	// Token: 0x040034A4 RID: 13476
	[HideInInspector]
	public string ToolName;

	// Token: 0x040034A5 RID: 13477
	protected string backStr;

	// Token: 0x040034A6 RID: 13478
	public TextStyleSetting ToolTitleTextStyle;

	// Token: 0x040034A7 RID: 13479
	public HoverTextConfiguration.TextStylePair Styles_Title;

	// Token: 0x040034A8 RID: 13480
	public HoverTextConfiguration.TextStylePair Styles_BodyText;

	// Token: 0x040034A9 RID: 13481
	public HoverTextConfiguration.TextStylePair Styles_Instruction;

	// Token: 0x040034AA RID: 13482
	public HoverTextConfiguration.TextStylePair Styles_Warning;

	// Token: 0x040034AB RID: 13483
	public HoverTextConfiguration.ValuePropertyTextStyles Styles_Values;

	// Token: 0x020018F6 RID: 6390
	[Serializable]
	public struct TextStylePair
	{
		// Token: 0x0400738D RID: 29581
		public TextStyleSetting Standard;

		// Token: 0x0400738E RID: 29582
		public TextStyleSetting Selected;
	}

	// Token: 0x020018F7 RID: 6391
	[Serializable]
	public struct ValuePropertyTextStyles
	{
		// Token: 0x0400738F RID: 29583
		public HoverTextConfiguration.TextStylePair Property;

		// Token: 0x04007390 RID: 29584
		public HoverTextConfiguration.TextStylePair Property_Decimal;

		// Token: 0x04007391 RID: 29585
		public HoverTextConfiguration.TextStylePair Property_Unit;
	}
}
