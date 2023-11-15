using System;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E43 RID: 3651
	public class TakeAim : CharacterOperation
	{
		// Token: 0x060048A4 RID: 18596 RVA: 0x000D3838 File Offset: 0x000D1A38
		public override void Run(Character owner)
		{
			Character target = this._controller.target;
			float y;
			if (this._platformTarget)
			{
				Collider2D collider2D;
				if (this._lastStandingCollider)
				{
					y = target.movement.controller.collisionState.lastStandingCollider.bounds.max.y;
				}
				else if (target.movement.TryGetClosestBelowCollider(out collider2D, this._groundMask, 100f))
				{
					y = collider2D.bounds.max.y;
				}
				else
				{
					y = target.transform.position.y;
				}
			}
			else
			{
				y = target.transform.position.y + target.collider.bounds.extents.y;
			}
			Vector3 vector = new Vector3(target.transform.position.x, y) - this._centerAxisPosition.transform.position;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this._centerAxisPosition.rotation = Quaternion.Euler(0f, 0f, z);
		}

		// Token: 0x040037B8 RID: 14264
		[SerializeField]
		private Transform _centerAxisPosition;

		// Token: 0x040037B9 RID: 14265
		[SerializeField]
		private AIController _controller;

		// Token: 0x040037BA RID: 14266
		[SerializeField]
		private bool _platformTarget;

		// Token: 0x040037BB RID: 14267
		[SerializeField]
		private bool _lastStandingCollider = true;

		// Token: 0x040037BC RID: 14268
		[SerializeField]
		private LayerMask _groundMask = Layers.groundMask;
	}
}
