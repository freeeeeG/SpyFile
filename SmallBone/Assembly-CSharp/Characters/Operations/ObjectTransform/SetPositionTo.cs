using System;
using Characters.Operations.SetPosition;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FB2 RID: 4018
	public class SetPositionTo : CharacterOperation
	{
		// Token: 0x06004DF5 RID: 19957 RVA: 0x000E930C File Offset: 0x000E750C
		public override void Run(Character owner)
		{
			if (this._type == SetPositionTo.Type.Owner)
			{
				this._object = owner.transform;
			}
			Vector2 position = this._targetInfo.GetPosition(owner);
			this._object.position = position;
		}

		// Token: 0x04003DE7 RID: 15847
		[SerializeField]
		private SetPositionTo.Type _type;

		// Token: 0x04003DE8 RID: 15848
		[SerializeField]
		private Transform _object;

		// Token: 0x04003DE9 RID: 15849
		[SerializeField]
		private TargetInfo _targetInfo;

		// Token: 0x02000FB3 RID: 4019
		private enum Type
		{
			// Token: 0x04003DEB RID: 15851
			Object,
			// Token: 0x04003DEC RID: 15852
			Owner
		}
	}
}
