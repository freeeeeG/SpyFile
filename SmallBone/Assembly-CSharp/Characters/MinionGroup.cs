using System;
using System.Collections;
using System.Collections.Generic;

namespace Characters
{
	// Token: 0x0200070A RID: 1802
	public sealed class MinionGroup : IEnumerable<Minion>, IEnumerable
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002480 RID: 9344 RVA: 0x0006DE6F File Offset: 0x0006C06F
		public int Count
		{
			get
			{
				return this._group.Count;
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x0006DE7C File Offset: 0x0006C07C
		public MinionGroup()
		{
			this._group = new LinkedList<Minion>();
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0006DE8F File Offset: 0x0006C08F
		public void Join(Minion minion)
		{
			this._group.AddLast(minion);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x0006DE9E File Offset: 0x0006C09E
		public void Leave(Minion minion)
		{
			this._group.Remove(minion);
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x0006DEAD File Offset: 0x0006C0AD
		public void DespawnOldest()
		{
			if (this._group.Count == 0)
			{
				return;
			}
			this._group.First.Value.Despawn();
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x0006DED4 File Offset: 0x0006C0D4
		public void DespawnAll()
		{
			for (int i = this._group.Count - 1; i >= 0; i--)
			{
				this._group.First.Value.Despawn();
			}
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x0006DF0E File Offset: 0x0006C10E
		public IEnumerator<Minion> GetEnumerator()
		{
			return this._group.GetEnumerator();
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x0006DF20 File Offset: 0x0006C120
		IEnumerator IEnumerable.GetEnumerator()
		{
			yield return this._group.GetEnumerator();
			yield break;
		}

		// Token: 0x04001F0A RID: 7946
		private LinkedList<Minion> _group;
	}
}
