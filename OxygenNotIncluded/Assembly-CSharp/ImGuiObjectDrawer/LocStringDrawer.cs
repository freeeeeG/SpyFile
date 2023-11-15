using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CCF RID: 3279
	public sealed class LocStringDrawer : InlineDrawer
	{
		// Token: 0x060068EE RID: 26862 RVA: 0x002795D1 File Offset: 0x002777D1
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<LocString>();
		}

		// Token: 0x060068EF RID: 26863 RVA: 0x002795D9 File Offset: 0x002777D9
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, string.Format("{0}({1})", member.value, ((LocString)member.value).text));
		}
	}
}
