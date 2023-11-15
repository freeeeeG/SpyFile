using System;

namespace flanne.Player
{
	// Token: 0x0200015D RID: 349
	public abstract class Buff
	{
		// Token: 0x060008E5 RID: 2277
		public abstract void OnAttach();

		// Token: 0x060008E6 RID: 2278
		public abstract void OnUnattach();

		// Token: 0x0400069E RID: 1694
		[NonSerialized]
		public PlayerBuffs owner;
	}
}
