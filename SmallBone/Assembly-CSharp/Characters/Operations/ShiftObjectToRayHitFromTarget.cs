using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DD2 RID: 3538
	public class ShiftObjectToRayHitFromTarget : CharacterOperation
	{
		// Token: 0x06004709 RID: 18185 RVA: 0x000CE2E8 File Offset: 0x000CC4E8
		public override void Run(Character owner)
		{
			Vector2 vector = (owner.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left;
			RaycastHit2D hit = Physics2D.Raycast(this._origin.position, vector, this._rayDistance, Layers.groundMask);
			if (hit)
			{
				this._object.position = new Vector2(hit.point.x + this._offsetX * vector.x, hit.point.y);
				return;
			}
			float num = vector.x * this._rayDistance;
			float num2 = vector.y * this._rayDistance;
			this._object.position = new Vector2(this._origin.position.x + num + this._offsetX * vector.x, this._origin.position.y + num2);
		}

		// Token: 0x040035E8 RID: 13800
		[SerializeField]
		private Transform _origin;

		// Token: 0x040035E9 RID: 13801
		[SerializeField]
		private Transform _object;

		// Token: 0x040035EA RID: 13802
		[SerializeField]
		private float _offsetX = -1f;

		// Token: 0x040035EB RID: 13803
		[SerializeField]
		private float _rayDistance;
	}
}
