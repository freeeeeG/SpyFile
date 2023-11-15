using System;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FD3 RID: 4051
	public class ChangeTransformParent : CharacterOperation
	{
		// Token: 0x06004E60 RID: 20064 RVA: 0x000EAC4C File Offset: 0x000E8E4C
		public override void Run(Character owner)
		{
			this._target.parent = this._newParent;
			if (this._resetPosition)
			{
				this._target.localPosition = Vector3.zero;
			}
		}

		// Token: 0x04003E6E RID: 15982
		[SerializeField]
		private Transform _target;

		// Token: 0x04003E6F RID: 15983
		[SerializeField]
		private Transform _newParent;

		// Token: 0x04003E70 RID: 15984
		[SerializeField]
		private bool _resetPosition;
	}
}
