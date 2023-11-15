using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FBA RID: 4026
	public class MoveTransform : CharacterOperation
	{
		// Token: 0x06004E0F RID: 19983 RVA: 0x000E9764 File Offset: 0x000E7964
		public override void Run(Character owner)
		{
			this._targetTransform.position = this._targetTransform.position + new Vector3((owner.lookingDirection == Character.LookingDirection.Right) ? this._xValue : (-this._xValue), this._yValue, 0f);
		}

		// Token: 0x04003E0A RID: 15882
		[SerializeField]
		[Range(-100f, 100f)]
		private float _xValue;

		// Token: 0x04003E0B RID: 15883
		[Range(-100f, 100f)]
		[SerializeField]
		private float _yValue;

		// Token: 0x04003E0C RID: 15884
		[SerializeField]
		private Transform _targetTransform;
	}
}
