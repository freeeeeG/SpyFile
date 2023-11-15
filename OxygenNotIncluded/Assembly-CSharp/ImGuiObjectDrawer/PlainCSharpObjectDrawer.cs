using System;
using ImGuiNET;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CDB RID: 3291
	public class PlainCSharpObjectDrawer : MemberDrawer
	{
		// Token: 0x0600691A RID: 26906 RVA: 0x00279BEF File Offset: 0x00277DEF
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return true;
		}

		// Token: 0x0600691B RID: 26907 RVA: 0x00279BF2 File Offset: 0x00277DF2
		public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			return MemberDrawType.Custom;
		}

		// Token: 0x0600691C RID: 26908 RVA: 0x00279BF5 File Offset: 0x00277DF5
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600691D RID: 26909 RVA: 0x00279BFC File Offset: 0x00277DFC
		protected override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				this.DrawContents(context, member, depth);
				ImGui.TreePop();
			}
		}

		// Token: 0x0600691E RID: 26910 RVA: 0x00279C43 File Offset: 0x00277E43
		protected virtual void DrawContents(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			DrawerUtil.DrawObjectContents(member.value, context, depth + 1);
		}
	}
}
