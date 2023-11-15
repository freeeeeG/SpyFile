using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CD2 RID: 3282
	public sealed class KAnimHashedStringDrawer : InlineDrawer
	{
		// Token: 0x060068F7 RID: 26871 RVA: 0x002796B6 File Offset: 0x002778B6
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is KAnimHashedString;
		}

		// Token: 0x060068F8 RID: 26872 RVA: 0x002796C8 File Offset: 0x002778C8
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			KAnimHashedString kanimHashedString = (KAnimHashedString)member.value;
			string str = kanimHashedString.ToString();
			string str2 = "0x" + kanimHashedString.HashValue.ToString("X");
			ImGuiEx.SimpleField(member.name, str + " (" + str2 + ")");
		}
	}
}
