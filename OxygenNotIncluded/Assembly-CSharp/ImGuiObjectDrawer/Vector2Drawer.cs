using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CD3 RID: 3283
	public sealed class Vector2Drawer : InlineDrawer
	{
		// Token: 0x060068FA RID: 26874 RVA: 0x00279732 File Offset: 0x00277932
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector2;
		}

		// Token: 0x060068FB RID: 26875 RVA: 0x00279744 File Offset: 0x00277944
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector2 vector = (Vector2)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1} )", vector.x, vector.y));
		}
	}
}
