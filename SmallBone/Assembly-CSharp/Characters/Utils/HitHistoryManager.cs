using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Utils
{
	// Token: 0x0200074A RID: 1866
	public class HitHistoryManager
	{
		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060025F8 RID: 9720 RVA: 0x000728B7 File Offset: 0x00070AB7
		public int Count
		{
			get
			{
				return this._hitHistory.Count;
			}
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000728C4 File Offset: 0x00070AC4
		public HitHistoryManager(int capacity)
		{
			this._hitHistory = new Dictionary<Target, HitHistoryManager.HitHistory>(capacity);
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000728D8 File Offset: 0x00070AD8
		public void AddOrUpdate(Target target)
		{
			HitHistoryManager.HitHistory value;
			if (this._hitHistory.TryGetValue(target, out value))
			{
				value.time = Time.time;
				value.count += 1f;
				this._hitHistory[target] = value;
				return;
			}
			value.time = Time.time;
			value.count = 1f;
			this._hitHistory.Add(target, value);
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x00002191 File Offset: 0x00000391
		public void ClearIfExpired()
		{
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x00072944 File Offset: 0x00070B44
		public void Clear()
		{
			this._hitHistory.Clear();
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x00072954 File Offset: 0x00070B54
		public bool CanAttack(Target target, int maxHit, int maxHitsPerUnit, float interval)
		{
			HitHistoryManager.HitHistory hitHistory;
			return this._hitHistory.Count < maxHit && (!this._hitHistory.TryGetValue(target, out hitHistory) || (Time.time - hitHistory.time > interval && hitHistory.count < (float)maxHitsPerUnit));
		}

		// Token: 0x040020B5 RID: 8373
		private readonly Dictionary<Target, HitHistoryManager.HitHistory> _hitHistory;

		// Token: 0x0200074B RID: 1867
		private struct HitHistory
		{
			// Token: 0x040020B6 RID: 8374
			public float time;

			// Token: 0x040020B7 RID: 8375
			public float count;
		}
	}
}
