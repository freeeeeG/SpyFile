using System;
using System.Collections;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CD9 RID: 3289
	public sealed class IEnumerableDrawer : CollectionDrawer
	{
		// Token: 0x06006913 RID: 26899 RVA: 0x00279AD0 File Offset: 0x00277CD0
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<IEnumerable>();
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x00279AD8 File Offset: 0x00277CD8
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return !((IEnumerable)member.value).GetEnumerator().MoveNext();
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x00279AF4 File Offset: 0x00277CF4
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			IEnumerable enumerable = (IEnumerable)member.value;
			int num = 0;
			using (IEnumerator enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object el = enumerator.Current;
					visit(context, new CollectionDrawer.Element(num, delegate()
					{
						DrawerUtil.Tooltip(el.GetType());
					}, () => new
					{
						value = el
					}));
					num++;
				}
			}
		}
	}
}
