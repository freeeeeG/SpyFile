using System;
using Level;
using UnityEngine;

namespace Characters.Movements
{
	// Token: 0x02000805 RID: 2053
	[RequireComponent(typeof(CharacterController2D))]
	public class DynamicPlatformAttacher : MonoBehaviour
	{
		// Token: 0x06002A0F RID: 10767 RVA: 0x000815B0 File Offset: 0x0007F7B0
		private void Awake()
		{
			this._controller = base.GetComponent<CharacterController2D>();
			this._controller.collisionState.belowCollisionDetector.OnEnter += this.OnBelowCollisionEnter;
			this._controller.collisionState.belowCollisionDetector.OnExit += this.OnBelowCollisionExit;
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x0008160C File Offset: 0x0007F80C
		private void OnDestroy()
		{
			this._controller.collisionState.belowCollisionDetector.OnEnter -= this.OnBelowCollisionEnter;
			this._controller.collisionState.belowCollisionDetector.OnExit -= this.OnBelowCollisionExit;
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x0008165C File Offset: 0x0007F85C
		private void OnBelowCollisionEnter(RaycastHit2D hit)
		{
			DynamicPlatform component = hit.collider.GetComponent<DynamicPlatform>();
			if (component == null)
			{
				return;
			}
			component.Attach(this._controller);
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x0008168C File Offset: 0x0007F88C
		private void OnBelowCollisionExit(RaycastHit2D hit)
		{
			if (hit.collider == null)
			{
				return;
			}
			DynamicPlatform component = hit.collider.GetComponent<DynamicPlatform>();
			if (component == null)
			{
				return;
			}
			component.Detach(this._controller);
		}

		// Token: 0x040023E5 RID: 9189
		private CharacterController2D _controller;
	}
}
