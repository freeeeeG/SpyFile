using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CD7 RID: 3287
	public sealed class ArrayDrawer : CollectionDrawer
	{
		// Token: 0x0600690B RID: 26891 RVA: 0x00279940 File Offset: 0x00277B40
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsArray;
		}

		// Token: 0x0600690C RID: 26892 RVA: 0x0027994D File Offset: 0x00277B4D
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return ((Array)member.value).Length == 0;
		}

		// Token: 0x0600690D RID: 26893 RVA: 0x00279964 File Offset: 0x00277B64
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			ArrayDrawer.<>c__DisplayClass2_0 CS$<>8__locals1 = new ArrayDrawer.<>c__DisplayClass2_0();
			CS$<>8__locals1.array = (Array)member.value;
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.array.Length; i = i2)
			{
				int j = i;
				System.Action draw_tooltip;
				if ((draw_tooltip = CS$<>8__locals1.<>9__0) == null)
				{
					draw_tooltip = (CS$<>8__locals1.<>9__0 = delegate()
					{
						DrawerUtil.Tooltip(CS$<>8__locals1.array.GetType().GetElementType());
					});
				}
				visit(context, new CollectionDrawer.Element(j, draw_tooltip, () => new
				{
					value = CS$<>8__locals1.array.GetValue(i)
				}));
				i2 = i + 1;
			}
		}
	}
}
