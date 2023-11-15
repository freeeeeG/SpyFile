using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FC1 RID: 4033
	public class SetScaleToDistance : CharacterOperation
	{
		// Token: 0x06004E1F RID: 19999 RVA: 0x000E9B80 File Offset: 0x000E7D80
		public override void Run(Character owner)
		{
			float num = Vector2.Distance(this._object.position, this._target.position);
			if (this._transformToApply == null)
			{
				this._transformToApply = this._object;
			}
			if (this._changeXScale)
			{
				this._transformToApply.localScale = new Vector3(num * this._multiplier, this._object.localScale.y);
				return;
			}
			this._transformToApply.localScale = new Vector3(this._object.localScale.x, num * this._multiplier);
		}

		// Token: 0x04003E22 RID: 15906
		[SerializeField]
		private Transform _object;

		// Token: 0x04003E23 RID: 15907
		[SerializeField]
		private Transform _target;

		// Token: 0x04003E24 RID: 15908
		[SerializeField]
		private Transform _transformToApply;

		// Token: 0x04003E25 RID: 15909
		[SerializeField]
		private float _multiplier = 1f;

		// Token: 0x04003E26 RID: 15910
		[SerializeField]
		private bool _changeXScale = true;
	}
}
