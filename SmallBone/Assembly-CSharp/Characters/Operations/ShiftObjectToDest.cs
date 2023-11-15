using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E05 RID: 3589
	public class ShiftObjectToDest : CharacterOperation
	{
		// Token: 0x060047BC RID: 18364 RVA: 0x000D0B74 File Offset: 0x000CED74
		public override void Run(Character owner)
		{
			float x = this._destination.transform.position.x + this._offsetX;
			float num;
			if (this._fromPlatform)
			{
				num = owner.movement.controller.collisionState.lastStandingCollider.bounds.max.y;
			}
			else
			{
				num = this._destination.transform.position.y;
			}
			num += this._offsetY;
			this._object.position = new Vector2(x, num);
		}

		// Token: 0x040036D3 RID: 14035
		[SerializeField]
		private Transform _destination;

		// Token: 0x040036D4 RID: 14036
		[SerializeField]
		private Transform _object;

		// Token: 0x040036D5 RID: 14037
		[SerializeField]
		private float _offsetY;

		// Token: 0x040036D6 RID: 14038
		[SerializeField]
		private float _offsetX;

		// Token: 0x040036D7 RID: 14039
		[SerializeField]
		private bool _fromPlatform;
	}
}
