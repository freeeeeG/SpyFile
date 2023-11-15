using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DC2 RID: 3522
	public class ShiftObjectToRayHit : CharacterOperation
	{
		// Token: 0x060046CA RID: 18122 RVA: 0x000CD764 File Offset: 0x000CB964
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

		// Token: 0x040035A2 RID: 13730
		[SerializeField]
		private Transform _origin;

		// Token: 0x040035A3 RID: 13731
		[SerializeField]
		private Transform _object;

		// Token: 0x040035A4 RID: 13732
		[SerializeField]
		private float _offsetX = -1f;

		// Token: 0x040035A5 RID: 13733
		[SerializeField]
		private float _rayDistance;
	}
}
