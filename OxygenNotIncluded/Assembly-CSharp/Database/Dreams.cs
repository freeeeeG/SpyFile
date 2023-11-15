using System;

namespace Database
{
	// Token: 0x02000CFC RID: 3324
	public class Dreams : ResourceSet<Dream>
	{
		// Token: 0x0600698D RID: 27021 RVA: 0x0028B1A1 File Offset: 0x002893A1
		public Dreams(ResourceSet parent) : base("Dreams", parent)
		{
			this.CommonDream = new Dream("CommonDream", this, "dream_tear_swirly_kanim", new string[]
			{
				"dreamIcon_journal"
			});
		}

		// Token: 0x04004B18 RID: 19224
		public Dream CommonDream;
	}
}
