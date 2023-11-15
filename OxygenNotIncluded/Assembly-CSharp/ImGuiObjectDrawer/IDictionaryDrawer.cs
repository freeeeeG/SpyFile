using System;
using System.Collections;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CD8 RID: 3288
	public sealed class IDictionaryDrawer : CollectionDrawer
	{
		// Token: 0x0600690F RID: 26895 RVA: 0x00279A1A File Offset: 0x00277C1A
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<IDictionary>();
		}

		// Token: 0x06006910 RID: 26896 RVA: 0x00279A22 File Offset: 0x00277C22
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return ((IDictionary)member.value).Count == 0;
		}

		// Token: 0x06006911 RID: 26897 RVA: 0x00279A38 File Offset: 0x00277C38
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			IDictionary dictionary = (IDictionary)member.value;
			int num = 0;
			using (IDictionaryEnumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DictionaryEntry kvp = (DictionaryEntry)enumerator.Current;
					visit(context, new CollectionDrawer.Element(num, delegate()
					{
						DrawerUtil.Tooltip(string.Format("{0} -> {1}", kvp.Key.GetType(), kvp.Value.GetType()));
					}, () => new
					{
						key = kvp.Key,
						value = kvp.Value
					}));
					num++;
				}
			}
		}
	}
}
