using System;
using Characters.Abilities.Weapons.DavyJones;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs
{
	// Token: 0x0200098B RID: 2443
	public sealed class DavyJonesEmptyMagazineConstraint : Constraint
	{
		// Token: 0x06003455 RID: 13397 RVA: 0x0009AD69 File Offset: 0x00098F69
		public override bool Pass()
		{
			return this._component.IsEmpty();
		}

		// Token: 0x04002A56 RID: 10838
		[SerializeField]
		private DavyJonesPassiveComponent _component;
	}
}
