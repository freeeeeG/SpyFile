using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects
{
	// Token: 0x020008E4 RID: 2276
	public sealed class AbyssShadowPool : MonoBehaviour
	{
		// Token: 0x060030A6 RID: 12454 RVA: 0x00091BBC File Offset: 0x0008FDBC
		private void Awake()
		{
			this.Load();
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x00091BC4 File Offset: 0x0008FDC4
		private void Load()
		{
			this._pool = new Queue<AbyssShadow>(this._maxCount);
			this._actives = new HashSet<AbyssShadow>();
			for (int i = 0; i < this._maxCount; i++)
			{
				AbyssShadow abyssShadow = UnityEngine.Object.Instantiate<AbyssShadow>(this._abyssShadowPrefab);
				abyssShadow.Initialize(this);
				abyssShadow.gameObject.SetActive(false);
				this._quintessence.onDiscard += delegate(Gear _)
				{
					UnityEngine.Object.Destroy(abyssShadow.gameObject);
				};
				this._pool.Enqueue(abyssShadow);
			}
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x00091C5A File Offset: 0x0008FE5A
		public void Push(AbyssShadow abyssShadow)
		{
			this._actives.Remove(abyssShadow);
			if (this._pool.Contains(abyssShadow))
			{
				return;
			}
			abyssShadow.transform.SetParent(base.transform);
			this._pool.Enqueue(abyssShadow);
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x00091C98 File Offset: 0x0008FE98
		public void PopAndSpawn(Transform point)
		{
			AbyssShadow abyssShadow = this._pool.Dequeue();
			abyssShadow.Spawn(point.position);
			this._actives.Add(abyssShadow);
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x00091CD0 File Offset: 0x0008FED0
		public void Bomb()
		{
			foreach (AbyssShadow abyssShadow in this._actives)
			{
				abyssShadow.Bomb();
			}
		}

		// Token: 0x04002826 RID: 10278
		[SerializeField]
		private Quintessence _quintessence;

		// Token: 0x04002827 RID: 10279
		[SerializeField]
		private int _maxCount = 30;

		// Token: 0x04002828 RID: 10280
		[SerializeField]
		private AbyssShadow _abyssShadowPrefab;

		// Token: 0x04002829 RID: 10281
		private Queue<AbyssShadow> _pool;

		// Token: 0x0400282A RID: 10282
		private HashSet<AbyssShadow> _actives;
	}
}
