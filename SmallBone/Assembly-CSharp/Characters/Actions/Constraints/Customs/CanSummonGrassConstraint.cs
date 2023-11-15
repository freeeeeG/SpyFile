using System;
using System.Collections.Generic;
using Characters.Usables;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs
{
	// Token: 0x02000989 RID: 2441
	public class CanSummonGrassConstraint : Constraint
	{
		// Token: 0x06003451 RID: 13393 RVA: 0x0009ACEC File Offset: 0x00098EEC
		public override bool Pass()
		{
			this._sharedOverlapper.contactFilter.SetLayerMask(4096);
			List<Liquid> components = this._sharedOverlapper.OverlapCollider(this._findRange).GetComponents<Liquid>(true);
			Debug.Log(components.Count);
			return components.Count < 1;
		}

		// Token: 0x04002A53 RID: 10835
		[SerializeField]
		private BoxCollider2D _findRange;

		// Token: 0x04002A54 RID: 10836
		private readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(1);
	}
}
