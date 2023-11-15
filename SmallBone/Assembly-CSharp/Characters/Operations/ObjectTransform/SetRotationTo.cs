using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FBF RID: 4031
	public class SetRotationTo : CharacterOperation
	{
		// Token: 0x06004E1B RID: 19995 RVA: 0x000E9A70 File Offset: 0x000E7C70
		public override void Run(Character owner)
		{
			this._transform.rotation = Quaternion.Euler(0f, 0f, this._rotation.value);
		}

		// Token: 0x04003E1B RID: 15899
		[SerializeField]
		private Transform _transform;

		// Token: 0x04003E1C RID: 15900
		[SerializeField]
		private CustomFloat _rotation;
	}
}
