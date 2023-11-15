using System;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E0C RID: 3596
	public class TakeAimTargetPlatform : CharacterOperation
	{
		// Token: 0x060047D7 RID: 18391 RVA: 0x000D120C File Offset: 0x000CF40C
		private void Awake()
		{
			this._originalDirection = 0f;
			this._originalScale = Vector3.one;
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x000D1224 File Offset: 0x000CF424
		public override void Run(Character owner)
		{
			RaycastHit2D raycastHit2D;
			if (!this._controller.target.movement.TryBelowRayCast(this._layerMask, out raycastHit2D, this._distance))
			{
				return;
			}
			Vector2 point = raycastHit2D.point;
			point.y += this._controller.target.collider.size.y;
			raycastHit2D.point = point;
			Vector2 vector = raycastHit2D.point - this._weaponAxisPosition.transform.position;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this._weaponAxisPosition.rotation = Quaternion.Euler(0f, 0f, this._originalDirection + num);
		}

		// Token: 0x040036F8 RID: 14072
		[SerializeField]
		private Transform _centerAxisPosition;

		// Token: 0x040036F9 RID: 14073
		[SerializeField]
		private Transform _weaponAxisPosition;

		// Token: 0x040036FA RID: 14074
		[SerializeField]
		private AIController _controller;

		// Token: 0x040036FB RID: 14075
		[SerializeField]
		private LayerMask _layerMask = 18;

		// Token: 0x040036FC RID: 14076
		[SerializeField]
		private float _distance = 10f;

		// Token: 0x040036FD RID: 14077
		private Vector3 _originalScale;

		// Token: 0x040036FE RID: 14078
		private float _originalDirection;
	}
}
