using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FBD RID: 4029
	public class RotateAngle : CharacterOperation
	{
		// Token: 0x06004E15 RID: 19989 RVA: 0x000E97D8 File Offset: 0x000E79D8
		public override void Run(Character owner)
		{
			Vector3 eulerAngles = this._centerAxisPosition.rotation.eulerAngles;
			this._centerAxisPosition.rotation = Quaternion.Euler(eulerAngles + new Vector3(0f, 0f, (!this._isAdded) ? this._angle : (-this._angle)));
		}

		// Token: 0x04003E10 RID: 15888
		[SerializeField]
		private Transform _centerAxisPosition;

		// Token: 0x04003E11 RID: 15889
		[SerializeField]
		[Range(0f, 90f)]
		private float _angle = 5f;

		// Token: 0x04003E12 RID: 15890
		[SerializeField]
		private bool _isAdded;
	}
}
