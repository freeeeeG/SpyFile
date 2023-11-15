using System;
using ImGuiNET;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CDA RID: 3290
	public class UnityObjectDrawer : PlainCSharpObjectDrawer
	{
		// Token: 0x06006917 RID: 26903 RVA: 0x00279B84 File Offset: 0x00277D84
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is UnityEngine.Object;
		}

		// Token: 0x06006918 RID: 26904 RVA: 0x00279B94 File Offset: 0x00277D94
		protected override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			UnityEngine.Object @object = (UnityEngine.Object)member.value;
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				base.DrawContents(context, member, depth);
				ImGui.TreePop();
			}
		}
	}
}
