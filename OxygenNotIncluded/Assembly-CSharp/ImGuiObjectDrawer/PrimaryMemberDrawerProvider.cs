using System;
using System.Collections.Generic;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000CCA RID: 3274
	public class PrimaryMemberDrawerProvider : IMemberDrawerProvider
	{
		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060068DD RID: 26845 RVA: 0x002794A6 File Offset: 0x002776A6
		public int Priority
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x060068DE RID: 26846 RVA: 0x002794AC File Offset: 0x002776AC
		public void AppendDrawersTo(List<MemberDrawer> drawers)
		{
			drawers.AddRange(new MemberDrawer[]
			{
				new NullDrawer(),
				new SimpleDrawer(),
				new LocStringDrawer(),
				new EnumDrawer(),
				new HashedStringDrawer(),
				new KAnimHashedStringDrawer(),
				new Vector2Drawer(),
				new Vector3Drawer(),
				new Vector4Drawer(),
				new UnityObjectDrawer(),
				new ArrayDrawer(),
				new IDictionaryDrawer(),
				new IEnumerableDrawer(),
				new PlainCSharpObjectDrawer(),
				new FallbackDrawer()
			});
		}
	}
}
