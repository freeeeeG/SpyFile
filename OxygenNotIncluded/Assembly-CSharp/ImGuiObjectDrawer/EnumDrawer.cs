using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CD0 RID: 3280
	public sealed class EnumDrawer : InlineDrawer
	{
		// Token: 0x060068F1 RID: 26865 RVA: 0x0027960E File Offset: 0x0027780E
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsEnum;
		}

		// Token: 0x060068F2 RID: 26866 RVA: 0x0027961B File Offset: 0x0027781B
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, member.value.ToString());
		}
	}
}
