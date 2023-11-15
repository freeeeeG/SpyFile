using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CCC RID: 3276
	public class NullDrawer : InlineDrawer
	{
		// Token: 0x060068E3 RID: 26851 RVA: 0x00279561 File Offset: 0x00277761
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}

		// Token: 0x060068E4 RID: 26852 RVA: 0x00279564 File Offset: 0x00277764
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value == null;
		}

		// Token: 0x060068E5 RID: 26853 RVA: 0x0027956F File Offset: 0x0027776F
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, "null");
		}
	}
}
