using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CD1 RID: 3281
	public sealed class HashedStringDrawer : InlineDrawer
	{
		// Token: 0x060068F4 RID: 26868 RVA: 0x0027963B File Offset: 0x0027783B
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is HashedString;
		}

		// Token: 0x060068F5 RID: 26869 RVA: 0x0027964C File Offset: 0x0027784C
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			HashedString hashedString = (HashedString)member.value;
			string str = hashedString.ToString();
			string str2 = "0x" + hashedString.HashValue.ToString("X");
			ImGuiEx.SimpleField(member.name, str + " (" + str2 + ")");
		}
	}
}
