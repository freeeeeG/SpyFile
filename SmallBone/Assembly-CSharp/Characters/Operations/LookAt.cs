using System;
using Characters.Operations.LookAtTargets;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DED RID: 3565
	public class LookAt : CharacterOperation
	{
		// Token: 0x06004768 RID: 18280 RVA: 0x000CF809 File Offset: 0x000CDA09
		public override void Run(Character owner)
		{
			owner.ForceToLookAt(this._target.GetDirectionFrom(owner));
		}

		// Token: 0x04003673 RID: 13939
		[SerializeField]
		[Target.SubcomponentAttribute]
		private Target _target;
	}
}
