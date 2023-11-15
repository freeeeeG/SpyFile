using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FBB RID: 4027
	public class MoveTransformFromPosition : CharacterOperation
	{
		// Token: 0x06004E11 RID: 19985 RVA: 0x000E97B3 File Offset: 0x000E79B3
		public override void Run(Character owner)
		{
			this._targetTransform.position = this._fromPosition.position;
		}

		// Token: 0x04003E0D RID: 15885
		[SerializeField]
		private Transform _fromPosition;

		// Token: 0x04003E0E RID: 15886
		[SerializeField]
		private Transform _targetTransform;
	}
}
