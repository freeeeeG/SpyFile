using System;
using CutScenes.SpecialMap;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Gear.Quintessences.Effects
{
	// Token: 0x020008E3 RID: 2275
	[RequireComponent(typeof(FollowMovement))]
	public sealed class AbyssShadow : MonoBehaviour
	{
		// Token: 0x060030A0 RID: 12448 RVA: 0x00091B0D File Offset: 0x0008FD0D
		public void Initialize(AbyssShadowPool pool)
		{
			this._pool = pool;
			this._followMovement = base.GetComponent<FollowMovement>();
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.Despawn;
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x00091B3D File Offset: 0x0008FD3D
		public void Spawn(Vector2 position)
		{
			base.transform.SetParent(null);
			base.transform.position = position;
			base.gameObject.SetActive(true);
			this._followMovement.Run();
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x00091B73 File Offset: 0x0008FD73
		public void Despawn()
		{
			base.gameObject.SetActive(false);
			this._pool.Push(this);
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x00091B8D File Offset: 0x0008FD8D
		public void Bomb()
		{
			UnityEvent onBomb = this._onBomb;
			if (onBomb == null)
			{
				return;
			}
			onBomb.Invoke();
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x00091B9F File Offset: 0x0008FD9F
		private void OnDestroy()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.Despawn;
		}

		// Token: 0x04002823 RID: 10275
		[SerializeField]
		private UnityEvent _onBomb;

		// Token: 0x04002824 RID: 10276
		[GetComponent]
		[SerializeField]
		private FollowMovement _followMovement;

		// Token: 0x04002825 RID: 10277
		private AbyssShadowPool _pool;
	}
}
