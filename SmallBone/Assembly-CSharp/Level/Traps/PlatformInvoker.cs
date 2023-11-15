using System;
using System.Collections.Generic;
using Characters;
using PhysicsUtils;
using Runnables;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000671 RID: 1649
	public class PlatformInvoker : MonoBehaviour
	{
		// Token: 0x06002104 RID: 8452 RVA: 0x000638BC File Offset: 0x00061ABC
		private void Start()
		{
			this._platformUpperBounds = this._platformCollider.bounds;
			this._platformUpperBounds.center = new Vector2(this._platformUpperBounds.center.x, this._platformUpperBounds.center.y + 0.1f);
			PlatformInvoker._lapper.contactFilter.SetLayerMask(this._targetLayer);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x0006392C File Offset: 0x00061B2C
		private void Update()
		{
			ReadonlyBoundedList<Collider2D> results = PlatformInvoker._lapper.OverlapArea(this._platformUpperBounds).results;
			if (results.Count <= 0)
			{
				if (this._isExecuted)
				{
					this._takeOffPlatform.Run();
					this._isExecuted = false;
				}
				return;
			}
			using (IEnumerator<Collider2D> enumerator = results.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Character character;
					if (enumerator.Current.TryFindCharacterComponent(out character) && this.SteppedOn(character) && !this._isExecuted)
					{
						this._takeOnPlatform.Run();
						this._isExecuted = true;
					}
				}
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000639D4 File Offset: 0x00061BD4
		private bool SteppedOn(Character character)
		{
			return !(character.movement.controller.collisionState.lastStandingCollider != this._platformCollider) && character.movement.isGrounded;
		}

		// Token: 0x04001C1A RID: 7194
		private static readonly NonAllocOverlapper _lapper = new NonAllocOverlapper(15);

		// Token: 0x04001C1B RID: 7195
		[SerializeField]
		private LayerMask _targetLayer;

		// Token: 0x04001C1C RID: 7196
		[SerializeField]
		private Collider2D _platformCollider;

		// Token: 0x04001C1D RID: 7197
		private Bounds _platformUpperBounds;

		// Token: 0x04001C1E RID: 7198
		[SerializeField]
		private Runnable _takeOnPlatform;

		// Token: 0x04001C1F RID: 7199
		[SerializeField]
		private Runnable _takeOffPlatform;

		// Token: 0x04001C20 RID: 7200
		private bool _isExecuted;
	}
}
