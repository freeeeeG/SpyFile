using System;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E44 RID: 3652
	public class TakeAimTowardsTheFront : CharacterOperation
	{
		// Token: 0x060048A6 RID: 18598 RVA: 0x000D3975 File Offset: 0x000D1B75
		private void Awake()
		{
			this._originalDirection = 0f;
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x000D3984 File Offset: 0x000D1B84
		public override void Run(Character owner)
		{
			Character target = this._controller.target;
			float y;
			if (this._platformTarget)
			{
				y = target.movement.controller.collisionState.lastStandingCollider.bounds.max.y;
			}
			else
			{
				y = target.transform.position.y + target.collider.bounds.extents.y;
			}
			Vector3 vector = new Vector3(target.transform.position.x, y) - this._centerAxisPosition.transform.position;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			if (owner.lookingDirection == Character.LookingDirection.Left)
			{
				num = 180f - num;
			}
			this._centerAxisPosition.rotation = Quaternion.Euler(0f, 0f, this._originalDirection + num);
		}

		// Token: 0x040037BD RID: 14269
		[SerializeField]
		private Transform _centerAxisPosition;

		// Token: 0x040037BE RID: 14270
		[SerializeField]
		private AIController _controller;

		// Token: 0x040037BF RID: 14271
		[SerializeField]
		private bool _platformTarget;

		// Token: 0x040037C0 RID: 14272
		private float _originalDirection;
	}
}
