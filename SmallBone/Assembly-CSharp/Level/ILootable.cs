using System;

namespace Level
{
	// Token: 0x020004F6 RID: 1270
	public interface ILootable
	{
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060018DC RID: 6364
		// (remove) Token: 0x060018DD RID: 6365
		event Action onLoot;

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060018DE RID: 6366
		bool looted { get; }

		// Token: 0x060018DF RID: 6367
		void Activate();
	}
}
