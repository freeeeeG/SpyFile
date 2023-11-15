using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FB1 RID: 4017
	public sealed class SetParentToCharacterAttach : CharacterOperation
	{
		// Token: 0x06004DF3 RID: 19955 RVA: 0x000E92A0 File Offset: 0x000E74A0
		public override void Run(Character owner)
		{
			if (owner.attach == null)
			{
				return;
			}
			if (this._transform == null)
			{
				this._transform = base.transform;
			}
			if (this._flipByOwnerDirection)
			{
				this._transform.SetParent(owner.attachWithFlip.transform);
				return;
			}
			this._transform.SetParent(owner.attach.transform);
		}

		// Token: 0x04003DE5 RID: 15845
		[SerializeField]
		private Transform _transform;

		// Token: 0x04003DE6 RID: 15846
		[SerializeField]
		private bool _flipByOwnerDirection;
	}
}
