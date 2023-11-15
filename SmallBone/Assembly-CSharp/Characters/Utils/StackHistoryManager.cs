using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Utils
{
	// Token: 0x0200074D RID: 1869
	public class StackHistoryManager<T>
	{
		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002603 RID: 9731 RVA: 0x00072BBA File Offset: 0x00070DBA
		public int Count
		{
			get
			{
				return this._history.Count;
			}
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x00072BC7 File Offset: 0x00070DC7
		public StackHistoryManager(int capacity)
		{
			this._history = new Dictionary<T, StackHistoryManager<T>.StackHistory>(capacity);
			this._expiredTargets = new List<T>(capacity);
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x00072BE8 File Offset: 0x00070DE8
		public bool IsElapsedFromLastTime(T target, float time, bool defaultResult = false)
		{
			StackHistoryManager<T>.StackHistory stackHistory;
			if (this._history.TryGetValue(target, out stackHistory))
			{
				return Time.time - stackHistory.lastTime >= time;
			}
			return defaultResult;
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x00072C1C File Offset: 0x00070E1C
		public bool TryAddStack(T target, int increasement, int maxStack, float lifeTime)
		{
			StackHistoryManager<T>.StackHistory stackHistory;
			if (this._history.TryGetValue(target, out stackHistory))
			{
				stackHistory.lastTime = Time.time;
				stackHistory.lifeTime = lifeTime;
				if (this.IsExpired(target))
				{
					stackHistory.startTime = Time.time;
					stackHistory.stack = increasement;
					this._history[target] = stackHistory;
					return true;
				}
				if (stackHistory.stack + increasement > maxStack)
				{
					stackHistory.stack = maxStack;
					this._history[target] = stackHistory;
					return false;
				}
				stackHistory.stack++;
				this._history[target] = stackHistory;
			}
			else
			{
				stackHistory.lastTime = Time.time;
				stackHistory.startTime = Time.time;
				stackHistory.lifeTime = lifeTime;
				stackHistory.stack = increasement;
				this._history.Add(target, stackHistory);
			}
			return true;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x00072CF0 File Offset: 0x00070EF0
		public bool IsExpired(T target)
		{
			StackHistoryManager<T>.StackHistory stackHistory;
			return this._history.TryGetValue(target, out stackHistory) && Time.time - stackHistory.startTime > stackHistory.lifeTime;
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x00072D26 File Offset: 0x00070F26
		public bool IsFull(int maxStack)
		{
			return this._history.Count < maxStack;
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x00072D39 File Offset: 0x00070F39
		public bool Has(T target)
		{
			return this._history.ContainsKey(target);
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x00072D47 File Offset: 0x00070F47
		public void Clear()
		{
			this._history.Clear();
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x00072D54 File Offset: 0x00070F54
		public void ClearIfExpired()
		{
			this._expiredTargets.Clear();
			foreach (T t in this._history.Keys)
			{
				if (this.IsExpired(t))
				{
					this._expiredTargets.Add(t);
				}
			}
			foreach (T key in this._expiredTargets)
			{
				this._history.Remove(key);
			}
		}

		// Token: 0x040020B8 RID: 8376
		private readonly Dictionary<T, StackHistoryManager<T>.StackHistory> _history;

		// Token: 0x040020B9 RID: 8377
		private readonly List<T> _expiredTargets;

		// Token: 0x0200074E RID: 1870
		private struct StackHistory
		{
			// Token: 0x040020BA RID: 8378
			public float startTime;

			// Token: 0x040020BB RID: 8379
			public float lastTime;

			// Token: 0x040020BC RID: 8380
			public int stack;

			// Token: 0x040020BD RID: 8381
			public float lifeTime;
		}
	}
}
