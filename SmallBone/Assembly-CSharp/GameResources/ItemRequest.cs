using System;
using Characters.Gear.Items;

namespace GameResources
{
	// Token: 0x02000190 RID: 400
	public sealed class ItemRequest : Request<Item>
	{
		// Token: 0x060008B4 RID: 2228 RVA: 0x00018EB3 File Offset: 0x000170B3
		public ItemRequest(string path) : base(path)
		{
		}
	}
}
