using System;
using System.Collections;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004E8 RID: 1256
	public sealed class GraveDiggerGrave : MonoBehaviour
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001898 RID: 6296 RVA: 0x0004CE0C File Offset: 0x0004B00C
		// (set) Token: 0x06001899 RID: 6297 RVA: 0x0004CE14 File Offset: 0x0004B014
		public bool activated { get; private set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x0600189A RID: 6298 RVA: 0x0004CE1D File Offset: 0x0004B01D
		public Vector3 position
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0004CE2C File Offset: 0x0004B02C
		public void Spawn(Vector3 position, GraveDiggerGraveContainer container)
		{
			RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 10f, Layers.groundMask);
			if (!hit)
			{
				return;
			}
			if (Physics2D.OverlapPoint(position, Layers.groundMask) != null)
			{
				return;
			}
			GraveDiggerGrave component = this._poolObject.Spawn(hit.point, true).GetComponent<GraveDiggerGrave>();
			component.Initialize(container);
			component.StartCoroutine(component.CLifeSpan());
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0004CEB0 File Offset: 0x0004B0B0
		public void Despawn()
		{
			this._poolObject.Despawn();
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0004CEBD File Offset: 0x0004B0BD
		public void Activate()
		{
			if (this.activated)
			{
				return;
			}
			this.activated = true;
			this._animator.SetBool(GraveDiggerGrave._activatedParameterHash, true);
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x0004CEE0 File Offset: 0x0004B0E0
		public void Deactivate()
		{
			if (!this.activated)
			{
				return;
			}
			this.activated = false;
			this._animator.SetBool(GraveDiggerGrave._activatedParameterHash, false);
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x0004CF03 File Offset: 0x0004B103
		private void Initialize(GraveDiggerGraveContainer container)
		{
			this._elapsed = 0f;
			this._container = container;
			this.Deactivate();
			this._container.Add(this);
			Renderer spriteRenderer = this._spriteRenderer;
			short sortingOrder = GraveDiggerGrave._sortingOrder;
			GraveDiggerGrave._sortingOrder = sortingOrder + 1;
			spriteRenderer.sortingOrder = (int)sortingOrder;
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0004CF42 File Offset: 0x0004B142
		private IEnumerator CLifeSpan()
		{
			while (this._elapsed < this._lifeTime)
			{
				this._elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			this._container.Remove(this);
			this.Despawn();
			yield break;
		}

		// Token: 0x04001563 RID: 5475
		private static short _sortingOrder;

		// Token: 0x04001564 RID: 5476
		private static readonly int _activatedParameterHash = Animator.StringToHash("Activated");

		// Token: 0x04001565 RID: 5477
		[GetComponent]
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x04001566 RID: 5478
		[SerializeField]
		private Animator _animator;

		// Token: 0x04001567 RID: 5479
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04001568 RID: 5480
		[SerializeField]
		[Space]
		private float _lifeTime;

		// Token: 0x04001569 RID: 5481
		private float _elapsed;

		// Token: 0x0400156A RID: 5482
		private GraveDiggerGraveContainer _container;
	}
}
