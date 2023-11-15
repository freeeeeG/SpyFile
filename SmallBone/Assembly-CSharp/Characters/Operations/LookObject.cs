using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DEE RID: 3566
	public class LookObject : CharacterOperation
	{
		// Token: 0x0600476A RID: 18282 RVA: 0x000CF81D File Offset: 0x000CDA1D
		public override void Run(Character owner)
		{
			owner.ForceToLookAt(this._target.transform.position.x);
		}

		// Token: 0x04003674 RID: 13940
		[SerializeField]
		private GameObject _target;
	}
}
