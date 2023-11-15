using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FC0 RID: 4032
	public class SetRotationToTarget : CharacterOperation
	{
		// Token: 0x06004E1D RID: 19997 RVA: 0x000E9A98 File Offset: 0x000E7C98
		public override void Run(Character owner)
		{
			if (this._target == null || this._rotateTransform == null)
			{
				return;
			}
			Vector3 vector = this._target.position - this._rotateTransform.position;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			if (this._flipXByRotateTransformDirection)
			{
				num += (float)((this._rotateTransform.lossyScale.x < 0f) ? -180 : 0);
			}
			if (this._flipYByRotateTransformDirection)
			{
				num *= (float)((this._rotateTransform.lossyScale.x < 0f) ? -1 : 1);
			}
			this._rotateTransform.localRotation = Quaternion.Euler(0f, 0f, num + this._customOffsetAngle.value);
		}

		// Token: 0x04003E1D RID: 15901
		[SerializeField]
		private Transform _rotateTransform;

		// Token: 0x04003E1E RID: 15902
		[SerializeField]
		private Transform _target;

		// Token: 0x04003E1F RID: 15903
		[SerializeField]
		private CustomFloat _customOffsetAngle;

		// Token: 0x04003E20 RID: 15904
		[SerializeField]
		private bool _flipXByRotateTransformDirection = true;

		// Token: 0x04003E21 RID: 15905
		[SerializeField]
		private bool _flipYByRotateTransformDirection;
	}
}
