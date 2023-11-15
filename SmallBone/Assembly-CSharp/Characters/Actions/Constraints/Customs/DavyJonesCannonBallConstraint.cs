using System;
using Characters.Abilities.Weapons.DavyJones;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs
{
	// Token: 0x0200098A RID: 2442
	public sealed class DavyJonesCannonBallConstraint : Constraint
	{
		// Token: 0x06003453 RID: 13395 RVA: 0x0009AD59 File Offset: 0x00098F59
		public override bool Pass()
		{
			return !this._component.IsEmpty();
		}

		// Token: 0x04002A55 RID: 10837
		[SerializeField]
		private DavyJonesPassiveComponent _component;
	}
}
