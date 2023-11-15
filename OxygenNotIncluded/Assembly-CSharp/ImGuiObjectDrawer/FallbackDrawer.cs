using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CCE RID: 3278
	public sealed class FallbackDrawer : SimpleDrawer
	{
		// Token: 0x060068EB RID: 26859 RVA: 0x002795C3 File Offset: 0x002777C3
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return true;
		}

		// Token: 0x060068EC RID: 26860 RVA: 0x002795C6 File Offset: 0x002777C6
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}
	}
}
