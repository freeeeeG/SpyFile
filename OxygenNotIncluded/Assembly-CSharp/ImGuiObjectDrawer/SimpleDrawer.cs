using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CCD RID: 3277
	public class SimpleDrawer : InlineDrawer
	{
		// Token: 0x060068E7 RID: 26855 RVA: 0x00279589 File Offset: 0x00277789
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}

		// Token: 0x060068E8 RID: 26856 RVA: 0x0027958C File Offset: 0x0027778C
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsPrimitive || member.CanAssignToType<string>();
		}

		// Token: 0x060068E9 RID: 26857 RVA: 0x002795A3 File Offset: 0x002777A3
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, member.value.ToString());
		}
	}
}
