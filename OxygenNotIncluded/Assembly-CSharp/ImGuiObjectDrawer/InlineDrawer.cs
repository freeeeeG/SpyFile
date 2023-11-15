using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CCB RID: 3275
	public abstract class InlineDrawer : MemberDrawer
	{
		// Token: 0x060068E0 RID: 26848 RVA: 0x0027954C File Offset: 0x0027774C
		public sealed override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			return MemberDrawType.Inline;
		}

		// Token: 0x060068E1 RID: 26849 RVA: 0x0027954F File Offset: 0x0027774F
		protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			this.DrawInline(context, member);
		}
	}
}
