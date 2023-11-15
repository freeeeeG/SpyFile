using System;
using Characters;
using UnityEngine;

namespace Level
{
	// Token: 0x020004AE RID: 1198
	public abstract class DestructibleObject : MonoBehaviour
	{
		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600170D RID: 5901
		public abstract Collider2D collider { get; }

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x00048785 File Offset: 0x00046985
		public bool blockCast
		{
			get
			{
				return this._blockCast;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x0004878D File Offset: 0x0004698D
		public bool spawnEffectOnHit
		{
			get
			{
				return this._spawnEffectOnHit;
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001710 RID: 5904 RVA: 0x00048795 File Offset: 0x00046995
		// (remove) Token: 0x06001711 RID: 5905 RVA: 0x000487AE File Offset: 0x000469AE
		public event Action onDestroy
		{
			add
			{
				this._onDestroy = (Action)Delegate.Combine(this._onDestroy, value);
			}
			remove
			{
				this._onDestroy = (Action)Delegate.Remove(this._onDestroy, value);
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x000487C7 File Offset: 0x000469C7
		// (set) Token: 0x06001713 RID: 5907 RVA: 0x000487CF File Offset: 0x000469CF
		public bool destroyed { get; protected set; }

		// Token: 0x06001714 RID: 5908 RVA: 0x000487D8 File Offset: 0x000469D8
		public void Hit(Character from, ref Damage damage)
		{
			this.Hit(from, ref damage, Vector2.zero);
		}

		// Token: 0x06001715 RID: 5909
		public abstract void Hit(Character from, ref Damage damage, Vector2 force);

		// Token: 0x04001430 RID: 5168
		protected Action _onDestroy;

		// Token: 0x04001431 RID: 5169
		[SerializeField]
		private bool _blockCast;

		// Token: 0x04001432 RID: 5170
		[SerializeField]
		private bool _spawnEffectOnHit = true;
	}
}
