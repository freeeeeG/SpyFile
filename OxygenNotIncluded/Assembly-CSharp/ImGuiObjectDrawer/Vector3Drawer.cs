using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CD4 RID: 3284
	public sealed class Vector3Drawer : InlineDrawer
	{
		// Token: 0x060068FD RID: 26877 RVA: 0x00279790 File Offset: 0x00277990
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector3;
		}

		// Token: 0x060068FE RID: 26878 RVA: 0x002797A0 File Offset: 0x002779A0
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector3 vector = (Vector3)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1}, {2} )", vector.x, vector.y, vector.z));
		}
	}
}
