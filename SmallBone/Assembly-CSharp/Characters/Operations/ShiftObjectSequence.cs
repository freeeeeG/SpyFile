using System;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E04 RID: 3588
	public class ShiftObjectSequence : CharacterOperation
	{
		// Token: 0x060047BA RID: 18362 RVA: 0x000D0AD4 File Offset: 0x000CECD4
		public override void Run(Character owner)
		{
			Character target = this._controller.target;
			if (target == null)
			{
				return;
			}
			Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			float x = this._origin.position.x + this._offsetX;
			float num = this._fromPlatform ? bounds.max.y : this._origin.position.y;
			num += this._offsetY;
			this._object.position = new Vector2(x, num);
		}

		// Token: 0x040036CA RID: 14026
		[SerializeField]
		private AIController _controller;

		// Token: 0x040036CB RID: 14027
		[SerializeField]
		private Transform _object;

		// Token: 0x040036CC RID: 14028
		[SerializeField]
		private Transform _origin;

		// Token: 0x040036CD RID: 14029
		[SerializeField]
		private int _index;

		// Token: 0x040036CE RID: 14030
		[SerializeField]
		private float _offsetY;

		// Token: 0x040036CF RID: 14031
		[SerializeField]
		private float _offsetX;

		// Token: 0x040036D0 RID: 14032
		[SerializeField]
		private float _deltaY;

		// Token: 0x040036D1 RID: 14033
		[SerializeField]
		private float _deltaX;

		// Token: 0x040036D2 RID: 14034
		[SerializeField]
		private bool _fromPlatform;
	}
}
