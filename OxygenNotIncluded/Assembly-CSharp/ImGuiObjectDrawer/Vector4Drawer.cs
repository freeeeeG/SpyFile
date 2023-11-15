using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CD5 RID: 3285
	public sealed class Vector4Drawer : InlineDrawer
	{
		// Token: 0x06006900 RID: 26880 RVA: 0x002797F7 File Offset: 0x002779F7
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector4;
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x00279808 File Offset: 0x00277A08
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector4 vector = (Vector4)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1}, {2}, {3} )", new object[]
			{
				vector.x,
				vector.y,
				vector.z,
				vector.w
			}));
		}
	}
}
