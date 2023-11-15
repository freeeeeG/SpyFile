using System;
using ImGuiNET;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CD6 RID: 3286
	public abstract class CollectionDrawer : MemberDrawer
	{
		// Token: 0x06006903 RID: 26883
		public abstract bool IsEmpty(in MemberDrawContext context, in MemberDetails member);

		// Token: 0x06006904 RID: 26884 RVA: 0x0027987C File Offset: 0x00277A7C
		public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			if (this.IsEmpty(context, member))
			{
				return MemberDrawType.Inline;
			}
			return MemberDrawType.Custom;
		}

		// Token: 0x06006905 RID: 26885 RVA: 0x0027988B File Offset: 0x00277A8B
		protected sealed override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Debug.Assert(this.IsEmpty(context, member));
			this.DrawEmpty(context, member);
		}

		// Token: 0x06006906 RID: 26886 RVA: 0x002798A2 File Offset: 0x00277AA2
		protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			Debug.Assert(!this.IsEmpty(context, member));
			this.DrawWithContents(context, member, depth);
		}

		// Token: 0x06006907 RID: 26887 RVA: 0x002798BD File Offset: 0x00277ABD
		private void DrawEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			ImGui.Text(member.name + "(empty)");
		}

		// Token: 0x06006908 RID: 26888 RVA: 0x002798D4 File Offset: 0x00277AD4
		private void DrawWithContents(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			CollectionDrawer.<>c__DisplayClass5_0 CS$<>8__locals1 = new CollectionDrawer.<>c__DisplayClass5_0();
			CS$<>8__locals1.depth = depth;
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && CS$<>8__locals1.depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				this.VisitElements(new CollectionDrawer.ElementVisitor(CS$<>8__locals1.<DrawWithContents>g__Visitor|0), context, member);
				ImGui.TreePop();
			}
		}

		// Token: 0x06006909 RID: 26889
		protected abstract void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member);

		// Token: 0x02001C0F RID: 7183
		// (Invoke) Token: 0x06009B8D RID: 39821
		protected delegate void ElementVisitor(in MemberDrawContext context, CollectionDrawer.Element element);

		// Token: 0x02001C10 RID: 7184
		protected struct Element
		{
			// Token: 0x06009B90 RID: 39824 RVA: 0x00349C46 File Offset: 0x00347E46
			public Element(string node_name, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			{
				this.node_name = node_name;
				this.draw_tooltip = draw_tooltip;
				this.get_object_to_inspect = get_object_to_inspect;
			}

			// Token: 0x06009B91 RID: 39825 RVA: 0x00349C5D File Offset: 0x00347E5D
			public Element(int index, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			{
				this = new CollectionDrawer.Element(string.Format("[{0}]", index), draw_tooltip, get_object_to_inspect);
			}

			// Token: 0x04007EC0 RID: 32448
			public readonly string node_name;

			// Token: 0x04007EC1 RID: 32449
			public readonly System.Action draw_tooltip;

			// Token: 0x04007EC2 RID: 32450
			public readonly Func<object> get_object_to_inspect;
		}
	}
}
